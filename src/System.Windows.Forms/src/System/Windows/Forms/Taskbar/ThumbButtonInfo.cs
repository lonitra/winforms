// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Windows.Forms;

public unsafe struct ThumbButtonInfo
{
    public ThumbButtonMask dwMask;
    public uint iId;
    public uint iBitmap;
    public nint hIcon;
    public string? szTip;
    public ThumbButtonFlags dwFlags;
}
