using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación.Seguridad
{
    public class SeguridadForm : Form
    {
        private string connectionString = "Server=localhost;Database=[DBFACTURAS];User Id=Alejo1234;Password=Alejo1234;";
        private ComboBox cboEmpleado;
        private Button btnAsignar, btnQuitar;
        private DataGridView dgvAsignaciones;
        private ErrorProvider errorProvider;
        private TextBox txtUsuario, txtClave;
        private string usuarioActual = "admin";

        public SeguridadForm()
        {
            Text = "Administrar Seguridad (Usuarios por Empleado)";
            BackColor = Color.White;

            var lblTitulo = new Label
            {
                Text = "ASIGNACIÓN DE USUARIO A EMPLEADO",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 120, 200),
                Left = 20, Top = 20, Width = 460
            };

            var lblEmpleado = new Label { Text = "Empleado:", Left = 20, Top = 60, Width = 80 };
            cboEmpleado = new ComboBox { Left = 110, Top = 55, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblUsuario = new Label { Text = "Usuario:", Left = 20, Top = 100, Width = 80 };
            txtUsuario = new TextBox { Left = 110, Top = 95, Width = 220 };

            var lblClave = new Label { Text = "Clave:", Left = 20, Top = 140, Width = 80 };
            txtClave = new TextBox { Left = 110, Top = 135, Width = 220, PasswordChar = '●' };

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

            dgvAsignaciones = new DataGridView
            {
                Left = 20, Top = 180, Width = 560, Height = 170,
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
            dgvAsignaciones.CellClick += DgvAsignaciones_CellClick;

            errorProvider = new ErrorProvider();

            Controls.AddRange(new Control[] {
                lblTitulo,lblEmpleado,cboEmpleado,lblUsuario,txtUsuario,lblClave,txtClave,btnAsignar,btnQuitar,dgvAsignaciones
            });

            Load += (s, e) => { CargarEmpleados(); CargarAsignaciones(); };
        }

        private void CargarEmpleados()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var da = new SqlDataAdapter("SELECT IdEmpleado, StrNombre FROM TBLEMPLEADO", conn);
                var dt = new DataTable();
                da.Fill(dt);
                cboEmpleado.DataSource = dt;
                cboEmpleado.DisplayMember = "StrNombre";
                cboEmpleado.ValueMember = "IdEmpleado";
            }
        }

        private void CargarAsignaciones()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var da = new SqlDataAdapter(
                    @"SELECT S.IdSeguridad, E.StrNombre AS Empleado, S.StrUsuario, S.StrClave, S.DtmFechaModifica, S.StrUsuarioModifico
                      FROM TBLSEGURIDAD S
                      JOIN TBLEMPLEADO E ON S.IdEmpleado = E.IdEmpleado", conn);
                var dt = new DataTable();
                da.Fill(dt);
                dgvAsignaciones.DataSource = dt;
            }
        }

        private void BtnAsignar_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            if (cboEmpleado.SelectedValue == null || string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtClave.Text))
            {
                errorProvider.SetError(cboEmpleado, "Seleccione empleado y complete usuario/clave.");
                return;
            }
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    @"INSERT INTO TBLSEGURIDAD (IdEmpleado, StrUsuario, StrClave, DtmFechaModifica, StrUsuarioModifico)
                      VALUES (@idEmpleado, @usuario, @clave, @fecha, @modifico)", conn);
                cmd.Parameters.AddWithValue("@idEmpleado", cboEmpleado.SelectedValue);
                cmd.Parameters.AddWithValue("@usuario", txtUsuario.Text.Trim());
                cmd.Parameters.AddWithValue("@clave", txtClave.Text.Trim());
                cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                cmd.Parameters.AddWithValue("@modifico", usuarioActual);
                cmd.ExecuteNonQuery();
            }
            CargarAsignaciones();
        }

        private void BtnQuitar_Click(object sender, EventArgs e)
        {
            if (dgvAsignaciones.SelectedRows.Count == 0) return;
            var id = dgvAsignaciones.SelectedRows[0].Cells["IdSeguridad"].Value.ToString();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM TBLSEGURIDAD WHERE IdSeguridad=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            CargarAsignaciones();
        }

        private void DgvAsignaciones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Puedes mostrar detalles si lo necesitas
        }
    }
}
