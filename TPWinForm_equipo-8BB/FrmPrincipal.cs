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


        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void cboTipo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
           // MessageBox.Show("Bienvenido a la aplicacion");  
            cboTipo.Items.Add("Nombre");
            cboTipo.Items.Add("Codigo");
            cboTipo.Items.Add("Marca");

            //Metodo para que se actualice el form principal de Articulos, cada vez que "Agrego" articulos
            cargar();

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
    }
}
