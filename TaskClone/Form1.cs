using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskClone.src;
using System.Windows.Forms;
using System.Security;
using System.Windows.Forms.VisualStyles;
using System.Diagnostics;
using TaskClone.src.helpers;
using System.Collections;
using System.ServiceProcess;
using System.Windows.Forms.DataVisualization.Charting;
using Org.Mentalis.Utilities;
using System.Threading;
using Microsoft.VisualBasic.ApplicationServices;

namespace TaskClone
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        bool iscontinue = true;
        private static CpuUsage CPU;
        helpers h = new helpers();
        Applications applications = new Applications();
        Services services = new Services();
        Performance Performance = new Performance();
        Networking Networking = new Networking();
        Users users = new Users();
        PerformanceCounter cpuUsage = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
        PerformanceCounter memoryUsage = new PerformanceCounter("Memory", "Available MBytes");
        public Form1()
        {
            InitializeComponent();

        }

        private void statusButtonsApplication(bool New, bool Switch, bool End) {
            btnNewTask.Enabled = End;
            btnEndTask.Enabled = Switch;
            btnSwitchTo.Enabled = New;
        }

        private void statusButtonProcess(bool end) {
            btnEndProcess.Enabled = end;
        }
        private void loadTasks() {
            Applications apps = new Applications();
            Dictionary<string, string> task = apps.get_task();
            dataGridViewTasks.Rows.Clear();
            foreach (KeyValuePair<string, string> t in task) {
                string[] row = { t.Key, t.Value };
                dataGridViewTasks.Rows.Add(row);

            }
        }

        private void loadProcesses() {
            Processes processes = new Processes();
            Dictionary<string, object> proc = processes.getProcesses();
            foreach (KeyValuePair<string, object> i in proc) {
                string[] values = (string[])i.Value;
                string[] row = { values[0], values[1], values[2], values[3], values[4], values[5] };
                dataGridViewProcesses.Rows.Add(row);
            }
        }

        private void loadNetworkAdapters() {
            Dictionary<string, object> nic = Networking.getNetworkAdapters();
            foreach (KeyValuePair<string, object> i in nic) {
                string[] values = (string[])i.Value;
                string[] row = { i.Key, values[0], values[1], values[2] };
                dataGridViewNetworkAdapters.Rows.Add(row);
            }
        }

        private void loadUser() {
            Dictionary<string, object> u = users.getListuser(); 
        }

        private void loadServices() {
            ServiceController service = new ServiceController();
            Dictionary<string, object> ser = services.getServices();
            foreach (KeyValuePair<string, object> sec in ser) {
                string[] values = (string[])sec.Value;
                string[] row = { values[0], values[1], values[2], values[3] };
                dataGridViewServices.Rows.Add(row);
            }
        }
        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            statusButtonsApplication(false, false, true);
            statusButtonProcess(false);
            loadTasks();
            loadProcesses();
            loadServices();
            GraphCPUUsage();
            loadNetworkAdapters();
            loadUser();
        }

        private void dataGridViewTasks_SelectionChanged(object sender, EventArgs e)
        {
           
            if (h.hasRowSelected(dataGridViewTasks))
            {
                statusButtonsApplication(true, true, true);
            }

            else {
                statusButtonsApplication(false, false, true);
            }
        }

        private void btnEndTask_Click(object sender, EventArgs e)
        {
            if (h.hasRowSelected(dataGridViewTasks)) {
                string processName = dataGridViewTasks.Rows[dataGridViewTasks.CurrentCell.RowIndex].Cells[dataGridViewTasks.CurrentCell.ColumnIndex].Value.ToString();
                h.killProccessApplications(processName.ToString());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabelCpuUsage.Text = (int)cpuUsage.NextValue() + "%";
            toolStripStatusLabelPyshicalMemory.Text = (int)memoryUsage.NextValue() + "MB";
            toolStripStatusLabelProcesses.Text = h.numberOfRunningProcess().ToString();

            lblRAM.Text = Performance.getAmoungMemory();
            lblCached.Text = Performance.getCachedMemory();
            lblAVAILABLE.Text = Performance.getAvailableMemory();
            lblFree.Text = Performance.getFreeMemory();
            progressBarCpuUsage.Value = (int)cpuUsage.NextValue();
           
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            lblHandles.Text = Performance.getHandlesSystemCount();
            lblThreads.Text = Performance.getThreadsSystemCount();
            lblProcesses.Text = Convert.ToString(h.numberOfRunningProcess());
            lblUpTime.Text = Performance.getUpTimeSystem().ToString(@"dd\:hh\:mm\:ss");
            lblCommitSize.Text = Performance.getCommitSize() + "/" + Performance.getAmoungMemory();
            lblPagedMemory.Text = Performance.getPagedKernelMemory();
            
        }

        private void btnNewTask_Click(object sender, EventArgs e)
        {
            frmNewTask frm = new frmNewTask();
            frm.Show();
        }

        private void btnSwitchTo_Click(object sender, EventArgs e)
        {
            if (h.hasRowSelected(dataGridViewTasks))
            {
                string processName = dataGridViewTasks.Rows[dataGridViewTasks.CurrentCell.RowIndex].Cells[dataGridViewTasks.CurrentCell.ColumnIndex].Value.ToString();
                applications.SwitchToApp(processName.ToString());
            }
        }

        private void newTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNewTask frm = new frmNewTask();
            frm.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnEndProcess_Click(object sender, EventArgs e)
        {
            if (h.hasRowSelected(dataGridViewProcesses))
            {
                string pid = dataGridViewProcesses.Rows[dataGridViewProcesses.CurrentCell.RowIndex].Cells[1].Value.ToString();
                h.KillProcessByPid(Convert.ToInt32(pid));
            }
        }

        private void dataGridViewProcesses_SelectionChanged(object sender, EventArgs e)
        {
            if (h.hasRowSelected(dataGridViewProcesses))
            {
                statusButtonProcess(true);
            }

            else
            {
                statusButtonProcess(false);
            }
        }


        private void GraphCPUUsage() {
            //Chart Settings 

            // Populating the data arrays.
         
            this.CpuUsagechart.Series.Clear();
            this.CpuUsagechart.Palette = ChartColorPalette.SeaGreen;

         

            // Add chart series
            Series series = this.CpuUsagechart.Series.Add("CPU Usage");
            

            // Add Initial Point as Zero.
            series.Points.Add(0);

            //Populating X Y Axis  Information \
            CpuUsagechart.Series[0].ChartType = SeriesChartType.Line;
            

            CpuUsagechart.ResetAutoValues();
            CpuUsagechart.ChartAreas[0].AxisY.Maximum = 100;//Max Y 
            CpuUsagechart.ChartAreas[0].AxisY.Minimum = 0;
            CpuUsagechart.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            CpuUsagechart.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
            CpuUsagechart.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;

            CpuUsagechart.ChartAreas[0].BackColor = Color.Black;
            CpuUsagechart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LawnGreen;
            CpuUsagechart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LawnGreen;

            populateCPUInfo();
        }
        private void populateCPUInfo()
        {
            try
            {
                // Creates and returns a CpuUsage instance that can be used to query the CPU time on this operating system.
                CPU = CpuUsage.Create();

                /// Creating a New Thread 
                Thread thread = new Thread(new ThreadStart(delegate ()
                {
                    try
                    {
                        while (iscontinue)
                        {
                            //To Update The UI Thread we have to Invoke  it. 
                            this.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                            {
                                int process = CPU.Query(); //Determines the current average CPU load.
                                //proVal.Text = process.ToString() + "%";
                                CpuUsagechart.Series[0].Points.AddY(process);//Add process to chart 

                                if (CpuUsagechart.Series[0].Points.Count > 40)//clear old data point after Thrad Sleep time * 40
                                    CpuUsagechart.Series[0].Points.RemoveAt(0);

                            }));

                            Thread.Sleep(450);//Thread sleep for 450 milliseconds 
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }));

                thread.Priority = ThreadPriority.Highest;
                thread.IsBackground = true;
                thread.Start();//Start the Thread
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        
    }
}
