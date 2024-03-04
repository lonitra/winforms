// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace ScratchProject;

// As we can't currently design in VS in the runtime solution, mark as "Default" so this opens in code view by default.
[DesignerCategory("Default")]
public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
/*        TaskbarItemInfo!.ProgressState = TaskbarItemProgressState.Error;
        TaskbarItemInfo!.ProgressValue = 0.5;
        TaskbarItemInfo.ToolTip = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        TaskbarItemInfo.SetOverlayIcon(SystemIcons.Question, null);
        ThumbButtonInfo thumbButton = new()
        {
            dwMask = ThumbButtonMask.Icon | ThumbButtonMask.Flags | ThumbButtonMask.Tooltip,
            szTip = null,
            hIcon = SystemIcons.Error.Handle,
            dwFlags = ThumbButtonFlags.Enabled,
            iId = 0
        };
        ThumbButtonInfo thumbButton2 = new()
        {
            dwMask = ThumbButtonMask.Icon | ThumbButtonMask.Flags | ThumbButtonMask.Tooltip,
            szTip = null,
            hIcon = SystemIcons.Error.Handle,
            dwFlags = ThumbButtonFlags.DismissOnClick,
            iId = 1
        };

        TaskbarItemInfo.AddThumbButtons(thumbButton, thumbButton2);*/
    }

    /*    private void button1_Click(object sender, EventArgs e)
        {
            ThumbButtonInfo thumbButton = new()
            {
                dwMask = ThumbButtonMask.Icon | ThumbButtonMask.Flags | ThumbButtonMask.Tooltip,
                szTip = null,
                hIcon = SystemIcons.Question.Handle,
            };

    *//*        ThumbButtonInfo thumbButton2 = new()
            {
                dwMask = ThumbButtonMask.Icon | ThumbButtonMask.Flags | ThumbButtonMask.Tooltip,
                szTip = null,
                hIcon = SystemIcons.Question.Handle,
                iId = 1,
            };*//*

           TaskbarItemInfo!.UpdateThumbButtons(thumbButton);
        }*/

    private void add_Click(object sender, EventArgs e)
    {
        ThumbButtonInfo thumbButton = new()
        {
            dwMask = ThumbButtonMask.Icon | ThumbButtonMask.Flags | ThumbButtonMask.Tooltip,
            szTip = null,
            hIcon = SystemIcons.Application.Handle,
            dwFlags = ThumbButtonFlags.Enabled,
            iId = 0
        };
        ThumbButtonInfo thumbButton2 = new()
        {
            dwMask = ThumbButtonMask.Icon | ThumbButtonMask.Flags | ThumbButtonMask.Tooltip,
            szTip = null,
            hIcon = SystemIcons.Application.Handle,
            dwFlags = ThumbButtonFlags.DismissOnClick,
            iId = 1
        };

        TaskbarItemInfo!.AddThumbButtons(thumbButton, thumbButton2);
    }

    private void trackBar1_Scroll(object sender, EventArgs e)
    {
        TaskbarItemInfo!.ProgressValue = ((TrackBar)sender).Value/100.0;
    }

    private void tooltip_Click(object sender, EventArgs e)
    {
        TaskbarItemInfo!.ToolTip = "This is a tooltip!";
    }

    private void overlay_Click(object sender, EventArgs e)
    {
        TaskbarItemInfo.SetOverlayIcon(SystemIcons.Warning, null);
    }

    private void indeterminate_Click(object sender, EventArgs e)
    {
        TaskbarItemInfo!.ProgressState = TaskbarItemProgressState.Indeterminate;
    }

    private void normal_Click(object sender, EventArgs e)
    {
        TaskbarItemInfo!.ProgressState = TaskbarItemProgressState.Normal;
        TaskbarItemInfo!.ProgressValue = 0;
    }

    private void error_Click(object sender, EventArgs e)
    {
        TaskbarItemInfo!.ProgressState = TaskbarItemProgressState.Error;
    }

    private void paused_Click(object sender, EventArgs e)
    {
        TaskbarItemInfo!.ProgressState = TaskbarItemProgressState.Paused;
    }
}
