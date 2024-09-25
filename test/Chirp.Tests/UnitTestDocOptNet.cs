using Xunit.Abstractions;
using DocoptNet;
using static Chirp.Program;
using Xunit.Sdk;

namespace Chirp.Tests;

public class UnitTestDocOptNet {
    
    private readonly ITestOutputHelper output;

    public UnitTestDocOptNet(ITestOutputHelper output)
    {
        this.output = output;
    }
    
    [Fact]
    public void TestVersion() { //are these tests really necessary?
        
       // Program p = new Program();
        //var arguments = new Docopt().Apply(p.usage, args, version: "1.0", exit: true)!;
        
        
    }
}