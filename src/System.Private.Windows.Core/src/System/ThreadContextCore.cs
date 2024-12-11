// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Private.Windows.Core.Resources;
using Windows.Win32;
using Windows.Win32.OLE;

namespace System;

internal static class ThreadContextCore
{
    private static bool s_oleInitialized;
    private static bool s_isSta;

    /// <summary>
    ///  returns true if the thread was successfully initialized for STA. Otherwise false.
    /// </summary>
    internal static unsafe bool EnsureOleInitializedSta()
    {
        if (!s_oleInitialized)
        {
            HRESULT hr = PInvokeCore.OleInitialize(pvReserved: (void*)null);

            s_oleInitialized = true;
            // Other result we could get is HRESULT.OLE_E_WRONGCOMPOBJ .. Should we throw here or just consider it MTA?
            // We had originally considered this "success" which seems wrong.
             s_isSta = hr.Succeeded;

            /*// HRESULT.RPC_E_CHANGED_MODE could happen if the thread was already initialized for MTA
            // and then we call OleInitialize which tries to initialize it for STA.
            // This currently happens while profiling.
            return hr != HRESULT.RPC_E_CHANGED_MODE;*/
        }

        return s_isSta;
    }

    /// <summary>
    ///  Returns true for successful shutdown, otherwise false.
    /// </summary>
    internal bool ShutdownOle(object sender, EventArgs args)
    {
        if (s_oleInitialized && s_isSta)
        {
            s_oleInitialized = false;
            PInvokeCore.OleUninitialize();
            return true;
        }

        return false;
    }

    internal void ThrowIfNotSta()
    {
        if (!s_isSta)
        {
            throw new ThreadStateException(SR.ThreadMustBeSTA);
        }
    }
}
