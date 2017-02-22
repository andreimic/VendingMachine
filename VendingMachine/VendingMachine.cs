using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using CreditCardModule;

namespace VendingMachine
{
    public class VendingMachine
    {
        private List<int> _choices = new List<int>();
        private double _total;
        private double _colaPrice;
        private Dictionary<int, double> _prices = new Dictionary<int, double>();
        private CreditCard _creditCard;
        private bool _valid;
        private int _choiceForCard;
        private readonly Dictionary<int, int> _choiceDictionary;

        public double Total { get { return _total; } }

        public VendingMachine()
        {
            _choiceDictionary = new Dictionary<int, int>();
        }

        public Can Deliver(int quantityKey)
        {
            var price = _prices.ContainsKey(quantityKey) ? _prices[quantityKey] : 0;
            if (!_choices.Contains(quantityKey) || _choiceDictionary[quantityKey] < 1 || _total < price)
            {
                return null;
            }

            _choiceDictionary[quantityKey] = _choiceDictionary[quantityKey] - 1;
            _total -= price;
            return new Can { Type = quantityKey };
        }

        public void AddChoice(int c, int n = int.MaxValue)
        {
            _choiceDictionary.Add(c, n);
            _choices.Add(c);
        }

        public void AddMultipleChoices(int[] choices, int[] counts)
        {
            for (int i = 0; i < choices.Length; i++)
            {
                int c = choices[i];
                _choiceDictionary.Add(c, counts[i]);
                _choices.Add(c);
            }
        }

        public void AddCoin(int value)
        {
            _total += value;
        }

        public double GiveChange()
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
            _choiceDictionary.Add(choice, quantity);
            _choices.Add(choice);
            _prices[choice] = price;
        }



        public double GetPrice(int choice)
        {
            return _prices[choice];
        }

        public void AcceptCard(CreditCard creditCard)
        {
            _creditCard = creditCard;
        }

        public void GetPinNumber(int pinNumber)
        {
            _valid = new CreditCardModule.CreditCardModule(_creditCard).HasValidPinNumber(pinNumber);
        }

        public void SelectChoiceForCard(int choice)
        {
            _choiceForCard = choice;
        }

        public Can DeliverChoiceForCard()
        {
            if (_valid && _choices.IndexOf(_choiceForCard) > -1 && _choiceDictionary[_choiceForCard] > 0)
            {
                _choiceDictionary[_choiceForCard] = _choiceDictionary[_choiceForCard] - 1;

                return new Can { Type = _choiceForCard };
            }
            return null;
        }
    }

    public class Can
    {
        public int Type { get; set; }
    }
}