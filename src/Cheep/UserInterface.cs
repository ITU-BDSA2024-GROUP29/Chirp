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
        public static void PrintChirps(int limit,string FilePath)
        {
            // Prints all chirps from the CSV file
            //var chirps = File.ReadLines("./Data/chirp_cli_db.csv").Skip(1);
            var chirps = CSVDatabase<Cheep>.GetDatabase(FilePath).Read(limit);
            
            foreach (Cheep cheep in chirps)
            {
                if (string.IsNullOrEmpty(cheep.ToString()))
                    continue;
                Console.WriteLine(cheep.Author + "@ " + cheep.Message + ": " + Utils.ConvertUnixTimeToDate(cheep.TimeStamp));
            }
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
        
        public static void Chirp(Cheep cheep,string FilePath)
        {
            // Write message with relevant information
            // string formattedMessage = CreateMessage(unixTime, message);
            CSVDatabase<Cheep>.GetDatabase(FilePath).Store(cheep);
            // StoreChirp(message, unixTime);
        }


}
