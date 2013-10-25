using ATM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

public class RepeatableRead
{
    public static void Main()
    {
        Console.WriteLine("Enter card number");
        string number = Console.ReadLine();
        Console.WriteLine("Enter card PIN");
        string pin = Console.ReadLine();
        Console.WriteLine("Enter ammount");
        decimal amount = decimal.Parse(Console.ReadLine());
        Withdraw(number, pin, amount);
    }

    public static void Withdraw(string number, string pin, decimal amount)
    {
        TransactionOptions options = new TransactionOptions();
        options.IsolationLevel = IsolationLevel.RepeatableRead;
        TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options);
        using (scope)
        {
            ATMEntities context = new ATMEntities();
            using (context)
            {
                var account = context.CardAccounts.First(x => x.CardNumber == number && x.CardPIN == pin);
                if (account != null)
                {
                    if (amount <= account.CardCash && amount > 0)
                    {
                        account.CardCash -= amount;
                        AddTransaction.Add(number, DateTime.Now, amount, context);
                    }
                }

                context.SaveChanges();
                scope.Complete();
            }
        }
    }
}