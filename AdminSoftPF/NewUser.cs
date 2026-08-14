using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AdminSoftPF
{
    public partial class NewUser : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        public NewUser()
        {
            InitializeComponent();

            // Inicialmente deshabilitar el botón de grabar
            btnGuardar.Enabled = false;

            //Inicialmente limpiar el label de usuario
            lblUsuario.Text = "";
        }

        private void NewUser_Load(object sender, EventArgs e)
        {


            
            NoEditarComboBoxes();
            

            LimiteDeTextBoxes();
            
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            

            
            // Mostrar cuadro de diálogo de confirmación
            DialogResult result = MessageBox.Show(
                "¿Estás seguro de que deseas limpiar los Campos?",
                "Confirmar cierre",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // Verificar la respuesta del usuario
            if (result == DialogResult.Yes)
            {
                LimpiarCampos(); // Limpiar el formulario
            }
            
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtCorreo.Clear();
            txtDocumento.Clear();
            txtCelular.Clear();

            lblUsuario.Text = "";

            txtPassword1.Clear();
            txtPassword2.Clear();

            cboEstado.SelectedIndex = -1;
            cboPermisos.SelectedIndex = -1;
        }


        private void NoEditarComboBoxes()
        {
            cboPermisos.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
        }



        private void LimiteDeTextBoxes()
        {
            // Establecer la propiedad MaxLength de cada TextBox a 20 caracteres
            txtNombre.MaxLength = 20;
            txtApellido.MaxLength = 20;
            txtCorreo.MaxLength = 35;
            txtDocumento.MaxLength = 11;
            txtCelular.MaxLength = 10;
            txtPassword1.MaxLength = 20;
            txtPassword2.MaxLength = 20;
        }

        private void TextBoxLettersOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verificar si el carácter presionado es una letra, una tecla de control o un espacio
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
            {
                // Si no es una letra, una tecla de control ni un espacio, cancelar el evento
                e.Handled = true;
            }
        }

        private void TextBoxNumbersOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verificar si el carácter presionado es un número o una tecla de control
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                // Si no es un número ni una tecla de control, cancelar el evento
                e.Handled = true;
            }
        }


        // Método para actualizar Usuario basado en textBox1 y textBox2
        private void CrearUsuario(object sender, EventArgs e)
        {
            // Obtener los dos primeros caracteres de txtNombreCompleto
            string partNombre = txtNombre.Text.Length >= 2 ? txtNombre.Text.Substring(0, 2) : txtNombre.Text;

            // Obtener la primera palabra de txtApellidoCompleto
            string partApellido = txtApellido.Text;
            int spaceIndex = partApellido.IndexOf(' ');
            string firstWord = spaceIndex > 0 ? partApellido.Substring(0, spaceIndex) : partApellido;

            // Combinar los textos, convertir a mayúsculas y asignarlos a txtUsuario
            lblUsuario.Text = (partNombre + firstWord).ToUpper();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
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

        private void txtCorreo_Leave(object sender, EventArgs e)
        {
            // Verificar si el TextBox contiene el carácter '@'
            if (!txtCorreo.Text.Contains("@"))
            {
                MessageBox.Show("El texto debe contener un '@'.", "Validación Incorrecta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCorreo.Focus(); // Volver al TextBox si la validación falla
            }
        }


        private void txtPassword1_TextChanged(object sender, EventArgs e)
        {
            ValidarPasswords();
        }

        private void txtPassword2_TextChanged(object sender, EventArgs e)
        {
            ValidarPasswords();
        }

        private void ValidarPasswords()
        {
            string password1 = txtPassword1.Text;
            string password2 = txtPassword2.Text;

            if (EsContraseñaValida(password1) && password1 == password2)
            {
                btnGuardar.Enabled = true;
            }
            else
            {
                btnGuardar.Enabled = false;
            }
        }

        private bool EsContraseñaValida(string contraseña)
        {
            string pattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*\W).{9,}$";
            return Regex.IsMatch(contraseña, pattern);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            // Validar si todos los campos tienen valores
            if (!ValidarCamposObligatorios())
            {
                return; // Salir si algún campo está vacío
            }

            if (!ValidarExistenciaUsuarioODocumento())
            {
                return;
            }

            // Intentar guardar el usuario
            if (GuardarUsuario())
            {
                // Mostrar mensaje de éxito si se guardó correctamente
                MessageBox.Show("Usuario guardado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar el formulario
                LimpiarCampos();
            }
            else
            {
                // Mostrar mensaje de error en caso de fallo
                MessageBox.Show("Ocurrió un error al guardar el usuario. Por favor, intente nuevamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private bool GuardarUsuario()
        {
            string nombreUsuario = txtNombre.Text.Trim();
            string apellidoUsuario = txtApellido.Text.Trim();
            string documento = txtDocumento.Text.Trim();
            string usuario = lblUsuario.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            string password = txtPassword2.Text;
            string estado = cboEstado.SelectedItem?.ToString() ?? "";
            string celular = txtCelular.Text.Trim();
            string permiso = cboPermisos.SelectedItem?.ToString() ?? "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        // Insertar en la tabla Usuario
                        string insertUsuarioQuery = @"
                INSERT INTO Usuario (NombreUsuario, ApellidoUsuario, Documento, Correo, Celular, Usuario, Password, EstadoUsuario)
                OUTPUT INSERTED.ID_Usuario
                VALUES (@NombreUsuario, @ApellidoUsuario, @Documento, @Correo, @Celular, @Usuario, @Password, @Estado)";

                        SqlCommand cmdUsuario = new SqlCommand(insertUsuarioQuery, connection, transaction);
                        cmdUsuario.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                        cmdUsuario.Parameters.AddWithValue("@ApellidoUsuario", apellidoUsuario);
                        cmdUsuario.Parameters.AddWithValue("@Documento", documento);
                        cmdUsuario.Parameters.AddWithValue("@Usuario", usuario);
                        cmdUsuario.Parameters.AddWithValue("@Correo", correo);
                        cmdUsuario.Parameters.AddWithValue("@Password", password);
                        cmdUsuario.Parameters.AddWithValue("@Estado", estado);
                        cmdUsuario.Parameters.AddWithValue("@Celular", celular);

                        object result = cmdUsuario.ExecuteScalar();
                        if (result == null || !int.TryParse(result.ToString(), out int idUsuario))
                        {
                            transaction.Rollback();
                            MessageBox.Show("Error al obtener el ID del usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        // Insertar en la tabla Permiso (sin el campo ID_Permiso si es autoincremental)
                        string insertPermisoQuery = @"
                INSERT INTO Permiso (ID_Permiso, ID_Usuario, Permiso)
                VALUES (@IdPermiso, @IdUsuario, @Permiso)";

                        SqlCommand cmdPermiso = new SqlCommand(insertPermisoQuery, connection, transaction);
                        cmdPermiso.Parameters.AddWithValue("@IdPermiso", idUsuario);
                        cmdPermiso.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        cmdPermiso.Parameters.AddWithValue("@Permiso", permiso);
                        cmdPermiso.ExecuteNonQuery();

                        // Confirmar la transacción
                        transaction.Commit();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
        }

        private bool ValidarExistenciaUsuarioODocumento()
        {
            bool usuarioExiste = false;
            bool documentoExiste = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT usuario, documento FROM usuario WHERE usuario = @Usuario OR documento = @Documento";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Usuario", lblUsuario.Text);
                    command.Parameters.AddWithValue("@Documento", txtDocumento.Text);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["usuario"].ToString() == lblUsuario.Text)
                            {
                                usuarioExiste = true;
                            }
                            if (reader["documento"].ToString() == txtDocumento.Text)
                            {
                                documentoExiste = true;
                            }
                        }
                    }
                }
            }

            if (usuarioExiste && documentoExiste)
            {
                MessageBox.Show("El usuario y el documento ya existen en el sistema.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (usuarioExiste)
            {
                MessageBox.Show("El usuario ya existe en el sistema.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (documentoExiste)
            {
                MessageBox.Show("El documento ya existe en el sistema.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }


        private bool ValidarCamposObligatorios()
        {
            // Verificar cada campo obligatorio
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MostrarMensajeCampoVacio("Nombre");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MostrarMensajeCampoVacio("Apellido");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                MostrarMensajeCampoVacio("Correo");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtDocumento.Text))
            {
                MostrarMensajeCampoVacio("Documento");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtCelular.Text))
            {
                MostrarMensajeCampoVacio("Celular");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPassword1.Text))
            {
                MostrarMensajeCampoVacio("Contraseña");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPassword2.Text))
            {
                MostrarMensajeCampoVacio("Confirmación de contraseña");
                return false;
            }

            return true; // Todos los campos están llenos
        }

        private void MostrarMensajeCampoVacio(string campo)
        {
            MessageBox.Show($"El campo '{campo}' no puede estar vacío.", "Campo Obligatorio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


    }
}
