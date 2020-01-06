using System;
using System.IO;
using System.Reflection;

namespace Playbook
{
    partial class GPForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GPForm));
            this.vlcControl = new Vlc.DotNet.Forms.VlcControl();
            this.storyLabel = new System.Windows.Forms.Label();
            this.exitBtn = new Playbook.RoundedBtn();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl)).BeginInit();
            this.SuspendLayout();
            // 
            // vlcControl
            // 
            this.vlcControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(123)))), ((int)(((byte)(88)))));
            this.vlcControl.Location = new System.Drawing.Point(125, 20);
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
            // storyLabel
            // 
            this.storyLabel.Font = new System.Drawing.Font("Nunito", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storyLabel.Location = new System.Drawing.Point(-2, 624);
            this.storyLabel.Name = "storyLabel";
            this.storyLabel.Size = new System.Drawing.Size(1052, 139);
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
            // GPForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(246)))), ((int)(((byte)(235)))));
            this.ClientSize = new System.Drawing.Size(1062, 793);
            this.ControlBox = false;
            this.Controls.Add(this.exitBtn);
            this.Controls.Add(this.vlcControl);
            this.Controls.Add(this.storyLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "GPForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Playbook Portal";
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Vlc.DotNet.Forms.VlcControl vlcControl;
        private System.Windows.Forms.Label storyLabel;
        private RoundedBtn exitBtn;
    }
}

