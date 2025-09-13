using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace Pantallas_Sistema_facturación.Tablas
{
    public class ClientesForm : Form
    {
        private string connectionString = "Server=localhost;Database=[DBFACTURAS];User Id=Alejo1234;Password=Alejo1234;";
        private DataGridView dgvClientes;
        private TextBox txtNombre, txtDocumento, txtDireccion, txtTelefono, txtEmail;
        private Button btnAgregar, btnEditar, btnEliminar;
        private ErrorProvider errorProvider;
        private string usuarioActual = "admin"; // Cambia esto por el usuario logueado

        public ClientesForm()
        {
            this.Text = "Administración de Clientes";
            this.BackColor = Color.White;
            this.Width = 900;
            this.Height = 500;

            Label lblTitulo = new Label()
            {
                Text = "ADMINISTRACIÓN DE CLIENTES",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 120, 200),
                Left = 20,
                Top = 20,
                Width = 350
            };

            Label lblNombre = new Label() { Text = "Nombre:", Left = 20, Top = 60, Width = 80, ForeColor = Color.Black };
            txtNombre = new TextBox() { Left = 110, Top = 60, Width = 200, ForeColor = Color.Black };

            Label lblDocumento = new Label() { Text = "Documento:", Left = 20, Top = 100, Width = 80, ForeColor = Color.Black };
            txtDocumento = new TextBox() { Left = 110, Top = 100, Width = 200, ForeColor = Color.Black };

            Label lblDireccion = new Label() { Text = "Dirección:", Left = 20, Top = 140, Width = 80, ForeColor = Color.Black };
            txtDireccion = new TextBox() { Left = 110, Top = 140, Width = 200, ForeColor = Color.Black };

            Label lblTelefono = new Label() { Text = "Teléfono:", Left = 20, Top = 180, Width = 80, ForeColor = Color.Black };
            txtTelefono = new TextBox() { Left = 110, Top = 180, Width = 200, ForeColor = Color.Black };

            Label lblEmail = new Label() { Text = "Email:", Left = 20, Top = 220, Width = 80, ForeColor = Color.Black };
            txtEmail = new TextBox() { Left = 110, Top = 220, Width = 200, ForeColor = Color.Black };

            btnAgregar = new Button()
            {
                Text = "Agregar",
                Left = 110,
                Top = 340,
                Width = 90,
                BackColor = Color.FromArgb(60, 120, 200),
                ForeColor = Color.White
            };
            btnAgregar.Click += BtnAgregar_Click;

            btnEditar = new Button()
            {
                Text = "Editar",
                Left = 210,
                Top = 340,
                Width = 90,
                BackColor = Color.FromArgb(60, 120, 200),
                ForeColor = Color.White
            };
            btnEditar.Click += BtnEditar_Click;

            btnEliminar = new Button()
            {
                Text = "Eliminar",
                Left = 310,
                Top = 340,
                Width = 90,
                BackColor = Color.FromArgb(200, 60, 60),
                ForeColor = Color.White
            };
            btnEliminar.Click += BtnEliminar_Click;

            // 🔹 Configuración del DataGridView con estilos
            dgvClientes = new DataGridView()
            {
                Left = 350,
                Top = 60,
                Width = 500,
                Height = 300,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.Black,
                    BackColor = Color.White,
                    SelectionBackColor = Color.LightBlue,
                    SelectionForeColor = Color.Black
                },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle()
                {
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(60, 120, 200),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                EnableHeadersVisualStyles = false
            };
            dgvClientes.CellClick += DgvClientes_CellClick;

            errorProvider = new ErrorProvider();

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblNombre);
            this.Controls.Add(txtNombre);
            this.Controls.Add(lblDocumento);
            this.Controls.Add(txtDocumento);
            this.Controls.Add(lblDireccion);
            this.Controls.Add(txtDireccion);
            this.Controls.Add(lblTelefono);
            this.Controls.Add(txtTelefono);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(btnAgregar);
            this.Controls.Add(btnEditar);
            this.Controls.Add(btnEliminar);
            this.Controls.Add(dgvClientes);

            Load += (s, e) => CargarClientes();
        }

        // Modifica el SELECT para traer todos los campos
        private void CargarClientes()
        {
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            using (var conn = new SqlConnection(connectionString))
            {
                var da = new SqlDataAdapter("SELECT IdCliente, StrNombre, NumDocumento, StrDireccion, StrTelefono, StrEmail, DtmFechaModifica, StrUsuarioModifica FROM TBLCLIENTES", conn);
                var dt = new DataTable();
                da.Fill(dt);
                dgvClientes.DataSource = dt;
            }
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos

            if (dgvClientes.Rows.Count > 0)
            {
                dgvClientes.Rows[0].Selected = true;
                DgvClientes_CellClick(this, new DataGridViewCellEventArgs(0, 0));
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtDocumento.Text))
            {
                errorProvider.SetError(txtNombre, "Nombre y Documento son obligatorios.");
                return;
            }
            if (!long.TryParse(txtDocumento.Text.Trim(), out _))
            {
                errorProvider.SetError(txtDocumento, "El documento debe ser un número.");
                return;
            }

#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
                var cmd = new SqlCommand(
                    "INSERT INTO TBLCLIENTES (StrNombre, NumDocumento, StrDireccion, StrTelefono, StrEmail, DtmFechaModifica, StrUsuarioModifica) " +
                    "VALUES (@nombre, @doc, @dir, @tel, @email, @fecha, @usuario)", conn);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
                cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                cmd.Parameters.AddWithValue("@doc", txtDocumento.Text.Trim());
                cmd.Parameters.AddWithValue("@dir", txtDireccion.Text.Trim());
                cmd.Parameters.AddWithValue("@tel", txtTelefono.Text.Trim());
                cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                cmd.Parameters.AddWithValue("@usuario", usuarioActual);
                cmd.ExecuteNonQuery();
            }
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
            CargarClientes();
            LimpiarCampos();
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count == 0) return;
            var id = dgvClientes.SelectedRows[0].Cells["IdCliente"].Value.ToString();

#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "UPDATE TBLCLIENTES SET StrNombre=@nombre, NumDocumento=@doc, StrDireccion=@dir, StrTelefono=@tel, StrEmail=@email, DtmFechaModifica=@fecha, StrUsuarioModifica=@usuario WHERE IdCliente=@id", conn);
                cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                cmd.Parameters.AddWithValue("@doc", txtDocumento.Text.Trim());
                cmd.Parameters.AddWithValue("@dir", txtDireccion.Text.Trim());
                cmd.Parameters.AddWithValue("@tel", txtTelefono.Text.Trim());
                cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                cmd.Parameters.AddWithValue("@usuario", usuarioActual);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
            CargarClientes();
            LimpiarCampos();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count == 0) return;
            var id = dgvClientes.SelectedRows[0].Cells["IdCliente"].Value.ToString();

#pragma warning disable CS0618
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM TBLCLIENTES WHERE IdCliente=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
#pragma warning restore CS0618
            CargarClientes();
            LimpiarCampos();
        }


        private void DgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvClientes.Rows[e.RowIndex];
            txtNombre.Text = row.Cells["StrNombre"].Value?.ToString();
            txtDocumento.Text = row.Cells["NumDocumento"].Value?.ToString();
            txtDireccion.Text = row.Cells["StrDireccion"].Value?.ToString();
            txtTelefono.Text = row.Cells["StrTelefono"].Value?.ToString();
            txtEmail.Text = row.Cells["StrEmail"].Value?.ToString();
        }


        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDocumento.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtNombre.Focus();
        }
    }
}
