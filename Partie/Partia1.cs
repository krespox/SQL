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
    public partial class Partia1 : Form
    {
        private string path = "c:\\temp\\";
        public Partia1()
        {
            InitializeComponent();
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView1, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView2, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView3, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView4, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView5, new object[] { true });
            //dateTimePicker1.Value = DateTime.Today;
            tbPartia.Text = "19/05/20/GS";
            tbKod.Text = "003-144";
            BindGrid();
            BindGrid2();
            BindGrid3();
            BindGrid4();
            BindGrid5();
        }

        private void Partia1_Load(object sender, EventArgs e)
        {

        }

        private void BindGrid()
        {
            string connString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(Query.Query.Query_HM_MG_A(tbPartia.Text, tbKod.Text), con);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGridView1.DataSource = dt;
                con.Close();
            }
        }

        private void BindGrid2()
        {
            string connString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(Query.Query.Query_PZ_Table(tbPartia.Text, tbKod.Text) + Query.Query.Query_Maynowanie_Table(tbPartia.Text, tbKod.Text) + Query.Query.Query_PP_WP(tbPartia.Text, tbKod.Text), con);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGridView2.DataSource = dt;
                con.Close();
            }
        }

        private void BindGrid3()
        {
            string connString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(Query.Query.Query_PZ_Table(tbPartia.Text, tbKod.Text) + Query.Query.Query_Maynowanie_Table(tbPartia.Text, tbKod.Text) + Query.Query.Query_Ryba_Table(tbPartia.Text, tbKod.Text) + Query.Query.Query_WP_Ryba(tbPartia.Text, tbKod.Text), con);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGridView3.DataSource = dt;
                con.Close();
            }
        }

        private void BindGrid4()
        {
            string connString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(Query.Query.Query_ZlPWG_Table(tbPartia.Text, tbKod.Text) + Query.Query.Query_PP_WyrobGotowy(tbPartia.Text, tbKod.Text), con);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGridView4.DataSource = dt;
                con.Close();
            }
        }

        private void BindGrid5()
        {
            string connString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(Query.Query.Query_ZlPWG_Table(tbPartia.Text, tbKod.Text) + Query.Query.Query_ZLPWZ_Table(tbPartia.Text, tbKod.Text) + Query.Query.Query_WZ_Odbiorca(tbPartia.Text, tbKod.Text), con);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGridView5.DataSource = dt;
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

            DataTable dt2 = new DataTable(); // create a table for storing selected rows
            var dtTemp2 = dataGridView2.DataSource as DataTable; // get the source table object
            dt2 = dtTemp2.Clone();  // clone the schema of the source table to new table

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (dataGridView2.Rows[i].Selected)
                {
                    var row = dt2.NewRow();  // create a new row with the schema 
                    for (int j = 0; j < dataGridView2.Columns.Count; j++)
                    {
                        row[j] = dataGridView2[j, i].Value;
                    }
                    dt2.Rows.Add(row);  // add rows to the new table
                }
            }

            DataTable dt3 = new DataTable(); // create a table for storing selected rows
            var dtTemp3 = dataGridView3.DataSource as DataTable; // get the source table object
            dt3 = dtTemp3.Clone();  // clone the schema of the source table to new table

            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                if (dataGridView3.Rows[i].Selected)
                {
                    var row = dt3.NewRow();  // create a new row with the schema 
                    for (int j = 0; j < dataGridView3.Columns.Count; j++)
                    {
                        row[j] = dataGridView3[j, i].Value;
                    }
                    dt3.Rows.Add(row);  // add rows to the new table
                }
            }

            DataTable dt4 = new DataTable(); // create a table for storing selected rows
            var dtTemp4 = dataGridView4.DataSource as DataTable; // get the source table object
            dt4 = dtTemp4.Clone();  // clone the schema of the source table to new table

            for (int i = 0; i < dataGridView4.Rows.Count; i++)
            {
                if (dataGridView4.Rows[i].Selected)
                {
                    var row = dt4.NewRow();  // create a new row with the schema 
                    for (int j = 0; j < dataGridView4.Columns.Count; j++)
                    {
                        row[j] = dataGridView4[j, i].Value;
                    }
                    dt4.Rows.Add(row);  // add rows to the new table
                }
            }

            DataTable dt5 = new DataTable(); // create a table for storing selected rows
            var dtTemp5 = dataGridView5.DataSource as DataTable; // get the source table object
            dt5 = dtTemp5.Clone();  // clone the schema of the source table to new table

            for (int i = 0; i < dataGridView5.Rows.Count; i++)
            {
                if (dataGridView5.Rows[i].Selected)
                {
                    var row = dt5.NewRow();  // create a new row with the schema 
                    for (int j = 0; j < dataGridView5.Columns.Count; j++)
                    {
                        row[j] = dataGridView5[j, i].Value;
                    }
                    dt5.Rows.Add(row);  // add rows to the new table
                }
            }

            FileInfo f = new FileInfo(path + "partia_" + tbPartia.Text.Replace("/", "") + "_" + tbKod.Text.Replace("-", "") + ".xlsx");
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
                var worksheet2 = excelFile.Workbook.Worksheets.Add("Arkusz2");
                worksheet2.Cells["A1"].LoadFromDataTable(dt2, PrintHeaders: true);
                worksheet2.Cells.AutoFitColumns(0);
                var worksheet3 = excelFile.Workbook.Worksheets.Add("Arkusz3");
                worksheet3.Cells["A1"].LoadFromDataTable(dt3, PrintHeaders: true);
                worksheet3.Cells.AutoFitColumns(0);
                var worksheet4 = excelFile.Workbook.Worksheets.Add("Arkusz4");
                worksheet4.Cells["A1"].LoadFromDataTable(dt4, PrintHeaders: true);
                worksheet4.Cells.AutoFitColumns(0);
                var worksheet5 = excelFile.Workbook.Worksheets.Add("Arkusz5");
                worksheet5.Cells["A1"].LoadFromDataTable(dt5, PrintHeaders: true);
                worksheet5.Cells.AutoFitColumns(0);
                excelFile.Save();
            }
            MessageBox.Show(" XLSX OK");
        }

        private void btnRef_Click(object sender, EventArgs e)
        {
            BindGrid();
            BindGrid2();
            BindGrid3();
            BindGrid4();
            BindGrid5();
        }
    }
}
