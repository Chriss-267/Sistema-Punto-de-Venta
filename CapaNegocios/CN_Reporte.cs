using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios
{
    public class CN_Reporte
    {
        private CD_Reporte obCD_Reporte = new CD_Reporte();

        public List<ReporteVentas> Venta(string fechainicio, string fechafin)
        {
            return obCD_Reporte.Venta(fechainicio, fechafin);
        }
           
    }
}
