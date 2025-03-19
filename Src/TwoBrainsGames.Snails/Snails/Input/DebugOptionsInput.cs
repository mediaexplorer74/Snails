
// Type: TwoBrainsGames.Snails.Input.DebugOptionsInput
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Input;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Input;


namespace TwoBrainsGames.Snails.Input
{
  internal class DebugOptionsInput : InputBase
  {
    public const Keys KEY_SHOW_HIDE_HELP = Keys.F9;
    public const Keys KEY_SHOW_HIDE_SPRITES = Keys.F2;
    public const Keys KEY_SHOW_HIDE_TILES = Keys.T;
    public const Keys KEY_SHOW_HIDE_BOUNDINGBOXES = Keys.B;
    public const Keys KEY_SHOW_HIDE_PATHS = Keys.P;
    public const Keys KEY_SHOW_QUADTREE = Keys.Q;
    public const Keys KEY_STAGE_EDITOR = Keys.F11;
    public const Keys KEY_RELOAD_STAGE = Keys.R;
    public const Keys KEY_NEXT_STAGE = Keys.M;
    public const Keys KEY_PREV_STAGE = Keys.N;
    public const Keys KEY_SHOW_HIDE_DEBUG_INFO = Keys.D;
    public const Keys KEY_ENABLE_AVERAGES = Keys.A;
    public const Keys KEY_DEBUG_INFO_POSITION = Keys.I;
    public const Keys KEY_GENERATE_ALL_THUMBS = Keys.G;
    public const Keys KEY_GENERATE_CURRENT_THUMB = Keys.H;

    public DebugOptionsInput.GameHelpButtons HelpButtons { get; private set; }

    public bool IsHelpButtonSet(DebugOptionsInput.GameHelpButtons help)
    {
      return (this.HelpButtons & help) == help;
    }

