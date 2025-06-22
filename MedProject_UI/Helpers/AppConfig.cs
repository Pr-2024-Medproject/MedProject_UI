using System.IO;
using Newtonsoft.Json;

namespace MedProject_UI.Services;

public class AppConfig
{
    public string MongoDbConnection { get; set; }
    public string DatabaseName { get; set; }
    public string PatientsCollection { get; set; }

    public static AppConfig Load(string path = "config.json")
    {
        var json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<AppConfig>(json);
    }
}