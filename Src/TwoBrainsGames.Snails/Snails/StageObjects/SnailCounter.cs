
// Type: TwoBrainsGames.Snails.StageObjects.SnailCounter
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class SnailCounter : StageObject
  {
    public const string FONT_SNAIL_COUNTER = "SNAIL_COUNTER_FONT";
    private const int FRAME_IDX_STAGE_EXIT = 0;
    private const int FRAME_IDX_RELEASE_RIGHT = 1;
    private const int FRAME_IDX_RELEASE_LEFT = 2;
    private const int FRAME_IDX_RELEASE_BOTH = 3;
    private int _counter;
    private TextFont _font;
    private Rectangle _textRect;

    public SnailCounter()
      : base(StageObjectType.SnailCounter)
    {
      this._counter = 0;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._font = BrainGame.ResourceManager.Load<TextFont>("fonts/snail-counter-" + Levels.CurrentTheme.ToString(), ResourceManager.ResourceManagerCacheType.Static);
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      this._font = ((SnailCounter) other)._font;
    }

    public override void Initialize()
    {
      base.Initialize();
      this._textRect = this.TransformCurrentFrameBB().ToRect();
    }

    public override void Draw(bool shadow)
    {
      base.Draw(shadow);
      this._font.DrawString(Stage.CurrentStage.SpriteBatch, this._counter.ToString(), this._textRect, TextFont.TextHorizontalAlign.Center);
    }

    public void SetCounter(int value) => this._counter = value;

    public void RemoveLinks()
    {
      if (this.LinkedObjects.Count == 0)
        return;
      foreach (StageObject linkedObject in this.LinkedObjects)
      {
        if (linkedObject is StageEntrance)
          ((StageEntrance) linkedObject).SnailCounter = (SnailCounter) null;
        else if (linkedObject is StageExit)
          ((StageExit) linkedObject).SnailCounter = (SnailCounter) null;
      }
      this.LinkString = "";
    }

    public void ConnectToEntrance(StageEntrance entrance)
    {
      switch (entrance.ReleaseDirection)
      {
        case StageEntrance.EntranceReleaseDirection.Clockwise:
          this.CurrentFrame = 1;
          break;
        case StageEntrance.EntranceReleaseDirection.CounterClockwise:
          this.CurrentFrame = 2;
          break;
        case StageEntrance.EntranceReleaseDirection.Both:
          this.CurrentFrame = 3;
          break;
      }
    }
  }
}
