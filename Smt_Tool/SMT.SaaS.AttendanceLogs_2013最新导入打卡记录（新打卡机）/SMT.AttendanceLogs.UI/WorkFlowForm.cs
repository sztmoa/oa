using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SMT.AttendanceLogs.UI
{
    public partial class WorkFlowForm : Form
    {
        private testEntities entitys = new testEntities();
        public WorkFlowForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void btnGetFlowDefine_Click(object sender, EventArgs e)
        {
            var ents = from ent in entitys.t_flow_definemaser
                       select ent;
            if(ents.Count()>0)
            { 
                dataGridView1.DataSource = ents.ToList();
            }
        }
    }
}
