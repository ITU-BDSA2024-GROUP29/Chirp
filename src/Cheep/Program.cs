using System.Text.RegularExpressions;
using SimpleDB;

namespace Chirp;
class Program
    {


        private static IDatabaseRepository<Cheep> Database = new CSVDatabase<Cheep>();

        static void Main(string[] args)
        {
            Console.WriteLine("Hi, welcome to Chirp!");
            Console.WriteLine("Available commands are <chirp, read, close>");

            // Ensure that the application continues running until the user exits
            while (true)
            {
                Console.Write("Enter a command: ");
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    var inputArgs = input.Split(' ', 2);
                    Start(inputArgs);
                }
            }
        }

        static void Start(string[] args)
        {
            if (args.Length == 0) return;

            long unixTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();

            switch (args[0].ToLower())
            {
                case "close":
                    UserInterface.close();
                    break;

                case "clear":
                    UserInterface.clear();
                    break;
                case "chirp":
                    if (args.Length > 1)
                    {
                        string message = args[1].Trim('"');
                        if (message.Length < 1)
                        {
                            Console.WriteLine("Message Cannot be empty");
                            return;
                        }
                        Chirp(new Cheep(Environment.UserName,message, unixTime));
                    }
                    else
                    {
                        Console.WriteLine("Error: No message provided for 'chirp'.");
                    }
                    break;
                case "read":
                    UserInterface.PrintChirps();
                    break;

                default:
                    Console.WriteLine("Unknown command. Please use 'chirp','read' or 'close'.");
                    break;
            }
        }

        /**
        *   Every metod used for Creating a chirp message
        *   Getting unixtime and enviorment.username
        *   and storing it in the CVS document
        **/

        static void Chirp(Cheep cheep)
        {
            // Write message with relevant information
            // string formattedMessage = CreateMessage(unixTime, message);
            UserInterface.PrintChirps();
            Database.Store(cheep);
            // StoreChirp(message, unixTime);
        }
    }
