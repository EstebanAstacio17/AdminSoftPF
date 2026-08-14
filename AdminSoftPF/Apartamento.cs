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
    public partial class Apartamento : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        int idProyecto = Utilidades.IdProyectoSeleccionado;
        public Apartamento()
        {
            InitializeComponent();
        }

        private void Apartamento_Load(object sender, EventArgs e)
        {
            // Llamamos al método para cargar las cuotas
            CargarApartamento(idProyecto);

            // Asignamos límites de caracteres a los TextBox
            AsignarLimiteCaracteres();

            btnActualizarApartamento.Visible = false;
            btnActualizarApartamento.Enabled = false;
        }

        public void CargarApartamento(int idProyecto)
        {
            // Crear la consulta SQL para obtener las cuotas correspondientes al idProyecto
            string query = "SELECT Apartamento, Detalle FROM Apartamento WHERE ID_Proyecto = @IdProyecto";

            // Crear una lista para almacenar los resultados
            DataTable dtapartamento = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);
                    adapter.Fill(dtapartamento);
                }

                dgvApartamento.DataSource = dtapartamento;
                dgvApartamento.Columns["Apartamento"].Width = 102;
                dgvApartamento.Columns["Detalle"].Width = 288;


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las Apartamento: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para asignar límite de caracteres a los TextBox
        private void AsignarLimiteCaracteres()
        {
            // Asignamos un límite de 20 caracteres al TextBox que se utilice para el "Detalle"
            txtDetalle.MaxLength = 20;  // Asignar límite de 20 caracteres

            // Asignamos un límite de 5 caracteres al TextBox que se utilice para la "Cuota"
            txtApartamento.MaxLength = 5;     // Asignar límite de 5 caracteres
        }

        private void TxtApartamento_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Convertimos el carácter actual en mayúscula
            e.KeyChar = char.ToUpper(e.KeyChar);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtApartamento.Text))
            {
                MessageBox.Show("El campo 'Apartamento' es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verificar si el apartamento ya existe
            if (ExisteApartamento(txtApartamento.Text.Trim(), idProyecto))
            {
                MessageBox.Show("El Apartamento ya existe en este Proyecto.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Insertar los datos en la base de datos
            string query = "INSERT INTO Apartamento (Detalle, Apartamento, ID_Proyecto) VALUES (@Detalle, @Apartamento, @IdProyecto)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Detalle", txtDetalle.Text.Trim());
                    command.Parameters.AddWithValue("@Apartamento", txtApartamento.Text.Trim());
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Apartamento agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();

                CargarApartamento(idProyecto);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el Apartamento: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LimpiarCampos()
        {
            // Limpiar los TextBox
            txtDetalle.Clear();
            txtApartamento.Clear();
        }

        private bool ExisteApartamento(string apartamento, int idProyecto)
        {
            string query = "SELECT COUNT(1) FROM Apartamento WHERE Apartamento = @Apartamento AND ID_Proyecto = @IdProyecto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Apartamento", apartamento);
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    int count = (int)command.ExecuteScalar();

                    return count > 0; // Retorna true si existe, false en caso contrario
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al verificar la existencia del Apartamento: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void dgvApartamento_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            // Verificar que la fila seleccionada es válida
            if (e.RowIndex >= 0)
            {
                // Limpiar los campos antes de asignar nuevos valores
                LimpiarCampos();
                btnEstadoApartamento.Text = "";
                btnEstadoApartamento.BackColor = SystemColors.Control;
                btnEstadoApartamento.ForeColor = Color.Black;

                // Obtener la fila seleccionada
                DataGridViewRow fila = dgvApartamento.Rows[e.RowIndex];

                // Obtener los valores de las columnas Cuenta y Banco
                string detalle = fila.Cells["Detalle"].Value?.ToString() ?? "";
                string apartamento = fila.Cells["Apartamento"].Value?.ToString() ?? "";

                // Asignar valores a los TextBox
                txtDetalle.Text = detalle;
                txtApartamento.Text = apartamento;

                // Obtener el estado desde la base de datos
                string estado = ObtenerEstadoDesdeBD(apartamento);

                // Asignar el texto y color del botón según el estado
                switch (estado)
                {
                    case "Activo":
                        btnEstadoApartamento.Text = "Activo";
                        btnEstadoApartamento.BackColor = Color.Green;
                        btnEstadoApartamento.ForeColor = Color.White;
                        break;
                    case "No Activo":
                        btnEstadoApartamento.Text = "No Activo";
                        btnEstadoApartamento.BackColor = Color.Red;
                        btnEstadoApartamento.ForeColor = Color.White;
                        break;
                    default:
                        btnEstadoApartamento.Text = "Desconocido";
                        btnEstadoApartamento.BackColor = SystemColors.Control;
                        btnEstadoApartamento.ForeColor = Color.Black;
                        break;
                }

                // Mostrar y habilitar btnActualizar, ocultar e inhabilitar btnGuardar
                btnActualizarApartamento.Visible = true;
                btnActualizarApartamento.Enabled = true;
                btnAgregar.Visible = false;
                btnAgregar.Enabled = false;
            }
        }

        private string ObtenerEstadoDesdeBD(string apartamento)
        {
            string query = "SELECT Estado FROM Apartamento WHERE Apartamento = @Apartamento AND ID_Proyecto = @IdProyecto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Apartamento", apartamento);
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

        private void btnEstadoApartamento_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtApartamento.Text))
            {
                MessageBox.Show("Seleccione una Apartamento antes de cambiar su Estado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nuevoEstado = btnEstadoApartamento.Text == "Activo" ? "No Activo" : "Activo";

            if (!ConfirmarAccion($"¿Está seguro de cambiar el Estado a '{nuevoEstado}'?")) return;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand("UPDATE Apartamento SET Estado = @Estado WHERE Apartamento = @Apartamento AND ID_Proyecto = @IdProyecto", connection))
                {
                    command.Parameters.AddWithValue("@Estado", nuevoEstado);
                    command.Parameters.AddWithValue("@Apartamento", txtApartamento.Text.Trim());
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Estado actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnEstadoApartamento.Text = nuevoEstado;
                        btnEstadoApartamento.BackColor = (nuevoEstado == "Activo") ? Color.Green : Color.Red;
                        btnEstadoApartamento.ForeColor = Color.White;
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la Apartamento para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el Estado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizarApartamento_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDetalle.Text) || string.IsNullOrWhiteSpace(txtApartamento.Text))
            {
                MessageBox.Show("Debe completar todos los campos antes de actualizar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvApartamento.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una Manzana para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string apartamentoOriginal = dgvApartamento.CurrentRow.Cells["Apartamento"].Value.ToString();
            string detalleOriginal = dgvApartamento.CurrentRow.Cells["Detalle"].Value.ToString();

            if (!ConfirmarAccion("¿Está seguro de que desea actualizar el Apartamento?")) return;

            string query = "UPDATE Apartamento SET Detalle = @Detalle, Apartamento = @Apartamento WHERE Apartamento = @ApartamentoOriginal AND Detalle = @DetalleOriginal AND ID_Proyecto = @IdProyecto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Detalle", txtDetalle.Text.Trim());
                    command.Parameters.AddWithValue("@Apartamento", txtApartamento.Text.Trim());
                    command.Parameters.AddWithValue("@ApartamentoOriginal", apartamentoOriginal);
                    command.Parameters.AddWithValue("@DetalleOriginal", detalleOriginal);
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Apartamento actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarApartamento(idProyecto);
                        LimpiarCampos();
                        btnActualizarApartamento.Visible = false;
                        btnActualizarApartamento.Enabled = false;
                        btnAgregar.Visible = true;
                        btnAgregar.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la Apartamento para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar la Apartamento: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ConfirmarAccion(string mensaje)
        {
            return MessageBox.Show(mensaje, "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}