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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Text;

namespace AdminSoftPF
{
    public partial class PerfilUsuario : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        public PerfilUsuario()
        {
            InitializeComponent();

            // Inicialmente ocultar e inhabilitar los botones Guardar y Cancelar
            InicializarEstadoBotones();

            lblUsuario.Text = "";
        }

        private void PerfilUsuario_Load(object sender, EventArgs e)
        {
            int idUsuario = Utilidades.IdUsuarioSeleccionado;

            // Cargar los datos del usuario
            CargarDatosUsuario(idUsuario);

            NoEditarComboBoxes();

            // Habilitar los controles para editar
            HabilitarControles(false);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            // Mostrar cuadro de diálogo de confirmación
            DialogResult result = MessageBox.Show(
                "¿Estás seguro de que deseas salir?",
                "Confirmar cierre",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // Verificar la respuesta del usuario
            if (result == DialogResult.Yes)
            {
                // Obtener la referencia al formulario que contiene CargarDatosUsuarios
                Usuarios formUsuarios = Application.OpenForms.OfType<Usuarios>().FirstOrDefault();

                // Verificar si la referencia es válida y llamar al método
                if (formUsuarios != null)
                {
                    formUsuarios.CargarDatosUsuarios();
                }

                Close(); // Cerrar el formulario actual
            }

        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            // Habilitar los controles para editar
            HabilitarControles(true);

            // Actualizar estado de botones
            btnActualizar.Visible = false;
            btnActualizar.Enabled = false;

            btnSalir.Visible = false;
            btnSalir.Enabled = false;

            btnGuardar.Visible = true;
            btnGuardar.Enabled = true;

            btnCancelar.Visible = true;
            btnCancelar.Enabled = true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            
            // Mostrar cuadro de diálogo de confirmación
            DialogResult result = MessageBox.Show(
                "¿Seguro que deseas Guardar los Cambios?",
                "Confirmar cierre",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // Verificar la respuesta del usuario
            if (result == DialogResult.Yes)
            {
                ActualizarCambios();
            }
            


        }

        private void ActualizarCambios()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta para actualizar los datos del usuario
                    string queryUsuario = "UPDATE usuario SET NombreUsuario = @Nombre, ApellidoUsuario = @Apellido, Documento = @Documento, Correo = @Correo, Celular = @Celular, EstadoUsuario = @Estado WHERE ID_Usuario = @IdUsuario";

                    using (SqlCommand commandUsuario = new SqlCommand(queryUsuario, connection))
                    {
                        // Asignar parámetros para la tabla usuario
                        commandUsuario.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                        commandUsuario.Parameters.AddWithValue("@Apellido", txtApellido.Text);
                        commandUsuario.Parameters.AddWithValue("@Documento", txtDocumento.Text);
                        commandUsuario.Parameters.AddWithValue("@Correo", txtCorreo.Text);
                        commandUsuario.Parameters.AddWithValue("@Celular", txtCelular.Text);
                        commandUsuario.Parameters.AddWithValue("@Estado", cboEstado.Text);
                        commandUsuario.Parameters.AddWithValue("@IdUsuario", Utilidades.IdUsuarioSeleccionado);

                        // Ejecutar la consulta para actualizar el usuario
                        commandUsuario.ExecuteNonQuery();
                    }

                    // Consulta para actualizar el permiso
                    string queryPermiso = "UPDATE permiso SET Permiso = @Permiso WHERE ID_Usuario = @IdUsuario";

                    using (SqlCommand commandPermiso = new SqlCommand(queryPermiso, connection))
                    {
                        // Asignar parámetros para la tabla permiso
                        commandPermiso.Parameters.AddWithValue("@Permiso", cboPermisos.Text);
                        commandPermiso.Parameters.AddWithValue("@IdUsuario", Utilidades.IdUsuarioSeleccionado);

                        // Ejecutar la consulta para actualizar el permiso
                        commandPermiso.ExecuteNonQuery();
                    }

                    MessageBox.Show("Datos actualizados correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Deshabilitar los controles después de guardar
                    HabilitarControles(false);

                    //HabilitarBotonGuardarCambios();

                    // Restaurar el estado de los botones
                    InicializarEstadoBotones();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatosUsuario(int idUsuario)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta SQL para obtener los datos del usuario
                    string queryUsuario = "SELECT NombreUsuario, ApellidoUsuario, Documento, Correo, Celular, Usuario, EstadoUsuario FROM usuario WHERE ID_Usuario = @IdUsuario";

                    using (SqlCommand command = new SqlCommand(queryUsuario, connection))
                    {
                        command.Parameters.AddWithValue("@IdUsuario", idUsuario);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Llenar los controles con los datos obtenidos
                                txtNombre.Text = reader["NombreUsuario"].ToString();
                                txtApellido.Text = reader["ApellidoUsuario"].ToString();
                                txtDocumento.Text = reader["Documento"].ToString();
                                txtCelular.Text = reader["Celular"].ToString();
                                txtCorreo.Text = reader["Correo"].ToString();
                                lblUsuario.Text = reader["Usuario"].ToString();

                                // Configurar estado del usuario
                                string estadoUsuario = reader["EstadoUsuario"].ToString();
                                cboEstado.SelectedItem = cboEstado.Items.Contains(estadoUsuario) ? estadoUsuario : null;
                            }
                        }
                    }

                    // Consulta para obtener el permiso del usuario
                    string queryPermiso = "SELECT Permiso FROM permiso WHERE ID_Usuario = @IdUsuario";
                    using (SqlCommand commandPermiso = new SqlCommand(queryPermiso, connection))
                    {
                        commandPermiso.Parameters.AddWithValue("@IdUsuario", idUsuario);

                        using (SqlDataReader readerPermiso = commandPermiso.ExecuteReader())
                        {
                            if (readerPermiso.Read())
                            {
                                string permisoBD = readerPermiso["Permiso"].ToString();

                                // Verificar si el permiso obtenido coincide con los valores predefinidos del combo box
                                if (cboPermisos.Items.Contains(permisoBD))
                                {
                                    cboPermisos.SelectedItem = permisoBD; // Seleccionar el valor que coincide
                                }
                                else
                                {
                                    MessageBox.Show($"El permiso '{permisoBD}' no coincide con las opciones disponibles.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    cboPermisos.SelectedIndex = -1; // Deseleccionar si no coincide
                                }
                            }
                            else
                            {
                                cboPermisos.SelectedIndex = -1; // Deseleccionar si no hay permisos en la BD
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HabilitarControles(bool habilitar)
        {
            // Habilitar o deshabilitar los controles según el parámetro
            txtNombre.Enabled = habilitar;
            txtApellido.Enabled = habilitar;
            txtDocumento.Enabled = habilitar;
            txtCelular.Enabled = habilitar;
            txtCorreo.Enabled = habilitar;
            cboEstado.Enabled = habilitar;
            cboPermisos.Enabled = habilitar;
        }
        
        private void NoEditarComboBoxes()
        {
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPermisos.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void btnCambioClave_Click(object sender, EventArgs e)
        {
            // Crear una instancia de la clase Utilidades
            Utilidades utilidades = new Utilidades();

            // Abrir el formulario Gestion dentro de un panel específico
            utilidades.AbrirFormularioEnPanel(new CambiarClave(), panelUsuarios); // Asegúrate de que `panelMenu` sea accesible
        }

        private void btnComentarios_Click(object sender, EventArgs e)
        {
            // Crear una instancia de la clase Utilidades
            Utilidades utilidades = new Utilidades();

            // Abrir el formulario Gestion dentro de un panel específico
            utilidades.AbrirFormularioEnPanel(new ComentariosUsuarios(), panelUsuarios); // Asegúrate de que `panelMenu` sea accesible
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

            // Mostrar cuadro de diálogo de confirmación
            DialogResult result = MessageBox.Show(
                "¿Seguro que deseas Cancelar?",
                "Confirmar cierre",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // Verificar la respuesta del usuario
            if (result == DialogResult.Yes)
            {
                // Deshabilitar los controles
                HabilitarControles(false);

                // Restaurar el estado de los botones
                InicializarEstadoBotones();

                // Recargar los datos originales del usuario
                CargarDatosUsuario(Utilidades.IdUsuarioSeleccionado);
            }
        }

        private void InicializarEstadoBotones()
        {
            // Estado inicial de los botones
            btnGuardar.Visible = false;
            btnGuardar.Enabled = false;

            btnCancelar.Visible = false;
            btnCancelar.Enabled = false;

            btnActualizar.Visible = true;
            btnActualizar.Enabled = true;

            btnSalir.Visible = true;
            btnSalir.Enabled = true;
        }

    }
}
