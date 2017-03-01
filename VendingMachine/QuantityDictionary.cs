using System;
using System.Collections.Generic;

namespace VendingMachine
{
    public class QuantityDictionary
    {
        private readonly Dictionary<int, int> _quantities = new Dictionary<int, int>();

        public void SetQuantity(int quantity, int value)
        {
            _quantities[quantity] = value;
        }

        public int GetQuantity(int value)
        {
            return _quantities[value];
        }

        public void Add(int c, int n)
        {
            _quantities[c] = n;
        }
    }
}