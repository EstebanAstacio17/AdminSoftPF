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
    public partial class Edificio : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        int idProyecto = Utilidades.IdProyectoSeleccionado;
        public Edificio()
        {
            InitializeComponent();
        }

        private void Edificio_Load(object sender, EventArgs e)
        {
            // Llamamos al método para cargar las cuotas
            CargarEdificio(idProyecto);

            // Asignamos límites de caracteres a los TextBox
            AsignarLimiteCaracteres();

            btnActualizarEdificio.Visible = false;
            btnActualizarEdificio.Enabled = false;
        }

        public void CargarEdificio(int idProyecto)
        {
            string query = "SELECT Edificio, Detalle FROM Edificio WHERE ID_Proyecto = @IdProyecto";
            DataTable dtEdificio = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);
                    adapter.Fill(dtEdificio);
                }

                dgvEdificio.DataSource = dtEdificio;
                dgvEdificio.Columns["Edificio"].Width = 102;
                dgvEdificio.Columns["Detalle"].Width = 288;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las Edificio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para asignar límite de caracteres a los TextBox
        private void AsignarLimiteCaracteres()
        {
            // Asignamos un límite de 20 caracteres al TextBox que se utilice para el "Detalle"
            txtDetalle.MaxLength = 20;  // Asignar límite de 20 caracteres

            // Asignamos un límite de 5 caracteres al TextBox que se utilice para la "Cuota"
            txtEdificio.MaxLength = 5;     // Asignar límite de 5 caracteres
        }
        private void TxtEdificio_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Convertimos el carácter actual en mayúscula
            e.KeyChar = char.ToUpper(e.KeyChar);
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtEdificio.Text))
            {
                MessageBox.Show("El campo 'Edificio' es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verificar si el edificio ya existe
            if (ExisteEdificio(txtEdificio.Text.Trim(), idProyecto))
            {
                MessageBox.Show("El Edificio ya existe en este proyecto.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Si pasan la validación, insertar los datos en la base de datos
            string query = "INSERT INTO Edificio (Detalle, Edificio, ID_Proyecto) VALUES (@Detalle, @Edificio, @IdProyecto)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add("@Detalle", SqlDbType.VarChar, 100).Value = txtDetalle.Text.Trim();
                    command.Parameters.Add("@Edificio", SqlDbType.VarChar, 50).Value = txtEdificio.Text.Trim();
                    command.Parameters.Add("@IdProyecto", SqlDbType.Int).Value = idProyecto;

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Edificio agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();

                // Refrescar el DataGridView
                CargarEdificio(idProyecto);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar la Edificio: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LimpiarCampos()
        {
            // Limpiar los TextBox
            txtDetalle.Clear();
            txtEdificio.Clear();
        }

        private bool ExisteEdificio(string edificio, int idProyecto)
        {
            string query = "SELECT COUNT(1) FROM Edificio WHERE Edificio = @Edificio AND ID_Proyecto = @IdProyecto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Edificio", edificio);
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    int count = (int)command.ExecuteScalar();

                    return count > 0; // Retorna true si existe, false en caso contrario
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al verificar la existencia del Edificio: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void dgvEdificio_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar que la fila seleccionada es válida
            if (e.RowIndex >= 0)
            {
                // Limpiar los campos antes de asignar nuevos valores
                LimpiarCampos();
                btnEstadoEdificio.Text = "";
                btnEstadoEdificio.BackColor = SystemColors.Control;
                btnEstadoEdificio.ForeColor = Color.Black;

                // Obtener la fila seleccionada
                DataGridViewRow fila = dgvEdificio.Rows[e.RowIndex];

                // Obtener los valores de las columnas Cuenta y Banco
                string detalle = fila.Cells["Detalle"].Value?.ToString() ?? "";
                string edificio = fila.Cells["Edificio"].Value?.ToString() ?? "";

                // Asignar valores a los TextBox
                txtDetalle.Text = detalle;
                txtEdificio.Text = edificio;

                // Obtener el estado desde la base de datos
                string estado = ObtenerEstadoDesdeBD(edificio);

                // Asignar el texto y color del botón según el estado
                switch (estado)
                {
                    case "Activo":
                        btnEstadoEdificio.Text = "Activo";
                        btnEstadoEdificio.BackColor = Color.Green;
                        btnEstadoEdificio.ForeColor = Color.White;
                        break;
                    case "No Activo":
                        btnEstadoEdificio.Text = "No Activo";
                        btnEstadoEdificio.BackColor = Color.Red;
                        btnEstadoEdificio.ForeColor = Color.White;
                        break;
                    default:
                        btnEstadoEdificio.Text = "Desconocido";
                        btnEstadoEdificio.BackColor = SystemColors.Control;
                        btnEstadoEdificio.ForeColor = Color.Black;
                        break;
                }

                // Mostrar y habilitar btnActualizar, ocultar e inhabilitar btnGuardar
                btnActualizarEdificio.Visible = true;
                btnActualizarEdificio.Enabled = true;
                btnAgregar.Visible = false;
                btnAgregar.Enabled = false;
            }
        }

        private string ObtenerEstadoDesdeBD(string edificio)
        {
            string query = "SELECT Estado FROM Edificio WHERE Edificio = @Edificio AND ID_Proyecto = @IdProyecto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Edificio", edificio);
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    return command.ExecuteScalar()?.ToString() ?? "Desconocido";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el Estado del Edificio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "Desconocido";
            }
        }

        private void btnEstadoEdificio_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEdificio.Text))
            {
                MessageBox.Show("Seleccione una Edificio antes de cambiar su Estado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nuevoEstado = btnEstadoEdificio.Text == "Activo" ? "No Activo" : "Activo";

            if (!ConfirmarAccion($"¿Está seguro de cambiar el Estado a '{nuevoEstado}'?")) return;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand("UPDATE Edificio SET Estado = @Estado WHERE Edificio = @Edificio AND ID_Proyecto = @IdProyecto", connection))
                {
                    command.Parameters.AddWithValue("@Estado", nuevoEstado);
                    command.Parameters.AddWithValue("@Edificio", txtEdificio.Text.Trim());
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Estado actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnEstadoEdificio.Text = nuevoEstado;
                        btnEstadoEdificio.BackColor = (nuevoEstado == "Activo") ? Color.Green : Color.Red;
                        btnEstadoEdificio.ForeColor = Color.White;
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el Edificio para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el Estado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizarEdificio_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDetalle.Text) || string.IsNullOrWhiteSpace(txtEdificio.Text))
            {
                MessageBox.Show("Debe completar todos los campos antes de actualizar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvEdificio.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una Edificio para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string edificioOriginal = dgvEdificio.CurrentRow.Cells["Edificio"].Value.ToString();
            string detalleOriginal = dgvEdificio.CurrentRow.Cells["Detalle"].Value.ToString();

            if (!ConfirmarAccion("¿Está seguro de que desea actualizar el Edificio?")) return;

            string query = "UPDATE Edificio SET Detalle = @Detalle, Edificio = @Edificio WHERE Edificio = @EdificioOriginal AND Detalle = @DetalleOriginal AND ID_Proyecto = @IdProyecto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Detalle", txtDetalle.Text.Trim());
                    command.Parameters.AddWithValue("@Edificio", txtEdificio.Text.Trim());
                    command.Parameters.AddWithValue("@EdificioOriginal", edificioOriginal);
                    command.Parameters.AddWithValue("@DetalleOriginal", detalleOriginal);
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Manzana actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarEdificio(idProyecto);
                        LimpiarCampos();
                        btnActualizarEdificio.Visible = false;
                        btnActualizarEdificio.Enabled = false;
                        btnAgregar.Visible = true;
                        btnAgregar.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el Edificio para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el Edificio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ConfirmarAccion(string mensaje)
        {
            return MessageBox.Show(mensaje, "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}