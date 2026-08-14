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
    public partial class Gestion : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        string proyectoSeleccionado = Utilidades.NombreProyectoSeleccionado;

        public Gestion(string proyectoSeleccionado)
        {
            InitializeComponent();

            this.proyectoSeleccionado = proyectoSeleccionado;

            LimpiarClienteYProyecto();

            // Asociar el evento Shown
            this.Shown += Gestion_Shown;
        }

        private void Gestion_Load(object sender, EventArgs e)
        {
            txtBucador.MaxLength = 20;

            // Limpiar selección inicial del DataGridView
            dgvClientes.ClearSelection();
        }
        
        private void Gestion_Shown(object sender, EventArgs e)
        {
            // Asegurarse de que no haya filas seleccionadas después de mostrar el formulario
            dgvClientes.ClearSelection();
        }
        
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Si el DataGridView aún no tiene datos, cargarlo
            if (dgvClientes.DataSource == null)
            {
                LlenarDataGridClientes();
            }

            // Aplicar el filtro directamente en las filas
            FiltrarDataGridView(txtBucador.Text);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            // Limpiar el DataGridView
            dgvClientes.DataSource = null;

            LimpiarClienteYProyecto();
        }

        private void LimpiarClienteYProyecto()
        {
            
            lblClienteSeleccionado.Text = "";

            txtBucador.Text = string.Empty;

        }
        /*
        public void LlenarDataGridClientes()
        {
            LimpiarClienteYProyecto();
            
            if (string.IsNullOrEmpty(proyectoSeleccionado))
            {
                MessageBox.Show("Por favor, seleccione un proyecto en el menú principal.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"
                            SELECT 
                                c.ID_Cliente,
                                d.DireccionCompleta,
                                c.NombreCompleto,
                                c.Documento,
                                d.Cuota,
                                d.Deuda
                            FROM 
                                direccion d
                            INNER JOIN 
                                cliente c
                            ON 
                                d.ID_Cliente = c.ID_Cliente
                            WHERE 
                                d.ProyectoNombre = @ProyectoNombre";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProyectoNombre", proyectoSeleccionado);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dgvClientes.DataSource = dataTable;

                    // Configurar tamaño de columnas específicas
                    dgvClientes.Columns["ID_Cliente"].Width = 70;
                    dgvClientes.Columns["DireccionCompleta"].Width = 120;
                    dgvClientes.Columns["NombreCompleto"].Width = 447;
                    dgvClientes.Columns["Documento"].Width = 110;
                    dgvClientes.Columns["Cuota"].Width = 90;
                    dgvClientes.Columns["Deuda"].Width = 85;

                    // Aplicar formato directamente en la columna del DataGridView
                    dgvClientes.Columns["Cuota"].DefaultCellStyle.Format = "N2";
                    dgvClientes.Columns["Cuota"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    // Aplicar formato directamente en la columna del DataGridView
                    dgvClientes.Columns["Deuda"].DefaultCellStyle.Format = "N2";
                    dgvClientes.Columns["Deuda"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    dgvClientes.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        */

        public void LlenarDataGridClientes()
        {
            LimpiarClienteYProyecto();

            if (string.IsNullOrEmpty(proyectoSeleccionado))
            {
                MessageBox.Show("Por favor, seleccione un proyecto en el menú principal.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"
                    SELECT 
                        c.ID_Cliente,
                        d.DireccionCompleta,
                        c.NombreCompleto,
                        c.Documento,
                        d.Cuota,
                        d.Deuda
                    FROM 
                        direccion d
                    INNER JOIN 
                        cliente c
                    ON 
                        d.ID_Cliente = c.ID_Cliente
                    WHERE 
                        d.ProyectoNombre = @ProyectoNombre";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProyectoNombre", proyectoSeleccionado);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Transformar la dirección para quitar el primer segmento
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row["DireccionCompleta"] != DBNull.Value)
                        {
                            string direccion = row["DireccionCompleta"].ToString();
                            row["DireccionCompleta"] = QuitarPrimerSegmentoDireccion(direccion);
                        }
                    }

                    dgvClientes.DataSource = dataTable;

                    // Configurar tamaño de columnas específicas
                    dgvClientes.Columns["ID_Cliente"].Width = 70;
                    dgvClientes.Columns["DireccionCompleta"].Width = 120;
                    dgvClientes.Columns["NombreCompleto"].Width = 447;
                    dgvClientes.Columns["Documento"].Width = 110;
                    dgvClientes.Columns["Cuota"].Width = 90;
                    dgvClientes.Columns["Deuda"].Width = 85;

                    // Aplicar formato directamente en la columna del DataGridView
                    dgvClientes.Columns["Cuota"].DefaultCellStyle.Format = "N2";
                    dgvClientes.Columns["Cuota"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    dgvClientes.Columns["Deuda"].DefaultCellStyle.Format = "N2";
                    dgvClientes.Columns["Deuda"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    dgvClientes.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string QuitarPrimerSegmentoDireccion(string direccion)
        {
            if (string.IsNullOrEmpty(direccion)) return direccion;

            // Dividir la dirección en segmentos usando el guion
            string[] partes = direccion.Split('-');

            // Verificar si hay más de un segmento para quitar el primero
            if (partes.Length > 1)
            {
                // Unir los segmentos a partir del segundo
                return string.Join("-", partes.Skip(1));
            }

            return direccion;
        }

        private void DgvClientes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvClientes.SelectedRows[0];

                string nombreCompleto = selectedRow.Cells["NombreCompleto"].Value?.ToString();
                string direccionCompleta = selectedRow.Cells["DireccionCompleta"].Value?.ToString();

                // Actualizar el label con la dirección ya transformada
                lblClienteSeleccionado.Text = $"{direccionCompleta ?? "N/A"} - {nombreCompleto ?? "N/A"}";
            }
            else
            {
                lblClienteSeleccionado.Text = string.Empty;
            }
        }


        /*
        private void DgvClientes_SelectionChanged(object sender, EventArgs e)
        {
            // Validar si hay una fila seleccionada
            if (dgvClientes.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvClientes.SelectedRows[0];

                // Obtener los valores de las columnas "NombreCompleto" y "DireccionCompleta"
                string nombreCompleto = selectedRow.Cells["NombreCompleto"].Value?.ToString();
                string direccionCompleta = selectedRow.Cells["DireccionCompleta"].Value?.ToString();

                // Construir el texto combinado para lblClienteSeleccionado
                lblClienteSeleccionado.Text = $"{direccionCompleta ?? "N/A"} - {nombreCompleto ?? "N/A"}";
            }
            else
            {
                // Limpiar el label si no hay selección
                lblClienteSeleccionado.Text = string.Empty;
            }
        }
        */

        private void FiltrarDataGridView(string filtro)
        {
            if (dgvClientes.Rows.Count > 0)
            {
                // Convertir el filtro a minúsculas para comparación insensible a mayúsculas
                filtro = filtro.ToLower();

                // Desactivar temporalmente el CurrencyManager para evitar errores
                CurrencyManager currencyManager = (CurrencyManager)BindingContext[dgvClientes.DataSource];
                currencyManager.SuspendBinding();

                // Iterar sobre todas las filas del DataGridView
                foreach (DataGridViewRow row in dgvClientes.Rows)
                {
                    // Asegurarse de que la fila no sea nula
                    if (row.IsNewRow) continue;

                    // Obtener valores de las columnas, convertir a string y minúsculas
                    string nombreCompleto = row.Cells["NombreCompleto"].Value?.ToString()?.ToLower() ?? "";
                    string direccionCompleta = row.Cells["DireccionCompleta"].Value?.ToString()?.ToLower() ?? "";
                    string documento = row.Cells["Documento"].Value?.ToString()?.ToLower() ?? "";

                    // Verificar si el valor del filtro está presente en alguna columna
                    bool contieneFiltro = nombreCompleto.Contains(filtro)
                                       || direccionCompleta.Contains(filtro)
                                       || documento.Contains(filtro);

                    // Mostrar u ocultar la fila en base al resultado
                    row.Visible = contieneFiltro;
                }

                // Reactivar el CurrencyManager después de modificar las filas
                currencyManager.ResumeBinding();

                // Limpiar la selección del DataGridView
                dgvClientes.ClearSelection();
                lblClienteSeleccionado.Text = string.Empty;

                // Verificar si al menos una fila es visible
                if (!dgvClientes.Rows.Cast<DataGridViewRow>().Any(r => r.Visible))
                {
                    MessageBox.Show("No se encontraron coincidencias.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("No hay datos para filtrar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvClientes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar si hay una fila seleccionada
            if (dgvClientes.SelectedRows.Count > 0)
            {
                // Obtener el valor de la columna ID_Cliente de la fila seleccionada
                int idCliente = Convert.ToInt32(dgvClientes.SelectedRows[0].Cells["ID_Cliente"].Value);

                // Acceder al formulario Menu y abrir el formulario Detalle
                Form menuForm = Application.OpenForms.OfType<Menu>().FirstOrDefault();

                if (menuForm != null)
                {
                    // Crear el formulario Detalle
                    Detalle detalleForm = new Detalle();

                    // Establecer el valor de ID_Cliente en el formulario Detalle
                    detalleForm.ID_Cliente = idCliente;

                    // Llamar al método en Menu para abrir el formulario Detalle en el panel
                    (menuForm as Menu)?.AbrirFormularioEnPanel(detalleForm);

                    // Si necesitas cargar datos en Detalle usando el ID_Cliente, puedes llamarlo aquí:
                    detalleForm.CargarDatosClienteSeleccionado();
                }
            }

        }

    }
}