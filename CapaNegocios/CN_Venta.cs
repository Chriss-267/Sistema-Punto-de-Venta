using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios
{
    public  class CN_Venta
    {
        private CD_Ventas obventas = new CD_Ventas();

        public bool RestarStock(int idproducto, int cantidad)
        {
            return obventas.RestarStock(idproducto, cantidad);
        }

        public bool SumarStock(int idProducto, int cantidad)
        {
            return obventas.SumarStock(idProducto, cantidad);
        }
        public int ObtenerCorrelativo()
        {
            return obventas.ObtenerCorrelativo();
        }

        public bool Registrar(Venta obj, DataTable DetalleCompra, out string Mensaje)
        {
            return obventas.Resgistrar(obj, DetalleCompra, out Mensaje);
        }

        public Venta ObtenerVenta(string numero)
        {
            Venta oventa = obventas.ObtenerVenta(numero);
            if(oventa.IdVenta != 0)
            {
                List<Detalle_Venta> oDetalleVenta = obventas.ObtenerDetalleVenta(oventa.IdVenta);
                oventa.oDetalleVenta= oDetalleVenta;
            }
            return oventa;
        }
    }

   
}
