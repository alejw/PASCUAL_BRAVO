using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace Pantallas_Sistema_facturación.Tablas
{
    public class CategoriasForm : Form
    {
        private TextBox txtNombre;
        private TextBox txtDescripcion;
        private Button btnAgregar;
        private ErrorProvider errorProvider;
        private DataGridView dgvCategorias;

        // === CAMBIO: BindingList para notificar al grid ===
        private BindingList<Categoria> categorias = new BindingList<Categoria>();
        private BindingSource bsCategorias = new BindingSource();

        public CategoriasForm()
        {
            this.Text = "Administración de Categorías";
            this.BackColor = Color.White;

            Label lblTitulo = new Label()
            {
                Text = "ADMINISTRACIÓN DE CATEGORÍAS",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 120, 200),
                Left = 20,
                Top = 20,
                Width = 350
            };

            Label lblNombre = new Label() { Text = "Nombre:", Left = 20, Top = 60, Width = 80 };
            txtNombre = new TextBox() { Left = 110, Top = 60, Width = 200 };

            Label lblDescripcion = new Label() { Text = "Descripción:", Left = 20, Top = 100, Width = 80 };
            txtDescripcion = new TextBox() { Left = 110, Top = 100, Width = 200 };

            btnAgregar = new Button()
            {
                Text = "Agregar",
                Left = 110,
                Top = 140,
                Width = 200,
                Height = 35,
                BackColor = Color.FromArgb(60, 120, 200),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnAgregar.Click += BtnAgregar_Click;

            errorProvider = new ErrorProvider();

            dgvCategorias = new DataGridView()
            {
                Left = 20,
                Top = 190,
                Width = 500,
                Height = 140,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.FixedSingle,

                // Estilos para que siempre se vea bien
                BackgroundColor = Color.White,
                GridColor = Color.Silver,
                EnableHeadersVisualStyles = false
            };

            // Encabezados claros
            dgvCategorias.ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro;
            dgvCategorias.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

            // Celdas con texto negro
            dgvCategorias.DefaultCellStyle.BackColor = Color.White;
            dgvCategorias.DefaultCellStyle.ForeColor = Color.Black;
            dgvCategorias.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dgvCategorias.DefaultCellStyle.SelectionForeColor = Color.Black;

            // === Enlace de datos ===
            bsCategorias.DataSource = categorias;
            dgvCategorias.DataSource = bsCategorias;   // AutoGenerateColumns = true por defecto

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblNombre);
            this.Controls.Add(txtNombre);
            this.Controls.Add(lblDescripcion);
            this.Controls.Add(txtDescripcion);
            this.Controls.Add(btnAgregar);
            this.Controls.Add(dgvCategorias);
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
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                errorProvider.SetError(txtDescripcion, "La descripción es obligatoria.");
                valido = false;
            }
            if (!valido) return;

            // === Con BindingList, esto actualiza el grid al instante ===
            categorias.Add(new Categoria
            {
                Nombre = txtNombre.Text.Trim(),
                Descripcion = txtDescripcion.Text.Trim()
            });

            txtNombre.Clear();
            txtDescripcion.Clear();
            txtNombre.Focus();
        }

        // === CAMBIO: pública para binding más predecible ===
        public class Categoria
        {
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
        }
    }
}
