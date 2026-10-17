namespace SunamoWpf.RegistryWin;

/// <summary>
/// Pomaha pracovat s registry pomoci nek. M.
/// Impl. IPrevedPpk pro prevody mezi PolozkaRegistru a RegistryKey
/// M IPrevedPpk zatim nejsou impl.
///
/// </summary>
public class RegistryHelper //: IRegistry //, IPrevedPpk<RegistryKey, PolozkaRegistru>
{
    #region DPP
    /// <summary>
    /// Znak, kterym se oddeluji tokeny v registru
    /// </summary>
    static string oddeloacRegistru = Path.AltDirectorySeparatorChar.ToString();
    #endregion
    static Type type = typeof(RegistryHelper);
    #region -!Hotovo!-
    /// <summary>
    /// V A1 projde vsechny prvky a vrati o [] jejich cestu, nazev a hodnotu.
    /// Hodnotu ne, leda ze by i nacitala sama PolozkaRegistru, coz nemam overene.
    /// Pokud je A2 true, nebudu ziskavat jen nazvy, ale i hodnoty.
    /// </summary>
    /// <param name="klic"></param>
    /// <param name="vsechnyHodnoty"></param>
    public static RegistryEntry[] GetAllItemFromRegistryKeyInArray(RegistryKey klic, bool vsechnyHodnoty)
    {
        string klicS = klic.ToString();
        List<RegistryEntry> vratit = new List<RegistryEntry>();
        List<string> strings = CA.ToListString(klic.GetValueNames());
        #region Polkud A2, vsechny je rek projdu a
        if (vsechnyHodnoty)
        {
            ThrowEx.Custom(Translate.FromKey(XlfKeys.MustNotBeEnteredWithA2True));
            //vratit.AddRange(VsechnyKlice(klic));
        }
        #endregion
        #region Jinak ulozim jen aktualni
        else
        {
            foreach (string var in strings)
            {
                vratit.Add(new RegistryEntry(klicS, var));
            }
        }
        #endregion
        return vratit.ToArray();
    }
    /// <summary>
    /// Rekurzivne projde vettev registru a G vsechny klice.
    /// </summary>
    /// <param name="adresar"></param>
    /// <summary>
    /// G pole, ktere bude obsahovat podklice A1 v klici A1 A2.
    /// </summary>
    /// <param name="keyNames"></param>
    /// <param name="adresar"></param>
    public static IList<RegistryKey> CombinePathWithKeys(List<string> keyNames, RegistryKey adresar)
    {
        List<RegistryKey> keys = new List<RegistryKey>();
        foreach (string var in keyNames)
        {
            keys.Add(adresar.OpenSubKey(var));
        }
        return keys;
    }
    /// <summary>
    /// G hodnoty vsech polozek ktere byly nalezeny v A1 nerek vc. hodnoty.
    /// </summary>
    public static RegistryEntry[] GetAllValues(string cesta)
    {
        List<RegistryEntry> entries = new List<RegistryEntry>();
        RegistryKey registryKey = GetObjectRegistryKey(cesta);
        string rks = registryKey.ToString();
        List<string> valueNames = CA.ToListString(registryKey.GetValueNames());
        foreach (string var in valueNames)
        {
            entries.Add(new RegistryEntry(Registry.GetValue(rks, var, null), cesta, var));
        }
        return entries.ToArray();
    }
    #region Zakladni
    /// <summary>
    /// G O RK, ktery uz ma nactenou cestu.
    /// </summary>
    /// <param name="cesta"></param>
    /// <summary>
    /// G O RegistryKey, ktery reprezentuje nekterou z hlavnich vetvi A1 a do A2 ulozim zbytek.
    /// </summary>
    /// <param name="cesta"></param>
    /// <param name="zbylaCesta"></param>
    public static RegistryKey GetObjectRegistryKey(string cesta)
    {
        string zbylaCesta = null;
        // TODO: Pridat kontrolu, zda tomu tak je
        string pred = null;
        SH.GetPartsByLocation(out pred, out zbylaCesta, cesta, cesta.IndexOf("\\"));
        var tokeny = SHSplit.Split(zbylaCesta, "\\");
        Type registryType = typeof(Registry);
        FieldInfo[] fields = registryType.GetFields();
        RegistryKey vratit = null;
        foreach (FieldInfo var in fields)
        {
            // Protoze je staticka, zkusim uzit null
            object rko = var.GetValue(null);
            RegistryKey registryKey = (RegistryKey)rko;
            if (pred == registryKey.ToString())
            {
                vratit = registryKey;
                break;
            }
        }
        foreach (string item in tokeny)
        {
            vratit = vratit.CreateSubKey(item);
        }
        return vratit;
    }
    #endregion
    #region IRegistry Members
    /// <summary>
    /// Do cesty A2, kde je zahr. i hodnota, zapisu A1.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="cesta"></param>
    public static void SetValue(object value, string cesta)
    {
        string nazevObjektu = "";
        cesta = ExtractPathFromPath(out nazevObjektu, cesta);
        RegistryKey registryKey = GetObjectRegistryKey(cesta);
        registryKey.SetValue(nazevObjektu, value);
    }
    /// <summary>
    /// G hodnotu z komplexni cesty vc. hodnoty A1.
    /// </summary>
    /// <param name="cesta"></param>
    public static object GetValue(string cesta)
    {
        string nazevObjektu = "";
        cesta = ExtractPathFromPath(out nazevObjektu, cesta);
        RegistryKey registryKey = GetObjectRegistryKey(cesta);
        return registryKey.GetValue(nazevObjektu);
    }
    #endregion
    #region Spojovani
    /// <summary>
    /// G spojene a1  a A2.
    /// </summary>
    /// <param name="parentPath"></param>
    /// <param name="childPath"></param>
    public static string CombinePaths(string parentPath, string childPath)
    {
        return Path.Combine(parentPath, childPath);
    }
    /// <summary>
    /// Otevre subklic A2 v A1.
    /// </summary>
    /// <param name="parentKey"></param>
    /// <param name="subKeyName"></param>
    public static RegistryKey CombinePaths(RegistryKey parentKey, string subKeyName)
    {
        return parentKey.OpenSubKey(subKeyName);
    }
    #endregion
    /// <summary>
    /// Rozdeli cestu a nazev hodnoty
    /// Pokud je hodnota spojena, tj, vc. hodnoty se kterou se ma spojit, vrati pouze klic k ni
    /// G cestu z A2 a do A1 ulozim zjistenou hodnotu.
    /// M nekontroluje, zda se jedna o H a muze vratit posledni token cesty.
    /// </summary>
    /// <param name="hodnota"></param>
    /// <param name="?"></param>
    static string ExtractPathFromPath(out string hodnota, string cesta)
    {
        int hodnotaOd = cesta.LastIndexOf(oddeloacRegistru);
        hodnota = cesta.Substring(hodnotaOd);
        string vratit = cesta.Substring(0, hodnotaOd);
        return vratit;
    }
    #endregion
    #region NI
    #region IPrevedPpk<RegistryKey,PolozkaRegistru> Members
    /// <summary>
    /// Prevede seznam PolozkaRegistru na seznam RegistryKey.
    /// Neprevadi nic, naplni vse null
    /// </summary>
    /// <param name="entries"></param>
    public static List<RegistryKey> ConvertPpk(List<RegistryEntry> entries)
    {
        List<RegistryKey> ppk = new List<RegistryKey>();
        foreach (RegistryEntry var in entries)
        {
            ppk.Add(null);
        }
        return ppk;
    }
    /// <summary>
    /// Prevede seznam PolozkaRegistru na seznam RegistryKey.
    /// Neprevadi nic, naplni vse null
    /// </summary>
    /// <param name="keys"></param>
    public static List<RegistryEntry> ConvertPpk(List<RegistryKey> keys)
    {
        List<RegistryEntry> ppk = new List<RegistryEntry>();
        foreach (RegistryKey var in keys)
        {
            ppk.Add(null);
        }
        return ppk;
    }
    #endregion
    #endregion
    public static void GetHkeyAndPath(string registryPath, out string hkey, out string key)
    {
        SH.GetPartsByLocation(out hkey, out key, registryPath, '\\');
    }
}