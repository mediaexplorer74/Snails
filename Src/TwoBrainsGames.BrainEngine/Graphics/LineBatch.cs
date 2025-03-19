
// Type: TwoBrainsGames.BrainEngine.Graphics.LineBatch
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace TwoBrainsGames.BrainEngine.Graphics
{
  public sealed class LineBatch
  {
    private GraphicsDevice graphicsDevice;
    private List<VertexPositionColor> points = new List<VertexPositionColor>();
    private List<short> indices = new List<short>();
    private BasicEffect basicEffect;

    public Matrix WorldTransform
    {
      set => this.basicEffect.World = value;
    }

    public Matrix ViewTransform
    {
      set => this.basicEffect.View = value;
    }

    public LineBatch(GraphicsDevice graphicsDevice, float alpha)
    {
      this.graphicsDevice = graphicsDevice;
      this.basicEffect = new BasicEffect(graphicsDevice);
      this.basicEffect.VertexColorEnabled = true;
      this.basicEffect.Alpha = alpha;
      this.basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0.0f, (float) graphicsDevice.Viewport.Width, (float) graphicsDevice.Viewport.Height, 0.0f, 0.0f, -1f);
      this.basicEffect.View = Matrix.Identity;
      this.basicEffect.World = Matrix.Identity;
    }

    public void Begin()
    {
      this.points.Clear();
      this.indices.Clear();
    }

    public void Batch(Vector2 startPoint, Vector2 endPoint, Color color, float layerDepth)
    {
      this.Batch(startPoint, color, layerDepth);
      this.Batch(endPoint, color, layerDepth);
    }

    public void Batch(
      Vector2 startPoint,
      Color startColor,
      Vector2 endPoint,
      Color endColor,
      float layerDepth)
    {
      this.Batch(startPoint, startColor, layerDepth);
      this.Batch(endPoint, endColor, layerDepth);
    }

    public void Batch(Vector2 point, Color color, float layerDepth)
    {
      this.points.Add(new VertexPositionColor(new Vector3(point, layerDepth), color));
      this.indices.Add((short) this.indices.Count);
    }

    public void End()
    {
      if (this.points.Count <= 0)
        return;
      foreach (EffectPass pass in this.basicEffect.CurrentTechnique.Passes)
      {
        pass.Apply();
        this.graphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, this.points.ToArray(), 0, this.points.Count, this.indices.ToArray(), 0, this.points.Count / 2);
      }
    }
  }
}
