
// Type: TwoBrainsGames.Snails.Tutorials.TutorialTopicParser
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;


namespace TwoBrainsGames.Snails.Tutorials
{
  internal class TutorialTopicParser
  {
    private const string LINE_BREAK = "[br]";
    private static Color _currentColor = Colors.TutorialTextColor;

    public static TutorialLine ParseCode(string stCode)
    {
      if (stCode == null)
        throw new SnailsException("Paremeter stCode cannot be null");
      TutorialLine tutorialLine = new TutorialLine();
      TutorialTopicParser.ParseCodeLine(stCode, tutorialLine);
      return tutorialLine;
    }

    public static void ParseCodeLine(string t, TutorialLine tutorialLine)
    {
      int num = t.IndexOf('[');
      if (num != -1)
      {
        string text = t.Substring(0, num);
        if (!string.IsNullOrEmpty(text))
          tutorialLine.Add((TutorialItem) new TutorialText(text, TutorialTopicParser._currentColor));
        TutorialTopicParser.ParseTag(t.Substring(num), tutorialLine);
      }
      else
      {
        if (string.IsNullOrEmpty(t))
          return;
        tutorialLine.Add((TutorialItem) new TutorialText(t, TutorialTopicParser._currentColor));
      }
    }

    private static void ParseTag(string t, TutorialLine tutorialLine)
    {
      t = TutorialTopicParser.ParseToken("[", t);
      string token;
      t = TutorialTopicParser.GetToken(t, out token);
      if (token == "img")
        t = TutorialTopicParser.ParseImage(t, tutorialLine);
      else if (token == "color")
        t = TutorialTopicParser.ParseColor(t);
      if (token == "enph")
        t = TutorialTopicParser.ParseEnphasize(t);
      else if (token == "unenph")
        t = TutorialTopicParser.ParseUnenphasize(t);
      TutorialTopicParser.ParseCodeLine(t, tutorialLine);
    }

    private static string ParseImage(string t, TutorialLine tutorialLine)
    {
      t = TutorialTopicParser.ParseToken("=", t);
      string token;
      t = TutorialTopicParser.GetToken(t, out token);
      tutorialLine.Add((TutorialItem) new TutorialImage(token));
      t = TutorialTopicParser.ParseToken("]", t);
      return t;
    }

    private static string ParseColor(string t)
    {
      t = TutorialTopicParser.ParseToken("=", t);
      string token1;
      t = TutorialTopicParser.GetToken(t, out token1);
      if (token1 != "DEFAULT")
      {
        t = TutorialTopicParser.ParseToken(",", t);
        string token2;
        t = TutorialTopicParser.GetToken(t, out token2);
        t = TutorialTopicParser.ParseToken(",", t);
        string token3;
        t = TutorialTopicParser.GetToken(t, out token3);
        TutorialTopicParser._currentColor = new Color((float) Convert.ToInt32(token1), (float) Convert.ToInt32(token2), (float) Convert.ToInt32(token3), 1f);
      }
      else
        TutorialTopicParser._currentColor = Colors.TutorialTextColor;
      t = TutorialTopicParser.ParseToken("]", t);
      return t;
    }

    private static string ParseEnphasize(string t)
    {
      TutorialTopicParser._currentColor = Colors.TutorialEnphasizeTextColor;
      t = TutorialTopicParser.ParseToken("]", t);
      return t;
    }

    private static string ParseUnenphasize(string t)
    {
      TutorialTopicParser._currentColor = Colors.TutorialTextColor;
      t = TutorialTopicParser.ParseToken("]", t);
      return t;
    }

    private static string GetToken(string t, out string token)
    {
      token = (string) null;
      t = t.TrimStart();
      int num = t.IndexOfAny(new char[3]{ '=', ',', ']' });
      if (num == -1)
        return "";
      token = t.Substring(0, num);
      return t.Substring(num);
    }

    private static string ParseToken(string token, string t)
    {
      t = t.TrimStart(' ');
      if (t.Substring(0, token.Length) != token)
        throw new SnailsException("Error parsing tutorial text. Token [" + token + "] not found.");
      return t.Substring(token.Length);
    }
  }
}
