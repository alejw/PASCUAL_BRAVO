using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación.Ayuda
{
    public class AcercaDeForm : Form
    {
        public AcercaDeForm()
        {
            Text = "Acerca de";
            BackColor = Color.White;

            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                BackColor = Color.White
            };

            var lblTitulo = new Label
            {
                Text = "Sistema de Facturación",
                Font = new Font("Segoe UI Semibold", 16),
                ForeColor = Color.FromArgb(60, 120, 200),
                Dock = DockStyle.Top,
                Height = 40
            };

            var lblVersion = new Label
            {
                Text = "Versión: 1.0.0",
                Font = new Font("Segoe UI", 10),
                Dock = DockStyle.Top,
                Height = 24
            };

            var lblEmpresa = new Label
            {
                Text = "Empresa: Tu Empresa S.A.S.",
                Font = new Font("Segoe UI", 10),
                Dock = DockStyle.Top,
                Height = 24
            };

            var lblDescripcion = new Label
            {
                Text = "Descripción:",
                Font = new Font("Segoe UI Semibold", 10),
                Dock = DockStyle.Top,
                Height = 24
            };

            var txtDescripcion = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Top,
                Height = 120,
                ReadOnly = true,
                Text = "Aplicación de escritorio de ejemplo para gestión de facturación.\r\n" +
                       "Incluye módulos de Tablas, Facturación, Seguridad y Ayuda.\r\n" +
                       "Este formulario se puede personalizar con más información."
            };

            var btnSitio = new Button
            {
                Text = "Sitio web",
                Width = 100,
                Height = 32,
                Left = 16,
                Top = 0,
                BackColor = Color.FromArgb(60, 120, 200),
                ForeColor = Color.White
            };
            btnSitio.Click += (_, __) => System.Diagnostics.Process.Start("https://www.microsoft.com");

            var btnCerrar = new Button
            {
                Text = "Cerrar",
                Width = 100,
                Height = 32,
                Left = 130,
                Top = 0,
                BackColor = Color.Gray,
                ForeColor = Color.White
            };
            btnCerrar.Click += (_, __) => this.Parent?.Controls.Remove(this);

            var acciones = new Panel { Dock = DockStyle.Top, Height = 40 };
            acciones.Controls.Add(btnSitio);
            acciones.Controls.Add(btnCerrar);

            panel.Controls.Add(acciones);
            panel.Controls.Add(txtDescripcion);
            panel.Controls.Add(lblDescripcion);
            panel.Controls.Add(lblEmpresa);
            panel.Controls.Add(lblVersion);
            panel.Controls.Add(lblTitulo);

            Controls.Add(panel);
        }
    }
}
