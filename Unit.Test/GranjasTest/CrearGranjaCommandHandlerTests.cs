using Aplicacion.Granjas.CrearGranja;
using Domain.Granjas;
using Dominio.Abstractions;
using Dominio.granjas.repository;
using Dominio.Granjas;
using Moq;
using Xunit;

namespace Unit.Test.GranjasTest;

public class CrearGranjaCommandHandlerTests
{

    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IGranjaRepository> _mockRepository;
    private readonly CrearGranjaCommandHandler _handler;

    public CrearGranjaCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockRepository = new Mock<IGranjaRepository>();
        _handler = new CrearGranjaCommandHandler(_mockUnitOfWork.Object, _mockRepository.Object);
    }

    [Fact]
    public async void Handle_ShouldReturnFailure_WhenGranjaAlreadyExists()
    {
        // Arrange
        var command = new CrearGranjaCommand("Nombre", 10, "Ubicacion", DateTime.Now);
        var existingGranja = Granja.Create(command.Nombre, command.Ubicacion!, command.NumeroCerdasCria, command.FechaInicioOperaciones);

        _mockRepository.Setup(r => r.ObtenerGranjaByNombre(command.Nombre))
                       .ReturnsAsync(existingGranja);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        _mockRepository.Verify(r => r.Agregar(It.IsAny<Granja>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldCreateGranjaAndReturnSuccess_WhenGranjaDoesNotExist()
    {
        // Arrange
        var command = new CrearGranjaCommand("Granja Nueva", 120, "Ubicación B", DateTime.UtcNow);

        _mockRepository.Setup(r => r.ObtenerGranjaByNombre(command.Nombre))
                       .ReturnsAsync((Granja?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        _mockRepository.Verify(r => r.Agregar(It.Is<Granja>(g =>
            g.Nombre == command.Nombre &&
            g.Ubicacion == command.Ubicacion &&
            g.NumeroCerdasCria == command.NumeroCerdasCria
        )), Times.Once);

        _mockRepository.Verify(r => r.AgregarParametrosProduccion(It.IsAny<ParametrosProduccion>()), Times.Once);
        _mockRepository.Verify(r => r.AgregarParametrosProduccionCalculados(It.IsAny<ParametrosCalculados>()), Times.Once);

        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPropagateException_WhenSaveFails()
    {
        // Arrange
        var command = new CrearGranjaCommand("Granja Ex", 80, "X", DateTime.UtcNow);

        _mockRepository.Setup(r => r.ObtenerGranjaByNombre(command.Nombre))
                       .ReturnsAsync((Granja?)null);

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .ThrowsAsync(new InvalidOperationException("DB error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }

}
