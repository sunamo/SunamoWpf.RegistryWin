#define ASYNC
namespace SunamoWpf.RegistryWin._sunamo;

internal class SHSplit
{

    public static List<string> Split(string text, params string[] newLine)
    {
        return text.Split(newLine, StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    public static List<string> Split(StringSplitOptions stringSplitOptions, string text, params string[] deli)
    {
        if (deli == null || deli.Count() == 0) throw new Exception("NoDelimiterDetermined");
        //var ie = CA.OneElementCollectionToMulti(deli);
        //var deli3 = new List<string>IEnumerable2(ie);
        var result = text.Split(deli, stringSplitOptions).ToList();
        CA.Trim(result);
        if (stringSplitOptions == StringSplitOptions.RemoveEmptyEntries)
            result = result.Where(item => item.Trim() != string.Empty).ToList();

        return result;
    }
}
