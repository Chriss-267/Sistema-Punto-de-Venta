using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace CapaPresentacion
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {

            // Leer la cultura predeterminada desde el archivo de configuración
            string culturaConfigurada = ConfigurationManager.AppSettings["DefaultCulture"];
            if (!string.IsNullOrEmpty(culturaConfigurada))
            {
                CultureInfo cultura = new CultureInfo(culturaConfigurada);
                Thread.CurrentThread.CurrentCulture = cultura;
                Thread.CurrentThread.CurrentUICulture = cultura;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Inicio());
        }
    }
}
