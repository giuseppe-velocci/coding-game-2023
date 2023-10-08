using Core;

namespace Test.Commons
{
    public class GenericEvent : IEvent
    {
        public string Name => nameof(GenericEvent);

        public Key Id { get; } = new();

        public int Version { get; }

        public GenericEvent(int version)
        {
            Version = version;
        }

        public GenericEvent(Key id, int version)
        {
            Id = id;
            Version = version;
        }

        public IEvent UpdateVersion(int version)
        {
            return new GenericEvent(Id, version);
        }
    }
}
