
// Type: TwoBrainsGames.BrainEngine.Resources.TextFont
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Data.DataFiles;


namespace TwoBrainsGames.BrainEngine.Resources
{
  public class TextFont : Image, IDataFileSerializable
  {
    private TextFontChar[] Chars { get; set; }

    private int SpaceWidth { get; set; }

    private int CharSpacing { get; set; }

    public int LineHeight { get; private set; }

    private string ImageId { get; set; }

    public Color Opacity { get; set; }

    public TextFont() => this.Opacity = Color.White;

    public override void LoadContent(ContentManager contentManager)
    {
      this.Texture = contentManager.Load<Texture2D>(this.ImageId);
    }

    public override bool Release(ContentManager contentManager) => base.Release(contentManager);

    private void DrawChar(
      SpriteBatch spriteBatch,
      Vector2 position,
      TextFontChar charToPrint,
      Color color,
      object param,
      Vector2 scale)
    {
      Rectangle destinationRectangle = new Rectangle((int) position.X, (int) position.Y, (int) ((double) charToPrint.Rectangle.Width * (double) scale.X), (int) ((double) charToPrint.Rectangle.Height * (double) scale.Y));
      spriteBatch.Draw(this.Texture, destinationRectangle, new Rectangle?(charToPrint.Rectangle), color, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
    }

    public void DrawString(SpriteBatch spriteBatch, string text, Vector2 position)
    {
      this.DrawStringWithCallback(spriteBatch, text, position, Color.White, new TextFont.DrawCharCallback(this.DrawChar), (object) null, new Vector2(1f, 1f));
    }

    public void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Vector2 scale)
    {
      this.DrawStringWithCallback(spriteBatch, text, position, Color.White, new TextFont.DrawCharCallback(this.DrawChar), (object) null, scale);
    }

    public void DrawString(
      SpriteBatch spriteBatch,
      string text,
      Vector2 position,
      Vector2 scale,
      Color color)
    {
      this.DrawStringWithCallback(spriteBatch, text, position, color, new TextFont.DrawCharCallback(this.DrawChar), (object) null, scale);
    }

    public void DrawString(
      SpriteBatch spriteBatch,
      string text,
      Rectangle destRect,
      TextFont.TextHorizontalAlign horizAlign)
    {
      this.DrawStringWithCallback(spriteBatch, text, destRect, horizAlign, Color.White, new TextFont.DrawCharCallback(this.DrawChar), (object) null);
    }

    public void DrawStringWithCallback(
      SpriteBatch spriteBatch,
      string text,
      Rectangle destRect,
      TextFont.TextHorizontalAlign horizAlign,
      Color color,
      TextFont.DrawCharCallback drawCallback,
      object param)
    {
      Vector2 position = new Vector2((float) destRect.X, (float) destRect.Y);
      float num = this.MeasureString(text, new Vector2(1f, 1f));
      switch (horizAlign)
      {
        case TextFont.TextHorizontalAlign.Right:
          position = new Vector2((float) (destRect.X + destRect.Width) - num, (float) destRect.Y);
          break;
        case TextFont.TextHorizontalAlign.Center:
          position = new Vector2((float) (destRect.X + destRect.Width / 2) - num / 2f, (float) destRect.Y);
          break;
      }
      this.DrawStringWithCallback(spriteBatch, text, position, color, drawCallback, param, new Vector2(1f, 1f));
    }

    public void DrawStringWithCallback(
      SpriteBatch spriteBatch,
      string text,
      Vector2 position,
      Color color,
      TextFont.DrawCharCallback drawCallback,
      object param,
      Vector2 scale)
    {
      for (int index = 0; index < text.Length; ++index)
      {
        if (text[index] == ' ')
        {
          position.X += (float) (this.SpaceWidth + this.CharSpacing) * scale.X;
        }
        else
        {
          TextFontChar charToPrint = this.Chars[(int) text[index]];
          if (charToPrint != null)
          {
            if (drawCallback != null)
              drawCallback(spriteBatch, new Vector2(position.X + (float) charToPrint.Spacing, position.Y), charToPrint, color, param, scale);
            position.X += (float) (charToPrint.Rectangle.Width + this.CharSpacing + charToPrint.Spacing + charToPrint.SpacingAfter) * scale.X;
          }
        }
      }
    }

    public float MeasureStringHeight(string text, Vector2 scale)
    {
      float num = 0.0f;
      for (int index = 0; index < text.Length; ++index)
      {
        TextFontChar textFontChar = this.Chars[(int) text[index]];
        if (textFontChar != null && (double) textFontChar.Rectangle.Height > (double) num)
          num = (float) textFontChar.Rectangle.Height;
      }
      return num;
    }

    public float MeasureString(string text) => this.MeasureString(text, new Vector2(1f, 1f));

    public float MeasureString(string text, Vector2 scale)
    {
      float num = 0.0f;
      for (int index = 0; index < text.Length; ++index)
      {
        if (text[index] == ' ')
          num += (float) (this.SpaceWidth + this.CharSpacing);
        else if (this.Chars.Length > (int) text[index])
        {
          TextFontChar textFontChar = this.Chars[(int) text[index]];
          if (textFontChar != null)
          {
            num += (float) textFontChar.Rectangle.Width;
            if (index + 1 < text.Length)
              num += (float) (this.CharSpacing + textFontChar.Spacing + textFontChar.SpacingAfter);
          }
        }
      }
      return num;
    }

    public static TextFont FromDataFileRecord(DataFileRecord record)
    {
      TextFont textFont = new TextFont();
      textFont.InitFromDataFileRecord(record);
      return textFont;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      this.Chars = new TextFontChar[256];
      this.ImageId = record.GetFieldValue<string>("ImageId");
      DataFileRecord dataFileRecord = record.SelectRecord("Characters");
      if (dataFileRecord == null)
        return;
      this.SpaceWidth = dataFileRecord.GetFieldValue<int>("SpaceWidth", 0);
      this.CharSpacing = dataFileRecord.GetFieldValue<int>("BetweenCharsWidth", 0);
      this.LineHeight = dataFileRecord.GetFieldValue<int>("LineHeight", 0);
      foreach (DataFileRecord selectRecord in dataFileRecord.SelectRecords("Character"))
      {
        TextFontChar textFontChar = TextFontChar.FromDataFile(selectRecord);
        this.Chars[(int) textFontChar.Character] = textFontChar;
      }
    }

    public override DataFileRecord ToDataFileRecord()
    {
      DataFileRecord dataFileRecord = new DataFileRecord("Font");
      dataFileRecord.AddField("ImageId", (object) this.ImageId);
      DataFileRecord record = new DataFileRecord("Characters");
      record.AddField("SpaceWidth", (object) this.SpaceWidth);
      record.AddField("BetweenCharsWidth", (object) this.CharSpacing);
      record.AddField("LineHeight", (object) this.LineHeight);
      dataFileRecord.AddRecord(record);
      foreach (TextFontChar textFontChar in this.Chars)
      {
        if (textFontChar != null)
          record.AddRecord(textFontChar.ToDataFileRecord());
      }
      return dataFileRecord;
    }

    public delegate void DrawCharCallback(
      SpriteBatch spriteBatch,
      Vector2 pos,
      TextFontChar charToPrint,
      Color color,
      object param,
      Vector2 scale);

    public enum TextHorizontalAlign
    {
      Left,
      Right,
      Center,
    }

    public enum TextVerticalAlign
    {
      Top,
      Bottom,
      Middle,
    }
  }
}
