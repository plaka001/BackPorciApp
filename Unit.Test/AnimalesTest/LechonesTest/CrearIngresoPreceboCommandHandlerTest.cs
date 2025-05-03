using Aplicacion.Animales.Lechones.Precebo;
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

public class CrearIngresoPreceboCommandHandlerTest
{
    private readonly Mock<IAnimalesRepository> _mockAnimalesRepository;
    private readonly Mock<IEspacioFisicoRepository> _mockEspacioFisicoRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CrearIngresoPreceboCommandHandler _handler;

    public CrearIngresoPreceboCommandHandlerTest()
    {
        _mockAnimalesRepository = new Mock<IAnimalesRepository>();
        _mockEspacioFisicoRepository = new Mock<IEspacioFisicoRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new CrearIngresoPreceboCommandHandler(
            _mockAnimalesRepository.Object,
            _mockEspacioFisicoRepository.Object,
            _mockUnitOfWork.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenDesteteNotFound()
    {
        // Arrange
        var command = new CrearIngresoPreceboCommand(
            DesteteId: Guid.NewGuid(),
            FechaIngreso: DateTime.Now,
            PesoPromedio: 6.5m,
            Comentario: "Test"
        );

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<DesteteId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Destete?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DesteteErrores.NoEncontrado, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenEspacioPreceboNotFound()
    {
        // Arrange
        var fakeDestete = FakeDestete.CreateDefault();
        var command = new CrearIngresoPreceboCommand(
            DesteteId: fakeDestete.Id.Value,
            FechaIngreso: DateTime.Now,
            PesoPromedio: 6.5m,
            Comentario: "Test"
        );

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<DesteteId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeDestete);

        _mockEspacioFisicoRepository
            .Setup(r => r.ObtenerSegunTipo(TipoEspacio.Precebo, It.IsAny<CancellationToken>()))
            .ReturnsAsync((EspacioFisico?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(EspacioFisicoErrores.TipoEspacioIncorrectoOSinCapacidad, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldCreatePrecebo_WhenAllConditionsMet()
    {
        // Arrange
        var fakeDestete = FakeDestete.CreateDefault();
        var fakeEspacio = EspacioFisico.Create(
            GranjaId.New(),
            TipoEspacio.Precebo.ToString(),
            cantidadEspacios: 10,
            capacidadPorEspacio: 1
        );

        var command = new CrearIngresoPreceboCommand(
            DesteteId: fakeDestete.Id.Value,
            FechaIngreso: DateTime.Now,
            PesoPromedio: 6.5m,
            Comentario: "Test"
        );

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<DesteteId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeDestete);

        _mockEspacioFisicoRepository
            .Setup(r => r.ObtenerSegunTipo(TipoEspacio.Precebo, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEspacio);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        // Verifica que se incrementó la capacidad del espacio
        Assert.Equal(fakeDestete.CantidadDestetados, fakeEspacio.Capacidad.CapacidadOcupada);

        // Verifica que se agregó el precebo
        _mockAnimalesRepository.Verify(
            r => r.agregarPrecebo(It.Is<Precebo>(p =>
                p.DesteteId.Value == fakeDestete.Id.Value &&
                p.PesoPromedioInicial == command.PesoPromedio
            )),
            Times.Once
        );

        // Verifica que se guardaron los cambios
        _mockUnitOfWork.Verify(
            u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task Handle_ShouldPropagateException_WhenSaveFails()
    {
        // Arrange
        var fakeDestete = FakeDestete.CreateDefault();
        var fakeEspacio = EspacioFisico.Create(
            GranjaId.New(),
            TipoEspacio.Precebo.ToString(),
            cantidadEspacios: 10,
            capacidadPorEspacio: 1
        );

        var command = new CrearIngresoPreceboCommand(
            DesteteId: fakeDestete.Id.Value,
            FechaIngreso: DateTime.Now,
            PesoPromedio: 6.5m,
            Comentario: "Test"
        );

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<DesteteId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeDestete);

        _mockEspacioFisicoRepository
            .Setup(r => r.ObtenerSegunTipo(TipoEspacio.Precebo, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEspacio);

        _mockUnitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("DB Error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenEspacioSinCapacidad()
    {
        // Arrange
        var fakeDestete = FakeDestete.CreateDefault();
        var fakeEspacio = EspacioFisico.Create(
            GranjaId.New(),
            TipoEspacio.Precebo.ToString(),
            cantidadEspacios: 1,
            capacidadPorEspacio: 1
        );

        // Ocupamos toda la capacidad
        fakeEspacio.IncrementarCapacidadOcupada(1);

        var command = new CrearIngresoPreceboCommand(
            DesteteId: fakeDestete.Id.Value,
            FechaIngreso: DateTime.Now,
            PesoPromedio: 6.5m,
            Comentario: "Test"
        );

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<DesteteId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeDestete);

        _mockEspacioFisicoRepository
            .Setup(r => r.ObtenerSegunTipo(TipoEspacio.Precebo, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEspacio);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None)
        );

        // Verifica el mensaje de la excepción
        Assert.Equal("No hay suficiente capacidad disponible.", exception.Message);
    }
}
