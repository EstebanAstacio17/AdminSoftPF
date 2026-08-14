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
    public partial class FacturacionU : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        public FacturacionU()
        {
            InitializeComponent();

            // Configurar el ComboBox al inicializar el formulario
            ConfigurarOpcinesDeCuota();

            // Deshabilitar la selección automática al cargar los datos
            dgvFactUnica.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFactUnica.ClearSelection();  // Desmarca cualquier fila seleccionada

        }

        private void txtDireccion_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Evitar que se ingresen espacios
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true; // Cancela el evento de tecla
                MessageBox.Show("No se permiten espacios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Limitar la longitud del texto a 15 caracteres
            if (txtDireccion.Text.Length >= 20 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Cancela el evento de tecla
                MessageBox.Show("Máximo 15 caracteres permitidos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtDetalle_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Limitar la longitud del texto a 30 caracteres
            if (txtDetalleFactExtra.Text.Length >= 30 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Cancela el evento de tecla
                MessageBox.Show("Máximo 30 caracteres permitidos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtValorExtra_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Evitar que se ingresen espacios
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true; // Cancela el evento de tecla
                MessageBox.Show("No se permiten espacios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Permitir solo números
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Cancela el evento de tecla
                MessageBox.Show("Solo se permiten números.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Limitar la longitud del texto a 5 caracteres
            if (txtValorExtra.Text.Length >= 5 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Cancela el evento de tecla
                MessageBox.Show("Solo se permiten hasta 5 números.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ConfigurarOpcinesDeCuota()
        {
            // Agregar opciones al ComboBox
            cboTipoCuota.Items.Add("Cuota Extra");
            cboTipoCuota.Items.Add("Cuota Regular");

            // Establecer la primera opción como seleccionada por defecto
            cboTipoCuota.SelectedIndex = -1;

            // Hacer que el ComboBox sea de solo lectura
            cboTipoCuota.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void btnSeleccionarDireccion_Click(object sender, EventArgs e)
        {
            // Validar que el TextBox no esté vacío
            if (string.IsNullOrWhiteSpace(txtDireccion.Text))
            {
                MessageBox.Show("Se requiere una dirección para buscar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Llamar a la función BuscarDireccion si la validación es exitosa
            BuscarDireccion();
        }

        private void btnCrearfactura_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCreacionFactura())
                {
                    if (cboTipoCuota.SelectedItem?.ToString() == "Cuota Extra")
                    {
                        CrearFacturaCuotaExtra();
                        MessageBox.Show("La factura se ha creado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCampos();
                    }
                    else if (cboTipoCuota.SelectedItem?.ToString() == "Cuota Regular")
                    {
                        CrearFacturaCuotaRegular();
                        MessageBox.Show("La factura regular se ha creado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show("Seleccione un tipo de cuota válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al crear la factura: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void CrearFacturaCuotaExtra()
        {
            if (dgvFactUnica.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(lblDireccionSeleccionada.Text))
                throw new Exception("Debe seleccionar una dirección válida.");

            if (string.IsNullOrWhiteSpace(txtValorExtra.Text) || string.IsNullOrWhiteSpace(txtDetalleFactExtra.Text))
                throw new Exception("Debe completar todos los campos requeridos para la cuota extra.");

            string direccionSeleccionada = dgvFactUnica.SelectedRows[0].Cells["DireccionCompleta"].Value.ToString().Trim();
            string proyectoNombre = Utilidades.NombreProyectoSeleccionado;
            if (!int.TryParse(txtValorExtra.Text, out int valorExtra))
                throw new Exception("El valor extra debe ser un número válido.");

            string detalleFactura = txtDetalleFactExtra.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insertar en la tabla Factura
                        int idFactura;
                        using (SqlCommand cmdFactura = new SqlCommand(@"INSERT INTO Factura (Direccion, ValorCuota, TipoCuota, DetalleFactura, ID_Direccion) 
                                                              OUTPUT INSERTED.ID_Factura 
                                                              VALUES (@Direccion, @ValorCuota, @TipoCuota, @DetalleFactura, 
                                                              (SELECT ID_Direccion FROM Direccion WHERE DireccionCompleta = @Direccion AND ProyectoNombre = @ProyectoNombre))", connection, transaction))
                        {
                            cmdFactura.Parameters.AddWithValue("@Direccion", direccionSeleccionada);
                            cmdFactura.Parameters.AddWithValue("@ValorCuota", valorExtra);
                            cmdFactura.Parameters.AddWithValue("@TipoCuota", "Cuota Extra");
                            cmdFactura.Parameters.AddWithValue("@DetalleFactura", detalleFactura);
                            cmdFactura.Parameters.AddWithValue("@ProyectoNombre", proyectoNombre);

                            idFactura = (int)cmdFactura.ExecuteScalar();
                        }

                        // Verificar si la dirección existe antes de actualizar
                        int rowsAffected;
                        using (SqlCommand cmdUpdateDireccion = new SqlCommand(@"UPDATE Direccion 
                                                                       SET Deuda = ISNULL(Deuda, 0) + @ValorCuota 
                                                                       WHERE DireccionCompleta = @Direccion AND ProyectoNombre = @ProyectoNombre", connection, transaction))
                        {
                            cmdUpdateDireccion.Parameters.AddWithValue("@ValorCuota", valorExtra);
                            cmdUpdateDireccion.Parameters.AddWithValue("@Direccion", direccionSeleccionada);
                            cmdUpdateDireccion.Parameters.AddWithValue("@ProyectoNombre", proyectoNombre);

                            rowsAffected = cmdUpdateDireccion.ExecuteNonQuery();
                        }

                        if (rowsAffected == 0)
                            throw new Exception("No se encontró la dirección especificada para actualizar la deuda.");

                        // Insertar en la tabla Historial
                        using (SqlCommand cmdHistorial = new SqlCommand(@"INSERT INTO Historial (ID_Factura, Direccion, Deuda, ID_Usuario, Usuario, Tipo) 
                                                                 VALUES (@ID_Factura, @Direccion, @Deuda, @ID_Usuario, @Usuario, @Tipo)", connection, transaction))
                        {
                            cmdHistorial.Parameters.AddWithValue("@ID_Factura", idFactura);
                            cmdHistorial.Parameters.AddWithValue("@Direccion", direccionSeleccionada);
                            cmdHistorial.Parameters.AddWithValue("@Deuda", valorExtra);
                            cmdHistorial.Parameters.AddWithValue("@ID_Usuario", Utilidades.IdUsuario);
                            cmdHistorial.Parameters.AddWithValue("@Usuario", Utilidades.Usuario);
                            cmdHistorial.Parameters.AddWithValue("@Tipo", "Cuota Extra");
                            cmdHistorial.ExecuteNonQuery();
                        }

                        // Confirmar transacción
                        transaction.Commit();
                    }
                    catch
                    {
                        // Revertir transacción en caso de error
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private void CrearFacturaCuotaRegular()
        {
            if (dgvFactUnica.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(lblDireccionSeleccionada.Text))
                throw new Exception("Debe seleccionar una dirección válida.");

            string direccionSeleccionada = dgvFactUnica.SelectedRows[0].Cells["DireccionCompleta"].Value.ToString().Trim();
            string proyectoNombre = Utilidades.NombreProyectoSeleccionado;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Obtener ID de Dirección y cuota
                        int idDireccion;
                        decimal cuota;

                        string queryDireccion = "SELECT ID_Direccion, Cuota FROM Direccion WHERE DireccionCompleta = @Direccion AND ProyectoNombre = @ProyectoNombre";
                        using (SqlCommand cmdDireccion = new SqlCommand(queryDireccion, connection, transaction))
                        {
                            cmdDireccion.Parameters.AddWithValue("@Direccion", direccionSeleccionada);
                            cmdDireccion.Parameters.AddWithValue("@ProyectoNombre", proyectoNombre);

                            using (SqlDataReader reader = cmdDireccion.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    MessageBox.Show($"No se encontró la dirección: {direccionSeleccionada} en la tabla Direccion.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                idDireccion = Convert.ToInt32(reader["ID_Direccion"]);
                                cuota = Convert.ToDecimal(reader["Cuota"]);
                            }
                        }

                        // Crear factura regular
                        int idFactura;
                        using (SqlCommand cmdFactura = new SqlCommand(@"
                    INSERT INTO Factura (Direccion, ValorCuota, TipoCuota, DetalleFactura, ID_Direccion) 
                    OUTPUT INSERTED.ID_Factura 
                    VALUES (@Direccion, @ValorCuota, @TipoCuota, @DetalleFactura, @ID_Direccion)", connection, transaction))
                        {
                            cmdFactura.Parameters.AddWithValue("@Direccion", direccionSeleccionada);
                            cmdFactura.Parameters.AddWithValue("@ValorCuota", cuota);
                            cmdFactura.Parameters.AddWithValue("@TipoCuota", "Cuota Regular");
                            cmdFactura.Parameters.AddWithValue("@DetalleFactura", "Cuota Regular");
                            cmdFactura.Parameters.AddWithValue("@ID_Direccion", idDireccion);

                            idFactura = (int)cmdFactura.ExecuteScalar();
                        }

                        // Actualizar deuda en la dirección
                        int rowsAffected;
                        using (SqlCommand cmdUpdateDireccion = new SqlCommand(@"
                    UPDATE Direccion 
                    SET Deuda = ISNULL(Deuda, 0) + @ValorCuota 
                    WHERE ID_Direccion = @ID_Direccion", connection, transaction))
                        {
                            cmdUpdateDireccion.Parameters.AddWithValue("@ValorCuota", cuota);
                            cmdUpdateDireccion.Parameters.AddWithValue("@ID_Direccion", idDireccion);

                            rowsAffected = cmdUpdateDireccion.ExecuteNonQuery();
                        }

                        if (rowsAffected == 0)
                            throw new Exception("No se pudo actualizar la deuda. Verifique los datos proporcionados.");

                        // Insertar en historial
                        using (SqlCommand cmdHistorial = new SqlCommand(@"
                    INSERT INTO Historial (ID_Factura, Direccion, Deuda, ID_Usuario, Usuario, Tipo) 
                    VALUES (@ID_Factura, @Direccion, @Deuda, @ID_Usuario, @Usuario, @Tipo)", connection, transaction))
                        {
                            cmdHistorial.Parameters.AddWithValue("@ID_Factura", idFactura);
                            cmdHistorial.Parameters.AddWithValue("@Direccion", direccionSeleccionada);
                            cmdHistorial.Parameters.AddWithValue("@Deuda", cuota);
                            cmdHistorial.Parameters.AddWithValue("@ID_Usuario", Utilidades.IdUsuario);
                            cmdHistorial.Parameters.AddWithValue("@Usuario", Utilidades.Usuario);
                            cmdHistorial.Parameters.AddWithValue("@Tipo", "Cuota Regular");

                            cmdHistorial.ExecuteNonQuery();
                        }

                        // Confirmar transacción
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Revertir transacción en caso de error
                        transaction.Rollback();
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw;
                    }
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            // Limpiar ComboBox
            cboTipoCuota.SelectedIndex = -1; // Deselecciona cualquier elemento

            // Limpiar TextBoxes
            txtDireccion.Clear();
            txtValorExtra.Clear();
            txtDetalleFactExtra.Clear();

            // Limpiar el DataGridView de forma segura
            if (dgvFactUnica.DataSource != null)
            {
                // Si está vinculado a una fuente de datos, desvincularla
                dgvFactUnica.DataSource = null;
            }
            dgvFactUnica.Rows.Clear();  // Elimina todas las filas del DataGridView
            dgvFactUnica.Refresh(); // Refresca para mostrar que los cambios han sido aplicados

            // Limpiar Labels
            lblDireccionSeleccionada.Text = string.Empty; // Limpia el texto del Label
        }

        private void BuscarDireccion()
        {
            string valorDireccion = txtDireccion.Text.Trim();
            int idProyecto = Utilidades.IdProyectoSeleccionado;

            if (string.IsNullOrEmpty(valorDireccion))
            {
                MessageBox.Show("Por favor, ingrese una dirección para buscar.",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (idProyecto <= 0)
            {
                MessageBox.Show("No se ha seleccionado un proyecto válido.",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🔹 Composición final: IdProyecto-ValorTextbox
            string direccionCompleta = $"{idProyecto}-{valorDireccion}";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                SELECT DireccionCompleta, Cuota, ProyectoNombre
                FROM Direccion
                WHERE DireccionCompleta = @DireccionCompleta";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DireccionCompleta", direccionCompleta);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable resultados = new DataTable();
                            adapter.Fill(resultados);

                            if (resultados.Rows.Count > 0)
                            {
                                dgvFactUnica.DataSource = resultados;
                                dgvFactUnica.ClearSelection();
                            }
                            else
                            {
                                MessageBox.Show("No se encontraron coincidencias.",
                                                "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                dgvFactUnica.DataSource = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar la dirección: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvFactUnica_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar que la fila seleccionada sea válida (no es el encabezado ni una fila fuera de rango)
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Obtener la fila seleccionada
                DataGridViewRow filaSeleccionada = dgvFactUnica.Rows[e.RowIndex];

                // Verificar que las columnas existan y no sean nulas
                if (filaSeleccionada.Cells["DireccionCompleta"].Value != null && filaSeleccionada.Cells["Cuota"].Value != null)
                {
                    // Obtener los valores de las columnas DireccionCompleta y Cuota
                    string direccion = filaSeleccionada.Cells["DireccionCompleta"].Value.ToString();
                    string cuota = filaSeleccionada.Cells["Cuota"].Value.ToString();

                    // Asignar ambos valores concatenados al label
                    lblDireccionSeleccionada.Text = $"{direccion}  -  {cuota}";

                    // Asegurar que la fila seleccionada permanezca seleccionada
                    dgvFactUnica.ClearSelection();
                    filaSeleccionada.Selected = true;
                }
                else
                {
                    MessageBox.Show("La fila seleccionada no contiene datos válidos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private bool ValidarCreacionFactura()
        {
            // Verificar si el DataGridView tiene filas
            if (dgvFactUnica.Rows.Count == 0)
            {
                MessageBox.Show("No hay direcciones disponibles en la lista.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Verificar si alguna fila ha sido seleccionada en el DataGridView
            if (dgvFactUnica.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar una Dirección de la lista.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Verificar que el Label tiene texto (es decir, que una dirección ha sido seleccionada)
            if (string.IsNullOrWhiteSpace(lblDireccionSeleccionada.Text))
            {
                MessageBox.Show("Debe seleccionar una Dirección.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Verificar que el ComboBox tiene una opción seleccionada
            if (cboTipoCuota.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un Tipo de Cuota.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Si la opción seleccionada es "Cuota Extra", verificar los valores en los TextBox
            if (cboTipoCuota.SelectedItem.ToString() == "Cuota Extra")
            {
                if (string.IsNullOrWhiteSpace(txtDetalleFactExtra.Text))
                {
                    MessageBox.Show("Debe ingresar un Detalle para la Cuota Extra.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtValorExtra.Text))
                {
                    MessageBox.Show("Debe ingresar un valor para la Cuota Extra.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            // Si todas las validaciones son correctas, retornar true
            return true;
        }

    }
}