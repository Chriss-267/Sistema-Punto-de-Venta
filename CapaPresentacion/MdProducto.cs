using CapaEntidad;
using CapaNegocios;
using CapaPresentacion.Utilidades;
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
    public partial class MdProducto : Form
    {
        public Producto _Producto {  get; set; }
        public MdProducto()
        {
            InitializeComponent();
        }

        private void MdProducto_Load(object sender, EventArgs e)
        {
            // Llenar comboBox busqueda con cabeceras de las columnas
            foreach (DataGridViewColumn columna in dgvProductos.Columns)
            {
                if (columna.Visible == true)
                {
                    cbobusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cbobusqueda.DisplayMember = "Texto";
            cbobusqueda.ValueMember = "Valor";
            cbobusqueda.SelectedIndex = 0;


            // Mostrar los productos
            List<Producto> lista = new CN_Productos().Listar();

            foreach (Producto item in lista)
            {
                dgvProductos.Rows.Add(new object[]
                {
                    
                    item.IdProducto,
                    item.codigo,
                    item.nombreProducto,
                    item.precio,
                    item.stock,
                });
            }

        }

        //obtener valores de productos del data
        private void dgvProductos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;  
            int iCol = e.ColumnIndex;

            if (iRow >= 0 && iCol >= 0)
            {
                _Producto = new Producto()
                {
                    IdProducto = Convert.ToInt32(dgvProductos.Rows[iRow].Cells["Id"].Value.ToString()),
                    codigo = dgvProductos.Rows[iRow].Cells["Codigo"].Value.ToString(),
                    nombreProducto = dgvProductos.Rows[iRow].Cells["Producto"].Value.ToString(),
                    precio = Convert.ToDecimal(dgvProductos.Rows[iRow].Cells["Precio"].Value.ToString()),
                    stock = Convert.ToInt32(dgvProductos.Rows[iRow].Cells["Stock"].Value.ToString())

                };
                this.DialogResult = DialogResult.OK;
                this.Close();
                
                    
            }
        }

        //buscar productos
        private void picBuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cbobusqueda.SelectedItem).Valor.ToString();

            if (dgvProductos.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvProductos.Rows)
                {

                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtbusqueda.Text.Trim().ToUpper()))
                        row.Visible = true;
                    else
                        row.Visible = false;
                }
            }
        }
        //limpiar txtbuscador
        private void btnLimpiarBuscador_Click(object sender, EventArgs e)
        {
            txtbusqueda.Text = "";
            foreach (DataGridViewRow row in dgvProductos.Rows)
            {
                row.Visible = true;
            }
        }
    }
}
