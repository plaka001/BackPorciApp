using Aplicacion.Animales.CerdaDeCria.Servicio.Registrar;
using Dominio.Abstractions;
using Dominio.Animales;
using Dominio.Animales.CerdasCria;
using Dominio.Animales.General;
using Dominio.Animales.Repository;
using Dominio.EspacioFisicos;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.EspacioFisicos.Repository;
using Dominio.EspaciosFisico;
using Moq;
using Unit.Test.Fakes;
using Xunit;

namespace Unit.Test.AnimalesTest.CerdaCriaTest;

public class RegistrarServicioCerdaCommandHandlerTest
{
    private readonly RegistrarServicioCerdaCommand command = new RegistrarServicioCerdaCommand("1",DateTime.Now );
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private readonly Mock<IAnimalesRepository> _mockAnimalesRepo = new();
    private readonly Mock<IEspacioFisicoRepository> _mockEspacioRepo = new();
    private readonly RegistrarServicioCerdaCommandHandler _handler;

    public RegistrarServicioCerdaCommandHandlerTest()
    {
        _handler = new RegistrarServicioCerdaCommandHandler(
            _mockUnitOfWork.Object,
            _mockAnimalesRepo.Object,
            _mockEspacioRepo.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCerdaNotFound()
    {

        _mockAnimalesRepo.Setup(r => r.ObtenerCerdaByIdentificacion(command.IdentificacionCerda)).ReturnsAsync((CerdaCria?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(CerdaCriaErrores.NoEncontrada, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenEstadoProductivoInvalid()
    {
        var cerda = new FakeCerdaCria(estadoProductivo: EstadoProductivo.Lactante);


        _mockAnimalesRepo.Setup(r => r.ObtenerCerdaByIdentificacion(command.IdentificacionCerda)).ReturnsAsync(cerda);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(CerdaCriaErrores.ErrorEstadoProductivoTrasladoMonta, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenEspacioFisicoActualNotFound()
    {
        var cerda = new FakeCerdaCria(estadoProductivo: EstadoProductivo.Ingreso);


        _mockAnimalesRepo.Setup(r => r.ObtenerCerdaByIdentificacion(command.IdentificacionCerda)).ReturnsAsync(cerda);
        _mockEspacioRepo.Setup(r => r.ObtenerSegunId(cerda.EspacioFisicoId, It.IsAny<CancellationToken>())).ReturnsAsync((EspacioFisico?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(EspacioFisicoErrores.EspacioFisicoNoExistente, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenNoEspacioMontaDisponible()
    {
        var cerda = new FakeCerdaCria(estadoProductivo: EstadoProductivo.Vacia);

        var actual = new FakeEspacioFisico("Gestacion");

        _mockAnimalesRepo.Setup(r => r.ObtenerCerdaByIdentificacion(command.IdentificacionCerda)).ReturnsAsync(cerda);
        _mockEspacioRepo.Setup(r => r.ObtenerSegunId(cerda.EspacioFisicoId, It.IsAny<CancellationToken>())).ReturnsAsync(actual);
        _mockEspacioRepo.Setup(r => r.ObtenerSegunTipo(TipoEspacio.Monta, It.IsAny<CancellationToken>())).ReturnsAsync((EspacioFisico?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(EspacioFisicoErrores.TipoEspacioIncorrectoOSinCapacidad, result.Error);
    }


    [Fact]
    public async Task Handle_ShouldChangeState_WhenAlreadyInMonta()
    {
        var cerda = new FakeCerdaCria(estadoProductivo:EstadoProductivo.Ingreso);

        var actual = new FakeEspacioFisico("Monta");

        _mockAnimalesRepo.Setup(r => r.ObtenerCerdaByIdentificacion(command.IdentificacionCerda)).ReturnsAsync(cerda);
        _mockEspacioRepo.Setup(r => r.ObtenerSegunId(cerda.EspacioFisicoId, It.IsAny<CancellationToken>())).ReturnsAsync(actual);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        _mockEspacioRepo.Verify(r => r.Actualizar(It.IsAny<EspacioFisico>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenSaveFails()
    {
        var cerda = new FakeCerdaCria(estadoProductivo: EstadoProductivo.Vacia,espacioFisicoId:new EspacioFisicoId(Guid.NewGuid()));

        var actual = new FakeEspacioFisico("Gestacion");
        var nuevo = new FakeEspacioFisico("Monta");

        _mockAnimalesRepo.Setup(r => r.ObtenerCerdaByIdentificacion(command.IdentificacionCerda)).ReturnsAsync(cerda);
        _mockEspacioRepo.Setup(r => r.ObtenerSegunId(It.IsAny<EspacioFisicoId>(), It.IsAny<CancellationToken>())).ReturnsAsync(actual);
        _mockEspacioRepo.Setup(r => r.ObtenerSegunTipo(TipoEspacio.Monta, It.IsAny<CancellationToken>())).ReturnsAsync(nuevo);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
}