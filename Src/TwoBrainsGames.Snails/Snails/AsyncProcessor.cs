
// Type: TwoBrainsGames.Snails.AsyncProcessor
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System;
using System.Collections.Generic;
using System.Threading;


namespace TwoBrainsGames.Snails
{
  internal class AsyncProcessor
  {
    private Thread _processorThread;

    public List<IAsyncOperation> Operations { get; private set; }

    //RnD
    public bool IsLoading => this._processorThread != null && this._processorThread.IsAlive;

    public Exception ExceptionThrown { get; private set; }

    public AsyncProcessor() => this.Operations = new List<IAsyncOperation>();

    private void ProcessorThreadEntryPoint(object param)
    {
      AsyncProcessor asyncProcessor = (AsyncProcessor) param;
      try
      {
        foreach (IAsyncOperation operation in this.Operations)
          operation.BeginLoad();
      }
      catch (Exception ex)
      {
        asyncProcessor.ExceptionThrown = ex;
      }
    }

    public void Begin()
    {
      this.ExceptionThrown = (Exception) null;
      //RnD
      this._processorThread = new Thread(new ParameterizedThreadStart(this.ProcessorThreadEntryPoint));
      this._processorThread.Start((object) this);
    }
  }
}
