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
using System.Xml;

namespace S4M.Zamowienia
{
    public partial class Zamowienia : Form
    {
        private string data = "";
        private string path = "c:\\temp\\";
        public Zamowienia()
        {
            InitializeComponent();
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView1, new object[] { true });
            BindGrid();
        }

        private void Zamowienia_Load(object sender, EventArgs e)
        {
            data = DateTime.Today.ToShortDateString();
            dateTimePicker1.Value = DateTime.Today;
        }

        private void BindGrid()
        {
            string connString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(Query.Query.Query_HM_ZO(data), con);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGridView1.DataSource = dt;
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    dataGridView1.Columns[i].Visible = false;
                }

                dataGridView1.Columns["id"].Visible = true;
                dataGridView1.Columns["kod"].Visible = true;
                dataGridView1.Columns["aktywny"].Visible = true;
                dataGridView1.Columns["nazwa"].Visible = true;
                dataGridView1.Columns["data"].Visible = true;
                dataGridView1.Columns["khid"].Visible = true;
                dataGridView1.Columns["odid"].Visible = true;
                dataGridView1.Columns["netto"].Visible = true;
                dataGridView1.Columns["netto"].DefaultCellStyle.Format = "F2";
                dataGridView1.Columns["netto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["vat"].Visible = true;
                dataGridView1.Columns["vat"].DefaultCellStyle.Format = "F2";
                dataGridView1.Columns["vat"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["typ_dk"].Visible = true;
                dataGridView1.Columns["waluta"].Visible = true;
                dataGridView1.Columns["walNetto"].Visible = true;
                dataGridView1.Columns["walNetto"].DefaultCellStyle.Format = "F2";
                dataGridView1.Columns["walNetto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                con.Close();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            data = dateTimePicker1.Value.ToShortDateString();
            BindGrid();
        }

        private void btnExpXML_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę wybrać z listy.");
                return;
            }

            int c = 0;

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);
                XmlElement jpk = doc.CreateElement("BAZA");
                doc.AppendChild(jpk);
                XmlNode element;

                XmlElement faktura = doc.CreateElement("Dokument");
                jpk.AppendChild(faktura);

                for (int j = 0; j < dataGridView1.SelectedRows[i].Cells.Count; j++)
                {
                    element = doc.CreateElement(dataGridView1.Columns[j].Name.Replace(" ", "_").Replace("/", "_").Replace("-", "_").Replace("(", "_").Replace(")", "_").Replace(":", "_")); //"E" + j.ToString("000")); //dataGridView1.SelectedRows[i].Cells[j].Tag.ToString());
                    element.AppendChild(doc.CreateTextNode(dataGridView1.SelectedRows[i].Cells[j].Value.ToString()));
                    faktura.AppendChild(element);
                }

                doc.Save(path + dataGridView1.SelectedRows[i].Cells[9].Value.ToString().Replace("/", "").Replace("-", "") + ".xml");
                c = i + 1;
            }

            MessageBox.Show(c.ToString("N0") + " XML OK");
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

            FileInfo f = new FileInfo(path + "hmzo.xlsx");
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
            MessageBox.Show(" XLSX OK");
        }

        private void btnRapZam_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Brak listy.");
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


            var form = new Raporty.RapZam(dt);
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }
    }
}
