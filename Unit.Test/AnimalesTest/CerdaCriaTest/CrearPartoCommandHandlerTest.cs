using Aplicacion.Animales.CerdaDeCria.Parto.Crear;
using Dominio.Abstractions;
using Dominio.Animales.CerdasCria;
using Dominio.Animales.ObjectValues;
using Dominio.Animales;
using Dominio.Animales.Repository;
using Moq;
using Xunit;
using Unit.Test.Fakes;

namespace Unit.Test.AnimalesTest.CerdaCriaTest;

public class CrearPartoCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IAnimalesRepository> _mockAnimalesRepository;
    private readonly CrearPartoCommandHandler _handler;

    public CrearPartoCommandHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockAnimalesRepository = new Mock<IAnimalesRepository>();
        _handler = new CrearPartoCommandHandler(_mockUnitOfWork.Object, _mockAnimalesRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCerdaDoesNotExist()
    {
        // Arrange
        var command = new CrearPartoCommand(
             Guid.NewGuid(),
             10,
             1,
             5.2m,
             2.2m,
             true,
             "Coment",
             DateTime.Now
        );


        _mockAnimalesRepository.Setup(r => r.ObtenerSegunId(It.IsAny<CerdaCriaId>(), CancellationToken.None))
            .ReturnsAsync((CerdaCria?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(CerdaCriaErrores.NoEncontrada, result.Error);
        _mockAnimalesRepository.Verify(r => r.AgregarParto(It.IsAny<Parto>()), Times.Never);
    }


    [Fact]
    public async Task Handle_ShouldCreateParto_WhenCerdaExists()
    {
        // Arrange
        var command = new CrearPartoCommand(
            Guid.NewGuid(),
            10,
            1,
            5.2m,
            2.2m,
            true,
            "Coment",
            DateTime.Now
       );

        var fakeCerda = new FakeCerdaCria(numeroParto: 2);

        _mockAnimalesRepository.Setup(r => r.ObtenerSegunId(It.IsAny<CerdaCriaId>(), CancellationToken.None))
            .ReturnsAsync(fakeCerda);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(fakeCerda.RegistrarPartoLlamado); // Verifica que se llamó al método
        Assert.Equal(3, fakeCerda.NumeroParto); // Verifica que se incrementó el número de partos
    }

    [Fact]
    public async Task Handle_ShouldPropagateException_WhenSaveFails()
    {
        // Arrange
        var command = new CrearPartoCommand(
           Guid.NewGuid(),
           10,
           1,
           5.2m,
           2.2m,
           true,
           "Coment",
           DateTime.Now
      );

        var fakeCerda = new FakeCerdaCria(numeroParto: 2);
        _mockAnimalesRepository.Setup(r => r.ObtenerSegunId(It.IsAny<CerdaCriaId>(), CancellationToken.None))
            .ReturnsAsync(fakeCerda);

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Error de base de datos"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None)
        );
    }

    [Theory]
    [InlineData(0, 0, 0, 0)] // Sin lechones vivos/muertos
    [InlineData(-1, 0, 1, 1)] // Cantidad negativa
    public async Task Handle_ShouldReturnFailure_WhenInvalidPartoData(
    int vivos, int muertos, decimal pesoVivos, decimal pesoMuertos)
    {
        // Arrange
        var command = new CrearPartoCommand(
            CerdaId: Guid.NewGuid(),
            FechaDeParto: DateTime.Now,
            CantidadVivos: vivos,
            CantidadMuertos: muertos,
            PesoPromedioVivos: pesoVivos,
            PesoPromedioMuertos: pesoMuertos,
            UsoOxitocina: false,
            Comentario: "Datos inválidos"
        );

        var fakeCerda = new FakeCerdaCria(numeroParto: 2);
        _mockAnimalesRepository.Setup(r => r.ObtenerSegunId(It.IsAny<CerdaCriaId>(),CancellationToken.None))
            .ReturnsAsync(fakeCerda);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _handler.Handle(command, CancellationToken.None)
        );

        // Verifica el mensaje de la excepción
        Assert.Equal("Las cantidades no pueden ser negativas", exception.Message);
    }
}
