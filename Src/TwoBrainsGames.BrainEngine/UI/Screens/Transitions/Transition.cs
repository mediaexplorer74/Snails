
// Type: TwoBrainsGames.BrainEngine.UI.Screens.Transitions.Transition
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using System;


namespace TwoBrainsGames.BrainEngine.UI.Screens.Transitions
{
  public abstract class Transition
  {
    public bool _ended;

    public event EventHandler OnTransitionEnded;

    public virtual void Initialize() => this._ended = false;

    public virtual void LoadContent()
    {
    }

    public virtual void Update(BrainGameTime gameTime)
    {
    }

    public virtual void Draw()
    {
    }

    public virtual void OnStart()
    {
    }

    public virtual void Reset()
    {
    }

    public virtual void TransitionOut()
    {
    }

    protected void InvokeTransitonEnded()
    {
      if (this.OnTransitionEnded == null)
        return;
      this.OnTransitionEnded((object) this, new EventArgs());
    }
  }
}
