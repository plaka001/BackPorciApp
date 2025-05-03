using Aplicacion.Abstractions.Data;
using Aplicacion.EspacioFisico.Calcular;
using Dapper;
using Moq;
using System.Data;
using Xunit;

namespace Unit.Test.EspacioFisicoTest;

public class ListarEspaciosFisicosQueryHandlerTest
{
    private readonly Mock<ISqlConnectionFactory> _mockConnectionFactory;
    private readonly Mock<IDapperWrapper> _mockDapper;
    private readonly ListarEspaciosFisicosQueryHandler _handler;
    private readonly Mock<IDbConnection> _mockDbConnection;

    public ListarEspaciosFisicosQueryHandlerTest()
    {
        _mockConnectionFactory = new Mock<ISqlConnectionFactory>();
        _mockDapper = new Mock<IDapperWrapper>();
        _mockDbConnection = new Mock<IDbConnection>();

        _mockConnectionFactory
            .Setup(f => f.CreateConnection())
            .Returns(_mockDbConnection.Object);

        _handler = new ListarEspaciosFisicosQueryHandler(
            _mockConnectionFactory.Object,
            _mockDapper.Object
        );
    }


    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoEspacios()
    {
        // Arrange
        var granjaId = Guid.NewGuid();
        var query = new ListarEspaciosFisicosQuery(granjaId);

        _mockDapper
            .Setup(d => d.QueryAsync<ListarEspacioFisicoResponse>(
                _mockDbConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>()
            ))
            .ReturnsAsync(new List<ListarEspacioFisicoResponse>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfEspacios_WhenDataExists()
    {
        // Arrange
        var granjaId = Guid.NewGuid();
        var fakeEspacios = new List<ListarEspacioFisicoResponse>
        {
            new()
            {
                Id = Guid.NewGuid(),
                GranjaId = granjaId,
                TipoEspacio = "Paridera",
                CantidadEspacios = 3,
                CapacidadPorEspacio = 5,
                CapacidadRecomendada = 15,
                CapacidadMaxima = 20,
                CapacidadOcupada = 10,
                FechaCreacion = DateTime.UtcNow
            }
        };

        var query = new ListarEspaciosFisicosQuery(granjaId);

        _mockDapper
            .Setup(d => d.QueryAsync<ListarEspacioFisicoResponse>(
                _mockDbConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>() // <-- aquí ya no uses lambda
            ))
            .ReturnsAsync(fakeEspacios);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);
        Assert.Equal("Paridera", result.Value.First().TipoEspacio);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenConnectionFails()
    {
        // Arrange
        var granjaId = Guid.NewGuid();
        var query = new ListarEspaciosFisicosQuery(granjaId);

        _mockConnectionFactory
            .Setup(f => f.CreateConnection())
            .Throws(new InvalidOperationException("Connection failed"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(query, CancellationToken.None)
        );
    }
}
