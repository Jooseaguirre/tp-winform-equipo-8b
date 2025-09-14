using dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using negocio;  

namespace TPWinForm_equipo_8BB
{
    public partial class FrmAltaMarca : Form
    {
        private MarcaNegocio negocio = new MarcaNegocio();
        public FrmAltaMarca()
        {
            InitializeComponent();
        }
    
    private Marca marca;

        public FrmAltaMarca(Marca marca)
        {
            InitializeComponent();
            this.marca = marca;
            txtNombreMarca.Text = marca.Descripcion;    
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (marca == null)
                    marca = new Marca();
                marca.Descripcion = txtNombreMarca.Text;
                if (marca.IdMarca != 0)
                {
                    negocio.modificar(marca);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    negocio.agregar(marca);
                    MessageBox.Show("Agregado exitosamente");
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la marca: " + ex.Message);
            }
        }   
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }       
    }
}
