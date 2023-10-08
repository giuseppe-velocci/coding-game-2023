using Core;

namespace Test.Commons
{
    public class SampleEvent : IEvent
    {
        public string Name => nameof(SampleEvent);

        public Key Id { get; } = new();

        public int Version { get; }

        public SampleEvent(int version)
        {
            Version = version;
        }

        public SampleEvent(Key id, int version)
        {
            Id = id;
            Version = version;
        }

        public IEvent UpdateVersion(int version)
        {
            return new SampleEvent(Id, version);
        }
    }
}
