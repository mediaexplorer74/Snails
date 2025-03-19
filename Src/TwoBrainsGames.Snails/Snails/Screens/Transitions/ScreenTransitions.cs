
// Type: TwoBrainsGames.Snails.Screens.Transitions.ScreenTransitions
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;


namespace TwoBrainsGames.Snails.Screens.Transitions
{
  internal class ScreenTransitions
  {
    private const float FADE_IN_SPEED = 0.002f;
    private const float FADE_OUT_SPEED = 0.008f;
    private static FadeInTransition _fadeInTransition;
    private static FadeInTransition _fadeInWhiteTransition;
    private static FadeOutTransition _fadeOutTransition;
    private static FadeOutTransition _fadeOutWhiteTransition;
    private static LeafTransition _leafsClosingTransition;
    private static LeafTransition _leafsClosedTransition;
    private static LeafTransition _leafsOpeningTransition;

    public static FadeInTransition FadeIn => ScreenTransitions._fadeInTransition;

    public static FadeInTransition FadeInWhite => ScreenTransitions._fadeInWhiteTransition;

    public static FadeOutTransition FadeOut => ScreenTransitions._fadeOutTransition;

    public static FadeOutTransition FadeOutWhite => ScreenTransitions._fadeOutWhiteTransition;

    public static LeafTransition LeafsClosing => ScreenTransitions._leafsClosingTransition;

    public static LeafTransition LeafsClosed => ScreenTransitions._leafsClosedTransition;

    public static LeafTransition LeafsOpening => ScreenTransitions._leafsOpeningTransition;

    public static void Initialize()
    {
      ScreenTransitions._fadeInTransition = new FadeInTransition(1f / 500f);
      ScreenTransitions._fadeInWhiteTransition = new FadeInTransition(1f / 500f);
      ScreenTransitions._fadeOutTransition = new FadeOutTransition(0.008f);
      ScreenTransitions._fadeOutWhiteTransition = new FadeOutTransition(0.008f, Color.White);
      ScreenTransitions._leafsClosingTransition = new LeafTransition(LeafTransition.State.Closing);
      ScreenTransitions._leafsClosingTransition.LoadContent();
      ScreenTransitions._leafsOpeningTransition = new LeafTransition(LeafTransition.State.Opening);
      ScreenTransitions._leafsOpeningTransition.LoadContent();
      ScreenTransitions._leafsClosedTransition = new LeafTransition(LeafTransition.State.ClosedStopped);
      ScreenTransitions._leafsClosedTransition.LoadContent();
    }
  }
}
