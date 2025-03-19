
// Type: TwoBrainsGames.BrainEngine.EnumerationExtensions
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using System;


namespace TwoBrainsGames.BrainEngine
{
    public static class EnumerationExtensions
    {
        public static bool Has<T>(this Enum type, T value) where T : Enum
        {
            try
            {
                return type.HasFlag(value as Enum);
            }
            catch
            {
                return false;
            }
        }

        public static bool Is<T>(this Enum type, T value) where T : Enum
        {
            try
            {
                return type.Equals(value as Enum);
            }
            catch
            {
                return false;
            }
        }

        public static T Add<T>(this Enum type, T value) where T : Enum
        {
            try
            {
                return (T)Enum.ToObject(typeof(T), Convert.ToInt32(type) | Convert.ToInt32(value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Could not append value from enumerated type '{0}'.", typeof(T).Name), ex);
            }
        }

        public static T Remove<T>(this Enum type, T value) where T : Enum
        {
            try
            {
                return (T)Enum.ToObject(typeof(T), Convert.ToInt32(type) & ~Convert.ToInt32(value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Could not remove value from enumerated type '{0}'.", typeof(T).Name), ex);
            }
        }
    }
}
