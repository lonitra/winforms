// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

namespace Windows.Win32.Foundation
{
    internal unsafe readonly partial struct PWSTR
    {
        public string? ToStringAndCoTaskMemFree()
        {
            if (Value is null)
            {
                return null;
            }

            string value = new(Value);
            Marshal.FreeCoTaskMem((IntPtr)Value);
            return value;
        }

        public bool IsNull => Value is null;
    }
}
