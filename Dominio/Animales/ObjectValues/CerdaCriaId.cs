
namespace Dominio.Animales.ObjectValues;

public record CerdaCriaId(Guid Value)
{
    public static CerdaCriaId New() => new(Guid.NewGuid());
}