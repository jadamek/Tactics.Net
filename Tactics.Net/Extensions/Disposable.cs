﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tactics.Net.Extensions
{
    //========================================================================================================================
    // ** IDisposable Extension
    //========================================================================================================================
    public class Disposable : IDisposable
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Dispose And Notify (Implementation)
        //--------------------------------------------------------------------------------------------------------------------
        // Extension which connects Dispose to the 'Disposed' event. Meant to be extended for actual
        // cleanup logic
        //--------------------------------------------------------------------------------------------------------------------
        public virtual void Dispose()
        {
            OnDisposal();
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Fire Disposed Event
        //--------------------------------------------------------------------------------------------------------------------
        protected virtual void OnDisposal()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        // Members - Events
        public event EventHandler<EventArgs> Disposed;
    }
}
