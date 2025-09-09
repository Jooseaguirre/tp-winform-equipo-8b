using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPWinForm_equipo_8BB
{
    internal class Marca
    {

        public int IdMarca { get; set; }

        public string Descripcion { get; set; }

        public override string ToString()
        {
            return IdMarca.ToString();
        }

    }


}
