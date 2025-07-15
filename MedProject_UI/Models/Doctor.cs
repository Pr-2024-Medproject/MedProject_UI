using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MedProject_UI.Models;

public class Doctor
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string LastName { get; set; } // Прізвище
    public string FirstName { get; set; } // Ім'я
    public string MiddleName { get; set; } // По батькові

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime BirthDate { get; set; } // Дата народження
    public string Phone { get; set; } // Номер телефону
    public string Email { get; set; } // Електронна пошта
    public string Address { get; set; } // Адреса проживання

    public string Position { get; set; } // Посада (e.g. Лікар-уролог)

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime StartDate { get; set; } // Дата початку роботи

    public string Username { get; set; } // Логін
    public string PasswordHash { get; set; } // Пароль (у вигляді хешу)
    public string AccessLevel { get; set; } // Рівень доступу: "Admin", "Doctor", "Viewer" тощо
    public List<WorkPeriod> WorkSchedule { get; set; } = new(); // Робочий графік

    [BsonIgnore] // Щоб не зберігати в MongoDB
    public string FullName => $"{LastName} {FirstName} {MiddleName}";

    [BsonIgnore] // Щоб не зберігати в MongoDB
    public string ShortName => $"{FirstName[0]}.{MiddleName[0]}. {LastName}";
    [BsonIgnore] // Щоб не зберігати в MongoDB
    public string ShortNameWithSpec => $"{FirstName[0]}.{MiddleName[0]}. {LastName} | {Position}";
    [BsonIgnore]
    public string OnDutyStatus { get; set; }
}

public class WorkPeriod
{
    public WorkStatus Status { get; set; } // "Р", "В", "В", "Н"

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime From { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime To { get; set; }
}

public enum WorkStatus
{
    None, // Н
    Work, // Р
    Vacation, // Відпустка
    DayOff // Вихідний
}