using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación.Seguridad
{
    public class EmpleadosForm : Form
    {
        private string connectionString = "Server=localhost;Database=[DBFACTURAS];User Id=Alejo1234;Password=Alejo1234;";
        private DataGridView dgvEmpleados;
        private TextBox txtNombre, txtDocumento, txtDireccion, txtTelefono, txtEmail, txtDatosAdicionales;
        private Button btnAgregar, btnEditar, btnEliminar;
        private ErrorProvider errorProvider;
        private ComboBox cboRol;
        private DateTimePicker dtpIngreso, dtpRetiro;
        private string usuarioActual = "admin"; // Suponiendo un usuario por defecto, cambiar según necesidad

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

            var lblDireccion = new Label { Text = "Dirección:", Left = 20, Top = 140, Width = 80 };
            txtDireccion = new TextBox { Left = 110, Top = 140, Width = 220 };

            var lblTelefono = new Label { Text = "Teléfono:", Left = 20, Top = 180, Width = 80 };
            txtTelefono = new TextBox { Left = 110, Top = 180, Width = 220 };

            var lblEmail = new Label { Text = "Email:", Left = 20, Top = 220, Width = 80 };
            txtEmail = new TextBox { Left = 110, Top = 220, Width = 220 };

            var lblRol = new Label { Text = "Rol:", Left = 20, Top = 260, Width = 80 };
            cboRol = new ComboBox { Left = 110, Top = 260, Width = 220 };
            cboRol.DisplayMember = "StrNombreRol";
            cboRol.ValueMember = "IdRolEmpleado";

            var lblIngreso = new Label { Text = "Fecha Ingreso:", Left = 20, Top = 300, Width = 100 };
            dtpIngreso = new DateTimePicker { Left = 130, Top = 300, Width = 200 };

            var lblRetiro = new Label { Text = "Fecha Retiro:", Left = 20, Top = 340, Width = 100 };
            dtpRetiro = new DateTimePicker { Left = 130, Top = 340, Width = 200 };

            txtDatosAdicionales = new TextBox { Left = 110, Top = 380, Width = 220, Height = 60, Multiline = true };
            var lblDatosAdicionales = new Label { Text = "Datos Adicionales:", Left = 20, Top = 380, Width = 110 };

            btnAgregar = CrearBtn("Agregar", 350, 60);
            btnEditar = CrearBtn("Editar", 350, 100);
            btnEliminar = CrearBtn("Eliminar", 350, 140);

            btnAgregar.Click += BtnAgregar_Click;
            btnEditar.Click += BtnEditar_Click;
            btnEliminar.Click += BtnEliminar_Click;

            var panelTabla = new Panel
            {
                Left = 20,
                Top = 450,
                Width = 560,
                Height = 230,
                AutoScroll = true
            };
            dgvEmpleados = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
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
            dgvEmpleados.CellClick += DgvEmpleados_CellClick;
            panelTabla.Controls.Add(dgvEmpleados);

            errorProvider = new ErrorProvider();

            Controls.AddRange(new Control[] {
                lblTitulo,lblNombre,txtNombre,lblDocumento,txtDocumento,
                lblDireccion,txtDireccion,lblTelefono,txtTelefono,
                lblEmail,txtEmail,lblRol,cboRol,
                lblIngreso,dtpIngreso,lblRetiro,dtpRetiro,
                lblDatosAdicionales,txtDatosAdicionales,
                btnAgregar,btnEditar,btnEliminar
            });
            Controls.Add(panelTabla);

            Load += (s, e) => {
                CargarEmpleados();
                CargarRoles();
            };

            txtDocumento.KeyPress += SoloNumeros_KeyPress;
            txtTelefono.KeyPress += SoloNumeros_KeyPress;
        }

        private Button CrearBtn(string t, int x, int y) => new Button
        {
            Text = t, Left = x, Top = y, Width = 120, Height = 30,
            BackColor = Color.FromArgb(60, 120, 200), ForeColor = Color.White,
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };

        private void CargarEmpleados()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var da = new SqlDataAdapter(
                    @"SELECT 
                        IdEmpleado,
                        StrNombre,
                        NumDocumento,
                        StrDireccion,
                        StrTelefono,
                        StrEmail,
                        IdRolEmpleado,
                        DtmIngreso,         
                        DtmRetiro,          
                        strDatosAdicionales,
                        DtmFechaModifica,
                        StrUsuarioModifico
                    FROM TBLEMPLEADO", conn);
                var dt = new DataTable();
                da.Fill(dt);
                dgvEmpleados.DataSource = dt;
            }
        }

        private void CargarRoles()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var da = new SqlDataAdapter("SELECT IdRolEmpleado, StrDescripcion FROM TBLROLES", conn);
                var dt = new DataTable();
                da.Fill(dt);
                cboRol.DataSource = dt;
                cboRol.DisplayMember = "StrDescripcion";
                cboRol.ValueMember = "IdRolEmpleado";
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtDocumento.Text) ||
                string.IsNullOrWhiteSpace(txtDireccion.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                cboRol.SelectedValue == null)
            {
                MessageBox.Show("Todos los campos son obligatorios.");
                return;
            }

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    @"INSERT INTO TBLEMPLEADO 
                    (StrNombre, NumDocumento, StrDireccion, StrTelefono, StrEmail, IdRolEmpleado, DtmIngreso, DtmRetiro, strDatosAdicionales, DtmFechaModifica, StrUsuarioModifico)
                    VALUES (@nombre, @doc, @dir, @tel, @email, @rol, @ingreso, @retiro, @datos, @fecha, @usuario)", conn);

                cmd.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = txtNombre.Text.Trim();
                cmd.Parameters.Add("@doc", SqlDbType.NVarChar, 50).Value = txtDocumento.Text.Trim();
                cmd.Parameters.Add("@dir", SqlDbType.NVarChar, 150).Value = txtDireccion.Text.Trim();
                cmd.Parameters.Add("@tel", SqlDbType.NVarChar, 50).Value = txtTelefono.Text.Trim();
                cmd.Parameters.Add("@email", SqlDbType.NVarChar, 100).Value = txtEmail.Text.Trim();
                cmd.Parameters.Add("@rol", SqlDbType.Int).Value = cboRol.SelectedValue;
                cmd.Parameters.Add("@ingreso", SqlDbType.DateTime).Value = dtpIngreso.Value;
                cmd.Parameters.Add("@retiro", SqlDbType.DateTime).Value = dtpRetiro.Value;
                cmd.Parameters.Add("@datos", SqlDbType.NVarChar, 250).Value = txtDatosAdicionales.Text.Trim();
                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@usuario", SqlDbType.NVarChar, 50).Value = usuarioActual;

                cmd.ExecuteNonQuery();
            }
            CargarEmpleados();
            LimpiarCampos();
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvEmpleados.SelectedRows.Count == 0) return;
            var id = dgvEmpleados.SelectedRows[0].Cells["IdEmpleado"].Value.ToString();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    @"UPDATE TBLEMPLEADO SET 
                        StrNombre=@nombre, 
                        NumDocumento=@doc, 
                        StrDireccion=@dir, 
                        StrTelefono=@tel, 
                        StrEmail=@email, 
                        IdRolEmpleado=@rol, 
                        DtmIngreso=@ingreso, 
                        DtmRetiro=@retiro, 
                        strDatosAdicionales=@datos, 
                        DtmFechaModifica=@fecha, 
                        StrUsuarioModifico=@usuario
                    WHERE IdEmpleado=@id", conn);

                cmd.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = txtNombre.Text.Trim();
                cmd.Parameters.Add("@doc", SqlDbType.NVarChar, 50).Value = txtDocumento.Text.Trim();
                cmd.Parameters.Add("@dir", SqlDbType.NVarChar, 150).Value = txtDireccion.Text.Trim();
                cmd.Parameters.Add("@tel", SqlDbType.NVarChar, 50).Value = txtTelefono.Text.Trim();
                cmd.Parameters.Add("@email", SqlDbType.NVarChar, 100).Value = txtEmail.Text.Trim();
                cmd.Parameters.Add("@rol", SqlDbType.Int).Value = cboRol.SelectedValue;
                cmd.Parameters.Add("@ingreso", SqlDbType.DateTime).Value = dtpIngreso.Value;
                cmd.Parameters.Add("@retiro", SqlDbType.DateTime).Value = dtpRetiro.Value;
                cmd.Parameters.Add("@datos", SqlDbType.NVarChar, 250).Value = txtDatosAdicionales.Text.Trim();
                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@usuario", SqlDbType.NVarChar, 50).Value = usuarioActual;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(id);

                cmd.ExecuteNonQuery();
            }
            CargarEmpleados();
            LimpiarCampos();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvEmpleados.SelectedRows.Count == 0) return;
            var id = dgvEmpleados.SelectedRows[0].Cells["IdEmpleado"].Value.ToString();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM TBLEMPLEADO WHERE IdEmpleado=@id", conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(id);
                cmd.ExecuteNonQuery();
            }
            CargarEmpleados();
            LimpiarCampos();
        }

        private void DgvEmpleados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvEmpleados.Rows[e.RowIndex];
            txtNombre.Text = row.Cells["StrNombre"].Value?.ToString();
            txtDocumento.Text = row.Cells["NumDocumento"].Value?.ToString();
            txtDireccion.Text = row.Cells["StrDireccion"].Value?.ToString();
            txtTelefono.Text = row.Cells["StrTelefono"].Value?.ToString();
            txtEmail.Text = row.Cells["StrEmail"].Value?.ToString();
            cboRol.SelectedValue = row.Cells["IdRolEmpleado"].Value;
            dtpIngreso.Value = Convert.ToDateTime(row.Cells["DtmIngreso"].Value); // <-- corregido
            dtpRetiro.Value = Convert.ToDateTime(row.Cells["DtmRetiro"].Value);   // <-- corregido
            txtDatosAdicionales.Text = row.Cells["strDatosAdicionales"].Value?.ToString();
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDocumento.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtDatosAdicionales.Clear();
            cboRol.SelectedIndex = -1;
            dtpIngreso.Value = DateTime.Now;
            dtpRetiro.Value = DateTime.Now;
            txtNombre.Focus();
        }

        private void SoloNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
    }
}
