namespace Playbook
{
    partial class PicForm
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
        private void InitializeComponent()
        {
            this.imgBox = new System.Windows.Forms.PictureBox();
            this.closeBtn = new Playbook.RoundedBtn();
            ((System.ComponentModel.ISupportInitialize)(this.imgBox)).BeginInit();
            this.SuspendLayout();
            // 
            // imgBox
            // 
            this.imgBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgBox.Location = new System.Drawing.Point(11, 10);
            this.imgBox.Name = "imgBox";
            this.imgBox.Size = new System.Drawing.Size(480, 320);
            this.imgBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgBox.TabIndex = 0;
            this.imgBox.TabStop = false;
            // 
            // closeBtn
            // 
            this.closeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(123)))), ((int)(((byte)(88)))));
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeBtn.ForeColor = System.Drawing.Color.White;
            this.closeBtn.Location = new System.Drawing.Point(384, 16);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(101, 35);
            this.closeBtn.TabIndex = 1;
            this.closeBtn.Text = "Close";
            this.closeBtn.UseVisualStyleBackColor = true;
            this.closeBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // PicForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(246)))), ((int)(((byte)(235)))));
            this.ClientSize = new System.Drawing.Size(497, 338);
            this.ControlBox = false;
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.imgBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PicForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Drawing";
            ((System.ComponentModel.ISupportInitialize)(this.imgBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox imgBox;
        private RoundedBtn closeBtn;
    }
}