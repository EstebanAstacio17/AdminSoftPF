using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminSoftPF
{
    public partial class Cuotas : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        int idProyecto = Utilidades.IdProyectoSeleccionado;
        public Cuotas()
        {
            InitializeComponent();
        }

        private void Cuotas_Load(object sender, EventArgs e)
        {
            // Llamamos al método para cargar las cuotas
            CargarCuotas(idProyecto);

            // Asignamos límites de caracteres a los TextBox
            AsignarLimiteCaracteres();

            btnActualizarCuota.Visible = false;
            btnActualizarCuota.Enabled = false;
        }

        public void CargarCuotas(int idProyecto)
        {
            string query = "SELECT Detalle, Cuota FROM Cuota WHERE ID_Proyecto = @IdProyecto";
            DataTable dtCuotas = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);
                    adapter.Fill(dtCuotas);
                }

                dgvCuotas.DataSource = dtCuotas;
                dgvCuotas.Columns["Detalle"].Width = 290;
                dgvCuotas.Columns["Cuota"].Width = 102;

                // Aplicar formato directamente en la columna del DataGridView
                dgvCuotas.Columns["Cuota"].DefaultCellStyle.Format = "N2";
                dgvCuotas.Columns["Cuota"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las Cuotas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para asignar límite de caracteres a los TextBox
        private void AsignarLimiteCaracteres()
        {
            // Asignamos un límite de 100 caracteres al TextBox que se utilice para el "Detalle"
            txtDetalle.MaxLength = 20;  // Asignar límite de 100 caracteres

            // Asignamos un límite de 50 caracteres al TextBox que se utilice para la "Cuota"
            txtCuota.MaxLength = 5;     // Asignar límite de 50 caracteres
        }

        // Este evento solo permite números
        private void txtSoloNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite solo números (0-9) y la tecla de retroceso (Backspace)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;  // Ignora la tecla si no es un número ni retroceso
            }
        }

        // Este evento solo permite letras
        private void txtSoloLetras_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite letras, la tecla de retroceso (Backspace) y espacios en blanco
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != ' ')
            {
                e.Handled = true; // Ignora la tecla si no es válida
            }
        }

        private void btnAgregarCuota_Click(object sender, EventArgs e)
        {
            // Validar que ambos TextBox estén llenos
            if (string.IsNullOrWhiteSpace(txtDetalle.Text))
            {
                MessageBox.Show("El campo 'Detalle' es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCuota.Text))
            {
                MessageBox.Show("El campo 'Cuota' es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Si pasan la validación, insertar los datos en la base de datos
            string query = "INSERT INTO Cuota (Detalle, Cuota, ID_Proyecto) VALUES (@Detalle, @Cuota, @IdProyecto)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add("@Detalle", SqlDbType.VarChar, 100).Value = txtDetalle.Text.Trim();
                    command.Parameters.Add("@Cuota", SqlDbType.VarChar, 50).Value = txtCuota.Text.Trim();
                    command.Parameters.Add("@IdProyecto", SqlDbType.Int).Value = idProyecto;

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Cuota agregada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();

                // Refrescar el DataGridView
                CargarCuotas(idProyecto);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar la Cuota: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            // Limpiar los TextBox
            txtDetalle.Clear();
            txtCuota.Clear();
        }

        private void dgvCuotas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar que la fila seleccionada es válida
            if (e.RowIndex >= 0)
            {
                // Limpiar los campos antes de asignar nuevos valores
                LimpiarCampos();
                btnEstadoCuota.Text = "";
                btnEstadoCuota.BackColor = SystemColors.Control;
                btnEstadoCuota.ForeColor = Color.Black;

                // Obtener la fila seleccionada
                DataGridViewRow fila = dgvCuotas.Rows[e.RowIndex];

                // Obtener los valores de las columnas Cuenta y Banco
                string detalle = fila.Cells["Detalle"].Value?.ToString() ?? "";
                string cuota = fila.Cells["Cuota"].Value?.ToString() ?? "";

                // Asignar valores a los TextBox
                txtDetalle.Text = detalle;
                txtCuota.Text = cuota;

                // Obtener el estado desde la base de datos
                string estado = ObtenerEstadoDesdeBD(cuota);

                // Asignar el texto y color del botón según el estado
                switch (estado)
                {
                    case "Activo":
                        btnEstadoCuota.Text = "Activo";
                        btnEstadoCuota.BackColor = Color.Green;
                        btnEstadoCuota.ForeColor = Color.White;
                        break;
                    case "No Activo":
                        btnEstadoCuota.Text = "No Activo";
                        btnEstadoCuota.BackColor = Color.Red;
                        btnEstadoCuota.ForeColor = Color.White;
                        break;
                    default:
                        btnEstadoCuota.Text = "Desconocido";
                        btnEstadoCuota.BackColor = SystemColors.Control;
                        btnEstadoCuota.ForeColor = Color.Black;
                        break;
                }

                // Mostrar y habilitar btnActualizar, ocultar e inhabilitar btnGuardar
                btnActualizarCuota.Visible = true;
                btnActualizarCuota.Enabled = true;
                btnAgregarCuota.Visible = false;
                btnAgregarCuota.Enabled = false;
            }
        }

        private string ObtenerEstadoDesdeBD(string cuota)
        {
            string query = "SELECT Estado FROM Cuota WHERE Cuota = @Cuota AND ID_Proyecto = @IdProyecto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Cuota", cuota);
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    return command.ExecuteScalar()?.ToString() ?? "Desconocido";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el Estado de la Cuota: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "Desconocido";
            }
        }

        private void btnEstadoCuota_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCuota.Text))
            {
                MessageBox.Show("Seleccione una Cuota antes de cambiar su Estado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nuevoEstado = btnEstadoCuota.Text == "Activo" ? "No Activo" : "Activo";

            if (!ConfirmarAccion($"¿Está seguro de cambiar el Estado a '{nuevoEstado}'?")) return;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand("UPDATE Cuota SET Estado = @Estado WHERE Cuota = @Cuota AND ID_Proyecto = @IdProyecto", connection))
                {
                    command.Parameters.AddWithValue("@Estado", nuevoEstado);
                    command.Parameters.AddWithValue("@Cuota", txtCuota.Text.Trim());
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Estado actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnEstadoCuota.Text = nuevoEstado;
                        btnEstadoCuota.BackColor = (nuevoEstado == "Activo") ? Color.Green : Color.Red;
                        btnEstadoCuota.ForeColor = Color.White;
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la Cuota para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el Estado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizarCuota_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDetalle.Text) || string.IsNullOrWhiteSpace(txtCuota.Text))
            {
                MessageBox.Show("Debe completar todos los campos antes de actualizar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvCuotas.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una cuenta para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string cuotaOriginal = dgvCuotas.CurrentRow.Cells["Cuota"].Value.ToString();
            string detalleOriginal = dgvCuotas.CurrentRow.Cells["Detalle"].Value.ToString();

            if (!ConfirmarAccion("¿Está seguro de que desea actualizar la Cuota?")) return;

            string query = "UPDATE Cuota SET Detalle = @Detalle, Cuota = @Cuota WHERE Cuota = @CuotaOriginal AND Detalle = @DetalleOriginal AND ID_Proyecto = @IdProyecto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Detalle", txtDetalle.Text.Trim());
                    command.Parameters.AddWithValue("@Cuota", txtCuota.Text.Trim());
                    command.Parameters.AddWithValue("@CuotaOriginal", cuotaOriginal);
                    command.Parameters.AddWithValue("@DetalleOriginal", detalleOriginal);
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Cuota actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarCuotas(idProyecto);
                        LimpiarCampos();
                        btnActualizarCuota.Visible = false;
                        btnActualizarCuota.Enabled = false;
                        btnAgregarCuota.Visible = true;
                        btnAgregarCuota.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la Cuota para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar la Cuota: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ConfirmarAccion(string mensaje)
        {
            return MessageBox.Show(mensaje, "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}