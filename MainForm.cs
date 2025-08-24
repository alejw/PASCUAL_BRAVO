using System;
using System.Drawing;
using System.Windows.Forms;
using Pantallas_Sistema_facturación.Tablas;
using Pantallas_Sistema_facturación.Facturacion;
using Pantallas_Sistema_facturación.Seguridad;
using Pantallas_Sistema_facturación.Ayuda;

namespace Pantallas_Sistema_facturación
{
    public class MainForm : Form
    {
        private MenuStrip menuStrip;
        private ToolStripMenuItem tablasMenu;
        private ToolStripMenuItem facturacionMenu;
        private ToolStripMenuItem seguridadMenu;
        private ToolStripMenuItem ayudaMenu;
        private Panel mainPanel;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel userStatus;
        private ToolStripStatusLabel timeStatus;
        private Timer clockTimer;

        public MainForm()
        {
            // Ventana principal
            Text = "Sistema de Facturación";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1000, 650);
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Segoe UI", 10F);
            DoubleBuffered = true;
            BackColor = Color.FromArgb(38, 41, 45);  // Fondo oscuro
            ForeColor = Color.White;                 // Texto blanco

            // ====== MENU ======
            menuStrip = new MenuStrip
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI Semibold", 10.5F),
                Padding = new Padding(10, 6, 10, 6),
                Renderer = new ToolStripProfessionalRenderer(new DarkMenuColors()) { RoundedEdges = false },
                BackColor = Color.FromArgb(38, 41, 45),
                ForeColor = Color.White
            };

            // Tablas
            tablasMenu = new ToolStripMenuItem("&Tablas");
            var clientesItem   = new ToolStripMenuItem("&Clientes",   null, (s, e) => MostrarEnPanel(new ClientesForm()));
            var productosItem  = new ToolStripMenuItem("&Productos",  null, (s, e) => MostrarEnPanel(new ProductosForm()));
            var categoriasItem = new ToolStripMenuItem("Cate&gorías", null, (s, e) => MostrarEnPanel(new CategoriasForm()));
            tablasMenu.DropDownItems.AddRange(new ToolStripItem[] { clientesItem, productosItem, categoriasItem });

            // Facturación
            facturacionMenu = new ToolStripMenuItem("&Facturación");
            var facturasItem = new ToolStripMenuItem("&Facturas", null, (s, e) => MostrarEnPanel(new FacturasForm()));
            var informesItem = new ToolStripMenuItem("&Informes", null, (s, e) => MostrarEnPanel(new InformesForm()));
            facturacionMenu.DropDownItems.AddRange(new ToolStripItem[] { facturasItem, informesItem });

            // Seguridad
            seguridadMenu = new ToolStripMenuItem("&Seguridad");
            var empleadosItem = new ToolStripMenuItem("&Empleados", null, (s, e) => MostrarEnPanel(new EmpleadosForm()));
            var rolesItem     = new ToolStripMenuItem("&Roles",     null, (s, e) => MostrarEnPanel(new RolesForm()));
            var permisosItem  = new ToolStripMenuItem("&Seguridad", null, (s, e) => MostrarEnPanel(new SeguridadForm()));
            seguridadMenu.DropDownItems.AddRange(new ToolStripItem[] { empleadosItem, rolesItem, permisosItem });

            // Ayuda
            ayudaMenu = new ToolStripMenuItem("Ay&uda");
            var ayudaItem    = new ToolStripMenuItem("&Ayuda",     null, (s, e) => MostrarEnPanel(new AyudaForm()));
            var acercaDeItem = new ToolStripMenuItem("&Acerca de", null, (s, e) => MostrarEnPanel(new AcercaDeForm()));
            ayudaMenu.DropDownItems.AddRange(new ToolStripItem[] { ayudaItem, acercaDeItem });

            // >>> IMPORTANTE: Agregar los menús al MenuStrip y el MenuStrip al formulario
            menuStrip.Items.AddRange(new ToolStripItem[] { tablasMenu, facturacionMenu, seguridadMenu, ayudaMenu });
            MainMenuStrip = menuStrip;
            Controls.Add(menuStrip);

            // ====== PANEL PRINCIPAL ======
            mainPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                BackColor = Color.FromArgb(45, 48, 52),
                ForeColor = Color.White
            };
            Controls.Add(mainPanel);

            // ====== BARRA DE ESTADO ======
            statusStrip = new StatusStrip
            {
                SizingGrip = false,
                BackColor = Color.FromArgb(38, 41, 45),
                ForeColor = Color.White
            };
            userStatus = new ToolStripStatusLabel("Usuario: admin");
            timeStatus = new ToolStripStatusLabel(DateTime.Now.ToString("dd/MM/yyyy HH:mm")) { Spring = true, TextAlign = ContentAlignment.MiddleRight };
            statusStrip.Items.AddRange(new ToolStripItem[] { userStatus, new ToolStripStatusLabel("|") { Enabled = false }, timeStatus });
            Controls.Add(statusStrip);

            // Reloj en status bar
            clockTimer = new Timer { Interval = 1000 };
            clockTimer.Tick += (_, __) => timeStatus.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            clockTimer.Start();

            // Vista inicial
            Shown += (_, __) => { MostrarBienvenida(); };
        }

        private void MostrarEnPanel(Form form)
        {
            mainPanel.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            // Heredar tema oscuro
            form.BackColor = Color.FromArgb(45, 48, 52);
            form.ForeColor = Color.White;

            mainPanel.Controls.Add(form);
            form.Show();
        }

        private void MostrarBienvenida()
        {
            mainPanel.Controls.Clear();
            var label = new Label
            {
                Text = "Bienvenido al Sistema de Facturación",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Semibold", 18F),
                ForeColor = Color.White
            };
            mainPanel.Controls.Add(label);
        }

        // ====== Colores del menú (oscuro) ======
        private sealed class DarkMenuColors : ProfessionalColorTable
        {
            public override Color MenuStripGradientBegin => Color.FromArgb(38, 41, 45);
            public override Color MenuStripGradientEnd => Color.FromArgb(38, 41, 45);
            public override Color ToolStripDropDownBackground => Color.FromArgb(45, 48, 52);
            public override Color ImageMarginGradientBegin => Color.FromArgb(45, 48, 52);
            public override Color ImageMarginGradientEnd => Color.FromArgb(45, 48, 52);
            public override Color MenuItemBorder => Color.FromArgb(80, 160, 255);
            public override Color MenuItemSelected => Color.FromArgb(60, 64, 70);
            public override Color MenuItemSelectedGradientBegin => Color.FromArgb(60, 64, 70);
            public override Color MenuItemSelectedGradientEnd => Color.FromArgb(60, 64, 70);
            public override Color MenuItemPressedGradientBegin => Color.FromArgb(50, 54, 58);
            public override Color MenuItemPressedGradientEnd => Color.FromArgb(50, 54, 58);
            public override Color SeparatorDark => Color.FromArgb(70, 74, 78);
            public override Color SeparatorLight => Color.FromArgb(70, 74, 78);
            public override Color ToolStripBorder => Color.FromArgb(38, 41, 45);
        }
    }
}
