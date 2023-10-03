using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace S4M.Spedycja
{
    public partial class Spedycja : Form
    {
        private string data = "";
        private string path = "c:\\temp\\";
        private DataTable dx;
        public Spedycja()
        {
            InitializeComponent();
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView1, new object[] { true });
            dateTimePicker1.Value = DateTime.Today;
            BindGrid();
        }

        private void Spedycja_Load(object sender, EventArgs e)
        {
            data = DateTime.Today.ToShortDateString();
        }

        private void BindGrid()
        {
            string connString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(Query.Query.Query_HM_ZO_Sped(data), con);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dx = dt.Copy();
                while (dt.Columns.Count > 40)
                {
                    dt.Columns.RemoveAt(40);
                }
                dataGridView1.DataSource = dt;
                con.Close();
            }
        }

        private void btnExpXLS_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable(); // create a table for storing selected rows
            dt = dx.Clone();  // clone the schema of the source table to new table
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Selected)
                {
                    var row = dt.NewRow();  // create a new row with the schema 
                    for (int j = 0; j < dx.Columns.Count; j++)
                    {
                        row[j] = dx.Rows[i].ItemArray[j];
                    }
                    dt.Rows.Add(row);  // add rows to the new table
                }
            }

            FileInfo f = new FileInfo(path + "MIRKOSLUGL.xlsx");
            if (f.Exists)
            {
                f.Delete();  // ensures we create a new workbook
                f = new FileInfo(f.FullName);
            }
            using (var excelFile = new ExcelPackage(f))
            {
                var worksheet = excelFile.Workbook.Worksheets.Add("Arkusz1");
                worksheet.Cells["A1"].LoadFromDataTable(dt, PrintHeaders: true);
                worksheet.Cells.AutoFitColumns(0);
                excelFile.Save();
            }

            MessageBox.Show(" XLSX OK ");
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            data = dateTimePicker1.Value.ToShortDateString();
            BindGrid();
        }
    }
}
