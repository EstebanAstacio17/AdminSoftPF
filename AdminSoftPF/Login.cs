using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Data.SqlClient;

namespace AdminSoftPF
{
    public partial class Login : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        public Login()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None; // Sin borde para aplicar esquinas redondeadas
            this.StartPosition = FormStartPosition.CenterScreen;

            SetRoundedCorners(15); // Define el radio de redondeo
        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtUsuario.MaxLength = 15;
            txtClave.MaxLength = 20;

            // Asociar eventos de teclado para Enter
            txtUsuario.KeyDown += Txt_KeyDown;
            txtClave.KeyDown += Txt_KeyDown;
        }

        // Este evento detecta si se presiona Enter en cualquiera de los TextBox
        private void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // evita el sonido "beep"
                btnLogin.PerformClick();   // ejecuta el evento del botón Login
            }
        }

        private void SetRoundedCorners(int radius)
        {
            int diameter = radius * 2;
            var bounds = new Rectangle(0, 0, this.Width, this.Height);
            var path = new System.Drawing.Drawing2D.GraphicsPath();

            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.X + bounds.Width - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.X + bounds.Width - diameter, bounds.Y + bounds.Height - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Y + bounds.Height - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            this.Region = new Region(path);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsuario.Text.Trim();
            string password = txtClave.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Id_Usuario, Usuario, EstadoUsuario, NombreUsuario, ApellidoUsuario, Password FROM Usuario WHERE Usuario = @Usuario";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Usuario", username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string estado = reader["EstadoUsuario"].ToString();
                                string storedPassword = reader["Password"].ToString();

                                int idUsuario = Convert.ToInt32(reader["Id_Usuario"]);
                                string nombreUsuario = reader["NombreUsuario"].ToString();
                                string apellidoUsuario = reader["ApellidoUsuario"].ToString();
                                string usuario = reader["Usuario"].ToString();
                                string estadoUsuario = reader["EstadoUsuario"].ToString();

                                if (estado != "Activo")
                                {
                                    MessageBox.Show("El usuario no está activo. Por favor, contacte al administrador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else if (storedPassword != password)
                                {
                                    MessageBox.Show("La contraseña es incorrecta. Por favor, intente de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    txtClave.Clear();
                                    txtClave.Focus();
                                }
                                else
                                {
                                    // 🔒 Ahora sí podemos cerrar el reader
                                    reader.Close();

                                    // Limpiar sesiones huérfanas
                                    string limpiarSesionesQuery = @"
                                    UPDATE Sesiones_Activas
                                    SET Activa = 0, Fecha_Cierre = GETDATE()
                                    WHERE ID_Usuario = @ID_Usuario AND Activa = 1";

                                    using (SqlCommand cmdLimpiar = new SqlCommand(limpiarSesionesQuery, connection))
                                    {
                                        cmdLimpiar.Parameters.AddWithValue("@ID_Usuario", idUsuario);
                                        cmdLimpiar.ExecuteNonQuery();
                                    }

                                    // Verificar si aún hay sesiones activas legítimas
                                    string verificarSesionQuery = "SELECT COUNT(*) FROM Sesiones_Activas WHERE ID_Usuario = @ID_Usuario AND Activa = 1";
                                    using (SqlCommand cmdSesion = new SqlCommand(verificarSesionQuery, connection))
                                    {
                                        cmdSesion.Parameters.AddWithValue("@ID_Usuario", idUsuario);
                                        int sesionesActivas = (int)cmdSesion.ExecuteScalar();

                                        if (sesionesActivas > 0)
                                        {
                                            MessageBox.Show("Este usuario ya tiene una sesión activa. Cierre la sesión anterior antes de iniciar una nueva.",
                                                            "Sesión activa detectada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            return;
                                        }
                                    }

                                    // Registrar nueva sesión
                                    string registrarSesionQuery = "INSERT INTO Sesiones_Activas (ID_Usuario) VALUES (@ID_Usuario)";
                                    using (SqlCommand cmdInsert = new SqlCommand(registrarSesionQuery, connection))
                                    {
                                        cmdInsert.Parameters.AddWithValue("@ID_Usuario", idUsuario);
                                        cmdInsert.ExecuteNonQuery();
                                    }

                                    // Guardar datos en Utilidades
                                    Utilidades.IdUsuario = idUsuario;
                                    Utilidades.Usuario = usuario;
                                    Utilidades.EstadoUsuario = estadoUsuario;
                                    Utilidades.NombreUsuario = nombreUsuario;
                                    Utilidades.ApellidoUsuario = apellidoUsuario;

                                    // Abrir formulario Menu
                                    Menu openMenu = new Menu();
                                    openMenu.Show();

                                    this.Hide();
                                    openMenu.FormClosed += (s, args) => this.Close();
                                }
                            }


                            else
                            {
                                MessageBox.Show("No existe un usuario con ese nombre.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtUsuario.Clear();
                                txtUsuario.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al conectar con la base de datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } 

        private void btnCancel_Click(object sender, EventArgs e)
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
                Close(); // Cerrar el formulario
            }
        }

    }
}