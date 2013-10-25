using ATM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AddTransaction
{
    static void Main()
    {
        using (ATMEntities context = new ATMEntities())
        {
            Add("999999", DateTime.Now, 100m, context);
        }
    }

    public static void Add(string number, DateTime date, decimal ammount, ATMEntities context)
    {
        CardHistoryLog log = new CardHistoryLog();
        log.CardNumber = number;
        log.OperationDate = date;
        log.amount = ammount;
        context.CardHistoryLogs.Add(log);
        context.SaveChanges();
    }
}