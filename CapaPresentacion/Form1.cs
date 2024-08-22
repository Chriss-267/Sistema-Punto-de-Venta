using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocios;
using CapaPresentacion.Utilidades;

namespace CapaPresentacion
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Llenar comboBox busqueda con cabeceras de las columnas
            foreach (DataGridViewColumn columna in dgvProductos.Columns)
            {
                if (columna.Visible == true && columna.Name != "btnseleccionar")
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
                    " ",
                    item.IdProducto,
                    item.codigo,
                    item.nombreProducto,
                    item.descripcion,
                    item.precio,
                    item.stock,
                });
            }
        }

        // Guardar productos
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            // Validar las entradas
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                mensaje += "Es necesario el codigo del Producto\n";
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                mensaje += "Es necesario el nombre completo del Producto\n";
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                mensaje += "Es necesario un precio válido para el Producto\n";
            }

            if (!int.TryParse(numpicStock.Value.ToString(), out int stock))
            {
                mensaje += "Es necesario un stock válido para el Producto\n";
            }

            if (!string.IsNullOrEmpty(mensaje))
            {
                MessageBox.Show(mensaje, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Producto obj = new Producto()
            {
                IdProducto = Convert.ToInt32(txtid.Text),
                codigo = txtCodigo.Text,
                nombreProducto = txtNombre.Text,
                descripcion = txtDescripcion.Text,
                precio = precio,
                stock = stock
            };

            if (obj.IdProducto == 0)
            {
                int idgenerado = new CN_Productos().Registrar(obj, out mensaje);

                if (idgenerado != 0)
                {
                    dgvProductos.Rows.Add(new object[]
                    {
                " ",
                idgenerado,
                txtCodigo.Text,
                txtNombre.Text,
                txtDescripcion.Text,
                txtPrecio.Text,
                numpicStock.Value
                    });

                    Limpiar();
                    MessageBox.Show("El producto se registró correctamente.", "Registro Exitoso!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show(mensaje);
                }
            }
            else
            {
                bool resultado = new CN_Productos().Editar(obj, out mensaje);

                if (resultado)
                {
                    DataGridViewRow row = dgvProductos.Rows[Convert.ToInt32(txtIndice.Text)];
                    row.Cells["Id"].Value = txtid.Text;
                    row.Cells["Codigo"].Value = txtCodigo.Text;
                    row.Cells["Producto"].Value = txtNombre.Text;
                    row.Cells["Descripcion"].Value = txtDescripcion.Text;
                    row.Cells["Precio"].Value = txtPrecio.Text;
                    row.Cells["Stock"].Value = numpicStock.Value;

                    Limpiar();
                    MessageBox.Show("El producto se editó correctamente.", "Edición Exitosa!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(mensaje);
                }
            }
        }

        public void Limpiar()
        {
            txtIndice.Text = "-1";
            txtid.Text = "0";
            txtCodigo.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtPrecio.Text = string.Empty;
            numpicStock.Value = 0;
        }

        // Mostrar icono de cheque en primera fila del DataGridView
        private void dgvProductos_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                var w = Properties.Resources.check20.Width;
                var h = Properties.Resources.check20.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.check20, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }

        // Tocar botón con icono para que devuelva los datos de la fila a los TextBox
        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvProductos.Columns[e.ColumnIndex].Name == "btnseleccionar")
            {
                int indice = e.RowIndex;
                if (indice >= 0)
                {
                    txtIndice.Text = indice.ToString();
                    txtid.Text = dgvProductos.Rows[indice].Cells["Id"].Value.ToString();
                    txtCodigo.Text = dgvProductos.Rows[indice].Cells["Codigo"].Value.ToString();
                    txtNombre.Text = dgvProductos.Rows[indice].Cells["Producto"].Value.ToString();
                    txtDescripcion.Text = dgvProductos.Rows[indice].Cells["Descripcion"].Value.ToString();
                    txtPrecio.Text = dgvProductos.Rows[indice].Cells["Precio"].Value.ToString();
                    numpicStock.Value = Convert.ToInt32(dgvProductos.Rows[indice].Cells["Stock"].Value);
                }
            }
        }

        // Eliminar
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtid.Text) != 0)
            {
                if (MessageBox.Show("¿Desea eliminar el Producto?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    Producto obj = new Producto()
                    {
                        IdProducto = Convert.ToInt32(txtid.Text)
                    };

                    bool respuesta = new CN_Productos().Eliminar(obj, out mensaje);

                    if (respuesta)
                    {
                        dgvProductos.Rows.RemoveAt(Convert.ToInt32(txtIndice.Text));
                        Limpiar();
                        MessageBox.Show("El producto se eliminó correctamente.", "Eliminación Exitosa!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        // buscar
        private void pictureBox1_Click(object sender, EventArgs e)
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

        //Limpiar buscador
        private void btnLimpaiBuscador_Click(object sender, EventArgs e)
        {
            txtbusqueda.Text = "";
            foreach (DataGridViewRow row in dgvProductos.Rows)
            {
                row.Visible = true;
            }
        }

        
    }
}

