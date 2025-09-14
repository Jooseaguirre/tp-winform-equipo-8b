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
            ImagenNegocio imagenNegocio = new ImagenNegocio();


            try
            {
                //TRAER LISTA DE ARTICULOS
                datos.setearConsulta("SELECT A.Id, A.Nombre, A.Codigo, A.Descripcion, M.Descripcion AS Marca, C.Descripcion AS Categoria,\r\n       A.Precio, A.IdCategoria, A.IdMarca FROM ARTICULOS A LEFT JOIN MARCAS M ON A.IdMarca = M.Id LEFT JOIN CATEGORIAS C ON A.IdCategoria = C.Id ORDER BY A.Id;");
                datos.ejecutarLectura();

           
                //leer ARTICULOS
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];


                    if (!(datos.Lector["Descripcion"] is DBNull)){
                        aux.Codigo = (string)datos.Lector["Codigo"];
                    }
                    else{
                        aux.Codigo = "SIN DATOS";
                    }
                        

                    if (!(datos.Lector["Descripcion"] is DBNull)){
                        aux.Nombre = (string)datos.Lector["Nombre"];
                    }
                    else{
                        aux.Nombre = "SIN DATOS";
                    }
                        

                    if (!(datos.Lector["Descripcion"] is DBNull)) {
                        aux.Descripcion = (string)datos.Lector["Descripcion"];
                    }
                    else{
                        aux.Descripcion = "SIN DESCRIPCION";
                    }


                    aux.Marca = new Marca();
                    aux.Marca.IdMarca = datos.Lector["IdMarca"] != DBNull.Value ? (int)datos.Lector["IdMarca"] : 0;
                    aux.Marca.Descripcion = datos.Lector["Marca"] != DBNull.Value ? (string)datos.Lector["Marca"] : "SIN DATOS";

                    aux.Categoria = new Categoria();
                    aux.Categoria.IdCategoria = datos.Lector["IdCategoria"] != DBNull.Value ? (int)datos.Lector["IdCategoria"] : 0;
                    aux.Categoria.Descripcion = datos.Lector["Categoria"] != DBNull.Value ? (string)datos.Lector["Categoria"] : "SIN DATOS";

                    aux.precio = (decimal)datos.Lector["Precio"];

                    aux.Imagenes = imagenNegocio.ListarPorIdArticulo(aux.Id);

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


        //METODO AGREGAR
        public int agregar(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                if (nuevo.Marca == null) nuevo.Marca = new Marca();
                if (nuevo.Categoria == null) nuevo.Categoria = new Categoria();

                datos.setearConsulta(@"INSERT INTO ARTICULOS(Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio)
                                        VALUES(@codigo, @nombre, @descripcion, @idMarca, @idCategoria, @precio);
                                        SELECT CAST(SCOPE_IDENTITY() AS int);");

                datos.setearParametro("@codigo", nuevo.Codigo);
                datos.setearParametro("@nombre", nuevo.Nombre);
                datos.setearParametro("@descripcion", nuevo.Descripcion);
                datos.setearParametro("@idMarca", nuevo.Marca.IdMarca);
                datos.setearParametro("@idCategoria", nuevo.Categoria.IdCategoria);
                datos.setearParametro("@precio", nuevo.precio);

                return datos.obtenerId();//Este metodo debe devolver el ID generado
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificar(Articulo modificar)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                if (modificar.Marca == null) modificar.Marca = new Marca();
                if (modificar.Categoria == null) modificar.Categoria = new Categoria();

                datos.setearConsulta(@"UPDATE ARTICULOS set Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @idMarca,  IdCategoria = @idCategoria, Precio = @precio
                                   WHERE Id = @id");

                datos.setearParametro("@codigo", modificar.Codigo);
                datos.setearParametro("@nombre", modificar.Nombre);
                datos.setearParametro("@descripcion", modificar.Descripcion);
                datos.setearParametro("@idMarca", modificar.Marca.IdMarca);
                datos.setearParametro("@idCategoria", modificar.Categoria.IdCategoria);
                datos.setearParametro("@precio", modificar.precio);
                datos.setearParametro("@id", modificar.Id);

                datos.ejecutarAccion();


            }

            catch (Exception ex) { throw ex; }

            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("Delete from Articulos where id =@id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }

            catch (Exception ex) { throw ex; }
        }

        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> listaArticulos = new List<Articulo>();   
            AccesoDatos datos = new AccesoDatos();  

            try
            {
                string consulta = "SELECT A.Id, A.Nombre, A.Codigo, A.Descripcion, M.Descripcion AS Marca, C.Descripcion AS Categoria, A.Precio FROM ARTICULOS A LEFT JOIN MARCAS M ON A.IdMarca = M.Id LEFT JOIN CATEGORIAS C ON A.IdCategoria = C.Id WHERE ";
                if (campo == "Precio")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "A.Precio > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "A.Precio < " + filtro;
                            break;
                        case "Igual a":
                            consulta += "A.Precio = " + filtro;
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "A.Nombre LIKE '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "A.Nombre LIKE '%" + filtro + "'";
                            break;
                        case "Contiene":
                            consulta += "A.Nombre LIKE '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Marca")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "M.Descripcion LIKE '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "M.Descripcion LIKE '%" + filtro + "'";
                            break;
                        case "Contiene":
                            consulta += "M.Descripcion LIKE '%" + filtro + "%'";
                            break;
                    }   


                }
                    
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();  
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Marca = new Marca();
                    aux.Marca.Descripcion = datos.Lector["Marca"] != DBNull.Value ? (string)datos.Lector["Marca"] : "SIN DATOS";
                    aux.Categoria = new Categoria();
                    aux.Categoria.Descripcion = datos.Lector["Categoria"] != DBNull.Value ? (string)datos.Lector["Categoria"] : "SIN DATOS";
                    aux.precio = (decimal)datos.Lector["Precio"];
                    listaArticulos.Add(aux);
                }


                return listaArticulos;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
