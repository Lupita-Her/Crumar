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
    public partial class frm_Stock : Form
    {
        public frm_Stock()
        {
            InitializeComponent();
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

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frm_Stock_Load(object sender, EventArgs e)
        {
            try
            {
                // Cargar las marcas disponibles en el ComboBox
                CargarMarcas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las marcas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarMarcas()
        {
            try
            {
                // Consulta para obtener todas las marcas de la tabla tbProductos
                string query = "SELECT DISTINCT marca FROM tbProductos";

                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.db_CRUMARConnectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dtMarcas = new DataTable();
                    adapter.Fill(dtMarcas);

                    // Llenar el ComboBox con las marcas disponibles
                    cbMarca.DataSource = dtMarcas;
                    cbMarca.DisplayMember = "marca";  // Mostramos la columna "marca"
                    cbMarca.SelectedIndex = -1; // Sin selección inicial
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las marcas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar si se ha seleccionado una marca
                if (cbMarca.SelectedIndex == -1)
                {
                    MessageBox.Show("Por favor, selecciona una marca", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Filtrar los productos por la marca seleccionada
                FiltrarProductosPorMarca();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al realizar la búsqueda: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FiltrarProductosPorMarca()
        {
            try
            {
                // Usamos Text para obtener el valor de la marca seleccionada
                string marcaSeleccionada = cbMarca.Text;

                string query = "SELECT codigoBarras, nombre, marca, existencia FROM tbProductos WHERE marca = @marca";

                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.db_CRUMARConnectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@marca", marcaSeleccionada);

                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "tbProductos");

                    // Depuración: imprimir las columnas recuperadas
                    foreach (DataColumn col in ds.Tables["tbProductos"].Columns)
                    {
                        Console.WriteLine($"Columna recuperada: {col.ColumnName}");
                    }

                    // Configurar el DataGridView
                    dgvStock.AutoGenerateColumns = true;
                    dgvStock.DataSource = ds;
                    dgvStock.DataMember = "tbProductos";

                    // Llamar al método para aplicar colores según la existencia
                    ColorearStock();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar los productos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ColorearStock()
        {
            try
            {
                // Recorrer cada fila del DataGridView y aplicar el color según la existencia
                foreach (DataGridViewRow row in dgvStock.Rows)
                {
                    // Verificar si la celda 'existencia' tiene un valor válido
                    if (row.Cells["existencia"].Value != DBNull.Value && row.Cells["existencia"].Value != null)
                    {
                        int existencia = Convert.ToInt32(row.Cells["existencia"].Value);

                        // Aplicar colores según la existencia
                        if (existencia == 0)
                        {
                            // Color rojo para productos agotados
                            row.DefaultCellStyle.BackColor = Color.Red;
                        }
                        else if (existencia < 5)
                        {
                            // Color naranja para productos con pocas existencias
                            row.DefaultCellStyle.BackColor = Color.Orange;
                        }
                        else
                        {
                            // Color verde para productos con más de 5 existencias
                            row.DefaultCellStyle.BackColor = Color.Green;
                        }
                    }
                    else
                    {
                        // Si la celda 'existencia' está vacía, aplicar un color gris para indicar que hay un problema
                        row.DefaultCellStyle.BackColor = Color.Gray;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al aplicar los colores: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}