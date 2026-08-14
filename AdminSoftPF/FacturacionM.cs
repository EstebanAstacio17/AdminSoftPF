using System;
using System.Collections;
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
    public partial class FacturacionM : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        public FacturacionM()
        {
            InitializeComponent();
        }

        private void FacturacionM_Load(object sender, EventArgs e)
        {
            ConfigCboCuotasProyectoActual();
        }

        public void CargarCuotas(int IdProyectoSeleccionado)
        {
            // Crear la consulta SQL para obtener las cuotas correspondientes al idProyecto
            string query = "SELECT Cuota FROM Cuota WHERE ID_Proyecto = @IdProyecto AND Estado = 'Activo' ";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@IdProyecto", IdProyectoSeleccionado);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    // Limpiar el ComboBox antes de llenarlo
                    cboCuotas.Items.Clear();
                    cboCuotaFacturar.Items.Clear();

                    while (reader.Read())
                    {
                        // Agregar cada cuota al ComboBox
                        cboCuotas.Items.Add(reader["Cuota"].ToString());
                        cboCuotaFacturar.Items.Add(reader["Cuota"].ToString());
                    }

                    reader.Close();
                }

                // Verificar si el ComboBox tiene elementos y seleccionar el primero si es necesario
                if (cboCuotas.Items.Count > 0)
                {
                    // Establecer la primera opción como seleccionada por defecto
                    cboCuotas.SelectedIndex = -1;
                    cboCuotaFacturar.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las cuotas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigCboCuotasProyectoActual()
        {
            // Hacer que el ComboBox sea de solo lectura
            cboCuotas.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCuotaFacturar.DropDownStyle = ComboBoxStyle.DropDownList;

            CargarCuotas(Utilidades.IdProyectoSeleccionado);

        }

        private void FormatoTextFact_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txtFacturacion = sender as TextBox;

            // Check if the input is a digit, a point, or control characters (like backspace)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Block invalid character
                MessageBox.Show("Solo se permiten números y el carácter '.'", "Entrada no válida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Prevent spaces
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("No se permiten espacios en este campo.", "Entrada no válida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Allow only one point (.)
            if (e.KeyChar == '.' && txtFacturacion.Text.Contains('.'))
            {
                e.Handled = true;
                MessageBox.Show("Solo se permite un punto decimal.", "Entrada no válida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Limit to 8 characters
            if (!char.IsControl(e.KeyChar) && txtFacturacion.Text.Length >= 8)
            {
                e.Handled = true;
                MessageBox.Show("Solo se permiten un máximo de 8 caracteres.", "Entrada no válida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FormatoTextDetalle_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Limit to 20 characters
            if (!char.IsControl(e.KeyChar) && txtDetalleFacturacionMasiva.Text.Length >= 20)
            {
                e.Handled = true;
                MessageBox.Show("Solo se permiten un máximo de 20 caracteres.", "Entrada no válida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSeleccionarCuota_Click(object sender, EventArgs e)
        {
            // Validar que el campo txtCuotaFacturar no esté vacío
            if (string.IsNullOrWhiteSpace(cboCuotaFacturar.Text))
            {
                MessageBox.Show("Por favor, ingrese un valor en el campo 'Cuota a Facturar'.", "Campo Vacío", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener el valor de la cuota a buscar
            string cuotaFacturar = cboCuotaFacturar.Text.Trim();

            // Asegurarse de que el nombre del proyecto no esté vacío
            string nombreProyecto = Utilidades.NombreProyectoSeleccionado;

            if (string.IsNullOrWhiteSpace(nombreProyecto))
            {
                MessageBox.Show("No se ha seleccionado un proyecto válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Modificación de la consulta SQL
            string query = @"
                            SELECT Direccion.DireccionCompleta, Direccion.Cuota, Direccion.ProyectoNombre
                            FROM Direccion
                            INNER JOIN Cliente ON Direccion.ID_Cliente = Cliente.ID_Cliente
                            WHERE Direccion.Cuota = @Cuota 
                            AND Direccion.ProyectoNombre = @ProyectoNombre
                            AND Cliente.EstadoCliente = 'Activo'"; 

            try
            {
                // Crear una tabla para almacenar los datos
                DataTable clientesAfacturar = new DataTable();

                // Crear conexión y comando
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agregar parámetros para evitar inyección SQL
                    command.Parameters.AddWithValue("@Cuota", cuotaFacturar);
                    command.Parameters.AddWithValue("@ProyectoNombre", nombreProyecto); // Asegurarse de agregar este parámetro

                    // Abrir la conexión
                    connection.Open();

                    // Ejecutar la consulta y cargar los datos en un adaptador
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        // Llenar el DataTable con los datos obtenidos
                        adapter.Fill(clientesAfacturar);
                    }
                }

                // Asignar los datos al DataGridView
                dgvFacturacionMasiva.DataSource = clientesAfacturar;

                // Verificar si se encontraron registros
                if (clientesAfacturar.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron registros con los criterios especificados.", "Sin Resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Mostrar un mensaje con la cantidad de registros encontrados
                    MessageBox.Show($"Se seleccionaron {clientesAfacturar.Rows.Count} Clientes.", "Clientes Seleccionados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Manejar errores de conexión o ejecución
                MessageBox.Show($"Error al realizar la consulta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuvaFacturacion_Click(object sender, EventArgs e)
        {
            LimpiarFacturacionMasiva();
        }

        public void LimpiarFacturacionMasiva()
        {
            try
            {
                // Limpiar los TextBox
                cboCuotaFacturar.Text = string.Empty;
                txtDetalleFacturacionMasiva.Text = string.Empty;

                // Limpiar el DataGridView
                dgvFacturacionMasiva.DataSource = null; // Elimina el enlace de datos
                dgvFacturacionMasiva.Rows.Clear(); // Limpia todas las filas
                dgvFacturacionMasiva.Refresh(); // Refresca el DataGridView

            }
            catch (Exception ex)
            {
                // Manejar errores si los hubiera
                MessageBox.Show($"Ocurrió un error al intentar limpiar los campos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFacturarCuota_Click(object sender, EventArgs e)
        {
            // Mostrar un cuadro de diálogo para confirmar la acción
            DialogResult resultado = MessageBox.Show(
                "¿Está seguro de que desea generar la Facturación?",
                "Confirmar Facturación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            // Verificar la respuesta del usuario
            if (resultado == DialogResult.Yes)
            {
                // Preguntar si desea generar con recargo
                DialogResult recargo = MessageBox.Show(
                    "¿Desea generar la Facturación con Recargo?",
                    "Opciones de Facturación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (recargo == DialogResult.Yes)
                {
                    // Ejecutar el método con recargo
                    GenerarFacturaMasivaConMoras();
                }
                else
                {
                    // Ejecutar el método sin recargo
                    GenerarFacturaMasiva();
                }
            }
            else
            {
                // No hacer nada si el usuario selecciona "No"
                MessageBox.Show(
                    "La Facturación ha sido cancelada.",
                    "Operación Cancelada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void GenerarFacturaMasiva()
        {

            if (dgvFacturacionMasiva.Rows.Count == 0)
            {
                MessageBox.Show("No hay registros para Facturar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDetalleFacturacionMasiva.Text))
            {
                MessageBox.Show("El campo de Detalle no puede estar vacío.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int registrosNuevos = 0;

            // Obtener los valores de la clase Utilidades
            int idUsuario = Utilidades.IdUsuario; // Asumimos que esta clase está accesible
            string usuario = Utilidades.Usuario;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction(); // Iniciar transacción

                    foreach (DataGridViewRow row in dgvFacturacionMasiva.Rows)
                    {
                        if (row.IsNewRow) continue;

                        // Validar que las celdas no sean nulas o vacías
                        if (row.Cells["DireccionCompleta"].Value == null || string.IsNullOrWhiteSpace(row.Cells["DireccionCompleta"].Value.ToString()))
                        {
                            MessageBox.Show("Se encontró un registro con la dirección vacía o nula. Este registro será omitido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }

                        string direccion = row.Cells["DireccionCompleta"].Value.ToString();

                        // Obtener ID_Direccion y otros datos de la tabla Direccion
                        string queryDireccion = "SELECT ID_Direccion, Cuota, Deuda FROM Direccion WHERE DireccionCompleta = @Direccion";
                        int idDireccion;
                        decimal cuota;
                        decimal deudaActual;

                        using (SqlCommand cmdDireccion = new SqlCommand(queryDireccion, connection, transaction))
                        {
                            cmdDireccion.Parameters.AddWithValue("@Direccion", direccion);

                            using (SqlDataReader reader = cmdDireccion.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    MessageBox.Show($"No se encontró la dirección: {direccion} en la tabla Direccion. Este registro será omitido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    continue;
                                }

                                // Validar los valores obtenidos para evitar valores nulos
                                idDireccion = reader["ID_Direccion"] != DBNull.Value ? Convert.ToInt32(reader["ID_Direccion"]) : 0;
                                cuota = reader["Cuota"] != DBNull.Value ? Convert.ToDecimal(reader["Cuota"]) : 0;
                                deudaActual = reader["Deuda"] != DBNull.Value ? Convert.ToDecimal(reader["Deuda"]) : 0;
                            }
                        }

                        // Insertar en la tabla Factura
                        string insertFactura = @"INSERT INTO Factura (ID_Direccion, Direccion, TipoCuota, ValorCuota, DetalleFactura)
                                 VALUES (@ID_Direccion, @Direccion, @TipoCuota, @ValorCuota, @DetalleFactura);
                                 SELECT SCOPE_IDENTITY();";

                        int idFactura;
                        using (SqlCommand cmdFactura = new SqlCommand(insertFactura, connection, transaction))
                        {
                            cmdFactura.Parameters.AddWithValue("@ID_Direccion", idDireccion);
                            cmdFactura.Parameters.AddWithValue("@Direccion", direccion);
                            cmdFactura.Parameters.AddWithValue("@TipoCuota", "Cuota Regular");
                            cmdFactura.Parameters.AddWithValue("@ValorCuota", cuota);
                            cmdFactura.Parameters.AddWithValue("@DetalleFactura", txtDetalleFacturacionMasiva.Text);

                            idFactura = Convert.ToInt32(cmdFactura.ExecuteScalar());
                        }

                        // Insertar en la tabla Historial, incluyendo el ID_Usuario y el Usuario
                        string insertHistorial = @"INSERT INTO Historial (ID_Factura, Direccion, Tipo, Deuda, ID_Usuario, Usuario)
                                   VALUES (@ID_Factura, @Direccion, @Tipo, @Deuda, @ID_Usuario, @Usuario);";

                        // Aquí, la deuda será igual al valor de la cuota, en lugar de ser la deuda actual sumada con la cuota
                        decimal deudaEnHistorial = cuota;

                        using (SqlCommand cmdHistorial = new SqlCommand(insertHistorial, connection, transaction))
                        {
                            cmdHistorial.Parameters.AddWithValue("@ID_Factura", idFactura);
                            cmdHistorial.Parameters.AddWithValue("@Direccion", direccion);
                            cmdHistorial.Parameters.AddWithValue("@Tipo", "Cuota Regular");
                            cmdHistorial.Parameters.AddWithValue("@Deuda", deudaEnHistorial); // Aquí asignamos la cuota como deuda
                            cmdHistorial.Parameters.AddWithValue("@ID_Usuario", idUsuario);
                            cmdHistorial.Parameters.AddWithValue("@Usuario", usuario);

                            cmdHistorial.ExecuteNonQuery();
                        }

                        // Actualizar la deuda en la tabla Direccion (opcional, según requerimiento de negocio)
                        string updateDireccion = "UPDATE Direccion SET Deuda = @Deuda WHERE ID_Direccion = @ID_Direccion";

                        using (SqlCommand cmdUpdateDireccion = new SqlCommand(updateDireccion, connection, transaction))
                        {
                            cmdUpdateDireccion.Parameters.AddWithValue("@Deuda", deudaActual + cuota); // Aquí sigue actualizando la deuda acumulada
                            cmdUpdateDireccion.Parameters.AddWithValue("@ID_Direccion", idDireccion);

                            cmdUpdateDireccion.ExecuteNonQuery();
                        }

                        registrosNuevos++;
                    }

                    // Confirmar la transacción si todo ha ido bien
                    transaction.Commit();

                    MessageBox.Show($"Se han registrado {registrosNuevos} nuevas facturas correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFacturacionMasiva();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al facturar cuotas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerarFacturaMasivaConMoras()
        {
            // Verifica si hay registros en la tabla de facturación masiva
            if (dgvFacturacionMasiva.Rows.Count == 0)
            {
                MessageBox.Show("No hay registros para facturar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verifica si el campo de detalle de facturación está vacío
            if (string.IsNullOrWhiteSpace(txtDetalleFacturacionMasiva.Text))
            {
                MessageBox.Show("El campo de detalle no puede estar vacío.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int registrosNuevos = 0;
            int idUsuario = Utilidades.IdUsuario;
            string usuario = Utilidades.Usuario;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    foreach (DataGridViewRow row in dgvFacturacionMasiva.Rows)
                    {
                        if (row.IsNewRow) continue;

                        // Verifica si la dirección está vacía o nula
                        if (row.Cells["DireccionCompleta"].Value == null || string.IsNullOrWhiteSpace(row.Cells["DireccionCompleta"].Value.ToString()))
                        {
                            MessageBox.Show("Se encontró un registro con la dirección vacía o nula. Este registro será omitido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }

                        string direccion = row.Cells["DireccionCompleta"].Value.ToString();

                        // Consulta la dirección en la base de datos para obtener ID, cuota y deuda
                        string queryDireccion = "SELECT ID_Direccion, Cuota, Deuda FROM Direccion WHERE DireccionCompleta = @Direccion";
                        int idDireccion;
                        decimal cuota;
                        decimal deudaActual;

                        using (SqlCommand cmdDireccion = new SqlCommand(queryDireccion, connection, transaction))
                        {
                            cmdDireccion.Parameters.AddWithValue("@Direccion", direccion);

                            using (SqlDataReader reader = cmdDireccion.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    MessageBox.Show($"No se encontró la dirección: {direccion} en la tabla Direccion. Este registro será omitido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    continue;
                                }

                                idDireccion = reader["ID_Direccion"] != DBNull.Value ? Convert.ToInt32(reader["ID_Direccion"]) : 0;
                                cuota = reader["Cuota"] != DBNull.Value ? Convert.ToDecimal(reader["Cuota"]) : 0;
                                deudaActual = reader["Deuda"] != DBNull.Value ? Convert.ToDecimal(reader["Deuda"]) : 0;
                            }
                        }

                        // Si la dirección tiene deuda, se aplica un recargo del 5%
                        decimal cuotaFinal = (deudaActual > 0) ? cuota + (cuota * 0.05m) : cuota;

                        // Inserta la nueva factura en la base de datos
                        string insertFactura = @"INSERT INTO Factura (ID_Direccion, Direccion, TipoCuota, ValorCuota, DetalleFactura)
                                         VALUES (@ID_Direccion, @Direccion, @TipoCuota, @ValorCuota, @DetalleFactura);
                                         SELECT SCOPE_IDENTITY();";

                        int idFactura;
                        using (SqlCommand cmdFactura = new SqlCommand(insertFactura, connection, transaction))
                        {
                            cmdFactura.Parameters.AddWithValue("@ID_Direccion", idDireccion);
                            cmdFactura.Parameters.AddWithValue("@Direccion", direccion);
                            cmdFactura.Parameters.AddWithValue("@TipoCuota", "Cuota Regular");
                            cmdFactura.Parameters.AddWithValue("@ValorCuota", cuotaFinal);
                            cmdFactura.Parameters.AddWithValue("@DetalleFactura", txtDetalleFacturacionMasiva.Text);

                            idFactura = Convert.ToInt32(cmdFactura.ExecuteScalar());
                        }

                        // Inserta un registro en el historial de facturación
                        string insertHistorial = @"INSERT INTO Historial (ID_Factura, Direccion, Tipo, Deuda, ID_Usuario, Usuario)
                                           VALUES (@ID_Factura, @Direccion, @Tipo, @Deuda, @ID_Usuario, @Usuario);";

                        using (SqlCommand cmdHistorial = new SqlCommand(insertHistorial, connection, transaction))
                        {
                            cmdHistorial.Parameters.AddWithValue("@ID_Factura", idFactura);
                            cmdHistorial.Parameters.AddWithValue("@Direccion", direccion);
                            cmdHistorial.Parameters.AddWithValue("@Tipo", "Cuota Regular");
                            cmdHistorial.Parameters.AddWithValue("@Deuda", cuotaFinal);
                            cmdHistorial.Parameters.AddWithValue("@ID_Usuario", idUsuario);
                            cmdHistorial.Parameters.AddWithValue("@Usuario", usuario);

                            cmdHistorial.ExecuteNonQuery();
                        }

                        // Actualiza la deuda en la tabla Dirección
                        string updateDireccion = "UPDATE Direccion SET Deuda = @Deuda WHERE ID_Direccion = @ID_Direccion";

                        using (SqlCommand cmdUpdateDireccion = new SqlCommand(updateDireccion, connection, transaction))
                        {
                            cmdUpdateDireccion.Parameters.AddWithValue("@Deuda", deudaActual + cuotaFinal);
                            cmdUpdateDireccion.Parameters.AddWithValue("@ID_Direccion", idDireccion);

                            cmdUpdateDireccion.ExecuteNonQuery();
                        }

                        registrosNuevos++;
                    }

                    transaction.Commit();

                    MessageBox.Show($"Se han registrado {registrosNuevos} nuevas facturas correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFacturacionMasiva();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al facturar cuotas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSeleccionarClientes_Click(object sender, EventArgs e)
        {
            // Validar que el campo txtCuotaActual no esté vacío
            if (string.IsNullOrWhiteSpace(txtCuotaActual.Text))
            {
                MessageBox.Show("Por favor, indique el valor de 'Cuota a Cambiar'.", "Campo Vacío", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener el valor de la cuota a Cambiar
            string cuotaActual = txtCuotaActual.Text.Trim();

            string nombreProyecto = Utilidades.NombreProyectoSeleccionado;

            // Validar que el nombre del proyecto no esté vacío
            if (string.IsNullOrWhiteSpace(nombreProyecto))
            {
                MessageBox.Show("El nombre del proyecto no está especificado.", "Campo Vacío", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Modificación de la consulta SQL
            string query = @"
                            SELECT Direccion.DireccionCompleta, Direccion.Cuota, Direccion.ProyectoNombre
                            FROM Direccion
                            INNER JOIN Cliente ON Direccion.ID_Cliente = Cliente.ID_Cliente
                            WHERE Direccion.Cuota = @Cuota 
                            AND Direccion.ProyectoNombre = @ProyectoNombre
                            AND Cliente.EstadoCliente = 'Activo'"; 

            try
            {
                // Crear una tabla para almacenar los datos
                DataTable clientesAcambiarCuota = new DataTable();

                // Crear conexión y comando
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agregar parámetros para evitar inyección SQL
                    command.Parameters.AddWithValue("@Cuota", cuotaActual);
                    command.Parameters.AddWithValue("@ProyectoNombre", nombreProyecto);

                    // Abrir la conexión
                    connection.Open();

                    // Ejecutar la consulta y cargar los datos en un adaptador
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        // Llenar el DataTable con los datos obtenidos
                        adapter.Fill(clientesAcambiarCuota);
                    }
                }

                // Asignar los datos al DataGridView
                dgvCambioCuotaMasiva.DataSource = clientesAcambiarCuota;

                // Verificar si se encontraron registros
                if (clientesAcambiarCuota.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron Clientes con los criterios especificados.", "Sin Resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Mostrar un mensaje con la cantidad de registros encontrados
                    MessageBox.Show($"Se seleccionaron {clientesAcambiarCuota.Rows.Count} Clientes.", "Clientes Seleccionados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                // Manejar errores de conexión o ejecución
                MessageBox.Show($"Error al realizar la consulta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevoCambioCuotas_Click(object sender, EventArgs e)
        {
            LimpiarCambioDeCuota();
        }

        public void LimpiarCambioDeCuota()
        {
            try
            {
                // Limpiar los TextBox
                txtCuotaActual.Text = string.Empty;
                cboCuotas.Text = string.Empty;

                // Limpiar el DataGridView
                dgvCambioCuotaMasiva.DataSource = null; // Elimina el enlace de datos
                dgvCambioCuotaMasiva.Rows.Clear(); // Limpia todas las filas
                dgvCambioCuotaMasiva.Refresh(); // Refresca el DataGridView

            }
            catch (Exception ex)
            {
                // Manejar errores si los hubiera
                MessageBox.Show($"Ocurrió un error al intentar limpiar los campos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAplicarNuevaCuota_Click(object sender, EventArgs e)
        {
            // Validar que el campo txtNuevaCuota no esté vacío
            if (string.IsNullOrWhiteSpace(cboCuotas.Text))
            {
                MessageBox.Show("Por favor, indique el nuevo valor de 'Cuota'.", "Campo Vacío", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal nuevaCuota;
            if (!decimal.TryParse(cboCuotas.Text.Trim(), out nuevaCuota))
            {
                MessageBox.Show("El valor de la nueva cuota debe ser un número válido.", "Entrada no válida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvCambioCuotaMasiva.Rows.Count == 0)
            {
                MessageBox.Show("No hay Clientes con estos criterios para aplicar cambios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int registrosActualizados = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (DataGridViewRow row in dgvCambioCuotaMasiva.Rows)
                    {
                        if (row.IsNewRow) continue;

                        // Validar que las celdas no sean nulas o vacías
                        if (row.Cells["DireccionCompleta"].Value == null || string.IsNullOrWhiteSpace(row.Cells["DireccionCompleta"].Value.ToString()))
                        {
                            MessageBox.Show("Se encontró un registro con la dirección vacía o nula. Este registro será omitido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }

                        string direccion = row.Cells["DireccionCompleta"].Value.ToString();

                        // Actualizar la cuota en la tabla Dirección
                        string updateQuery = "UPDATE Direccion SET Cuota = @NuevaCuota WHERE DireccionCompleta = @Direccion";

                        using (SqlCommand cmdUpdate = new SqlCommand(updateQuery, connection))
                        {
                            cmdUpdate.Parameters.AddWithValue("@NuevaCuota", nuevaCuota);
                            cmdUpdate.Parameters.AddWithValue("@Direccion", direccion);

                            int result = cmdUpdate.ExecuteNonQuery();
                            if (result > 0)
                            {
                                registrosActualizados++;
                            }
                        }
                    }
                }

                MessageBox.Show($"Se han actualizado {registrosActualizados} Clientes correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                LimpiarCambioDeCuota(); // Limpiar después de actualizar
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al aplicar la nueva cuota: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}