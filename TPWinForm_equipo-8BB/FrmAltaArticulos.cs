using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;
using System.Configuration;



namespace TPWinForm_equipo_8BB
{
    public partial class FrmAltaArticulo : Form
    {
        private Articulo articulo = null;
        private OpenFileDialog archivo = null;

        public FrmAltaArticulo()
        {
            InitializeComponent();
        }
        public FrmAltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Imagen imagenArticulo = new Imagen();
            ImagenNegocio imagenNegocio = new ImagenNegocio();

            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                if (articulo == null)
                    articulo = new Articulo();

                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Marca = (Marca)cboMarca.SelectedItem;
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                articulo.Descripcion = txtDescripcion.Text;
                imagenArticulo.ImagenUrl = textUrlImagen.Text;
                articulo.precio = decimal.Parse(txtPrecio.Text);

                if (articulo.Id != 0)
                {
                    negocio.modificar(articulo);

                    foreach (Imagen img in articulo.Imagenes)
                    {
                        //para imag nuevas
                        if (img.Id == 0) 
                        {
                            img.IdArticulo = articulo.Id;
                            imagenNegocio.agregar(img);
                        }
                    }

                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    int idGenerado = negocio.agregar(articulo);

                    foreach (Imagen img in articulo.Imagenes)
                    {
                        //para imag nuevas
                        if (img.Id == 0)
                        {
                            img.IdArticulo = idGenerado;
                            imagenNegocio.agregar(img);
                        }
                    }

                    MessageBox.Show("Agregado exitosamente");
                }

                Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void FrmAltaArticulo_Load(object sender, EventArgs e)
        {
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            MarcaNegocio marcaNegocio = new MarcaNegocio();

            try
            {
                cboCategoria.DataSource = categoriaNegocio.listar();
                cboCategoria.ValueMember = "idCategoria";
                cboCategoria.DisplayMember = "Descripcion";

                cboMarca.DataSource = marcaNegocio.listar();
                cboMarca.ValueMember = "idMarca";
                cboMarca.DisplayMember = "Descripcion";

                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtPrecio.Text = articulo.precio.ToString();


                    cboMarca.SelectedValue = articulo.Marca.IdMarca;
                    cboCategoria.SelectedValue = articulo.Categoria.IdCategoria;

                    //ListBox para cargar más deuna imagen
                    lstImagenes.DataSource = articulo.Imagenes;
                    lstImagenes.DisplayMember = "ImagenUrl";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void textUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(textUrlImagen.Text);
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxArticulo.Load("https://media.istockphoto.com/id/1147544807/vector/thumbnail-image-vector-graphic.jpg?s=612x612&w=0&k=20&c=rnCKVbdxqkjlcs3xH87-9gocETqpspHFXu5dIGB4wuM=");
            }
        }

        private void btnImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                textUrlImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);
            }
        }


        private void btnEliminarImagen_Click(object sender, EventArgs e)
        {
            if (lstImagenes.SelectedItem != null)
            {
                Imagen seleccionada = (Imagen)lstImagenes.SelectedItem;

                if (seleccionada.Id != 0)
                {
                    ImagenNegocio imgNegocio = new ImagenNegocio();
                    imgNegocio.eliminar(seleccionada.Id);
                }

                articulo.Imagenes.Remove(seleccionada);

                lstImagenes.DataSource = null;
                lstImagenes.DataSource = articulo.Imagenes;
                lstImagenes.DisplayMember = "ImagenUrl";
            }
        }

        private void btnAgregarURL_Click(object sender, EventArgs e)
        {
            if (textUrlImagen.Text != null && textUrlImagen.Text.Length != 0)
            {

                // nuevo articulo
                if (articulo == null)
                {
                    articulo = new Articulo();
                    articulo.Imagenes = new List<Imagen>();
                }

                Imagen nueva = new Imagen
                {
                    IdArticulo = articulo.Id,
                    ImagenUrl = textUrlImagen.Text
                };

                articulo.Imagenes.Add(nueva);

                lstImagenes.DataSource = null;
                lstImagenes.DataSource = articulo.Imagenes;
                lstImagenes.DisplayMember = "ImagenUrl";

                cargarImagen(textUrlImagen.Text);
                textUrlImagen.Clear();
            }
        }
    }
}