namespace Aplicacion.Dtos;

public record class EventoSanitarioDto(
      int DiaDesdeAsignacion,
      string NombreEvento,
      string? Comentario
  );