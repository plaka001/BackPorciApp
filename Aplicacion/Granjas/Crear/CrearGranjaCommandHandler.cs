using Application.Abstractions.Messaging;
using Domain.Granjas;
using Dominio.Abstractions;
using Dominio.granjas.repository;
using Dominio.Granjas;

namespace Aplicacion.Granjas.CrearGranja;

public sealed class CrearGranjaCommandHandler : ICommandHandler<CrearGranjaCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGranjaRepository _repository;

    public CrearGranjaCommandHandler(IUnitOfWork unitOfWork, IGranjaRepository repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task<Result> Handle(CrearGranjaCommand request, CancellationToken cancellationToken)
    {

        var granjaExistente = await _repository.ObtenerGranjaByNombre(request.Nombre);
        if (granjaExistente != null) return Result.Failure(GranjaErrores.GranjaNoExistente);

        var granja = Granja.Create(request.Nombre, request.Ubicacion!, request.NumeroCerdasCria, request.FechaInicioOperaciones);
        _repository.Agregar(granja);

        // inserta parametros
        var parametros = CrearParametrosProduccion(granja);

        // calcula parametros productivos
        CalcularEspaciosInfraestructura(granja, parametros);

        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    private ParametrosProduccion CrearParametrosProduccion(Granja granja)
    {
        var parametros = ParametrosProduccion.Create(granja.Id!);
        _repository.AgregarParametrosProduccion(parametros);
        return parametros;
    }

    private void CalcularEspaciosInfraestructura(Granja granja, ParametrosProduccion parametros)
    {
        var parametrosCalculados = ParametrosCalculados.CalcularEspaciosInfraestructura(granja, parametros);
        _repository.AgregarParametrosProduccionCalculados(parametrosCalculados);
    }
}
