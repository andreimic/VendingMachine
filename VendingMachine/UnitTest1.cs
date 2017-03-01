using System;
using CreditCardModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VendingMachine
{
    [TestClass]
    public class VendingMachineTests
    {
        private int _colaChoice = 0;
        private int _fantaChoice = 1;

        [TestMethod]
        public void ShouldReturnNothingIfEmpty()
        {
            var vendingMachine = new VendingMachine();
            var result = vendingMachine.Deliver(_colaChoice);
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void ShouldReturnACan()
        {
            var vendingMachine = new VendingMachine();
            vendingMachine.AddChoice(_colaChoice);
            var can = vendingMachine.Deliver(_colaChoice);
            Assert.IsNotNull(can);
        }

        [TestMethod]
        public void ShouldReturnACanOfTheSelectedChoice()
        {
            var vendingMachine = new VendingMachine();
            vendingMachine.AddMultipleChoices(new int[] { _fantaChoice, _colaChoice }, new[] { int.MaxValue, int.MaxValue });
            var can = vendingMachine.Deliver(_fantaChoice);
            Assert.AreEqual(_fantaChoice, can.Type);
        }

        [TestMethod]
        public void ShouldSetTheInventoryCorrectly()
        {
            var vendingMachine = new VendingMachine();
            vendingMachine.AddChoice(_colaChoice, 1);
            vendingMachine.Deliver(_colaChoice);
            var can = vendingMachine.Deliver(_colaChoice);
            Assert.IsNull(can);
        }

        [TestMethod]
        public void ShouldHaveTotalZeroInitialy()
        {
            var vendingMachine = new VendingMachine();
            Assert.AreEqual(0, vendingMachine.Total);
        }

        [TestMethod]
        public void ShouldAcceptCoins()
        {
            var vendingMachine = new VendingMachine();
            vendingMachine.AddCoin(5);
            Assert.AreEqual(5, vendingMachine.Total);
        }

        [TestMethod]
        public void ShouldAddCoinsToTotal()
        {
            var vendingMachine = new VendingMachine();
            vendingMachine.AddCoin(5);
            vendingMachine.AddCoin(1);
            Assert.AreEqual(6, vendingMachine.Total);
        }

        [TestMethod]
        public void ShouldReturnChange()
        {
            var vendingMachine = new VendingMachine();
            vendingMachine.AddCoin(5);
            var change = vendingMachine.Change();
            Assert.AreEqual(5, change);
        }

        [TestMethod]
        public void ShouldSetBalanceToZero()
        {
            var vendingMachine = new VendingMachine();
            vendingMachine.AddCoin(5);
            vendingMachine.Change();
            Assert.AreEqual(0, vendingMachine.Total);
        }

        [TestMethod]
        public void ShouldAcceptCostForOption()
        {
            var vendingMachine = new VendingMachine();
            vendingMachine.Stock(_colaChoice, 5, 5.5);
            var colaPrice = vendingMachine.GetPrice(_colaChoice);
            Assert.AreEqual(5.5, colaPrice);
        }

        [TestMethod]
        public void ShouldAcceptDifferentCostsForOptions()
        {
            var vendingMachine = new VendingMachine();
            vendingMachine.Stock(_colaChoice, 5, 5.5);
            vendingMachine.Stock(_fantaChoice, 5, 2.5);
            var colaPrice = vendingMachine.GetPrice(_colaChoice);
            var fantaPrice = vendingMachine.GetPrice(_fantaChoice);
            Assert.AreEqual(5.5, colaPrice);
            Assert.AreEqual(2.5, fantaPrice);
        }

        [TestMethod]
        public void ShouldGiveProductIfPriceIsLessThanBalance()
        {
            var vendingMachine = new VendingMachine();
            vendingMachine.AddChoice(_colaChoice, 5);
            vendingMachine.AddPrice(_colaChoice, 3);
            vendingMachine.AddCoin(5);
            var colaCan = vendingMachine.Deliver(_colaChoice);
            Assert.IsNotNull(colaCan);
        }

        [TestMethod]
        public void ShouldSubstractPriceFromBalance()
        {
            var vendingMachine = new VendingMachine();
            vendingMachine.Stock(_colaChoice, 5, 3);
            vendingMachine.AddCoin(5);
            vendingMachine.Deliver(_colaChoice);
            Assert.AreEqual(2, vendingMachine.Total);
        }

        [TestMethod]
        public void ShouldNotReturnCanIfBalanceLowerThanCost()
        {
            var vendingMachine = new VendingMachine();
            vendingMachine.AddChoice(_colaChoice);
            vendingMachine.AddPrice(_colaChoice, 2.5);
            var can = vendingMachine.Deliver(_colaChoice);
            Assert.IsNull(can);
        }

        [TestMethod]
        public void ShouldAcceptCardAsPaymentMethod()
        {
            var vendingMachine = new VendingMachine();
            vendingMachine.AddChoice(_colaChoice);
            vendingMachine.AddPrice(_colaChoice, 0);
            vendingMachine.AcceptCard(new CreditCard("holder name", "serial number", "card company", 1234));
            vendingMachine.SelectChoiceForCard(_colaChoice);
            vendingMachine.GetPinNumber(1234);
            Can colaCan = vendingMachine.DeliverChoiceForCard();
            Assert.IsNotNull(colaCan);
        }

        [TestMethod]
        public void ShouldFailForInvalidPin()
        {
            var vendingMachine = new VendingMachine();
            vendingMachine.AddChoice(_colaChoice);
            vendingMachine.AddPrice(_colaChoice, 2.3);
            vendingMachine.AcceptCard(new CreditCard("holder name", "serial number", "card company", 1234));
            vendingMachine.SelectChoiceForCard(_colaChoice);
            vendingMachine.GetPinNumber(3456);
            Can colaCan = vendingMachine.DeliverChoiceForCard();
            Assert.IsNull(colaCan);
        }

        [TestMethod]
        public void ShouldReturnTheCorrectChoiceWhenPayingByCard()
        {
            var vendingMachine = new VendingMachine();
            vendingMachine.AddChoice(_colaChoice);
            vendingMachine.AddChoice(_fantaChoice);
            vendingMachine.AddPrice(_colaChoice, 2.3);
            vendingMachine.AcceptCard(new CreditCard("holder name", "serial number", "card company", 1234));
            vendingMachine.SelectChoiceForCard(_fantaChoice);
            vendingMachine.GetPinNumber(1234);
            Can colaCan = vendingMachine.DeliverChoiceForCard();
            Assert.AreEqual(_fantaChoice, colaCan.Type);
        }
    }
}
