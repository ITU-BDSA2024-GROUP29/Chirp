using Chirp.ChirpUtils;

namespace Chirp.Tests;
using static UserInterface;




public class UnitTestUserInterface {
    //make unix time test
    
    [Fact]
    public void Test1() {
        Assert.True(true);
    }

    [Fact]
    public void CheepAChirp() {
        //System.IO.File.WriteAllText(@"Data.Tests/chirp_cli_db.csv",string.Empty);
        //UserInterface ui = new UserInterface();
        Cheep TestCheep = new Cheep("UnitTester", "unittest", 1727253543);
        
        Chirp(TestCheep);
        PrintChirps(0);

        var arrStr = "";
        var chirps = CSVDatabase<Cheep>.GetDatabase().Read(0);
        
        //int Counter = 0;
        //arrStr = Console.ReadLine();
        
        //Console.WriteLine(arrStr);
        //using (var reader = new StreamReader("Data/cheeps.csv"));
            
        
        //Assert.Equal("unittest", arrStr);

    }
    
    
}