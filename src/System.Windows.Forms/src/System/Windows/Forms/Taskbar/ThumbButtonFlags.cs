// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Windows.Forms;

[Flags]
public enum ThumbButtonFlags : byte
{
    /// <inheritdoc cref="THUMBBUTTONFLAGS.THBF_ENABLED"/>
    Enabled = THUMBBUTTONFLAGS.THBF_ENABLED,

    /// <inheritdoc cref="THUMBBUTTONFLAGS.THBF_DISABLED"/>
    Disabled = THUMBBUTTONFLAGS.THBF_DISABLED,

    /// <inheritdoc cref="THUMBBUTTONFLAGS.THBF_DISMISSONCLICK"/>
    DismissOnClick = THUMBBUTTONFLAGS.THBF_DISMISSONCLICK,

    /// <inheritdoc cref="THUMBBUTTONFLAGS.THBF_NOBACKGROUND"/>
    NoBackground = THUMBBUTTONFLAGS.THBF_NOBACKGROUND,

    /// <inheritdoc cref="THUMBBUTTONFLAGS.THBF_HIDDEN"/>
    Hidden = THUMBBUTTONFLAGS.THBF_HIDDEN,

    /// <inheritdoc cref="THUMBBUTTONFLAGS.THBF_NONINTERACTIVE"/>
    NonInteractive = THUMBBUTTONFLAGS.THBF_NONINTERACTIVE,
}
