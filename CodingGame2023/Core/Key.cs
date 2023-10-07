﻿namespace Core
{
    public class Key
    {
        public string Value { get; }
        public Key()
        {
             Value = Guid.NewGuid().ToString();
        }
        
        public Key(string value)
        {
             Value = value;
        }
    }
}