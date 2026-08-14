using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace AdminSoftPF
{
    public partial class Estatus : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        public Estatus()
        {
            InitializeComponent();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDireccionEstado.Text))
            {
                MessageBox.Show(
                    "Debe ingresar una unidad o dirección.",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            int idProyecto = Utilidades.IdProyectoSeleccionado;
            string unidad = txtDireccionEstado.Text.Trim();

            CargarHistorial(idProyecto, unidad);
        }

        private void CargarHistorial(int idProyecto, string unidad)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    cn.Open();

                    string query = @"
                        SELECT 
                            ID_Historial,
                            FechaRegistro,
                            Tipo,
                            Deuda,
                            Pago
                        FROM Historial
                        WHERE
                            -- Proyecto: tomar el primer valor antes del guion
                            CAST(
                                LEFT(Direccion, CHARINDEX('-', Direccion + '-') - 1)
                                AS INT
                            ) = @IdProyecto
                            AND
                            -- Unidad completa luego del IdProyecto-
                            Direccion LIKE @Direccion
                        ORDER BY FechaRegistro ASC";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.Add("@IdProyecto", SqlDbType.Int).Value = idProyecto;
                        cmd.Parameters.Add("@Direccion", SqlDbType.VarChar)
                            .Value = idProyecto + "-" + unidad + "%";

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dgvHistorial.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al consultar historial:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnPrintPDF_Click(object sender, EventArgs e)
        {
            if (dgvHistorial.Rows.Count == 0)
            {
                MessageBox.Show(
                    "No hay información para imprimir.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }

            GenerarPDFEstadoCuenta();
        }

        private void GenerarPDFEstadoCuenta()
        {
            SaveFileDialog save = new SaveFileDialog
            {
                Filter = "PDF (*.pdf)|*.pdf",
                FileName = "EstadoCuenta.pdf"
            };

            if (save.ShowDialog() != DialogResult.OK) return;

            Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
            PdfWriter.GetInstance(doc, new FileStream(save.FileName, FileMode.Create));
            doc.Open();

            Font titulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
            Font normal = FontFactory.GetFont(FontFactory.HELVETICA, 9);
            Font bold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);

            Paragraph encabezado = new Paragraph("ESTADO DE CUENTA\n\n", titulo)
            {
                Alignment = Element.ALIGN_CENTER
            };
            doc.Add(encabezado);

            doc.Add(new Paragraph($"Fecha de emisión: {DateTime.Now:dd/MM/yyyy}", normal));
            doc.Add(new Paragraph($"Unidad: {txtDireccionEstado.Text}\n\n", normal));

            PdfPTable table = new PdfPTable(5)
            {
                WidthPercentage = 100
            };
            table.SetWidths(new float[] { 15, 40, 15, 15, 15 });

            AgregarCelda(table, "Fecha", bold);
            AgregarCelda(table, "Concepto", bold);
            AgregarCelda(table, "Cargos", bold);
            AgregarCelda(table, "Abonos", bold);
            AgregarCelda(table, "Saldo", bold);

            decimal totalCargos = 0;
            decimal totalAbonos = 0;
            decimal saldoFinal = 0;

            foreach (DataGridViewRow row in dgvHistorial.Rows)
            {
                if (row.IsNewRow) continue;

                DateTime fecha = Convert.ToDateTime(row.Cells["FechaRegistro"].Value);
                string concepto = row.Cells["ConceptoMovimiento"].Value.ToString();
                decimal cargo = Convert.ToDecimal(row.Cells["Cargo"].Value);
                decimal abono = Convert.ToDecimal(row.Cells["Abono"].Value);
                decimal saldo = Convert.ToDecimal(row.Cells["Saldo"].Value);

                totalCargos += cargo;
                totalAbonos += abono;
                saldoFinal = saldo;

                AgregarCelda(table, fecha.ToShortDateString(), normal);
                AgregarCelda(table, concepto, normal);
                AgregarCelda(table, cargo.ToString("N2"), normal);
                AgregarCelda(table, abono.ToString("N2"), normal);
                AgregarCelda(table, saldo.ToString("N2"), normal);
            }

            doc.Add(table);
            doc.Add(new Paragraph("\n"));

            doc.Add(new Paragraph($"TOTAL CARGOS: {totalCargos:N2}", bold));
            doc.Add(new Paragraph($"TOTAL ABONOS: {totalAbonos:N2}", bold));
            doc.Add(new Paragraph($"SALDO FINAL: {saldoFinal:N2}", bold));

            doc.Close();

            MessageBox.Show(
                "PDF generado correctamente.",
                "Éxito",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }


        private void AgregarCelda(PdfPTable table, string texto, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(texto, font))
            {
                Padding = 5,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            table.AddCell(cell);
        }

    }
}
