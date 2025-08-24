using System;
using System.Windows.Forms;
// === Agregados para PDF ===
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;

namespace Pantallas_Sistema_facturación.Facturacion
{
    public class InformesForm : Form
    {
        private ComboBox cmbInforme;
        private ComboBox cmbOrdenar;
        private DateTimePicker dtpInicio;
        private DateTimePicker dtpFinal;
        private RadioButton rbPantalla;
        private RadioButton rbPdf;
        private RadioButton rbExcel;
        private Button btnGenerar;
        private Button btnSalir;
        private Panel panelResultado;

        public InformesForm()
        {
            this.Text = "Generador de Informes de Facturación";
            this.Width = 800;
            this.Height = 500;
            this.BackColor = System.Drawing.Color.White;

            Label lblTitulo = new Label()
            {
                Text = "GENERADOR DE INFORMES DE FACTURACIÓN",
                Left = 250,
                Top = 20,
                Width = 350,
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold)
            };

            Label lblInforme = new Label() { Text = "SELECCIONE INFORME", Left = 40, Top = 70, Width = 150 };
            cmbInforme = new ComboBox() { Left = 200, Top = 65, Width = 200 };
            cmbInforme.Items.AddRange(new string[] { "Ventas", "Clientes", "Productos" });
            cmbInforme.SelectedIndex = 0;

            Label lblOrdenar = new Label() { Text = "Ordenar por:", Left = 420, Top = 70, Width = 100 };
            cmbOrdenar = new ComboBox() { Left = 530, Top = 65, Width = 150 };
            cmbOrdenar.Items.AddRange(new string[] { "Fecha", "Cliente", "Producto" });
            cmbOrdenar.SelectedIndex = 0;

            LinkLabel lblFechaInicio = new LinkLabel() { Text = "Fecha Inicio", Left = 40, Top = 110, Width = 100 };
            dtpInicio = new DateTimePicker() { Left = 150, Top = 105, Width = 150 };

            LinkLabel lblFechaFinal = new LinkLabel() { Text = "Fecha Final", Left = 320, Top = 110, Width = 100 };
            dtpFinal = new DateTimePicker() { Left = 430, Top = 105, Width = 150 };

            rbPantalla = new RadioButton() { Text = "En Pantalla", Left = 150, Top = 150, Checked = true };
            rbPdf = new RadioButton() { Text = "Pdf", Left = 270, Top = 150 };
            rbExcel = new RadioButton() { Text = "Excel", Left = 340, Top = 150 };

            btnGenerar = new Button()
            {
                Text = "GENERAR INFORME",
                Left = 200,
                Top = 200,
                Width = 180,
                Height = 40,
                BackColor = System.Drawing.Color.FromArgb(60, 120, 200),
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold)
            };
            btnGenerar.Click += BtnGenerar_Click;

            btnSalir = new Button()
            {
                Text = "SALIR",
                Left = 400,
                Top = 200,
                Width = 120,
                Height = 40,
                BackColor = System.Drawing.Color.FromArgb(60, 120, 200),
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold)
            };
            btnSalir.Click += (s, e) => { this.Parent?.Controls.Remove(this); };

            panelResultado = new Panel()
            {
                Left = 40,
                Top = 260,
                Width = 700,
                Height = 180,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = System.Drawing.Color.White // recuadro blanco
            };

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblInforme);
            this.Controls.Add(cmbInforme);
            this.Controls.Add(lblOrdenar);
            this.Controls.Add(cmbOrdenar);
            this.Controls.Add(lblFechaInicio);
            this.Controls.Add(dtpInicio);
            this.Controls.Add(lblFechaFinal);
            this.Controls.Add(dtpFinal);
            this.Controls.Add(rbPantalla);
            this.Controls.Add(rbPdf);
            this.Controls.Add(rbExcel);
            this.Controls.Add(btnGenerar);
            this.Controls.Add(btnSalir);
            this.Controls.Add(panelResultado);
        }

        private void BtnGenerar_Click(object sender, EventArgs e)
        {
            panelResultado.Controls.Clear();

            string resumen =
                $"Informe generado: {cmbInforme.Text}\n" +
                $"Ordenado por: {cmbOrdenar.Text}\n" +
                $"Del {dtpInicio.Value.ToShortDateString()} al {dtpFinal.Value.ToShortDateString()}\n" +
                $"Salida: {(rbPantalla.Checked ? "Pantalla" : rbPdf.Checked ? "PDF" : "Excel")}";

            Label lbl = new Label()
            {
                Text = resumen,
                Left = 10,
                Top = 10,
                Width = 680,
                Height = 80,
                ForeColor = System.Drawing.Color.Black // texto negro para que se vea sobre blanco
            };
            panelResultado.Controls.Add(lbl);

            // Exportar a PDF si se seleccionó PDF
            if (rbPdf.Checked)
            {
                try
                {
                    ExportarComoPdf(resumen);
                    MessageBox.Show("PDF generado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo generar el PDF.\n\nDetalle: " + ex.Message,
                        "Error al generar PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ====== Generación de PDF usando "Microsoft Print to PDF" ======
        private void ExportarComoPdf(string contenido)
        {
            // Verifica impresora instalada
            string impresoraPdf = PrinterSettings.InstalledPrinters
                .Cast<string>()
                .FirstOrDefault(p => p.IndexOf("Microsoft Print to PDF", StringComparison.OrdinalIgnoreCase) >= 0);

            if (impresoraPdf == null)
                throw new InvalidOperationException(
                    "No se encontró la impresora 'Microsoft Print to PDF'. Actívala en Configuración de Windows > Impresoras y escáneres.");

            using var sfd = new SaveFileDialog
            {
                Filter = "Archivo PDF (*.pdf)|*.pdf",
                FileName = $"Informe_{cmbInforme.Text}_{DateTime.Now:yyyyMMdd_HHmm}.pdf",
                OverwritePrompt = true
            };
            if (sfd.ShowDialog(this) != DialogResult.OK)
                return;

            using var doc = new PrintDocument();
            doc.PrinterSettings.PrinterName = impresoraPdf;
            doc.PrinterSettings.PrintToFile = true;
            doc.PrinterSettings.PrintFileName = sfd.FileName;
            doc.PrintController = new StandardPrintController(); // sin diálogo
            doc.DefaultPageSettings.Margins = new Margins(50, 50, 60, 60);

            doc.PrintPage += (s, e) =>
            {
                var g = e.Graphics;
                var bounds = e.MarginBounds;

                using var titleFont = new Font("Segoe UI Semibold", 14f);
                using var bodyFont = new Font("Segoe UI", 11f);

                string titulo = $"Informe de {cmbInforme.Text}";
                g.DrawString(titulo, titleFont, Brushes.Black, bounds.Left, bounds.Top);
                int y = bounds.Top + 28;
                g.DrawLine(Pens.Black, bounds.Left, y, bounds.Right, y);

                var rect = new RectangleF(bounds.Left, y + 12, bounds.Width, bounds.Height - 40);
                g.DrawString(contenido, bodyFont, Brushes.Black, rect);

                e.HasMorePages = false;
            };

            doc.Print();
        }
    }
}
