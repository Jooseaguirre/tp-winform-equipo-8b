using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;

namespace negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> Listar()
        {
            List<Articulo> listaArticulos = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
           
            

            try
            {
                //TRAER LISTA DE ARTICULOS
                datos.setearConsulta("SELECT A.Id, A.Nombre, A.Codigo, A.Descripcion, M.Descripcion AS Marca, C.Descripcion AS Categoria, A.Precio, I.ImagenUrl FROM ARTICULOS A LEFT JOIN MARCAS M ON A.IdMarca = M.Id LEFT JOIN CATEGORIAS C ON A.IdCategoria = C.Id LEFT JOIN IMAGENES I ON A.Id = I.IdArticulo;");
                datos.ejecutarLectura();

           
                //leer ARTICULOS
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Marca = new Marca();
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Descripcion = datos.Lector["Categoria"] != DBNull.Value ? (string)datos.Lector["Marca"] : "SIN DATOS";
                    aux.precio = (decimal)datos.Lector["Precio"];

                    aux.Imagenes = new List<Imagen>();
                    Imagen imagen = new Imagen();
                    imagen.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    imagen.IdArticulo = aux.Id;
                    aux.Imagenes.Add(imagen);

                    listaArticulos.Add(aux);
                }
                return listaArticulos;


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

            //return listaArticulos;

        }
    }
}
