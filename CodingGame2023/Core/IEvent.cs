namespace Core
{
    public interface IEvent
    {
        public string Name { get; }
        public Key Id { get; }
        public int Version { get; }
    }
}