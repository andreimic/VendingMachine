using System;

namespace VendingMachine
{
    public class QuantityDictionary
    {
        private int[] _quantityKeys = { };
        private int[] _quantityValues = { };

        public void SetQuantity(int quantity, int value)
        {
            _quantityValues[Array.IndexOf(_quantityKeys, quantity)] = value;
        }

        public int GetQuantity(int value)
        {
            return _quantityValues[Array.IndexOf(_quantityKeys, value)];
        }

        public void Add(int c, int n)
        {
            Array.Resize(ref _quantityKeys, _quantityKeys.Length + 1);
            Array.Resize(ref _quantityValues, _quantityValues.Length + 1);
            _quantityKeys[_quantityKeys.Length - 1] = c;
            _quantityValues[_quantityValues.Length - 1] = n;
        }
    }
}