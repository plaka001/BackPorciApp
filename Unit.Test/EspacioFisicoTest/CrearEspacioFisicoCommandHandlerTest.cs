using Aplicacion.EspacioFisico.Crear;
using Aplicacion.EspacioFisicos.Crear;
using Domain.Granjas;
using Dominio.Abstractions;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.EspacioFisicos.Repository;
using Dominio.granjas.ObjectValues;
using Dominio.granjas.repository;
using Dominio.Granjas;
using Moq;
using Unit.Test.Fakes;
using Xunit;

namespace Unit.Test.EspacioFisicoTest;

public class CrearEspacioFisicoCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IEspacioFisicoRepository> _mockRepository;
    private readonly Mock<IGranjaRepository> _mockGranjaRepository;
    private readonly CrearEspacioFisicoCommandHandler _handler;

    public CrearEspacioFisicoCommandHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockRepository = new Mock<IEspacioFisicoRepository>();
        _mockGranjaRepository = new Mock<IGranjaRepository>();
        _handler = new CrearEspacioFisicoCommandHandler(
            _mockUnitOfWork.Object,
            _mockRepository.Object,
            _mockGranjaRepository.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenGranjaNotFound()
    {
        // Arrange
        var command = new CrearEspacioFisicoCommand(
            GranjaId: Guid.NewGuid(),
            TipoEspacio: TipoEspacio.Ceba,
            CantidadEspacios: 10,
            CapacidadPorEspacio: 1
        );

        _mockGranjaRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<GranjaId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Granja?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(GranjaErrores.GranjaNoExistente, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenParametrosCalculadosNotFound()
    {
        // Arrange
        var fakeGranja = new FakeGranja();
        var command = new CrearEspacioFisicoCommand(
            GranjaId: fakeGranja.Id.Value,
            TipoEspacio: TipoEspacio.Ceba,
            CantidadEspacios: 10,
            CapacidadPorEspacio: 1
        );

        _mockGranjaRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<GranjaId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeGranja);

        _mockGranjaRepository
            .Setup(r => r.obtenerParametrsCalculados(It.IsAny<GranjaId>()))
            .ReturnsAsync((ParametrosCalculados?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ParametrosCalculadosErrores.ParametrosCalculadosNoExisten, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenTipoEspacioNotSupported()
    {
        // Arrange
        var fakeGranja = new FakeGranja();
        var fakeParametros = new FakeParametrosCalculados();
        var command = new CrearEspacioFisicoCommand(
            GranjaId: fakeGranja.Id.Value,
            TipoEspacio: (TipoEspacio)999, // Tipo inválido
            CantidadEspacios: 10,
            CapacidadPorEspacio: 1
        );

        _mockGranjaRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<GranjaId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeGranja);

        _mockGranjaRepository
            .Setup(r => r.obtenerParametrsCalculados(It.IsAny<GranjaId>()))
            .ReturnsAsync(fakeParametros);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_ShouldCreateEspacioCeba_WhenAllConditionsMet()
    {
        // Arrange
        var fakeGranja = new FakeGranja();
        var fakeParametros = new FakeParametrosCalculados(espaciosCeba: 50); // Capacidad esperada: 50
        var command = new CrearEspacioFisicoCommand(
            GranjaId: fakeGranja.Id.Value,
            TipoEspacio: TipoEspacio.Ceba,
            CantidadEspacios: 10,
            CapacidadPorEspacio: 1
        );

        _mockGranjaRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<GranjaId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeGranja);

        _mockGranjaRepository
            .Setup(r => r.obtenerParametrsCalculados(It.IsAny<GranjaId>()))
            .ReturnsAsync(fakeParametros);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        // Verifica que se creó el espacio con la capacidad recomendada correcta (50)
        _mockRepository.Verify(
            r => r.Agregar(It.Is<Dominio.EspacioFisicos.EspacioFisico>(e =>
                e.GranjaId.Value == command.GranjaId &&
                e.TipoEspacio == command.TipoEspacio.ToString() &&
                e.CapacidadRecomendada == 50 // Verifica el cálculo
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
        var fakeGranja = new FakeGranja();
        var fakeParametros = new FakeParametrosCalculados();
        var command = new CrearEspacioFisicoCommand(
            GranjaId: fakeGranja.Id.Value,
            TipoEspacio: TipoEspacio.Ceba,
            CantidadEspacios: 10,
            CapacidadPorEspacio: 1
        );

        _mockGranjaRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<GranjaId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeGranja);

        _mockGranjaRepository
            .Setup(r => r.obtenerParametrsCalculados(It.IsAny<GranjaId>()))
            .ReturnsAsync(fakeParametros);

        _mockUnitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("DB Error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_ShouldCreateEspacioParidera_WhenTipoIsParidera()
    {
        // Arrange
        var fakeGranja = new FakeGranja();
        var fakeParametros = new FakeParametrosCalculados(espaciosParideras: 20);
        var command = new CrearEspacioFisicoCommand(
            GranjaId: fakeGranja.Id.Value,
            TipoEspacio: TipoEspacio.Paridera,
            CantidadEspacios: 5,
            CapacidadPorEspacio: 1
        );

        _mockGranjaRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<GranjaId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeGranja);

        _mockGranjaRepository
            .Setup(r => r.obtenerParametrsCalculados(It.IsAny<GranjaId>()))
            .ReturnsAsync(fakeParametros);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _mockRepository.Verify(
            r => r.Agregar(It.Is<Dominio.EspacioFisicos.EspacioFisico>(e =>
                e.CapacidadRecomendada == 20
            )),
            Times.Once
        );
    }
}
