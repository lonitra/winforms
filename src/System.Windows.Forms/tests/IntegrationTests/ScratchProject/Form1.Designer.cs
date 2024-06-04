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
        copyButton = new Button();
        pasteButton = new Button();
        beingDrag = new Button();
        SuspendLayout();
        // 
        // copyButton
        // 
        copyButton.Location = new Point(12, 12);
        copyButton.Name = "copyButton";
        copyButton.Size = new Size(75, 23);
        copyButton.TabIndex = 0;
        copyButton.Text = "Copy";
        copyButton.UseVisualStyleBackColor = true;
        copyButton.Click += copyButton_Click;
        // 
        // pasteButton
        // 
        pasteButton.Location = new Point(12, 89);
        pasteButton.Name = "pasteButton";
        pasteButton.Size = new Size(75, 23);
        pasteButton.TabIndex = 1;
        pasteButton.Text = "Paste";
        pasteButton.UseVisualStyleBackColor = true;
        pasteButton.Click += pasteButton_Click;
        // 
        // beingDrag
        // 
        beingDrag.Location = new Point(517, 12);
        beingDrag.Name = "beingDrag";
        beingDrag.Size = new Size(75, 23);
        beingDrag.TabIndex = 2;
        beingDrag.Text = "BeginDrag";
        beingDrag.UseVisualStyleBackColor = true;
        beingDrag.MouseDown += beginDrag_MouseDown;
        // 
        // Form1
        // 
        AllowDrop = true;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(beingDrag);
        Controls.Add(pasteButton);
        Controls.Add(copyButton);
        Name = "Form1";
        Text = "Form1";
        DragDrop += Form1_DragDrop;
        DragOver += Form1_DragOver;
        ResumeLayout(false);
    }

    #endregion

    private Button copyButton;
    private Button pasteButton;
    private Button beingDrag;
}
