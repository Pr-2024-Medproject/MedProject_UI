using MedProject_UI.Helpers;
using MedProject_UI.Models;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MedProject_UI;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static Doctor? CurrentUser { get; set; }

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

    public Dictionary<int, string> dictOverallItem13 = new()
    {
        { 0, "В нормі" },
        { 1, "Знижений" },
        { 2, "Дізурія" }
    };
}