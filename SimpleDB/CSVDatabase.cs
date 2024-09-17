
using System.Reflection;
using System.Text;
using System.Text.Json;
using SimpleDB;

public class CSVDatabase<T>  : IDatabaseRepository<T> where T : class
{
    public static string filePath = "./Data/chirp_cli_db.csv";
    public IEnumerable<T> Read(int? limit = null)
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath);
        }
        var lines = File.ReadLines(filePath);

        // Get the properties of T
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var constructor = typeof(T).GetConstructors().First();

        // Skip the header line and iterate over the rest
        foreach (var line in lines.Skip(0))
        {
            var values = line.Split(',');

            // Convert values to appropriate types and pass them to the constructor
            var parameters = properties.Select((prop, index) =>
                Convert.ChangeType(values[index], prop.PropertyType)).ToArray();

            // Create a new instance of T using the constructor
            T record = (T)constructor.Invoke(parameters);

            yield return record;
        }
    }

    public static CSVDatabase<T> GetDatabase() {
        if (database == null) {
            database = new CSVDatabase<T>();
        }
        return database;
    }

    private static CSVDatabase<T> database = null;

    public void Store(T record)
    {
        StringBuilder csv = new StringBuilder();

        var properties = typeof(T).GetProperties();
        if (!File.Exists(filePath)) {
            File.Create(filePath);
            // Add the header row
            foreach (var prop in properties)
            {
                csv.Append(prop.Name + ",");
            }
            csv.Length--; // Remove the last comma
            csv.AppendLine();
        }

            // Get the properties of the type T


        foreach (var prop in properties)
        {
            var value = prop.GetValue(record)?.ToString();
            csv.Append(value + ",");
        }
        csv.Length--; // Remove the last comma
        csv.AppendLine();


        // Stores new messages in CSV file
        using (StreamWriter sw = File.AppendText(filePath))
        {
            sw.WriteLine(csv);
        }
    }
}
