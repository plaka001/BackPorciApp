namespace Aplicacion.EspacioFisico.Calcular;

public sealed class ListarEspacioFisicoResponse
{
    public Guid Id { get; set; }
    public Guid GranjaId { get; set; }
    public string TipoEspacio { get; set; }
    public int CantidadEspacios { get; set; }
    public int CapacidadPorEspacio { get; set; }
    public int CapacidadRecomendada { get; set; }
    public int CapacidadMaxima { get; set; }
    public int CapacidadOcupada { get; set; }
    public DateTime FechaCreacion { get; set; }
}
