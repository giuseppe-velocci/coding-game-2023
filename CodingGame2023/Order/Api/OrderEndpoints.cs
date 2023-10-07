using Core;

namespace Order.Api
{
    public class OrderEndpoints
    {
        public Key GetOrder(Key id)
        {
            return id;
        }

        public Key CreateOrder()
        {
            return new Key();
        }
    }
}
