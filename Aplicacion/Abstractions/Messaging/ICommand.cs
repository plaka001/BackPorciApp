

using Dominio.Abstractions;
using MediatR;

namespace Aplicacion.Abstractions.Messaging;

public interface ICommand:IRequest<Result>,IBaseCommand
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
{

}

public interface IBaseCommand
{ }
