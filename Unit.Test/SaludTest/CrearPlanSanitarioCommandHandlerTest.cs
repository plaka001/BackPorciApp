using Aplicacion.Salud.PlanesSanitarios.Crear;
using Dominio.Abstractions;
using Dominio.Salud;
using Dominio.Salud.Repository;
using Moq;
using Unit.Test.Fakes;
using Xunit;

namespace Unit.Test.SaludTest;

public class CrearPlanSanitarioCommandHandlerTest
{
    private readonly Mock<IPlanSanitarioRepository> _mockRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CrearPlanSanitarioCommandHandler _handler;

    public CrearPlanSanitarioCommandHandlerTest()
    {
        _mockRepository = new Mock<IPlanSanitarioRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new CrearPlanSanitarioCommandHandler(_mockRepository.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenPlanAlreadyExists()
    {
        var command = FakeCrearPlanSanitarioCommand.Default();

        _mockRepository
            .Setup(r => r.ObtenerSegunNombre(command.Nombre, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FakePlanSanitario());

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(PlanSanitarioErrores.PlanSanitarioExistente, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenNoEventos()
    {
        var command = FakeCrearPlanSanitarioCommand.WithoutEventos();

        _mockRepository
            .Setup(r => r.ObtenerSegunNombre(command.Nombre, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PlanSanitario?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(PlanSanitarioErrores.DebeAlMenosTenerUnEvento, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldSucceed_WhenValidInput()
    {
        var command = FakeCrearPlanSanitarioCommand.Default();

        _mockRepository
            .Setup(r => r.ObtenerSegunNombre(command.Nombre, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PlanSanitario?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        _mockRepository.Verify(r => r.Agregar(It.IsAny<PlanSanitario>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenSaveFails()
    {
        var command = FakeCrearPlanSanitarioCommand.Default();

        _mockRepository
            .Setup(r => r.ObtenerSegunNombre(command.Nombre, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PlanSanitario?)null);

        _mockUnitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("DB Failure"));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
}
