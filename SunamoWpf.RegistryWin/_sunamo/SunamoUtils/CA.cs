namespace SunamoWpf.RegistryWin._sunamo;

internal class CA
{

    internal static List<string> Trim(List<string> items)
    {
        for (var index = 0; index < items.Count; index++) items[index] = items[index].Trim();
        return items;
    }
    internal static void InitFillWith(List<string> datas, int pocet, string initWith = "")
    {
        InitFillWith<string>(datas, pocet, initWith);
    }

    internal static void InitFillWith<T>(List<T> datas, int pocet, T initWith)
    {
        for (var index = 0; index < pocet; index++) datas.Add(initWith);
    }

    internal static List<string> ToListString(params string[] values)
    {
        return values.ToList();
    }
}