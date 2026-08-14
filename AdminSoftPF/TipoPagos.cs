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
    public partial class TipoPagos: Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        int idProyecto = Utilidades.IdProyectoSeleccionado;

        public TipoPagos()
        {
            InitializeComponent();
        }

        private void TipoPagos_Load(object sender, EventArgs e)
        {
            // Llamamos al método para cargar las cuotas
            CargarTipoDePago(idProyecto);

            // Asignamos límites de caracteres a los TextBox
            AsignarLimiteCaracteres();

            btnActualizar.Visible = false;
            btnActualizar.Enabled = false;
        }

        public void CargarTipoDePago(int idProyecto)
        {
            string query = "SELECT TipoDePago, Descripcion FROM TipoDePago WHERE ID_Proyecto = @IdProyecto";
            DataTable dtTipoDePago = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);
                    adapter.Fill(dtTipoDePago);
                }

                dgvTipoPagos.DataSource = dtTipoDePago;
                dgvTipoPagos.Columns["TipoDePago"].Width = 150;
                dgvTipoPagos.Columns["Descripcion"].Width = 240;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los Tipos De Pagos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AsignarLimiteCaracteres()
        {
            txtTipoPago.MaxLength = 20;

            txtDescripcion.MaxLength = 50;
        }

        private void TxtTipoDePago_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Convertimos el carácter actual en mayúscula
            e.KeyChar = char.ToUpper(e.KeyChar);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtTipoPago.Text))
            {
                MessageBox.Show("El 'Tipo De Pago' es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verificar si el Manzana ya existe
            if (ExisteManzana(txtTipoPago.Text.Trim(), idProyecto))
            {
                MessageBox.Show("El 'Tipo De Pago' ya existe en este Proyecto.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Si pasan la validación, insertar los datos en la base de datos
            string query = "INSERT INTO TipoDePago (Descripcion, TipoDePago, ID_Proyecto) VALUES (@Descripcion, @TipoDePago, @IdProyecto)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add("@Descripcion", SqlDbType.VarChar, 100).Value = txtDescripcion.Text.Trim();
                    command.Parameters.Add("@TipoDePago", SqlDbType.VarChar, 50).Value = txtTipoPago.Text.Trim();
                    command.Parameters.Add("@IdProyecto", SqlDbType.Int).Value = idProyecto;

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Tipo De Pago agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();

                // Refrescar el DataGridView
                CargarTipoDePago(idProyecto);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar la Manzana: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtDescripcion.Clear();
            txtTipoPago.Clear();
        }

        private bool ExisteManzana(string tipoDePago, int idProyecto)
        {
            string query = "SELECT COUNT(1) FROM TipoDePago WHERE TipoDePago = @TipoDePago AND ID_Proyecto = @IdProyecto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TipoDePago", tipoDePago);
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al verificar la existencia del Tipo De Pago: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void dgvTipoDePago_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar que la fila seleccionada es válida
            if (e.RowIndex >= 0)
            {
                // Limpiar los campos antes de asignar nuevos valores
                LimpiarCampos();
                btnEstado.Text = "";
                btnEstado.BackColor = SystemColors.Control;
                btnEstado.ForeColor = Color.Black;

                // Obtener la fila seleccionada
                DataGridViewRow fila = dgvTipoPagos.Rows[e.RowIndex];

                // Obtener los valores de las columnas Cuenta y Banco
                string descripcion = fila.Cells["Descripcion"].Value?.ToString() ?? "";
                string tipoDePago = fila.Cells["TipoDePago"].Value?.ToString() ?? "";

                // Asignar valores a los TextBox
                txtDescripcion.Text = descripcion;
                txtTipoPago.Text = tipoDePago;

                // Obtener el estado desde la base de datos
                string estado = ObtenerEstadoDesdeBD(tipoDePago);

                // Asignar el texto y color del botón según el estado
                switch (estado)
                {
                    case "Activo":
                        btnEstado.Text = "Activo";
                        btnEstado.BackColor = Color.Green;
                        btnEstado.ForeColor = Color.White;
                        break;
                    case "No Activo":
                        btnEstado.Text = "No Activo";
                        btnEstado.BackColor = Color.Red;
                        btnEstado.ForeColor = Color.White;
                        break;
                    default:
                        btnEstado.Text = "Desconocido";
                        btnEstado.BackColor = SystemColors.Control;
                        btnEstado.ForeColor = Color.Black;
                        break;
                }

                // Mostrar y habilitar btnActualizar, ocultar e inhabilitar btnGuardar
                btnActualizar.Visible = true;
                btnActualizar.Enabled = true;
                btnAgregar.Visible = false;
                btnAgregar.Enabled = false;
            }
        }

        private string ObtenerEstadoDesdeBD(string tipoDePago)
        {
            string query = "SELECT Estado FROM TipoDePago WHERE TipoDePago = @TipoDePago AND ID_Proyecto = @IdProyecto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TipoDePago", tipoDePago);
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    return command.ExecuteScalar()?.ToString() ?? "Desconocido";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el Estado del Tipo De Pago: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "Desconocido";
            }
        }

        private void btnEstadoTipoDePago_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTipoPago.Text))
            {
                MessageBox.Show("Seleccione un Tipo De Pago antes de cambiar su Estado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nuevoEstado = btnEstado.Text == "Activo" ? "No Activo" : "Activo";

            if (!ConfirmarAccion($"¿Está seguro de cambiar el Estado a '{nuevoEstado}'?")) return;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand("UPDATE TipoDePago SET Estado = @Estado WHERE TipoDePago = @TipoDePago AND ID_Proyecto = @IdProyecto", connection))
                {
                    command.Parameters.AddWithValue("@Estado", nuevoEstado);
                    command.Parameters.AddWithValue("@TipoDePago", txtTipoPago.Text.Trim());
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Estado actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnEstado.Text = nuevoEstado;
                        btnEstado.BackColor = (nuevoEstado == "Activo") ? Color.Green : Color.Red;
                        btnEstado.ForeColor = Color.White;
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el Tipo De Pago para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el Estado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizarTipoDePago_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text) || string.IsNullOrWhiteSpace(txtTipoPago.Text))
            {
                MessageBox.Show("Debe completar todos los campos antes de actualizar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvTipoPagos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un Tipo De Pago para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tipoDePagoOriginal = dgvTipoPagos.CurrentRow.Cells["TipoDePago"].Value.ToString();
            string descripcionOriginal = dgvTipoPagos.CurrentRow.Cells["Descripcion"].Value.ToString();

            if (!ConfirmarAccion("¿Está seguro de que desea actualizar el Tipo De Pago?")) return;

            string query = "UPDATE TipoDePago SET Descripcion = @Descripcion, TipoDePago = @TipoDePago WHERE TipoDePago = @TipoDePagoOriginal AND Descripcion = @DescripcionOriginal AND ID_Proyecto = @IdProyecto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Descripcion", txtDescripcion.Text.Trim());
                    command.Parameters.AddWithValue("@TipoDePago", txtTipoPago.Text.Trim());
                    command.Parameters.AddWithValue("@TipoDePagoOriginal", tipoDePagoOriginal);
                    command.Parameters.AddWithValue("@DescripcionOriginal", descripcionOriginal);
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("El Tipo De Pago actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarTipoDePago(idProyecto);
                        LimpiarCampos();
                        btnActualizar.Visible = false;
                        btnActualizar.Enabled = false;
                        btnAgregar.Visible = true;
                        btnAgregar.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("No se encontró un Tipo De Pago para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el Tipo De Pago: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ConfirmarAccion(string mensaje)
        {
            return MessageBox.Show(mensaje, "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

    }
}
