using Core;
using Test.Commons;

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
        public void Store_WhenEventIsNew_Succeeds()
        {
            var result = _sut.Store(new GenericEvent(1));
            Assert.True(result.Success);
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
    }
}