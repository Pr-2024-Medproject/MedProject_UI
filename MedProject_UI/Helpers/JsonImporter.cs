using System.IO;
using MedProject_UI.Models;
using MedProject_UI.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MedProject_UI.Helpers;

internal static class JsonImporter
{
    public static async Task ImportFromJsonAsync(string path, MongoDbService mongoService)
    {
        if (!File.Exists(path)) return;

        var json = await File.ReadAllTextAsync(path);
        var raw = JsonConvert.DeserializeObject<List<JObject>>(json);

        foreach (var obj in raw)
        {
            var visit = new Visit
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                Notes = "", 
                DocumentGenerated = false,
                Symptoms = obj.Properties()
                    .Where(p => p.Name.StartsWith("_field") && p.Name != "_fieldDoctor")
                    .ToDictionary(p => p.Name, p => p.Value.ToString())
            };

            var patient = new Patient
            {
                CardNumber = obj["_colCardNumber"]?.ToString(),
                LastName = obj["_colLastName"]?.ToString(),
                FirstName = obj["_colFirstName"]?.ToString(),
                MiddleName = obj["_colMiddleName"]?.ToString(),
                BirthDate = (DateTime)TryParseDate(obj["_colBirthDay"]?.ToString()),
                Age = int.TryParse(obj["_colAge"]?.ToString(), out var age) ? age : 0,
                Address = obj["_colAddress"]?.ToString(),
                Profession = obj["_colProfession"]?.ToString(),

                Phone = "0000000000",
                Email = "unknown@example.com",
                Gender = "unknown",

                Doctor = obj["_fieldDoctor"]?.ToString(),
                Visits = new List<Visit> { visit }
            };

            await mongoService.AddPatientAsync(patient);
        }
    }

    private static DateTime? TryParseDate(string? dateStr)
    {
        if (DateTime.TryParse(dateStr, out var result))
            return result;
        return null;
    }
}