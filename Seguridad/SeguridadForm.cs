using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación.Seguridad
{
    public class SeguridadForm : Form
    {
        private ComboBox cboEmpleado, cboRol;
        private Button btnAsignar, btnQuitar;
        private ErrorProvider errorProvider;
        private DataGridView dgvAsignaciones;

        // Listas simuladas (se pueden llenar desde BD futuro)
        private BindingList<Empleado> empleados = new BindingList<Empleado>();
        private BindingList<Rol> roles = new BindingList<Rol>();
        private BindingList<Asignacion> asignaciones = new BindingList<Asignacion>();

        private BindingSource bsAsig = new BindingSource();

        public SeguridadForm()
        {
            Text = "Administrar Seguridad (Roles por Empleado)";
            BackColor = Color.White;

            var lblTitulo = new Label
            {
                Text = "ASIGNACIÓN DE ROLES A EMPLEADOS",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 120, 200),
                Left = 20, Top = 20, Width = 460
            };

            var lblEmpleado = new Label { Text = "Empleado:", Left = 20, Top = 60, Width = 80 };
            cboEmpleado = new ComboBox { Left = 110, Top = 55, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblRol = new Label { Text = "Rol:", Left = 20, Top = 100, Width = 80 };
            cboRol = new ComboBox { Left = 110, Top = 95, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };

            btnAsignar = new Button
            {
                Text = "Asignar", Left = 350, Top = 55, Width = 120, Height = 30,
                BackColor = Color.FromArgb(60, 120, 200), ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnQuitar = new Button
            {
                Text = "Quitar selección", Left = 350, Top = 95, Width = 120, Height = 30,
                BackColor = Color.FromArgb(150, 50, 50), ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnAsignar.Click += BtnAsignar_Click;
            btnQuitar.Click += BtnQuitar_Click;

            errorProvider = new ErrorProvider();

            dgvAsignaciones = new DataGridView
            {
                Left = 20, Top = 150, Width = 560, Height = 240,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                BackgroundColor = Color.White,
                GridColor = Color.Silver,
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            dgvAsignaciones.ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro;
            dgvAsignaciones.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvAsignaciones.DefaultCellStyle.BackColor = Color.White;
            dgvAsignaciones.DefaultCellStyle.ForeColor = Color.Black;
            dgvAsignaciones.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dgvAsignaciones.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Datos de ejemplo (simulados)
            empleados.Add(new Empleado { Id = 1, Nombre = "Ana Ruiz" });
            empleados.Add(new Empleado { Id = 2, Nombre = "Carlos Pérez" });
            roles.Add(new Rol { Id = 1, Nombre = "Administrador" });
            roles.Add(new Rol { Id = 2, Nombre = "Cajero" });
            roles.Add(new Rol { Id = 3, Nombre = "Consulta" });

            cboEmpleado.DataSource = empleados;
            cboEmpleado.DisplayMember = nameof(Empleado.Nombre);
            cboEmpleado.ValueMember = nameof(Empleado.Id);

            cboRol.DataSource = roles;
            cboRol.DisplayMember = nameof(Rol.Nombre);
            cboRol.ValueMember = nameof(Rol.Id);

            bsAsig.DataSource = asignaciones;
            dgvAsignaciones.DataSource = bsAsig;

            Controls.AddRange(new Control[] {
                lblTitulo,lblEmpleado,cboEmpleado,lblRol,cboRol,btnAsignar,btnQuitar,dgvAsignaciones
            });
        }

        private void BtnAsignar_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            if (cboEmpleado.SelectedItem == null) { errorProvider.SetError(cboEmpleado, "Seleccione un empleado."); return; }
            if (cboRol.SelectedItem == null) { errorProvider.SetError(cboRol, "Seleccione un rol."); return; }

            var emp = (Empleado)cboEmpleado.SelectedItem;
            var rol = (Rol)cboRol.SelectedItem;

            // Evitar duplicados
            if (asignaciones.Any(a => a.EmpleadoId == emp.Id && a.RolId == rol.Id))
            {
                MessageBox.Show("Este empleado ya tiene ese rol asignado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            asignaciones.Add(new Asignacion
            {
                EmpleadoId = emp.Id,
                Empleado = emp.Nombre,
                RolId = rol.Id,
                Rol = rol.Nombre
            });
        }

        private void BtnQuitar_Click(object sender, EventArgs e)
        {
            if (dgvAsignaciones.CurrentRow == null) { MessageBox.Show("Seleccione una fila.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            var item = (Asignacion)bsAsig[dgvAsignaciones.CurrentRow.Index];
            asignaciones.Remove(item);
        }

        public class Empleado
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }
        public class Rol
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }
        public class Asignacion
        {
            public int EmpleadoId { get; set; }
            public string Empleado { get; set; }
            public int RolId { get; set; }
            public string Rol { get; set; }
        }
    }
}
