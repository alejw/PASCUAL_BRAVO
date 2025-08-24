using System;
using System.ComponentModel; // BindingList
using System.Windows.Forms;
using System.Drawing;

namespace Pantallas_Sistema_facturación.Tablas
{
    public class ClientesForm : Form
    {
        private TextBox txtNombre;
        private TextBox txtDocumento;
        private TextBox txtTelefono;
        private Button btnAgregar;
        private ErrorProvider errorProvider;
        private DataGridView dgvClientes;

        // CAMBIO: BindingList + BindingSource
        private BindingList<Cliente> clientes = new BindingList<Cliente>();
        private BindingSource bsClientes = new BindingSource();

        public ClientesForm()
        {
            this.Text = "Administración de Clientes";
            this.BackColor = Color.White;

            Label lblTitulo = new Label()
            {
                Text = "ADMINISTRACIÓN DE CLIENTES",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 120, 200),
                Left = 20,
                Top = 20,
                Width = 350
            };

            Label lblNombre = new Label() { Text = "Nombre:", Left = 20, Top = 60, Width = 80 };
            txtNombre = new TextBox() { Left = 110, Top = 60, Width = 200 };

            Label lblDocumento = new Label() { Text = "Documento:", Left = 20, Top = 100, Width = 80 };
            txtDocumento = new TextBox() { Left = 110, Top = 100, Width = 200 };

            Label lblTelefono = new Label() { Text = "Teléfono:", Left = 20, Top = 140, Width = 80 };
            txtTelefono = new TextBox() { Left = 110, Top = 140, Width = 200 };

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

            dgvClientes = new DataGridView()
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

            dgvClientes.ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro;
            dgvClientes.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvClientes.DefaultCellStyle.BackColor = Color.White;
            dgvClientes.DefaultCellStyle.ForeColor = Color.Black;
            dgvClientes.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dgvClientes.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Enlace de datos
            bsClientes.DataSource = clientes;
            dgvClientes.DataSource = bsClientes;

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblNombre);
            this.Controls.Add(txtNombre);
            this.Controls.Add(lblDocumento);
            this.Controls.Add(txtDocumento);
            this.Controls.Add(lblTelefono);
            this.Controls.Add(txtTelefono);
            this.Controls.Add(btnAgregar);
            this.Controls.Add(dgvClientes);
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
            if (string.IsNullOrWhiteSpace(txtDocumento.Text))
            {
                errorProvider.SetError(txtDocumento, "El documento es obligatorio.");
                valido = false;
            }
            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                errorProvider.SetError(txtTelefono, "El teléfono es obligatorio.");
                valido = false;
            }
            if (!valido) return;

            // BindingList notifica al grid
            clientes.Add(new Cliente
            {
                Nombre = txtNombre.Text.Trim(),
                Documento = txtDocumento.Text.Trim(),
                Telefono = txtTelefono.Text.Trim()
            });

            txtNombre.Clear();
            txtDocumento.Clear();
            txtTelefono.Clear();
            txtNombre.Focus();
        }

        // Pública para binding más predecible
        public class Cliente
        {
            public string Nombre { get; set; }
            public string Documento { get; set; }
            public string Telefono { get; set; }
        }
    }
}
