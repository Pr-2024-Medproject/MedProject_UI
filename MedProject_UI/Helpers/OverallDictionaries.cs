using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedProject_UI.Helpers
{
    internal class OverallDictionaries
    {
        public OverallDictionaries()
        {
            Dictionaries = new Dictionary<string, Dictionary<int, string>>()
            {
                { "dictOverallItem1",  dictOverallItem1 },
                { "dictOverallItem2",  dictOverallItem2 },
                { "dictOverallItem3",  dictOverallItem3 },
                { "dictOverallItem4",  dictOverallItem4 },
                { "dictOverallItem5",  dictOverallItem5 },
                { "dictOverallItem6",  dictOverallItem6 },
                { "dictOverallItem8",  dictOverallItem8 },
                { "dictOverallItem10", dictOverallItem10 },
                { "dictOverallItem13", dictOverallItem13 },
                { "dictOverallItem14", dictOverallItem14 }
            };
        }

        public Dictionary<string, Dictionary<int, string>> Dictionaries { get; set; }

        public Dictionary<int, string> dictOverallItem1 = new()
        {
            { 0, "Задовільний" },
            { 1, "Відносно задовільний" },
            { 2, "Середньої важкості" },
            { 3, "Важкий" }
        };

        public Dictionary<int, string> dictOverallItem2 = new()
        {
            { 0, "Нормостенік" },
            { 1, "Астенік" },
            { 2, "Гиперстенік" }
        };

        public Dictionary<int, string> dictOverallItem3 = new()
        {
            { 0, "Підвищене" },
            { 1, "Помірне" },
            { 2, "Знижене" },
            { 3, "Кахексія" }
        };

        public Dictionary<int, string> dictOverallItem4 = new()
        {
            { 0, "Немає" },
            { 1, "При фізичному навантаженні" },
            { 2, "При розмові" },
            { 3, "В спокої" }
        };

        public Dictionary<int, string> dictOverallItem5 = new()
        {
            { 0, "Звичайного кольору" },
            { 1, "Бліда" },
            { 2, "Акроцианоз" },
            { 3, "Центральний цианоз" }
        };

        public Dictionary<int, string> dictOverallItem6 = new()
        {
            { 0, "Вологий, чистий" },
            { 1, "Обкладений" },
            { 2, "Сухий" }
        };

        public Dictionary<int, string> dictOverallItem8 = new()
        {
            { 0, "В нормі" },
            { 1, "Закрепи" },
            { 2, "Проноси" }
        };

        public Dictionary<int, string> dictOverallItem10 = new()
        {
            { 0, "Звичайної форми" },
            { 1, "Не збільшений у розмірах" },
            { 2, "Не роздутий" },
            { 3, "Бере участь у дихані" },
            { 4, "При пальпації м'який, безболісний" },
            { 5, "Перитонеальні симптоми негативні" }
        };

        public Dictionary<int, string> dictOverallItem13 = new()
        {
            { 0, "В нормі" },
            { 1, "Знижений" },
            { 2, "Дізурія" }
        };

        public Dictionary<int, string> dictOverallItem14 = new()
        {
            { 0, "Чисті" },
            { 1, "Приглушені" },
            { 2, "Ритмічні" },
            { 3, "Екстрасистолія" },
            { 4, "Мерцальна аритмія" }
        };
    }
}
