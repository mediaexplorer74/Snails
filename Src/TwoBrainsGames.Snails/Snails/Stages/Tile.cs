
// Type: TwoBrainsGames.Snails.Stages.Tile
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;


namespace TwoBrainsGames.Snails.Stages
{
  public class Tile : Object2D, ISnailsDataFileSerializable, IDataFileSerializable
  {
    public const float TILE_LAYER_DEPTH = 0.2f;
    public PathBehaviour _leftPath;
    public PathBehaviour _topPath;
    public PathBehaviour _rightPath;
    public PathBehaviour _bottomPath;
    public WalkFlags _walkFlags;
    public string _id;
    public string _contentManagerId;

    public int StyleGroupId { get; set; }

    private bool Breakable { get; set; }

    public bool DrawUnderWater { get; set; }

    public ThemeType ValidThemes { get; set; }

    public Tile()
    {
      this.ResourceId = "TILES";
      this.SpriteId = "Tiles";
      this.BlendColor = Color.White;
      this.Breakable = true;
      this._contentManagerId = "__TEMPORARY__";
      this.LayerDepth = 0.2f;
    }

    public Tile(Tile other) => this.Copy(other);

    public virtual void Copy(Tile other)
    {
      this.Copy((Object2D) other);
      Tile tile = other;
      this._walkFlags = tile._walkFlags;
      this._leftPath = tile._leftPath;
      this._topPath = tile._topPath;
      this._rightPath = tile._rightPath;
      this._bottomPath = tile._bottomPath;
      this.Breakable = tile.Breakable;
      this.BlendColor = tile.BlendColor;
    }

    public static Tile Empty()
    {
      return new Tile() { Breakable = false };
    }

    public virtual void LoadContent()
    {
      this.Sprite = BrainGame.ResourceManager.GetSprite(this.ResourceId + "/" + this.SpriteId, this._contentManagerId);
    }

    public virtual void Update(GameTime gameTime)
    {
    }

    public virtual void Draw(Vector2 pos)
    {
      if (this.Sprite == null)
        return;
      this.Sprite.Draw(pos, this.CurrentFrame, 0.0f, this.SpriteEffect, 0.2f, this.BlendColor, 1f, Stage.CurrentStage.SpriteBatch);
    }

    public void DrawShadow(Vector2 pos)
    {
      this.Sprite.Draw(pos + GenericConsts.TilesShadowDepth, this.CurrentFrame, 0.0f, this.SpriteEffect, 0.2f, Levels.CurrentThemeSettings._shadowColor, 1f, Stage.CurrentStage.SpriteBatch);
    }

    public bool IsBreakable => this.Breakable;

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.StyleGroupId = record.GetFieldValue<int>("styleGroupId", 0);
      this.Breakable = record.GetFieldValue<bool>("breakable", this.Breakable);
      this.DrawUnderWater = record.GetFieldValue<bool>("drawUnderWater", this.DrawUnderWater);
      this._id = record.GetFieldValue<string>("id");
      this.BlendColor = record.GetFieldValue<Color>("color", this.BlendColor);
      this._leftPath = (PathBehaviour) Enum.Parse(typeof (PathBehaviour), record.GetFieldValue<string>("leftPath", PathBehaviour.None.ToString()), false);
      this._topPath = (PathBehaviour) Enum.Parse(typeof (PathBehaviour), record.GetFieldValue<string>("topPath", PathBehaviour.None.ToString()), false);
      this._rightPath = (PathBehaviour) Enum.Parse(typeof (PathBehaviour), record.GetFieldValue<string>("rightPath", PathBehaviour.None.ToString()), false);
      this._bottomPath = (PathBehaviour) Enum.Parse(typeof (PathBehaviour), record.GetFieldValue<string>("bottomPath", PathBehaviour.None.ToString()), false);
      if (this._leftPath != PathBehaviour.None)
        this._walkFlags |= WalkFlags.Left;
      if (this._topPath != PathBehaviour.None)
        this._walkFlags |= WalkFlags.Top;
      if (this._rightPath != PathBehaviour.None)
        this._walkFlags |= WalkFlags.Right;
      if (this._bottomPath != PathBehaviour.None)
        this._walkFlags |= WalkFlags.Bottom;
      this._walkFlags |= (WalkFlags) Enum.Parse(typeof (WalkFlags), record.GetFieldValue<string>("walkFlags", WalkFlags.None.ToString()), false);
      this.ValidThemes = (ThemeType) Enum.Parse(typeof (ThemeType), record.GetFieldValue<string>("theme", ThemeType.All.ToString()), false);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord();
      dataFileRecord.Name = nameof (Tile);
      switch (context)
      {
        case ToDataFileRecordContext.StageDataSave:
          dataFileRecord.AddField("leftPath", (object) ((int) this._leftPath).ToString());
          dataFileRecord.AddField("topPath", (object) ((int) this._topPath).ToString());
          dataFileRecord.AddField("rightPath", (object) ((int) this._rightPath).ToString());
          dataFileRecord.AddField("bottomPath", (object) ((int) this._bottomPath).ToString());
          dataFileRecord.AddField("walkFlags", (object) this._walkFlags.ToString());
          dataFileRecord.AddField("breakable", (object) this.Breakable);
          dataFileRecord.AddField("drawUnderWater", (object) this.DrawUnderWater);
          dataFileRecord.AddField("styleGroupId", (object) this.StyleGroupId);
          dataFileRecord.AddField("theme", (object) this.ValidThemes.ToString());
          if (this.BlendColor != Color.White)
          {
            dataFileRecord.AddField("color", (object) this.BlendColor);
            break;
          }
          break;
        case ToDataFileRecordContext.StageSave:
          dataFileRecord.RemoveField("res");
          dataFileRecord.RemoveField("sprite");
          dataFileRecord.RemoveField("frame");
          break;
      }
      return dataFileRecord;
    }
  }
}
