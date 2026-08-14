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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AdminSoftPF
{
    public partial class Reportes : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        public Reportes()
        {
            InitializeComponent();
            ConfigureComboBoxes();
            LlenarCbos();
            ConfigureTextBoxes();
        }

        private void Reportes_Load(object sender, EventArgs e)
        {
            CargarProyectosCbo();
        }

        private void ConfigureComboBoxes()
        {
            // Configurar los ComboBox para que no sean editables
            cboReporte.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProyecto.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFormato.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LlenarCbos()
        {
            // Opciones para el primer ComboBox
            var opcionesReportes = new List<string> 
            {
                "Observación por Fecha",
                "Cobro por Fecha",
                "Deuda por Balance",
                "Información Clientes",
                "Información Proyecto",
                "Gestion por Cartera",
                "Autorizaciones Proyecto",
                "Balance Rapido"
            };
            cboReporte.Items.AddRange(opcionesReportes.ToArray()); 

            // Opciones para el tercer ComboBox
            var opcionesFormato = new List<string> 
            { 
                "Excel",
                "TXT",
                "PDF" 
            };
            cboFormato.Items.AddRange(opcionesFormato.ToArray());
        }

        private void ConfigureTextBoxes()
        {
            // Configurar eventos para limitar el número de caracteres en los TextBox
            txtRangoInicial.MaxLength = 2;
            txtRangoFinal.MaxLength = 2;
        }

        private void CargarProyectosCbo()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                                    SELECT ID_Proyecto, NombreProyecto
                                    FROM Proyecto
                                    WHERE EstadoProyecto = 'Activo'
                                    ORDER BY NombreProyecto";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cboProyecto.DataSource = dt;
                    cboProyecto.DisplayMember = "NombreProyecto";
                    cboProyecto.ValueMember = "ID_Proyecto";
                    cboProyecto.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar proyectos: " + ex.Message);
            }
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            if (cboReporte.SelectedItem != null)
            {
                string seleccion = cboReporte.SelectedItem.ToString();

                if (seleccion == "Deuda por Balance" || seleccion == "Información Clientes" || seleccion == "Información Proyecto" || seleccion == "Gestion por Cartera")
                {
                    // No se requieren fechas para estos reportes, llamamos directamente a la función correspondiente
                    switch (seleccion)
                    {
                        case "Deuda por Balance":
                            CargarDeudaBalance();
                            break;
                        case "Información Clientes":
                            CargarInformacionClientes();
                            break;
                        case "Información Proyecto":
                            CargarInformacionProyecto();  
                            break;
                        case "Gestion por Cartera":
                            CargarGestionCartera();  
                            break; 
                        
                    }
                }
                else
                {
                    // Si es otro reporte, entonces validamos las fechas
                    if (dtpInicial.Value != null && dtpFinal.Value != null)
                    {
                        if (dtpInicial.Value <= dtpFinal.Value)
                        {
                            switch (seleccion)
                            {
                                case "Observación por Fecha":
                                    CargarObservaciones(dtpInicial.Value, dtpFinal.Value);
                                    break;
                                case "Cobro por Fecha":
                                    CargarCobros(dtpInicial.Value, dtpFinal.Value);
                                    break;
                                case "Autorizaciones Proyecto":
                                    Autorizaciones(dtpInicial.Value, dtpFinal.Value);
                                    break;
                                default:
                                    MessageBox.Show("Opción no implementada.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                case "Balance Rapido":
                                    CargarBalanceRapido(dtpInicial.Value, dtpFinal.Value);
                                    break;

                            }
                        }
                        else
                        {
                            MessageBox.Show("La fecha de inicio no puede ser mayor que la fecha final.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe seleccionar un rango de fechas válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione una opción válida en el combo box antes de generar el reporte.",
                                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CargarObservaciones(DateTime fechaInicio, DateTime fechaFinal)
        {
            try
            {
                int? idProyecto = null;

                // Validar si hay proyecto seleccionado
                if (cboProyecto.SelectedIndex >= 0)
                {
                    idProyecto = Convert.ToInt32(cboProyecto.SelectedValue);
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                                    SELECT 
                                        Direccion,
                                        Tipo,
                                        Descripcion,
                                        Usuario,
                                        FechaObservacion
                                    FROM Observacion
                                    WHERE FechaObservacion >= @FechaInicio
                                      AND FechaObservacion < DATEADD(DAY, 1, @FechaFinal)
                                      AND (
                                            @ID_Proyecto IS NULL OR
                                            CAST(
                                                LEFT(Direccion, CHARINDEX('-', Direccion + '-') - 1)
                                                AS INT
                                            ) = @ID_Proyecto
                                          )
                                    ORDER BY FechaObservacion DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Fechas sin hora
                        command.Parameters.AddWithValue("@FechaInicio", fechaInicio.Date);
                        command.Parameters.AddWithValue("@FechaFinal", fechaFinal.Date);

                        // Parámetro del proyecto (opcional)
                        if (idProyecto.HasValue)
                            command.Parameters.AddWithValue("@ID_Proyecto", idProyecto.Value);
                        else
                            command.Parameters.AddWithValue("@ID_Proyecto", DBNull.Value);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            dgvReportes.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al cargar observaciones: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void CargarCobros(DateTime fechaInicio, DateTime fechaFinal)
        {
            try
            {
                int? idProyecto = null;

                if (cboProyecto.SelectedIndex >= 0)
                    idProyecto = Convert.ToInt32(cboProyecto.SelectedValue);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                                    SELECT 
                                        Direccion,
                                        ID_Recibo,
                                        FormaDePago,
                                        Usuario,
                                        ValorPago,
                                        DetallePago,
                                        DetalleBanco,
                                        FechaPago
                                    FROM Recibo
                                    WHERE FechaPago >= @FechaInicio
                                      AND FechaPago < DATEADD(DAY, 1, @FechaFinal)
                                      AND (
                                            @ID_Proyecto IS NULL OR
                                            CAST(
                                                LEFT(Direccion, CHARINDEX('-', Direccion + '-') - 1)
                                                AS INT
                                            ) = @ID_Proyecto
                                          )
                                    ORDER BY FechaPago DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FechaInicio", fechaInicio.Date);
                        command.Parameters.AddWithValue("@FechaFinal", fechaFinal.Date);

                        if (idProyecto.HasValue)
                            command.Parameters.AddWithValue("@ID_Proyecto", idProyecto.Value);
                        else
                            command.Parameters.AddWithValue("@ID_Proyecto", DBNull.Value);

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvReportes.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar cobros: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDeudaBalance()
        {
            try
            {
                int? idProyecto = null;

                if (cboProyecto.SelectedIndex >= 0)
                    idProyecto = Convert.ToInt32(cboProyecto.SelectedValue);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                                    SELECT 
                                        d.DireccionCompleta,
                                        c.NombreCompleto,
                                        d.Cuota,
                                        d.Deuda,
                                        c.EstadoCliente
                                    FROM Direccion d
                                    INNER JOIN Cliente c ON d.ID_Cliente = c.ID_Cliente
                                    WHERE (
                                            @ID_Proyecto IS NULL OR
                                            CAST(
                                                LEFT(d.DireccionCompleta, CHARINDEX('-', d.DireccionCompleta + '-') - 1)
                                                AS INT
                                            ) = @ID_Proyecto
                                          )";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (idProyecto.HasValue)
                            command.Parameters.AddWithValue("@ID_Proyecto", idProyecto.Value);
                        else
                            command.Parameters.AddWithValue("@ID_Proyecto", DBNull.Value);

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvReportes.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al cargar deuda balance: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void CargarInformacionClientes()
        {
            try
            {
                int? idProyecto = null;

                if (cboProyecto.SelectedIndex >= 0)
                    idProyecto = Convert.ToInt32(cboProyecto.SelectedValue);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                                    SELECT 
                                        d.*,
                                        c.*
                                    FROM Direccion d
                                    INNER JOIN Cliente c ON d.ID_Cliente = c.ID_Cliente
                                    WHERE (
                                            @ID_Proyecto IS NULL OR
                                            CAST(
                                                LEFT(d.DireccionCompleta, CHARINDEX('-', d.DireccionCompleta + '-') - 1)
                                                AS INT
                                            ) = @ID_Proyecto
                                          )";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (idProyecto.HasValue)
                            command.Parameters.AddWithValue("@ID_Proyecto", idProyecto.Value);
                        else
                            command.Parameters.AddWithValue("@ID_Proyecto", DBNull.Value);

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvReportes.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al cargar información de clientes: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void CargarInformacionProyecto()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta SQL para obtener toda la información de la tabla Proyecto
                    string query = "SELECT * FROM Proyecto";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // Cargar los datos al DataGridView
                            dgvReportes.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar información del proyecto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarGestionCartera()
        {
            try
            {
                int? idProyecto = null;

                if (cboProyecto.SelectedIndex >= 0)
                    idProyecto = Convert.ToInt32(cboProyecto.SelectedValue);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                                    SELECT 
                                        c.NombreCompleto,
                                        c.Celular1,
                                        c.Celular2,
                                        c.Telefono,
                                        c.Correo,
                                        d.DireccionCompleta,
                                        d.Deuda
                                    FROM Cliente c
                                    INNER JOIN Direccion d ON c.ID_Cliente = d.ID_Cliente
                                    WHERE c.EstadoCliente = 'Activo'
                                      AND (
                                            @ID_Proyecto IS NULL OR
                                            CAST(
                                                LEFT(d.DireccionCompleta, CHARINDEX('-', d.DireccionCompleta + '-') - 1)
                                                AS INT
                                            ) = @ID_Proyecto
                                          )";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (idProyecto.HasValue)
                            cmd.Parameters.AddWithValue("@ID_Proyecto", idProyecto.Value);
                        else
                            cmd.Parameters.AddWithValue("@ID_Proyecto", DBNull.Value);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvReportes.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al cargar gestión por cartera: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void Autorizaciones(DateTime fechaInicio, DateTime fechaFinal)
        {
            try
            {
                int? idProyecto = null;

                if (cboProyecto.SelectedIndex >= 0)
                    idProyecto = Convert.ToInt32(cboProyecto.SelectedValue);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                                    SELECT 
                                        ID_Autorizacion,
                                        ID_Proyecto,
                                        ID_Usuario,
                                        Proveedor,
                                        ValorAutorizacion,
                                        Concepto,
                                        FormaDePago,
                                        CuentaDePago,
                                        TipoDePago,
                                        Fecha
                                    FROM Autorizaciones
                                    WHERE Fecha >= @FechaInicio
                                      AND Fecha < DATEADD(DAY, 1, @FechaFinal)
                                      AND (@ID_Proyecto IS NULL OR ID_Proyecto = @ID_Proyecto)
                                    ORDER BY Fecha DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio.Date);
                        cmd.Parameters.AddWithValue("@FechaFinal", fechaFinal.Date);

                        if (idProyecto.HasValue)
                            cmd.Parameters.AddWithValue("@ID_Proyecto", idProyecto.Value);
                        else
                            cmd.Parameters.AddWithValue("@ID_Proyecto", DBNull.Value);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvReportes.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void CargarBalanceRapido(DateTime fechaInicio, DateTime fechaFinal)
        {
            try
            {
                int? idProyecto = null;

                // Validar si hay proyecto seleccionado
                if (cboProyecto.SelectedIndex >= 0)
                {
                    idProyecto = Convert.ToInt32(cboProyecto.SelectedValue);
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                                    SELECT 
                                        FormaDePago,
                                        SUM(TotalRecibos) AS TotalRecibos,
                                        SUM(TotalAutorizaciones) AS TotalAutorizaciones,
                                        SUM(TotalRecibos) - SUM(TotalAutorizaciones) AS Balance
                                    FROM
                                    (
                                        /* ===================== RECIBOS ===================== */
                                        SELECT 
                                            FormaDePago,
                                            SUM(ValorPago) AS TotalRecibos,
                                            0 AS TotalAutorizaciones
                                        FROM Recibo
                                        WHERE FechaPago >= @FechaInicio
                                          AND FechaPago < DATEADD(DAY, 1, @FechaFinal)
                                          AND TipoPago = 'Recibo'
                                          AND (
                                                @ID_Proyecto IS NULL OR
                                                CAST(
                                                    LEFT(Direccion, CHARINDEX('-', Direccion + '-') - 1)
                                                    AS INT
                                                ) = @ID_Proyecto
                                              )
                                        GROUP BY FormaDePago

                                        UNION ALL

                                        /* ================= AUTORIZACIONES ================= */
                                        SELECT 
                                            FormaDePago,
                                            0 AS TotalRecibos,
                                            SUM(ValorAutorizacion) AS TotalAutorizaciones
                                        FROM Autorizaciones
                                        WHERE Fecha >= @FechaInicio
                                          AND Fecha < DATEADD(DAY, 1, @FechaFinal)
                                          AND (
                                                @ID_Proyecto IS NULL OR
                                                ID_Proyecto = @ID_Proyecto
                                              )
                                        GROUP BY FormaDePago
                                    ) AS Datos
                                    GROUP BY FormaDePago
                                    ORDER BY FormaDePago";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio.Date);
                        cmd.Parameters.AddWithValue("@FechaFinal", fechaFinal.Date);

                        if (idProyecto.HasValue)
                            cmd.Parameters.AddWithValue("@ID_Proyecto", idProyecto.Value);
                        else
                            cmd.Parameters.AddWithValue("@ID_Proyecto", DBNull.Value);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvReportes.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al generar Balance Rápido: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            cboReporte.SelectedItem = null;
            cboProyecto.SelectedItem = null;
            cboFormato.SelectedItem = null;

            txtRangoInicial.Clear();
            txtRangoFinal.Clear();

            dtpInicial.Value = DateTime.Now;
            dtpFinal.Value = DateTime.Now;

            dgvReportes.DataSource = null;

            dgvReportes.Rows.Clear();
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            // Verificar si el ComboBox de formato tiene una selección válida
            if (cboFormato.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione un formato (Excel, PDF o TXT).", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verificar si dgvReportes tiene datos
            if (dgvReportes == null || dgvReportes.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos en el reporte para exportar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verificar el formato seleccionado y llamar a la función de exportación correspondiente
            string formatoSeleccionado = cboFormato.SelectedItem.ToString();

            if (formatoSeleccionado == "Excel")
            {
                ExportarAExcel();
            }
            else if (formatoSeleccionado == "PDF")
            {
                ExportarAPdf();
            }
            else if (formatoSeleccionado == "TXT")
            {
                ExportarATxt();
            }
            else
            {
                MessageBox.Show("Formato no válido seleccionado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ExportarAExcel()
        {
            try
            {
                // Crear una instancia de un Excel Application
                var excelApp = new Microsoft.Office.Interop.Excel.Application();
                var workbooks = excelApp.Workbooks;
                var workbook = workbooks.Add();
                var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];

                // Establecer los nombres de las columnas en la primera fila
                for (int i = 0; i < dgvReportes.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dgvReportes.Columns[i].HeaderText;
                }

                // Llenar las filas con los datos
                for (int row = 0; row < dgvReportes.Rows.Count; row++)
                {
                    for (int col = 0; col < dgvReportes.Columns.Count; col++)
                    {
                        worksheet.Cells[row + 2, col + 1] = dgvReportes.Rows[row].Cells[col].Value.ToString();
                    }
                }

                // Mostrar el archivo Excel
                excelApp.Visible = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar a Excel: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportarAPdf(){}

        private void ExportarATxt()
        {
            try
            {
                // Abrir un cuadro de diálogo para elegir la ubicación y el nombre del archivo
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Archivo de texto (*.txt)|*.txt";
                    saveFileDialog.Title = "Guardar Reporte como TXT";
                    saveFileDialog.FileName = "Reporte.txt";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        StringBuilder sb = new StringBuilder();

                        // Agregar los encabezados de las columnas
                        for (int i = 0; i < dgvReportes.Columns.Count; i++)
                        {
                            sb.Append(dgvReportes.Columns[i].HeaderText);
                            if (i < dgvReportes.Columns.Count - 1)
                            {
                                sb.Append("\t"); // Separar con tabulaciones
                            }
                        }
                        sb.AppendLine();

                        // Agregar los datos de las filas
                        foreach (DataGridViewRow row in dgvReportes.Rows)
                        {
                            if (!row.IsNewRow) // Ignorar la fila nueva vacía
                            {
                                for (int i = 0; i < dgvReportes.Columns.Count; i++)
                                {
                                    sb.Append(row.Cells[i].Value?.ToString() ?? "");
                                    if (i < dgvReportes.Columns.Count - 1)
                                    {
                                        sb.Append("\t"); // Separar con tabulaciones
                                    }
                                }
                                sb.AppendLine();
                            }
                        }

                        // Guardar el contenido en el archivo TXT
                        System.IO.File.WriteAllText(saveFileDialog.FileName, sb.ToString(), Encoding.UTF8);

                        MessageBox.Show("Reporte exportado exitosamente a TXT.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar a TXT: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}