namespace MedProject_UI.Models;

public class Visit
{
    public DateTime Date { get; set; }
    public Dictionary<string, string> Symptoms { get; set; } = new();
    public string Notes { get; set; }
    public bool DocumentGenerated { get; set; }
}