using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class MongoDbDictionary
{
    private static readonly string add = "Add";
    private static readonly string change = "Change";
    private static readonly string find = "Find";
    private static readonly string exit = "Exit";

    static void Main()
    {
        var client = new MongoClient("mongodb://localhost/");
        var server = client.GetServer();
        var dictionaryDb = server.GetDatabase("Dictionary");
        var english = dictionaryDb.GetCollection("English");

        Console.WriteLine("Enter command - Add, Change, Find or Exit");
        string command = Console.ReadLine();
        while (command != exit)
        {
            if (command == add)
            {
                AddToDictionary(english);
            }
            else if (command == change)
            {
                ChangeTranslation(english);
            }
            else if (command == find)
            {
                ShowTranslation(english);
            }
            else if (command != exit)
            {
                Console.WriteLine("Enter invalid command. Please try again.");
            }

            Console.WriteLine("Enter command - Add, Change, Find or Exit");
            command = Console.ReadLine();
        }
    }

    private static void ShowTranslation(MongoCollection english)
    {
        Console.WriteLine("Enter word");
        string word = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(word))
        {
            var query = Query.And(Query.EQ("Name", word));
            var filtered = english.FindAs<Word>(query);
            var count = filtered.Count();
            if (count > 0)
            {
                Console.WriteLine("Translation of the word {0} is:", filtered.First().Name);
                Console.WriteLine(filtered.First().Translation);
            }
            else
            {
                Console.WriteLine("There is no word {0}.", word);
            }
        }
        else
        {
            Console.WriteLine("You enter null or empty string for word. Please try again.");
        }
    }

    private static void ChangeTranslation(MongoCollection english)
    {
        Console.WriteLine("Enter word.");
        string word = Console.ReadLine();
        Console.WriteLine("Enter translation.");
        string translation = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(word) && !string.IsNullOrWhiteSpace(translation))
        {
            var query = Query.And(Query.EQ("Name", word));
            var filtered = english.FindAs<Word>(query);
            var count = filtered.Count();
            if (count > 0)
            {
                foreach (var item in filtered)
                {
                    var update = Update.Set("Translation", translation);
                    var queryId = Query.EQ("_id", item.Id);
                    english.Update(queryId, update);
                    Console.WriteLine("Translation of the word {0} is changed.", word);
                }
            }
            else
            {
                Console.WriteLine("There is no word {0}.", word);
            }
        }
        else
        {
            Console.WriteLine("You enter null or empty string for word or translation. Please try again.");
        }
    }

    private static void AddToDictionary(MongoCollection english)
    {
        Console.WriteLine("Enter word.");
        string word = Console.ReadLine();
        Console.WriteLine("Enter translation.");
        string translation = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(word) && !string.IsNullOrWhiteSpace(translation))
        {
            var query = Query.And(Query.EQ("Name", word));
            var filtered = english.FindAs<Word>(query);
            var count = filtered.Count();
            if (count == 0)
            {
                english.Insert(new Word(word, translation));
                Console.WriteLine("The word {0} is added.", word);
            }
            else
            {
                Console.WriteLine("The word {0} is already added.", word);
            }
        }
        else
        {
            Console.WriteLine("You enter null or empty string for word or translation. Please try again.");
        }
    }
}