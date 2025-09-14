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
    public partial class FrmCategoria : Form
    {
        private CategoriaNegocio negocio = new CategoriaNegocio();
        public FrmCategoria()
        {
            InitializeComponent();
        }

        private void FrmCategoria_Load(object sender, EventArgs e)
        {
            cargar();
        }
        
        private void cargar()
        {
            try
            {
                List<Categoria> lista = negocio.listar();
                dgvCategoria.AutoGenerateColumns = true;
                dgvCategoria.DataSource = lista;
                dgvCategoria.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                if (dgvCategoria.Columns["IdCategoria"] != null)
                    dgvCategoria.Columns["IdCategoria"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar categorias: " + ex.Message);
            }
        }

        private void btnModificarCategoria_Click(object sender, EventArgs e)
        {
            if (dgvCategoria.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una categoria: ");
                return;
            }

            Categoria seleccionada = (Categoria)dgvCategoria.CurrentRow.DataBoundItem;
            FrmAltaCategoria modificar = new FrmAltaCategoria(seleccionada);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminarCategoria_Click(object sender, EventArgs e)
        {
            if (dgvCategoria.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una categoria: ");
                return;
            }

            Categoria seleccionada = (Categoria)dgvCategoria.CurrentRow.DataBoundItem;

            DialogResult respuesta = MessageBox.Show(
                "¿Está seguro que desea eliminar la categoria '" + seleccionada.Descripcion + "'?",
                "Eliminar Categoria",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (respuesta == DialogResult.Yes)
            {
                try
                {
                    negocio.eliminar(seleccionada.IdCategoria);
                    cargar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message);
                }
            }

        }

        private void btnAgregarCategoria_Click(object sender, EventArgs e)
        {
            FrmAltaCategoria alta = new FrmAltaCategoria();
            alta.ShowDialog();
            cargar();
        }
    }
}

