using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace TPWinForm_equipo_8BB
{
    public partial class FrmPrincipal : Form
    {
        private List<Articulo> listaArticulo;
        private List<Imagen> listaImagenArticulo;

        private Articulo articulo = null;

        public FrmPrincipal()
        {
            InitializeComponent();
        }


        public FrmPrincipal(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
        }

        private void cboTipo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            // MessageBox.Show("Bienvenido a la aplicacion");  


            //Metodo para que se actualice el form principal de Articulos, cada vez que "Agrego" articulos
            cargar();
            cboCampo.Items.Add("Precio");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Marca");


        }

        //MENSAJE DE CIERRE APP
        private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MessageBox.Show("Gracias por usar la aplicacion");  
        }


        //EVENTO QUE MUESTRA LA IMAGEN DEL ARTICULO
        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            //Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            //cargarImagen(seleccionado.Imagenes[0].ImagenUrl);

            if (dgvArticulos.CurrentRow == null)
            {
                return;
            }

            Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            ImagenNegocio imagenNegocio = new ImagenNegocio();
            List<Imagen> imagenes = imagenNegocio.ListarPorIdArticulo(seleccionado.Id);

            if (imagenes != null && imagenes.Count > 0 && !string.IsNullOrEmpty(imagenes[0].ImagenUrl))
            {
                cargarImagen(imagenes[0].ImagenUrl);
            }
            else
            {
                cargarImagen("https://media.istockphoto.com/id/1147544807/vector/thumbnail-image-vector-graphic.jpg?s=612x612&w=0&k=20&c=rnCKVbdxqkjlcs3xH87-9gocETqpspHFXu5dIGB4wuM=");
            }


        }

        //CARGAR LISTA DE ARTICULOS
        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                //Listar Articulos
                listaArticulo = negocio.Listar();
                dgvArticulos.DataSource = listaArticulo;
                //dgvArticulos.Columns["UrlImagen"].Visible = false;
                dgvArticulos.Columns["Id"].Visible = false;
                //cargarImagen(listaArticulo[0].Imagenes[0].ImagenUrl);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }



        //METODO QUE CARGA LA IMAGEN
        private void cargarImagen(string imagen)
        {
            try
            {
                pboxArticulo.Load(imagen);
            }
            catch (Exception ex)
            {
                pboxArticulo.Load("https://media.istockphoto.com/id/1147544807/vector/thumbnail-image-vector-graphic.jpg?s=612x612&w=0&k=20&c=rnCKVbdxqkjlcs3xH87-9gocETqpspHFXu5dIGB4wuM=");
            }
        }

        //AGREGAR ARTICULO
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FrmAltaArticulo altaArticulo = new FrmAltaArticulo();
            altaArticulo.ShowDialog();
            cargar();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            if (opcion == "Nombre" || opcion == "Marca")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
            else if (opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
        }
        private bool validarFiltro()
        {
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el campo para filtrar.");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el criterio para filtrar.");
                return true;
            }
            if (cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtFiltro.Text))
                {
                    MessageBox.Show("Debes cargar el filtro para numéricos...");
                    return true;
                }
                if (!(soloNumeros(txtFiltro.Text)))
                {
                    MessageBox.Show("Solo nros para filtrar por un campo numérico...");
                    return true;
                }

            }

            return false;
        }

        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }
            return true;
        }


        private void btnFiltro_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if(validarFiltro())
                {
                    return;
                }

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltro.Text;
                dgvArticulos.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

            FrmAltaArticulo modificar = new FrmAltaArticulo(seleccionado);
            modificar.ShowDialog();
            cargar();


        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;

            try
            {
                DialogResult respuesta = MessageBox.Show("¿Estas seguro de borrar?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado.Id);
                    cargar();
                }  
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void menuMarcaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMarcas marcas = new FrmMarcas();
            marcas.ShowDialog();    
        }
    }
}
