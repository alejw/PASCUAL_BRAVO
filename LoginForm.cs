using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación
{
    public class LoginForm : Form
    {
        private TextBox txtUsuario;
        private TextBox txtContraseña;
        private Button btnLogin;
        private Button btnSalir;
        private Label lblUsuario;
        private Label lblContraseña;
        private Panel card;
        private ErrorProvider errorProvider;
        private Button btnEye;
        private Timer revealTimer;
        private bool eyeUsedOnce = false;

        public LoginForm()
        {
            // Ventana
            Text = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(420, 300);
            BackColor = Color.FromArgb(38, 41, 45); // fondo oscuro

            // Tarjeta central
            card = new Panel
            {
                Width = 360,
                Height = 220,
                BackColor = Color.FromArgb(45, 48, 52),
                Left = (ClientSize.Width - 360) / 2,
                Top = (ClientSize.Height - 220) / 2,
                Padding = new Padding(20)
            };
            Controls.Add(card);

            var lblTitulo = new Label
            {
                Text = "Inicio de sesión",
                Dock = DockStyle.Top,
                Height = 34,
                Font = new Font("Segoe UI Semibold", 14f),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(lblTitulo);

            // Usuario
            lblUsuario = new Label { Text = "Usuario", Left = 10, Top = 56, Width = 90, ForeColor = Color.Gainsboro };
            txtUsuario = new TextBox
            {
                Left = 110, Top = 52, Width = 220,
                BackColor = Color.FromArgb(60, 63, 65),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Contraseña
            lblContraseña = new Label { Text = "Contraseña", Left = 10, Top = 100, Width = 90, ForeColor = Color.Gainsboro };
            txtContraseña = new TextBox
            {
                Left = 110, Top = 96, Width = 220,
                UseSystemPasswordChar = true,
                BackColor = Color.FromArgb(60, 63, 65),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Botón ojo (👁) para ver clave una sola vez por 2 segundos
            btnEye = new Button
            {
                Left = 335, Top = 95, Width = 24, Height = 24,
                FlatStyle = FlatStyle.Flat,
                Text = "👁",
                Font = new Font(Font.FontFamily, 10f),
                BackColor = Color.FromArgb(60, 63, 65),
                ForeColor = Color.White,
                TabStop = false
            };
            btnEye.FlatAppearance.BorderSize = 0;
            btnEye.Click += (s, e) => RevealPasswordOnce();

            // Botones
            btnLogin = new Button
            {
                Text = "Ingresar",
                Left = 110, Top = 150, Width = 110, Height = 36,
                BackColor = Color.FromArgb(57, 121, 199),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10f)
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            btnSalir = new Button
            {
                Text = "Salir",
                Left = 230, Top = 150, Width = 100, Height = 36,
                BackColor = Color.FromArgb(80, 83, 88),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSalir.FlatAppearance.BorderSize = 0;
            btnSalir.Click += (s, e) => Close();

            // ErrorProvider
            errorProvider = new ErrorProvider { BlinkStyle = ErrorBlinkStyle.NeverBlink };

            // Timer para ocultar la contraseña tras mostrarla temporalmente
            revealTimer = new Timer { Interval = 2000 }; // 2 segundos
            revealTimer.Tick += (s, e) =>
            {
                revealTimer.Stop();
                txtContraseña.UseSystemPasswordChar = true;
                btnEye.Enabled = false;            // deshabilitar ojo tras el primer uso
                btnEye.Text = "✓";                 // marca visual de que ya se usó
                btnEye.BackColor = Color.FromArgb(70, 73, 75);
                btnEye.ForeColor = Color.Gainsboro;
            };

            // Añadir controles a la tarjeta
            card.Controls.Add(lblUsuario);
            card.Controls.Add(txtUsuario);
            card.Controls.Add(lblContraseña);
            card.Controls.Add(txtContraseña);
            card.Controls.Add(btnEye);
            card.Controls.Add(btnLogin);
            card.Controls.Add(btnSalir);

            // Comodidades
            AcceptButton = btnLogin; // Enter
            CancelButton = btnSalir; // Esc
        }

        private void RevealPasswordOnce()
        {
            if (eyeUsedOnce) return;
            eyeUsedOnce = true;

            // Mostrar texto plano por 2 segundos
            txtContraseña.UseSystemPasswordChar = false;
            revealTimer.Start();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            // Validación con ErrorProvider
            errorProvider.Clear();
            bool ok = true;

            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                errorProvider.SetError(txtUsuario, "El usuario es obligatorio.");
                ok = false;
            }
            if (string.IsNullOrWhiteSpace(txtContraseña.Text))
            {
                errorProvider.SetError(txtContraseña, "La contraseña es obligatoria.");
                ok = false;
            }
            if (!ok) return;

            // Demo: credenciales fijas
            if (txtUsuario.Text.Trim().Equals("admin", StringComparison.OrdinalIgnoreCase)
                && txtContraseña.Text == "admin")
            {
                Hide();
                using (var main = new MainForm())
                {
                    main.ShowDialog(this);
                }
                Close();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContraseña.SelectAll();
                txtContraseña.Focus();
            }
        }
    }
}
