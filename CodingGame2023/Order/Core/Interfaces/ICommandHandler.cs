using Core;

namespace Order.Core.Interfaces
{
    public interface ICommandHandler<T> where T : class
    {
        OperationResult<Key> Handle(ICommand command);
    }
}
