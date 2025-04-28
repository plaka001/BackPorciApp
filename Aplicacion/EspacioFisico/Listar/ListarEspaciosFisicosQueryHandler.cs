using Aplicacion.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Dominio.Abstractions;

namespace Aplicacion.EspacioFisico.Calcular;

internal class ListarEspaciosFisicosQueryHandler : IQueryHandler<ListarEspaciosFisicosQuery, List<ListarEspacioFisicoResponse>>
{
    private readonly ISqlConnectionFactory _sqlConectionFactory;

    public ListarEspaciosFisicosQueryHandler(ISqlConnectionFactory sqlConectionFactory)
    {
        _sqlConectionFactory = sqlConectionFactory;
    }

    public async Task<Result<List<ListarEspacioFisicoResponse>>> Handle(ListarEspaciosFisicosQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConectionFactory.CreateConnection();

        const string sql = """
            SELECT
                Id,
                GranjaId,
                TipoEspacio,
                CantidadEspacios,
                CapacidadPorEspacio,
                CapacidadRecomendada,
                CapacidadMaxima,
                CapacidadOcupada,
                FechaCreacion
            FROM EspacioFisico
            WHERE GranjaId = @GranjaId
        """;

        var espacios = await connection.QueryAsync<ListarEspacioFisicoResponse>(
            sql,
            new { GranjaId = request.GranjaId }
        );

        return espacios.ToList();
    }
}
