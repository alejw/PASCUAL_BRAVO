using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación.Seguridad
{
    public class RolesForm : Form
    {
        private string connectionString = "Server=localhost;Database=[DBFACTURAS];User Id=Alejo1234;Password=Alejo1234;";
        private DataGridView dgvRoles;
        private TextBox txtDescripcion;
        private Button btnAgregar, btnEditar, btnEliminar;
        private ErrorProvider errorProvider;

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

            var lblDescripcion = new Label { Text = "Descripción:", Left = 20, Top = 60, Width = 80 };
            txtDescripcion = new TextBox { Left = 110, Top = 60, Width = 220, Multiline = true, Height = 60 };

            btnAgregar = CrearBtn("Agregar", 350, 60);
            btnEditar = CrearBtn("Editar", 350, 100);
            btnEliminar = CrearBtn("Eliminar", 350, 140);

            btnAgregar.Click += BtnAgregar_Click;
            btnEditar.Click += BtnEditar_Click;
            btnEliminar.Click += BtnEliminar_Click;

            dgvRoles = new DataGridView
            {
                Left = 20, Top = 130, Width = 560, Height = 160,
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
            dgvRoles.CellClick += DgvRoles_CellClick;

            errorProvider = new ErrorProvider();

            Controls.AddRange(new Control[] {
                lblTitulo,lblDescripcion,txtDescripcion,
                btnAgregar,btnEditar,btnEliminar,dgvRoles
            });

            Load += (s, e) => CargarRoles();
        }

        private Button CrearBtn(string t, int x, int y) => new Button
        {
            Text = t, Left = x, Top = y, Width = 120, Height = 30,
            BackColor = Color.FromArgb(60, 120, 200), ForeColor = Color.White,
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };

        private void CargarRoles()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var da = new SqlDataAdapter("SELECT IdRolEmpleado, StrDescripcion FROM TBLROLES", conn);
                var dt = new DataTable();
                da.Fill(dt);
                dgvRoles.DataSource = dt;
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                errorProvider.SetError(txtDescripcion, "La descripción es obligatoria.");
                return;
            }
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO TBLROLES (StrDescripcion) VALUES (@desc)", conn);
                cmd.Parameters.AddWithValue("@desc", txtDescripcion.Text.Trim());
                cmd.ExecuteNonQuery();
            }
            CargarRoles();
            LimpiarCampos();
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvRoles.SelectedRows.Count == 0) return;
            var id = dgvRoles.SelectedRows[0].Cells["IdRolEmpleado"].Value.ToString();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "UPDATE TBLROLES SET StrDescripcion=@desc WHERE IdRolEmpleado=@id", conn);
                cmd.Parameters.AddWithValue("@desc", txtDescripcion.Text.Trim());
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            CargarRoles();
            LimpiarCampos();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvRoles.SelectedRows.Count == 0) return;
            var id = dgvRoles.SelectedRows[0].Cells["IdRolEmpleado"].Value.ToString();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM TBLROLES WHERE IdRolEmpleado=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            CargarRoles();
            LimpiarCampos();
        }

        private void DgvRoles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvRoles.Rows[e.RowIndex];
            txtDescripcion.Text = row.Cells["StrDescripcion"].Value?.ToString();
        }

        private void LimpiarCampos()
        {
            txtDescripcion.Clear();
            txtDescripcion.Focus();
        }
    }
}
