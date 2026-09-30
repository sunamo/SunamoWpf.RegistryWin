#define ASYNC
namespace SunamoWpf.RegistryWin._sunamo;

internal class CA
{

    public static List<string> Trim(List<string> l)
    {
        for (var i = 0; i < l.Count; i++) l[i] = l[i].Trim();
        return l;
    }
    public static void InitFillWith(List<string> datas, int pocet, string initWith = "")
    {
        InitFillWith<string>(datas, pocet, initWith);
    }

    public static void InitFillWith<T>(List<T> datas, int pocet, T initWith)
    {
        for (var i = 0; i < pocet; i++) datas.Add(initWith);
    }

    public static List<string> ToListString(params string[] v)
    {
        return v.ToList();
    }
}
