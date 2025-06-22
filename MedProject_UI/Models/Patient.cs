using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MedProject_UI.Models;

public class Patient
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string CardNumber { get; set; } // _colCardNumber
    public string LastName { get; set; } // _colLastName
    public string FirstName { get; set; } // _colFirstName
    public string MiddleName { get; set; } // _colMiddleName
    public DateTime BirthDate { get; set; } // _colBirthDay
    public int Age { get; set; } // _colAge
    public string Address { get; set; } // _colAddress
    public string Profession { get; set; } // _colProfession
    public DateTime? HospitalDate { get; set; } // _colHospitalDate
    public DateTime? LeaveDate { get; set; } // _colLeaveDate

    public string Phone { get; set; } = "0000000000";
    public string Email { get; set; } = "unknown@example.com";
    public string Gender { get; set; } = "unknown";

    public string Doctor { get; set; } // из _fieldDoctor
    public List<Visit> Visits { get; set; } = new();
}