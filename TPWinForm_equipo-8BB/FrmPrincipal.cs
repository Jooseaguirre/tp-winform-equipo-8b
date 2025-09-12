using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPWinForm_equipo_8BB
{
    public partial class FrmPrincipal : Form
    {
        private List<Articulo> listaArticulo;


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

            ArticuloNegocio negocio = new ArticuloNegocio();
            listaArticulo = negocio.Listar();
            dgvArticulos.DataSource = listaArticulo;
            pboxArticulo.Load(listaArticulo[0].Imagenes[0].ImagenUrl);
        }

        //MENSAJE DE CIERRE APP
        private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MessageBox.Show("Gracias por usar la aplicacion");  
        }


        //EVENTO QUE MUESTRA LA IMAGEN DEL ARTICULO
        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            cargarImagen(seleccionado.Imagenes[0].ImagenUrl);

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


    }
}
