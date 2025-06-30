using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedProject_UI.Models
{
    internal class Doctor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string LastName { get; set; } // Прізвище
        public string FirstName { get; set; } // Ім'я
        public string MiddleName { get; set; } // По батькові

        public DateTime BirthDate { get; set; } // Дата народження
        public string Phone { get; set; } // Номер телефону
        public string Email { get; set; } // Електронна пошта
        public string Address { get; set; } // Адреса проживання

        public string Position { get; set; } // Посада (e.g. Лікар-уролог)
        public DateTime StartDate { get; set; } // Дата початку роботи

        public string Username { get; set; } // Логін
        public string PasswordHash { get; set; } // Пароль (у вигляді хешу)
        public string AccessLevel { get; set; } // Рівень доступу: "Admin", "Doctor", "Viewer" тощо

        public List<string> PatientIds { get; set; } = new(); // ID пацієнтів
        public List<WorkShift> WorkSchedule { get; set; } = new(); // Робочий графік
    }

    public class WorkShift
    {
        public DayOfWeek Day { get; set; } // День тижня
        public TimeSpan StartTime { get; set; } // Початок
        public TimeSpan EndTime { get; set; } // Кінець
    }
}