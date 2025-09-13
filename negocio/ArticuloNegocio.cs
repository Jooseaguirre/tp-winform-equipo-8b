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
                datos.setearConsulta("SELECT A.Id, A.Nombre, A.Codigo, A.Descripcion, M.Descripcion AS Marca, C.Descripcion AS Categoria, A.Precio FROM ARTICULOS A LEFT JOIN MARCAS M ON A.IdMarca = M.Id LEFT JOIN CATEGORIAS C ON A.IdCategoria = C.Id");
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
                    if (!(datos.Lector["Marca"] is DBNull)){
                        aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    }
                    else
                    {
                        aux.Marca.Descripcion = "SIN DATOS";
                    }
                    
                    
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
                datos.setearConsulta("INSERT INTO ARTICULOS(Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) " +
                                     "VALUES('" + nuevo.Codigo + "', '" + nuevo.Nombre + "', '" + nuevo.Descripcion + "', @IdMarca, @IdCategoria, " + nuevo.precio + "); " +
                                     "SELECT SCOPE_IDENTITY();");
                datos.setearParametro("@IdMarca", nuevo.Marca.IdMarca);
                datos.setearParametro("@IdCategoria", nuevo.Categoria.IdCategoria);
                // datos.ejecutarAccion();

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
