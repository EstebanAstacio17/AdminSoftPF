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
    public partial class Menu : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        private int idUsuarioActual = Utilidades.IdUsuario; // ID del usuario que inició sesión

        public Menu()
        {
            InitializeComponent();

            OcultarBotonesFact();

            // Configurar el ComboBox para que no sea editable
            cboProyecto.DropDownStyle = ComboBoxStyle.DropDownList;

            // Cargar datos al ComboBox
            CargarProyectosAlCbo();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            PerfilUsuario();

            // Verificar si el permiso es "Administrador" y habilitar el botón btnCambioClave
            HabilitarBotoneAdministradores();
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Mensaje de confirmación antes de cerrar
            DialogResult resultado = MessageBox.Show(
                "¿Está seguro de que desea Salir?",
                "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.No)
            {
                e.Cancel = true; // Cancela el cierre del formulario
            }

            CerrarSesion(idUsuarioActual);
        }

        private void CerrarSesion(int idUsuario)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Sesiones_Activas SET Activa = 0, Fecha_Cierre = GETDATE() WHERE ID_Usuario = @ID_Usuario AND Activa = 1";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID_Usuario", idUsuario);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar sesión: {ex.Message}");
            }
        }


        private void btnFacturaciones_Click(object sender, EventArgs e)
        {
            // Verificar si hay un proyecto seleccionado en el ComboBox
            if (cboProyecto.SelectedItem == null || string.IsNullOrEmpty(cboProyecto.SelectedItem.ToString()))
            {
                // Mostrar mensaje de advertencia si no hay proyecto seleccionado
                MessageBox.Show("Por favor, seleccione un proyecto primero.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Salir del evento sin ejecutar el resto del código
            }

            // Alterna la visibilidad de los botones
            if (btnFactMasiv.Visible && btnFactUnica.Visible)
            {
                OcultarBotonesFact();
            }
            else
            {
                DesplegarBotonesFact();
            }
        }

        private void OcultarBotonesFact()
        {
            // Ocultar los botones al iniciar el formulario
            btnFactMasiv.Visible = false;
            btnFactUnica.Visible = false;
            btnEstatus.Visible = false;
        }

        private void DesplegarBotonesFact()
        {
            // Hacer visibles y habilitar los botones de facturación
            btnFactMasiv.Visible = true;
            btnFactUnica.Visible = true;
            btnEstatus.Visible = true;

            // Asegurar que los botones estén habilitados
            btnFactMasiv.Enabled = true;
            btnFactUnica.Enabled = true;
            btnEstatus.Enabled = true;

            // Ocultar cualquier formulario abierto en el panel antes de mostrar los botones
            CerrarFormulariosAbiertosEnPanel();
        }

        private void CerrarFormulariosAbiertosEnPanel()
        {
            // Cierra cualquier formulario abierto en el panel
            foreach (Control control in panelMenu.Controls)
            {
                if (control is Form form)
                {
                    form.Close();
                }
            }
        }

        public void AbrirFormularioEnPanel(Form formulario)
        {
            // Cierra cualquier formulario abierto en el panel
            foreach (Control control in panelMenu.Controls)
            {
                if (control is Form form)
                {
                    form.Close();
                }
            }

            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            panelMenu.Controls.Add(formulario);
            panelMenu.Tag = formulario;
            formulario.Show();
        }

        private void btnGestion_Click(object sender, EventArgs e)
        {
            // Verificar si hay un proyecto seleccionado en el ComboBox
            if (cboProyecto.SelectedItem == null || string.IsNullOrEmpty(cboProyecto.SelectedItem.ToString()))
            {
                // Mostrar mensaje de advertencia si no hay proyecto seleccionado
                MessageBox.Show("Por favor, seleccione un proyecto primero.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string proyectoSeleccionado = cboProyecto.SelectedItem.ToString();

            // Si hay un proyecto seleccionado, ejecutar la lógica normal
            OcultarBotonesFact();

            Gestion gestionForm = new Gestion(proyectoSeleccionado);
            AbrirFormularioEnPanel(gestionForm);
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            // Verificar si hay un proyecto seleccionado en el ComboBox
            if (cboProyecto.SelectedItem == null || string.IsNullOrEmpty(cboProyecto.SelectedItem.ToString()))
            {
                // Mostrar mensaje de advertencia si no hay proyecto seleccionado
                MessageBox.Show("Por favor, seleccione un proyecto primero.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Si hay un proyecto seleccionado, ejecutar la lógica normal
                OcultarBotonesFact();

                AbrirFormularioEnPanel(new Registro());
            }
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            OcultarBotonesFact();

            AbrirFormularioEnPanel(new Reportes());
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            // Permitir acceso SOLO a S Administrador
            if (lblPermisoEmpleado.Text != "S Administrador")
            {
                MessageBox.Show(
                    "No tiene permisos para acceder a este módulo.",
                    "Acceso denegado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            OcultarBotonesFact();

            AbrirFormularioEnPanel(new Usuarios());
        }


        private void btnManeger_Click(object sender, EventArgs e)
        {
            OcultarBotonesFact();

            AbrirFormularioEnPanel(new Maneger());
        }

        private void btnFactUnica_Click(object sender, EventArgs e)
        {
            OcultarBotonesFact();

            AbrirFormularioEnPanel(new FacturacionU());
        }

        private void btnFactMasiv_Click(object sender, EventArgs e)
        {
            OcultarBotonesFact();

            AbrirFormularioEnPanel(new FacturacionM());
        }

        private void HabilitarBotoneAdministradores()
        {
            // Verificar si el valor de lblPermisoEmpleado es "Administrador"
            if (lblPermisoEmpleado.Text == "S Administrador" ||
                lblPermisoEmpleado.Text == "Administrador")
            {
                // Habilitar los botones si el permiso es "Administrador"
                btnUsuarios.Enabled = true;
                btnManeger.Enabled = true;
            }
            else
            {
                // Deshabilitar los botones si no es "Administrador"
                btnUsuarios.Enabled = false;
                btnManeger.Enabled = false;
            }
        }


        private void PerfilUsuario()
        {
            // Llenar los labels con los valores de la clase Utilidades
            lblIdEmpleado.Text = $"ID Usuario: {Utilidades.IdUsuario}";

            // LLENAR NOMBRE Y APELLIDO
            lblNombreEmpleado.Text = Utilidades.NombreUsuario.ToString();
            lblApellidoEmpleado.Text = Utilidades.ApellidoUsuario.ToString();

            // Obtener el permiso del usuario y asignarlo al label
            if (string.IsNullOrEmpty(Utilidades.PermisoUsuario))
            {
                // Si el permiso no ha sido cargado, llamamos al método para obtenerlo
                Utilidades.ObtenerPermisoUsuario();
            }

            // Asignar el permiso al label correspondiente
            lblPermisoEmpleado.Text = Utilidades.PermisoUsuario ?? "Permiso no asignado";
        }

        public void CboProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProyecto.SelectedItem != null)
            {
                string nombreProyectoSeleccionado = cboProyecto.SelectedItem.ToString();

                try
                {
                    CerrarFormulariosAbiertosEnPanel(); // Cierra cualquier formulario abierto al cambiar de proyecto

                    string query = "SELECT ID_Proyecto FROM Proyecto WHERE NombreProyecto = @NombreProyecto ";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NombreProyecto", nombreProyectoSeleccionado);
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            Utilidades.IdProyectoSeleccionado = Convert.ToInt32(result);
                            Utilidades.NombreProyectoSeleccionado = nombreProyectoSeleccionado;
                            MessageBox.Show($"Proyecto seleccionado: {nombreProyectoSeleccionado}",
                                "Proyecto Seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ActualizarCuotas();
                        }
                        else
                        {
                            MessageBox.Show("No se encontró el ID del proyecto seleccionado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener el ID del proyecto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ActualizarCuotas()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is FacturacionM facturacionMForm)
                {
                    // Actualizar cuotas en FacturacionM
                    facturacionMForm.CargarCuotas(Utilidades.IdProyectoSeleccionado);
                }
                else if (form is Registro registroForm)
                {
                    // Actualizar información del proyecto en Registro
                    registroForm.InformacionDeProyectoSeleccionado();
                }
            }
        }

        private void CargarProyectosAlCbo()
        {
            string query = "SELECT NombreProyecto FROM Proyecto where EstadoProyecto = 'Activo' ";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Agregar los nombres de los proyectos al ComboBox
                            cboProyecto.Items.Add(reader["NombreProyecto"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los proyectos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFinanzas_Click(object sender, EventArgs e)
        {

            // Verificar si hay un proyecto seleccionado en el ComboBox
            if (cboProyecto.SelectedItem == null || string.IsNullOrEmpty(cboProyecto.SelectedItem.ToString()))
            {
                // Mostrar mensaje de advertencia si no hay proyecto seleccionado
                MessageBox.Show("Por favor, seleccione un proyecto primero.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                OcultarBotonesFact();

                AbrirFormularioEnPanel(new Autorizaciones());
            }




            
        }

        private void btnEstatus_Click(object sender, EventArgs e)
        {
            OcultarBotonesFact();

            AbrirFormularioEnPanel(new Estatus());
        }
    }
}