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

namespace TPWinForm_equipo_8BB
{
    public partial class FrmAltaCategoria : Form
    {
        private CategoriaNegocio negocio = new CategoriaNegocio();
        private Categoria Categoria;
        public FrmAltaCategoria()
        {
            InitializeComponent();
        }

        private Categoria categoria;

        public FrmAltaCategoria(Categoria categoria)
        {
            InitializeComponent();
            this.categoria = categoria;
            txtNombreCategoria.Text = categoria.Descripcion;
        }

        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (categoria == null)
                    categoria = new Categoria();
                categoria.Descripcion = txtNombreCategoria.Text;
                if (categoria.IdCategoria != 0)
                {
                    negocio.modificar(categoria);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    negocio.agregar(categoria);
                    MessageBox.Show("Agregado exitosamente");
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la categoria: " + ex.Message);
            }
        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
