﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Drawing;
using Windows.Win32.Foundation;

namespace Windows.Win32
{
    internal static partial class PInvoke
    {
        public unsafe static int MapWindowPoints(HWND hWndFrom, HWND hWndTo, ref RECT lpRect)
        {
            fixed (void* p = &lpRect)
            {
                return MapWindowPoints(hWndFrom, hWndTo, (POINT*)p, cPoints: 2);
            }
        }

        public unsafe static int MapWindowPoints<T>(in T hWndFrom, HWND hWndTo, ref RECT lpRect)
            where T : IHandle<HWND>
        {
            fixed (void* p = &lpRect)
            {
                int result = MapWindowPoints(hWndFrom.Handle, hWndTo, (POINT*)p, cPoints: 2);
                GC.KeepAlive(hWndFrom);
                return result;
            }
        }

        public unsafe static int MapWindowPoints<T>(HWND hWndFrom, in T hWndTo, ref RECT lpRect)
            where T : IHandle<HWND>
        {
            fixed (void* p = &lpRect)
            {
                int result = MapWindowPoints(hWndFrom, hWndTo.Handle, (POINT*)p, cPoints: 2);
                GC.KeepAlive(hWndTo);
                return result;
            }
        }

        public unsafe static int MapWindowPoints(HWND hWndFrom, HWND hWndTo, ref Point lpPoint)
        {
            fixed (void* p = &lpPoint)
            {
                int result = MapWindowPoints(hWndFrom, hWndTo, (POINT*)p, cPoints: 1);
                return result;
            }
        }

        public unsafe static int MapWindowPoints<T>(HWND hWndFrom, in T hWndTo, ref Point lpPoint)
            where T : IHandle<HWND>
        {
            fixed (void* p = &lpPoint)
            {
                int result = MapWindowPoints(hWndFrom, hWndTo.Handle, (POINT*)p, cPoints: 1);
                GC.KeepAlive(hWndTo);
                return result;
            }
        }

        public unsafe static int MapWindowPoints<T>(in T hWndFrom, HWND hWndTo, ref Point lpPoint)
            where T : IHandle<HWND>
        {
            fixed (void* p = &lpPoint)
            {
                int result = MapWindowPoints(hWndFrom.Handle, hWndTo, (POINT*)p, cPoints: 1);
                GC.KeepAlive(hWndFrom);
                return result;
            }
        }

        public unsafe static int MapWindowPoints<T>(in T hWndFrom, in T hWndTo, ref Point lpPoint)
            where T : IHandle<HWND>
        {
            fixed (void* p = &lpPoint)
            {
                int result = MapWindowPoints(hWndFrom.Handle, hWndTo.Handle, (POINT*)p, cPoints: 1);
                GC.KeepAlive(hWndFrom);
                GC.KeepAlive(hWndTo);
                return result;
            }
        }
    }
}
