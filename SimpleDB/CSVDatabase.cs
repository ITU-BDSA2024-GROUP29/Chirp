using SimpleDB;
using CsvHelper;
using System.Globalization;


public record Cheep(String Author, String Message, long TimeStamp);
public class CSVDatabase<T> : IDatabaseRepository<T>
{
    public static CSVDatabase<Cheep> csvdatabase;
    public CSVDatabase(){

    }
     public IEnumerable<T> Read(int? limit){
        List<Cheep> ListCheep = new List<Cheep>();
        int i = 0;
        using (var reader = new StreamReader("Data/cheeps.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var cheeps = csv.GetRecords<Cheep>();
            cheeps = cheeps.Reverse();
            foreach(Cheep cheep in cheeps){
                ListCheep.Add(cheep);
                i++;
                if(i == limit){
                    break;
                }
            }
        }
        return ListCheep.Cast<T>();
     }
    public void Store(T record){
        using (var Stream = File.Open("Data/cheeps.csv",FileMode.Append))
        using (var writer = new StreamWriter(Stream))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecord(record);
            writer.WriteLine();
        }
        Console.WriteLine();
        Console.WriteLine("Cheep was succesfully sent!");
    }

    public static CSVDatabase<Cheep> GetInstance()
    {
        return csvdatabase ??= new CSVDatabase<Cheep>(); 
    }
}