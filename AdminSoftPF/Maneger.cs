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
    public partial class Maneger : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        public Maneger()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void Maneger_Load(object sender, EventArgs e)
        {
            try
            {
                LlenarComboProyecto();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los proyectos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void InitializeForm()
        {
            ConfigureButtons();
            ConfigureComboBoxes();
            DisableControls();
        }

        private void ConfigureButtons()
        {
            btnGuardar.Enabled = false;
            btnActualizar.Enabled = true;
            btnAgregar.Enabled = false;
            btnAgregar.Visible = false;
        }

        private void ConfigureComboBoxes()
        {
            cboProyecto.SelectedIndex = -1;
            cboProyecto.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstadoProyecto.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (!ValidarSeleccionProyecto()) return;

            ToggleEditMode(true);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarSeleccionProyecto()) return;

            if (!ValidarCamposProyecto())
            {
                MessageBox.Show("Por favor, complete todos los campos antes de actualizar el proyecto.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ActualizarProyecto();
                LlenarComboProyecto();
                LimpiarCampos();
                ResetButtonsState();
                MessageBox.Show("Proyecto actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el proyecto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ActualizarProyecto()
        {
            string query = @"
                UPDATE Proyecto 
                SET 
                    RncProyecto = @RncProyecto,
                    DireccionProyecto = @DireccionProyecto,
                    Oficina = @Oficina,
                    Telefono = @Telefono,
                    Correo = @Correo,
                    EstadoProyecto = @EstadoProyecto
                WHERE NombreProyecto = @NombreProyecto";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@NombreProyecto", cboProyecto.SelectedItem.ToString());
                command.Parameters.AddWithValue("@RncProyecto", txtRnc.Text);
                command.Parameters.AddWithValue("@DireccionProyecto", txtDireccion.Text);
                command.Parameters.AddWithValue("@Oficina", txtOficina.Text);
                command.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                command.Parameters.AddWithValue("@Correo", txtCorreo.Text);
                command.Parameters.AddWithValue("@EstadoProyecto", cboEstadoProyecto.SelectedItem.ToString());

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new Exception("No se encontró el proyecto a actualizar. Verifique los datos ingresados.");
                }
            }
        }

        private bool ValidarSeleccionProyecto()
        {
            if (cboProyecto.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, selecciona un proyecto antes de continuar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void ToggleEditMode(bool enable)
        {
            EnableControls(enable);

            btnActualizar.Visible = !enable;
            btnActualizar.Enabled = !enable;

            btnGuardar.Visible = enable;
            btnGuardar.Enabled = enable;
        }

        private void ResetButtonsState()
        {
            btnActualizar.Enabled = true;
            btnActualizar.Visible = true;
            btnGuardar.Enabled = false;
            btnGuardar.Visible = false;
        }

        private void EnableControls(bool enable = true)
        {
            txtNombre.Enabled = enable;
            txtRnc.Enabled = enable;
            txtDireccion.Enabled = enable;
            txtOficina.Enabled = enable;
            txtTelefono.Enabled = enable;
            txtCorreo.Enabled = enable;
            cboEstadoProyecto.Enabled = enable;
        }

        private void DisableControls()
        {
            EnableControls(false);
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtRnc.Clear();
            txtDireccion.Clear();
            txtOficina.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            cboProyecto.SelectedIndex = -1;
            cboEstadoProyecto.SelectedIndex = -1;
            DisableControls();
        }

        private bool ValidarCamposProyecto()
        {
            return !string.IsNullOrWhiteSpace(txtNombre.Text) &&
                   !string.IsNullOrWhiteSpace(txtRnc.Text) &&
                   !string.IsNullOrWhiteSpace(txtDireccion.Text) &&
                   !string.IsNullOrWhiteSpace(txtOficina.Text) &&
                   !string.IsNullOrWhiteSpace(txtTelefono.Text) &&
                   !string.IsNullOrWhiteSpace(txtCorreo.Text) &&
                   cboEstadoProyecto.SelectedIndex != -1;
        }

        private void LlenarComboProyecto()
        {
            cboProyecto.Items.Clear();
            string query = "SELECT NombreProyecto FROM Proyecto";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cboProyecto.Items.Add(reader["NombreProyecto"].ToString());
                    }
                }
            }
        }

        private void btnCuentas_Click(object sender, EventArgs e)
        {
            if (!ValidarSeleccionProyecto()) return;

            AbrirFormularioEnPanel(new Cuentas());
        }

        private void btnCuotas_Click(object sender, EventArgs e)
        {
            if (!ValidarSeleccionProyecto()) return;

            AbrirFormularioEnPanel(new Cuotas());
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Estás seguro de que desea Cerrar?", "Confirmar cierre", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            ResetButtonsState();
            ResetFormulariosPanel();
        }

        private void btnNuevoProyecto_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            EnableControls();
            btnAgregar.Enabled = true;
            btnAgregar.Visible = true;

            ResetFormulariosPanel();

            btnGuardar.Enabled = false;
            btnGuardar.Visible = false;
            btnActualizar.Enabled = true;
            btnActualizar.Visible = true;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!ValidarCamposProyecto())
            {
                MessageBox.Show("Por favor, complete todos los campos antes de agregar el proyecto.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ExisteProyecto(txtNombre.Text))
            {
                MessageBox.Show("El proyecto con este nombre ya existe. Por favor, elija otro nombre.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GuardarProyecto();
            LlenarComboProyecto();
            LimpiarCampos();
            btnAgregar.Enabled = false;
            btnAgregar.Visible = false;
        }

        private bool ExisteProyecto(string nombreProyecto)
        {
            string query = "SELECT COUNT(*) FROM Proyecto WHERE NombreProyecto = @NombreProyecto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NombreProyecto", nombreProyecto);
                    connection.Open();

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al validar la existencia del proyecto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true; // Si ocurre un error, asumimos que existe para evitar duplicados.
            }
        }

        private void cboProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProyecto.SelectedIndex == -1) return;

            ObtenerDetallesProyecto(cboProyecto.SelectedItem.ToString());
            ResetFormulariosPanel();

            btnAgregar.Enabled = false;
            btnAgregar.Visible = false;
        }

        private void ResetFormulariosPanel()
        {
            foreach (Control control in panelDetalles.Controls)
            {
                if (control is Form form)
                {
                    form.Close();
                }
            }
        }

        private void ObtenerDetallesProyecto(string nombreProyecto)
        {
            string query = "SELECT * FROM Proyecto WHERE NombreProyecto = @NombreProyecto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NombreProyecto", nombreProyecto);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            LlenarDetallesProyecto(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los detalles del proyecto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LlenarDetallesProyecto(SqlDataReader reader)
        {
            txtNombre.Text = reader["NombreProyecto"].ToString();
            txtRnc.Text = reader["RncProyecto"].ToString();
            txtDireccion.Text = reader["DireccionProyecto"].ToString();
            txtOficina.Text = reader["Oficina"].ToString();
            txtTelefono.Text = reader["Telefono"].ToString();
            txtCorreo.Text = reader["Correo"].ToString();
            cboEstadoProyecto.SelectedItem = reader["EstadoProyecto"].ToString();
            Utilidades.IdProyectoSeleccionado = Convert.ToInt32(reader["ID_Proyecto"]);
        }

        private void AbrirFormularioEnPanel(Form formulario)
        {
            ResetFormulariosPanel();
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            panelDetalles.Controls.Add(formulario);
            formulario.Show();
        }

        private void GuardarProyecto()
        {
            string query = "INSERT INTO Proyecto (NombreProyecto, RncProyecto, DireccionProyecto, Oficina, Telefono, Correo, EstadoProyecto) " +
                           "VALUES (@NombreProyecto, @RncProyecto, @DireccionProyecto, @Oficina, @Telefono, @Correo, @EstadoProyecto)";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NombreProyecto", txtNombre.Text);
                    command.Parameters.AddWithValue("@RncProyecto", txtRnc.Text);
                    command.Parameters.AddWithValue("@DireccionProyecto", txtDireccion.Text);
                    command.Parameters.AddWithValue("@Oficina", txtOficina.Text);
                    command.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                    command.Parameters.AddWithValue("@Correo", txtCorreo.Text);
                    command.Parameters.AddWithValue("@EstadoProyecto", cboEstadoProyecto.SelectedItem.ToString());

                    connection.Open();
                    command.ExecuteNonQuery();

                    MessageBox.Show("Proyecto agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el proyecto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnManzana_Click(object sender, EventArgs e)
        {
            if (!ValidarSeleccionProyecto()) return;

            AbrirFormularioEnPanel(new Manzana());
        }

        private void btnEdificio_Click(object sender, EventArgs e)
        {
            if (!ValidarSeleccionProyecto()) return;

            AbrirFormularioEnPanel(new Edificio());
        }

        private void btnApartamento_Click(object sender, EventArgs e)
        {
            if (!ValidarSeleccionProyecto()) return;

            AbrirFormularioEnPanel(new Apartamento());
        }

        private void btnProveedor_Click(object sender, EventArgs e)
        {
            if (!ValidarSeleccionProyecto()) return;

            AbrirFormularioEnPanel(new Proveedor());
        }

        private void btnTipoPagos_Click(object sender, EventArgs e)
        {
            if (!ValidarSeleccionProyecto()) return;

            AbrirFormularioEnPanel(new TipoPagos());
        }
    }
}