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
using System.Drawing.Printing;
using System.IO;

namespace AdminSoftPF
{
    public partial class Detalle : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        // Propiedad pública para almacenar el ID_Cliente
        public int ID_Cliente { get; set; }

        // Variable para almacenar el nombre completo en el formulario
        public string NombreCompletoCliente { get; private set; }

        // Variable pública para almacenar el ID_Proyecto
        public int ID_Proyecto { get; private set; }

        // Config Mostrar Recibo
        private PrintPreviewDialog printPreviewDialogRecibo;

        private PrintPreviewDialog printPreviewDialogHistorial;

        private PrintDocument printDocument;

        // Variable para almacenar el ID_Recibo de la fila seleccionada
        private string selectedIDRecibo = "";
        private string fechaPago = "";
        private string usuario = "";
        private string direccion = "";
        private string monto = "";
        private string formaPago = "";
        private string detalleBanco = "";
        private string detalle = "";

        // Variables para FACTURA
        private PrintPreviewDialog printPreviewDialogFactura;

        // Variable para almacenar el ID_Factura de la fila seleccionada
        private string selectedIDFactura = "";

        private string iD_DireccionFactura = "";
        private string direccionFactura = "";
        private string valorFactura = "";
        private string detalleFactura = "";
        private string fechaFactura = "";

        private string deudaClienteFactura = "";

        public string documentoClienteFactura { get; private set; }

        public Detalle()
        {
            InitializeComponent();

            // Configurar opciones iniciales
            ConfigurarEstadoComboBox();

            // Bloquear los campos al inicio
            BloquearCampos();

            // Registrar eventos de cambio en los ComboBoxes
            RegistrarEventosComboBoxes();

            // Inicializar los botones
            InicializarBotones();

            // Config Mostrar Recibo
            //Inicializar PrintDocument
            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintRecibo_PrintPage;

            // Inicializar PrintPreviewDialog
            printPreviewDialogRecibo = new PrintPreviewDialog
            {
                Document = printDocument,
                Width = 800,
                Height = 600
            };

            // Config Mostrar Historial
            // Inicializar PrintDocument
            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintHistorial_PrintPage;

            // Inicializar PrintPreviewDialog
            printPreviewDialogHistorial = new PrintPreviewDialog
            {
                Document = printDocument,
                Width = 800,
                Height = 600
            };

            // Para FACTURA
            // Inicializar PrintDocument
            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintFactura_PrintPage;

            // Inicializar PrintPreviewDialog
            printPreviewDialogFactura = new PrintPreviewDialog
            {
                Document = printDocument,
                Width = 800,
                Height = 600
            };
        }

        private void Detalle_Load(object sender, EventArgs e)
        {
            // PROVISIONAL MENTEEEE
            CargarFacturas();
            CargarRecibos();
            CargarObservaciones();

            // Limitar el reordenamiento y ordenamiento de columnas una vez que los datos hayan sido cargados
            LimitarDgvsDetalle();

            Utilidades.CargarDatosProyecto();

            ObtnerNombreClienteDetalle();

            // PARA FACTURA
            ObtenerDireccionFactura();

            ObtenerClienteFactura();

            PermisoAnularRecibo();
        }

        private void PermisoAnularRecibo()
        {
            btnAnularRecibo.Enabled = Utilidades.PermisoUsuario == "S Administrador";
        }

