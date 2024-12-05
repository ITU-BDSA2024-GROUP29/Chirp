
using System.Diagnostics;

namespace Chirp.Razor.test.ChirpRazor.Tests{
    public static class ServerBoot{
     

        public static Process ServerStart(){
            var _ServerProcess = new Process
            {
                StartInfo = {
                   FileName = "dotnet",
				    Arguments = "run",
				    WorkingDirectory = Path.GetFullPath("../../../../../src/Chirp.Razor", AppContext.BaseDirectory),
				    UseShellExecute = false,
				    RedirectStandardOutput = true,
				    RedirectStandardError = true,
				    CreateNoWindow = true
                }
            };


            var WorkingDirectory = Path.GetFullPath("../../../../../src/Chirp.Razor", AppContext.BaseDirectory);
            Console.WriteLine("Path to look at " + WorkingDirectory);
            return _ServerProcess;
            
        }

        public static async Task WaitForServerToBoot(){
        using var httpClient = new HttpClient();
        var retries = 10;
        while (retries-- > 0)
        {
            try
            {
                var response = await httpClient.GetAsync("http://localhost:5273/");
                if (response.IsSuccessStatusCode) return;
            }
            catch
            {
                // Ignore connection errors and retry
            }

            await Task.Delay(1000); // Wait and retry
        }

        throw new Exception("Server did not start in time.");
    }

    public static void ServerStop(Process process)
	    {
		    if (process != null && !process.HasExited)
		    {
			    process.Kill();
			    process.Dispose();
		    }
	    }
    }
}
