using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace AdminSoftPF
{
    public partial class Pago : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        int IdProyecto = Utilidades.IdProyectoSeleccionado;

        string CondominoActual = Utilidades.DireccionCompleta;

        private Detalle formularioDetalle; // Instancia del formulario Detalle

        public string ProyectoDireccion { get; private set; }

        // Variables de clase para almacenar los datos del pago
        private decimal valorEfectivo;
        private decimal valorTarjeta;
        private decimal valorTransferencia;
        private decimal valorTotal;
        private string formaDePago;
        private string detalleBanco;
        private string comentario;
        private DateTime fechaPago;

        // Variables de clase para almacenar los datos del recibo
        private int idRecibo;
        private string direccionRecibo;
        private string formaDePagoRecibo;
        private decimal valorPagoRecibo;
        private string detallePagoRecibo;
        private string detalleBancoRecibo;
        private string usuarioRecibo;
        private DateTime fechaPagoRecibo;

        public Pago(Detalle detalle)
        {
            InitializeComponent();

            CamposNoEditables();

            formularioDetalle = detalle; // Asignar la instancia del formulario Detalle
        }

        private void Pago_Load(object sender, EventArgs e)
        {
            ObtenerNombreProyecto();

            ObtenerDireccionCondominio();

            GenerarProyectoDireccion();

            LlenarComboBoxBancoCuenta();

            EstablecerFechaHoraActual();

            txtTarjeta.TextChanged += txtTarjeta_TextChanged;
            txtTransferencia.TextChanged += txtTransferencia_TextChanged;

        }

        private void ValidarSoloNumeros(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                // Permitir solo números (0-9), Backspace, Delete, flechas, y punto decimal
                if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != '.' &&
                    e.KeyChar != (char)Keys.Delete && e.KeyChar != (char)Keys.Left && e.KeyChar != (char)Keys.Right)
                {
                    e.Handled = true; // Bloquear la tecla
                    return;
                }

                // Manejar la presencia de un punto decimal
                string text = textBox.Text;

                if (e.KeyChar == '.')
                {
                    // Permitir solo un punto decimal
                    if (text.Contains("."))
                    {
                        e.Handled = true;
                        return;
                    }

                    // No permitir punto como primer carácter
                    if (string.IsNullOrEmpty(text))
                    {
                        e.Handled = true;
                        return;
                    }

                    return;
                }

                // Dividir el texto en parte entera y parte decimal
                string[] parts = text.Split('.');

                // Validar parte entera (máximo 6 dígitos)
                if (parts[0].Length >= 6 && textBox.SelectionStart <= parts[0].Length)
                {
                    // Permitir acciones como mover el cursor, pero no permitir más dígitos
                    if (char.IsDigit(e.KeyChar))
                    {
                        e.Handled = true;
                    }
                    return;
                }

                // Validar parte decimal (máximo 2 dígitos después del punto)
                if (parts.Length > 1 && textBox.SelectionStart > text.IndexOf('.') && parts[1].Length >= 2)
                {
                    // Permitir acciones como mover el cursor, pero no permitir más dígitos
                    if (char.IsDigit(e.KeyChar))
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void LimpiarPago()
        {
            // Limpiar TextBox
            txtEfectivo.Clear();
            txtTarjeta.Clear();
            txtTransferencia.Clear();

            // Limpiar RichTextBox
            rtbComentario.Clear();

            // Limpiar ComboBox
            cboTarjeta.SelectedIndex = -1;
            cboBancoCuenta.SelectedIndex = -1;

            // Limpiar Labels (Si es necesario, puedes dejar un texto específico o vacío)
            txtNombreProyecto.Clear();
            txtDireccionCondomino.Clear();
        }

        private void ActualizarTotal_TextChanged(object sender, EventArgs e)
        {
            //decimal total = 0;
            decimal.TryParse(txtEfectivo.Text, out valorEfectivo);
            decimal.TryParse(txtTarjeta.Text, out valorTarjeta);
            decimal.TryParse(txtTransferencia.Text, out valorTransferencia);

            valorTotal = valorEfectivo + valorTarjeta + valorTransferencia;
            txtTotal.Text = valorTotal.ToString("0.00"); // Mostrar el total con dos decimales
        }

        private void EstablecerFechaHoraActual()
        {
            // Establecer la fecha y hora actual en el DateTimePicker
            fechaPago = DateTime.Now;
            dtpFecha.Value = fechaPago;

            // Dar formato al DateTimePicker
            dtpFecha.Format = DateTimePickerFormat.Custom;
            dtpFecha.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Formato personalizado (puedes cambiarlo según necesites)
        }

        private void LlenarComboBoxBancoCuenta()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Banco, Cuenta FROM Cuenta WHERE ID_Proyecto = @ID_Proyecto AND Estado = 'Activo'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID_Proyecto", IdProyecto);

                        SqlDataReader reader = cmd.ExecuteReader();
                        cboBancoCuenta.Items.Clear();

                        while (reader.Read())
                        {
                            string banco = reader["Banco"].ToString();
                            string cuenta = reader["Cuenta"].ToString();
                            string item = $"{banco} ({cuenta.Substring(cuenta.Length - 4)})"; // Últimos 4 dígitos de la cuenta
                            cboBancoCuenta.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
        }

        private void CamposNoEditables()
        {
            // Configurar restricciones para los ComboBox
            cboTarjeta.DropDownStyle = ComboBoxStyle.DropDownList;
            cboBancoCuenta.DropDownStyle = ComboBoxStyle.DropDownList;

            // Configurar restricciones para el TextBox
            txtTotal.ReadOnly = true;
            txtTotal.TabStop = false; // Evita que el foco se mueva al TextBox

            //Inhabilita los Cbo al inicio
            cboTarjeta.Enabled = false;
            cboBancoCuenta.Enabled = false;

            txtNombreProyecto.ReadOnly = true;
            txtNombreProyecto.TabStop = false;
            txtDireccionCondomino.ReadOnly = true;
            txtDireccionCondomino.TabStop = false;

            // Hacer el TextBox de tarjeta no editable
            txtTarjeta.ReadOnly = true;

            // Configurar restricciones para el DateTimePicker
            dtpFecha.Enabled = false;
        }

        private void GenerarProyectoDireccion()
        {
            if (!string.IsNullOrEmpty(CondominoActual))
            {
                ProyectoDireccion = $"{IdProyecto}-{CondominoActual}";
            }
            else
            {
                ProyectoDireccion = $"{IdProyecto}-Sin Dirección";
            }
        }

        private void ObtenerDireccionCondominio()
        {
            txtDireccionCondomino.Text = CondominoActual;
        }

        private void ObtenerNombreProyecto()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Consulta SQL para obtener el nombre del proyecto por su ID
                    string query = "SELECT NombreProyecto FROM Proyecto WHERE ID_Proyecto = @ID_Proyecto";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Asignar el valor del ID_Proyecto como parámetro
                        cmd.Parameters.AddWithValue("@ID_Proyecto", IdProyecto);

                        // Ejecutar la consulta y leer el resultado
                        var result = cmd.ExecuteScalar();

                        // Verificar si se obtuvo el valor
                        if (result != null)
                        {
                            txtNombreProyecto.Text = result.ToString(); // Asignar el nombre del proyecto al label
                        }
                        else
                        {
                            txtNombreProyecto.Text = "No se encontró el proyecto"; // Si no se encuentra el proyecto
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el nombre del proyecto: " + ex.Message);
            }
        }

        private void btnCancelarPago_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("¿Está seguro de que desea Cancelar el Pago?",
                                                     "Confirmar",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                LimpiarPago();
                this.Close();
            }
        }

        private void txtTarjeta_Click(object sender, EventArgs e)
        {
            using (PagoConTarjeta datosTarjeta = new PagoConTarjeta())
            {
                if (datosTarjeta.ShowDialog() == DialogResult.OK)
                {
                    // Obtener el valor del Total desde el formulario secundario
                    txtTarjeta.Text = datosTarjeta.TotalTarjeta;
                }
            }
        }

        private bool ValidarFormulario()
        {
            // Validar que al menos un campo de pago tenga un valor
            if (string.IsNullOrWhiteSpace(txtEfectivo.Text) &&
                string.IsNullOrWhiteSpace(txtTarjeta.Text) &&
                string.IsNullOrWhiteSpace(txtTransferencia.Text))
            {
                MessageBox.Show("Debe ingresar al menos un valor en los campos de pago.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validar selección de combo boxes cuando corresponda
            if (!string.IsNullOrWhiteSpace(txtTarjeta.Text) && cboTarjeta.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar una tarjeta válida.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtTransferencia.Text) && cboBancoCuenta.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un banco válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validar que txtTotal sea un valor decimal válido
            if (!decimal.TryParse(txtTotal.Text, out decimal total) || total <= 0)
            {
                MessageBox.Show("El valor total del pago no es válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 🔴 VALIDACIÓN OBLIGATORIA DE COMENTARIO
            if (string.IsNullOrWhiteSpace(rtbComentario.Text))
            {
                MessageBox.Show("Debe ingresar un comentario antes de aplicar el pago.",
                                "Validación",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                rtbComentario.Focus();
                return false;
            }

            return true;
        }

        private void btnAplicarPago_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarFormulario())
                {
                    return;
                }

                AplicarPagoRecibo();

                // Preguntar si desea imprimir el recibo
                DialogResult result = MessageBox.Show("¿Desea imprimir el recibo?", "Confirmar Impresión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Llamar a la función de impresión después de aplicar el pago
                    ImprimirRecibo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado al aplicar el pago: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public void AplicarPagoRecibo()
        {
            try
            {
                valorEfectivo = string.IsNullOrWhiteSpace(txtEfectivo.Text) ? 0 : Convert.ToDecimal(txtEfectivo.Text);
                valorTarjeta = string.IsNullOrWhiteSpace(txtTarjeta.Text) ? 0 : Convert.ToDecimal(txtTarjeta.Text);
                valorTransferencia = string.IsNullOrWhiteSpace(txtTransferencia.Text) ? 0 : Convert.ToDecimal(txtTransferencia.Text);
                valorTotal = valorEfectivo + valorTarjeta + valorTransferencia;

                formaDePago = "";
                if (valorEfectivo > 0) formaDePago = "Efectivo";
                if (valorTarjeta > 0) formaDePago += (string.IsNullOrEmpty(formaDePago) ? "" : ", ") + "Tarjeta";
                if (valorTransferencia > 0) formaDePago += (string.IsNullOrEmpty(formaDePago) ? "" : ", ") + "Transferencia";

                detalleBanco = cboBancoCuenta.SelectedItem?.ToString() ?? "";
                comentario = rtbComentario.Text;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Validar existencia de ID_Direccion
                            if (!DireccionExiste(conn, transaction, Utilidades.ID_Direccion))
                            {
                                MessageBox.Show("La dirección asociada no existe. No se puede aplicar el pago.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // Insertar recibo
                            int idRecibo = InsertarRecibo(conn, transaction, valorTotal, formaDePago, detalleBanco);

                            // Actualizar deuda en la tabla Dirección
                            ActualizarDeuda(conn, transaction, valorTotal);

                            // Registrar historial
                            RegistrarHistorial(conn, transaction, idRecibo, valorTotal);

                            transaction.Commit();
                            MessageBox.Show("Pago aplicado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Llamar al método CargarRecibos() del formulario Detalle
                            formularioDetalle?.CargarRecibos();

                            LimpiarPago();

                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Error al procesar el pago: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ReducirEspacios(string sinEspacio)
        {
            if (string.IsNullOrWhiteSpace(sinEspacio)) return string.Empty;
            return System.Text.RegularExpressions.Regex.Replace(sinEspacio.Trim(), @"\s+", " ");
        }

        private bool DireccionExiste(SqlConnection conn, SqlTransaction transaction, int idDireccion)
        {
            string query = "SELECT COUNT(1) FROM Direccion WHERE ID_Direccion = @ID_Direccion";
            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@ID_Direccion", idDireccion);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public int InsertarRecibo(SqlConnection conn, SqlTransaction transaction, decimal valorPago, string formaDePago, string detalleBanco)
        {
            string query = @"INSERT INTO Recibo 
                             (ID_Direccion, Direccion, FormaDePago, ValorPago, TipoPago, DetallePago, DetalleBanco, Usuario, FechaPago)
                             VALUES 
                             (@ID_Direccion, @Direccion, @FormaDePago, @ValorPago, @TipoPago, @DetallePago, @DetalleBanco, @Usuario, @FechaPago); 
                             SELECT SCOPE_IDENTITY();";

            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@ID_Direccion", Utilidades.ID_Direccion);
                cmd.Parameters.AddWithValue("@Direccion", ProyectoDireccion);
                cmd.Parameters.AddWithValue("@FormaDePago", formaDePago);
                cmd.Parameters.AddWithValue("@ValorPago", valorPago);
                cmd.Parameters.AddWithValue("@TipoPago", "Recibo");
                cmd.Parameters.AddWithValue("@DetallePago", ReducirEspacios(comentario));
                cmd.Parameters.AddWithValue("@DetalleBanco", detalleBanco);
                cmd.Parameters.AddWithValue("@Usuario", Utilidades.Usuario);
                cmd.Parameters.AddWithValue("@FechaPago", fechaPago);

                // Obtener el ID_Recibo generado
                idRecibo = Convert.ToInt32(cmd.ExecuteScalar());

                // Almacenar la información del recibo en las variables de clase
                direccionRecibo = ProyectoDireccion;
                formaDePagoRecibo = formaDePago;
                valorPagoRecibo = valorPago;
                detallePagoRecibo = ReducirEspacios(comentario);
                detalleBancoRecibo = detalleBanco;
                usuarioRecibo = Utilidades.Usuario;
                fechaPagoRecibo = fechaPago;

                return idRecibo;
            }
        }

        private void ActualizarDeuda(SqlConnection conn, SqlTransaction transaction, decimal valorPago)
        {
            string query = "UPDATE Direccion SET Deuda = Deuda - @ValorPago WHERE ID_Direccion = @ID_Direccion";
            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@ValorPago", valorPago);
                cmd.Parameters.AddWithValue("@ID_Direccion", Utilidades.ID_Direccion);
                cmd.ExecuteNonQuery();
            }
        }

        private void RegistrarHistorial(SqlConnection conn, SqlTransaction transaction, int idRecibo, decimal valorPago)
        {
            string query = @"INSERT INTO Historial 
                             (ID_Recibo, ID_Usuario, Direccion, Usuario, Tipo, Pago, FechaRegistro)
                             VALUES 
                             (@ID_Recibo, @ID_Usuario, @Direccion, @Usuario, @TipoPago, @Pago, @FechaRegistro);";

            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@ID_Recibo", idRecibo);
                cmd.Parameters.AddWithValue("@ID_Usuario", Utilidades.IdUsuario);
                cmd.Parameters.AddWithValue("@Direccion", ProyectoDireccion);
                cmd.Parameters.AddWithValue("@Usuario", Utilidades.Usuario);
                cmd.Parameters.AddWithValue("@TipoPago", "Recibo");
                cmd.Parameters.AddWithValue("@Pago", valorPago);
                cmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
        }
        
        private void ImprimirRecibo()
        {
            try
            {
                // Obtener la información del recibo
                int idRecibo = ObtenerIdRecibo();
                string direccion = ObtenerDireccionRecibo();
                string formaDePago = ObtenerFormaDePagoRecibo();
                decimal valorPago = ObtenerValorPagoRecibo();
                string detallePago = ObtenerDetallePagoRecibo();
                string detalleBanco = ObtenerDetalleBancoRecibo();
                string usuario = ObtenerUsuarioRecibo();
                DateTime fechaPago = ObtenerFechaPagoRecibo();

                // Crear un objeto PrintDocument
                PrintDocument printDoc = new PrintDocument();

                // Configurar la impresora (BIXOLON SRP-350III)
                printDoc.PrinterSettings.PrinterName = "BIXOLON SRP-275III";

                // Verificar si la impresora está configurada correctamente
                if (!printDoc.PrinterSettings.IsValid)
                {
                    MessageBox.Show("No se encontró la impresora BIXOLON SRP-275III. Verifique la configuración.", "Error de impresión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Asignar el evento PrintPage para manejar la impresión
                printDoc.PrintPage += new PrintPageEventHandler((sender, e) => PrintRecibo(sender, e, idRecibo, direccion, formaDePago, valorPago, detallePago, detalleBanco, usuario, fechaPago));

                // Iniciar la impresión
                printDoc.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al imprimir el recibo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintRecibo(object sender, PrintPageEventArgs e, int idRecibo, string direccion, string formaDePago, decimal valorPago, string detallePago, string detalleBanco, string usuario, DateTime fechaPago)
        {
            Graphics g = e.Graphics;
            Font fontTitulo = new Font("Arial", 12, FontStyle.Bold);
            Font fontContenido = new Font("Arial", 10);
            Font fontImportante = new Font("Arial", 10, FontStyle.Bold);

            float yPos = 10;
            int leftMargin = 4;
            int pageWidth = (int)(70 / 25.4 * 100); // Ancho del recibo en píxeles (70mm)

            // Función para centrar con desplazamiento a la izquierda
            void DrawLeftShiftedString(string text, Font font, Brush brush, float yPosition, float shift = 10)
            {
                SizeF textSize = g.MeasureString(text, font);
                float xPosition = (pageWidth - textSize.Width) / 2 - shift;
                if (xPosition < leftMargin) xPosition = leftMargin;
                g.DrawString(text, font, brush, xPosition, yPosition);
            }

            // Función para dibujar texto con ajuste de línea automático
            float DrawWrappedString(string text, Font font, Brush brush, float x, float y, float maxWidth)
            {
                RectangleF rect = new RectangleF(x, y, maxWidth, 1000); // 1000 es una altura arbitrariamente grande
                g.DrawString(text, font, brush, rect);
                return g.MeasureString(text, font, new SizeF(maxWidth, 1000)).Height; // Devuelve la altura ocupada
            }

            // Encabezado - Desplazado hacia la izquierda
            DrawLeftShiftedString("Condominio Al Día, CONDAY", fontTitulo, Brushes.Black, yPos, 20);
            yPos += 20;
            DrawLeftShiftedString($"{Utilidades.RncProyecto}", fontContenido, Brushes.Black, yPos, 20);
            yPos += 20;
            DrawLeftShiftedString($"{Utilidades.Telefono}", fontContenido, Brushes.Black, yPos, 20);
            yPos += 30;

            DrawLeftShiftedString("* * * Pago de Mantenimiento * * *", fontImportante, Brushes.Black, yPos);
            yPos += 30;

            // Datos del formulario
            string fecha = fechaPago.ToString("yyyy-MM-dd HH:mm:ss");
            string nombreProyecto = Utilidades.NombreProyectoSeleccionado;
            string direccionCondomino = Utilidades.DireccionCompleta;
            string efectivo = txtEfectivo.Text;
            string tarjeta = txtTarjeta.Text;
            string transferencia = txtTransferencia.Text;
            string total = txtTotal.Text;
            string comentario = rtbComentario.Text;

            string formaDePagoStr = "";
            if (!string.IsNullOrEmpty(efectivo) && efectivo != "0.00") formaDePagoStr += "Efectivo, ";
            if (!string.IsNullOrEmpty(tarjeta)) formaDePagoStr += "Tarjeta, ";
            if (!string.IsNullOrEmpty(transferencia)) formaDePagoStr += "Transferencia, ";
            formaDePagoStr = formaDePagoStr.TrimEnd(',', ' ');

            // Contenido alineado a la izquierda
            g.DrawString($"{nombreProyecto}", fontContenido, Brushes.Black, leftMargin, yPos); yPos += 20;
            g.DrawString($"{formularioDetalle.NombreCompletoCliente}", fontContenido, Brushes.Black, leftMargin, yPos); yPos += 20;
            g.DrawString($"{formularioDetalle.documentoClienteFactura}", fontContenido, Brushes.Black, leftMargin, yPos); yPos += 20;
            g.DrawString($"{direccionCondomino}", fontContenido, Brushes.Black, leftMargin, yPos); yPos += 30;

            // Detalle del pago
            DrawLeftShiftedString("* * * Detalle de Pago * * *", fontImportante, Brushes.Black, yPos);
            yPos += 30;
            g.DrawString($"Fecha: {fecha}", fontContenido, Brushes.Black, leftMargin, yPos); yPos += 20;

            // Comentario con ajuste de línea automático
            float alturaComentario = DrawWrappedString($"Comentario: {detallePagoRecibo}", fontContenido, Brushes.Black, leftMargin, yPos, pageWidth - leftMargin * 2);
            yPos += alturaComentario + 10; // Espacio adicional después del comentario

            //g.DrawString($"Total: {valorPagoRecibo}", fontContenido, Brushes.Black, leftMargin, yPos); yPos += 20;
            var culturaRD = new CultureInfo("es-DO");
            g.DrawString($"Total: RD$ {valorPagoRecibo.ToString("N2", culturaRD)}", fontContenido, Brushes.Black, leftMargin, yPos);
            yPos += 15;
            // Forma de pago y detalle del banco con ajuste de línea automático
            string formaPagoBanco = $"{formaDePagoRecibo} {detalleBancoRecibo}";
            float alturaFormaPago = DrawWrappedString(formaPagoBanco, fontContenido, Brushes.Black, leftMargin, yPos, pageWidth - leftMargin * 2);
            yPos += alturaFormaPago + 10; // Espacio adicional después de la forma de pago

            // Mensaje importante
            DrawLeftShiftedString("* * * IMPORTANTE! * * *", fontImportante, Brushes.Black, yPos);
            yPos += 20;
            g.DrawString("Recibo válido si está sellado.", fontContenido, Brushes.Black, leftMargin, yPos);
            yPos += 30;

            // Datos finales
            g.DrawString($"Recibo No: {idRecibo}", fontContenido, Brushes.Black, leftMargin, yPos); yPos += 20;
            g.DrawString($"Usuario: {usuario}", fontContenido, Brushes.Black, leftMargin, yPos); yPos += 30;

            // Fin de recibo
            DrawLeftShiftedString("* * * * Fin de Recibo * * * *", fontContenido, Brushes.Black, yPos);
            yPos += 50;
        }

        private void rtbComentario_KeyDown(object sender, KeyEventArgs e)
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

        public int ObtenerIdRecibo()
        {
            return idRecibo;
        }

        public string ObtenerDireccionRecibo()
        {
            return direccionRecibo;
        }

        public string ObtenerFormaDePagoRecibo()
        {
            return formaDePagoRecibo;
        }

        public decimal ObtenerValorPagoRecibo()
        {
            return valorPagoRecibo;
        }

        public string ObtenerDetallePagoRecibo()
        {
            return detallePagoRecibo;
        }

        public string ObtenerDetalleBancoRecibo()
        {
            return detalleBancoRecibo;
        }

        public string ObtenerUsuarioRecibo()
        {
            return usuarioRecibo;
        }

        public DateTime ObtenerFechaPagoRecibo()
        {
            return fechaPagoRecibo;
        }

        private void txtTarjeta_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTarjeta.Text))
            {
                cboTarjeta.Enabled = true;
            }
            else
            {
                cboTarjeta.SelectedIndex = -1;
                cboTarjeta.Enabled = false;
            }
        }

        private void txtTransferencia_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTransferencia.Text))
            {
                cboBancoCuenta.Enabled = true;
            }
            else
            {
                cboBancoCuenta.SelectedIndex = -1;
                cboBancoCuenta.Enabled = false;
            }
        }

    }
}