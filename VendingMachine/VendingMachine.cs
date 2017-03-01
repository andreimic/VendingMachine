using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using CreditCardModule;

namespace VendingMachine
{
    public class VendingMachine
    {
        private List<int> _choices = new List<int>();
        private int[] _quantityKeys = { };
        private int[] _quantityValues = { };
        private double _total;
        private double _colaPrice;
        private Dictionary<int, double> _prices = new Dictionary<int, double>();
        private CreditCard _cc;
        private bool _valid;
        private int _selectedCard;

        public double T { get { return _total; } }

        public VendingMachine()
        {
        }

        public Can Deliver(int value)
        {
            var price = _prices.ContainsKey(value) ? _prices[value] : 0;
            if (!_choices.Contains(value) || GetQuantity(value) < 1 || _total < price)
            {
                return null;
            }

            DecrementQuantity(value);
            _total -= price;
            return new Can { Type = value };
        }

        private void DecrementQuantity(int value)
        {
            _quantityValues[Array.IndexOf(_quantityKeys, value)] = GetQuantity(value) - 1;
        }

        private int GetQuantity(int value)
        {
            return _quantityValues[Array.IndexOf(_quantityKeys, value)];
        }

        public void AddChoice(int c, int n = int.MaxValue)
        {
            Array.Resize(ref _quantityKeys, _quantityKeys.Length + 1);
            Array.Resize(ref _quantityValues, _quantityValues.Length + 1);
            _quantityKeys[_quantityKeys.Length - 1] = c;
            _quantityValues[_quantityValues.Length - 1] = n;
            _choices.Add(c);
        }

        public void AddMultipleChoices(int[] choices, int[] counts)
        {
            for (int i = 0; i < choices.Length; i++)
            {
                AddChoice(choices[i], counts[i]);
            }
        }

        public void AddCoin(int v)
        {
            _total += v;
        }

        public double Change()
        {
            var v = _total;
            _total = 0;
            return v;
        }

        public void AddPrice(int i, double v)
        {
            _prices[i] = v;
        }

        public void Stock(int choice, int quantity, double price)
        {
            AddChoice(choice, quantity);
            _prices[choice] = price;
        }


        public double GetPrice(int choice)
        {
            return _prices[choice];
        }

        public void AcceptCard(CreditCard myCC)
        {
            _cc = myCC;
        }

        public void GetPinNumber(int pinNumber)
        {
            _valid = new CreditCardModule.CreditCardModule(_cc).HasValidPinNumber(pinNumber);
        }

        public void SelectChoiceForCard(int choice)
        {
            _selectedCard = choice;
        }

        public Can DeliverChoiceForCard()
        {
            var card = _selectedCard;
            if (_valid && _choices.IndexOf(card) > -1 && _quantityValues[Array.IndexOf(_quantityKeys, card)] > 0)
            {
                _quantityValues[Array.IndexOf(_quantityKeys, card)] = _quantityValues[Array.IndexOf(_quantityKeys, card)] - 1;
                return new Can { Type = card };
            }
            else
            {
                return null;
            }
        }
    }

    public class Can
    {
        public int Type { get; set; }
    }
}