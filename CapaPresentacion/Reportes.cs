using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaPresentacion.Utilidades;
using CapaEntidad;
using CapaNegocios;
using ClosedXML.Excel;

namespace CapaPresentacion
{
    public partial class Reportes : Form
    {
        public Reportes()
        {
            InitializeComponent();
        }

        private void Reportes_Load(object sender, EventArgs e)
        {
            foreach(DataGridViewColumn columna in dgvProductos.Columns)
            {
                cboBusqueda.Items.Add(new OpcionCombo()
                {
                    Valor = columna.Name, Texto = columna.HeaderText
                });

                cboBusqueda.DisplayMember = "Texto";
                cboBusqueda.ValueMember = "Valor";
                cboBusqueda.SelectedIndex = 0;
            }
        }

        private void btnBuscarReporte_Click(object sender, EventArgs e)
        {
            List<ReporteVentas> lista = new List<ReporteVentas>();
            lista = new CN_Reporte().Venta(txtFechaInicio.Value.ToString("dd/MM/yyyy"), txtFechaFin.Value.ToString("dd/MM/yyyy"));
            dgvProductos.Rows.Clear();

            foreach(ReporteVentas rv in lista)
            {
                dgvProductos.Rows.Add(new object[]
                {
                    rv.FechaRegistro,
                    rv.NumeroDocumento,
                    rv.MontoTotal,
                    rv.NombreCliente,
                    rv.CodigoProducto,
                    rv.NombreProducto,
                    rv.PrecioVenta,
                    rv.Cantidad,
                    rv.SubTotal
                });
            }
        }

        private void btnBuscarConFiltro_Click(object sender, EventArgs e)
        {
            string ColumnaFiltro = ((OpcionCombo)cboBusqueda.SelectedItem).Valor.ToString();

            if(dgvProductos.Rows.Count > 0)
            {
                foreach(DataGridViewRow row in dgvProductos.Rows)
                {
                    if (row.Cells[ColumnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))
                    {
                        row.Visible = true;
                    }
                    else
                    {
                        row.Visible= false;
                    }
                }
            }
        }

        private void btnLimpaiBuscador_Click(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";
            foreach(DataGridViewRow row in dgvProductos.Rows)
            {
                row.Visible = true;
            }
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            if (dgvProductos.Rows.Count < 1)
            {

                MessageBox.Show("No hay registros para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            else
            {

                DataTable dt = new DataTable();

                foreach (DataGridViewColumn columna in dgvProductos.Columns)
                {
                    dt.Columns.Add(columna.HeaderText, typeof(string));
                }

                foreach (DataGridViewRow row in dgvProductos.Rows)
                {
                    if (row.Visible)
                        dt.Rows.Add(new object[] {
                            row.Cells[0].Value.ToString(),
                            row.Cells[1].Value.ToString(),
                            row.Cells[2].Value.ToString(),
                            row.Cells[3].Value.ToString(),
                            row.Cells[4].Value.ToString(),
                            row.Cells[5].Value.ToString(),
                            row.Cells[6].Value.ToString(),
                            row.Cells[7].Value.ToString(),
                            row.Cells[8].Value.ToString()


                        });
                }

                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = string.Format("ReporteVentas_{0}.xlsx", DateTime.Now.ToString("ddMMyyyyHHmmss"));
                savefile.Filter = "Excel Files | *.xlsx";

                if (savefile.ShowDialog() == DialogResult.OK)
                {

                    try
                    {
                        XLWorkbook wb = new XLWorkbook();
                        var hoja = wb.Worksheets.Add(dt, "Informe");
                        hoja.ColumnsUsed().AdjustToContents();
                        wb.SaveAs(savefile.FileName);
                        MessageBox.Show("Reporte Generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch
                    {
                        MessageBox.Show("Error al generar reporte", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }

            }
        }
    }
}
