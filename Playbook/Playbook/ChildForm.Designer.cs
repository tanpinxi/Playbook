using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace Playbook
{
    partial class ChildForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
            /**
            string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string libPath = exePath + "\\libvlc\\win-x86";
            this.vlcControl.VlcLibDirectory = new System.IO.DirectoryInfo(libPath);
            **/
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChildForm));
            this.vlcControl = new Vlc.DotNet.Forms.VlcControl();
            this.nextVidBtn = new Playbook.RoundedBtn();
            this.storyLabel = new System.Windows.Forms.Label();
            this.exitBtn = new Playbook.RoundedBtn();
            this.imageBtn = new Playbook.RoundedBtn();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl)).BeginInit();
            this.SuspendLayout();
            // 
            // vlcControl
            // 
            this.vlcControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(123)))), ((int)(((byte)(88)))));
            this.vlcControl.Location = new System.Drawing.Point(125, 32);
            this.vlcControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.vlcControl.Name = "vlcControl";
            this.vlcControl.Size = new System.Drawing.Size(800, 600);
            this.vlcControl.Spu = -1;
            this.vlcControl.TabIndex = 0;
            this.vlcControl.Text = "vlcControl";
            string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string libPath = exePath + "\\libvlc\\win-x86";
            this.vlcControl.VlcLibDirectory = new System.IO.DirectoryInfo(libPath);
            this.vlcControl.VlcMediaplayerOptions = null;
            // 
            // nextVidBtn
            // 
            this.nextVidBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(123)))), ((int)(((byte)(88)))));
            this.nextVidBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nextVidBtn.Font = new System.Drawing.Font("Nunito", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextVidBtn.ForeColor = System.Drawing.Color.White;
            this.nextVidBtn.Location = new System.Drawing.Point(910, 55);
            this.nextVidBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nextVidBtn.Name = "nextVidBtn";
            this.nextVidBtn.Size = new System.Drawing.Size(140, 40);
            this.nextVidBtn.TabIndex = 2;
            this.nextVidBtn.Text = "Next Video";
            this.nextVidBtn.UseVisualStyleBackColor = true;
            this.nextVidBtn.Visible = false;
            this.nextVidBtn.Click += new System.EventHandler(this.nextVidBtn_Click);
            // 
            // storyLabel
            // 
            this.storyLabel.Font = new System.Drawing.Font("Nunito", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storyLabel.Location = new System.Drawing.Point(-2, 638);
            this.storyLabel.Name = "storyLabel";
            this.storyLabel.Size = new System.Drawing.Size(1052, 115);
            this.storyLabel.TabIndex = 4;
            this.storyLabel.Text = "Waiting for connection...\r\n.";
            this.storyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // exitBtn
            // 
            this.exitBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(123)))), ((int)(((byte)(88)))));
            this.exitBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitBtn.Font = new System.Drawing.Font("Nunito", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitBtn.ForeColor = System.Drawing.Color.White;
            this.exitBtn.Location = new System.Drawing.Point(910, 742);
            this.exitBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(140, 40);
            this.exitBtn.TabIndex = 5;
            this.exitBtn.Text = "Exit";
            this.exitBtn.UseVisualStyleBackColor = true;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // imageBtn
            // 
            this.imageBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(123)))), ((int)(((byte)(88)))));
            this.imageBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.imageBtn.Font = new System.Drawing.Font("Nunito", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.imageBtn.ForeColor = System.Drawing.Color.White;
            this.imageBtn.Location = new System.Drawing.Point(910, 11);
            this.imageBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.imageBtn.Name = "imageBtn";
            this.imageBtn.Size = new System.Drawing.Size(140, 40);
            this.imageBtn.TabIndex = 6;
            this.imageBtn.Text = "Send Image";
            this.imageBtn.UseVisualStyleBackColor = true;
            this.imageBtn.Visible = false;
            this.imageBtn.Click += new System.EventHandler(this.imageBtn_Click);
            // 
            // ChildForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(246)))), ((int)(((byte)(235)))));
            this.ClientSize = new System.Drawing.Size(1062, 793);
            this.ControlBox = false;
            this.Controls.Add(this.exitBtn);
            this.Controls.Add(this.imageBtn);
            this.Controls.Add(this.storyLabel);
            this.Controls.Add(this.nextVidBtn);
            this.Controls.Add(this.vlcControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ChildForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Playbook Portal";
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private Vlc.DotNet.Forms.VlcControl vlcControl;
        private System.Windows.Forms.Label storyLabel;
        private RoundedBtn nextVidBtn;
        private RoundedBtn exitBtn;
        private RoundedBtn imageBtn;
    }
}

