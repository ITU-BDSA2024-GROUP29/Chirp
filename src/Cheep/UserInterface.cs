using System.Text.RegularExpressions;

using static Chirp.Program;
using Chirp.ChirpUtils;
using Chirp;
using SimpleDB;


class UserInterface{

        /**
        *   Every printstatement function
        *   Regex matcher, Time formater and general printstatement.
        **/
        public static void PrintChirps()
        {
            // Prints all chirps from the CSV file
            //var chirps = File.ReadLines("./Data/chirp_cli_db.csv").Skip(1);
            var chirps = CSVDatabase<Cheep>.GetDatabase().Read();

            foreach (var chirp in chirps)
            {
                if (string.IsNullOrEmpty(chirp.ToString()))
                    continue;
                Console.WriteLine(chirp.author + " " + chirp.message + " " + Utils.ConvertUnixTimeToDate(chirp.Timestamp));
            }
        }


        public static void close(){
            Console.WriteLine("Closing the application...");
            Thread.Sleep(2000);
            Environment.Exit(0);
        }

        public static void clear(){
            Console.Write("Clearing consle");
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(200);
                Console.Write(".");
            }
            Console.Clear();
        }
        
        public static void Chirp(Cheep cheep)
        {
            // Write message with relevant information
            // string formattedMessage = CreateMessage(unixTime, message);
            UserInterface.PrintChirps();
            CSVDatabase<Cheep>.GetDatabase().Store(cheep);
            // StoreChirp(message, unixTime);
        }


}