    public override void Update(BrainGameTime gameTime)
    {
      this.HelpButtons = DebugOptionsInput.GameHelpButtons.None;
      if (InputBase._gamepad != null)
      {
        if (InputBase._gamepad.IsButtonClicked(Buttons.Start))
          this.HelpButtons |= DebugOptionsInput.GameHelpButtons.HideDebugOptionsInput;
        if (InputBase._gamepad.IsButtonClicked(Buttons.A))
          this.HelpButtons |= DebugOptionsInput.GameHelpButtons.ShowBoundingBoxes;
        if (InputBase._gamepad.IsButtonClicked(Buttons.DPadUp))
          this.HelpButtons |= DebugOptionsInput.GameHelpButtons.ShowHidePaths;
        if (InputBase._gamepad.IsButtonClicked(Buttons.DPadDown))
          this.HelpButtons |= DebugOptionsInput.GameHelpButtons.ShowTiles;
        if (InputBase._gamepad.IsButtonClicked(Buttons.DPadLeft))
          this.HelpButtons |= DebugOptionsInput.GameHelpButtons.ShowHideSprites;
        if (InputBase._gamepad.IsButtonClicked(Buttons.DPadRight))
          this.HelpButtons |= DebugOptionsInput.GameHelpButtons.ShowHideQuatree;
        if (InputBase._gamepad.IsButtonClicked(Buttons.B))
          this.HelpButtons |= DebugOptionsInput.GameHelpButtons.ShowDebugInfo;
        if (InputBase._gamepad.IsButtonClicked(Buttons.Y))
          this.HelpButtons |= DebugOptionsInput.GameHelpButtons.EnableAverage;
        if (InputBase._gamepad.IsButtonClicked(Buttons.X))
          this.HelpButtons |= DebugOptionsInput.GameHelpButtons.ReloadStage;
        if (InputBase._gamepad.IsButtonClicked(Buttons.RightTrigger))
          this.HelpButtons |= DebugOptionsInput.GameHelpButtons.NextStage;
        if (InputBase._gamepad.IsButtonClicked(Buttons.LeftTrigger))
          this.HelpButtons |= DebugOptionsInput.GameHelpButtons.PrevStage;
        if (InputBase._gamepad.IsButtonClicked(Buttons.RightShoulder))
          this.HelpButtons |= DebugOptionsInput.GameHelpButtons.DebugInfoChangePosition;
      }
      if (InputBase._keyboard == null)
        return;
      if (InputBase._keyboard.IsKeyPressed(Keys.F9))
        this.HelpButtons |= DebugOptionsInput.GameHelpButtons.HideDebugOptionsInput;
      if (InputBase._keyboard.IsKeyPressed(Keys.F2))
        this.HelpButtons |= DebugOptionsInput.GameHelpButtons.ShowHideSprites;
      if (InputBase._keyboard.IsKeyPressed(Keys.T))
        this.HelpButtons |= DebugOptionsInput.GameHelpButtons.ShowTiles;
      if (InputBase._keyboard.IsKeyPressed(Keys.P))
        this.HelpButtons |= DebugOptionsInput.GameHelpButtons.ShowHidePaths;
      if (InputBase._keyboard.IsKeyPressed(Keys.Q))
        this.HelpButtons |= DebugOptionsInput.GameHelpButtons.ShowHideQuatree;
      if (InputBase._keyboard.IsKeyPressed(Keys.B))
        this.HelpButtons |= DebugOptionsInput.GameHelpButtons.ShowBoundingBoxes;
      if (InputBase._keyboard.IsKeyPressed(Keys.D))
        this.HelpButtons |= DebugOptionsInput.GameHelpButtons.ShowDebugInfo;
      if (InputBase._keyboard.IsKeyPressed(Keys.I))
        this.HelpButtons |= DebugOptionsInput.GameHelpButtons.DebugInfoChangePosition;
      if (InputBase._keyboard.IsKeyPressed(Keys.A))
        this.HelpButtons |= DebugOptionsInput.GameHelpButtons.EnableAverage;
      if (InputBase._keyboard.IsKeyPressed(Keys.F11))
        this.HelpButtons |= DebugOptionsInput.GameHelpButtons.ShowStageEditor;
      if (InputBase._keyboard.IsKeyPressed(Keys.F11))
        this.HelpButtons |= DebugOptionsInput.GameHelpButtons.ShowStageEditor;
      if (InputBase._keyboard.IsKeyPressed(Keys.R))
        this.HelpButtons |= DebugOptionsInput.GameHelpButtons.ReloadStage;
      if (InputBase._keyboard.IsKeyPressed(Keys.M))
        this.HelpButtons |= DebugOptionsInput.GameHelpButtons.NextStage;
      if (InputBase._keyboard.IsKeyPressed(Keys.N))
        this.HelpButtons |= DebugOptionsInput.GameHelpButtons.PrevStage;
      if (InputBase._keyboard.IsKeyPressed(Keys.G))
        this.HelpButtons |= DebugOptionsInput.GameHelpButtons.GenerateAllThumbs;
      if (!InputBase._keyboard.IsKeyPressed(Keys.H))
        return;
      this.HelpButtons |= DebugOptionsInput.GameHelpButtons.GenerateCurrentThumb;
    }

    public override void Reset() => this.HelpButtons = DebugOptionsInput.GameHelpButtons.None;

    [Flags]
    public enum GameHelpButtons
    {
      None = 0,
      ShowHideSprites = 2,
      ShowTiles = 4,
      ShowBoundingBoxes = 8,
      ShowHidePaths = 16, // 0x00000010
      ShowHideQuatree = 32, // 0x00000020
      XXX1 = 64, // 0x00000040
      ShowDebugInfo = 128, // 0x00000080
      EnableAverage = 256, // 0x00000100
      DebugInfoChangePosition = 512, // 0x00000200
      ShowStageEditor = 1024, // 0x00000400
      XXX2 = 2048, // 0x00000800
      ReloadStage = 4096, // 0x00001000
      NextStage = 8192, // 0x00002000
      PrevStage = 16384, // 0x00004000
      HideDebugOptionsInput = 32768, // 0x00008000
      GenerateAllThumbs = 65536, // 0x00010000
      GenerateCurrentThumb = 131072, // 0x00020000
    }
  }
}
