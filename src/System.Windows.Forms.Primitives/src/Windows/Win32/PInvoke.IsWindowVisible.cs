﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Win32.Foundation;

namespace Windows.Win32
{
    internal static partial class PInvoke
    {
        public static BOOL IsWindowVisible<T>(in T hWnd) where T : IHandle<HWND>
        {
            BOOL result = IsWindowVisible(hWnd.Handle);
            GC.KeepAlive(hWnd);
            return result;
        }
    }
}
