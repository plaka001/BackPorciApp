using Aplicacion.Animales.Lechones.Ceba.RegistrarEntrada;
using Dominio.Abstractions;
using Dominio.Animales.Lechones;
using Dominio.Animales.ObjectValues;
using Dominio.Animales.Repository;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.EspacioFisicos;
using Dominio.EspacioFisicos.Repository;
using Dominio.EspaciosFisico;
using Moq;
using Unit.Test.Fakes;
using Xunit;
using Dominio.granjas.ObjectValues;

namespace Unit.Test.AnimalesTest.LechonesTest;

public class RegistrarEntradaCebaCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IAnimalesRepository> _mockAnimalesRepository;
    private readonly Mock<IEspacioFisicoRepository> _mockEspacioFisicoRepository;
    private readonly RegistrarEntradaCebaCommandHandler _handler;

    public RegistrarEntradaCebaCommandHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockAnimalesRepository = new Mock<IAnimalesRepository>();
        _mockEspacioFisicoRepository = new Mock<IEspacioFisicoRepository>();
        _handler = new RegistrarEntradaCebaCommandHandler(
            _mockUnitOfWork.Object,
            _mockAnimalesRepository.Object,
            _mockEspacioFisicoRepository.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenPreceboNotFound()
    {
        // Arrange
        var command = new RegistrarEntradaCebaCommand(
            PreceboId: Guid.NewGuid(),
            CantidadInicial: 10,
            PesoPromedioInicial: 20.5m,
            FechaIngreso: DateTime.Now,
            Comentario: "Test"
        );

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<PreceboId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Precebo?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(PreceboErrores.NoEncontrado, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCantidadInvalida()
    {
        // Arrange
        var fakePrecebo = new FakePrecebo(cantidadInicial: 10);
        var command = new RegistrarEntradaCebaCommand(
            PreceboId: fakePrecebo.Id.Value,
            CantidadInicial: 15, // Mayor que la cantidad inicial (10)
            PesoPromedioInicial: 20.5m,
            FechaIngreso: DateTime.Now,
            Comentario: "Test"
        );

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<PreceboId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakePrecebo);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(CebaErrores.CebaErrorCantidad, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenEspacioNotFound()
    {
        // Arrange
        var fakePrecebo = new FakePrecebo(cantidadInicial: 10);
        var command = new RegistrarEntradaCebaCommand(
            PreceboId: fakePrecebo.Id.Value,
            CantidadInicial: 8,
            PesoPromedioInicial: 20.5m,
            FechaIngreso: DateTime.Now,
            Comentario: "Test"
        );

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<PreceboId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakePrecebo);

        _mockEspacioFisicoRepository
            .Setup(r => r.ObtenerSegunTipo(TipoEspacio.Ceba, It.IsAny<CancellationToken>()))
            .ReturnsAsync((EspacioFisico?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(EspacioFisicoErrores.TipoEspacioIncorrectoOSinCapacidad, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldRegisterEntrada_WhenAllConditionsMet()
    {
        // Arrange
        var fakePrecebo = new FakePrecebo(cantidadInicial: 10);
        var espacioAntiguo = EspacioFisico.Create(
            GranjaId.New(),
            "Precebo",
            cantidadEspacios: 10,
            capacidadPorEspacio: 1
        );
        espacioAntiguo.IncrementarCapacidadOcupada(10); // Precebo ocupa 10 espacios

        var espacioCeba = EspacioFisico.Create(
            GranjaId.New(),
            "Ceba",
            cantidadEspacios: 20,
            capacidadPorEspacio: 1
        );

        var command = new RegistrarEntradaCebaCommand(
            PreceboId: fakePrecebo.Id.Value,
            CantidadInicial: 8,
            PesoPromedioInicial: 20.5m,
            FechaIngreso: DateTime.Now,
            Comentario: "Test"
        );

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<PreceboId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakePrecebo);

        _mockEspacioFisicoRepository
            .Setup(r => r.ObtenerSegunId(fakePrecebo.EspacioFisicoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(espacioAntiguo);

        _mockEspacioFisicoRepository
            .Setup(r => r.ObtenerSegunTipo(TipoEspacio.Ceba, It.IsAny<CancellationToken>()))
            .ReturnsAsync(espacioCeba);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        // Verifica que se registró la salida del precebo
        Assert.True(fakePrecebo.SalidaRegistrada);
        Assert.Equal(2, fakePrecebo.CantidadMuertosSimulada); // 10 - 8 = 2 muertos

        // Verifica espacios físicos
        Assert.Equal(0, espacioAntiguo.Capacidad.CapacidadOcupada); // 10 - 8 = 2 (solo quedan los muertos)
        Assert.Equal(8, espacioCeba.Capacidad.CapacidadOcupada); // Se agregaron 8

        // Verifica creación de ceba
        _mockAnimalesRepository.Verify(
            r => r.AgregarCeba(It.Is<Ceba>(c =>
                c.PreceboId.Value == fakePrecebo.Id.Value &&
                c.CantidadInicial == command.CantidadInicial
            )),
            Times.Once
        );

        // Verifica persistencia
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPropagateException_WhenSaveFails()
    {
        // Arrange
        var fakePrecebo = new FakePrecebo(cantidadInicial: 10);
        var espacioAntiguo = EspacioFisico.Create(GranjaId.New(), "Precebo", 10, 1);
        var espacioCeba = EspacioFisico.Create(GranjaId.New(), "Ceba", 20, 1);

        var command = new RegistrarEntradaCebaCommand(
            PreceboId: fakePrecebo.Id.Value,
            CantidadInicial: 8,
            PesoPromedioInicial: 20.5m,
            FechaIngreso: DateTime.Now,
            Comentario: "Test"
        );

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<PreceboId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakePrecebo);

        _mockEspacioFisicoRepository
            .Setup(r => r.ObtenerSegunId(fakePrecebo.EspacioFisicoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(espacioAntiguo);

        _mockEspacioFisicoRepository
            .Setup(r => r.ObtenerSegunTipo(TipoEspacio.Ceba, It.IsAny<CancellationToken>()))
            .ReturnsAsync(espacioCeba);

        _mockUnitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("DB Error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None)
        );
    }
}
