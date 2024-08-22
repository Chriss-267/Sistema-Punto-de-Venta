using CapaEntidad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocios;

namespace CapaPresentacion
{
    public partial class Ventas : Form
    {
       
        public Ventas()
        {
            InitializeComponent();
        }

        private void Ventas_Load(object sender, EventArgs e)
        {
            lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtCliente.Text = " Cliente General";
           

            txtCodigo.Text = " Ingrese el Código";

            //mostrar correlativo
            CN_Venta cnVentas = new CN_Venta();
            int correlativo = cnVentas.ObtenerCorrelativo();
            string numeroCorrelativo = correlativo.ToString().PadLeft(5, '0'); 
            lblCorrelativo.Text = numeroCorrelativo;


        }


        //fecha y hora actual 
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        // para cuando se escriba en el texbox quede nuevamente cliente general
        private void txtCliente_Click(object sender, EventArgs e)
        {
            if (txtCliente.Text == " Cliente General")
            {
                txtCliente.Text = "";
            }
        }

        private void txtCliente_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCliente.Text))
            {
                txtCliente.Text = " Cliente General";
            }
        }

        //cuando se escriba el codigo del producto
        private void txtCodigo_Click(object sender, EventArgs e)
        {
            if(txtCodigo.Text == " Ingrese el Código")
            {
                txtCodigo.Text = "";
            }
        }

        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                txtCodigo.Text = " Ingrese el Código";
            }
        }

        //abrir modal para buscar productos y obtenerlos
        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            using (var modal = new MdProducto())
            {
                var result = modal.ShowDialog();
                if (result == DialogResult.OK)
                {
                    txtIdProducto.Text = modal._Producto.IdProducto.ToString();
                    txtCodigo.Text = modal._Producto.codigo;
                    txtProducto.Text = modal._Producto.nombreProducto;
                    txtPrecio.Text = modal._Producto.precio.ToString("0.00");
                    txtStock.Text = modal._Producto.stock.ToString();
                }
                else
                {
                    txtCodigo.Select();
                }
            }

        }

        //introducir codigo producto para que traiga los datos de este
        private void txtCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                Producto oProducto = new CN_Productos().Listar().Where(p=> p.codigo==txtCodigo.Text).FirstOrDefault();

                if (oProducto != null)
                {
                   
                    txtIdProducto.Text = oProducto.IdProducto.ToString();
                    txtProducto.Text = oProducto.nombreProducto;
                    txtPrecio.Text = oProducto.precio.ToString("0.00");
                    txtStock.Text = oProducto.stock.ToString();
                    numpicCantidad.Select();
                }
                else
                {
                    
                    txtIdProducto.Text = "0";
                    txtCodigo.Text = "";
                    txtProducto.Text= "";
                    txtPrecio.Text= "";
                    txtStock.Text = "";
                    numpicCantidad.Value = 1;
                    
                }
                
            }
        }

        //agregar productos
        private void btnAgregarItem_Click(object sender, EventArgs e)
        {
            decimal precio = 0;
            bool producto_existe = false;

            if (int.Parse(txtIdProducto.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un producto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if(!decimal.TryParse(txtPrecio.Text, out precio))
            {
                MessageBox.Show("Precio - Formato moneda incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if(Convert.ToInt32(txtStock.Text) < Convert.ToInt32(numpicCantidad.Value.ToString()))
            {
                MessageBox.Show("La cantidad no puede ser mayor al Stock", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                return;
            }

            foreach(DataGridViewRow fila in dgvProductos.Rows)
            {
                if (fila.Cells["Id"].Value.ToString() == txtIdProducto.Text)
                {
                    producto_existe = true;
                    break;
                }
            }

            if(!producto_existe)
            {
                //aqui se resta el stock al añadir al datagrid productos
                bool respuesta = new CN_Venta().RestarStock(

                    Convert.ToInt32(txtIdProducto.Text),
                    Convert.ToInt32(numpicCantidad.Value.ToString())
                ) ;

                if (respuesta)
                {
                    dgvProductos.Rows.Add(new object[]
                  {
                    txtIdProducto.Text,
                    txtProducto.Text,
                    precio.ToString("0.00"),
                    numpicCantidad.Value.ToString(),
                    (numpicCantidad.Value * precio).ToString("0.00")


                  });

                    CalcularTotal();
                    LimpiarProducto();
                }

                
                
            }

          

        }

        //calcular total

        private void CalcularTotal()
        {
            decimal total = 0;
            if(dgvProductos.Rows.Count > 0)
            {
                foreach(DataGridViewRow row in dgvProductos.Rows )
                {
                    total+= Convert.ToDecimal(row.Cells["SubTotal"].Value.ToString());
                }

            }

            lblSumas.Text= total.ToString("0.00");
           
        }

        //limpiar campos
        private void LimpiarProducto()
        {
            txtIdProducto.Text = "0";
            txtCodigo.Text = " Ingrese el Código";
            txtProducto.Text = "";
            txtPrecio.Text = "";
            txtStock.Text = "";
            numpicCantidad.Value = 0;
        }

        //mostrar icono de basurero para eliminar producto
        private void dgvProductos_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if(e.ColumnIndex == 5)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.basurero20.Width;
                var h = Properties.Resources.basurero20.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.basurero20, new Rectangle(x, y, w, h));
                e.Handled = true;
            }

        }

        //tocar basurero para eliminar
        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvProductos.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                int index = e.RowIndex;
                if(index >= 0)
                {

                    //aqui suma el stock si se quita una cantidad de productos del dtagrid
                    bool respuesta = new CN_Venta().SumarStock(

                       Convert.ToInt32( dgvProductos.Rows[index].Cells["Id"].Value.ToString()),
                        Convert.ToInt32(dgvProductos.Rows[index].Cells["Cantidad"].Value.ToString())
                    );
                    if (respuesta)
                    {
                        dgvProductos.Rows.RemoveAt(index);
                        CalcularTotal();
                    }
                   
                }
            }
        }

        //para que no meta en el precio valores no validos
        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;

            }
            else
            {
                if(txtPrecio.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true;
                }
                else
                {
                    if(Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }



        //para que no meta en efectivo valores no validos

        private void txtEfectivo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;

            }
            else
            {
                if (txtEfectivo.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true;
                }
                else
                {
                    if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }
        //para que no ingrese un valor invalido en descuento
        private void txtDescuento_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;

            }
            else
            {
                if (txtDescuento.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true;
                }
                else
                {
                    if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }


        //verificar si hay productos en la venta
        private bool VerificarExistenciaProductos()
        {
            if (lblSumas.Text.Trim() == "0.00")
            {
                MessageBox.Show("No existen productos en la venta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }
        //calcular descuento

        private void CalcularDescuento()
        {
          

            decimal descuento;
            decimal total = Convert.ToDecimal(lblSumas.Text);

            if (txtDescuento.Text.Trim() == "")
            {
                txtDescuento.Text = "0";
            }

            if (decimal.TryParse(txtDescuento.Text.Trim(), out descuento))
            {
                if (descuento > total)
                {
                    lblTotal.Text = "0.00";
                }
                else
                {
                    decimal totalConDescuento = total - descuento;
                    lblTotal.Text = totalConDescuento.ToString("0.00");

                }
            }
        }


        //calcular cambio
        private void CalcularCambio()
        {
           

            decimal efectivo;
            decimal total = string.IsNullOrWhiteSpace(lblTotal.Text) ? Convert.ToDecimal(lblSumas.Text) : Convert.ToDecimal(lblTotal.Text);



            if (txtEfectivo.Text.Trim() == "")
            {
                txtEfectivo.Text = "0";
            }

            if (decimal.TryParse(txtEfectivo.Text.Trim(), out efectivo))
            {
                if(efectivo< total)
                {
                    lblCambio.Text = "0.00";
                }
                else
                {
                    decimal cambio = efectivo - total;
                    lblCambio.Text = cambio.ToString("0.00");
                }
            }
        }

        private void txtEfectivo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                if (VerificarExistenciaProductos())
                {
                    CalcularDescuento();  // Asegura que lblTotal esté actualizado
                    CalcularCambio();
                }
            }
        }

        
        private void txtDescuento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (VerificarExistenciaProductos())
                {
                    CalcularDescuento();
                }
            }
        }

        //registrar venta 
        private void btnVender_Click(object sender, EventArgs e)
        {

            if (dgvProductos.Rows.Count < 1)
            {
                MessageBox.Show("Debe Ingresar Productos a la venta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            PlantillaFactura.CreaTicket Ticket1 = new PlantillaFactura.CreaTicket();

            Ticket1.TextoCentro("Empresa xxxxx "); //imprime una linea de descripcion
            Ticket1.TextoCentro("**********************************");

            Ticket1.TextoIzquierda("Dirc: xxxx");
            Ticket1.TextoIzquierda("Tel:xxxx ");
            Ticket1.TextoIzquierda("Rnc: xxxx");
            Ticket1.TextoIzquierda("");
            Ticket1.TextoCentro("Factura de Venta"); //imprime una linea de descripcion
            Ticket1.TextoIzquierda("No Fac:" + lblCorrelativo.Text);
            Ticket1.TextoIzquierda("Fecha:" + DateTime.Now.ToShortDateString() + " Hora:" + DateTime.Now.ToShortTimeString());
            Ticket1.TextoIzquierda("Le Atendio: xxxx");
            Ticket1.TextoIzquierda("");
            PlantillaFactura.CreaTicket.LineasGuion();

            PlantillaFactura.CreaTicket.EncabezadoVenta();
            PlantillaFactura.CreaTicket.LineasGuion();
            foreach (DataGridViewRow r in dgvProductos.Rows)
            {
                // PROD                     //PrECIO                                    CANT                         TOTAL
                Ticket1.AgregaArticulo(r.Cells[1].Value.ToString(), double.Parse(r.Cells[2].Value.ToString()), int.Parse(r.Cells[3].Value.ToString()), double.Parse(r.Cells[4].Value.ToString())); //imprime una linea de descripcion
            }


            PlantillaFactura.CreaTicket.LineasGuion();
            Ticket1.AgregaTotales("Sub-Total", double.Parse("000")); // imprime linea con Subtotal
            Ticket1.AgregaTotales("Menos Descuento", double.Parse("000")); // imprime linea con decuento total
            Ticket1.AgregaTotales("Mas ITBIS", double.Parse("000")); // imprime linea con ITBis total
            Ticket1.TextoIzquierda(" ");
            Ticket1.AgregaTotales("Total", double.Parse(lblTotal.Text)); // imprime linea con total
            Ticket1.TextoIzquierda(" ");
            Ticket1.AgregaTotales("Efectivo Entregado:", double.Parse(txtEfectivo.Text));
            Ticket1.AgregaTotales("Efectivo Devuelto:", double.Parse(lblCambio.Text));


            // Ticket1.LineasTotales(); // imprime linea 

            Ticket1.TextoIzquierda(" ");
            Ticket1.TextoCentro("**********************************");
            Ticket1.TextoCentro("*     Gracias por preferirnos    *");

            Ticket1.TextoCentro("**********************************");
            Ticket1.TextoIzquierda(" ");
            string impresora = "Microsoft XPS Document Writer";

            Ticket1.ImprimirTiket(impresora);

            string filePath = @"C:\ruta\al\archivo\factura.txt";

            Ticket1.ImprimirTiket(filePath);




            MessageBox.Show("Gracias por preferirnos");






           

            DataTable detalle_venta = new DataTable();
            detalle_venta.Columns.Add("Id", typeof(int));
            detalle_venta.Columns.Add("Precio", typeof(decimal));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("SubTotal", typeof(decimal));

            foreach(DataGridViewRow row in dgvProductos.Rows)
            {
                detalle_venta.Rows.Add(new object[]
                {
                    row.Cells["Id"].Value.ToString(),
                    row.Cells["Precio"].Value.ToString(),
                    row.Cells["Cantidad"].Value.ToString(),
                    row.Cells["SubTotal"].Value.ToString(),

                });
            }

            int idCorrelativo = new CN_Venta().ObtenerCorrelativo();
            string numeroCorrelativo = string.Format("{0:00000}", idCorrelativo);

            if (VerificarExistenciaProductos())
            {
                CalcularDescuento();  // Asegura que lblTotal esté actualizado
                CalcularCambio();
            }
            Venta oventa = new Venta()
            {

                NumeroDocumento = numeroCorrelativo,
                NombreCliente= txtCliente.Text,
                MontoPago = Convert.ToDecimal(txtEfectivo.Text),
                MontoDescuento = Convert.ToDecimal(txtDescuento.Text),
                MontoCambio = Convert.ToDecimal(lblCambio.Text),
                MontoTotal = Convert.ToDecimal(lblTotal.Text)
            };

            string mensaje = string.Empty;
            bool respuesta = new CN_Venta().Registrar(oventa, detalle_venta, out mensaje);

            if(respuesta)
            {
                var result = MessageBox.Show("Numero de venta generada:\n" + numeroCorrelativo + "\n\n¿Desea copiar al portapapeles?", "Mensaje",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if(result == DialogResult.Yes)
                {
                    Clipboard.SetText(numeroCorrelativo);
                }

                txtCliente.Text = " Cliente General";
                dgvProductos.Rows.Clear();
                CalcularTotal();
                txtDescuento.Text = "0";
                txtEfectivo.Text = "0";
                lblTotal.Text = "0.00";
                lblCambio.Text = "0.00";


                    
            }
            else
            {
                MessageBox.Show(mensaje,"Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            CN_Venta cnVentas = new CN_Venta();
            int correlativo = cnVentas.ObtenerCorrelativo();
            string NCorrelativo = correlativo.ToString().PadLeft(5, '0');
            lblCorrelativo.Text = NCorrelativo;
        }
    }
}
