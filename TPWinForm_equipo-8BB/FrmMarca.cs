using negocio;
using dominio;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TPWinForm_equipo_8BB
{
    public partial class FrmMarcas : Form
    {
        private MarcaNegocio negocio = new MarcaNegocio();

        public FrmMarcas()
        {
            InitializeComponent();
        }

        

        private void FrmMarcas_Load(object sender, EventArgs e)
        {
            cargar();
        }

        private void cargar()
        {
            try
            {
                List<Marca> lista = negocio.listar();
                dgvMarca.AutoGenerateColumns = true;
                dgvMarca.DataSource = lista;
                dgvMarca.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                if (dgvMarca.Columns["IdMarca"] != null)
                    dgvMarca.Columns["IdMarca"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar marcas: " + ex.Message);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FrmAltaMarca alta = new FrmAltaMarca();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvMarca.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una marca.");
                return;
            }

            Marca seleccionada = (Marca)dgvMarca.CurrentRow.DataBoundItem;
            FrmAltaMarca modificar = new FrmAltaMarca(seleccionada);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvMarca.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una marca.");
                return;
            }

            Marca seleccionada = (Marca)dgvMarca.CurrentRow.DataBoundItem;

            DialogResult respuesta = MessageBox.Show(
                "¿Está seguro que desea eliminar la marca '" + seleccionada.Descripcion + "'?",
                "Eliminar Marca",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (respuesta == DialogResult.Yes)
            {
                try
                {
                    negocio.eliminar(seleccionada.IdMarca);
                    cargar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message);
                }
            }
        }

       
    }
}