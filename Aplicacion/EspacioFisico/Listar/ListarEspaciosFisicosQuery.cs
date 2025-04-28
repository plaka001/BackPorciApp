using Application.Abstractions.Messaging;

namespace Aplicacion.EspacioFisico.Calcular;

public sealed record ListarEspaciosFisicosQuery(Guid GranjaId) : IQuery<List<ListarEspacioFisicoResponse>>;
