using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class Inicio : Form
    {
        private static Form FormularioActivo = null;
        public Inicio()
        {
            InitializeComponent();
        }

        private void Inicio_Load(object sender, EventArgs e)
        {

        }
        private void AbrirFormulario(Form formulario)
        {

            

            if (FormularioActivo != null)
            {
                FormularioActivo.Close();
            }

            FormularioActivo = formulario;
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            formulario.BackColor = Color.WhiteSmoke;

            contenedor.Controls.Add(formulario);
            formulario.Show();


        }

        private void btnInventario_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new Form1());
        }

        private void vENTASToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new Ventas());
        }

        private void btnDetalleVentas_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new DetalleVenta());
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new Reportes());
        }
    }
}
