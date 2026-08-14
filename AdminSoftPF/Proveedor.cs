using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace AdminSoftPF
{
    public partial class Proveedor: Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        int idProyecto = Utilidades.IdProyectoSeleccionado;
        public Proveedor()
        {
            InitializeComponent();
        }

        private void Proveedor_Load(object sender, EventArgs e)
        {
            // Llamamos al método para cargar las cuotas
            CargarProveedor(idProyecto);

            // Asignamos límites de caracteres a los TextBox
            AsignarLimiteCaracteres();

            btnActualizarProveedor.Visible = false;
            btnActualizarProveedor.Enabled = false;
        }

        public void CargarProveedor(int idProyecto)
        {
            string query = "SELECT Identificacion, Proveedor FROM Proveedor WHERE ID_Proyecto = @IdProyecto";
            DataTable dtProveedor = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);
                    adapter.Fill(dtProveedor);
                }

                dgvProveedor.DataSource = dtProveedor;
                dgvProveedor.Columns["Identificacion"].Width = 120;
                dgvProveedor.Columns["Proveedor"].Width = 272;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar Proveedores: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AsignarLimiteCaracteres()
        {
            txtIdentificacion.MaxLength = 13;

            txtProveedor.MaxLength = 50;
        }

        private void txtSoloNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void txtSoloNumeros_TextChanged(object sender, EventArgs e)
        {
            // Patrones de validación para RNC, cédula y pasaporte
            string patronRNC = @"^\d{1}-\d{2}-\d{5}-\d{1}$";
            string patronCedula = @"^\d{3}-\d{7}-\d{1}$";
            string patronPasaporte = @"^RD\d{7}$";

            // Valida el contenido del TextBox
            if (Regex.IsMatch(txtIdentificacion.Text, patronRNC) ||
                Regex.IsMatch(txtIdentificacion.Text, patronCedula) ||
                Regex.IsMatch(txtIdentificacion.Text, patronPasaporte))
            {
                txtIdentificacion.ForeColor = Color.Green;
            }
            else
            {
                txtIdentificacion.ForeColor = Color.Red;
            }
        }

        private void txtSoloLetras_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        private void btnAgregarProveedor_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdentificacion.Text))
            {
                MessageBox.Show("El campo 'Identificación' es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtProveedor.Text))
            {
                MessageBox.Show("El campo 'Proveedor' es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!EsIdentificacionValida(txtIdentificacion.Text))
            {
                MessageBox.Show("El formato de 'Identificación' no es válido. Acepta RNC, Cédula o Pasaporte.",
                                "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO Proveedor (Identificacion, Proveedor, ID_Proyecto) VALUES (@Identificacion, @Proveedor, @IdProyecto)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add("@Identificacion", SqlDbType.VarChar, 100).Value = txtIdentificacion.Text.Trim();
                    command.Parameters.Add("@Proveedor", SqlDbType.VarChar, 100).Value = txtProveedor.Text.Trim();
                    command.Parameters.Add("@IdProyecto", SqlDbType.Int).Value = idProyecto;

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Proveedor agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();

                CargarProveedor(idProyecto);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar Proveedor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private bool EsIdentificacionValida(string identificacion)
        {
            string patronRNC = @"^\d{1}-\d{2}-\d{5}-\d{1}$";
            string patronCedula = @"^\d{3}-\d{7}-\d{1}$";
            string patronPasaporte = @"^RD\d{7}$";

            return Regex.IsMatch(identificacion, patronRNC) ||
                   Regex.IsMatch(identificacion, patronCedula) ||
                   Regex.IsMatch(identificacion, patronPasaporte);
        }

        private void LimpiarCampos()
        {
            txtIdentificacion.Clear();
            txtProveedor.Clear();
        }

        private void dgvProveedor_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtIdentificacion.Clear();
                txtProveedor.Clear();
                btnEstadoProveedor.Text = "";
                btnEstadoProveedor.BackColor = SystemColors.Control;
                btnEstadoProveedor.ForeColor = Color.Black;

                DataGridViewRow fila = dgvProveedor.Rows[e.RowIndex];

                string rnc = fila.Cells["Identificacion"].Value?.ToString() ?? "";
                string proveedor = fila.Cells["Proveedor"].Value?.ToString() ?? "";

                txtIdentificacion.Text = rnc;
                txtProveedor.Text = proveedor;

                string estado = ObtenerEstadoDesdeBD(rnc);

                switch (estado)
                {
                    case "Activo":
                        btnEstadoProveedor.Text = "Activo";
                        btnEstadoProveedor.BackColor = Color.Green;
                        btnEstadoProveedor.ForeColor = Color.White;
                        break;
                    case "No Activo":
                        btnEstadoProveedor.Text = "No Activo";
                        btnEstadoProveedor.BackColor = Color.Red;
                        btnEstadoProveedor.ForeColor = Color.White;
                        break;
                    default:
                        btnEstadoProveedor.Text = "Desconocido";
                        btnEstadoProveedor.BackColor = SystemColors.Control;
                        btnEstadoProveedor.ForeColor = Color.Black;
                        break;
                }

                btnActualizarProveedor.Visible = true;
                btnActualizarProveedor.Enabled = true;
                btnAgregarProveedor.Visible = false;
                btnAgregarProveedor.Enabled = false;
            }
        }

        private string ObtenerEstadoDesdeBD(string rnc)
        {
            string query = "SELECT Estado FROM Proveedor WHERE Identificacion = @Identificacion AND ID_Proyecto = @IdProyecto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Identificacion", rnc);
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    return command.ExecuteScalar()?.ToString() ?? "Desconocido";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el estado del Proveedor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "Desconocido";
            }
        }

        private void btnEstadoProveedor_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdentificacion.Text))
            {
                MessageBox.Show("Seleccione un Proveedor antes de cambiar su estado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nuevoEstado = btnEstadoProveedor.Text == "Activo" ? "No Activo" : "Activo";

            if (!ConfirmarAccion($"¿Está seguro de cambiar el Estado a '{nuevoEstado}'?")) return;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand("UPDATE Proveedor SET Estado = @Estado WHERE Identificacion = @Identificacion AND ID_Proyecto = @IdProyecto", connection))
                {
                    command.Parameters.AddWithValue("@Estado", nuevoEstado);
                    command.Parameters.AddWithValue("@Identificacion", txtIdentificacion.Text.Trim());
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Proveedor actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnEstadoProveedor.Text = nuevoEstado;
                        btnEstadoProveedor.BackColor = (nuevoEstado == "Activo") ? Color.Green : Color.Red;
                        btnEstadoProveedor.ForeColor = Color.White;
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el Proveedor para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el estado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizarProveedor_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProveedor.Text) || string.IsNullOrWhiteSpace(txtIdentificacion.Text))
            {
                MessageBox.Show("Debe completar todos los campos antes de actualizar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvProveedor.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un Proveedor para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string rncOriginal = dgvProveedor.CurrentRow.Cells["Identificacion"].Value.ToString();
            string proveedorOriginal = dgvProveedor.CurrentRow.Cells["Proveedor"].Value.ToString();

            if (!ConfirmarAccion("¿Está seguro de que desea actualizar los datos del Proveedor?")) return;

            string query = "UPDATE Proveedor SET Identificacion = @Identificacion, Proveedor = @Proveedor WHERE Identificacion = @IdentificacionOriginal AND Proveedor = @ProveedorOriginal AND ID_Proyecto = @IdProyecto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Identificacion", txtIdentificacion.Text.Trim());
                    command.Parameters.AddWithValue("@Proveedor", txtProveedor.Text.Trim());
                    command.Parameters.AddWithValue("@IdentificacionOriginal", rncOriginal);
                    command.Parameters.AddWithValue("@ProveedorOriginal", proveedorOriginal);
                    command.Parameters.AddWithValue("@IdProyecto", idProyecto);

                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Proveedor actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarProveedor(idProyecto);
                        LimpiarCampos();
                        btnActualizarProveedor.Visible = false;
                        btnActualizarProveedor.Enabled = false;
                        btnAgregarProveedor.Visible = true;
                        btnAgregarProveedor.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el Proveedor para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el Proveedor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private bool ConfirmarAccion(string mensaje)
        {
            return MessageBox.Show(mensaje, "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        
    }
}
