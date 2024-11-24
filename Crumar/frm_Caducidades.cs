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
    public partial class frm_Caducidades : Form
    {
        public frm_Caducidades()
        {
            InitializeComponent();
        }

        private void frm_Caducidades_Load(object sender, EventArgs e)
        {
            try
            {
                // Cargar las opciones del ComboBox
                cbCaducidad.Items.Clear();
                cbCaducidad.Items.Add("Próximos a caducar");
                cbCaducidad.Items.Add("Caducados"); // Cambié "No caducados aún" por "Caducados"
                cbCaducidad.SelectedIndex = 0; // Establecer por defecto "Próximos a caducar"

                // Cargar los datos en el DataGridView
                this.tbProductosTableAdapter.Fill(this.db_CRUMARDataSet4.tbProductos);
                dgvCaducidad.ReadOnly = true; // Hacer que el DataGridView sea solo lectura
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFiltrar_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Aplicar el filtro seleccionado en el ComboBox y actualizar el DataGridView
                FiltrarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al aplicar el filtro: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FiltrarProductos()
        {
            try
            {
                string query = string.Empty;
                DateTime fechaActual = DateTime.Now;

                if (cbCaducidad.SelectedItem.ToString() == "Próximos a caducar")
                {
                    // Filtrar productos cuya fecha de caducidad esté dentro de los próximos 7 días
                    query = "SELECT codigoBarras, nombre, marca, precioVenta, fechaCaducidad " +
                           "FROM tbProductos WHERE fechaCaducidad > @fechaActual";
                }
                else if (cbCaducidad.SelectedItem.ToString() == "Caducados") // Cambié la opción a "Caducados"
                {
                    // Filtrar productos cuya fecha de caducidad ya haya pasado
                    query = "SELECT codigoBarras, nombre, marca, precioVenta, fechaCaducidad " +
                            "FROM tbProductos WHERE fechaCaducidad < @fechaActual"; // Cambié la condición para los productos caducados
                }

                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.db_CRUMARConnectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@fechaActual", fechaActual);
                    adapter.SelectCommand.Parameters.AddWithValue("@fechaLimite", fechaActual.AddDays(7)); // Próximos 7 días

                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "tbProductos");

                    // Asignar los resultados al DataGridView
                    dgvCaducidad.DataSource = ds;
                    dgvCaducidad.DataMember = "tbProductos";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar los productos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFechas_Click(object sender, EventArgs e)
        {
            try
            {
                // Mostrar las fechas de caducidad de todos los productos en el DataGridView
                string query = "SELECT codigoBarras, nombre, marca, precioVenta, fechaCaducidad FROM tbProductos";

                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.db_CRUMARConnectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "tbProductos");

                    // Asignar los resultados al DataGridView
                    dgvCaducidad.DataSource = ds;
                    dgvCaducidad.DataMember = "tbProductos";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las fechas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
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