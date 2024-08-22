using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public  class Detalle_Venta
    {
        public int IdDetalleventa { get; set; }
        public Producto oProducto { get; set; }
        public decimal Precioventa { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal {  get; set; }
        public string FechaRegistro { get; set; }
    }
}
