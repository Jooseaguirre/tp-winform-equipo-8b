using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TPWinForm_equipo_8BB;

namespace TPWinForm_equipo_8BB
{
    public partial class FrmDetalleArticulo : Form
    {
        private Articulo articulo;
        private int indiceImagen = 0;
        private List<Imagen> imagenesArticulo = new List<Imagen>();

        public FrmDetalleArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
        }

        private void FrmDetalleArticulo_Load(object sender, EventArgs e)
        {
            if (articulo != null)
            {
                CargarArticulo();
                if (imagenesArticulo.Count > 0)
                    MostrarImagen(0);
            }
        }

        private void CargarArticulo()
        {
            try
            {
                List<Articulo> lista = new List<Articulo> { articulo };
                ImagenNegocio imagenNegocio = new ImagenNegocio();

                txtNombre.Text = articulo.Nombre;
                txtCodigo.Text = articulo.Codigo;
                txtDescripcion.Text = articulo.Descripcion;
                txtMarca.Text = articulo.Marca.Descripcion;
                txtCategoria.Text = articulo.Categoria.Descripcion;
                txtPrecio.Text = articulo.precio.ToString("C2");

                // Traer todas las imágenes del artículo
                imagenesArticulo = imagenNegocio.ListarPorIdArticulo(articulo.Id);

                lstImagenes.DataSource = null;
                lstImagenes.DataSource = imagenesArticulo;
                lstImagenes.DisplayMember = "ImagenUrl";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el artículo: " + ex.Message);
            }
        }

        private void MostrarImagen(int index)
        {
            if (imagenesArticulo.Count == 0) return;
            if (index < 0 || index >= imagenesArticulo.Count) return;

            indiceImagen = index;

            try
            {
                pboxDetalleArticulo.Load(imagenesArticulo[indiceImagen].ImagenUrl);
            }
            catch
            {
                pboxDetalleArticulo.Load("https://media.istockphoto.com/id/1147544807/vector/thumbnail-image-vector-graphic.jpg?s=612x612&w=0&k=20&c=rnCKVbdxqkjlcs3xH87-9gocETqpspHFXu5dIGB4wuM=");
            }

            lstImagenes.SelectedIndex = indiceImagen;
        }

        // Flechas
        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            indiceImagen = (indiceImagen + 1) % imagenesArticulo.Count;
            MostrarImagen(indiceImagen);

        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (imagenesArticulo.Count == 0) return;
            int nuevoIndice = (indiceImagen - 1 + imagenesArticulo.Count) % imagenesArticulo.Count;
            MostrarImagen(nuevoIndice);
        }


        private void btnVolver_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}