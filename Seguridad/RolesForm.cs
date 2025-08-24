using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación.Seguridad
{
    public class RolesForm : Form
    {
        private TextBox txtNombre, txtDescripcion;
        private Button btnAgregar, btnActualizar, btnEliminar, btnLimpiar;
        private ErrorProvider errorProvider;
        private DataGridView dgvRoles;

        private BindingList<Rol> roles = new BindingList<Rol>();
        private BindingSource bs = new BindingSource();
        private Rol seleccionado;

        public RolesForm()
        {
            Text = "Administración de Roles";
            BackColor = Color.White;

            var lblTitulo = new Label
            {
                Text = "ADMINISTRACIÓN DE ROLES",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 120, 200),
                Left = 20, Top = 20, Width = 320
            };

            var lblNombre = new Label { Text = "Nombre:", Left = 20, Top = 60, Width = 80 };
            txtNombre = new TextBox { Left = 110, Top = 60, Width = 220 };

            var lblDescripcion = new Label { Text = "Descripción:", Left = 20, Top = 100, Width = 80 };
            txtDescripcion = new TextBox { Left = 110, Top = 100, Width = 220, Multiline = true, Height = 60 };

            btnAgregar = CrearBtn("Agregar", 350, 60);
            btnActualizar = CrearBtn("Actualizar", 350, 100);
            btnEliminar = CrearBtn("Eliminar", 350, 140);
            btnLimpiar = CrearBtn("Limpiar", 350, 180);

            btnAgregar.Click += (_, __) => { if (Validar()) { roles.Add(new Rol { Nombre = txtNombre.Text.Trim(), Descripcion = txtDescripcion.Text.Trim() }); Limpiar(); } };
            btnActualizar.Click += (_, __) =>
            {
                if (!Validar()) return;
                if (seleccionado == null) { MessageBox.Show("Seleccione un rol de la lista.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                seleccionado.Nombre = txtNombre.Text.Trim();
                seleccionado.Descripcion = txtDescripcion.Text.Trim();
                bs.ResetCurrentItem();
                Limpiar();
            };
            btnEliminar.Click += (_, __) =>
            {
                if (seleccionado == null) { MessageBox.Show("Seleccione un rol para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                roles.Remove(seleccionado);
                Limpiar();
            };
            btnLimpiar.Click += (_, __) => Limpiar();

            errorProvider = new ErrorProvider();

            dgvRoles = new DataGridView
            {
                Left = 20, Top = 230, Width = 560, Height = 160,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                BackgroundColor = Color.White,
                GridColor = Color.Silver,
                EnableHeadersVisualStyles = false
            };
            dgvRoles.ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro;
            dgvRoles.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvRoles.DefaultCellStyle.BackColor = Color.White;
            dgvRoles.DefaultCellStyle.ForeColor = Color.Black;
            dgvRoles.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dgvRoles.DefaultCellStyle.SelectionForeColor = Color.Black;

            bs.DataSource = roles;
            dgvRoles.DataSource = bs;
            dgvRoles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRoles.MultiSelect = false;
            dgvRoles.CellClick += (_, e) =>
            {
                if (e.RowIndex < 0) return;
                seleccionado = (Rol)bs[e.RowIndex];
                txtNombre.Text = seleccionado.Nombre;
                txtDescripcion.Text = seleccionado.Descripcion;
            };

            Controls.AddRange(new Control[] {
                lblTitulo,lblNombre,txtNombre,lblDescripcion,txtDescripcion,
                btnAgregar,btnActualizar,btnEliminar,btnLimpiar,dgvRoles
            });
        }

        private Button CrearBtn(string t, int x, int y) => new Button
        {
            Text = t, Left = x, Top = y, Width = 120, Height = 30,
            BackColor = Color.FromArgb(60, 120, 200), ForeColor = Color.White,
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };

        private bool Validar()
        {
            errorProvider.Clear();
            bool ok = true;
            if (string.IsNullOrWhiteSpace(txtNombre.Text)) { errorProvider.SetError(txtNombre, "El nombre es obligatorio."); ok = false; }
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text)) { errorProvider.SetError(txtDescripcion, "La descripción es obligatoria."); ok = false; }
            return ok;
        }

        private void Limpiar()
        {
            txtNombre.Clear(); txtDescripcion.Clear();
            seleccionado = null;
            txtNombre.Focus();
        }

        public class Rol
        {
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
        }
    }
}
