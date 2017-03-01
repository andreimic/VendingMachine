using System;
using System.Collections.Generic;
using CreditCardModule;

namespace VendingMachine
{
    public class VendingMachine
    {
        private readonly List<int> _choices = new List<int>();
        private readonly Dictionary<int, double> _prices = new Dictionary<int, double>();
        private CreditCard _creditCard;
        private bool _valid;
        private int _selectedCard;
        private readonly QuantityDictionary _quantityDictionary = new QuantityDictionary();

        public double Total { get; private set; }

        public Item Deliver(int itemType)
        {
            var price = _prices.ContainsKey(itemType) ? _prices[itemType] : 0;
            if (!_choices.Contains(itemType) || _quantityDictionary.GetQuantity(itemType) < 1 || Total < price)
            {
                return null;
            }

            _quantityDictionary.SetQuantity(itemType, _quantityDictionary.GetQuantity(itemType) - 1);
            Total -= price;
            return new Item { Type = itemType };
        }

        public void AddChoice(int c, int n = int.MaxValue)
        {
            _quantityDictionary.Add(c, n);
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
            Total += v;
        }

        public double Change()
        {
            var v = Total;
            Total = 0;
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

        public void AcceptCard(CreditCard card)
        {
            _creditCard = card;
        }

        public void GetPinNumber(int pinNumber)
        {
            _valid = new CreditCardModule.CreditCardModule(_creditCard).HasValidPinNumber(pinNumber);
        }

        public void SelectChoiceForCard(int choice)
        {
            _selectedCard = choice;
        }

        public Item DeliverChoiceForCard()
        {
            if (_valid && _choices.IndexOf(_selectedCard) > -1 && _quantityDictionary.GetQuantity(_selectedCard) > 0)
            {
                _quantityDictionary.SetQuantity(_selectedCard, _quantityDictionary.GetQuantity(_selectedCard) - 1);
                return new Item { Type = _selectedCard };
            }
            return null;
        }
    }

    public class Item
    {
        public int Type { get; set; }
    }
}