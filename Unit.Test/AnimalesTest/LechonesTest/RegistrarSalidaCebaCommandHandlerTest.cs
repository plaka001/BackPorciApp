using Aplicacion.Animales.Lechones.Ceba.RegistrarSalida;
using Dominio.Abstractions;
using Dominio.Animales.Lechones;
using Dominio.Animales.ObjectValues;
using Dominio.Animales.Repository;
using Dominio.EspacioFisicos;
using Dominio.EspacioFisicos.Repository;
using Dominio.EspaciosFisico;
using Dominio.granjas.ObjectValues;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unit.Test.Fakes;
using Xunit;

namespace Unit.Test.AnimalesTest.LechonesTest;

public class RegistrarSalidaCebaCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IEspacioFisicoRepository> _mockEspacioFisicoRepository;
    private readonly Mock<IAnimalesRepository> _mockAnimalesRepository;
    private readonly RegistrarSalidaCebaCommandHandler _handler;

    public RegistrarSalidaCebaCommandHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockEspacioFisicoRepository = new Mock<IEspacioFisicoRepository>();
        _mockAnimalesRepository = new Mock<IAnimalesRepository>();
        _handler = new RegistrarSalidaCebaCommandHandler(
            _mockUnitOfWork.Object,
            _mockEspacioFisicoRepository.Object,
            _mockAnimalesRepository.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCebaNotFound()
    {
        // Arrange
        var command = new RegistrarSalidaCebaCommand(
            CebaId: Guid.NewGuid(),
            FechaSalida: DateTime.Now,
            PesoPromedioFinal: 85.5m,
            CantidadMuertos: 2,
            CantidadVivos: 10
        );

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<CebaId>(),CancellationToken.None))
            .ReturnsAsync((Ceba?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(CebaErrores.CebaNoEncontrada, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenEspacioNotFound()
    {
        // Arrange
        var fakeCeba = new FakeCeba(cantidadInicial: 15);
        var command = new RegistrarSalidaCebaCommand(
            CebaId: fakeCeba.Id.Value,
            FechaSalida: DateTime.Now,
            PesoPromedioFinal: 85.5m,
            CantidadMuertos: 2,
            CantidadVivos: 10
        );

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<CebaId>(), CancellationToken.None))
            .ReturnsAsync(fakeCeba);

        _mockEspacioFisicoRepository
            .Setup(r => r.ObtenerSegunId(fakeCeba.EspacioFisicoId, CancellationToken.None))
            .ReturnsAsync((EspacioFisico?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(EspacioFisicoErrores.EspacioFisicoNoExistente, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldRegisterExit_WhenAllConditionsMet()
    {
        // Arrange
        var fakeCeba = new FakeCeba(cantidadInicial: 15);
        var fakeEspacio = EspacioFisico.Create(
            GranjaId.New(),
            "Ceba",
            cantidadEspacios: 1,
            capacidadPorEspacio: 20
        );
        fakeEspacio.IncrementarCapacidadOcupada(15); // Ocupamos 15 espacios

        var command = new RegistrarSalidaCebaCommand(
            CebaId: fakeCeba.Id.Value,
            FechaSalida: DateTime.Now,
            PesoPromedioFinal: 85.5m,
            CantidadMuertos: 2,
            CantidadVivos: 10
        );

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<CebaId>(), CancellationToken.None))
            .ReturnsAsync(fakeCeba);

        _mockEspacioFisicoRepository
            .Setup(r => r.ObtenerSegunId(fakeCeba.EspacioFisicoId, CancellationToken.None))
            .ReturnsAsync(fakeEspacio);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        // Verifica que se registró la salida en la ceba
        Assert.True(fakeCeba.SalidaRegistrada);
        Assert.Equal(command.PesoPromedioFinal, fakeCeba.PesoPromedioFinal);
        Assert.Equal(command.CantidadMuertos, fakeCeba.CantidadMuertos);

        // Verifica que se liberó el espacio correctamente (12 = 10 vivos + 2 muertos)
        Assert.Equal(3, fakeEspacio.Capacidad.CapacidadOcupada); // 15 inicial - 12 liberados

        // Verifica que se creó la salida de ceba
        _mockAnimalesRepository.Verify(
            r => r.AgregarSalidaCeba(It.Is<SalidaCeba>(s =>
                s.CebaId.Value == fakeCeba.Id.Value &&
                s.CantidadVivos == command.CantidadVivos
            )),
            Times.Once
        );

        // Verifica persistencia
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Theory]
    [InlineData(-1, 10)]  // Muertos negativos
    [InlineData(2, -1)]   // Vivos negativos
    public async Task Handle_ShouldFail_WhenInvalidQuantities(int muertos, int vivos)
    {
        // Arrange
        var fakeCeba = new FakeCeba();
        var fakeEspacio = EspacioFisico.Create(GranjaId.New(), "Ceba", 20, 1);

        var command = new RegistrarSalidaCebaCommand(
            CebaId: fakeCeba.Id.Value,
            FechaSalida: DateTime.Now,
            PesoPromedioFinal: 85.5m,
            CantidadMuertos: muertos,
            CantidadVivos: vivos
        );

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<CebaId>(), CancellationToken.None))
            .ReturnsAsync(fakeCeba);

        _mockEspacioFisicoRepository
            .Setup(r => r.ObtenerSegunId(fakeCeba.EspacioFisicoId, CancellationToken.None))
            .ReturnsAsync(fakeEspacio);

        await Assert.ThrowsAsync<ArgumentException>(() =>
          _handler.Handle(command, CancellationToken.None)
      );
    }

    [Fact]
    public async Task Handle_ShouldPropagateException_WhenSaveFails()
    {
        // Arrange
        var fakeCeba = new FakeCeba();
        var fakeEspacio = EspacioFisico.Create(GranjaId.New(), "Ceba", 20, 1);

        var command = new RegistrarSalidaCebaCommand(
            CebaId: fakeCeba.Id.Value,
            FechaSalida: DateTime.Now,
            PesoPromedioFinal: 85.5m,
            CantidadMuertos: 2,
            CantidadVivos: 10
        );

        _mockAnimalesRepository
            .Setup(r => r.ObtenerSegunId(It.IsAny<CebaId>(), CancellationToken.None))
            .ReturnsAsync(fakeCeba);

        _mockEspacioFisicoRepository
            .Setup(r => r.ObtenerSegunId(fakeCeba.EspacioFisicoId, CancellationToken.None))
            .ReturnsAsync(fakeEspacio);

        _mockUnitOfWork
            .Setup(u => u.SaveChangesAsync(CancellationToken.None))
            .ThrowsAsync(new InvalidOperationException("DB Error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None)
        );
    }
}
