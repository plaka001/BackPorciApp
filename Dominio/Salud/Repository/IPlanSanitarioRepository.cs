using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Salud.Repository;

public interface IPlanSanitarioRepository
{
    Task<PlanSanitario?> ObtenerSegunNombre(string nonbre, CancellationToken cancellationToken);

    public void Agregar(PlanSanitario entity);

    public void Agregar(EventoSanitarioPlan entity);
}
