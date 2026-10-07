#define ASYNC
namespace SunamoWpf.RegistryWin._sunamo;

internal class SH
{

    public static void GetPartsByLocation(out string pred, out string rest, string text, char separator)
    {
        var dex = text.IndexOf(separator);
        GetPartsByLocation(out pred, out rest, text, dex);
    }

    public static void GetPartsByLocation(out string pred, out string rest, string text, int pozice)
    {
        if (pozice == -1)
        {
            pred = text;
            rest = "";
        }
        else
        {
            pred = text.Substring(0, pozice);
            if (text.Length > pozice + 1)
                rest = text.Substring(pozice + 1);
            else
                rest = string.Empty;
        }
    }
}
