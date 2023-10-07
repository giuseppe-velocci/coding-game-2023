using Core;

namespace Order.Core
{
    public interface ICommandHandler<T> where T : class
    {
        OperationResult<Key> Handle(ICommand command);
    }
}
