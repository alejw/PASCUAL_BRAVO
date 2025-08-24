using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación.Seguridad
{
    public class EmpleadosForm : Form
    {
        private TextBox txtNombre;
        private TextBox txtDocumento;
        private TextBox txtCargo;
        private Button btnAgregar, btnActualizar, btnEliminar, btnLimpiar;
        private ErrorProvider errorProvider;
        private DataGridView dgvEmpleados;

        private BindingList<Empleado> empleados = new BindingList<Empleado>();
        private BindingSource bs = new BindingSource();
        private Empleado seleccionado;

        public EmpleadosForm()
        {
            Text = "Administración de Empleados";
            BackColor = Color.White;

            var lblTitulo = new Label
            {
                Text = "ADMINISTRACIÓN DE EMPLEADOS",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 120, 200),
                Left = 20, Top = 20, Width = 400
            };

            var lblNombre = new Label { Text = "Nombre:", Left = 20, Top = 60, Width = 80 };
            txtNombre = new TextBox { Left = 110, Top = 60, Width = 220 };

            var lblDocumento = new Label { Text = "Documento:", Left = 20, Top = 100, Width = 80 };
            txtDocumento = new TextBox { Left = 110, Top = 100, Width = 220 };

            var lblCargo = new Label { Text = "Cargo:", Left = 20, Top = 140, Width = 80 };
            txtCargo = new TextBox { Left = 110, Top = 140, Width = 220 };

            btnAgregar = CrearBtn("Agregar", 350, 60);
            btnActualizar = CrearBtn("Actualizar", 350, 100);
            btnEliminar = CrearBtn("Eliminar", 350, 140);
            btnLimpiar = CrearBtn("Limpiar", 350, 180);

            btnAgregar.Click += (_, __) => { if (Validar()) { empleados.Add(new Empleado { Nombre = txtNombre.Text.Trim(), Documento = txtDocumento.Text.Trim(), Cargo = txtCargo.Text.Trim() }); Limpiar(); } };
            btnActualizar.Click += (_, __) =>
            {
                if (!Validar()) return;
                if (seleccionado == null) { MessageBox.Show("Seleccione un empleado de la lista.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                seleccionado.Nombre = txtNombre.Text.Trim();
                seleccionado.Documento = txtDocumento.Text.Trim();
                seleccionado.Cargo = txtCargo.Text.Trim();
                bs.ResetCurrentItem();
                Limpiar();
            };
            btnEliminar.Click += (_, __) =>
            {
                if (seleccionado == null) { MessageBox.Show("Seleccione un empleado para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                empleados.Remove(seleccionado);
                Limpiar();
            };
            btnLimpiar.Click += (_, __) => Limpiar();

            errorProvider = new ErrorProvider();

            dgvEmpleados = new DataGridView
            {
                Left = 20, Top = 230, Width = 560, Height = 160,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                BackgroundColor = Color.White,
                GridColor = Color.Silver,
                EnableHeadersVisualStyles = false
            };
            dgvEmpleados.ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro;
            dgvEmpleados.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvEmpleados.DefaultCellStyle.BackColor = Color.White;
            dgvEmpleados.DefaultCellStyle.ForeColor = Color.Black;
            dgvEmpleados.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dgvEmpleados.DefaultCellStyle.SelectionForeColor = Color.Black;

            bs.DataSource = empleados;
            dgvEmpleados.DataSource = bs;
            dgvEmpleados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmpleados.MultiSelect = false;
            dgvEmpleados.CellClick += (_, e) =>
            {
                if (e.RowIndex < 0) return;
                seleccionado = (Empleado)bs[e.RowIndex];
                txtNombre.Text = seleccionado.Nombre;
                txtDocumento.Text = seleccionado.Documento;
                txtCargo.Text = seleccionado.Cargo;
            };

            Controls.AddRange(new Control[] {
                lblTitulo,lblNombre,txtNombre,lblDocumento,txtDocumento,lblCargo,txtCargo,
                btnAgregar,btnActualizar,btnEliminar,btnLimpiar,dgvEmpleados
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
            if (string.IsNullOrWhiteSpace(txtDocumento.Text)) { errorProvider.SetError(txtDocumento, "El documento es obligatorio."); ok = false; }
            if (string.IsNullOrWhiteSpace(txtCargo.Text)) { errorProvider.SetError(txtCargo, "El cargo es obligatorio."); ok = false; }
            return ok;
        }

        private void Limpiar()
        {
            txtNombre.Clear(); txtDocumento.Clear(); txtCargo.Clear();
            seleccionado = null;
            txtNombre.Focus();
        }

        public class Empleado
        {
            public string Nombre { get; set; }
            public string Documento { get; set; }
            public string Cargo { get; set; }
        }
    }
}
