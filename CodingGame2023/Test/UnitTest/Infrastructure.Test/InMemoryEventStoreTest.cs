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
        public async Task Store_WhenEventIsNewAndVersionIsOne_Success()
        {
            var result = await _sut.StoreAsync(new SampleEvent(0));
            Assert.True(result.Success);
        }

        [Fact]
        public async Task Store_WhenEventIsNewAndVersionIsNotOne_Failure()
        {
            var result = await _sut.StoreAsync(new SampleEvent(2));
            Assert.False(result.Success);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        public async Task Store_WhenEventIsFoundAndVersionDoesNotMatch_Failure(int version)
        {
            Key id = new();
            var _ = await _sut.StoreAsync(new SampleEvent(id, 0));
            var result = await _sut.StoreAsync(new SampleEvent(id, version));
            Assert.Single(await _sut.GetEventsAsync(id));
            Assert.False(result.Success);
        }

        [Fact]
        public async Task Store_WhenEventIsFoundAndVersionDoMatch_Success()
        {
            Key id = new();
            var _ = await _sut.StoreAsync(new SampleEvent(id, 0));
            var result = await _sut.StoreAsync(new SampleEvent(id, 1));
            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetEvents_WhenEventIsFound_ReturnsList()
        {
            Key id = new();
            var _ = await _sut.StoreAsync(new SampleEvent(id, 0));
            var __ = await _sut.StoreAsync(new SampleEvent(id, 1));
            var result = await _sut.GetEventsAsync(id);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetEvents_WhenEventIdIsNullEvenIfEventsReStored_ReturnsList()
        {
            Key id = new();
            var _ = await _sut.StoreAsync(new SampleEvent(id, 0));
            var __ = await _sut.StoreAsync(new SampleEvent(id, 1));
            var result = await _sut.GetEventsAsync(null!);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetEvents_WhenEventIsNotFound_ReturnsEmptyList()
        {
            Key id = new();
            var result = await _sut.GetEventsAsync(id);
            Assert.Empty(result);
        }
    }
}