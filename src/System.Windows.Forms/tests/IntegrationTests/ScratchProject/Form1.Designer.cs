// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace ScratchProject;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        overlay = new Button();
        add = new Button();
        tooltip = new Button();
        indeterminate = new Button();
        normal = new Button();
        error = new Button();
        paused = new Button();
        trackBar1 = new TrackBar();
        ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
        SuspendLayout();
        // 
        // overlay
        // 
        overlay.Location = new Point(12, 116);
        overlay.Name = "overlay";
        overlay.Size = new Size(93, 23);
        overlay.TabIndex = 0;
        overlay.Text = "Add Overlay";
        overlay.UseVisualStyleBackColor = true;
        overlay.Click += overlay_Click;
        // 
        // add
        // 
        add.Location = new Point(12, 12);
        add.Name = "add";
        add.Size = new Size(93, 23);
        add.TabIndex = 1;
        add.Text = "Add Buttons";
        add.UseVisualStyleBackColor = true;
        add.Click += add_Click;
        // 
        // tooltip
        // 
        tooltip.Location = new Point(12, 62);
        tooltip.Name = "tooltip";
        tooltip.Size = new Size(93, 23);
        tooltip.TabIndex = 2;
        tooltip.Text = "Add Tooltip";
        tooltip.UseVisualStyleBackColor = true;
        tooltip.Click += tooltip_Click;
        // 
        // indeterminate
        // 
        indeterminate.Location = new Point(12, 193);
        indeterminate.Name = "indeterminate";
        indeterminate.Size = new Size(93, 23);
        indeterminate.TabIndex = 3;
        indeterminate.Text = "Indeterminate";
        indeterminate.UseVisualStyleBackColor = true;
        indeterminate.Click += indeterminate_Click;
        // 
        // normal
        // 
        normal.Location = new Point(111, 193);
        normal.Name = "normal";
        normal.Size = new Size(93, 23);
        normal.TabIndex = 4;
        normal.Text = "Normal";
        normal.UseVisualStyleBackColor = true;
        normal.Click += normal_Click;
        // 
        // error
        // 
        error.Location = new Point(210, 193);
        error.Name = "error";
        error.Size = new Size(93, 23);
        error.TabIndex = 5;
        error.Text = "Error";
        error.UseVisualStyleBackColor = true;
        error.Click += error_Click;
        // 
        // paused
        // 
        paused.Location = new Point(309, 193);
        paused.Name = "paused";
        paused.Size = new Size(93, 23);
        paused.TabIndex = 6;
        paused.Text = "Paused";
        paused.UseVisualStyleBackColor = true;
        paused.Click += paused_Click;
        // 
        // trackBar1
        // 
        trackBar1.Location = new Point(12, 260);
        trackBar1.Maximum = 100;
        trackBar1.Name = "trackBar1";
        trackBar1.Size = new Size(291, 45);
        trackBar1.TabIndex = 7;
        trackBar1.Scroll += trackBar1_Scroll;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(trackBar1);
        Controls.Add(paused);
        Controls.Add(error);
        Controls.Add(normal);
        Controls.Add(indeterminate);
        Controls.Add(tooltip);
        Controls.Add(add);
        Controls.Add(overlay);
        Name = "Form1";
        Text = "Form1";
        ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button overlay;
    private Button add;
    private Button tooltip;
    private Button indeterminate;
    private Button normal;
    private Button error;
    private Button paused;
    private TrackBar trackBar1;
}
