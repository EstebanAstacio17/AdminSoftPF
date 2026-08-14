using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Data.SqlClient;

namespace AdminSoftPF
{
    public partial class CambiarClave : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        public CambiarClave()
        {
            InitializeComponent();
        }

        private void CambiarClave_Load(object sender, EventArgs e)
        {
            // Deshabilitar el botón "Guardar" al cargar el formulario
            btnActualizarClave.Enabled = false;

            // Obtener el IdUsuarioSeleccionado desde la clase Utilidades
            int idUsuario = Utilidades.IdUsuarioSeleccionado;

            // Consultar la base de datos para obtener el password del usuario
            string passwordActual = ObtenerPasswordPorUsuario(idUsuario);

            // Mostrar la contraseña actual en el TextBox (puede ser en texto oculto)
            txtClaveActual.Text = passwordActual;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Mostrar cuadro de diálogo de confirmación
            DialogResult result = MessageBox.Show(
                "¿Estás seguro de que deseas Salir?",
                "Confirmar cierre",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // Verificar la respuesta del usuario
            if (result == DialogResult.Yes)
            {
                Close(); // Cerrar el formulario
            }
        }

        private void txtnuevaclave_TextChanged(object sender, EventArgs e)
        {
            // Validar el contenido del textbox
            if (IsPasswordValid(txtNuevaClave.Text))
            {
                // Habilitar el botón "Guardar" si la clave es válida
                btnActualizarClave.Enabled = true;
            }
            else
            {
                // Deshabilitar el botón "Guardar" si la clave no es válida
                btnActualizarClave.Enabled = false;
            }
        }


        // Función que valida la contraseña según las reglas
        private bool IsPasswordValid(string password)
        {
            // Expresión regular para validar los criterios
            string pattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{9,}$";

            // Verificar si la contraseña cumple con las condiciones
            return Regex.IsMatch(password, pattern);
        }

        // Función para obtener el password actual del usuario desde la base de datos
        private string ObtenerPasswordPorUsuario(int idUsuario)
        {
            string password = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Consulta SQL para obtener la contraseña del usuario
                    string query = "SELECT password FROM usuario WHERE id_usuario = @idUsuario";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parámetro para la consulta
                        command.Parameters.AddWithValue("@idUsuario", idUsuario);

                        connection.Open();

                        // Ejecutar la consulta y obtener el valor de la columna password
                        var result = command.ExecuteScalar();

                        if (result != null)
                        {
                            password = result.ToString(); // Asignar el valor de la contraseña
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener la contraseña: " + ex.Message);
                }
            }

            return password;
        }

        private void btnActualizarClave_Click(object sender, EventArgs e)
        {
            // Obtener el valor de la nueva contraseña
            string nuevaClave = txtNuevaClave.Text;

            // Verificar si la nueva contraseña es válida
            if (IsPasswordValid(nuevaClave))
            {
                // Obtener el Id del usuario desde la clase Utilidades
                int idUsuario = Utilidades.IdUsuarioSeleccionado;

                // Actualizar la contraseña en la base de datos
                if (ActualizarPassword(idUsuario, nuevaClave))
                {
                    // Limpiar el TextBox de la nueva clave
                    txtClaveActual.Clear();
                    txtNuevaClave.Clear();

                    // Mostrar mensaje de éxito
                    MessageBox.Show("La contraseña se ha actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Close();
                }
                else
                {
                    // Mostrar mensaje de error en caso de que la actualización falle
                    MessageBox.Show("Error al actualizar la contraseña. Intenta nuevamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("La nueva contraseña no cumple con los requisitos. Verifica que tenga al menos una mayúscula, un número, un carácter especial y 9 caracteres.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        // Función para actualizar el password en la base de datos
        private bool ActualizarPassword(int idUsuario, string nuevaClave)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Consulta SQL para actualizar la contraseña del usuario
                    string query = "UPDATE usuario SET password = @nuevaClave WHERE id_usuario = @idUsuario";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parámetros para la consulta
                        command.Parameters.AddWithValue("@nuevaClave", nuevaClave);
                        command.Parameters.AddWithValue("@idUsuario", idUsuario);

                        connection.Open();

                        // Ejecutar la consulta
                        int rowsAffected = command.ExecuteNonQuery();

                        // Si se actualizó al menos una fila, la actualización fue exitosa
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar la contraseña: " + ex.Message);
                    return false;
                }
            }
        }

    }
}
