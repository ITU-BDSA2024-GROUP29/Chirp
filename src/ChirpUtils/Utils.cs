
using System.Text.RegularExpressions;

namespace Chirp.ChirpUtils;

public static class Utils
{
    public static string FormatFromFileToConsole(string line)
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

    public static string ConvertUnixTimeToDate(long unixTime)
    {
        DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).LocalDateTime;
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static long GetUnixTime() {
        return ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
    }
    
    
}
