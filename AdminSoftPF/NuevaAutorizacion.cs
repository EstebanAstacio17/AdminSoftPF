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
    public partial class NuevaAutorizacion: Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        int IdProyecto = Utilidades.IdProyectoSeleccionado;

        int IdUsuario = Utilidades.IdUsuario;
        public NuevaAutorizacion()
        {
            InitializeComponent();
        }

        private void NuevaAutorizacion_Load(object sender, EventArgs e)
        {
            LlenarCbos();
            LlenarComboBoxBancoCuenta();
            LlenarComboBoxTipoDePago();
            LlenarComboBoxBeneficiario();
            NoEditarCboS();

            // Agregar evento para habilitar/deshabilitar cboCuentas
            cboFormaDePago.SelectedIndexChanged += CboFormaDePago_SelectedIndexChanged;
            CboFormaDePago_SelectedIndexChanged(null, null); // Validar estado inicial
        }

        private void CboFormaDePago_SelectedIndexChanged(object sender, EventArgs e)
        {
            string formaPagoSeleccionada = cboFormaDePago.SelectedItem?.ToString();
            bool esTransferenciaOCheque = (formaPagoSeleccionada == "Transferencia" || formaPagoSeleccionada == "Cheque");

            cboCuentas.Enabled = esTransferenciaOCheque;

            if (!esTransferenciaOCheque && cboCuentas.SelectedIndex != -1)
            {
                cboCuentas.SelectedIndex = -1;
            }
        }

        private void LlenarCbos()
        {
            // Opciones para el primer ComboBox
            var formasDePagos = new List<string>
            {
                "Efectivo",
                "Transferencia",
                "Cheque",
                "Otro"
            };
            cboFormaDePago.Items.AddRange(formasDePagos.ToArray());

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
                        cboCuentas.Items.Clear();

                        while (reader.Read())
                        {
                            string banco = reader["Banco"].ToString();
                            string cuenta = reader["Cuenta"].ToString();
                            string item = $"{banco} ({cuenta.Substring(cuenta.Length - 4)})"; // Últimos 4 dígitos de la cuenta
                            cboCuentas.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
        }

        private void LlenarComboBoxTipoDePago()
        {
            cboTipoSolicitud.Items.Clear(); 

            string query = "SELECT TipoDePago FROM TipoDePago WHERE ID_Proyecto = @ID_Proyecto AND Estado = 'Activo'";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Asegúrate de agregar los parámetros dentro del mismo contexto de la conexión
                        command.Parameters.AddWithValue("@ID_Proyecto", IdProyecto);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cboTipoSolicitud.Items.Add(reader["TipoDePago"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al llenar el ComboBox: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LlenarComboBoxBeneficiario()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Identificacion, Proveedor, ID_Proveedor FROM Proveedor WHERE ID_Proyecto = @ID_Proyecto AND Estado = 'Activo'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID_Proyecto", IdProyecto);

                        SqlDataReader reader = cmd.ExecuteReader();
                        cboBeneficiario.Items.Clear();

                        while (reader.Read())
                        {
                            string identificacion = reader["Identificacion"].ToString();
                            string proveedor = reader["Proveedor"].ToString();
                            string item = $"{proveedor} ({identificacion})";
                            cboBeneficiario.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
        }

        private void NoEditarCboS()
        {
            cboBeneficiario.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoSolicitud.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFormaDePago.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCuentas.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {

            try
            {
                if (ValidarCampos())
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = @"INSERT INTO Autorizaciones (Fecha, Proveedor, ValorAutorizacion, TipoDePago, Concepto, FormaDePago, CuentaDePago, ID_Proyecto, ID_Usuario)
                                         VALUES (@Fecha, @Proveedor, @ValorAutorizacion, @TipoDePago, @Concepto, @FormaDePago, @CuentaDePago, @ID_Proyecto, @ID_Usuario)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Fecha", dtpFechaSolicitud.Value);
                            cmd.Parameters.AddWithValue("@Proveedor", cboBeneficiario.SelectedItem);
                            cmd.Parameters.AddWithValue("@ValorAutorizacion", txtValorSolicitud.Text);
                            cmd.Parameters.AddWithValue("@TipoDePago", cboTipoSolicitud.SelectedItem);
                            cmd.Parameters.AddWithValue("@Concepto", ReducirEspacios(rtbConcepto.Text));
                            cmd.Parameters.AddWithValue("@FormaDePago", cboFormaDePago.SelectedItem);
                            cmd.Parameters.AddWithValue("@CuentaDePago", cboCuentas.Enabled ? cboCuentas.SelectedItem : (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@ID_Proyecto", IdProyecto);
                            cmd.Parameters.AddWithValue("@ID_Usuario", IdUsuario);

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Autorización registrada correctamente.");
                            LimpiarCampos();
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar la autorización: " + ex.Message);
            }
        }


        private bool ValidarCampos()
        {
            StringBuilder mensajeError = new StringBuilder();

            if (cboBeneficiario.SelectedItem == null) mensajeError.AppendLine("- Beneficiario");
            if (string.IsNullOrWhiteSpace(txtValorSolicitud.Text)) mensajeError.AppendLine("- Valor");
            if (cboTipoSolicitud.SelectedItem == null) mensajeError.AppendLine("- Tipo de solicitud");
            if (string.IsNullOrWhiteSpace(rtbConcepto.Text)) mensajeError.AppendLine("- Concepto");
            if (cboFormaDePago.SelectedItem == null) mensajeError.AppendLine("- Forma de pago");
            if (cboCuentas.Enabled && cboCuentas.SelectedItem == null) mensajeError.AppendLine("- Cuenta de pago (requerido para Transferencia o Cheque)");

            if (mensajeError.Length > 0)
            {
                MessageBox.Show("Los siguientes campos están vacíos o no seleccionados:\n" + mensajeError.ToString(), "Campos Requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private string ReducirEspacios(string sinEspacio)
        {
            if (string.IsNullOrWhiteSpace(sinEspacio)) return string.Empty;
            return System.Text.RegularExpressions.Regex.Replace(sinEspacio.Trim(), @"\s+", " ");
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                "¿Estás seguro de que deseas cancelar?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                LimpiarCampos();
                this.Close();
            }
        }

        private void LimpiarCampos()
        {
            cboBeneficiario.SelectedIndex = -1;
            txtValorSolicitud.Clear();
            cboTipoSolicitud.SelectedIndex = -1;
            rtbConcepto.Clear();
            cboFormaDePago.SelectedIndex = -1;
            cboCuentas.SelectedIndex = -1;
        }
    }
}