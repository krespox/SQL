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

namespace S4M.Partie
{
    public partial class ZlecProdMagDok : Form
    {
        private string path = "c:\\temp\\";
        private string data = "";
        public ZlecProdMagDok()
        {
            InitializeComponent();
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView1, new object[] { true });
            tbPartia.Text = "ZLPM003354/2023";
            tbKod.Text = "100-017";
            dateTimePicker1.Value = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime da = dateTimePicker1.Value;
            data = String.Format("{0}-{1}-{2}", da.Year.ToString("0000"), da.Month.ToString("00"), da.Day.ToString("00"));
            BindGrid();
        }

        private void ZlecProdMagDok_Load(object sender, EventArgs e)
        {

        }

        private void BindGrid()
        {
            string connString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(Query.Query.Query_ZLP_DokMag(tbPartia.Text, tbKod.Text, data), con);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGridView1.DataSource = dt;
                lIlosc.Text = dt.Rows.Count.ToString("0");
                con.Close();
            }
        }

        private void btbExpXLS_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę wybrać z listy.");
                return;
            }

            DataTable dt = new DataTable(); // create a table for storing selected rows
            var dtTemp = dataGridView1.DataSource as DataTable; // get the source table object
            dt = dtTemp.Clone();  // clone the schema of the source table to new table

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Selected)
                {
                    var row = dt.NewRow();  // create a new row with the schema 
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        row[j] = dataGridView1[j, i].Value;
                    }
                    dt.Rows.Add(row);  // add rows to the new table
                }
            }

            FileInfo f = new FileInfo(path + "zlecprodmagdok_" + tbPartia.Text.Replace("/", "") + "_" + tbKod.Text.Replace("-", "") + ".xlsx");
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
               ;
                excelFile.Save();
            }
            MessageBox.Show(" XLSX OK");
        }

        private void btnRef_Click(object sender, EventArgs e)
        {
            DateTime da = dateTimePicker1.Value;
            data = String.Format("{0}-{1}-{2}", da.Year.ToString("0000"), da.Month.ToString("00"), da.Day.ToString("00"));
            BindGrid();
        }
    }
}
