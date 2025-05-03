using Dominio.Abstractions;
using Dominio.Animales.ObjectValues;
using Dominio.EspacioFisicos.ObjectValues;
using System;

namespace Dominio.Animales.Lechones
{
    public class Precebo : Entity<PreceboId>
    {
        public DesteteId DesteteId { get; private set; }
        public DateTime FechaIngreso { get; private set; }
        public int CantidadInicial { get; private set; }
        public decimal PesoPromedioInicial { get; private set; }
        public EspacioFisicoId EspacioFisicoId { get; private set; }
        public string Comentario { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        public int CantidadMuertos { get; private set; }
        public decimal? PesoPromedioFinal { get; private set; }
        public DateTime? FechaSalida { get; private set; }

        protected Precebo(
            PreceboId id,
            DesteteId desteteId,
            DateTime fechaIngreso,
            int cantidadInicial,
            decimal pesoPromedioInicial,
            EspacioFisicoId espacioFisicoId,
            string comentario,
            int cantidadMuertos = 0,
            decimal? pesoPromedioFinal = null,
            DateTime? fechaSalida = null) : base(id)
        {
            DesteteId = desteteId;
            FechaIngreso = fechaIngreso;
            CantidadInicial = cantidadInicial;
            PesoPromedioInicial = pesoPromedioInicial;
            EspacioFisicoId = espacioFisicoId;
            Comentario = comentario;
            FechaCreacion = DateTime.UtcNow;
            CantidadMuertos = cantidadMuertos;
            PesoPromedioFinal = pesoPromedioFinal;
            FechaSalida = fechaSalida;
        }

        public static Precebo Create(
            DesteteId desteteId,
            DateTime fechaIngreso,
            int cantidadInicial,
            decimal pesoPromedioInicial,
            EspacioFisicoId espacioFisicoId,
            string comentario = null)
        {
            // Validaciones
            if (desteteId == null)
                throw new ArgumentException("El ID del destete es requerido");

            if (fechaIngreso > DateTime.UtcNow)
                throw new ArgumentException("La fecha de ingreso no puede ser futura");

            if (cantidadInicial <= 0)
                throw new ArgumentException("La cantidad inicial debe ser mayor a 0");

            if (pesoPromedioInicial <= 0)
                throw new ArgumentException("El peso promedio inicial debe ser mayor a 0");

            if (espacioFisicoId == null)
                throw new ArgumentException("El ID del espacio físico es requerido");

            return new Precebo(
                PreceboId.New(),
                desteteId,
                fechaIngreso,
                cantidadInicial,
                pesoPromedioInicial,
                espacioFisicoId,
                comentario);
        }

        // Método para registrar salida de precebo
        public virtual void RegistrarSalida(decimal pesoPromedioFinal, DateTime fechaSalida, int cantidadMuertos = 0)
        {
            if (fechaSalida < FechaIngreso)
                throw new ArgumentException("La fecha de salida no puede ser anterior a la fecha de ingreso");

            if (fechaSalida > DateTime.UtcNow)
                throw new ArgumentException("La fecha de salida no puede ser futura");

            if (pesoPromedioFinal <= 0)
                throw new ArgumentException("El peso promedio final debe ser mayor a 0");

            if (cantidadMuertos < 0 || cantidadMuertos > CantidadInicial)
                throw new ArgumentException("La cantidad de muertos no es válida");

            PesoPromedioFinal = pesoPromedioFinal;
            FechaSalida = fechaSalida;
            CantidadMuertos = cantidadMuertos;
        }
    }
}