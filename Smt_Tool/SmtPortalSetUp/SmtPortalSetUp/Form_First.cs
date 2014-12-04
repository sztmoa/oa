using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmtPortalSetUp
{
    public partial class Form_First : Form
    {
        public Form_First()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            Form_Second form = new Form_Second();
            form.Show();
            this.Hide();
        }
    }
}
