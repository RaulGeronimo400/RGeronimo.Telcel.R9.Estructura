using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telcel.R9.Estructura.Negocio
{
    public class Puesto
    {
        public int PuestoID { get; set; }
        public string Descripcion { get; set; }
        public List<object> Puestos { get; set; }

        public static Result GetAll()
        {
            Result result = new Result();
            try
            {
                using (Telcel.R9.Estructura.AccesoDatos.RGeronimoEstructuraEntities context = new AccesoDatos.RGeronimoEstructuraEntities())
                {
                    var query = context.PuestoGetAll().ToList();
                    result.Objects = new List<object>();

                    if (query.Count > 0)
                    {
                        foreach (var obj in query)
                        {
                            Puesto puesto = new Puesto();
                            puesto.PuestoID = obj.PuestoID;
                            puesto.Descripcion = obj.Descripcion;
                            result.Objects.Add(puesto);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrio un problema al mostrar la lista";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
    }
}
