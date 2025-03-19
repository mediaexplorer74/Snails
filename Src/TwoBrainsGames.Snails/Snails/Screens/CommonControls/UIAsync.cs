
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UIAsync
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UIAsync : UIControl
  {
    public AsyncProcessor _asyncProcessor;

    public event UIControl.UIEvent OnAsyncOperationEnded;

    public UIAsync(UIScreen screenOwner)
      : base(screenOwner)
    {
      this._asyncProcessor = new AsyncProcessor();
      this.Enabled = false;
    }

    public void ClearOperations() => this._asyncProcessor.Operations.Clear();

    public void AddOperation(IAsyncOperation operation)
    {
      if (operation == null)
        throw new SnailsException("Async operation cannot be null.");
      this._asyncProcessor.Operations.Add(operation);
    }

    public void StartLoad()
    {
      BrainGame.HddAccessIcon.Visible = true;
      this.Enabled = true;
      this._asyncProcessor.Begin();
    }

    public override void Update(BrainGameTime gameTime)
    {
      if (!this.Enabled || this._asyncProcessor.IsLoading)
        return;
      if (this._asyncProcessor.ExceptionThrown != null)
        throw new SnailsException(this._asyncProcessor.ExceptionThrown.Message, this._asyncProcessor.ExceptionThrown);
      this.Enabled = false;
      BrainGame.HddAccessIcon.Visible = false;
      if (this.OnAsyncOperationEnded == null)
        return;
      this.OnAsyncOperationEnded((IUIControl) this);
    }
  }
}
