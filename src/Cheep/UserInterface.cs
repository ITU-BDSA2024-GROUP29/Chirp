using System.Text.RegularExpressions;
using ;
using static Chirp.Program;
using Cheep.Utils;

class UserInterface{

        /**
        *   Every printstatement function
        *   Regex matcher, Time formater and general printstatement.
        **/
        public static void PrintChirps(IEnumerable<Cheep> cheeps)
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


}
