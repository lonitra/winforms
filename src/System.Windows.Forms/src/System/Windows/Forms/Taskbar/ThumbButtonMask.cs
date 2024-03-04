// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Windows.Forms;

[Flags]
public enum ThumbButtonMask : byte
{
    /// <inheritdoc cref="THUMBBUTTONMASK.THB_BITMAP"/>
    Bitmap = THUMBBUTTONMASK.THB_BITMAP,

    /// <inheritdoc cref="THUMBBUTTONMASK.THB_ICON"/>
    Icon = THUMBBUTTONMASK.THB_ICON,

    /// <inheritdoc cref="THUMBBUTTONMASK.THB_TOOLTIP"/>
    Tooltip = THUMBBUTTONMASK.THB_TOOLTIP,

    /// <inheritdoc cref="THUMBBUTTONMASK.THB_FLAGS"/>
    Flags = THUMBBUTTONMASK.THB_FLAGS,
}
