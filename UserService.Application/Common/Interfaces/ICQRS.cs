using MediatR;

namespace UserService.Application.Common.Interfaces
{
    public interface ICommand : IRequest { }
    public interface ICommand<TResponse> : IRequest<TResponse> { }

    public interface IQuery<TResponse> : IRequest<TResponse> { }

    public interface IDomainEvent : INotification { }

}
