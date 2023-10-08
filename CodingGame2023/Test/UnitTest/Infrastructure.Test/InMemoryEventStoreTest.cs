using Core;
using Test.Commons;

namespace Infrastructure.Test
{
    public class InMemoryEventStoreTest
    {
        private readonly InMemoryEventStore<Item> _sut;

        public InMemoryEventStoreTest()
        {
            _sut = new();
        }

        [Fact]
        public void Store_WhenEventIsNewAndVersionIsOne_Success()
        {
            var result = _sut.Store(new SampleEvent(0));
            Assert.True(result.Success);
        }

        [Fact]
        public void Store_WhenEventIsNewAndVersionIsNotOne_Failure()
        {
            var result = _sut.Store(new SampleEvent(2));
            Assert.False(result.Success);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        public void Store_WhenEventIsFoundAndVersionDoesNotMatch_Failure(int version)
        {
            Key id = new();
            var _ = _sut.Store(new SampleEvent(id, 0));
            var result = _sut.Store(new SampleEvent(id, version));
            Assert.Single(_sut.GetEvents(id));
            Assert.False(result.Success);
        }

        [Fact]
        public void Store_WhenEventIsFoundAndVersionDoMatch_Success()
        {
            Key id = new();
            var _ = _sut.Store(new SampleEvent(id, 0));
            var result = _sut.Store(new SampleEvent(id, 1));
            Assert.True(result.Success);
        }

        [Fact]
        public void GetEvents_WhenEventIsFound_ReturnsList()
        {
            Key id = new();
            var _ = _sut.Store(new SampleEvent(id, 0));
            var __ = _sut.Store(new SampleEvent(id, 1));
            var result = _sut.GetEvents(id);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetEvents_WhenEventIdIsNullEvenIfEventsReStored_ReturnsList()
        {
            Key id = new();
            var _ = _sut.Store(new SampleEvent(id, 0));
            var __ = _sut.Store(new SampleEvent(id, 1));
            var result = _sut.GetEvents(null!);
            Assert.Empty(result);
        }

        [Fact]
        public void GetEvents_WhenEventIsNotFound_ReturnsEmptyList()
        {
            Key id = new();
            var result = _sut.GetEvents(id);
            Assert.Empty(result);
        }
    }
}