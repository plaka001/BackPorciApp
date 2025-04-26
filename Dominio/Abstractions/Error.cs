namespace Dominio.Abstractions;

public class Error
{
    public string Code { get; }
    public string Message { get; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    // Método para formatear el mensaje con argumentos
    public Error WithParams(params object[] args)
    {
        return new Error(Code, string.Format(Message, args));
    }

    public static readonly Error None = new Error(string.Empty, string.Empty);
    public static readonly Error NullValue = new Error("Value.Null", "El valor no puede ser nulo.");
}
