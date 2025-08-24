using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación.Ayuda
{
    public class AyudaForm : Form
    {
        private WebBrowser web;

        public AyudaForm()
        {
            Text = "Ayuda en línea";
            BackColor = Color.White;

            var lbl = new Label
            {
                Text = "AYUDA (documentación en línea)",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 120, 200),
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0)
            };

            web = new WebBrowser
            {
                Dock = DockStyle.Fill,
                ScriptErrorsSuppressed = true
            };

            Controls.Add(web);
            Controls.Add(lbl);

            // URL sugerida en el requerimiento: sitio oficial Microsoft
            Shown += (_, __) => web.Navigate("https://support.microsoft.com/es-es");
        }
    }
}
