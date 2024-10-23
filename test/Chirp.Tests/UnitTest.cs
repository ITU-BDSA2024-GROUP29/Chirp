using System;
using System.Diagnostics;

namespace Unit.Test
{
    public class UnitTests{
    public bool one(int tal){

    if (tal == 1){
    Console.WriteLine("det er 1");
        return true;
    }
        /*
        public bool Istime(double unixTimeStamp){

        
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        String timestamptime = dateTime.ToString("MM/dd/yy H:mm:ss");

    
    if (timestamptime == "16:00:00")
    {
        Console.WriteLine("Time is correct");
        return true;
    }
    */

    throw new NotImplementedException("Test is not correct time is not 16:00:00");
}
    }
}
