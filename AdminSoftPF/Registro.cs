using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AdminSoftPF
{
    public partial class Registro : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        public int idProyecto = Utilidades.IdProyectoSeleccionado;

        private int idClienteGenerado; // Variable para almacenar el ID del cliente generado después del INSERT

        public Registro()
        {
            InitializeComponent();

        }

        private void Registro_Load(object sender, EventArgs e)
        {
            InformacionDeProyectoSeleccionado();

            LlenarComboEstado();

        }

        public void InformacionDeProyectoSeleccionado()
        {
            // Validar si idProyecto es mayor que 0
            if (idProyecto > 0)
            {
                // Obtener el nombre del proyecto desde la base de datos
                string nombreProyecto = ObtenerNombreProyectoPorID(idProyecto);

                if (!string.IsNullOrEmpty(nombreProyecto))
                {
                    // Asignar el nombre al label
                    lblProyecto.Text = nombreProyecto;

                    // Llamar a los métodos para llenar los ComboBoxes
                    LlenarComboManzana();
                    LlenarComboEdificio();
                    LlenarComboApartamento();
                    LlenarComboCuota();
                }
                else
                {
                    MessageBox.Show("No se encontró el proyecto en la base de datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("El ID del proyecto no está definido o es inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // Método para obtener el nombre del proyecto por ID
        private string ObtenerNombreProyectoPorID(int idProyecto)
        {
            try
            {
                // Suponiendo que tienes una conexión a la base de datos establecida
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT NombreProyecto FROM Proyecto WHERE ID_Proyecto = @ID_Proyecto";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID_Proyecto", idProyecto);

                        // Ejecutar la consulta y obtener el resultado
                        object resultado = command.ExecuteScalar();

                        // Retornar el nombre del proyecto si existe
                        return resultado != null ? resultado.ToString() : string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el nombre del proyecto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }

        private void LlenarComboManzana()
        {
            try
            {
                // Crear la consulta SQL para obtener los valores de la columna Manzana de la tabla Manzana
                string query = "SELECT Manzana FROM Manzana WHERE ID_Proyecto = @ID_Proyecto AND Estado = 'Activo' ";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ID_Proyecto", idProyecto);

                    connection.Open();

                    // Ejecutar la consulta y llenar el ComboBox con los resultados
                    SqlDataReader reader = command.ExecuteReader();
                    List<string> manzanas = new List<string> { "Manzana..." }; // Agregar la opción predeterminada
                    while (reader.Read())
                    {
                        manzanas.Add(reader["Manzana"].ToString());
                    }

                    cboManzana.DataSource = manzanas;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al llenar Manzana: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LlenarComboEdificio()
        {
            try
            {
                // Crear la consulta SQL para obtener los valores de la columna Edificio de la tabla Edificio
                string query = "SELECT Edificio FROM Edificio WHERE ID_Proyecto = @ID_Proyecto AND Estado = 'Activo' ";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ID_Proyecto", idProyecto);

                    connection.Open();

                    // Ejecutar la consulta y llenar el ComboBox con los resultados
                    SqlDataReader reader = command.ExecuteReader();
                    List<string> edificios = new List<string> { "Edificio..." }; // Agregar la opción predeterminada
                    while (reader.Read())
                    {
                        edificios.Add(reader["Edificio"].ToString());
                    }

                    cboEdificio.DataSource = edificios;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al llenar Edificio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LlenarComboApartamento()
        {
            try
            {
                // Crear la consulta SQL para obtener los valores de la columna Apartamento de la tabla Apartamento
                string query = "SELECT Apartamento FROM Apartamento WHERE ID_Proyecto = @ID_Proyecto AND Estado = 'Activo' ";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ID_Proyecto", idProyecto);

                    connection.Open();

                    // Ejecutar la consulta y llenar el ComboBox con los resultados
                    SqlDataReader reader = command.ExecuteReader();
                    List<string> apartamentos = new List<string> { "Apartamento..." }; // Agregar la opción predeterminada

                    while (reader.Read())
                    {
                        apartamentos.Add(reader["Apartamento"].ToString());
                    }

                    cboApartamento.DataSource = apartamentos;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al llenar Apartamento: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Actualizar el Label con las selecciones actuales de los ComboBoxes
            ActualizarLabelSeleccion();
        }

        private void ActualizarLabelSeleccion()
        {
            // Verificar si el SelectedItem no es null antes de acceder a él
            string manzanaSeleccionada = cboManzana.SelectedItem?.ToString() ?? "Manzana no seleccionada";
            string edificioSeleccionado = cboEdificio.SelectedItem?.ToString() ?? "Edificio no seleccionado";
            string apartamentoSeleccionado = cboApartamento.SelectedItem?.ToString() ?? "Apartamento no seleccionado";

            // Actualizar el texto del Label con el formato requerido
            lblDireccionCompleta.Text = $"{manzanaSeleccionada}-{edificioSeleccionado}-{apartamentoSeleccionado}";
        }

        private void LlenarComboCuota()
        {
            try
            {
                // Crear la consulta SQL para obtener los valores de la columna Apartamento de la tabla Apartamento
                string query = "SELECT Cuota FROM Cuota WHERE ID_Proyecto = @ID_Proyecto AND Estado = 'Activo' ";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ID_Proyecto", idProyecto);

                    connection.Open();

                    // Ejecutar la consulta y llenar el ComboBox con los resultados
                    SqlDataReader reader = command.ExecuteReader();
                    List<string> apartamentos = new List<string> { "Cuota..." }; // Agregar la opción predeterminada


                    while (reader.Read())
                    {
                        apartamentos.Add(reader["Cuota"].ToString());
                    }

                    cboCuota.DataSource = apartamentos;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al llenar Cuota: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LlenarComboEstado()
        {
            // Llenar el ComboBox con los valores "Activo" y "No Activo"
            cboEstado.Items.Add("Activo");
            cboEstado.Items.Add("No Activo");

            // Opcional: establecer un valor predeterminado, por ejemplo, "Activo"
            cboEstado.SelectedIndex = 0;
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {

            // Validar los campos antes de continuar con la lógica de creación
            if (ValidarCampos())
            {
                // Validar que las opciones seleccionadas en los ComboBox no sean las predeterminadas
                if (EsSeleccionValida())
                {
                    // Realizar la validación de existencia de la dirección con el proyecto
                    if (DireccionExiste())
                    {
                        MessageBox.Show("Ya existe un registro con esta dirección para el proyecto.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        CrearClienteNuevo();  // Crear el cliente y la dirección

                        // Limpiar los campos después de crear el cliente
                        LimpiarCampos();

                        // Cerrar el formulario actual
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar valores válidos en todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }


        }


        private bool EsSeleccionValida()
        {
            // Verificar que ninguno de los ComboBox tenga la opción predeterminada
            if (cboManzana.SelectedItem?.ToString() == "Manzana..." ||
                cboEdificio.SelectedItem?.ToString() == "Edificio..." ||
                cboApartamento.SelectedItem?.ToString() == "Apartamento..." ||
                cboCuota.SelectedItem?.ToString() == "Cuota...")
            {
                return false; // Si alguno tiene la opción predeterminada, la selección no es válida
            }
            return true; // Si todos los ComboBox tienen valores válidos
        }

        private void CrearClienteNuevo()
        {
            try
            {
                // 1. Insertar el Cliente en la tabla Cliente
                string queryCliente = "INSERT INTO Cliente (NombreCompleto, Documento, Celular1, Celular2, Telefono, Correo, EstadoCliente) " +
                                      "VALUES (@NombreCompleto, @Documento, @Celular1, @Celular2, @Telefono, @Correo, @EstadoCliente); " +
                                      "SELECT SCOPE_IDENTITY();"; // Obtiene el ID del cliente recién insertado

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryCliente, connection);
                    command.Parameters.Add("@NombreCompleto", SqlDbType.VarChar, 30).Value = txtNombre.Text;
                    command.Parameters.Add("@Documento", SqlDbType.VarChar, 11).Value = txtDocumento.Text;
                    command.Parameters.Add("@Celular1", SqlDbType.VarChar, 10).Value = txtCelular1.Text;
                    command.Parameters.Add("@Celular2", SqlDbType.VarChar, 10).Value = txtCelular2.Text ?? (object)DBNull.Value;
                    command.Parameters.Add("@Telefono", SqlDbType.VarChar, 10).Value = txtTelefono.Text;
                    command.Parameters.Add("@Correo", SqlDbType.VarChar, 50).Value = txtCorreo.Text;
                    command.Parameters.Add("@EstadoCliente", SqlDbType.VarChar, 10).Value = cboEstado.SelectedItem.ToString();

                    connection.Open();
                    // Obtener el ID del cliente insertado
                    idClienteGenerado = Convert.ToInt32(command.ExecuteScalar());
                }

                // 2. Insertar la Dirección en la tabla Direccion, usando el ID_Cliente recién insertado
                string queryDireccion = "INSERT INTO Direccion (ID_Cliente, Manzana, Edificio, Apartamento, Cuota, ProyectoNombre, idProyecto) " +
                                        "VALUES (@ID_Cliente, @Manzana, @Edificio, @Apartamento, @Cuota, @ProyectoNombre, @idProyecto);";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryDireccion, connection);
                    command.Parameters.Add("@ID_Cliente", SqlDbType.Int).Value = idClienteGenerado;
                    command.Parameters.Add("@Manzana", SqlDbType.VarChar, 50).Value = cboManzana.SelectedItem?.ToString() ?? (object)DBNull.Value;
                    command.Parameters.Add("@Edificio", SqlDbType.VarChar, 50).Value = cboEdificio.SelectedItem?.ToString() ?? (object)DBNull.Value;
                    command.Parameters.Add("@Apartamento", SqlDbType.VarChar, 50).Value = cboApartamento.SelectedItem?.ToString() ?? (object)DBNull.Value;
                    command.Parameters.Add("@Cuota", SqlDbType.VarChar, 50).Value = cboCuota.SelectedItem?.ToString() ?? (object)DBNull.Value;
                    command.Parameters.Add("@ProyectoNombre", SqlDbType.VarChar, 100).Value = lblProyecto.Text;
                    command.Parameters.Add("@idProyecto", SqlDbType.Int).Value = idProyecto;

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Cliente creado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarCampos()
        {
            // Verificar si el nombre solo tiene letras y espacios, y no excede los 30 caracteres
            if (!Regex.IsMatch(txtNombre.Text, @"^[a-zA-ZáéíóúÁÉÍÓÚ\s]{1,30}$"))
            {
                MessageBox.Show("El nombre solo puede contener letras (mayúsculas o minúsculas) y espacios, y no debe exceder los 30 caracteres.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Verificar si el documento tiene entre 9 y 11 caracteres sin espacios
            if (!Regex.IsMatch(txtDocumento.Text, @"^\d{9,11}$"))
            {
                MessageBox.Show("El documento debe tener entre 9 y 11 caracteres sin espacios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Eliminar todos los espacios del celular 1 y verificar que tenga entre 10 y 15 dígitos
            string celular1 = txtCelular1.Text.Replace(" ", "");
            if (!Regex.IsMatch(celular1, @"^\d{10,15}$"))
            {
                MessageBox.Show("El celular 1 debe tener entre 10 y 15 dígitos. Se permiten espacios, pero no se considerarán en la validación.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Eliminar todos los espacios del celular 2 y verificar que tenga entre 10 y 15 dígitos
            string celular2 = txtCelular2.Text.Replace(" ", "");
            if (!Regex.IsMatch(celular2, @"^\d{10,15}$"))
            {
                MessageBox.Show("El celular 2 debe tener entre 10 y 15 dígitos. Se permiten espacios, pero no se considerarán en la validación.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Eliminar todos los espacios del teléfono y verificar que tenga entre 10 y 15 dígitos
            string telefono = txtTelefono.Text.Replace(" ", "");
            if (!Regex.IsMatch(telefono, @"^\d{10,15}$"))
            {
                MessageBox.Show("El teléfono debe tener entre 10 y 15 dígitos. Se permiten espacios, pero no se considerarán en la validación.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Verificar si el correo tiene un formato válido y no excede los 50 caracteres
            if (!Regex.IsMatch(txtCorreo.Text, @"^[\w\.-]+@[\w\.-]+\.[a-zA-Z]{2,}$") || txtCorreo.Text.Length > 50)
            {
                MessageBox.Show("El correo debe contener un '@' y no puede exceder los 50 caracteres.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Verificar que todos los campos necesarios estén llenos
            if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtDocumento.Text) ||
                string.IsNullOrEmpty(txtCelular1.Text) || string.IsNullOrEmpty(txtTelefono.Text) ||
                string.IsNullOrEmpty(txtCorreo.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios. Por favor, complete todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true; // Todos los campos son válidos
        }


        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            // Limpiar los TextBoxes
            txtNombre.Clear();
            txtDocumento.Clear();
            txtCelular1.Clear();
            txtCelular2.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();

            // Restablecer los ComboBox a sus valores predeterminados
            cboEstado.SelectedIndex = 0; // Asumiendo que "Activo" es la opción por defecto

            cboManzana.SelectedIndex = 0; // "Manzana..." es la opción predeterminada
            cboEdificio.SelectedIndex = 0; // "Edificio..." es la opción predeterminada
            cboApartamento.SelectedIndex = 0; // "Apartamento..." es la opción predeterminada

            cboCuota.SelectedIndex = 0; // "Cuota..." es la opción predeterminada

        }

        private bool DireccionExiste()
        {
            try
            {
                // Obtener los valores de los ComboBox
                string manzana = cboManzana.SelectedItem?.ToString() ?? string.Empty;
                string edificio = cboEdificio.SelectedItem?.ToString() ?? string.Empty;
                string apartamento = cboApartamento.SelectedItem?.ToString() ?? string.Empty;

                // Formatear la dirección completa correctamente
                string direccionCompleta = $"{manzana.Trim()} - {edificio.Trim()} - {apartamento.Trim()}".Trim();

                // Nombre del proyecto desde el label
                string nombreProyecto = lblProyecto.Text.Trim();

                // Asegúrate de que los valores no estén vacíos antes de proceder
                if (string.IsNullOrEmpty(direccionCompleta) || string.IsNullOrEmpty(nombreProyecto))
                {
                    MessageBox.Show("No se ha completado la dirección o el nombre del proyecto.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Crear la consulta SQL para verificar si ya existe un registro con la misma dirección y proyecto
                string query = "SELECT COUNT(*) FROM Direccion WHERE DireccionCompleta = @DireccionCompleta AND ProyectoNombre = @ProyectoNombre";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add("@DireccionCompleta", SqlDbType.VarChar).Value = direccionCompleta;
                    command.Parameters.Add("@ProyectoNombre", SqlDbType.VarChar).Value = nombreProyecto;

                    connection.Open();

                    // Ejecutar la consulta y obtener el número de registros encontrados
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    // Si ya existe un registro con la misma dirección y proyecto, devolver true
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar la existencia de la dirección: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;  // En caso de error, consideramos que la dirección no existe
            }
        }



    }
}
