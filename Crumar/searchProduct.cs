using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crumar
{
    public partial class searchProduct : Form
    {
        public searchProduct()
        {
            InitializeComponent();
        }

        private void searchProduct_Load(object sender, EventArgs e)
        {
            try
            {
                // Cargar datos en el DataGridView
                this.tbProductosTableAdapter.Fill(this.db_CRUMARDataSet2.tbProductos);

                // Deshabilitar edición en el DataGridView
                dgvProducts.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtProducts_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Validar si hay opción seleccionada en el ComboBox
                if (string.IsNullOrWhiteSpace(cbOptions.Text))
                {
                    MessageBox.Show("Por favor, seleccione un criterio de búsqueda.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection cadena = new SqlConnection(Properties.Settings.Default.db_CRUMARConnectionString))
                {
                    // Consulta dinámica con parámetros
                    string query = $"SELECT * FROM tbProductos WHERE {cbOptions.Text} LIKE @searchText";
                    SqlDataAdapter adap = new SqlDataAdapter(query, cadena);
                    adap.SelectCommand.Parameters.AddWithValue("@searchText", $"%{txtProducts.Text}%");

                    DataSet ds = new DataSet();
                    adap.Fill(ds, "tbProductos");
                    dgvProducts.DataSource = ds;
                    dgvProducts.DataMember = "tbProductos";
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error de base de datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}