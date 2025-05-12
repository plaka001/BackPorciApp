using Aplicacion.Animales.CerdaDeCria.Crear;
using Dominio.Abstractions;
using Dominio.Animales.CerdasCria;
using Dominio.Animales.General;
using Dominio.Animales.Repository;
using Dominio.EspacioFisicos;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.EspacioFisicos.Repository;
using Dominio.granjas.ObjectValues;
using Moq;
using Unit.Test.Fakes;
using Xunit;

namespace Unit.Test.AnimalesTest.CerdaCriaTest;

public class CrearCerdaCriaCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IAnimalesRepository> _mockAnimalesRepository;
    private readonly Mock<IEspacioFisicoRepository> _mockEspacioFisicoRepository;
    private readonly CrearCerdaCriaCommandHandler _handler;

    public CrearCerdaCriaCommandHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockAnimalesRepository = new Mock<IAnimalesRepository>();
        _mockEspacioFisicoRepository = new Mock<IEspacioFisicoRepository>();
        _handler = new CrearCerdaCriaCommandHandler(
            _mockAnimalesRepository.Object,
            _mockEspacioFisicoRepository.Object,
            _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCerdaAlreadyExists()
    {
        // Arrange
        var command = new CrearCerdaCriaCommand(
            Guid.NewGuid(), "ID123", EstadoProductivo.Servida, DateTime.Now, Guid.NewGuid(), 1, Guid.NewGuid());

        var existingCerda = CerdaCria.Create(
            new GranjaId(command.GranjaId),
            command.Identificacion,
            command.EstadoProductivo,
            DateTime.Now,
            DateTime.Now,
            new EspacioFisicoId(command.EspacioFisicoId),
            command.NumeroDeParto,
            command.PlanSanitarioId);

        _mockAnimalesRepository.Setup(r => r.ObtenerCerdaByIdentificacion(command.Identificacion))
            .ReturnsAsync(existingCerda);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        _mockAnimalesRepository.Verify(r => r.AgregarCerdaCria(It.IsAny<CerdaCria>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenEspacioFisicoDoesNotExist()
    {
        // Arrange
        var command = new CrearCerdaCriaCommand(
            Guid.NewGuid(), "ID123", EstadoProductivo.Servida, DateTime.Now, Guid.NewGuid(), 1, Guid.NewGuid());


        _mockAnimalesRepository.Setup(r => r.ObtenerCerdaByIdentificacion(command.Identificacion))
            .ReturnsAsync((CerdaCria?)null);

        _mockEspacioFisicoRepository.Setup(r => r.ObtenerSegunId(It.IsAny<EspacioFisicoId>(), CancellationToken.None))
            .ReturnsAsync((EspacioFisico?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        _mockAnimalesRepository.Verify(r => r.AgregarCerdaCria(It.IsAny<CerdaCria>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenTipoEspacioIncorrecto()
    {
        // Arrange
        var command = new CrearCerdaCriaCommand(
            Guid.NewGuid(), "ID123", EstadoProductivo.Reformada, DateTime.Now, Guid.NewGuid(), 1, Guid.NewGuid());

        // Usamos FakeEspacioFisico con EsTipoCorrecto = false
        var fakeEspacio = new FakeEspacioFisico(
            tipoEspacio: "Reformada",
            tieneCapacidad: true,
            esTipoCorrecto: false);

        _mockAnimalesRepository.Setup(r => r.ObtenerCerdaByIdentificacion(command.Identificacion))
            .ReturnsAsync((CerdaCria?)null);

        _mockEspacioFisicoRepository.Setup(r => r.ObtenerSegunId(It.IsAny<EspacioFisicoId>(), CancellationToken.None))
            .ReturnsAsync(fakeEspacio); // ← Pasamos el fake

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        _mockAnimalesRepository.Verify(r => r.AgregarCerdaCria(It.IsAny<CerdaCria>()), Times.Never);
    }
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNoCapacity()
    {
        // Arrange
        var command = new CrearCerdaCriaCommand(
            Guid.NewGuid(), "ID123", EstadoProductivo.Ingreso, DateTime.Now, Guid.NewGuid(), 20, Guid.NewGuid());

        // Fake con capacidad = false
        var fakeEspacio = new FakeEspacioFisico(
            tipoEspacio: "Ingreso",
            tieneCapacidad: false, // ← Sin capacidad
            esTipoCorrecto: true);

        _mockAnimalesRepository.Setup(r => r.ObtenerCerdaByIdentificacion(command.Identificacion))
            .ReturnsAsync((CerdaCria?)null);

        _mockEspacioFisicoRepository.Setup(r => r.ObtenerSegunId(It.IsAny<EspacioFisicoId>(), CancellationToken.None))
            .ReturnsAsync(fakeEspacio);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        _mockAnimalesRepository.Verify(r => r.AgregarCerdaCria(It.IsAny<CerdaCria>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldCreateCerda_WhenAllConditionsAreMet()
    {
        // Arrange
        var command = new CrearCerdaCriaCommand(
            Guid.NewGuid(), "ID123", EstadoProductivo.Servida, DateTime.Now, Guid.NewGuid(), 1, Guid.NewGuid());

        // Fake con todo correcto
        var fakeEspacio = new FakeEspacioFisico(
            tipoEspacio: "Servida",
            tieneCapacidad: true,
            esTipoCorrecto: true);

        _mockAnimalesRepository.Setup(r => r.ObtenerCerdaByIdentificacion(command.Identificacion))
            .ReturnsAsync((CerdaCria?)null);

        _mockEspacioFisicoRepository.Setup(r => r.ObtenerSegunId(It.IsAny<EspacioFisicoId>(), CancellationToken.None))
            .ReturnsAsync(fakeEspacio);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _mockAnimalesRepository.Verify(r => r.AgregarCerdaCria(It.IsAny<CerdaCria>()), Times.Once);
        Assert.Equal(1, fakeEspacio.Capacidad.CapacidadOcupada); // Verificamos que se incrementó la capacidad
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
