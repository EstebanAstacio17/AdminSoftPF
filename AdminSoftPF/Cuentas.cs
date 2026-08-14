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
    public partial class Cuentas : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        int idProyecto = Utilidades.IdProyectoSeleccionado;
        public Cuentas()
        {
            InitializeComponent();
        }

        private void Cuentas_Load(object sender, EventArgs e)
        {
            // Llamamos al método para cargar las cuotas
            CargarCuentas(idProyecto);

            // Asignamos límites de caracteres a los TextBox
            AsignarLimiteCaracteres();

            btnActualizarCuenta.Visible = false;
            btnActualizarCuenta.Enabled = false;
        }

        public void CargarCuentas(int idProyecto)
        {
            string query = "SELECT Banco, Cuenta FROM Cuenta WHERE ID_Proyecto = @IdProyecto";
            DataTable dtCuentas = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);
                    adapter.Fill(dtCuentas);
                }

                dgvCuentas.DataSource = dtCuentas;
                dgvCuentas.Columns["Banco"].Width = 230;
                dgvCuentas.Columns["Cuenta"].Width = 162;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las cuentas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para asignar límite de caracteres a los TextBox
        private void AsignarLimiteCaracteres()
        {
            // Asignamos un límite de 100 caracteres al TextBox que se utilice para el "Detalle"
            txtBanco.MaxLength = 20;  // Asignar límite de 100 caracteres

            // Asignamos un límite de 50 caracteres al TextBox que se utilice para la "Cuota"
            txtCuenta.MaxLength = 20;     // Asignar límite de 50 caracteres
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

        private void btnAgregarCuenta_Click(object sender, EventArgs e)
        {
            // Validar que ambos TextBox estén llenos
            if (string.IsNullOrWhiteSpace(txtBanco.Text))
            {
                MessageBox.Show("El campo 'Banco' es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCuenta.Text))
            {
                MessageBox.Show("El campo 'Cuenta' es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Si pasan la validación, insertar los datos en la base de datos
            string query = "INSERT INTO Cuenta (Banco, Cuenta, ID_Proyecto) VALUES (@Banco, @Cuenta, @IdProyecto)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add("@Banco", SqlDbType.VarChar, 100).Value = txtBanco.Text.Trim();
                    command.Parameters.Add("@Cuenta", SqlDbType.VarChar, 50).Value = txtCuenta.Text.Trim();
                    command.Parameters.Add("@IdProyecto", SqlDbType.Int).Value = idProyecto;

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Cuenta agregada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();

                // Refrescar el DataGridView
                CargarCuentas(idProyecto);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar la cuota: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            // Limpiar los TextBox
            txtBanco.Clear();
            txtCuenta.Clear();
        }

        private void dgvCuentas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar que la fila seleccionada es válida
            if (e.RowIndex >= 0)
            {
                // Limpiar los campos antes de asignar nuevos valores
                txtCuenta.Clear();
                txtBanco.Clear();
                btnEstadoCuenta.Text = "";
                btnEstadoCuenta.BackColor = SystemColors.Control;
                btnEstadoCuenta.ForeColor = Color.Black;

                // Obtener la fila seleccionada
                DataGridViewRow fila = dgvCuentas.Rows[e.RowIndex];

                // Obtener los valores de las columnas Cuenta y Banco
                string cuenta = fila.Cells["Cuenta"].Value?.ToString() ?? "";
                string banco = fila.Cells["Banco"].Value?.ToString() ?? "";

                // Asignar valores a los TextBox
                txtCuenta.Text = cuenta;
                txtBanco.Text = banco;

                // Obtener el estado desde la base de datos
                string estado = ObtenerEstadoDesdeBD(cuenta);

                // Asignar el texto y color del botón según el estado
                switch (estado)
                {
                    case "Activo":
                        btnEstadoCuenta.Text = "Activo";
                        btnEstadoCuenta.BackColor = Color.Green;
                        btnEstadoCuenta.ForeColor = Color.White;
                        break;
                    case "No Activo":
                        btnEstadoCuenta.Text = "No Activo";
                        btnEstadoCuenta.BackColor = Color.Red;
                        btnEstadoCuenta.ForeColor = Color.White;
                        break;
                    default:
                        btnEstadoCuenta.Text = "Desconocido";
                        btnEstadoCuenta.BackColor = SystemColors.Control;
                        btnEstadoCuenta.ForeColor = Color.Black;
                        break;
                }

                // Mostrar y habilitar btnActualizar, ocultar e inhabilitar btnGuardar
                btnActualizarCuenta.Visible = true;
                btnActualizarCuenta.Enabled = true;
                btnAgregarCuenta.Visible = false;
                btnAgregarCuenta.Enabled = false;
            }
        }

        private string ObtenerEstadoDesdeBD(string cuenta)
        {
            string query = "SELECT Estado FROM Cuenta WHERE Cuenta = @Cuenta AND ID_Proyecto = @IdProyecto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Cuenta", cuenta);
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    return command.ExecuteScalar()?.ToString() ?? "Desconocido";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el estado de la cuenta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "Desconocido";
            }
        }

        private void btnEstadoCuenta_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCuenta.Text))
            {
                MessageBox.Show("Seleccione una cuenta antes de cambiar su estado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nuevoEstado = btnEstadoCuenta.Text == "Activo" ? "No Activo" : "Activo";

            if (!ConfirmarAccion($"¿Está seguro de cambiar el Estado a '{nuevoEstado}'?")) return;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand("UPDATE Cuenta SET Estado = @Estado WHERE Cuenta = @Cuenta AND ID_Proyecto = @IdProyecto", connection))
                {
                    command.Parameters.AddWithValue("@Estado", nuevoEstado);
                    command.Parameters.AddWithValue("@Cuenta", txtCuenta.Text.Trim());
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Estado actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnEstadoCuenta.Text = nuevoEstado;
                        btnEstadoCuenta.BackColor = (nuevoEstado == "Activo") ? Color.Green : Color.Red;
                        btnEstadoCuenta.ForeColor = Color.White;
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la cuenta para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el estado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizarCuentas_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBanco.Text) || string.IsNullOrWhiteSpace(txtCuenta.Text))
            {
                MessageBox.Show("Debe completar todos los campos antes de actualizar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvCuentas.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una cuenta para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string cuentaOriginal = dgvCuentas.CurrentRow.Cells["Cuenta"].Value.ToString();
            string bancoOriginal = dgvCuentas.CurrentRow.Cells["Banco"].Value.ToString();

            if (!ConfirmarAccion("¿Está seguro de que desea actualizar los datos de la cuenta?")) return;

            string query = "UPDATE Cuenta SET Banco = @Banco, Cuenta = @Cuenta WHERE Cuenta = @CuentaOriginal AND Banco = @BancoOriginal AND ID_Proyecto = @IdProyecto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Banco", txtBanco.Text.Trim());
                    command.Parameters.AddWithValue("@Cuenta", txtCuenta.Text.Trim());
                    command.Parameters.AddWithValue("@CuentaOriginal", cuentaOriginal);
                    command.Parameters.AddWithValue("@BancoOriginal", bancoOriginal);
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Cuenta actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarCuentas(idProyecto);
                        LimpiarCampos();
                        btnActualizarCuenta.Visible = false;
                        btnActualizarCuenta.Enabled = false;
                        btnAgregarCuenta.Visible = true;
                        btnAgregarCuenta.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la cuenta para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar la cuenta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ConfirmarAccion(string mensaje)
        {
            return MessageBox.Show(mensaje, "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}