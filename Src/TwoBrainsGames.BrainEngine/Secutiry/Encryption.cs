
// Type: TwoBrainsGames.BrainEngine.Secutiry.Encryption
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using System;
using System.IO;


namespace TwoBrainsGames.BrainEngine.Secutiry
{
  public class Encryption
  {
    public static byte[] Encrypt(Stream stream, string key)
    {
      byte[] numArray = new byte[stream.Length];
      int offset = 0;
      int num;
      while ((num = stream.Read(numArray, offset, (int) Math.Min(1024L, stream.Length - (long) offset))) > 0)
        offset += num;
      return Encryption.Encrypt(numArray, key);
    }

    public static byte[] Encrypt(byte[] toEncrypt, string key) => toEncrypt;

    public static byte[] Decrypt(Stream cryptedScream, string key)
    {
      MemoryStream memoryStream = new MemoryStream();
      byte[] buffer1 = new byte[1024];
      int count;
      while ((count = cryptedScream.Read(buffer1, 0, 1024)) > 0)
        memoryStream.Write(buffer1, 0, count);
      memoryStream.Seek(0L, SeekOrigin.Begin);
      byte[] buffer2 = new byte[memoryStream.Length];
      int offset = 0;
      int num;
      while ((num = memoryStream.Read(buffer2, offset, (int) Math.Min(1024L, memoryStream.Length - (long) offset))) > 0)
        offset += num;
      return buffer2;
    }

    public static byte[] Decrypt(byte[] cryptedData, string key) => cryptedData;
  }
}
