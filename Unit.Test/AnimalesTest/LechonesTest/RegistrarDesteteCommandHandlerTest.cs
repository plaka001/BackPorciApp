using Aplicacion.Animales.Lechones.Destete;
using Dominio.Abstractions;
using Dominio.Animales.ObjectValues;
using Dominio.Animales;
using Dominio.Animales.Repository;
using Dominio.EspacioFisicos.Repository;
using Moq;
using Xunit;
using Dominio.Animales.CerdasCria;
using Unit.Test.Fakes;
using Dominio.EspacioFisicos;
using Dominio.EspaciosFisico;
using Dominio.Animales.General;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.granjas.ObjectValues;

namespace Unit.Test.AnimalesTest.LechonesTest;

public class RegistrarDesteteCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IAnimalesRepository> _mockAnimalesRepository;
    private readonly Mock<IEspacioFisicoRepository> _mockEspacioFisicoRepository;
    private readonly RegistrarDesteteCommandHandler _handler;

    public RegistrarDesteteCommandHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockAnimalesRepository = new Mock<IAnimalesRepository>();
        _mockEspacioFisicoRepository = new Mock<IEspacioFisicoRepository>();
        _handler = new RegistrarDesteteCommandHandler(
            _mockUnitOfWork.Object,
            _mockAnimalesRepository.Object,
            _mockEspacioFisicoRepository.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenPartoNotFound()
    {
        // Arrange
        var command = new RegistrarDesteteCommand(
            PartoId: Guid.NewGuid(),
            FechaDestete: DateTime.Now,
            CantidadVivos: 8,
            CantidadMuertos: 0,
            PesoPromedio: 6.5m,
            Comentario: "Test"
        );

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<PartoId>(),CancellationToken.None))
            .ReturnsAsync((Parto?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(PartoErrores.NoEncontrada, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCerdaNotFound()
    {
        // Arrange
        var fakeParto = new FakeParto();
        var command = new RegistrarDesteteCommand(
            PartoId: fakeParto.Id.Value,
            FechaDestete: DateTime.Now,
            CantidadVivos: 8,
            CantidadMuertos: 0,
            PesoPromedio: 6.5m,
            Comentario: "Test"
        );

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<PartoId>(), CancellationToken.None))
            .ReturnsAsync(fakeParto);

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(fakeParto.CerdaCriaId, CancellationToken.None))
            .ReturnsAsync((CerdaCria?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(CerdaCriaErrores.NoEncontrada, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenEspacioNotFound()
    {
        // Arrange
        var fakeParto = new FakeParto();
        var fakeCerda = new FakeCerdaCria(); // Asume que existe este fake
        var command = new RegistrarDesteteCommand(
            PartoId: fakeParto.Id.Value,
            FechaDestete: DateTime.Now,
            CantidadVivos: 8,
            CantidadMuertos: 0,
            PesoPromedio: 6.5m,
            Comentario: "Test"
        );

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<PartoId>(), CancellationToken.None))
            .ReturnsAsync(fakeParto);

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(fakeParto.CerdaCriaId, CancellationToken.None))
            .ReturnsAsync(fakeCerda);

        _mockEspacioFisicoRepository
            .Setup(r => r.ObtenerSegunId(fakeCerda.EspacioFisicoId, CancellationToken.None))
            .ReturnsAsync((EspacioFisico?)null); // Espacio actual no encontrado

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(EspacioFisicoErrores.TipoEspacioIncorrectoOSinCapacidad, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldRegisterDestete_WhenAllConditionsMet()
    {
        // Arrange
        var fakeParto = new FakeParto();
        var fakeCerda = new FakeCerdaCria(estadoProductivo: EstadoProductivo.Lactante);
        var espacioActual = EspacioFisico.Create(
            GranjaId.New(),
            "Gestación",
            cantidadEspacios: 10,
            capacidadPorEspacio: 1
        );
        espacioActual.IncrementarCapacidadOcupada(1); // Cerda ocupa 1 espacio

        var espacioMonta = EspacioFisico.Create(
            GranjaId.New(),
            "Monta",
            cantidadEspacios: 5,
            capacidadPorEspacio: 1
        );

        var command = new RegistrarDesteteCommand(
            PartoId: fakeParto.Id.Value,
            FechaDestete: DateTime.Now,
            CantidadVivos: 8,
            CantidadMuertos: 0,
            PesoPromedio: 6.5m,
            Comentario: "Test"
        );

        // Configuración de mocks
        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<PartoId>(), CancellationToken.None))
            .ReturnsAsync(fakeParto);

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(fakeParto.CerdaCriaId, CancellationToken.None))
            .ReturnsAsync(fakeCerda);

        _mockEspacioFisicoRepository
            .Setup(r => r.ObtenerSegunId(fakeCerda.EspacioFisicoId, CancellationToken.None))
            .ReturnsAsync(espacioActual);

        _mockEspacioFisicoRepository
            .Setup(r => r.ObtenerSegunTipo(TipoEspacio.Monta, CancellationToken.None))
            .ReturnsAsync(espacioMonta);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        // Verifica cambios en la cerda
        Assert.Equal(EstadoProductivo.Vacia, fakeCerda.EstadoProductivo);
        Assert.Equal(espacioMonta.Id, fakeCerda.EspacioFisicoId);

        // Verifica cambios en espacios físicos
        Assert.Equal(0, espacioActual.Capacidad.CapacidadOcupada); // Cerda salió
        Assert.Equal(1, espacioMonta.Capacidad.CapacidadOcupada); // Cerda entró

        // Verifica creación de destete
        _mockAnimalesRepository.Verify(
            r => r.AgregarDestete(It.Is<Dominio.Animales.Lechones.Destete>(d =>
                d.PartoId.Value == fakeParto.Id.Value &&
                d.CantidadDestetados == command.CantidadVivos
            )),
            Times.Once
        );

        // Verifica persistencia
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPropagateException_WhenSaveFails()
    {
        // Arrange
        var fakeParto = new FakeParto();
        var fakeCerda = new FakeCerdaCria(estadoProductivo: EstadoProductivo.Lactante);
        var espacioActual = EspacioFisico.Create(GranjaId.New(), "Vacia", 10, 1);
        var espacioMonta = EspacioFisico.Create(GranjaId.New(), "Monta", 5, 1);

        var command = new RegistrarDesteteCommand(
            PartoId: fakeParto.Id.Value,
            FechaDestete: DateTime.Now,
            CantidadVivos: 8,
            CantidadMuertos: 0,
            PesoPromedio: 6.5m,
            Comentario: "Test"
        );

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<PartoId>(), CancellationToken.None))
            .ReturnsAsync(fakeParto);

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(fakeParto.CerdaCriaId, CancellationToken.None))
            .ReturnsAsync(fakeCerda);

        _mockEspacioFisicoRepository
            .Setup(r => r.ObtenerSegunId(fakeCerda.EspacioFisicoId, CancellationToken.None))
            .ReturnsAsync(espacioActual);

        _mockEspacioFisicoRepository
            .Setup(r => r.ObtenerSegunTipo(TipoEspacio.Monta, CancellationToken.None))
            .ReturnsAsync(espacioMonta);

        _mockUnitOfWork
            .Setup(u => u.SaveChangesAsync(CancellationToken.None))
            .ThrowsAsync(new InvalidOperationException("DB Error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None)
        );
    }
}
