using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminSoftPF
{
    public partial class Autorizaciones: Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        private DataGridViewRow selectedRow;
        public Autorizaciones()
        {
            InitializeComponent();
            CargarAutorizaciones();
        }

        private void Autorizaciones_Load(object sender, EventArgs e)
        {
            Utilidades.CargarDatosProyecto();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            using (NuevaAutorizacion agregarAutorizacion = new NuevaAutorizacion())
            {
                agregarAutorizacion.ShowDialog();
                CargarAutorizaciones();
            }
        }

        private void CargarAutorizaciones()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT ID_Autorizacion, Proveedor, Concepto, ValorAutorizacion, TipoDePago, FormaDePago, CuentaDePago, Fecha FROM Autorizaciones WHERE Id_Proyecto = @IdProyecto";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdProyecto", Utilidades.IdProyectoSeleccionado);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvAutorizaciones.DataSource = dt;

                        // Aplicar formato directamente en la columna del DataGridView
                        dgvAutorizaciones.Columns["ValorAutorizacion"].DefaultCellStyle.Format = "N2";
                        dgvAutorizaciones.Columns["ValorAutorizacion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar autorizaciones: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvAutorizaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvAutorizaciones.Rows.Count)
            {
                selectedRow = dgvAutorizaciones.Rows[e.RowIndex];
                using (PrintDocument printDoc = new PrintDocument())
                {
                    printDoc.PrintPage += PrintReceipt;
                    using (PrintPreviewDialog previewDialog = new PrintPreviewDialog())
                    {
                        previewDialog.Document = printDoc;
                        previewDialog.WindowState = FormWindowState.Maximized;
                        previewDialog.ShowDialog();
                    }
                }
            }
        }

        private void PrintReceipt(object sender, PrintPageEventArgs e)
        {
            if (selectedRow == null)
            {
                MessageBox.Show("No hay autorización seleccionada para imprimir.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Font fontBold = new Font("Arial", 12, FontStyle.Bold);
            Font fontRegular = new Font("Arial", 10);
            Font fontPeque = new Font("Arial", 8);
            Brush brush = Brushes.Black;

            float marginX = 50;
            float marginY = 50;
            float y = marginY;
            float lineSpacing = 25;

            string noAutorizacion = selectedRow.Cells["ID_Autorizacion"].Value?.ToString() ?? "N/A";
            string proveedor = selectedRow.Cells["Proveedor"].Value?.ToString() ?? "N/A";
            string concepto = selectedRow.Cells["Concepto"].Value?.ToString() ?? "N/A";
            string monto = selectedRow.Cells["ValorAutorizacion"].Value != null ? Convert.ToDecimal(selectedRow.Cells["ValorAutorizacion"].Value).ToString("N2") : "0.00";
            string tipoPago = selectedRow.Cells["TipoDePago"].Value?.ToString() ?? "N/A";
            string formaPago = selectedRow.Cells["FormaDePago"].Value?.ToString() ?? "N/A";
            string cuentaPago = selectedRow.Cells["CuentaDePago"].Value?.ToString() ?? "N/A";
            string fecha = selectedRow.Cells["Fecha"].Value != null ? Convert.ToDateTime(selectedRow.Cells["Fecha"].Value).ToShortDateString() : "N/A";

            e.Graphics.DrawString("ADMINSOFTPF - Software de Gestión de Condominios", fontPeque, brush, marginX + 430, y);

            y += lineSpacing;
            e.Graphics.DrawString($"{Utilidades.NombreProyectoSeleccionado}", fontBold, brush, marginX, y);
            y += lineSpacing;
            e.Graphics.DrawString($"{Utilidades.RncProyecto}", fontRegular, brush, marginX, y);
            y += lineSpacing;
            e.Graphics.DrawString($"Teléfono: {Utilidades.Oficina} - WhatsApp: {Utilidades.Telefono}", fontRegular, brush, marginX, y);
            y += lineSpacing * 2;
            
            string detallesPago = "DETALLES DEL PAGO";
            float detallesPagoWidth = e.Graphics.MeasureString(detallesPago, fontBold).Width;
            float centerX = (e.PageBounds.Width - detallesPagoWidth) / 2;
            e.Graphics.DrawString(detallesPago, fontBold, brush, centerX, y);

            y += lineSpacing * 2;
            e.Graphics.DrawString($"Solicitud No: {noAutorizacion}    -    Tipo de Pago: {tipoPago}", fontRegular, brush, marginX, y);
            
            y += lineSpacing;
            e.Graphics.DrawString($"Beneficiario: {proveedor}", fontRegular, brush, marginX, y);
            y += lineSpacing;
            e.Graphics.DrawString($"Valor de Pago: RD${monto}", fontRegular, brush, marginX, y);
            y += lineSpacing;
            
            // Modificación aquí para el salto de línea en Concepto
            string conceptoText = $"Concepto: {concepto}";
            SizeF conceptoSize = e.Graphics.MeasureString(conceptoText, fontRegular);
            if (conceptoSize.Width > e.PageBounds.Width - marginX * 2) // Verificar si el texto sobrepasa el límite
            {
                // Si el texto es muy largo, hacerlo en múltiples líneas
                string[] lines = WrapText(conceptoText, fontRegular, e.PageBounds.Width - marginX * 2);
                foreach (var line in lines)
                {
                    e.Graphics.DrawString(line, fontRegular, brush, marginX, y);
                    y += lineSpacing;
                }
            }
            else
            {
                e.Graphics.DrawString(conceptoText, fontRegular, brush, marginX, y);
                y += lineSpacing;
            }

            e.Graphics.DrawString($"Vía de Pago: {formaPago} - {cuentaPago}", fontRegular, brush, marginX, y);
            y += lineSpacing;
            e.Graphics.DrawString($"Fecha: {fecha}", fontRegular, brush, marginX, y);
            y += lineSpacing * 2;
            e.Graphics.DrawString("Aprobado por: __________________________", fontRegular, brush, marginX, y);
            y += lineSpacing * 3;
            e.Graphics.DrawString("Recibido por: __________________________", fontRegular, brush, marginX, y);
            y += lineSpacing * 2;
            e.Graphics.DrawString("* Este documento no es válido sin firma o sello correspondientes.", fontRegular, brush, marginX, y);

        }

        private void btnFiiltrar_Click(object sender, EventArgs e)
        {
            DateTime fechaInicio = dtpInicial.Value;
            DateTime fechaFin = dtpFinal.Value;
            string busqueda = txtBusqueda.Text.Trim();  // Obtener el valor de búsqueda

            if (fechaInicio > fechaFin)
            {
                MessageBox.Show("La fecha inicial no puede ser mayor que la fecha final.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Llamar al método de cargar autorizaciones con los parámetros de fecha y búsqueda
            CargarAutorizaciones(fechaInicio, fechaFin, busqueda);
        }

        private void CargarAutorizaciones(DateTime? fechaInicio = null, DateTime? fechaFin = null, string busqueda = "")
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT ID_Autorizacion, Proveedor, Concepto, ValorAutorizacion, TipoDePago, FormaDePago, CuentaDePago, Fecha 
                             FROM Autorizaciones 
                             WHERE Id_Proyecto = @IdProyecto";

                    // Si se proporcionan fechas, agregar la condición de fechas
                    if (fechaInicio.HasValue && fechaFin.HasValue)
                    {
                        query += " AND Fecha BETWEEN @FechaInicio AND @FechaFin";
                    }

                    // Si hay texto de búsqueda, agregar la condición de búsqueda
                    if (!string.IsNullOrEmpty(busqueda))
                    {
                        query += " AND (Proveedor LIKE @Busqueda OR Concepto LIKE @Busqueda)";
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdProyecto", Utilidades.IdProyectoSeleccionado);

                        if (fechaInicio.HasValue && fechaFin.HasValue)
                        {
                            command.Parameters.AddWithValue("@FechaInicio", fechaInicio.Value.Date);
                            command.Parameters.AddWithValue("@FechaFin", fechaFin.Value.Date);
                        }

                        if (!string.IsNullOrEmpty(busqueda))
                        {
                            command.Parameters.AddWithValue("@Busqueda", "%" + busqueda + "%");  // Usar % para hacer la búsqueda parcial
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvAutorizaciones.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar autorizaciones: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para dividir el texto en múltiples líneas si es necesario
        private string[] WrapText(string text, Font font, float maxWidth)
        {
            List<string> lines = new List<string>();
            string[] words = text.Split(' ');
            StringBuilder currentLine = new StringBuilder();

            foreach (string word in words)
            {
                string testLine = currentLine.Length == 0 ? word : currentLine + " " + word;
                SizeF size = TextRenderer.MeasureText(testLine, font);

                if (size.Width > maxWidth)
                {
                    lines.Add(currentLine.ToString());
                    currentLine.Clear();
                    currentLine.Append(word);
                }
                else
                {
                    currentLine.Append(word + " ");
                }
            }

            if (currentLine.Length > 0)
            {
                lines.Add(currentLine.ToString());
            }

            return lines.ToArray();
        }
    }
}