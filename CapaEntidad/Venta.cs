﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public  class Venta
    {
        public int IdVenta {  get; set; }
        public string NumeroDocumento {  get; set; }
        public string NombreCliente { get; set; }
        public decimal MontoPago { get; set; }
        public decimal MontoDescuento { get; set; }
        public decimal MontoCambio { get; set; }
        public decimal MontoTotal { get; set; }
        public List<Detalle_Venta> oDetalleVenta { get; set; }
        public string FechaRegistro {  get; set; }

    }
}
