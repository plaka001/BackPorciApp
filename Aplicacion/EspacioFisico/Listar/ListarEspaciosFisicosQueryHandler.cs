using Aplicacion.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Dominio.Abstractions;

namespace Aplicacion.EspacioFisico.Calcular;

public class ListarEspaciosFisicosQueryHandler : IQueryHandler<ListarEspaciosFisicosQuery, List<ListarEspacioFisicoResponse>>
{
    private readonly ISqlConnectionFactory _sqlConectionFactory;
    private readonly IDapperWrapper _dapper;

    public ListarEspaciosFisicosQueryHandler(ISqlConnectionFactory sqlConectionFactory, IDapperWrapper dapper)
    {
        _sqlConectionFactory = sqlConectionFactory;
        _dapper = dapper;
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

        var espacios = await _dapper.QueryAsync<ListarEspacioFisicoResponse>(
            connection,
            sql,
            new { GranjaId = request.GranjaId }
        );

        return espacios.ToList();
    }
}
