using System.Text.RegularExpressions;
using SimpleDB;
using DocoptNet;

const string usage = @"Chirp CLI version.

Usage: 
    chirp read <limit>
    chirp cheep <message>
    chirp (-h | --help)
    chirp --version

Options:
  -h --help     Show this screen.
  --version     Show version.
";
var arguments = new Docopt().Apply(usage, args, version: "1.0", exit: true)!;

namespace Chirp
{
    class Program
    {

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
                    Console.WriteLine("Closing the application...");
                    Thread.Sleep(2000);
                    Environment.Exit(0);
                    break;

                case "clear":
                    Console.Write("Clearing consle");
                    for (int i = 0; i < 5; i++)
                    {
                        Thread.Sleep(200);
                        Console.Write(".");
                    }
                    Console.Clear();
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
                        Chirp(message, unixTime);
                    }
                    else
                    {
                        Console.WriteLine("Error: No message provided for 'chirp'.");
                    }
                    break;
                case "read":
                    PrintChirps();
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

        static void Chirp(string message, long unixTime)
        {
            // Write message with relevant information
            string formattedMessage = CreateMessage(unixTime, message);
            Console.WriteLine(formattedMessage);
            StoreChirp(message, unixTime);
        }

        // Creates the message in the correct format
        static string CreateMessage(long unixTime, string message)
        {
            // Base construction of message
            string formattedDate = ConvertUnixTimeToDate(unixTime);
            return $"{Environment.UserName} @ {formattedDate}: {message}";
        }

        static void StoreChirp(string message, long unixTime)
        {
            // Stores new messages in CSV file
            using (StreamWriter sw = File.AppendText("./Data/chirp_cli_db.csv"))
            {
                sw.WriteLine($"{Environment.UserName},\"{message}\",{unixTime}");
            }
        }


        /**
        *   Every printstatement function
        *   Regex matcher, Time formater and general printstatement.
        **/
        static void PrintChirps()
        {
            // Prints all chirps from the CSV file
            var chirps = File.ReadLines("./Data/chirp_cli_db.csv").Skip(1);
            foreach (var chirp in chirps)
            {
                if (string.IsNullOrEmpty(chirp))
                    continue;
                Console.WriteLine(FormatFromFileToConsole(chirp));
            }
        }

        static string FormatFromFileToConsole(string line)
        {
            var regex = new Regex("^(?<author>[\\w_-]+),\"(?<message>.+)\",(?<timeStamp>\\d+)$");
            var match = regex.Match(line);

            if (!match.Success)
            {
                throw new NoMatchRegexException("failiure to match regex of the input message");
            }

            string author = match.Groups["author"].Value;
            string message = match.Groups["message"].Value;
            string timestamp = ConvertUnixTimeToDate(long.Parse(match.Groups["timeStamp"].Value));

            return $"@{author} | {timestamp}: {message}";
        }

        static string ConvertUnixTimeToDate(long unixTime)
        {
            DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).LocalDateTime;
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
