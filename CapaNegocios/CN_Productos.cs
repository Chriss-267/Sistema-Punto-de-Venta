using System;
using System.Collections.Generic;
using System.Text;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocios
{
    public class CN_Productos
    {
        CD_Productos obcd_productos = new CD_Productos();

        // Para mostrar
        public List<Producto> Listar()
        {
            return obcd_productos.Listar();
        }

        // Para registrar
        public int Registrar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.codigo))
            {
                Mensaje += "Es necesario el código del Producto\n";
            }

            if (string.IsNullOrWhiteSpace(obj.nombreProducto))
            {
                Mensaje += "Es necesario el nombre completo del Producto\n";
            }

            if (obj.precio <= 0)
            {
                Mensaje += "Es necesario el precio del Producto\n";
            }

            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return obcd_productos.Registrar(obj, out Mensaje);
            }
        }

        // Para editar
        public bool Editar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.codigo))
            {
                Mensaje += "Es necesario el código del Producto\n";
            }

            if (string.IsNullOrWhiteSpace(obj.nombreProducto))
            {
                Mensaje += "Es necesario el nombre del Producto\n";
            }

            if (obj.precio <= 0)
            {
                Mensaje += "Es necesario el precio del Producto\n";
            }

            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return obcd_productos.Editar(obj, out Mensaje);
            }
        }

        // Para eliminar
        public bool Eliminar(Producto obj, out string Mensaje)
        {
            return obcd_productos.Eliminar(obj, out Mensaje);
        }
    }
}
