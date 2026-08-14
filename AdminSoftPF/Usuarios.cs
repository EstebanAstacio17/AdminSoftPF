using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AdminSoftPF
{
    public partial class Usuarios : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        public Usuarios()
        {
            InitializeComponent();
        }

        private void Usuarios_Load(object sender, EventArgs e)
        {
            CargarDatosUsuarios();
        }

        public void CargarDatosUsuarios()
        {
            try
            {
                // Crear la conexión
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Crear el comando para obtener los datos
                    string query = "SELECT ID_Usuario, NombreUsuario, ApellidoUsuario, Documento, Correo, Celular, Usuario, EstadoUsuario FROM usuario"; // Ajusta según el esquema de tu tabla
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Llenar un DataTable con los datos obtenidos
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // Asignar los datos al DataGridView
                            dgvUsuarios.DataSource = dataTable; // Asegúrate de que este control exista en tu formulario

                            ConfiguracionDGVUsuarios();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfiguracionDGVUsuarios()
        {
            // Set specific widths for each column
            dgvUsuarios.Columns["ID_Usuario"].Width = 45;
            dgvUsuarios.Columns["NombreUsuario"].Width = 155;
            dgvUsuarios.Columns["ApellidoUsuario"].Width = 155;  
            dgvUsuarios.Columns["Documento"].Width = 95;
            dgvUsuarios.Columns["Correo"].Width = 220;
            dgvUsuarios.Columns["Celular"].Width = 95;
            dgvUsuarios.Columns["Usuario"].Width = 95;
            dgvUsuarios.Columns["EstadoUsuario"].Width = 85;
        }

        private void btnAgregarUsuario_Click(object sender, EventArgs e)
        {
            // AGREGAR LA OPCION DE QUE EL MISMO USUARIO NO PUEDA OTORGARSE MAS PODERES O CAMBIOS EN SU PERFIL SI NO OTRO USUARIO

            NewUser addNewUser = new NewUser();
            addNewUser.ShowDialog();
        }

        private void dgvUsuarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Obtener el valor de la columna "ID_Usuario" de la fila seleccionada
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow selectedRow = dgvUsuarios.Rows[e.RowIndex];
                    int idUsuario = Convert.ToInt32(selectedRow.Cells["ID_Usuario"].Value);

                    // Almacenar el valor en una clase estática
                    Utilidades.IdUsuarioSeleccionado = idUsuario;

                    // Abrir el formulario PerfilUsuario
                    PerfilUsuario openPerfilUsuario = new PerfilUsuario();
                    openPerfilUsuario.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al seleccionar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        

        }


    }
}
