using Aplicacion.EspacioFisico.Crear;
using Application.Abstractions.Messaging;
using Dominio.Abstractions;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.EspacioFisicos.Repository;
using Dominio.granjas.ObjectValues;
using Dominio.granjas.repository;
using Dominio.Granjas;

namespace Aplicacion.EspacioFisicos.Crear;

public sealed class CrearEspacioFisicoCommandHandler : ICommandHandler<CrearEspacioFisicoCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEspacioFisicoRepository _repository;
    private readonly IGranjaRepository _granjaRepository;

    public CrearEspacioFisicoCommandHandler(IUnitOfWork unitOfWork, IEspacioFisicoRepository repository, IGranjaRepository granjaRepository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _granjaRepository = granjaRepository;
    }

    public async Task<Result> Handle(CrearEspacioFisicoCommand request, CancellationToken cancellationToken)
    {

        //Obtiene la granja
        var granja = await _granjaRepository.ObtenerSegunId(new GranjaId(request.GranjaId), cancellationToken);
        if (granja == null) return Result.Failure(GranjaErrores.GranjaNoExistente);

        //Obtiene parametros ya calculados
        var parametrosCalculados = await _granjaRepository.obtenerParametrsCalculados(new GranjaId(request.GranjaId));
        if (parametrosCalculados == null) return Result.Failure(ParametrosCalculadosErrores.ParametrosCalculadosNoExisten);


        var capacidadRecomendada = Calcular(parametrosCalculados!, request.TipoEspacio);


        var espacioFisico = Dominio.EspacioFisicos.EspacioFisico.Create(new GranjaId(request.GranjaId), request.TipoEspacio.ToString(), request.CantidadEspacios, request.CapacidadPorEspacio, capacidadRecomendada);

        _repository.Agregar(espacioFisico);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }


    private static readonly Dictionary<TipoEspacio, Func<ParametrosCalculados, int>> _mapaCapacidad = new()
    {
        { TipoEspacio.Monta, x => x.EspaciosMontas },
        { TipoEspacio.Gestacion, x => x.EspaciosGestacion },
        { TipoEspacio.Parideras, x => x.EspaciosParideras },
        { TipoEspacio.Precebo, x => x.EspaciosPrecebo },
        { TipoEspacio.Ceba, x => x.EspaciosCeba }
    };

    public static int Calcular(ParametrosCalculados parametrosCalculados, TipoEspacio tipoEspacio)
    {
        if (!_mapaCapacidad.TryGetValue(tipoEspacio, out var selector))
        {
            throw new ArgumentException($"Tipo de espacio no soportado: {tipoEspacio}");
        }

        return selector(parametrosCalculados);
    }
}
