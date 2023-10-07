using System;

namespace Core
{
    public class Key
    {
        public string Value { get; }
        public Key()
        {
             Value = Guid.NewGuid().ToString();
        }
    }
}