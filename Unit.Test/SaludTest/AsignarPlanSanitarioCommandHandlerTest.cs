using Aplicacion.Salud.PlanesSanitarios.Asignar;
using Dominio.Abstractions;
using Dominio.Animales.CerdasCria;
using Dominio.Animales.ObjectValues;
using Dominio.Animales.Repository;
using Dominio.Salud;
using Dominio.Salud.ObjectValues;
using Dominio.Salud.Repository;
using Moq;
using Unit.Test.Fakes;
using Xunit;

namespace Unit.Test.PlanSanitarioTest;

public class AsignarPlanSanitarioCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPlanSanitarioRepository> _mockPlanRepository;
    private readonly Mock<IAnimalesRepository> _mockAnimalesRepository;
    private readonly AsignarPlanSanitarioCommandHandler _handler;

    public AsignarPlanSanitarioCommandHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPlanRepository = new Mock<IPlanSanitarioRepository>();
        _mockAnimalesRepository = new Mock<IAnimalesRepository>();

        _handler = new AsignarPlanSanitarioCommandHandler(
            _mockUnitOfWork.Object,
            _mockPlanRepository.Object,
            _mockAnimalesRepository.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenPlanDoesNotExist()
    {
        var command = FakeAsignarPlanSanitario.Default();

        _mockPlanRepository
            .Setup(r => r.ObtenerPorIdConEventosAsync(It.IsAny<PlanSanitarioId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((PlanSanitario?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(PlanSanitarioErrores.PlanSanitarioNoExistente, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenEntityDoesNotExist()
    {
        var command = FakeAsignarPlanSanitario.Default();
        var plan = FakeAsignarPlanSanitario.PlanConEventos();

        _mockPlanRepository
            .Setup(r => r.ObtenerPorIdConEventosAsync(It.IsAny<PlanSanitarioId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(plan);

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<CerdaCriaId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<CerdaCria>());

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(EventoSanitarioProgramadoErrores.EntidadNoExiste, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldSucceed_WhenValid()
    {
        var command = FakeAsignarPlanSanitario.Default();
        var plan = FakeAsignarPlanSanitario.PlanConEventos();

        _mockPlanRepository
            .Setup(r => r.ObtenerPorIdConEventosAsync(It.IsAny<PlanSanitarioId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(plan);

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<CerdaCriaId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(FakeCerdaCria.CreateDefault(Dominio.Animales.General.EstadoProductivo.Ingreso));

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenSaveFails()
    {
        var command = FakeAsignarPlanSanitario.Default();
        var plan = FakeAsignarPlanSanitario.PlanConEventos();

        _mockPlanRepository
            .Setup(r => r.ObtenerPorIdConEventosAsync(It.IsAny<PlanSanitarioId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(plan);

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<CerdaCriaId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(FakeCerdaCria.CreateDefault(estadoProductivo: Dominio.Animales.General.EstadoProductivo.Ingreso));

        _mockUnitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
}
