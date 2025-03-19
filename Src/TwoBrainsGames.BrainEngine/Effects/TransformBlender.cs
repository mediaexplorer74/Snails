
// Type: TwoBrainsGames.BrainEngine.Effects.TransformBlender
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace TwoBrainsGames.BrainEngine.Effects
{
  public class TransformBlender
  {
    private List<TransformBlender.EffectData> _EffectList;
    private Vector3 _Position;
    private Vector4 _color;
    public Vector2 _scale;

    public float Rotation { get; set; }

    public Color Color
    {
      get => new Color(this._color);
      set => this._color = value.ToVector4();
    }

    public int Count => this._EffectList.Count;

    public Vector3 Position => this._Position;

    public Vector2 PositionV2 => new Vector2(this._Position.X, this._Position.Y);

    public Vector3 VirtualPosition { get; set; }

    public Vector2 VirtualPositionV2 => new Vector2(this.VirtualPosition.X, this.VirtualPosition.Y);

    public float VirtualRotation { get; set; }

    public bool Active { get; set; }

    public ITransformEffect this[int i] => this._EffectList[i]._Effect;

    public TransformBlender()
    {
      this._EffectList = new List<TransformBlender.EffectData>();
      this.Active = true;
      this._color = new Vector4(1f, 1f, 1f, 1f);
    }

    public void DisableAll()
    {
      foreach (TransformBlender.EffectData effect in this._EffectList)
        effect._Effect.Active = false;
      this._Position = Vector3.Zero;
      this.Rotation = 0.0f;
      this._color = new Vector4(1f, 1f, 1f, 1f);
    }

    public void Clear()
    {
      foreach (TransformBlender.EffectData effect in this._EffectList)
        effect._Deleted = true;
      this._Position = Vector3.Zero;
      this.Rotation = 0.0f;
      this._color = new Vector4(1f, 1f, 1f, 1f);
    }

    public void Add(ITransformEffect effect) => this.Add(effect, 0);

    public void Add(ITransformEffect effect, int id)
    {
      this._EffectList.Add(new TransformBlender.EffectData(id, effect));
    }

    private void DeleteEffect(TransformBlender.EffectData effect)
    {
      effect._Deleted = true;
      effect._Effect.Ended = true;
    }

    public void DeleteEffects(Type type)
    {
      for (int index = 0; index < this._EffectList.Count; ++index)
      {
        if ((object) this._EffectList[index]._Effect.GetType() == (object) type)
          this.DeleteEffect(this._EffectList[index]);
      }
    }

    public void DeleteEffects(int id)
    {
      for (int index = 0; index < this._EffectList.Count; ++index)
      {
        if (this._EffectList[index]._Id == id)
          this.DeleteEffect(this._EffectList[index]);
      }
    }

    public void DeleteEffectsExcept(int id)
    {
      for (int index = 0; index < this._EffectList.Count; ++index)
      {
        if (this._EffectList[index]._Id != id)
          this.DeleteEffect(this._EffectList[index]);
      }
    }

    public void DeleteAllEffects()
    {
      for (int index = 0; index < this._EffectList.Count; ++index)
        this.DeleteEffect(this._EffectList[index]);
    }

    public void Update(BrainGameTime gameTime)
    {
      this._Position = Vector3.Zero;
      this.Rotation = 0.0f;
      this._color = new Vector4(1f, 1f, 1f, 1f);
      this._scale = Vector2.Zero;
      this.VirtualPosition = Vector3.Zero;
      this.VirtualRotation = 0.0f;
      if (this._EffectList.Count == 0 || !this.Active)
        return;
      for (int index = 0; index < this._EffectList.Count; ++index)
      {
        TransformBlender.EffectData effect = this._EffectList[index];
        if (!effect._Deleted && effect._Effect.Active)
        {
          effect._Effect.LastScale = effect._Effect.Scale;
          effect._Effect.InternalUpdate(gameTime);
          this._Position += effect._Effect.Position;
          this.Rotation += effect._Effect.Rotation;
          this._color *= effect._Effect.ColorVector;
          this._scale += effect._Effect.Scale - effect._Effect.LastScale;
          this.VirtualPosition += effect._Effect.VirtualPosition;
          this.VirtualRotation += effect._Effect.VirtualRotation;
          if (effect._Effect.Ended && effect._Effect.AutoDeleteOnEnd)
            effect._Deleted = true;
        }
      }
      this.RemoveDeletedEffects();
    }

    public bool WithActiveEffects
    {
      get
      {
        for (int index = 0; index < this._EffectList.Count; ++index)
        {
          if (!this._EffectList[index]._Deleted && this._EffectList[index]._Effect.Active && !this._EffectList[index]._Effect.Ended)
            return true;
        }
        return false;
      }
    }

    private void RemoveDeletedEffects()
    {
      for (int index = 0; index < this._EffectList.Count; ++index)
      {
        if (this._EffectList[index]._Deleted)
          this._EffectList.RemoveAt(index--);
      }
    }

    public ITransformEffect FindEffect(int id)
    {
      foreach (TransformBlender.EffectData effect in this._EffectList)
      {
        if (effect._Id == id && !effect._Deleted)
          return effect._Effect;
      }
      return (ITransformEffect) null;
    }

    public bool Contains(int id) => this.FindEffect(id) != null;

    private class EffectData
    {
      public int _Id;
      public ITransformEffect _Effect;
      public bool _Deleted;

      public EffectData(int id, ITransformEffect effect)
      {
        this._Id = id;
        this._Effect = effect;
        this._Deleted = false;
      }
    }
  }
}
