using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace S4M.Receptury
{
    public partial class ReceptEwid : Form
    {
        public ReceptEwid()
        {
            InitializeComponent();
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView1, new object[] { true });
            //dateTimePicker1.Value = DateTime.Today;
            
            BindGrid();
        }

        private void ReceptEwid_Load(object sender, EventArgs e)
        {

        }

        private void BindGrid()
        {
            string connString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(Query.Query.Query_MF_PR(""), con);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGridView1.DataSource = dt;
                con.Close();
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę wybrać z listy.");
                return;
            }

            string id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string na = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            var form = new ReceptOper(id, na);
            form.ShowDialog();
            form.Dispose();
        }
    }
}
