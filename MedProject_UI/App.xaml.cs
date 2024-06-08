using System.Configuration;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace MedProject_UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public sealed class DataItem
        {
            public bool? _colCheckBox { get; set; }
            public string _colCardNumber { get; set; }
            public string? _colLastName { get; set; }
            public string? _colFirstName { get; set; }
            public string _colMiddleName { get; set; }
            public DateTime? _colBirthDay { get; set; }
            public int _colAge
            {
                get
                {
                    return _colBirthDay.Value.Date > DateTime.Today.AddYears(-(DateTime.Today.Year - _colBirthDay.Value.Year))
                        ? DateTime.Today.Year - _colBirthDay.Value.Year - 1
                        : DateTime.Today.Year - _colBirthDay.Value.Year;
                }
            }
            public string? _colAddress { get; set; }
            public string? _colProfession { get; set; }
            public DateTime? _colHospitalDate { get; set; }
            public DateTime? _colLeaveDate { get; set; }
            public string[]? _fieldClaims { get; set; } //Скарги
            public string? _fieldEntrDiagnosis { get; set; } //Діагноз при госпіталізації
            public string? _fieldFinalDiagnosis { get; set; } //Заключний діагноз
            public string? _fieldComplication { get; set; } //Ускладнення
            public string? _fieldAdditionalDiagnosis { get; set; } //Супутній діагноз
            public string? _fieldMKX { get; set; } //МКХ Шифр
            public string? _fieldOperationName { get; set; } //Назва Операції
            public DateTime? _fieldOperationDate { get; set; } //Дата операції
            public string? _fieldChemotherapy { get; set; } //Схема ПХТ
            public DateTime? _fieldChemotherapyDate { get; set; } //Дата ЛТ/ПХТ
            public string? _fieldHistology { get; set; } //Гістологія/Цистологія
            public string? _fieldDoctor { get; set; } //Лікуючий лікар
            public string? _fieldDepartmentHead { get; set; } //Завідувач відділенням
            public string? _fieldDepartHeadAssistant { get; set; } //В.о. завідувача відділенням
            public int? _fieldOverallItem1 { get; set; } //Загальний стан
            public int? _fieldOverallItem2 { get; set; } //Конституція
            public int? _fieldOverallItem3 { get; set; } //Харчування
            public int? _fieldOverallItem4 { get; set; } //Задишка
            public int? _fieldOverallItem5 { get; set; } //Шкіра
            public int? _fieldOverallItem6 { get; set; } //Язик
            public string? _fieldOverallItem7 { get; set; } //Печінка
            public int? _fieldOverallItem8 { get; set; } //Стілець
            public bool? _fieldOverallItem9_1 { get; set; } //Симптом Пастернацького (статус)
            public bool? _fieldOverallItem9_2 { get; set; } //Симптом Пастернацького (місце)
            public int[]? _fieldOverallItem10 { get; set; } //Живіт
            public int? _fieldOverallItem11 { get; set; } //Пульс
            public string? _fieldOverallItem12 { get; set; } //Артеріальний тиск
            public int? _fieldOverallItem13 { get; set; } //Діурез
            public int[]? _fieldOverallItem14 { get; set; } //Тони серця
            public int? _fieldOverallItem15 { get; set; } //Частота дихальних рухів (ЧДР)
            public string? _fieldAnamnesisItem1 { get; set; } //Хворіє з
            public string? _fieldAnamnesisItem2 { get; set; } //Зміни виявлені при профогляді
            public string? _fieldAnamnesisItem3 { get; set; } //Звернувся в
            public string? _fieldAnamnesisItem4 { get; set; } //Виставлено діагноз
            public string? _fieldAnamnesisItem5 { get; set; } //Проведено дообстеження
            public string? _fieldAnamnesisItem6 { get; set; } //Лікування
            public string? _fieldAnamnesisItem7 { get; set; } //Попередня ХЛТ
            public string? _fieldAnamnesisItem8 { get; set; } //КТ ОГК, ОБП, ОМТ, ГМ
            public string? _fieldAnamnesisItem9 { get; set; } //МРТ ГМ
            public string? _fieldAnamnesisItem10 { get; set; } //ФБС
            public string? _fieldAnamnesisItem11 { get; set; } //ФЕГДС
            public string? _fieldLifeAnamnesisItem1 { get; set; } //Туберкульоз
            public string? _fieldLifeAnamnesisItem2 { get; set; } //Венеричні захворювання
            public string? _fieldLifeAnamnesisItem3 { get; set; } //Цукровий діабет
            public string? _fieldLifeAnamnesisItem4 { get; set; } //Гіпертонічна хвороба
            public string? _fieldLifeAnamnesisItem5 { get; set; } //Ішемічна хвороба серця
            public string? _fieldLifeAnamnesisItem6 { get; set; } //ХОЗЛ
            public string? _fieldLifeAnamnesisItem7 { get; set; } //Інші хвороби
            public int? _fieldLifeAnamnesisItem8 { get; set; } //На л/л з
            public int? _fieldLifeAnamnesisItem9 { get; set; } //Всього л/л
            public string? _fieldLifeAnamnesisItem10 { get; set; } //Постійно приймає
            public string[]? _fieldLocusMorbiItem1 { get; set; } //Надключні л/в
            public string[]? _fieldLocusMorbiItem2 { get; set; } //Пахові л/в
            public bool? _fieldLocusMorbiItem3 { get; set; } //Над легенями дихання
            public string[]? _fieldLocusMorbiItem4 { get; set; } //Ослаблення дихання
            public bool? _fieldLocusMorbiItem5 { get; set; } //Перкуторно
            public string[]? _fieldLocusMorbiItem6 { get; set; } //Притуплення
            public string[]? _fieldLocusMorbiItem7 { get; set; } //Хрипи

            public DataItem()
            {
                _colCardNumber = GuidHelper.GenerateShortGuid();

                _colLastName = GetRandomLastName();
                _colFirstName = GetRandomFirstName();
                _colMiddleName = new List<string> { "Олександр", "Сергій", "Дмитро", "Андрій", "Максим", "Владислав", "Євген", "Володимир", "Віталій", "Ігор", "Роман", "Юрій", "Олег", "Артем", "Олексій", "Микола", "Денис", "Іван", "Руслан", "Віктор", "Михайло", "Антон", "Анатолій", "Станіслав", "Богдан", "Вадим", "В'ячеслав", "Павло", "Ярослав", "Валерій", "Костянтин", "Василь", "Данило", "Артур", "Микита", "Валентин", "Ілля", "Ростислав", "Кирило", "Едуард", "Тарас", "Єгор", "Петро", "Назар", "Геннадій", "Григорій", "Леонід", "Борис", "Георгій", "Тимур" }.IndexOf(_colFirstName) >= 0
                        ? new List<string> { "Олександрович", "Андрійович", "Сергійович", "Вікторович", "Іванович", "Михайлович", "Володимирович", "Юрійович", "Петрович", "Романович", "Вадимович", "Тарасович", "Артемович", "Денисович", "Богданович", "Євгенович", "Ігорович", "Олегович", "Дмитрович", "Павлович", "Максимович", "Анатолійович", "Костянтинович", "Васильович", "Леонідович", "Олексійович", "Валерійович", "Ростиславович", "Борисович", "Святославович", "Григорійович", "Антонович", "Юліанович", "Кирилович", "Тимофійович", "Владиславович", "В’ячеславович", "Захарович", "Станіславович", "Єгорович", "Назарович", "Федорович", "Рустамович", "Геннадійович", "Валентинович", "Євгенійович", "Віталійович", "Богуславович", "Мирославович", "Данилович", "Ярославович", "Веніамінович", "Захарович", "Святославович", "Романович", "Борисович", "Гаврилович", "Ілліч", "Левкович", "Миронович", "Назарович", "Опанасович", "Пантелеймонович", "Радіонович", "Семенович", "Теодорович", "Устимович", "Харитонович", "Юхимович", "Якимович", "Захарович", "Назарович", "Рустамович", "Євгенович", "Борисович", "Віталійович", "Анатолійович", "Артемович", "Богданович", "Денисович", "Кирилович", "Костянтинович", "Максимович", "Миколайович", "Назарович", "Олександрович", "Петропавлович", "Ростиславович", "Сергійович", "Тарасович", "Федорович", "Харитонович", "Юхимович", "Ярославович", "Валентинович", "Іванович", "Юрійович", "Вікторович", "Олегович", "Павлович" }[new Random().Next(0, 99)]
                        : new List<string> { "Олександрівна", "Андріївна", "Сергіївна", "Вікторівна", "Іванівна", "Михайлівна", "Володимирівна", "Юріївна", "Петрівна", "Романівна", "Вадимівна", "Тарасівна", "Артемівна", "Денисівна", "Богданівна", "Євгенівна", "Ігорівна", "Олегівна", "Дмитрівна", "Павлівна", "Максимівна", "Анатоліївна", "Костянтинівна", "Василівна", "Леонідівна", "Олексіївна", "Валеріївна", "Ростиславівна", "Борисівна", "Святославівна", "Григоріївна", "Антонівна", "Юліанівна", "Кирилівна", "Тимофіївна", "Владиславівна", "В’ячеславівна", "Захарівна", "Станіславівна", "Єгорівна", "Назарівна", "Федорівна", "Рустамівна", "Геннадіївна", "Валентинівна", "Євгеніївна", "Віталіївна", "Богуславівна", "Мирославівна", "Данилівна", "Ярославівна", "Веніамінівна", "Захарівна", "Святославівна", "Романівна", "Борисівна", "Гаврилівна", "Іллівна", "Левківна", "Миронівна", "Назарівна", "Опанасівна", "Пантелеймонівна", "Радіонівна", "Семенівна", "Теодорівна", "Устимівна", "Харитонівна", "Юхимівна", "Якимівна", "Захарівна", "Назарівна", "Рустамівна", "Євгенівна", "Борисівна", "Віталіївна", "Анатоліївна", "Артемівна", "Богданівна", "Денисівна", "Кирилівна", "Костянтинівна", "Максимівна", "Миколаївна", "Назарівна", "Олександрівна", "Петропавлівна", "Ростиславівна", "Сергіївна", "Тарасівна", "Федорівна", "Харитонівна", "Юхимівна", "Ярославівна", "Валентинівна", "Іванівна", "Юріївна", "Вікторівна", "Олегівна", "Павлівна" }[new Random().Next(0, 99)];
            }
        }

        private List<DataItem> dataItems = new List<DataItem>();

        public Dictionary<int, string> dictOverallItem1 = new Dictionary<int, string>() {
            { 0, "Задовільний" },
            { 1, "Відносно задовільний" },
            { 2, "Середньої важкості"},
            { 3, "Важкий" }
        };

        public Dictionary<int, string> dictOverallItem2 = new Dictionary<int, string>() {
            { 0, "Нормостенік" },
            { 1, "Астенік" },
            { 2, "Гиперстенік"}
        };

        public Dictionary<int, string> dictOverallItem3 = new Dictionary<int, string>() {
            { 0, "Підвищене" },
            { 1, "Помірне" },
            { 2, "Знижене"},
            { 3, "Кахексія" }
        };

        public Dictionary<int, string> dictOverallItem4 = new Dictionary<int, string>() {
            { 0, "Немає" },
            { 1, "При фізичному навантаженні" },
            { 2, "При розмові"},
            { 3, "В спокої" }
        };

        public Dictionary<int, string> dictOverallItem5 = new Dictionary<int, string>() {
            { 0, "Звичайного кольору" },
            { 1, "Бліда" },
            { 2, "Акроцианоз"},
            { 3, "Центральний цианоз" }
        };

        public Dictionary<int, string> dictOverallItem6 = new Dictionary<int, string>() {
            { 0, "Вологий, чистий" },
            { 1, "Обкладений" },
            { 2, "Сухий"}
        };

        public Dictionary<int, string> dictOverallItem8 = new Dictionary<int, string>() {
            { 0, "В нормі" },
            { 1, "Закрепи" },
            { 2, "Проноси"}
        };

        public Dictionary<int, string> dictOverallItem10 = new Dictionary<int, string>() {
            { 0, "Звичайної форми" },
            { 1, "Не збільшений у розмірах" },
            { 2, "Не роздутий"},
            { 3, "Бере участь у дихані" },
            { 4, "При пальпації м'який, безболісний" },
            { 5, "Перитонеальні симптоми негативні" }
        };

        public Dictionary<int, string> dictOverallItem13 = new Dictionary<int, string>() {
            { 0, "В нормі" },
            { 1, "Знижений" },
            { 2, "Дізурія"}
        };

        public Dictionary<int, string> dictOverallItem14 = new Dictionary<int, string>() {
            { 0, "Чисті" },
            { 1, "Приглушені" },
            { 2, "Ритмічні"},
            { 3, "Екстрасистолія" },
            { 4, "Мерцальна аритмія" }
        };

        private static string GetRandomLastName()
        {
            List<string> lastNames = new List<string>();
            lastNames.AddRange(["Мельник", "Шевченко", "Коваленко", "Бондаренко", "Бойко", "Ткаченко", "Кравченко", "Ковальчук", "Коваль", "Олійник", "Шевчук", "Поліщук", "Івановарос", "Ткачук", "Савченко", "Бондар", "Марченко", "Руденко", "Мороз", "Лисенко", "Петренко", "Клименко", "Павленко", "Кравчук", "Івановрос", "Кузьменко", "Пономаренко", "Савчук", "Василенко", "Левченко", "Харченко", "Сидоренко", "Карпенко", "Гаврилюк", "Швець", "Мельничук", "Поповарос", "Романюк", "Панченко", "Юрченко", "Мазур", "Хоменко", "Попович", "Павлюк", "Кушнірїд", "Литвиненко", "Мартинюк", "Гончаренко", "Приходько", "Костенко", "Кулик", "Романенко", "Костюк", "Семенюк", "Назаренко", "Ткач", "Кравець", "Коломієць", "Козак", "Яковенко", "Федоренко", "Ковтун", "Білоус", "Нестеренко", "Терещенко", "Колесник", "Поповрос", "Зінченко", "Тарасенко", "Міщенко", "Вовк", "Демченко", "Дяченко", "Ковальоварос", "Пилипенко", "Іщенко", "Макаренко", "Бабенко", "Кириченко", "Тищенко", "Тимошенко", "Жук", "Москаленко", "Марчук", "Власенко", "Гуменюк", "Яценко", "Радченко", "Герасименко", "Сергієнко", "Корнієнко", "Гончар", "Мартиненко", "Гордієнко", "Степаненко", "Прокопенко", "Шульга", "Волошин", "Величко", "Денисенко"]);
            return lastNames[new Random().Next(0, 99)];
        }

        private static string GetRandomFirstName()
        {
            List<string> firstNames = new List<string>();
            firstNames.AddRange(["Олександр", "Сергій", "Дмитро", "Андрій", "Максим", "Владислав", "Євген", "Володимир", "Віталій", "Ігор", "Роман", "Юрій", "Олег", "Артем", "Олексій", "Микола", "Денис", "Іван", "Руслан", "Віктор", "Михайло", "Антон", "Анатолій", "Станіслав", "Богдан", "Вадим", "В'ячеслав", "Павло", "Ярослав", "Валерій", "Костянтин", "Василь", "Данило", "Артур", "Микита", "Валентин", "Ілля", "Ростислав", "Кирило", "Едуард", "Тарас", "Єгор", "Петро", "Назар", "Геннадій", "Григорій", "Леонід", "Борис", "Георгій", "Тимур", "Наталя", "Тетяна", "Юлія", "Анна", "Олена", "Ірина", "Вікторія", "Марина", "Анастасія", "Ольга", "Катерина", "Світлана", "Аліна", "Людмила", "Альона", "Оксана", "Яніна", "Марія", "Дар'я", "Інна", "Олександра", "Валентина", "Ксенія", "Валерія", "Карина", "Євгенія", "Надія", "Алла", "Лариса", "Лілія", "Єлизавета", "Діана", "Леся", "Любов", "Галина", "Вероніка", "Віталія", "Христина", "Анжеліка", "Владислава", "Маргарита", "Ніна", "Ілона", "Поліна", "Софія", "Тамара", "Лідія", "Жанна", "Антоніна", "Віра"]);
            return firstNames[new Random().Next(0, 99)];
        }

        private static string GetRandomStreet()
        {
            List<string> adresses = new List<string>();
            adresses.AddRange(["Абрикосова", "Ананасна", "Смородинова", "Малинова", "Сунична", "Порічкова", "Зелена", "Огіркова", "Баклажанна", "Виноградна", "Калинова", "Медова", "Плодова", "Фруктова", "Шовковична", "Ягідна", "Яблунева", "Бузинова", "Бузкова", "Вербна", "Волошкова", "Зелена", "Каштанова", "Липова", "Липська", "Осокорська", "Проліскова", "Пшенична", "Тополина", "Фіалкова", "Ялинкова", "Дібровна", "Джерельна", "Долинна", "Залісна", "Залужна", "Зарічна", "Левадна", "Лісова", "Лугова", "Нагірна", "Над’ярна", "Озерна", "Паркова", "Підгірна", "Підлісна", "Приозерна", "Прирічна", "Польова", "Річна", "Садова", "Синьоозерна", "Ставкова", "Багряна дуброва", "Золоті джерела", "Квіткові луки", "Клейова долина", "Березнева", "Вереснева", "Весняна", "Зимова", "Квітнева", "Липнева", "Листопадна", "Літня", "Осіння", "Серпнева", "Травнева", "1 Травня", "9 Травня", "Введенська", "Воздвиженська", "Воскресенська", "Покровська", "Стрітенська", "Фестивальна", "Відпочинку", "Дачна", "Добрий Шлях", "Добросусідська", "Дружня", "Живописна", "Затишна", "Золота", "Зоряна", "Квітуча", "Курортна", "Лазурна", "Мальовнича", "Миру", "Привітна", "Радісна", "Райдужна", "Рожева", "Світла", "Сонячна", "Спокійна", "Творча", "Тиха", "Успішна"]);
            return $"вул. {adresses[new Random().Next(0, 99)]}, {new Random().Next(1, 199)}";
        }

        private static string GetRandomProfession()
        {
            List<string> professions = new List<string>();
            professions.AddRange(["Онлайн-терапевт", "Оператор віддаленої хірургії", "Експерт з індивідуальної фармакології", "Персональний менеджер з мікробіому людини", "Розробник кіберпротезів та імплантів", "Спеціаліст з імплантів мозку", "Творець частин тіла й органів", "Інженер-генетик", "Спеціаліст з біохакінгу і програмованого здоров’я", "Оператор медичних роботів", "Архітектор живих систем", "Інженер у галузі синтетичної біології", "Проєктувальник кіберорганізмов", "Творець мікроорганізмів із заданими функціями", "Фахівець з відродження вимерлих видів", "Агрокібернетик", "Інженер з 3D-друку продуктів харчування", "Оператор автоматизованої сільгосптехніки", "Сіті-фермер", "Фахівець зі штучного вирощування м’яса", "Оператор «розумної» переробки сміття", "Інженер з управління погодою", "Фахівець зі зміни клімату", "Експерт з точного прогнозу землетрусів", "Консультант з альтернативних видів енергії", "Оператор підземних дронів-прохідників", "Розробник систем мікрогенераціі енергії", "Дизайнер портативних енергоприладів", "Фахівець з керованого термоядерного синтезу", "Архітектор «зелених» міст", "Проєктувальник інфраструктури «розумного» будинку", "Будівельник підводних міст", "Архітектор енергоавтономних будівель", "Проєктувальник 3D-друку в будівництві", "Проєктувальник нових видів транспорту", "Автозаправник альтернативними видами палива", "Розробник «розумних» доріг", "Професійний пілот дрона", "Оператор автономних морських суден", "Інструктор летючих автомобілів", "Регулювальник руху безпілотного автотранспорту", "Юрист у сфері безпілотного транспорту", "Наноінженер", "Розробник «розумних» і композитних матеріалів", "Проєктувальник нанороботів", "Нанокриміналіст", "Дизайнер «розумного» одягу та взуття", "Техностиліст", "Персональний кравець для 3D-друку одягу", "Інженер домашніх роботів", "Проєктувальник роботів для дітей", "Розробник медичних роботів", "Творець бойових роботів", "Юрист у сфері робототехніки", "Розробник інтернету речей", "Фахівець у сфері квантових обчислень", "Квантовий криптолог", "Проєктувальник нейроінтерфейсів", "Інженер з оцифрування і зберігання пам’яті", "Консультант зі зняття цифрової залежності", "Творець цифрових двійників", "Фахівець з ІТ-та ШІ-етики", "Утилізатор цифрового сміття у сфері Big Data", "Проєктувальник особистої безпеки", "Фахівець з кібербезпеки", "Знищувач цифрових слідів", "Контролер достовірності новинного контенту (медіаполіцейський)", "Активатор корпоративного конкурентного середовища", "Консультант з цифрової трансформації компаній", "Операціоніст криптовалютного банку", "Оцінювач інтелектуальної власності", "Тайм-брокер", "Експерт з блокчейн-розвитку бізнесу", "Автор освітніх курсів на базі ШІ", "Інтегратор міждисциплінарних знань", "Експерт з «образу майбутнього» дитини", "Персональний гід з освіти і кар’єрного зростання", "Програміст бот-вчителів", "Тренер з майнд-фітнесу", "Експерт з пошуку і розвитку талантів", "Дизайнер віртуальної реальності", "Інженер доповненої реальності", "ШІ-композитор", "ШІ-письменник", "ШІ-художник", "Оператор голографічного мовлення", "Продюсер телепрограм змішаної реальності", "Агрегатор персональних новин (narrowcaster)", "Психолог з адаптації до нової реальності", "Мережевий юрист", "Цифровий лінгвіст-перекладач", "Експерт з взаємодії людей і машин", "Менеджер з відстроченої старості", "Пілот комерційних космічних кораблів", "Гід у сфері космічного туризму", "Розробник корисних копалин у космосі", "Прибиральник космічного сміття", "Космобіолог", "Проєктувальник позаземних поселень", "Спеціаліст з тераформування планет"]);
            return professions[new Random().Next(0, 99)];
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            for (int i = 0; i < 1; i++)
            {
                dataItems.Add(new DataItem
                {
                    //_colCheckBox = false,
                    _colBirthDay = DateTime.Now.AddYears(new Random().Next(16, 50) * -1)
                                               .AddMonths(new Random().Next(1, 12) * -1)
                                               .AddDays(new Random().Next(1, 28) * -1)
                                               .AddHours(new Random().Next(1, 23) * -1)
                                               .AddMinutes(new Random().Next(1, 59) * -1)
                                               .AddSeconds(new Random().Next(1, 59) * -1),
                    _colAddress = GetRandomStreet(),
                    _colProfession = GetRandomProfession(),
                    _colHospitalDate = DateTime.Now.AddYears(new Random().Next(1, 3) * -1)
                                                   .AddMonths(new Random().Next(1, 12) * -1)
                                                   .AddDays(new Random().Next(0, 28) * -1)
                                                   .AddHours(new Random().Next(0, 23) * -1)
                                                   .AddMinutes(new Random().Next(0, 59) * -1)
                                                   .AddSeconds(new Random().Next(0, 59) * -1),
                    _colLeaveDate = DateTime.Now.AddMonths(new Random().Next(1, 12) * -1)
                                                .AddDays(new Random().Next(0, 28) * -1)
                                                .AddHours(new Random().Next(0, 23) * -1)
                                                .AddMinutes(new Random().Next(0, 59) * -1)
                                                .AddSeconds(new Random().Next(0, 59) * -1)
                });
            }

            if (File.Exists("..\\..\\..\\database.json"))
            {
                try
                {
                    var lines = File.ReadLines("..\\..\\..\\database.json");
                    foreach (var line in lines)
                    {
                        dataItems.Add(JsonSerializer.Deserialize<DataItem>(line)!);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Виникла помилка при читанні з бази даних!", "Помилка читання з бази", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                }
            }

        }

        public List<DataItem> GetDataItems()
        {
            return dataItems;
        }

        public DataItem? GetDataItemsByID(string searchId)
        {
            if (dataItems.FindIndex(x => x._colCardNumber == searchId) != -1)
            {
                try
                {
                    return dataItems[dataItems.FindIndex(x => x._colCardNumber == searchId)];
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "Неочікувана помилка!", MessageBoxButton.OKCancel, MessageBoxImage.Error);

                }
            }
            else
            {
                MessageBox.Show("Користувача не знайдено в базі даних!", "Користувач відстуній", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            return null;
        }

        public void AddDataToStorage(DataItem data)
        {
            try
            {
                dataItems.Add(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Неочікувана помилка!", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        public void RemoveDataFromStorage(DataItem data)
        {
            try
            {
                string fileName = "..\\..\\..\\database.json";
                if (File.Exists(fileName))
                {
                    var oldLines = File.ReadAllLines(fileName);
                    var newLines = oldLines.Where(line => !line.Contains($"{data._colCardNumber}"));
                    File.WriteAllLines(fileName, newLines);
                }
                dataItems.Remove(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Неочікувана помилка!", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }


        public void EditDataInStorage(DataItem data)
        {
            if (dataItems.FindIndex(x => x._colCardNumber == data._colCardNumber) != -1)
            {
                try
                {
                    dataItems[dataItems.FindIndex(x => x._colCardNumber == data._colCardNumber)] = data;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "Неочікувана помилка!", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Користувача не знайдено в базі даних!", "Користувач відстуній", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }
    }

}