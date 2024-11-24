using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crumar
{
    public partial class Price : Form
    {
        public Price()
        {
            InitializeComponent();
        }

        private void Price_Load(object sender, EventArgs e)
        {
            try
            {
                // Cargar las opciones del ComboBox con los criterios de búsqueda válidos
                cbOptions.Items.Clear();
                cbOptions.Items.Add("codigoBarras");
                cbOptions.Items.Add("nombre");
                cbOptions.SelectedIndex = 0; // Puedes establecer uno por defecto si lo deseas

                // Cargar datos en el DataGridView
                this.tbProductosTableAdapter.Fill(this.db_CRUMARDataSet3.tbProductos);
                dgvProducts.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar la aplicación: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                frm_search miForm = new frm_search();
                this.Hide();
                miForm.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ventana de búsqueda: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAscendente_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT codigoBarras, nombre, marca, precioVenta FROM tbProductos ORDER BY precioVenta ASC";

                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.db_CRUMARConnectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "tbProductos");

                    dgvProducts.DataSource = ds;
                    dgvProducts.DataMember = "tbProductos";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDescendente_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT codigoBarras, nombre, marca, precioVenta FROM tbProductos ORDER BY precioVenta DESC";

                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.db_CRUMARConnectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "tbProductos");

                    dgvProducts.DataSource = ds;
                    dgvProducts.DataMember = "tbProductos";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cbOptions.Text))
                {
                    MessageBox.Show("Por favor, seleccione un criterio de búsqueda.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    this.tbProductosTableAdapter.Fill(this.db_CRUMARDataSet3.tbProductos);
                    return;
                }

                using (SqlConnection cadena = new SqlConnection(Properties.Settings.Default.db_CRUMARConnectionString))
                {
                    string query = $"SELECT codigoBarras, nombre, marca, precioVenta FROM tbProductos WHERE {cbOptions.Text} LIKE @searchText";

                    SqlDataAdapter adap = new SqlDataAdapter(query, cadena);
                    adap.SelectCommand.Parameters.AddWithValue("@searchText", $"%{txtSearch.Text}%");

                    DataSet ds = new DataSet();
                    adap.Fill(ds, "tbProductos");

                    dgvProducts.DataSource = ds;
                    dgvProducts.DataMember = "tbProductos";

                    if (ds.Tables["tbProductos"].Rows.Count == 0)
                    {
                        MessageBox.Show("No se encontraron productos que coincidan con la búsqueda.", "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
    }
}