using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Playbook
{
    public partial class PicForm : Form
    {
        public PicForm()
        {
            InitializeComponent();
        }

        public void SetImage(Image img)
        {
            this.imgBox.Image = img;
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
