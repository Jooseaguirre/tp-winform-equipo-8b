using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPWinForm_equipo_8BB
{
    internal class ArticuloNegocio
    {
        public List<Articulo> Listar()
        {
            List<Articulo> listaArticulos = new List<Articulo>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comandoArticulo = new SqlCommand();
            SqlDataReader lectorArticulo;
           
            

            try
            {
                //CONEXION A BASE DE DATOS
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true";
                conexion.Open();


                //ARTICULOS
                comandoArticulo.CommandType = System.Data.CommandType.Text;
                comandoArticulo.CommandText = "SELECT a.Id, a.Nombre, a.Codigo, a.Descripcion, a.IdMarca, a.IdCategoria, a.Precio, i.ImagenUrl FROM ARTICULOS a LEFT JOIN IMAGENES i ON i.IdArticulo = a.Id;";
                comandoArticulo.Connection = conexion;


           
                //leer ARTICULOS
                lectorArticulo = comandoArticulo.ExecuteReader();
                while (lectorArticulo.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)lectorArticulo["Id"];
                    aux.Codigo = (string)lectorArticulo["Codigo"];
                    aux.Nombre = (string)lectorArticulo["Nombre"];
                    aux.Descripcion = (string)lectorArticulo["Descripcion"];
                    aux.Marca = new Marca();
                    aux.Marca.IdMarca = (int)lectorArticulo["IdMarca"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.IdCategoria = (int)lectorArticulo["IdCategoria"];
                    aux.precio = (decimal)lectorArticulo["Precio"];

                    aux.Imagenes = new List<Imagen>();
                    Imagen imagen = new Imagen();
                    imagen.ImagenUrl = (string)lectorArticulo["ImagenUrl"];
                    imagen.IdArticulo = aux.Id;
                    aux.Imagenes.Add(imagen);

                    listaArticulos.Add(aux);
                }
                //lectorArticulo.Close();

                



            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al leer los datos: {ex.Message}");
            }
            finally
            {
                conexion.Close();
            }

            return listaArticulos;

        }
    }
}
