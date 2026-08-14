using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminSoftPF
{
    public partial class PagoConTarjeta : Form
    {
        public string TotalTarjeta { get; private set; } // Propiedad para almacenar el valor total
        public PagoConTarjeta()
        {
            InitializeComponent();
        }

        private void PagoConTarjeta_Load(object sender, EventArgs e)
        {
            CamposNoEditables();
        }

        private void CamposNoEditables()
        {
            ConfiguracionDgv(dgvDatosTarjeta);

        }

        private void ConfiguracionDgv(DataGridView dgv)
        {
            // Deshabilitar el reordenamiento de columnas
            dgv.AllowUserToOrderColumns = false;

            // Deshabilitar el ordenamiento de columnas
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void ValidarSoloNumeros(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                // Permitir solo números (0-9), la tecla Backspace y el punto decimal
                if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != '.')
                {
                    e.Handled = true; // Bloquea la tecla
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
                    e.Handled = true;
                    return;
                }

                // Validar parte decimal (máximo 2 dígitos después del punto)
                if (parts.Length > 1 && textBox.SelectionStart > text.IndexOf('.') && parts[1].Length >= 2)
                {
                    e.Handled = true;
                }
            }
        }

        private void ValidarReferencia(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                // Permitir solo números (0-9), la tecla Backspace y el punto decimal
                if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != '.')
                {
                    e.Handled = true; // Bloquea la tecla
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
                    e.Handled = true;
                    return;
                }

                // Validar parte decimal (máximo 2 dígitos después del punto)
                if (parts.Length > 1 && textBox.SelectionStart > text.IndexOf('.') && parts[1].Length >= 2)
                {
                    e.Handled = true;
                }
            }
        }

        private void TxtReferencia_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                // Permitir solo letras, números y la tecla Backspace
                if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true; // Bloquear cualquier otro carácter
                    return;
                }

                // Convertir el carácter actual a mayúscula
                e.KeyChar = char.ToUpper(e.KeyChar);

                // Limitar a 12 caracteres
                if (textBox.Text.Length >= 12 && e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true; // Bloquear entrada adicional
                }
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Validar que los TextBox no estén vacíos
            if (string.IsNullOrWhiteSpace(txtReferencia.Text) || string.IsNullOrWhiteSpace(txtMonto.Text))
            {
                MessageBox.Show("Por favor, complete los campos de Referencia y Monto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar que el monto sea un número válido
            if (!decimal.TryParse(txtMonto.Text, out decimal monto))
            {
                MessageBox.Show("El monto debe ser un valor numérico válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Agregar los datos al DataGridView
            dgvDatosTarjeta.Rows.Add(txtReferencia.Text, monto.ToString("N2"));

            ActualizarTotal();

            LimpiarCampos();

        }

        private void LimpiarCampos()
        {
            // Limpiar los campos para permitir nuevas entradas
            txtReferencia.Clear();
            txtMonto.Clear();
            txtReferencia.Focus();
        }

        private void ActualizarTotal()
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dgvDatosTarjeta.Rows)
            {
                if (row.Cells["Monto"].Value != null && decimal.TryParse(row.Cells["Monto"].Value.ToString(), out decimal monto))
                {
                    total += monto;
                }
            }

            lblTotal.Text = $"Total: {total:N2}";
        }

        private void dgvDatosTarjeta_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            ActualizarTotal();
        }

        private void dgvDatosTarjeta_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            ActualizarTotal();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDatosTarjeta.SelectedRows.Count > 0)
            {
                var confirmResult = MessageBox.Show(
                    "¿Está seguro de que desea eliminar el pago seleccionado?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    foreach (DataGridViewRow selectedRow in dgvDatosTarjeta.SelectedRows)
                    {
                        if (!selectedRow.IsNewRow)
                        {
                            dgvDatosTarjeta.Rows.Remove(selectedRow);
                        }
                    }

                    ActualizarTotal();
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un registro para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAgregarPago_Click(object sender, EventArgs e)
        {
            // Verificar si el DataGridView tiene registros
            if (dgvDatosTarjeta.Rows.Count > 0)
            {
                // Tomar el valor de lblTotal sin el prefijo "Total: "
                TotalTarjeta = lblTotal.Text.Replace("Total: ", "").Trim();

                // Cerrar el formulario con un resultado exitoso
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Debe agregar al menos un pago antes de continuar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Mostrar cuadro de diálogo de confirmación
            DialogResult result = MessageBox.Show(
                "¿Está seguro de que desea cancelar y cerrar el formulario? Los datos no guardados se perderán.",
                "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            // Verificar la respuesta del usuario
            if (result == DialogResult.Yes)
            {
                LimpiarCampos();

                this.Close();
            }
        }
    }
}
