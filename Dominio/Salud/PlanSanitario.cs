using Dominio.Abstractions;
using Dominio.granjas.ObjectValues;
using Dominio.Salud.Enums;
using Dominio.Salud.ObjectValues;
using System;
using System.Collections.Generic;

namespace Dominio.Salud
{
    public class PlanSanitario : Entity<PlanSanitarioId>
    {
        public GranjaId GranjaId { get; private set; }
        public string Nombre { get; private set; }
        public TipoAplicacionSanitaria TipoAplicacion { get; private set; }
        public DateTime FechaCreacion { get; private set; }

        // Eventos programados específicos (relación 1 a muchos)
        private readonly List<EventoSanitarioProgramado> _eventos = new();
        public IReadOnlyCollection<EventoSanitarioProgramado> Eventos => _eventos.AsReadOnly();

        // Eventos del plan (plantilla de eventos, relación 1 a muchos)
        private readonly List<EventoSanitarioPlan> _eventosPlan = new();
        public IReadOnlyCollection<EventoSanitarioPlan> EventosPlan => _eventosPlan.AsReadOnly();

        protected PlanSanitario(
            PlanSanitarioId id,
            GranjaId granjaId,
            string nombre,
            TipoAplicacionSanitaria tipoAplicacion) : base(id)
        {
            GranjaId = granjaId;
            Nombre = nombre;
            TipoAplicacion = tipoAplicacion;
            FechaCreacion = DateTime.UtcNow;
        }

        public static PlanSanitario Create(
            GranjaId granjaId,
            string nombre,
            TipoAplicacionSanitaria tipoAplicacion)
        {
            // Validaciones (se mantienen igual)
            if (granjaId == null)
                throw new ArgumentException("El ID de la granja es requerido");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del plan sanitario es requerido");

            if (!Enum.IsDefined(typeof(TipoAplicacionSanitaria), tipoAplicacion))
                throw new ArgumentException("Tipo de aplicación no válido");

            return new PlanSanitario(
                PlanSanitarioId.New(),
                granjaId,
                nombre,
                tipoAplicacion);
        }

        public void AgregarEvento(
            int tipoEntidadId,
            Guid entidadId,
            string nombreEvento,
            DateTime fechaProgramada,
            string comentario = null)
        {
            var evento = EventoSanitarioProgramado.Create(
                tipoEntidadId,
                entidadId,
                Id,
                nombreEvento,
                fechaProgramada,
                comentario);

            _eventos.Add(evento);
        }

        public void AgregarEventoPlan(
            int diaDesdeAsignacion,
            string nombreEvento,
            string comentario = null)
        {
            var evento = EventoSanitarioPlan.Create(
                Id,
                diaDesdeAsignacion,
                nombreEvento,
                comentario);

            _eventosPlan.Add(evento);

            Console.WriteLine($"Evento creado: {evento.NombreEvento} - ID: {evento.Id.Value}");
        }

        public void EliminarEventoPlan(EventoSanitarioPlanId eventoId)
        {
            var evento = _eventosPlan.Find(e => e.Id == eventoId);
            if (evento != null)
            {
                _eventosPlan.Remove(evento);
            }
        }
    }
}