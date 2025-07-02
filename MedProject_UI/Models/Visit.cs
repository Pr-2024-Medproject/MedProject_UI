using MongoDB.Bson.Serialization.Attributes;

namespace MedProject_UI.Models;

public class Visit
{
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime StartDate { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime EndDate { get; set; }
    public Dictionary<string, string> Symptoms { get; set; } = new();
    public string Notes { get; set; }
    public bool DocumentGenerated { get; set; }
}