using Aplicacion.Animales.CerdaDeCria.Prenez;
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

public class ConfirmarPrenezCerdaCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private readonly Mock<IAnimalesRepository> _mockAnimalesRepo = new();
    private readonly Mock<IEspacioFisicoRepository> _mockEspacioRepo = new();
    private readonly ConfirmarPrenezCerdaCommandHandler _handler;

    public ConfirmarPrenezCerdaCommandHandlerTest()
    {
        _handler = new ConfirmarPrenezCerdaCommandHandler(
            _mockAnimalesRepo.Object,
            _mockUnitOfWork.Object,
            _mockEspacioRepo.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCerdaNotFound()
    {
        var command = FakeConfirmarPrenez.Default();
        _mockAnimalesRepo.Setup(r => r.ObtenerCerdaByIdentificacion(command.IdentificacionCerda)).ReturnsAsync((CerdaCria?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(CerdaCriaErrores.NoEncontrada, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenEstadoNotServida()
    {
        var cerda = FakeCerdaCria.ConEstado(EstadoProductivo.Vacia);
        var command = FakeConfirmarPrenez.Default();

        _mockAnimalesRepo.Setup(r => r.ObtenerCerdaByIdentificacion(command.IdentificacionCerda)).ReturnsAsync(cerda);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(ConfirmarPrenezCerdaErrores.CerdaExistente, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenEspacioFisicoNoExiste()
    {
        var cerda = FakeCerdaCria.ConEstado(EstadoProductivo.Servida);
        var command = FakeConfirmarPrenez.Default(estaPrenada: true);

        _mockAnimalesRepo.Setup(r => r.ObtenerCerdaByIdentificacion(command.IdentificacionCerda)).ReturnsAsync(cerda);
        _mockEspacioRepo.Setup(r => r.ObtenerSegunTipo(TipoEspacio.Gestacion, It.IsAny<CancellationToken>())).ReturnsAsync((EspacioFisico?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(EspacioFisicoErrores.EspacioFisicoNoExistente, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenNoCapacidad()
    {
        var cerda = FakeCerdaCria.ConEstado(EstadoProductivo.Servida);
        var newEspacio = FakeEspacioFisico.DeTipo("Gestacion", tieneCapacidad: false);
        var oldEspacio = FakeEspacioFisico.DeTipo("Monta");

        var command = FakeConfirmarPrenez.Default(estaPrenada: true);

        _mockAnimalesRepo.Setup(r => r.ObtenerCerdaByIdentificacion(command.IdentificacionCerda)).ReturnsAsync(cerda);
        _mockEspacioRepo.Setup(r => r.ObtenerSegunTipo(TipoEspacio.Gestacion, It.IsAny<CancellationToken>())).ReturnsAsync(newEspacio);
        _mockEspacioRepo.Setup(r => r.ObtenerSegunId(cerda.EspacioFisicoId, It.IsAny<CancellationToken>())).ReturnsAsync(oldEspacio);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(EspacioFisicoErrores.SinCapacidad, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldTransfer_WhenPrenada()
    {
        var cerda = FakeCerdaCria.ConEstado(EstadoProductivo.Servida);
        var newEspacio = FakeEspacioFisico.DeTipo("Gestacion");
        var oldEspacio = FakeEspacioFisico.DeTipo("Monta");
        oldEspacio.IncrementarCapacidadOcupada(1);

        var command = FakeConfirmarPrenez.Default(estaPrenada: true);

        _mockAnimalesRepo.Setup(r => r.ObtenerCerdaByIdentificacion(command.IdentificacionCerda)).ReturnsAsync(cerda);
        _mockEspacioRepo.Setup(r => r.ObtenerSegunTipo(TipoEspacio.Gestacion, It.IsAny<CancellationToken>())).ReturnsAsync(newEspacio);
        _mockEspacioRepo.Setup(r => r.ObtenerSegunId(cerda.EspacioFisicoId, It.IsAny<CancellationToken>())).ReturnsAsync(oldEspacio);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        _mockEspacioRepo.Verify(r => r.Actualizar(newEspacio), Times.Once);
        _mockEspacioRepo.Verify(r => r.Actualizar(oldEspacio), Times.Once);
        _mockUnitOfWork.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldChangeStateToVacia_WhenNoPrenada()
    {
        var cerda = FakeCerdaCria.ConEstado(EstadoProductivo.Servida);
        var command = FakeConfirmarPrenez.Default(estaPrenada: false);

        _mockAnimalesRepo.Setup(r => r.ObtenerCerdaByIdentificacion(command.IdentificacionCerda)).ReturnsAsync(cerda);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        _mockAnimalesRepo.Verify(r => r.Actualizar(cerda), Times.Once);
        _mockUnitOfWork.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
