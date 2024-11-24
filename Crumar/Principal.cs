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
    public partial class Principal : Form
    {
        public Principal()
        {
            InitializeComponent();


        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            subMenuSearch.Visible = !subMenuSearch.Visible;
            
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            
            frm_search miForm = new frm_search();
            this.Hide();
            miForm.ShowDialog();
        }
        
    }
}