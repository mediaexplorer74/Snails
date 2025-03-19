
// Type: TwoBrainsGames.Snails.Tutorials.TutorialLine
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.Data.DataFiles;


namespace TwoBrainsGames.Snails.Tutorials
{
  public class TutorialLine
  {
    public List<TutorialItem> Items { get; private set; }

    public Vector2 Position { get; set; }

    public string StCode { get; set; }

    public bool WithCustomPosition => this.Position != Vector2.Zero;

    public TutorialLine() => this.Items = new List<TutorialItem>();

    public void Add(TutorialItem item) => this.Items.Add(item);

    public static TutorialLine CreateFromDataFileRecord(DataFileRecord record)
    {
      TutorialLine fromDataFileRecord = new TutorialLine();
      fromDataFileRecord.InitFromDataFileRecord(record);
      return fromDataFileRecord;
    }

    public void ParseStCode()
    {
      this.Items.Clear();
      TutorialTopicParser.ParseCodeLine(this.StCode, this);
    }

    public void InitFromDataFileRecord(DataFileRecord record)
    {
      this.StCode = record.GetFieldValue<string>("stCode");
      this.Position = record.GetFieldValue<Vector2>("position", Vector2.Zero);
    }

    public DataFileRecord ToDataFileRecord()
    {
      DataFileRecord dataFileRecord = new DataFileRecord("line");
      if (this.WithCustomPosition)
        dataFileRecord.AddField("position", (object) this.Position);
      dataFileRecord.AddField("stCode", (object) this.StCode);
      return dataFileRecord;
    }
  }
}
