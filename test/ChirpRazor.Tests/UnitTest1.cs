namespace Chirp.Razor.test.ChirpRazor.Tests;
using Xunit;
using Unit.Test;

public class UnitTest1 {
    [Fact]
    public void Test1(){
        var unitTests = new UnitTests();
        // should be 16:00:00
        bool result = unitTests.Istime(57600);
        
        Console.WriteLine();

        Assert.True(result, "time should be 16:00");
    }
}