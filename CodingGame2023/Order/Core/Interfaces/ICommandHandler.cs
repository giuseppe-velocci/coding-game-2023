using Core;

namespace Order.Core.Interfaces
{
    public interface ICommandHandler<T> where T : class
    {
        Task<OperationResult<Key>> HandleAsync(ICommand command);
    }
}
