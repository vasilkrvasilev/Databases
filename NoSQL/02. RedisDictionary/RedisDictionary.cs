using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class RedisDictionary
{
    private static readonly string add = "Add";
    private static readonly string change = "Change";
    private static readonly string find = "Find";
    private static readonly string exit = "Exit";
    private static readonly string dictionary = "Dictionary";

    static void Main()
    {
        RedisClient client = new RedisClient("127.0.0.1:6379");
        using (client)
        {
            Console.WriteLine("Enter command - Add, Change, Find or Exit");
            string command = Console.ReadLine();
            while (command != exit)
            {
                if (command == add)
                {
                    AddToDictionary(client);
                }
                else if (command == change)
                {
                    ChangeTranslation(client);
                }
                else if (command == find)
                {
                    ShowTranslation(client);
                }
                else if (command != exit)
                {
                    Console.WriteLine("Enter invalid command. Please try again.");
                }

                Console.WriteLine("Enter command - Add, Change, Find or Exit");
                command = Console.ReadLine();
            }
        }
    }

    private static void ShowTranslation(RedisClient client)
    {
        Console.WriteLine("Enter word");
        string word = Console.ReadLine();
        byte[] wordInBytes = Extensions.ToAsciiCharArray(word);
        if (!string.IsNullOrWhiteSpace(word))
        {
            if (client.HExists(dictionary, wordInBytes) == 1)
            {
                byte[] translation = client.HGet(dictionary, wordInBytes);
                Console.WriteLine("Translation of the word {0} is:", word);
                Console.WriteLine(Extensions.StringFromByteArray(translation));
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

    private static void ChangeTranslation(RedisClient client)
    {
        Console.WriteLine("Enter word.");
        string word = Console.ReadLine();
        byte[] wordInBytes = Extensions.ToAsciiCharArray(word);
        Console.WriteLine("Enter translation.");
        string translation = Console.ReadLine();
        byte[] translationInBytes = Extensions.ToAsciiCharArray(translation);
        if (!string.IsNullOrWhiteSpace(word) && !string.IsNullOrWhiteSpace(translation))
        {
            if (client.HExists(dictionary, wordInBytes) == 1)
            {
                client.HSet(dictionary, wordInBytes, translationInBytes);
                Console.WriteLine("Translation of the word {0} is changed.", word);
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

    private static void AddToDictionary(RedisClient client)
    {
        Console.WriteLine("Enter word.");
        string word = Console.ReadLine();
        byte[] wordInBytes = Extensions.ToAsciiCharArray(word);
        Console.WriteLine("Enter translation.");
        string translation = Console.ReadLine();
        byte[] translationInBytes = Extensions.ToAsciiCharArray(translation);
        if (!string.IsNullOrWhiteSpace(word) && !string.IsNullOrWhiteSpace(translation))
        {
            if (client.HExists(dictionary, wordInBytes) == 0)
            {
                client.HSet(dictionary, wordInBytes, translationInBytes);
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