using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminSoftPF
{
    public class Utilidades
    {
        // Cadena de conexión desde el archivo de configuración
        private static string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        // Propiedad para almacenar el permiso obtenido
        public static string PermisoUsuario { get; set; }

        public void AbrirFormularioEnPanel(Form formulario, Panel panelMenu)
        {
            // Limpiar el panel antes de abrir un nuevo formulario
            panelMenu.Controls.Clear();

            // Configurar el formulario para abrirse dentro del panel
            formulario.TopLevel = false;
            formulario.Dock = DockStyle.Fill;

            // Agregar el formulario al panel y mostrarlo
            panelMenu.Controls.Add(formulario);
            formulario.Show();
        }

        public static int IdUsuarioSeleccionado { get; set; }


        // LOGIN MENU PERFIL USUARIO ACTIVO
        public static int IdUsuario { get; set; }
        public static string Usuario { get; set; }
        public static string EstadoUsuario { get; set; }
        public static string NombreUsuario { get; set; }
        public static string ApellidoUsuario { get; set; }

        
        // Método para obtener el permiso basado en el IdUsuario
        public static void ObtenerPermisoUsuario()
        {
            try
            {
                // Cadena de conexión desde el archivo de configuración
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta SQL para obtener el permiso
                    string query = "SELECT Permiso FROM permiso WHERE ID_Usuario = @IdUsuario";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Asignar el valor del IdUsuario como parámetro
                        command.Parameters.AddWithValue("@IdUsuario", IdUsuario);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Almacenar el valor de Permiso en la propiedad estática
                                PermisoUsuario = reader["Permiso"].ToString();
                            }
                            else
                            {
                                // Manejar el caso donde no se encuentre un permiso
                                PermisoUsuario = null;
                                MessageBox.Show("No se encontró un permiso para el usuario.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar errores en la conexión o consulta
                MessageBox.Show($"Error al obtener el permiso: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PermisoUsuario = null;
            }
        }


        // Nueva variable pública para almacenar el proyecto seleccionado
        public static string NombreProyectoSeleccionado { get; set; }

        // Variable pública para almacenar el IdProyecto
        public static int IdProyectoSeleccionado { get; set; }

        public static string DireccionCompleta { get; set; }

        public static int ID_Direccion { get; set; }

        // Variables estáticas para almacenar los valores de las columnas de PROYECTO
        public static void CargarDatosProyecto()
        {
            // Verificar si el ID del proyecto seleccionado es válido
            if (IdProyectoSeleccionado <= 0)
            {
                throw new InvalidOperationException("El ID del proyecto seleccionado no es válido.");
            }

            try
            {
                // Crear la conexión y el comando SQL
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                    SELECT DireccionProyecto, RncProyecto, Oficina, Telefono, Correo
                    FROM Proyecto
                    WHERE ID_Proyecto = @IdProyecto";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Agregar el parámetro al comando
                        command.Parameters.AddWithValue("@IdProyecto", IdProyectoSeleccionado);

                        // Ejecutar la consulta y leer los resultados
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DireccionProyecto = reader["DireccionProyecto"]?.ToString();
                                RncProyecto = reader["RncProyecto"]?.ToString();
                                Oficina = reader["Oficina"]?.ToString();
                                Telefono = reader["Telefono"]?.ToString();
                                Correo = reader["Correo"]?.ToString();
                            }
                            else
                            {
                                throw new Exception("No se encontraron datos para el proyecto seleccionado.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar errores (puedes usar un logger o mostrar un mensaje al usuario)
                throw new Exception("Error al cargar los datos del proyecto: " + ex.Message, ex);
            }
        }

        public static string DireccionProyecto { get; private set; }
        public static string RncProyecto { get; private set; }
        public static string Oficina { get; private set; }
        public static string Telefono { get; private set; }
        public static string Correo { get; private set; }

        



    }
}
