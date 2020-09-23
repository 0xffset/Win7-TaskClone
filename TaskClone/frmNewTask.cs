using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskClone.src;
using TaskClone.src.helpers;

namespace TaskClone
{
    
    public partial class frmNewTask : Form
    {
        Applications apps = new Applications();
        SQLite sQLite = new SQLite();
        public frmNewTask()
        {
            InitializeComponent();
        }

        private void stateButtons(bool ok, bool cancel, bool browse) {
            btnOk.Enabled = ok;
            btnCancel.Enabled = cancel;
            btnBrowse.Enabled = browse;
        }

        private void setStoreLog() {
             List<string> items = sQLite.getStoreLog();
            for (int i = 0; i < items.Count; i++) {
                comboBox1.Items.Add(items[i]);
            }
            comboBox1.Text = items[0];
        }   
        private void frmNewTask_Load(object sender, EventArgs e)
        {
            stateButtons(false, true, true);
            setStoreLog();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            stateButtons(true, true, true);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openProgram = new OpenFileDialog();
            openProgram.Filter = "Programs |*.exe";
            openProgram.FilterIndex = 1;
            openProgram.Multiselect = false;

            if (openProgram.ShowDialog() == DialogResult.OK) {
                comboBox1.Text = openProgram.FileName;
               
             }
        }

     private void btnOk_Click(object sender, EventArgs e)
        {
            string task = comboBox1.Text;
            sQLite.storeLog(task);
            if (apps.NewTask(task)) {
                this.Close();
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
