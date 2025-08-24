using System;
using System.ComponentModel; // BindingList
using System.Windows.Forms;
using System.Drawing;

namespace Pantallas_Sistema_facturación.Tablas
{
    public class ProductosForm : Form
    {
        private TextBox txtNombre;
        private TextBox txtCodigo;
        private TextBox txtPrecio;
        private Button btnAgregar;
        private ErrorProvider errorProvider;
        private DataGridView dgvProductos;

        // Binding que notifica al grid
        private BindingList<Producto> productos = new BindingList<Producto>();
        private BindingSource bsProductos = new BindingSource();

        public ProductosForm()
        {
            this.Text = "Administración de Productos";
            this.BackColor = Color.White;

            Label lblTitulo = new Label()
            {
                Text = "ADMINISTRACIÓN DE PRODUCTOS",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 120, 200),
                Left = 20,
                Top = 20,
                Width = 350
            };

            Label lblNombre = new Label() { Text = "Nombre:", Left = 20, Top = 60, Width = 80 };
            txtNombre = new TextBox() { Left = 110, Top = 60, Width = 200 };

            Label lblCodigo = new Label() { Text = "Código:", Left = 20, Top = 100, Width = 80 };
            txtCodigo = new TextBox() { Left = 110, Top = 100, Width = 200 };

            Label lblPrecio = new Label() { Text = "Precio:", Left = 20, Top = 140, Width = 80 };
            txtPrecio = new TextBox() { Left = 110, Top = 140, Width = 200 };

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

            dgvProductos = new DataGridView()
            {
                Left = 20,
                Top = 230,
                Width = 500,
                Height = 140,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.FixedSingle,

                // Estilos para legibilidad
                BackgroundColor = Color.White,
                GridColor = Color.Silver,
                EnableHeadersVisualStyles = false
            };

            dgvProductos.ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro;
            dgvProductos.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvProductos.DefaultCellStyle.BackColor = Color.White;
            dgvProductos.DefaultCellStyle.ForeColor = Color.Black;
            dgvProductos.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dgvProductos.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Enlace de datos
            bsProductos.DataSource = productos;
            dgvProductos.DataSource = bsProductos;

            // Formato de columna Precio cuando exista
            dgvProductos.DataBindingComplete += (s, e) =>
            {
                if (dgvProductos.Columns["Precio"] != null)
                    dgvProductos.Columns["Precio"].DefaultCellStyle.Format = "C2"; // moneda local
            };

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblNombre);
            this.Controls.Add(txtNombre);
            this.Controls.Add(lblCodigo);
            this.Controls.Add(txtCodigo);
            this.Controls.Add(lblPrecio);
            this.Controls.Add(txtPrecio);
            this.Controls.Add(btnAgregar);
            this.Controls.Add(dgvProductos);
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            bool valido = true;

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                errorProvider.SetError(txtNombre, "El nombre es obligatorio.");
                valido = false;
            }
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                errorProvider.SetError(txtCodigo, "El código es obligatorio.");
                valido = false;
            }
            if (string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                errorProvider.SetError(txtPrecio, "El precio es obligatorio.");
                valido = false;
            }
            else
            {
                if (!decimal.TryParse(txtPrecio.Text, out _))
                {
                    errorProvider.SetError(txtPrecio, "El precio debe ser numérico.");
                    valido = false;
                }
            }

            if (!valido) return;

            // Convertir a decimal seguro
            decimal precio = decimal.Parse(txtPrecio.Text);

            // Con BindingList, el grid se actualiza al instante
            productos.Add(new Producto
            {
                Nombre = txtNombre.Text.Trim(),
                Codigo = txtCodigo.Text.Trim(),
                Precio = precio
            });

            txtNombre.Clear();
            txtCodigo.Clear();
            txtPrecio.Clear();
            txtNombre.Focus();
        }

        // Pública para binding y con tipo decimal para formato de moneda
        public class Producto
        {
            public string Nombre { get; set; }
            public string Codigo { get; set; }
            public decimal Precio { get; set; }
        }
    }
}
