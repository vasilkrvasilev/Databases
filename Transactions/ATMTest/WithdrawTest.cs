using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ATM.Data;

namespace _04.ATMTests
{
    [TestClass]
    public class WithdrawTests
    {
        [TestMethod]
        public void WithdrawTo0()
        {
            decimal money = 1000m;
            string number = "1111111111";
            string cardPin = "1111";

            using (ATMEntities contextOut = new ATMEntities())
            {
                contextOut.CardAccounts.Add(new CardAccount() { CardCash = money, CardNumber = number, CardPIN = cardPin });
                contextOut.SaveChanges();
                RepeatableRead.Withdraw(number, cardPin, money);

                var actual = (from c in contextOut.CardAccounts
                              select c).First();

                Assert.AreEqual(0, actual.CardCash);
                contextOut.Dispose();
            }
        }

        [TestMethod]
        public void CheckRecordNumber()
        {
            decimal money = 2000m;
            decimal toWithdraw = 1000m;
            string number = "1111111111";
            string cardPin = "1111";

            using (ATMEntities contextOut = new ATMEntities())
            {
                contextOut.CardAccounts.Add(new CardAccount() 
                { CardCash = money, CardNumber = number, CardPIN = cardPin });

                contextOut.SaveChanges();

                RepeatableRead.Withdraw(number, cardPin, toWithdraw);

                var actual = (from c in contextOut.CardHistoryLogs
                              select c).First();

                Assert.AreEqual(number, actual.CardNumber);
                contextOut.Dispose();
            }
        }

        [TestMethod]
        public void CheckRecordAmount()
        {
            decimal money = 2000m;
            decimal toWithdraw = 1000m;
            string number = "1111111111";
            string cardPin = "1111";

            using (ATMEntities contextOut = new ATMEntities())
            {
                contextOut.CardAccounts.Add(new CardAccount() { CardCash = money, CardNumber = number, CardPIN = cardPin });

                contextOut.SaveChanges();

                RepeatableRead.Withdraw(number, cardPin, toWithdraw);

                var actual = (from c in contextOut.CardHistoryLogs
                              select c).First();

                Assert.AreEqual(1000, actual.amount);
                contextOut.Dispose();
            }
        }
    }
}