using System.Text.RegularExpressions;

namespace Chirp;

public class Controller
{
    public static void PrintChirps()
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
    
    public static void Chirp(string message, long unixTime)
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
        return $"{Environment.UserName.Replace(' ','_')} @ {formattedDate}: {message}";   //.replace for bad PC names
    }

    static void StoreChirp(string message, long unixTime)
    {
        // Stores new messages in CSV file
        using (StreamWriter sw = File.AppendText("./Data/chirp_cli_db.csv"))
        {
            sw.WriteLine($"{Environment.UserName.Replace(' ','_')},\"{message}\",{unixTime}");  //.replace for bad PC names
        }
    }

    public static long getunixTime() {
        return ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
    }
    
}