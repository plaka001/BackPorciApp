using Aplicacion.Animales.CerdaDeCria.Trasladar;
using Dominio.Abstractions;
using Dominio.Animales.CerdasCria;
using Dominio.Animales.Repository;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.EspacioFisicos;
using Dominio.EspacioFisicos.Repository;
using Dominio.EspaciosFisico;
using Dominio.granjas.ObjectValues;
using Moq;
using Xunit;
using Dominio.Animales.General;

namespace Unit.Test.AnimalesTest.CerdaCriaTest;

public class TrasladarCerdaCriaCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IAnimalesRepository> _mockAnimalesRepository;
    private readonly Mock<IEspacioFisicoRepository> _mockEspacioFisicoRepository;
    private readonly TrasladarCerdaCriaCommandHandler _handler;

    public TrasladarCerdaCriaCommandHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockAnimalesRepository = new Mock<IAnimalesRepository>();
        _mockEspacioFisicoRepository = new Mock<IEspacioFisicoRepository>();
        _handler = new TrasladarCerdaCriaCommandHandler(
            _mockUnitOfWork.Object,
            _mockAnimalesRepository.Object,
            _mockEspacioFisicoRepository.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenEspacioFisicoDoesNotExist()
    {
        // Arrange
        var command = new TrasladarCerdaCriaCommand(Guid.NewGuid(), Guid.NewGuid(), "Id123", EstadoProductivo.Reformada);

        var cerda = CerdaCria.Create(
            new GranjaId(Guid.NewGuid()),
            command.IdentificacionCerda,
            EstadoProductivo.Reformada,
            DateTime.Now,
            DateTime.Now,
            new EspacioFisicoId(command.EspacioFisicoOld),
            1,
            Guid.NewGuid()
        );

        _mockAnimalesRepository.Setup(r => r.ObtenerCerdaByIdentificacion(command.IdentificacionCerda))
            .ReturnsAsync(cerda);

        _mockEspacioFisicoRepository.Setup(r => r.ObtenerSegunId(It.IsAny<EspacioFisicoId>(),CancellationToken.None))
            .ReturnsAsync((EspacioFisico?)null); // Simulamos que no existe el espacio

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(EspacioFisicoErrores.EspacioFisicoNoExistente, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenTipoEspacioIncorrecto()
    {
        // Arrange
        var command = new TrasladarCerdaCriaCommand(Guid.NewGuid(), Guid.NewGuid(), "Id123", EstadoProductivo.Ingreso);

        var cerda = CerdaCria.Create(
            new GranjaId(Guid.NewGuid()),
            command.IdentificacionCerda,
            EstadoProductivo.Ingreso,
            DateTime.Now,
            DateTime.Now,
            new EspacioFisicoId(command.EspacioFisicoOld),
            1,
            Guid.NewGuid()
        );

        // Espacio físico antiguo (tipo correcto)
        var espacioOld = EspacioFisico.Create(
            new GranjaId(Guid.NewGuid()),
            "Gestacion", // Tipo correcto para la cerda actual
            10,
            1,
            0
        );

        // Espacio físico nuevo (tipo incorrecto para "Gestante")
        var espacioNew = EspacioFisico.Create(
            new GranjaId(Guid.NewGuid()),
            "Paridera", // Tipo incorrecto para Gestante
            10,
            1,
            0
        );

        _mockAnimalesRepository.Setup(r => r.ObtenerCerdaByIdentificacion(command.IdentificacionCerda))
            .ReturnsAsync(cerda);

        _mockEspacioFisicoRepository.Setup(r => r.ObtenerSegunId(new EspacioFisicoId(command.EspacioFisicoOld),CancellationToken.None))
            .ReturnsAsync(espacioOld);

        _mockEspacioFisicoRepository.Setup(r => r.ObtenerSegunId(new EspacioFisicoId(command.EspacioFisicoNew),CancellationToken.None))
            .ReturnsAsync(espacioNew);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(EspacioFisicoErrores.TipoEspacioIncorrecto, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNoCapacity()
    {
        // Arrange
        var command = new TrasladarCerdaCriaCommand(Guid.NewGuid(), Guid.NewGuid(), "Id123", EstadoProductivo.Servida);

        var cerda = CerdaCria.Create(
            new GranjaId(Guid.NewGuid()),
            command.IdentificacionCerda,
            EstadoProductivo.Ingreso,
            DateTime.Now,
            DateTime.Now,
            new EspacioFisicoId(command.EspacioFisicoOld),
            1,
            Guid.NewGuid()
        );

        // Espacio físico antiguo
        var espacioOld = EspacioFisico.Create(
            new GranjaId(Guid.NewGuid()),
            "Monta",
            10,
            1,
            0
        );

        // Espacio físico nuevo sin capacidad (ocupamos todos los espacios)
        var espacioNew = EspacioFisico.Create(
            new GranjaId(Guid.NewGuid()),
            "Monta",
            10,
            1,
            0
        );

        // Ocupamos toda la capacidad
        for (int i = 0; i < 10; i++)
        {
            espacioNew.IncrementarCapacidadOcupada(1);
        }

        _mockAnimalesRepository.Setup(r => r.ObtenerCerdaByIdentificacion(command.IdentificacionCerda))
            .ReturnsAsync(cerda);

        _mockEspacioFisicoRepository.Setup(r => r.ObtenerSegunId(new EspacioFisicoId(command.EspacioFisicoOld),CancellationToken.None))
            .ReturnsAsync(espacioOld);

        _mockEspacioFisicoRepository.Setup(r => r.ObtenerSegunId(new EspacioFisicoId(command.EspacioFisicoNew),CancellationToken.None))
            .ReturnsAsync(espacioNew);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(EspacioFisicoErrores.SinCapacidad, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldTrasladarCerda_WhenAllConditionsAreMet()
    {
        // Arrange
        var command = new TrasladarCerdaCriaCommand(Guid.NewGuid(), Guid.NewGuid(), "Id123", EstadoProductivo.Servida);

        var cerda = CerdaCria.Create(
            new GranjaId(Guid.NewGuid()),
            command.IdentificacionCerda,
            EstadoProductivo.Ingreso,
            DateTime.Now,
            DateTime.Now,
            new EspacioFisicoId(command.EspacioFisicoOld),
            1,
            Guid.NewGuid()
        );

        // Espacios físicos con capacidad
        var espacioOld = EspacioFisico.Create(
            new GranjaId(Guid.NewGuid()),
            "Monta",
            10,
            1,
            0
        );
        espacioOld.IncrementarCapacidadOcupada(1); // Cerda ocupa un espacio

        var espacioNew = EspacioFisico.Create(
            new GranjaId(Guid.NewGuid()),
            "Monta",
            10,
            1,
            0
        );

        _mockAnimalesRepository.Setup(r => r.ObtenerCerdaByIdentificacion(command.IdentificacionCerda))
            .ReturnsAsync(cerda);

        _mockEspacioFisicoRepository.Setup(r => r.ObtenerSegunId(new EspacioFisicoId(command.EspacioFisicoOld), CancellationToken.None))
            .ReturnsAsync(espacioOld);

        _mockEspacioFisicoRepository.Setup(r => r.ObtenerSegunId(new EspacioFisicoId(command.EspacioFisicoNew),CancellationToken.None))
            .ReturnsAsync(espacioNew);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(command.EspacioFisicoNew, cerda.EspacioFisicoId.Value); // Verificamos el nuevo espacio
        Assert.Equal(0, espacioOld.Capacidad.CapacidadOcupada); // Espacio antiguo liberado
        Assert.Equal(1, espacioNew.Capacidad.CapacidadOcupada); // Espacio nuevo ocupado
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
