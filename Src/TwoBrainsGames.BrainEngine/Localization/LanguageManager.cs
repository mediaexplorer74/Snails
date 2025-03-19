
// Type: TwoBrainsGames.BrainEngine.Localization.LanguageManager
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using System.Collections.Generic;
using System.Globalization;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.BrainEngine.Localization
{
  public class LanguageManager
  {
    private static Dictionary<string, string> localization;

    internal static void LanguageChanged()
    {
      LanguageManager.localization = (Dictionary<string, string>) null;
    }

    public static string GetString(string id)
    {
      if (string.IsNullOrEmpty(id))
        return id;
      if (LanguageManager.localization == null)
      {
        DataFileRecord dataFileRecord = BrainGame.ResourceManager.Load<DataFileRecord>("localization/language_" + BrainGame.CurrentLanguage.ToString(), ResourceManager.ResourceManagerCacheType.Static);
        LanguageManager.localization = new Dictionary<string, string>();
        foreach (DataFileRecord selectRecord in dataFileRecord.SelectRecords("text"))
        {
          string fieldValue1 = selectRecord.GetFieldValue<string>(nameof (id));
          string fieldValue2 = selectRecord.GetFieldValue<string>("value");
          LanguageManager.localization.Add(fieldValue1, fieldValue2);
        }
      }
      string text = (string) null;
      if (!LanguageManager.localization.TryGetValue(id, out text))
        throw new BrainException("String with id '" + id + "' not found in language file.");
      text = LanguageManager.ParseId(text);
      return !string.IsNullOrEmpty(text) ? text : id;
    }

    public static string[] GetMultiString(string id) => LanguageManager.GetString(id).Split('|');

    public static LanguageCode GetDefaultSystemLanguage()
    {
      CultureInfo currentUiCulture = CultureInfo.CurrentUICulture;
      if (currentUiCulture.TwoLetterISOLanguageName.ToLower() == "pt")
        return LanguageCode.pt;
      if (currentUiCulture.TwoLetterISOLanguageName.ToLower() == "es")
        return LanguageCode.es;
      if (currentUiCulture.TwoLetterISOLanguageName.ToLower() == "it")
        return LanguageCode.it;
      if (currentUiCulture.TwoLetterISOLanguageName.ToLower() == "fr")
        return LanguageCode.fr;
      return currentUiCulture.TwoLetterISOLanguageName.ToLower() == "de" ? LanguageCode.de : LanguageCode.en;
    }

    private static string ParseId(string text)
    {
      if (string.IsNullOrEmpty(text))
        return text;
      string str = (string) null;
      if (BrainGame.Settings.UseMouse)
        str = "INPUT_MOUSE";
      if (BrainGame.Settings.UseTouch)
        str = "INPUT_TOUCH";
      if (BrainGame.Settings.UseGamepad)
        str = "INPUT_GAMEPAD";
      int num = text.IndexOf(str + "#");
      if (num == -1)
        return text;
      text = text.Substring(num + str.Length + 1);
      int length = text.IndexOf("#");
      return length == -1 ? text : text.Substring(0, length);
    }
  }
}
