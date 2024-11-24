using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crumar
{
    public partial class frm_search : Form
    {
        public frm_search()
        {
            InitializeComponent();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Principal miForm = new Principal();
            this.Hide();
            miForm.ShowDialog();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            searchProduct miForm = new searchProduct();
            this.Hide();
            miForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Price miForm = new Price();
            this.Hide();
            miForm.ShowDialog();
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            frm_Caducidades miForm = new frm_Caducidades();
            this.Hide();
            miForm.ShowDialog();
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            frm_Stock miForm = new frm_Stock();
            this.Hide();
            miForm.ShowDialog();
        }
    }
}
