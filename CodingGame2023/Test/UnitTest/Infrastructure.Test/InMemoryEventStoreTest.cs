using Core;
using Test.Commons;
using Xunit;

namespace Infrastructure.Test
{
    public class InMemoryEventStoreTest
    {
        private readonly InMemoryEventStore<Item> _sut;

        public InMemoryEventStoreTest()
        {
            _sut = new ();
        }

        [Fact]
        public void Store_WhenEventIsNewAndVersionIsOne_Succeeds()
        {
            var result = _sut.Store(new GenericEvent(1));
            Assert.True(result.Success);
        }
        
        [Fact]
        public void Store_WhenEventIsNewAndVersionIsNotOne_Fails()
        {
            var result = _sut.Store(new GenericEvent(2));
            Assert.False(result.Success);
        }

        [Fact]
        public void Store_WhenEventIsFoundAndVersionDoesNotMatch_Fails()
        {
            Key id = new();
            var _ = _sut.Store(new GenericEvent(id, 1));
            var result = _sut.Store(new GenericEvent(id, 1));
            Assert.False(result.Success);
        }

        [Fact]
        public void Store_WhenEventIsFoundAndVersionDoMatch_Succeeds()
        {
            Key id = new();
            var _ = _sut.Store(new GenericEvent(id, 1));
            var result = _sut.Store(new GenericEvent(id, 2));
            Assert.True(result.Success);
        }

        [Fact]
        public void GetEvents_WhenEventIsFound_ReturnsList()
        {
            Key id = new();
            var _ = _sut.Store(new GenericEvent(id, 1));
            var __ = _sut.Store(new GenericEvent(id, 2));
            var result = _sut.GetEvents(id);
            Assert.Equal(2, result.Count);
        }
        
        [Fact]
        public void GetEvents_WhenEventIdIsNull_ReturnsList()
        {
            Key id = new();
            var _ = _sut.Store(new GenericEvent(id, 1));
            var __ = _sut.Store(new GenericEvent(id, 2));
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