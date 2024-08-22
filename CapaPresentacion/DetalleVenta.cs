using CapaNegocios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaEntidad;



namespace CapaPresentacion
{
    public partial class DetalleVenta : Form
    {
        public DetalleVenta()
        {
            InitializeComponent();
        }

        private void DetalleVenta_Load(object sender, EventArgs e)
        {
            txtBuscar.Select();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Venta oVenta = new CN_Venta().ObtenerVenta(txtBuscar.Text);
            if (oVenta.IdVenta != 0)
            {
                txtCorrelativo.Text = oVenta.NumeroDocumento;
                txtFecha.Text = oVenta.FechaRegistro;
                txtNombreCliente.Text = oVenta.NombreCliente;

                dgvProductos.Rows.Clear();
                foreach (Detalle_Venta dv in oVenta.oDetalleVenta)
                {
                    dgvProductos.Rows.Add(new object[] { dv.oProducto.nombreProducto, dv.Precioventa, dv.Cantidad, dv.SubTotal });
                }

                txtMontoTotal.Text = oVenta.MontoTotal.ToString("0.00");
                txtMontoDescuento.Text = oVenta.MontoDescuento.ToString("0.00");
                txtMontoPago.Text = oVenta.MontoPago.ToString("0.00");
                txtMontocambio.Text = oVenta.MontoCambio.ToString("0.00");
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFecha.Text = "";
            txtNombreCliente.Text = "";
            txtCorrelativo.Text = "";
            dgvProductos.Rows.Clear();
            txtMontoTotal.Text = "0.00";
            txtMontoDescuento.Text = "0.00";
            txtMontoPago.Text = "0.00";
            txtMontocambio.Text = "0.00";
                
        }

        
    }
}
