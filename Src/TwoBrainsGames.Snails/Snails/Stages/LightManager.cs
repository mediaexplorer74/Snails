
// Type: TwoBrainsGames.Snails.Stages.LightManager
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;


namespace TwoBrainsGames.Snails.Stages
{
  public class LightManager
  {
    private List<LightSource> _lightSources;
    private RenderTarget2D _lightMapRenderTarget;
    private BlendState _blendState;
    private BlendState _lightTintblendState;

    public bool LightEnabled { get; set; }

    public Color LightColor { get; set; }

    public LightManager()
    {
      this._lightSources = new List<LightSource>();
      this.LightEnabled = false;
      this.LightColor = new Color(0.0f, 0.0f, 0.0f, 1f);
    }

    public void Initialize()
    {
      this._lightMapRenderTarget = new RenderTarget2D(BrainGame.Graphics, BrainGame.Viewport.Width, BrainGame.Viewport.Height);
      this._blendState = new BlendState();
      this._blendState.AlphaBlendFunction = BlendFunction.ReverseSubtract;
      this._blendState.AlphaSourceBlend = Blend.DestinationAlpha;
      this._blendState.AlphaDestinationBlend = Blend.One;
      this._blendState.ColorBlendFunction = this._blendState.AlphaBlendFunction;
      this._blendState.ColorDestinationBlend = this._blendState.AlphaDestinationBlend;
      this._blendState.ColorSourceBlend = this._blendState.AlphaSourceBlend;
      this._blendState.BlendFactor = Color.White;
      this._blendState.MultiSampleMask = -1;
      this._lightTintblendState = new BlendState();
      this._lightTintblendState.AlphaBlendFunction = BlendFunction.Add;
      this._lightTintblendState.AlphaSourceBlend = Blend.One;
      this._lightTintblendState.AlphaDestinationBlend = Blend.Zero;
      this._lightTintblendState.ColorBlendFunction = this._lightTintblendState.AlphaBlendFunction;
      this._lightTintblendState.ColorDestinationBlend = this._lightTintblendState.AlphaDestinationBlend;
      this._lightTintblendState.ColorSourceBlend = this._lightTintblendState.AlphaSourceBlend;
      this._lightTintblendState.BlendFactor = Color.White;
      this._lightTintblendState.MultiSampleMask = -1;
    }

    public void Unload()
    {
      if (this._lightMapRenderTarget == null)
        return;
      this._lightMapRenderTarget.Dispose();
    }

    public void AddLightSource(LightSource light) => this._lightSources.Add(light);

    public void RemoveLightSource(LightSource light) => this._lightSources.Remove(light);

    public void Update(BrainGameTime gameTime)
    {
      if (!this.LightEnabled)
        return;
      foreach (LightSource lightSource in this._lightSources)
      {
        if (lightSource.IsOn)
          lightSource.Update(gameTime);
      }
      this.CreateMap();
    }

    public void Draw()
    {
      if (!this.LightEnabled)
        return;
      BrainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, (SamplerState) null, (DepthStencilState) null, (RasterizerState) null, (Effect) Stage.CurrentStage.Camera.StageRenderEffect);
      foreach (LightSource lightSource in this._lightSources)
      {
        if (lightSource.IsOn)
          lightSource.DrawTint(BrainGame.SpriteBatch);
      }
      BrainGame.SpriteBatch.End();
      BrainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) BrainGame.RenderEffect);
      BrainGame.SpriteBatch.Draw((Texture2D) this._lightMapRenderTarget, Vector2.Zero, Color.White);
      BrainGame.SpriteBatch.End();
    }

    public void CreateMap()
    {
      BrainGame.Graphics.SetRenderTarget(this._lightMapRenderTarget);
      BrainGame.Graphics.Clear(this.LightColor);
      if (this.LightColor.A == (byte) 0)
      {
        BrainGame.Graphics.SetRenderTarget((RenderTarget2D) null);
        BrainGame.Graphics.Viewport = BrainGame.Viewport;
      }
      else
      {
        BrainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, this._blendState, (SamplerState) null, (DepthStencilState) null, (RasterizerState) null, (Effect) Stage.CurrentStage.Camera.StageRenderEffect);
        foreach (LightSource lightSource in this._lightSources)
        {
          if (lightSource.IsOn)
            lightSource.Draw(BrainGame.SpriteBatch);
        }
        BrainGame.SpriteBatch.End();
        BrainGame.Graphics.SetRenderTarget((RenderTarget2D) null);
        BrainGame.Graphics.Viewport = BrainGame.Viewport;
      }
    }
  }
}
