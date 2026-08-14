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

namespace AdminSoftPF
{
    public partial class Credito : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        int IdProyecto = Utilidades.IdProyectoSeleccionado;

        string CondominoActual = Utilidades.DireccionCompleta;

        private Detalle formularioDetalle; // Instancia del formulario Detalle
        public Credito(Detalle detalle)
        {
            InitializeComponent();

            formularioDetalle = detalle; // Asignar la instancia del formulario Detalle
        }

        private void Credito_Load(object sender, EventArgs e)
        {
            CamposNoEditables();

            // Establecer la fecha y hora actual en el DateTimePicker
            EstablecerFechaHoraActual();
        }

        private void LimitarTextoDetalle(object sender, EventArgs e)
        {
            if (rtbDetalle.Text.Length > 50)
            {
                // Truncar el texto a 50 caracteres
                rtbDetalle.Text = rtbDetalle.Text.Substring(0, 50);
                rtbDetalle.SelectionStart = rtbDetalle.Text.Length; // Mantener el cursor al final

                // Mostrar un mensaje al usuario (opcional)
                MessageBox.Show("El detalle no puede exceder los 50 caracteres.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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


        private void EstablecerFechaHoraActual()
        {
            // Establecer la fecha y hora actual en el DateTimePicker
            DateTime fechaHoraActual = DateTime.Now;
            dtpCredito.Value = fechaHoraActual;

            // Dar formato al DateTimePicker
            dtpCredito.Format = DateTimePickerFormat.Custom;
            dtpCredito.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Formato personalizado (puedes cambiarlo según necesites)
        }

        private void CamposNoEditables()
        {
            // Configurar restricciones para el DateTimePicker
            dtpCredito.Enabled = false;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Preguntar al usuario si está seguro de limpiar los campos
            DialogResult resultado = MessageBox.Show("¿Está seguro de que desea Cancelar el Credito?",
                                                     "Confirmar",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Question);

            // Si el usuario selecciona "Sí", limpiar los campos
            if (resultado == DialogResult.Yes)
            {
                LimpiarPago();
                this.Close();
            }
        }

        private void LimpiarPago()
        {
            // Limpiar TextBox
            txtVelor.Clear();

            // Limpiar RichTextBox
            rtbDetalle.Clear();
        }

        private void btnAcreditar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que los campos no estén vacíos antes de aplicar el pago
                if (string.IsNullOrWhiteSpace(txtVelor.Text))
                {
                    MessageBox.Show("El campo 'Valor' no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Salir sin ejecutar el resto del código
                }

                if (string.IsNullOrWhiteSpace(rtbDetalle.Text))
                {
                    MessageBox.Show("El campo 'Detalle' no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Salir sin ejecutar el resto del código
                }

                // Aplicar el crédito si las validaciones son exitosas
                AplicarCredito();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado al aplicar el crédito: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void AplicarCredito()
        {
            // Validar entrada de valor
            if (!decimal.TryParse(txtVelor.Text, out decimal valorPago) || valorPago <= 0)
            {
                MessageBox.Show("Por favor, ingrese un valor de pago válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Iniciar conexión y transacción
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Insertar recibo
                        int idRecibo = InsertarRecibo(conn, transaction);

                        // Actualizar deuda
                        ActualizarDeuda(conn, transaction, valorPago);

                        // Registrar en el historial
                        RegistrarHistorial(conn, transaction, idRecibo, valorPago);

                        // Confirmar transacción
                        transaction.Commit();

                        MessageBox.Show("Crédito aplicado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Llamar al método CargarRecibos() del formulario Detalle
                        formularioDetalle?.CargarRecibos();

                        // Limpiar campos
                        LimpiarPago();

                        // Cerrar el formulario actual
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        // Revertir transacción en caso de error
                        transaction.Rollback();
                        MessageBox.Show($"Error al aplicar el crédito: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private int InsertarRecibo(SqlConnection conn, SqlTransaction transaction)
        {
            string query = @"INSERT INTO Recibo 
                             (ID_Direccion, Direccion, FormaDePago, ValorPago, TipoPago, DetallePago, Usuario, FechaPago)
                             VALUES 
                             (@ID_Direccion, @Direccion, @FormaDePago, @ValorPago, @TipoPago, @DetallePago, @Usuario, @FechaPago); 
                             SELECT SCOPE_IDENTITY();";

            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@ID_Direccion", Utilidades.ID_Direccion);
                cmd.Parameters.AddWithValue("@Direccion",  $"{Utilidades.IdProyectoSeleccionado}-{Utilidades.DireccionCompleta}");
                cmd.Parameters.AddWithValue("@FormaDePAgo", "Credito");
                cmd.Parameters.AddWithValue("@ValorPago", txtVelor.Text);
                cmd.Parameters.AddWithValue("@TipoPago", "Credito");
                cmd.Parameters.AddWithValue("@DetallePago", rtbDetalle.Text);
                cmd.Parameters.AddWithValue("@Usuario", Utilidades.Usuario);
                cmd.Parameters.AddWithValue("@FechaPago", DateTime.Now);

                return Convert.ToInt32(cmd.ExecuteScalar());
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
                cmd.Parameters.AddWithValue("@Direccion", $"{Utilidades.IdProyectoSeleccionado}-{Utilidades.DireccionCompleta}");
                cmd.Parameters.AddWithValue("@Usuario", Utilidades.Usuario);
                cmd.Parameters.AddWithValue("@TipoPago", "Credito");
                cmd.Parameters.AddWithValue("@Pago", valorPago);
                cmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
        }

        private void rtbDetalle_KeyDown(object sender, KeyEventArgs e)
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
    }
}
