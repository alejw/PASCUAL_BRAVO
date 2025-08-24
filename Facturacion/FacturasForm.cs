using System;
using System.ComponentModel; // BindingList
using System.Windows.Forms;
using System.Drawing;

namespace Pantallas_Sistema_facturación.Facturacion
{
    public class FacturasForm : Form
    {
        private TextBox txtNumero;
        private TextBox txtCliente;
        private TextBox txtTotal;
        private Button btnAgregar;
        private ErrorProvider errorProvider;
        private DataGridView dgvFacturas;

        // CAMBIO: BindingList para que el grid se refresque automáticamente
        private BindingList<Factura> facturas = new BindingList<Factura>();
        private BindingSource bsFacturas = new BindingSource();

        public FacturasForm()
        {
            this.Text = "Administración de Facturas";
            this.BackColor = Color.White;

            Label lblTitulo = new Label()
            {
                Text = "ADMINISTRACIÓN DE FACTURAS",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 120, 200),
                Left = 20,
                Top = 20,
                Width = 350
            };

            Label lblNumero = new Label() { Text = "Número:", Left = 20, Top = 60, Width = 80 };
            txtNumero = new TextBox() { Left = 110, Top = 60, Width = 200 };

            Label lblCliente = new Label() { Text = "Cliente:", Left = 20, Top = 100, Width = 80 };
            txtCliente = new TextBox() { Left = 110, Top = 100, Width = 200 };

            Label lblTotal = new Label() { Text = "Total:", Left = 20, Top = 140, Width = 80 };
            txtTotal = new TextBox() { Left = 110, Top = 140, Width = 200 };

            btnAgregar = new Button()
            {
                Text = "Agregar",
                Left = 110,
                Top = 180,
                Width = 200,
                Height = 35,
                BackColor = Color.FromArgb(60, 120, 200),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnAgregar.Click += BtnAgregar_Click;

            errorProvider = new ErrorProvider();

            dgvFacturas = new DataGridView()
            {
                Left = 20,
                Top = 230,
                Width = 500,
                Height = 140,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                BackgroundColor = Color.White,
                GridColor = Color.Silver,
                EnableHeadersVisualStyles = false
            };

            dgvFacturas.ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro;
            dgvFacturas.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvFacturas.DefaultCellStyle.BackColor = Color.White;
            dgvFacturas.DefaultCellStyle.ForeColor = Color.Black;
            dgvFacturas.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dgvFacturas.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Enlace de datos
            bsFacturas.DataSource = facturas;
            dgvFacturas.DataSource = bsFacturas;

            // Formato moneda en la columna Total
            dgvFacturas.DataBindingComplete += (s, e) =>
            {
                if (dgvFacturas.Columns["Total"] != null)
                    dgvFacturas.Columns["Total"].DefaultCellStyle.Format = "C2";
            };

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblNumero);
            this.Controls.Add(txtNumero);
            this.Controls.Add(lblCliente);
            this.Controls.Add(txtCliente);
            this.Controls.Add(lblTotal);
            this.Controls.Add(txtTotal);
            this.Controls.Add(btnAgregar);
            this.Controls.Add(dgvFacturas);
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            bool valido = true;

            if (string.IsNullOrWhiteSpace(txtNumero.Text))
            {
                errorProvider.SetError(txtNumero, "El número es obligatorio.");
                valido = false;
            }
            if (string.IsNullOrWhiteSpace(txtCliente.Text))
            {
                errorProvider.SetError(txtCliente, "El cliente es obligatorio.");
                valido = false;
            }

            decimal total = 0;
            if (string.IsNullOrWhiteSpace(txtTotal.Text))
            {
                errorProvider.SetError(txtTotal, "El total es obligatorio.");
                valido = false;
            }
            else if (!decimal.TryParse(txtTotal.Text, out total))
            {
                errorProvider.SetError(txtTotal, "El total debe ser numérico.");
                valido = false;
            }

            if (!valido) return;

            // Con BindingList, el grid se refresca al instante
            facturas.Add(new Factura
            {
                Numero = txtNumero.Text.Trim(),
                Cliente = txtCliente.Text.Trim(),
                Total = total
            });

            txtNumero.Clear();
            txtCliente.Clear();
            txtTotal.Clear();
            txtNumero.Focus();
        }

        // Pública para binding
        public class Factura
        {
            public string Numero { get; set; }
            public string Cliente { get; set; }
            public decimal Total { get; set; }
        }
    }
}
