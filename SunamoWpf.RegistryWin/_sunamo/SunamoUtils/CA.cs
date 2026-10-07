#define ASYNC
namespace SunamoWpf.RegistryWin._sunamo;

internal class CA
{

    public static List<string> Trim(List<string> items)
    {
        for (var index = 0; index < items.Count; index++) items[index] = items[index].Trim();
        return items;
    }
    public static void InitFillWith(List<string> datas, int pocet, string initWith = "")
    {
        InitFillWith<string>(datas, pocet, initWith);
    }

    public static void InitFillWith<T>(List<T> datas, int pocet, T initWith)
    {
        for (var index = 0; index < pocet; index++) datas.Add(initWith);
    }

    public static List<string> ToListString(params string[] values)
    {
        return values.ToList();
    }
}