        public void ObtenerIDDireccionPorCliente()
        {
            if (ID_Cliente <= 0)
            {
                MessageBox.Show("ID de cliente no válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                            SELECT ID_Direccion
                            FROM Direccion
                            WHERE ID_Cliente = @ID_Cliente";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID_Cliente", ID_Cliente);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        // Asignamos el ID_Direccion a la variable pública en Utilidades
                        Utilidades.ID_Direccion = Convert.ToInt32(result);
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la dirección para este cliente.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el ID de dirección: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ObtnerNombreClienteDetalle()
        {
            // Verificar si el ID del cliente es válido
            if (ID_Cliente <= 0)
            {
                MessageBox.Show("ID de cliente no válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Abrir la conexión a la base de datos
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Consulta SQL para obtener el nombre completo formateado
                    string query = @"
                                    SELECT (
                                        SELECT 
                                            STRING_AGG(
                                                UPPER(LEFT(value, 1)) + LOWER(SUBSTRING(value, 2, LEN(value))),
                                                ' '
                                            ) 
                                        FROM (
                                            SELECT TOP 3 
                                                value
                                            FROM STRING_SPLIT(C.NombreCompleto, ' ')
                                        ) AS SplitWords
                                    ) AS NombreFormateado
                                    FROM Cliente C
                                    WHERE ID_Cliente = @ID_Cliente";

                    // Crear el comando SQL y agregar el parámetro
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID_Cliente", ID_Cliente);

                    // Ejecutar la consulta y obtener el resultado
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        // Almacenar el resultado en la variable del formulario
                        NombreCompletoCliente = result.ToString();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el cliente con el ID proporcionado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar errores en la conexión o consulta
                MessageBox.Show($"Error al obtener el nombre del cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimitarDgvsDetalle()
        {
            // Deshabilitar el reordenamiento de columnas y el ordenamiento para todos los DataGridViews
            LimitarOrdenamiento(dgvFactura);
            LimitarOrdenamiento(dgvRecibo);
            LimitarOrdenamiento(dgvObservaciones);
        }

        private void LimitarOrdenamiento(DataGridView dgv)
        {
            // Deshabilitar el reordenamiento de columnas
            dgv.AllowUserToOrderColumns = false;

            // Deshabilitar el ordenamiento de columnas
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        // DATOS CLIENTES
        private void InicializarBotones()
        {
            // Al iniciar el formulario, ocultamos el botón Guardar y lo inhabilitamos
            btnGuardarDetalle.Visible = false;
            btnGuardarDetalle.Enabled = false;

            // El botón Actualizar se muestra y se habilita
            btnActualizarDetalle.Visible = true;
            btnActualizarDetalle.Enabled = true;
        }

        private void ConfigurarEstadoComboBox()
        {
            // Agregar opciones "Activo" y "No Activo" al ComboBox
            cboEstado.Items.Clear();
            cboEstado.Items.Add("Activo");
            cboEstado.Items.Add("No Activo");
        }

        private void RegistrarEventosComboBoxes()
        {
            cboManzana.SelectedIndexChanged += ActualizarDireccionCompleta;
            cboEdificio.SelectedIndexChanged += ActualizarDireccionCompleta;
            cboApartamento.SelectedIndexChanged += ActualizarDireccionCompleta;
        }

        private void ActualizarDireccionCompleta(object sender, EventArgs e)
        {
            try
            {
                // Verificar que todos los ComboBox tengan valores seleccionados antes de continuar
                if (cboManzana.SelectedItem == null || cboEdificio.SelectedItem == null || cboApartamento.SelectedItem == null)
                {
                    lblDireccionCompleta.Text = "Por favor, seleccione todos los campos.";
                    Utilidades.DireccionCompleta = null; // Limpia la variable pública en caso de error
                    return;
                }

                // Construir la dirección completa
                string direccionCompleta = $"{cboManzana.SelectedItem}-{cboEdificio.SelectedItem}-{cboApartamento.SelectedItem}";

                // Actualizar el Label y la variable pública
                lblDireccionCompleta.Text = direccionCompleta;
                Utilidades.DireccionCompleta = direccionCompleta; // Asignar el valor a la variable pública

            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                MessageBox.Show($"Ocurrió un error al actualizar la dirección completa: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblDireccionCompleta.Text = "Error al actualizar la dirección."; // Mostrar un mensaje en el Label
                Utilidades.DireccionCompleta = null; // Limpia la variable pública en caso de error
            }

        }

        // Puedes usar esta propiedad en otros métodos dentro de este formulario
        public void CargarDatosClienteSeleccionado()
        {
            // Validar si ID_Cliente es válido
            if (ID_Cliente <= 0)
            {
                MessageBox.Show("ID del cliente no válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Consultar los datos del cliente
                    string queryCliente = @"
                                            SELECT NombreCompleto, Documento, Celular1, Celular2, Telefono, Correo, EstadoCliente
                                            FROM Cliente
                                            WHERE ID_Cliente = @ID_Cliente";

                    SqlCommand cmdCliente = new SqlCommand(queryCliente, conn);
                    cmdCliente.Parameters.AddWithValue("@ID_Cliente", ID_Cliente);

                    SqlDataReader readerCliente = cmdCliente.ExecuteReader();
                    if (readerCliente.Read())
                    {
                        txtNombre.Text = readerCliente["NombreCompleto"].ToString();
                        txtDocumento.Text = readerCliente["Documento"].ToString();
                        txtCelular1.Text = readerCliente["Celular1"].ToString();
                        txtCelular2.Text = readerCliente["Celular2"].ToString();
                        txtTelefono.Text = readerCliente["Telefono"].ToString();
                        txtCorreo.Text = readerCliente["Correo"].ToString();
                        cboEstado.SelectedItem = readerCliente["EstadoCliente"].ToString();
                    }
                    readerCliente.Close();

                    // Consultar la dirección del cliente
                    string queryDireccion = @"
                                            SELECT Manzana, Edificio, Apartamento, Cuota, ProyectoNombre
                                            FROM Direccion
                                            WHERE ID_Cliente = @ID_Cliente";

                    SqlCommand cmdDireccion = new SqlCommand(queryDireccion, conn);
                    cmdDireccion.Parameters.AddWithValue("@ID_Cliente", ID_Cliente);

                    SqlDataReader readerDireccion = cmdDireccion.ExecuteReader();
                    string manzana = string.Empty;
                    string edificio = string.Empty;
                    string apartamento = string.Empty;
                    string cuota = string.Empty;
                    string proyectoNombre = string.Empty;

                    if (readerDireccion.Read())
                    {
                        manzana = readerDireccion["Manzana"].ToString();
                        edificio = readerDireccion["Edificio"].ToString();
                        apartamento = readerDireccion["Apartamento"].ToString();
                        cuota = readerDireccion["Cuota"].ToString();
                        proyectoNombre = readerDireccion["ProyectoNombre"].ToString();

                        lblProyecto.Text = proyectoNombre;
                    }
                    readerDireccion.Close();

                    // Consultar el ID_Proyecto
                    if (!string.IsNullOrEmpty(lblProyecto.Text))
                    {
                        string queryProyecto = @"
                                                SELECT ID_Proyecto
                                                FROM Proyecto
                                                WHERE NombreProyecto = @ProyectoNombre";

                        SqlCommand cmdProyecto = new SqlCommand(queryProyecto, conn);
                        cmdProyecto.Parameters.AddWithValue("@ProyectoNombre", lblProyecto.Text);

                        object result = cmdProyecto.ExecuteScalar();
                        if (result != null)
                        {
                            ID_Proyecto = Convert.ToInt32(result);

                            // Llamamos al método para obtener el ID_Direccion
                            ObtenerIDDireccionPorCliente();
                            LlenarCombosPorProyecto(conn, manzana, edificio, apartamento, cuota);
                        }
                        else
                        {
                            MessageBox.Show("No se encontró un proyecto con ese nombre.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para llenar los ComboBoxes y seleccionar los valores actuales
        private void LlenarCombosPorProyecto(SqlConnection conn, string manzana, string edificio, string apartamento, string cuota)
        {
            try
            {
                // Llenar y seleccionar cboManzana
                LlenarComboBox(conn, "SELECT Manzana FROM Manzana WHERE ID_Proyecto = @ID_Proyecto AND Estado = 'Activo'", cboManzana, manzana);

                // Llenar y seleccionar cboEdificio
                LlenarComboBox(conn, "SELECT Edificio FROM Edificio WHERE ID_Proyecto = @ID_Proyecto AND Estado = 'Activo' ", cboEdificio, edificio);

                // Llenar y seleccionar cboApartamento
                LlenarComboBox(conn, "SELECT Apartamento FROM Apartamento WHERE ID_Proyecto = @ID_Proyecto AND Estado = 'Activo'", cboApartamento, apartamento);

                // Llenar y seleccionar cboCuota
                LlenarComboBox(conn, "SELECT Cuota FROM Cuota WHERE ID_Proyecto = @ID_Proyecto AND Estado = 'Activo' ", cboCuota, cuota);



                // Actualizar lblDireccionCompleta
                ActualizarDireccionCompleta(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al llenar los ComboBoxes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método genérico para llenar un ComboBox y seleccionar un valor
        private void LlenarComboBox(SqlConnection conn, string query, ComboBox comboBox, string valorSeleccionado)
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ID_Proyecto", ID_Proyecto);

            SqlDataReader reader = cmd.ExecuteReader();
            comboBox.Items.Clear();
            while (reader.Read())
            {
                string item = reader[0].ToString();
                comboBox.Items.Add(item);
            }
            reader.Close();

            // Seleccionar el valor correspondiente
            if (!string.IsNullOrEmpty(valorSeleccionado) && comboBox.Items.Contains(valorSeleccionado))
            {
                comboBox.SelectedItem = valorSeleccionado;
            }
        }

        private void btnActualizarDetalle_Click(object sender, EventArgs e)
        {
            DesbloquearCampos();

            // Ocultar el botón Actualizar y mostrar el botón Guardar
            btnActualizarDetalle.Visible = false;
            btnActualizarDetalle.Enabled = false;

            btnGuardarDetalle.Visible = true;
            btnGuardarDetalle.Enabled = true;
        }

        private void BloquearCampos()
        {
            txtNombre.Enabled = false;
            txtDocumento.Enabled = false;
            txtCelular1.Enabled = false;
            txtCelular2.Enabled = false;
            txtTelefono.Enabled = false;
            txtCorreo.Enabled = false;
            cboEstado.Enabled = false;
            cboManzana.Enabled = false;
            cboEdificio.Enabled = false;
            cboApartamento.Enabled = false;
            cboCuota.Enabled = false;
        }

        private void DesbloquearCampos()
        {
            txtNombre.Enabled = true;
            txtDocumento.Enabled = true;
            txtCelular1.Enabled = true;
            txtCelular2.Enabled = true;
            txtTelefono.Enabled = true;
            txtCorreo.Enabled = true;
            cboEstado.Enabled = true;
            cboManzana.Enabled = true;
            cboEdificio.Enabled = true;
            cboApartamento.Enabled = true;
            cboCuota.Enabled = true;
        }

        private void btnGuardarDetalle_Click(object sender, EventArgs e)
        {
            // Confirmación para guardar los cambios
            DialogResult resultado = MessageBox.Show("¿Desea actualizar los datos?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                // Llamar al método para actualizar los datos en la base de datos
                ActualizarDatosCliente();

                // Volver a bloquear los campos
                BloquearCampos();
                 
                // Ocultar el botón Guardar y mostrar el botón Actualizar
                btnGuardarDetalle.Visible = false;
                btnGuardarDetalle.Enabled = false;

                btnActualizarDetalle.Visible = true;
                btnActualizarDetalle.Enabled = true;

                // Mensaje de éxito
                MessageBox.Show("Datos actualizados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ActualizarDatosCliente()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Actualizar los datos del cliente
                    string queryCliente = @"
                                            UPDATE Cliente
                                            SET NombreCompleto = @NombreCompleto, Documento = @Documento, 
                                                Celular1 = @Celular1, Celular2 = @Celular2, Telefono = @Telefono, 
                                                Correo = @Correo, EstadoCliente = @EstadoCliente
                                            WHERE ID_Cliente = @ID_Cliente";

                    SqlCommand cmdCliente = new SqlCommand(queryCliente, conn);
                    cmdCliente.Parameters.AddWithValue("@ID_Cliente", ID_Cliente);
                    cmdCliente.Parameters.AddWithValue("@NombreCompleto", txtNombre.Text);
                    cmdCliente.Parameters.AddWithValue("@Documento", txtDocumento.Text);
                    cmdCliente.Parameters.AddWithValue("@Celular1", txtCelular1.Text);
                    cmdCliente.Parameters.AddWithValue("@Celular2", txtCelular2.Text);
                    cmdCliente.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                    cmdCliente.Parameters.AddWithValue("@Correo", txtCorreo.Text);
                    cmdCliente.Parameters.AddWithValue("@EstadoCliente", cboEstado.SelectedItem.ToString());

                    cmdCliente.ExecuteNonQuery();

                    // Actualizar la dirección del cliente
                    string queryDireccion = @"
                                                UPDATE Direccion
                                                SET Manzana = @Manzana, Edificio = @Edificio, Apartamento = @Apartamento, Cuota = @Cuota
                                                WHERE ID_Cliente = @ID_Cliente";

                    SqlCommand cmdDireccion = new SqlCommand(queryDireccion, conn);
                    cmdDireccion.Parameters.AddWithValue("@ID_Cliente", ID_Cliente);
                    cmdDireccion.Parameters.AddWithValue("@Manzana", cboManzana.SelectedItem.ToString());
                    cmdDireccion.Parameters.AddWithValue("@Edificio", cboEdificio.SelectedItem.ToString());
                    cmdDireccion.Parameters.AddWithValue("@Apartamento", cboApartamento.SelectedItem.ToString());
                    cmdDireccion.Parameters.AddWithValue("@Cuota", cboCuota.SelectedItem.ToString());

                    cmdDireccion.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void CargarFacturas()
        {
            try
            {
                // Verificar que el ID_Cliente es válido
                if (ID_Cliente <= 0)
                {
                    MessageBox.Show("ID de cliente no válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Consulta SQL para obtener las facturas ordenadas por fecha
                    string queryFactura = @"
                                            SELECT 
                                                    f.ID_Factura,
                                                    f.FechaFactura,
                                                    f.Direccion,
                                                    f.TipoCuota,
                                                    f.ValorCuota
                                            FROM Factura f
                                            JOIN Direccion d ON f.ID_Direccion = d.ID_Direccion
                                            JOIN Cliente c ON d.ID_Cliente = c.ID_Cliente
                                            WHERE c.ID_Cliente = @ID_Cliente
                                            ORDER BY f.FechaFactura ASC"; // Orden ascendente por la fecha

                    using (SqlCommand cmd = new SqlCommand(queryFactura, conn))
                    {
                        cmd.Parameters.Add("@ID_Cliente", SqlDbType.Int).Value = ID_Cliente;

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable facturaTable = new DataTable();
                        adapter.Fill(facturaTable);

                        if (facturaTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Este Cliente aun no posee Facturas.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        // Modificar las columnas Direccion y FechaFactura
                        foreach (DataRow row in facturaTable.Rows)
                        {
                            // Ocultar el primer valor de la dirección
                            if (row["Direccion"] != DBNull.Value)
                            {
                                string direccion = row["Direccion"].ToString();
                                row["Direccion"] = OcultarPrimerValorDireccion(direccion);
                            }

                            // Formatear la fecha para mostrar solo la fecha sin la hora
                            if (row["FechaFactura"] != DBNull.Value && DateTime.TryParse(row["FechaFactura"].ToString(), out DateTime fechaFactura))
                            {
                                row["FechaFactura"] = fechaFactura.ToString("yyyy-MM-dd"); // Formato solo fecha
                            }
                        }

                        // Asignar los datos al DataGridView
                        dgvFactura.DataSource = facturaTable;

                        // Configurar tamaño de columnas específicas
                        dgvFactura.Columns["ID_Factura"].Width = 100;
                        dgvFactura.Columns["FechaFactura"].Width = 130;
                        dgvFactura.Columns["Direccion"].Width = 140;
                        dgvFactura.Columns["TipoCuota"].Width = 160;
                        dgvFactura.Columns["ValorCuota"].Width = 110;

                        // Aplicar formato directamente en la columna del DataGridView
                        dgvFactura.Columns["ValorCuota"].DefaultCellStyle.Format = "N2";
                        dgvFactura.Columns["ValorCuota"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la tabla Factura: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        public void CargarRecibos()
        {
            try
            {
                // Verificar que el ID_Cliente es válido
                if (ID_Cliente <= 0)
                {
                    MessageBox.Show("ID de cliente no válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string queryFactura = @"
                                            SELECT 
                                            r.ID_Recibo,
                                            r.FechaPago,
                                            r.Direccion,
                                            r.TipoPago,
                                            r.ValorPago,
                                            r.FormaDePago
                                    FROM Recibo r
                                    JOIN Direccion d ON r.ID_Direccion = d.ID_Direccion
                                    JOIN Cliente c ON d.ID_Cliente = c.ID_Cliente
                                    WHERE c.ID_Cliente = @ID_Cliente";

                    using (SqlCommand cmd = new SqlCommand(queryFactura, conn))
                    {
                        cmd.Parameters.Add("@ID_Cliente", SqlDbType.Int).Value = ID_Cliente;

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable facturaTable = new DataTable();
                        adapter.Fill(facturaTable);

                        if (facturaTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Este Cliente aun no posee Recibos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        // Modificar las columnas Direccion y FechaPago
                        foreach (DataRow row in facturaTable.Rows)
                        {
                            // Ocultar el primer valor y guion en la dirección
                            if (row["Direccion"] != DBNull.Value)
                            {
                                string direccion = row["Direccion"].ToString();
                                row["Direccion"] = OcultarPrimerValorDireccion(direccion);
                            }

                            // Formatear la fecha para mostrar solo la fecha sin la hora
                            if (row["FechaPago"] != DBNull.Value && DateTime.TryParse(row["FechaPago"].ToString(), out DateTime fechaPago))
                            {
                                row["FechaPago"] = fechaPago.ToString("yyyy-MM-dd"); // Solo fecha
                            }
                        }

                        dgvRecibo.DataSource = facturaTable;

                        // Configurar tamaño de columnas específicas
                        dgvRecibo.Columns["ID_Recibo"].Width = 100;
                        dgvRecibo.Columns["FechaPago"].Width = 120;
                        dgvRecibo.Columns["Direccion"].Width = 140;
                        dgvRecibo.Columns["TipoPago"].Width = 110;
                        dgvRecibo.Columns["ValorPago"].Width = 110;
                        dgvRecibo.Columns["FormaDePago"].Width = 250;

                        // Aplicar formato directamente en la columna del DataGridView
                        dgvRecibo.Columns["ValorPago"].DefaultCellStyle.Format = "N2";
                        dgvRecibo.Columns["ValorPago"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la tabla Recibo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para ocultar el primer valor y guion en la dirección
        private string OcultarPrimerValorDireccion(string direccion)
        {
            if (string.IsNullOrEmpty(direccion))
                return direccion;

            // Dividir la dirección en partes y eliminar la primera
            string[] partes = direccion.Split('-');
            if (partes.Length > 1)
            {
                return string.Join("-", partes.Skip(1)); // Reunir las partes sin el primer elemento
            }

            return direccion;
        }

        private void CargarObservaciones()
        {
            try
            {
                // Verificar que el ID_Cliente es válido
                if (ID_Cliente <= 0)
                {
                    MessageBox.Show("ID de cliente no válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string queryFactura = @"
                                            SELECT 
                                            o.FechaObservacion,
                                            o.Tipo,
                                            o.Descripcion,
                                            o.Usuario
                                    FROM Observacion o
                                    JOIN Direccion d ON o.ID_Direccion = d.ID_Direccion
                                    JOIN Cliente c ON d.ID_Cliente = c.ID_Cliente
                                    WHERE c.ID_Cliente = @ID_Cliente";

                    using (SqlCommand cmd = new SqlCommand(queryFactura, conn))
                    {
                        cmd.Parameters.Add("@ID_Cliente", SqlDbType.Int).Value = ID_Cliente;

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable facturaTable = new DataTable();
                        adapter.Fill(facturaTable);

                        if (facturaTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Este Cliente aun no posee Observaciones.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        // Asignar el origen de datos
                        dgvObservaciones.DataSource = facturaTable;

                        // Ajustar ancho de columnas
                        dgvObservaciones.Columns["FechaObservacion"].Width = 170;
                        dgvObservaciones.Columns["Tipo"].Width = 45;
                        dgvObservaciones.Columns["Descripcion"].Width = 593;
                        dgvObservaciones.Columns["Usuario"].Width = 115;

                        // Ajustar el alto de las filas automáticamente
                        dgvObservaciones.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                        dgvObservaciones.DefaultCellStyle.WrapMode = DataGridViewTriState.True; // Permitir que el texto se ajuste en varias líneas
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la tabla Observacione: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPagoFactura_Click(object sender, EventArgs e)
        {
            // Validación obligatoria antes de permitir pagos
            if (!ValidarClienteAntesDePago(ID_Cliente))
            {
                return; // ❌ Se bloquea el pago
            }

            // Verificar si el DataGridView tiene filas
            if (dgvFactura.Rows.Count == 0)
            {
                MessageBox.Show("No hay facturas disponibles para procesar el pago.",
                                "Información",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            // Si hay facturas, abrir el formulario Pago
            Pago openPago = new Pago(this);
            openPago.ShowDialog();
        }

        private void btnCredito_Click(object sender, EventArgs e)
        {
            Credito darCredito = new Credito(this);
            darCredito.ShowDialog();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que los campos no estén vacíos antes de guardar la observación
                if (string.IsNullOrWhiteSpace(txtTipoObservacion.Text))
                {
                    MessageBox.Show("El campo 'Tipo de Observación' no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Salir sin ejecutar el resto del código
                }

                if (string.IsNullOrWhiteSpace(rtbObservacion.Text))
                {
                    MessageBox.Show("El campo 'Observación' no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Salir sin ejecutar el resto del código
                }

                // Iniciar conexión y transacción
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Llamar al método para insertar la observación
                            InsertarObservacion(conn, transaction);

                            // Confirmar la transacción
                            transaction.Commit();

                            MessageBox.Show("Observación guardada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            CargarObservaciones();

                            LimpiarObservacion();

                        }
                        catch (Exception ex)
                        {
                            // Revertir transacción en caso de error
                            transaction.Rollback();
                            MessageBox.Show($"Error al guardar la observación: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        
        private void InsertarObservacion(SqlConnection conn, SqlTransaction transaction)
        {
            string query = @"INSERT INTO Observacion 
                     (ID_Usuario, ID_Direccion, Direccion, Tipo, Descripcion, Usuario, FechaObservacion)
                     VALUES 
                     (@ID_Usuario, @ID_Direccion, @Direccion, @Tipo, @Descripcion, @Usuario, @FechaObservacion);";

            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@ID_Usuario", Utilidades.IdUsuario);
                cmd.Parameters.AddWithValue("@ID_Direccion", Utilidades.ID_Direccion);
                cmd.Parameters.AddWithValue("@Direccion", ($"{Utilidades.IdProyectoSeleccionado}-{Utilidades.DireccionCompleta}"));
                cmd.Parameters.AddWithValue("@Tipo", (txtTipoObservacion.Text));
                cmd.Parameters.AddWithValue("@Descripcion", ReducirEspacios(rtbObservacion.Text));
                cmd.Parameters.AddWithValue("@Usuario", Utilidades.Usuario);
                cmd.Parameters.AddWithValue("@FechaObservacion", DateTime.Now);

                cmd.ExecuteNonQuery();
            }
        }

        // Método para reducir múltiples espacios a un solo espacio
        private string ReducirEspacios(string sinEspacio)
        {
            if (string.IsNullOrWhiteSpace(sinEspacio)) return string.Empty;
            return System.Text.RegularExpressions.Regex.Replace(sinEspacio.Trim(), @"\s+", " ");
        }

        private void LimpiarObservacion()
        {
            // Limpiar campos
            txtTipoObservacion.Clear();
            rtbObservacion.Clear();
        }

        private void LimitarTextObservacion(object sender, EventArgs e)
        {
            if (rtbObservacion.Text.Length > 250)
            {
                // Truncar el texto a 250 caracteres
                rtbObservacion.Text = rtbObservacion.Text.Substring(0, 250);
                rtbObservacion.SelectionStart = rtbObservacion.Text.Length; // Mantener el cursor al final

                // Mostrar un mensaje al usuario (opcional)
                MessageBox.Show("La Observacion no puede exceder los 250 caracteres.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LimitarTextTipoObservacion(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                // Filtrar caracteres no numéricos
                string textoFiltrado = new string(textBox.Text.Where(char.IsDigit).ToArray());

                // Limitar el texto a 3 caracteres numéricos
                if (textoFiltrado.Length > 3)
                {
                    textoFiltrado = textoFiltrado.Substring(0, 3);
                }

                // Actualizar el texto del TextBox solo si cambió
                if (textBox.Text != textoFiltrado)
                {
                    textBox.Text = textoFiltrado;
                    textBox.SelectionStart = textBox.Text.Length; // Mantener el cursor al final
                }
            }
        }

        private void dgvRecibo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0) // Verifica que no sea un encabezado
                {
                    DataGridViewRow row = dgvRecibo.Rows[e.RowIndex];
                    selectedIDRecibo = row.Cells["ID_Recibo"].Value?.ToString() ?? ""; // Obtén el valor de la columna ID_Recibo

                    // Obtener datos del recibo seleccionado
                    ObtenerDatosRecibo();

                    // Muestra la vista previa del recibo
                    printPreviewDialogRecibo.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al procesar la fila seleccionada: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ObtenerDatosRecibo()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT FechaPago, Usuario, Direccion, ValorPago, FormaDePago, DetalleBanco, DetallePago FROM Recibo WHERE ID_Recibo = @ID_Recibo";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID_Recibo", selectedIDRecibo);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                fechaPago = reader["FechaPago"].ToString();
                                usuario = reader["Usuario"].ToString();
                                direccion = reader["Direccion"].ToString();
                                monto = reader["ValorPago"].ToString();
                                formaPago = reader["FormaDePago"].ToString();
                                detalleBanco = reader["DetalleBanco"].ToString();
                                detalle = reader["DetallePago"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener datos del recibo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintRecibo_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Configuración general
            Font fontBold = new Font("Arial", 15, FontStyle.Bold);
            Font fontTema = new Font("Arial", 12, FontStyle.Bold);
            Font fontRegular = new Font("Arial", 12);
            Font fontPeque = new Font("Arial", 10);

            Brush brush = Brushes.Black;

            // Dimensiones del tamaño carta (A4) y área de media carta
            float pageWidth = e.PageBounds.Width; // Ancho total de la página (carta o A4)
            float pageHeight = e.PageBounds.Height; // Altura total de la página
            float mediaCartaHeight = pageHeight / 2; // Altura de media carta (mitad vertical de la página)

            // Margen y espaciado
            float marginX = 50; // Margen desde el borde izquierdo
            float marginY = 50; // Margen desde el borde superior
            float y = marginY; // Coordenada inicial Y
            float lineSpacing = 25; // Espaciado entre líneas

            // Configuración de imagen
            float imageHeight = lineSpacing * 5; // La imagen ocupará 6 líneas de alto
            float imageWidth = imageHeight; // La imagen será cuadrada

            // Centrar todo en el área de media carta
            RectangleF printArea = new RectangleF(
                marginX,
                marginY,
                pageWidth - 2 * marginX,
                mediaCartaHeight - 2 * marginY
            );

            string imagePath = Path.Combine(Application.StartupPath, "LOG.png");
            Image logo = Image.FromFile(imagePath);

            // Dibujar la imagen (alineada a la izquierda dentro del área)
            e.Graphics.DrawImage(logo, printArea.Left + 40, y, imageWidth, imageHeight);

            // Función auxiliar para centrar texto en el área
            void DrawCenteredText(string text, Font font, float posY)
            {
                SizeF textSize = e.Graphics.MeasureString(text, font);
                float posX = (printArea.Width - textSize.Width) / 2 + printArea.Left; // Centrar dentro del área de impresión
                e.Graphics.DrawString(text, font, brush, posX, posY);
            }

            // Línea 1: Nombre de la empresa
            DrawCenteredText("Condominio Al Día, CONDAY SRL", fontBold, y);
            y += lineSpacing;

            // Línea 2: RNC
            DrawCenteredText($"RNC:{Utilidades.RncProyecto}", fontRegular, y);
            y += lineSpacing * 3/4;

            // Línea 3: Residencial Actual
            DrawCenteredText($"{Utilidades.NombreProyectoSeleccionado}", fontBold, y);
            y += lineSpacing;

            // Línea 4: Dirección
            DrawCenteredText($"Dirección:{Utilidades.DireccionProyecto}", fontRegular, y);
            y += lineSpacing * 3/4;

            // Línea 5: Teléfono y WhatsApp
            DrawCenteredText($"Teléfono: {Utilidades.Oficina} - WhatsApp: {Utilidades.Telefono}", fontRegular, y);
            y += lineSpacing;

            // Línea 6: Espacio vacío
            y += lineSpacing;

            // Línea 7: Título del recibo
            DrawCenteredText("RECIBO DE PAGO", fontBold, y);
            y += lineSpacing;

            // Línea 8: Espacio vacío
            y += lineSpacing;

            // Línea 9: No. Recibo, Fecha, Usuario
            e.Graphics.DrawString($"No. Recibo: {selectedIDRecibo}", fontRegular, brush, printArea.Left + 20, y); // Izquierda
            DrawCenteredText($"Fecha:  {fechaPago}", fontRegular, y); // Centro
            e.Graphics.DrawString($"Usuario: {usuario}", fontRegular, brush, printArea.Right - 240, y); // Derecha
            y += lineSpacing;

            // Línea 10: Cliente y Dirección
            e.Graphics.DrawString($"Cliente: {NombreCompletoCliente}", fontRegular, brush, printArea.Left + 20, y); // Izquierda

            e.Graphics.DrawString($"Dirección: {OcultarPrimerValorDireccionPrint(direccion)}", fontRegular, brush, printArea.Right - 253, y); // Derecha

            y += lineSpacing;

            // Línea 11: Monto y Forma de Pago
            e.Graphics.DrawString($"Monto RD$:{Convert.ToDecimal(monto).ToString("N2")}", fontRegular, brush, printArea.Left + 20, y); // Izquierda
            e.Graphics.DrawString($"Forma de Pago: {formaPago}", fontRegular, brush, printArea.Right - 440, y); // Derecha
            y += lineSpacing;
            e.Graphics.DrawString($"Detalle de Banco: {detalleBanco}", fontRegular, brush, printArea.Right - 440, y); // Derecha
            y += lineSpacing;

            // Línea 12: Detalle de Pago
            e.Graphics.DrawString("Detalle de Pago", fontTema, brush, printArea.Left + 20, y);
            y += lineSpacing;

            // Calcular el ancho disponible entre los márgenes izquierdo y derecho
            float anchoDisponible = printArea.Width - (20 + 40);

            // Configurar el formato del texto para permitir saltos de línea
            StringFormat stringFormat = new StringFormat
            {
                Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.LineLimit // Permite múltiples líneas
            };

            // Dibujar el texto dentro del área definida
            RectangleF textArea = new RectangleF(printArea.Left + 20, y, anchoDisponible, lineSpacing * 5);
            e.Graphics.DrawString($"{detalle}", fontPeque, brush, textArea, stringFormat);

            // Líneas 13-14: Espacios vacíos
            y += lineSpacing * 2;

            // Líneas 15-16: Aviso
            e.Graphics.DrawString("* Revisar su recibo antes de irse.", fontTema, brush, printArea.Left + 20, y);
            y += lineSpacing ;
            e.Graphics.DrawString("* Conserve su recibo sellado para tener reclamo.", fontTema, brush, printArea.Left + 20, y);

            // Configuración de la pluma para dibujar una línea discontinua
            using (Pen dashedPen = new Pen(brush, 1))
            {
                dashedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                // Dibujar una línea intermitente (discontinua)
                e.Graphics.DrawLine(dashedPen, printArea.Left, y + lineSpacing, printArea.Right, y + lineSpacing);
            }

            // Aumentar el valor de Y después de la línea
            y += lineSpacing * 2;

        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            try
            {
                // Muestra la vista previa del recibo
                printPreviewDialogHistorial.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al procesar la fila seleccionada: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintHistorial_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Configuración general
            Font fontBold = new Font("Arial", 15, FontStyle.Bold);
            Font fontTema = new Font("Arial", 12, FontStyle.Bold);
            Font fontRegular = new Font("Arial", 12);
            Font fontPeque = new Font("Arial", 10);

            Brush brush = Brushes.Black;

            // Dimensiones del tamaño carta (A4) y área de media carta
            float pageWidth = e.PageBounds.Width; // Ancho total de la página (carta o A4)
            float pageHeight = e.PageBounds.Height; // Altura total de la página
            float mediaCartaHeight = pageHeight / 2; // Altura de media carta (mitad vertical de la página)

            // Margen y espaciado
            float marginX = 50; // Margen desde el borde izquierdo
            float marginY = 50; // Margen desde el borde superior
            float y = marginY; // Coordenada inicial Y
            float lineSpacing = 25; // Espaciado entre líneas

            // Configuración de imagen
            float imageHeight = lineSpacing * 5; // La imagen ocupará 6 líneas de alto
            float imageWidth = imageHeight; // La imagen será cuadrada

            // Centrar todo en el área de media carta
            RectangleF printArea = new RectangleF(
                marginX,
                marginY,
                pageWidth - 2 * marginX,
                mediaCartaHeight - 2 * marginY
            );

            string imagePath = Path.Combine(Application.StartupPath, "LOG.png");
            Image logo = Image.FromFile(imagePath);

            // Dibujar la imagen (alineada a la izquierda dentro del área)
            e.Graphics.DrawImage(logo, printArea.Left + 40, y, imageWidth, imageHeight);

            // Función auxiliar para centrar texto en el área
            void DrawCenteredText(string text, Font font, float posY)
            {
                SizeF textSize = e.Graphics.MeasureString(text, font);
                float posX = (printArea.Width - textSize.Width) / 2 + printArea.Left; // Centrar dentro del área de impresión
                e.Graphics.DrawString(text, font, brush, posX, posY);
            }

            // Línea 1: Nombre de la empresa
            DrawCenteredText("Condominio Al Día, CONDAY SRL", fontBold, y);
            y += lineSpacing;

            // Línea 2: RNC
            DrawCenteredText($"RNC: {Utilidades.RncProyecto}", fontRegular, y);
            y += lineSpacing;

            // Línea 3: Residencial Actual
            DrawCenteredText($"{Utilidades.NombreProyectoSeleccionado}", fontRegular, y);
            y += lineSpacing;

            // Línea 7: Título del recibo
            DrawCenteredText("Historico", fontBold, y);
            y += lineSpacing * 2;

            DrawCenteredText($"Fecha: {DateTime.Now:dd/MM/yyyy}", fontRegular, y); // Centro
            y += lineSpacing;

            // Línea 10: Cliente y Dirección
            e.Graphics.DrawString("Cliente: {Cliente}", fontRegular, brush, printArea.Left + 20, y); // Izquierda
            e.Graphics.DrawString($"Unidad: {Utilidades.DireccionCompleta}", fontRegular, brush, printArea.Right - 255, y); // Derecha
            y += lineSpacing;


        }

        private void dgvFactura_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvFactura.Rows.Count > 0) // Verifica que haya filas
                {
                    int lastRowIndex = dgvFactura.Rows.Count - 1; // Última fila

                    if (e.RowIndex == lastRowIndex) // Permitir solo en la última fila
                    {
                        DataGridViewRow row = dgvFactura.Rows[lastRowIndex];
                        selectedIDFactura = row.Cells["ID_Factura"].Value?.ToString() ?? "";

                        // Obtener datos del recibo seleccionado
                        ObtenerDatosFactura();

                        // Muestra la vista previa del recibo
                        printPreviewDialogFactura.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Solo puedes ver la última Factura.", "Acción no permitida",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al procesar la Factura seleccionada: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ObtenerDatosFactura()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT ID_Direccion, Direccion, ValorCuota, DetalleFactura, FechaFactura FROM Factura WHERE ID_Factura = @ID_Factura";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID_Factura", selectedIDFactura);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                iD_DireccionFactura = reader["ID_Direccion"].ToString();
                                direccionFactura = reader["Direccion"].ToString();
                                valorFactura = reader["ValorCuota"].ToString();
                                detalleFactura = reader["DetalleFactura"].ToString();
                                fechaFactura = reader["FechaFactura"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener datos del recibo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ObtenerDireccionFactura()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Deuda FROM Direccion WHERE ID_Cliente = @IDCliente";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IDCliente", ID_Cliente);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                deudaClienteFactura = reader["Deuda"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener datos del recibo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ObtenerClienteFactura()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Documento FROM Cliente WHERE ID_Cliente = @iDClienteFactura";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@iDClienteFactura", ID_Cliente);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                documentoClienteFactura = reader["Documento"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener datos del recibo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string OcultarPrimerValorDireccionPrint(string direccion)
        {
            if (string.IsNullOrEmpty(direccion))
                return direccion;

            // Dividir la dirección en partes y eliminar la primera
            string[] partes = direccion.Split('-');
            if (partes.Length > 1)
            {
                return string.Join("-", partes.Skip(1)); // Reunir las partes sin el primer elemento
            }

            return direccion;
        }

        private void rtbObservacion_KeyDown(object sender, KeyEventArgs e)
        {
            // Si se presiona Ctrl+V (pegar)
            if (e.Control && e.KeyCode == Keys.V)
            {
                if (Clipboard.ContainsImage())
                {
                    MessageBox.Show("No se permite pegar imágenes en este campo.",
                                    "Acción no permitida",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.SuppressKeyPress = true; // Bloquea la acción
                }
            }
        }

        private bool ValidarClienteAntesDePago(int idCliente)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            EstadoCliente,
                            Documento,
                            Celular1,
                            Correo
                        FROM Cliente
                        WHERE ID_Cliente = @ID_Cliente";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID_Cliente", idCliente);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show(
                                    "No se encontró el cliente asociado.",
                                    "Validación",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                );
                                return false;
                            }

                            string estadoCliente = reader["EstadoCliente"]?.ToString();
                            string documento = reader["Documento"]?.ToString();
                            string celular = reader["Celular1"]?.ToString();
                            string correo = reader["Correo"]?.ToString();

                            // 1️⃣ Validar estado del cliente
                            if (!estadoCliente.Equals("Activo", StringComparison.OrdinalIgnoreCase))
                            {
                                MessageBox.Show(
                                    "El cliente no está activo. No se pueden registrar pagos.",
                                    "Cliente Inactivo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                );
                                return false;
                            }

                            // 2️⃣ Validar campos obligatorios y detectar cuáles faltan
                            List<string> camposFaltantes = new List<string>();

                            if (string.IsNullOrWhiteSpace(documento))
                                camposFaltantes.Add("Documento");

                            if (string.IsNullOrWhiteSpace(celular))
                                camposFaltantes.Add("Celular");

                            if (string.IsNullOrWhiteSpace(correo))
                                camposFaltantes.Add("Correo");

                            if (camposFaltantes.Count > 0)
                            {
                                MessageBox.Show(
                                    "El cliente no tiene información completa.\n\n" +
                                    "Campos pendientes:\n- " + string.Join("\n- ", camposFaltantes) +
                                    "\n\nDebe actualizar estos datos antes de realizar pagos.",
                                    "Datos incompletos",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                );
                                return false;
                            }
                        }
                    }
                }

                return true; // ✅ Cliente válido
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al validar cliente: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
        }



        private void PrintFactura_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Configuración general
            Font fontBold = new Font("Arial", 15, FontStyle.Bold);
            Font fontTema = new Font("Arial", 12, FontStyle.Bold);
            Font fontRegular = new Font("Arial", 12);
            Font fontPeque = new Font("Arial", 10);

            Brush brush = Brushes.Black;

            // Dimensiones del tamaño carta (A4) y área de media carta
            float pageWidth = e.PageBounds.Width; // Ancho total de la página (carta o A4)
            float pageHeight = e.PageBounds.Height; // Altura total de la página
            float mediaCartaHeight = pageHeight / 2; // Altura de media carta (mitad vertical de la página)

            // Margen y espaciado
            float marginX = 50; // Margen desde el borde izquierdo
            float marginY = 50; // Margen desde el borde superior
            float y = marginY; // Coordenada inicial Y
            float lineSpacing = 25; // Espaciado entre líneas

            // Configuración de imagen
            float imageHeight = lineSpacing * 5; // La imagen ocupará 6 líneas de alto
            float imageWidth = imageHeight; // La imagen será cuadrada

            // Centrar todo en el área de media carta
            RectangleF printArea = new RectangleF(
                marginX,
                marginY,
                pageWidth - 2 * marginX,
                mediaCartaHeight - 2 * marginY
            );

            string imagePath = Path.Combine(Application.StartupPath, "LOG.png");
            Image logo = Image.FromFile(imagePath);

            // Calcular total de deuda
            decimal PendientedeudaClienteFactura = Convert.ToDecimal(deudaClienteFactura) - Convert.ToDecimal(valorFactura);

            // Dibujar la imagen (alineada a la izquierda dentro del área)
            e.Graphics.DrawImage(logo, printArea.Right - 140, y, imageWidth, imageHeight);

            // Función auxiliar para centrar texto en el área
            void DrawCenteredText(string text, Font font, float posY)
            {
                SizeF textSize = e.Graphics.MeasureString(text, font);
                float posX = (printArea.Width - textSize.Width) / 2 + printArea.Left; // Centrar dentro del área de impresión
                e.Graphics.DrawString(text, font, brush, posX, posY);
            }

            // Encabezado de la factura
            e.Graphics.DrawString("Condominio Al Dia, CONDAY SRL", fontBold, brush, printArea.Left + 20, y);
            y += lineSpacing;
            e.Graphics.DrawString($"{Utilidades.NombreProyectoSeleccionado}", fontBold, brush, printArea.Left + 20, y);
            y += lineSpacing;
            e.Graphics.DrawString($"RNC:{Utilidades.RncProyecto}", fontRegular, brush, printArea.Left + 21, y);
            y += lineSpacing * 3 / 4;
            e.Graphics.DrawString($"Teléfono: {Utilidades.Oficina}", fontRegular, brush, printArea.Left + 20, y);
            y += lineSpacing * 2;

            // Título de la factura
            DrawCenteredText("Factura por Servicio de Mantenimiento", fontBold, y);
            y += lineSpacing * 2;

            // INICIO CUADRADOS

            // Configuración del área izquierda
            float leftBoxWidth = printArea.Width / 2 - 30; // Mitad del ancho disponible menos margen
            RectangleF leftBox = new RectangleF(printArea.Left + 20, y, leftBoxWidth, lineSpacing * 3); // Un cuadro para el área izquierda

            // Configuración del área derecha
            float rightBoxWidth = printArea.Width / 2 - 30; // Mitad del ancho disponible menos margen
            RectangleF rightBox = new RectangleF(printArea.Right - rightBoxWidth - 20, y, rightBoxWidth, lineSpacing * 3); // Un cuadro para el área derecha

            // Dibujar los cuadros
            using (Pen boxPen = new Pen(brush, 1))
            {
                e.Graphics.DrawRectangle(boxPen, leftBox.Left, leftBox.Top, leftBox.Width, leftBox.Height); // Cuadro izquierdo
                e.Graphics.DrawRectangle(boxPen, rightBox.Left, rightBox.Top, rightBox.Width, rightBox.Height); // Cuadro derecho
            }

            // Coordenadas iniciales dentro de los cuadros
            float leftTextY = y + 5; // Margen interno superior
            float rightTextY = y + 5;

            // Dibujar texto en el cuadro izquierdo
            leftTextY += lineSpacing;
            e.Graphics.DrawString($"Factura No.: {selectedIDFactura}", fontRegular, brush, leftBox.Left + 10, leftTextY);
            leftTextY += lineSpacing;
            //e.Graphics.DrawString($"Fecha: {fechaFactura}", fontRegular, brush, leftBox.Left + 10, leftTextY);
            // Mostrar solo la fecha sin la hora
            if (DateTime.TryParse(fechaFactura, out DateTime fecha))
            {
                fechaFactura = fecha.ToString("yyyy-MM-dd"); // Solo fecha
            }

            e.Graphics.DrawString($"Fecha: {fechaFactura}", fontRegular, brush, leftBox.Left + 10, leftTextY);

            leftTextY += lineSpacing;

            // Dibujar texto en el cuadro derecho
            leftTextY += lineSpacing;
            
            // Definir el área de impresión para el nombre del cliente
            RectangleF clienteArea = new RectangleF(
                rightBox.Left + 10, // Posición X
                rightTextY,         // Posición Y
                rightBox.Width - 20, // Ancho del área (ajustado para dejar margen)
                lineSpacing * 2      // Altura del área (puedes ajustar según sea necesario)
            );

            // Dibujar el texto del cliente dentro del área definida
            using (StringFormat stringFormat = new StringFormat())
            {
                stringFormat.Alignment = StringAlignment.Near; // Alineado a la izquierda
                stringFormat.LineAlignment = StringAlignment.Near; // Alineado superior
                stringFormat.Trimming = StringTrimming.Word; // Recortar por palabras

                e.Graphics.DrawString($"Cliente: {NombreCompletoCliente}", fontRegular, brush, clienteArea, stringFormat);
            }

            rightTextY += lineSpacing;
            e.Graphics.DrawString($"Documento: {documentoClienteFactura}", fontRegular, brush, rightBox.Left + 10, rightTextY);
            rightTextY += lineSpacing;
            e.Graphics.DrawString($"Dirección: {OcultarPrimerValorDireccionPrint(direccionFactura)}", fontRegular, brush, rightBox.Left + 10, rightTextY);

            leftTextY += lineSpacing;

            // Actualizar coordenada Y para continuar con el resto del contenido
            y += Math.Max(leftBox.Height, rightBox.Height) + lineSpacing;

            // Configuración del cuadro
            float boxHeight = lineSpacing + 10; // Altura del cuadro con margen
            RectangleF detailBox = new RectangleF(printArea.Left + 20, y, printArea.Width - 40, boxHeight);

            // Dibujar el fondo del cuadro
            using (Brush grayBrush = new SolidBrush(Color.LightGray))
            {
                e.Graphics.FillRectangle(grayBrush, detailBox);
            }

            // Dibujar el borde del cuadro
            using (Pen boxPen = new Pen(brush, 1))
            {
                e.Graphics.DrawRectangle(boxPen, detailBox.Left, detailBox.Top, detailBox.Width, detailBox.Height);
            }

            // Márgenes internos
            float textMargin = 10; // Margen alrededor de las palabras
            float textY = y + 5; // Posición Y interna del texto

            // Calcular posiciones para cada columna
            float detalleWidth = detailBox.Width * 0.4f; // 40% del ancho para "Detalle"
            float cuotaWidth = (detailBox.Width - detalleWidth) * 0.4f; // Mitad del espacio restante para "Cuota"
            float moraWidth = cuotaWidth; // Igual a "Cuota"
            float totalX = detailBox.Right - textMargin; // Posición X para "Total"

            // Dibujar los textos en el cuadro
            e.Graphics.DrawString("Detalle", fontTema, brush, detailBox.Left + textMargin, textY); // "Detalle" al 40%
            e.Graphics.DrawString("Cuota", fontTema, brush, detailBox.Left + detalleWidth + textMargin, textY); // "Cuota" después de "Detalle"
            e.Graphics.DrawString("Mora", fontTema, brush, detailBox.Left + detalleWidth + cuotaWidth + textMargin, textY); // "Mora" después de "Cuota"
            SizeF totalSize = e.Graphics.MeasureString("Total", fontTema);
            e.Graphics.DrawString("Total", fontTema, brush, totalX - totalSize.Width, textY); // "Total" alineado a la derecha

            // Actualizar posición Y para la siguiente línea de contenido
            y += boxHeight;

            // Agregar valores debajo del cuadro
            float valuesY = y; // Posición Y para los valores debajo del cuadro
            // Dibujar los valores alineados debajo de cada palabra
            e.Graphics.DrawString($"{detalleFactura}", fontRegular, brush, detailBox.Left + textMargin, valuesY); // Valor debajo de "Detalle"
            e.Graphics.DrawString("1 Cuota", fontRegular, brush, detailBox.Left + detalleWidth + textMargin, valuesY); // Valor debajo de "Cuota"
            e.Graphics.DrawString("{valorMora}", fontRegular, brush, detailBox.Left + detalleWidth + cuotaWidth + textMargin, valuesY); // Valor debajo de "Mora"
            SizeF totalValueSize = e.Graphics.MeasureString("RD$ 5,500.00", fontRegular);
            e.Graphics.DrawString($" {Convert.ToDecimal(valorFactura).ToString("N2")} ", fontRegular, brush, totalX - totalValueSize.Width , valuesY); // Valor debajo de "Total"
            y += lineSpacing * 2;

            // Texto a mostrar
            string headerText = "";
            string messageText = "RECUERDE: Que al pagar su cuota de mantenimiento a tiempo, contribuye a mantener las áreas comunes de la mejor manera.";

            // Definir área de impresión (de la izquierda a la mitad de la página)
            float textAreaWidth = printArea.Width / 2 - 30; // Mitad izquierda de la página con márgenes
            RectangleF textArea = new RectangleF(printArea.Left + 30, y, textAreaWidth, lineSpacing * 4);

            // Dibujar el encabezado
            e.Graphics.DrawString(headerText, fontRegular, brush, textArea.Left, textArea.Top);
            y += lineSpacing; // Incrementar Y para el mensaje

            // Ajustar y dividir el mensaje en líneas
            using (StringFormat stringFormat = new StringFormat())
            {
                stringFormat.Alignment = StringAlignment.Near; // Alineado a la izquierda
                stringFormat.LineAlignment = StringAlignment.Near; // Alineado superior

                // Crear una fuente más pequeña para el mensaje, si es necesario
                using (Font fontSmall = new Font(fontRegular.FontFamily, fontRegular.Size - 1, fontRegular.Style))
                {
                    e.Graphics.DrawString(messageText, fontSmall, brush, new RectangleF(textArea.Left, y, textAreaWidth, lineSpacing * 3), stringFormat);
                }
            }

            // Definir el cuadro que ocupará la mitad derecha de la página
            float boxWidth = printArea.Width / 2 - 40; // Mitad derecha de la página (con márgenes)
            float boxX = printArea.Left + printArea.Width / 2 + 20; // Comienza desde la mitad de la página hacia la derecha
            RectangleF totalBox = new RectangleF(boxX, y, boxWidth, lineSpacing * 3); // Cuadro para "Pendiente" y "Total a Pagar"

            // Dibujar el fondo blanco para el cuadro
            using (Brush whiteBrush = new SolidBrush(Color.White))
            {
                e.Graphics.FillRectangle(whiteBrush, totalBox);
            }

            // Dibujar el borde del cuadro
            using (Pen boxPen = new Pen(brush, 1))
            {
                e.Graphics.DrawRectangle(boxPen, totalBox.Left, totalBox.Top, totalBox.Width, totalBox.Height);
            }

            // Dibujar los textos dentro del cuadro, alineados a la derecha
            float textX = totalBox.Right - 20; // Alineado a la derecha con un margen
            
            // Mostrar valores con formato numérico de dos decimales
            e.Graphics.DrawString($"Pendiente RD$ {Convert.ToDecimal(PendientedeudaClienteFactura).ToString("N2")}", fontRegular, brush, textX - e.Graphics.MeasureString("Pendiente RD$ 5,500.00", fontRegular).Width, totalBox.Top + 5);
            y += lineSpacing * 2; // Incrementar Y para la siguiente línea

            e.Graphics.DrawString($"Total a Pagar: RD$ {Convert.ToDecimal(deudaClienteFactura).ToString("N2")}", fontRegular, brush, textX - e.Graphics.MeasureString("Total a Pagar: RD$ 5,500.00", fontRegular).Width, totalBox.Top + lineSpacing + 5);
            y += lineSpacing;

            // Nota
            e.Graphics.DrawString("Nota: Fecha Límite de Pago Día 30", fontTema, brush, printArea.Right - 310, y);
            y += lineSpacing * 2;

            // Medios de pago
            e.Graphics.DrawString("Medios de Pago:", fontTema, brush, printArea.Left + 20, y);
            y += lineSpacing;
            e.Graphics.DrawString("-> Transferencia bancaria o Depósitos", fontRegular, brush, printArea.Left + 20, y);
            y += lineSpacing;
            e.Graphics.DrawString("Banco APAP - Cuenta Ahorro No.: 1030015694", fontPeque, brush, printArea.Left + 50, y);
            y += lineSpacing;
            e.Graphics.DrawString("Banco Popular - Cuenta Ahorro No.: 797-104-825", fontPeque, brush, printArea.Left + 50, y);
            y += lineSpacing;
            e.Graphics.DrawString("Banco BHD - Cuenta Ahorro No.: 04984220028", fontPeque, brush, printArea.Left + 50, y);
            y += lineSpacing;
            e.Graphics.DrawString("Banco Banreservas - Cuenta Ahorro No.: 9606086175", fontPeque, brush, printArea.Left + 50, y);
            y += lineSpacing;
            e.Graphics.DrawString("-> Efectivo en Oficina", fontRegular, brush, printArea.Left + 20, y);
            y += lineSpacing * 2;

            // Información de contacto
            e.Graphics.DrawString("Escribir al correo: cobros@condominioaldia.com.do, o dirigirse a la oficina de nosotros en su proyecto.", fontPeque, brush, printArea.Left + 20, y);
            y += lineSpacing;
            e.Graphics.DrawString($"Teléfono: {Utilidades.Oficina} / {Utilidades.Telefono}", fontPeque, brush, printArea.Left + 20, y);
            y += lineSpacing * 2;

            // Finalización de la factura
            using (Pen dashedPen = new Pen(brush, 1))
            {
                dashedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                e.Graphics.DrawLine(dashedPen, printArea.Left, y, printArea.Right, y);
            }
            
        }

        private void btnAnularRecibo_Click(object sender, EventArgs e)
        {
            if (dgvRecibo.SelectedRows.Count == 0)
            {
                MessageBox.Show("Primero seleccione un Recibo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmacion = MessageBox.Show("¿Está seguro de que desea Anular este Recibo?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacion == DialogResult.No)
            {
                return;
            }

            int idRecibo = Convert.ToInt32(dgvRecibo.SelectedRows[0].Cells["ID_Recibo"].Value);
            string direccion = "";
            decimal valorPago = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    // Obtener Dirección y ValorPago del Recibo
                    using (SqlCommand cmd = new SqlCommand("SELECT Direccion, ValorPago FROM Recibo WHERE ID_Recibo = @ID_Recibo", conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ID_Recibo", idRecibo);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                direccion = reader["Direccion"].ToString();
                                valorPago = Convert.ToDecimal(reader["ValorPago"]);
                            }
                            else
                            {
                                MessageBox.Show("No se encontró el Recibo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    // Verificar si existe el registro en la tabla Dirección
                    using (SqlCommand cmd = new SqlCommand("SELECT Deuda FROM Direccion WHERE DireccionCompleta = @Direccion", conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Direccion", direccion);
                        object deudaActual = cmd.ExecuteScalar();

                        if (deudaActual == null)
                        {
                            MessageBox.Show("No se encontró la Dirección asociada al Recibo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        decimal deudaNueva = Convert.ToDecimal(deudaActual) + valorPago;

                        // Actualizar la Deuda en la tabla Dirección
                        using (SqlCommand updateCmd = new SqlCommand("UPDATE Direccion SET Deuda = @Deuda WHERE DireccionCompleta = @Direccion", conn, transaction))
                        {
                            updateCmd.Parameters.AddWithValue("@Deuda", deudaNueva);
                            updateCmd.Parameters.AddWithValue("@Direccion", direccion);
                            updateCmd.ExecuteNonQuery();
                        }
                    }

                    // Eliminar el registro de la tabla Historial
                    using (SqlCommand deleteHistorialCmd = new SqlCommand("DELETE FROM Historial WHERE ID_Recibo = @ID_Recibo", conn, transaction))
                    {
                        deleteHistorialCmd.Parameters.AddWithValue("@ID_Recibo", idRecibo);
                        deleteHistorialCmd.ExecuteNonQuery();
                    }

                    // Eliminar el registro de la tabla Recibo
                    using (SqlCommand deleteReciboCmd = new SqlCommand("DELETE FROM Recibo WHERE ID_Recibo = @ID_Recibo", conn, transaction))
                    {
                        deleteReciboCmd.Parameters.AddWithValue("@ID_Recibo", idRecibo);
                        deleteReciboCmd.ExecuteNonQuery();
                    }

                    // Confirmar la transacción
                    transaction.Commit();
                    MessageBox.Show("Recibo Cancelado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CargarRecibos();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error al cancelar el Recibo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}