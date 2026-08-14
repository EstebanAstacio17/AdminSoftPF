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
    public partial class Manzana : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        int idProyecto = Utilidades.IdProyectoSeleccionado;

        public Manzana()
        {
            InitializeComponent();
        }

        private void Manzana_Load(object sender, EventArgs e)
        {
            // Llamamos al método para cargar las cuotas
            CargarManzana(idProyecto);

            // Asignamos límites de caracteres a los TextBox
            AsignarLimiteCaracteres();

            btnActualizarManzana.Visible = false;
            btnActualizarManzana.Enabled = false;
        }

        public void CargarManzana(int idProyecto)
        {
            string query = "SELECT Manzana, Detalle FROM Manzana WHERE ID_Proyecto = @IdProyecto";
            DataTable dtManzana = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);
                    adapter.Fill(dtManzana);
                }

                dgvManzana.DataSource = dtManzana;
                dgvManzana.Columns["Manzana"].Width = 102;
                dgvManzana.Columns["Detalle"].Width = 288;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las Manzanas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para asignar límite de caracteres a los TextBox
        private void AsignarLimiteCaracteres()
        {
            // Asignamos un límite de 20 caracteres al TextBox que se utilice para el "Detalle"
            txtDetalle.MaxLength = 20;  // Asignar límite de 20 caracteres

            // Asignamos un límite de 10 caracteres al TextBox que se utilice para la "Cuota"
            txtManzana.MaxLength = 10;     // Asignar límite de 10 caracteres
        }

        private void TxtManzana_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Convertimos el carácter actual en mayúscula
            e.KeyChar = char.ToUpper(e.KeyChar);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(txtManzana.Text))
            {
                MessageBox.Show("El campo 'Manzana' es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verificar si el Manzana ya existe
            if (ExisteManzana(txtManzana.Text.Trim(), idProyecto))
            {
                MessageBox.Show("La Manzana ya existe en este Proyecto.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Si pasan la validación, insertar los datos en la base de datos
            string query = "INSERT INTO Manzana (Detalle, Manzana, ID_Proyecto) VALUES (@Detalle, @Manzana, @IdProyecto)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add("@Detalle", SqlDbType.VarChar, 100).Value = txtDetalle.Text.Trim();
                    command.Parameters.Add("@Manzana", SqlDbType.VarChar, 50).Value = txtManzana.Text.Trim();
                    command.Parameters.Add("@IdProyecto", SqlDbType.Int).Value = idProyecto;

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Manzana agregada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();

                // Refrescar el DataGridView
                CargarManzana(idProyecto);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar la Manzana: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            // Limpiar los TextBox
            txtDetalle.Clear();
            txtManzana.Clear();
        }

        private bool ExisteManzana(string manzana, int idProyecto)
        {
            string query = "SELECT COUNT(1) FROM Manzana WHERE Manzana = @Manzana AND ID_Proyecto = @IdProyecto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Manzana", manzana);
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    int count = (int)command.ExecuteScalar();

                    return count > 0; // Retorna true si existe, false en caso contrario
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al verificar la existencia de la Manzana: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void dgvManzana_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar que la fila seleccionada es válida
            if (e.RowIndex >= 0)
            {
                // Limpiar los campos antes de asignar nuevos valores
                LimpiarCampos();
                btnEstadoManzana.Text = "";
                btnEstadoManzana.BackColor = SystemColors.Control;
                btnEstadoManzana.ForeColor = Color.Black;

                // Obtener la fila seleccionada
                DataGridViewRow fila = dgvManzana.Rows[e.RowIndex];

                // Obtener los valores de las columnas Cuenta y Banco
                string detalle = fila.Cells["Detalle"].Value?.ToString() ?? "";
                string manzana = fila.Cells["Manzana"].Value?.ToString() ?? "";

                // Asignar valores a los TextBox
                txtDetalle.Text = detalle;
                txtManzana.Text = manzana;

                // Obtener el estado desde la base de datos
                string estado = ObtenerEstadoDesdeBD(manzana);

                // Asignar el texto y color del botón según el estado
                switch (estado)
                {
                    case "Activo":
                        btnEstadoManzana.Text = "Activo";
                        btnEstadoManzana.BackColor = Color.Green;
                        btnEstadoManzana.ForeColor = Color.White;
                        break;
                    case "No Activo":
                        btnEstadoManzana.Text = "No Activo";
                        btnEstadoManzana.BackColor = Color.Red;
                        btnEstadoManzana.ForeColor = Color.White;
                        break;
                    default:
                        btnEstadoManzana.Text = "Desconocido";
                        btnEstadoManzana.BackColor = SystemColors.Control;
                        btnEstadoManzana.ForeColor = Color.Black;
                        break;
                }

                // Mostrar y habilitar btnActualizar, ocultar e inhabilitar btnGuardar
                btnActualizarManzana.Visible = true;
                btnActualizarManzana.Enabled = true;
                btnAgregar.Visible = false;
                btnAgregar.Enabled = false;
            }
        }

        private string ObtenerEstadoDesdeBD(string manzana)
        {
            string query = "SELECT Estado FROM Manzana WHERE Manzana = @Manzana AND ID_Proyecto = @IdProyecto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Manzana", manzana);
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    return command.ExecuteScalar()?.ToString() ?? "Desconocido";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el Estado de la Manzana: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "Desconocido";
            }
        }

        private void btnEstadoManzana_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtManzana.Text))
            {
                MessageBox.Show("Seleccione una Manzana antes de cambiar su Estado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nuevoEstado = btnEstadoManzana.Text == "Activo" ? "No Activo" : "Activo";

            if (!ConfirmarAccion($"¿Está seguro de cambiar el Estado a '{nuevoEstado}'?")) return;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand("UPDATE Manzana SET Estado = @Estado WHERE Manzana = @Manzana AND ID_Proyecto = @IdProyecto", connection))
                {
                    command.Parameters.AddWithValue("@Estado", nuevoEstado);
                    command.Parameters.AddWithValue("@Manzana", txtManzana.Text.Trim());
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Estado actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnEstadoManzana.Text = nuevoEstado;
                        btnEstadoManzana.BackColor = (nuevoEstado == "Activo") ? Color.Green : Color.Red;
                        btnEstadoManzana.ForeColor = Color.White;
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la Manzana para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el Estado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizarManzana_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDetalle.Text) || string.IsNullOrWhiteSpace(txtManzana.Text))
            {
                MessageBox.Show("Debe completar todos los campos antes de actualizar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvManzana.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una Manzana para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string manzanaOriginal = dgvManzana.CurrentRow.Cells["Manzana"].Value.ToString();
            string detalleOriginal = dgvManzana.CurrentRow.Cells["Detalle"].Value.ToString();

            if (!ConfirmarAccion("¿Está seguro de que desea actualizar la Manzana?")) return;

            string query = "UPDATE Manzana SET Detalle = @Detalle, Manzana = @Manzana WHERE Manzana = @ManzanaOriginal AND Detalle = @DetalleOriginal AND ID_Proyecto = @IdProyecto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Detalle", txtDetalle.Text.Trim());
                    command.Parameters.AddWithValue("@Manzana", txtManzana.Text.Trim());
                    command.Parameters.AddWithValue("@ManzanaOriginal", manzanaOriginal);
                    command.Parameters.AddWithValue("@DetalleOriginal", detalleOriginal);
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Manzana actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarManzana(idProyecto);
                        LimpiarCampos();
                        btnActualizarManzana.Visible = false;
                        btnActualizarManzana.Enabled = false;
                        btnAgregar.Visible = true;
                        btnAgregar.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la Manzana para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar la Manzana: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ConfirmarAccion(string mensaje)
        {
            return MessageBox.Show(mensaje, "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}