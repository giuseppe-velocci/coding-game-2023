namespace Core
{
    public sealed record class OperationResult<T>
    {
        public T Value { get; private init; } = default!;
        public bool Success { get; private init; } = false;
        public string Message { get; private init; } = string.Empty;


        public static OperationResult<T> CreateSuccess(T value)
        {
            return new() { Success = true, Value = value };
        }

        public static OperationResult<T> CreateFailure(string message)
        {
            return new() { Message = message };
        }

        public static OperationResult<None> CreateSuccess()
        {
            return OperationResult<None>.CreateSuccess(new None());
        }
    }
}
