using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace S4M
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        private void Main_Load(object sender, EventArgs e)
        {

        }
        private void btnSymZam_Click(object sender, EventArgs e)
        {
            var form = new Zamowienia.Zamowienia();
            form.ShowDialog();
            form.Dispose();
        }

        private void btnRecept_Click(object sender, EventArgs e)
        {
            var form = new Receptury.ReceptEwid();
            form.ShowDialog();
            form.Dispose();
        }

        private void btnSped_Click(object sender, EventArgs e)
        {
            var form = new Spedycja.Spedycja();
            form.ShowDialog();
            form.Dispose();
        }

        private void btnPartia_Click(object sender, EventArgs e)
        {
            var form = new Partie.Partia1();
            form.ShowDialog();
            form.Dispose();
        }

        private void btnZPMD_Click(object sender, EventArgs e)
        {
            var form = new Partie.ZlecProdMagDok();
            form.ShowDialog();
            form.Dispose();
        }
    }
}
