using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telcel.R9.Estructura.Negocio
{
    public class Empleado
    {
        public int EmpleadoID { get; set; }
        public string Nombre { get; set; }
        public Puesto Puesto { get; set; }
        public Departamento Departamento { get; set; }
        public List<object> Empleados { get; set; }

        public static Result Add(Empleado empleado)
        {
            Result result = new Result();
            try
            {
                using (Telcel.R9.Estructura.AccesoDatos.RGeronimoEstructuraEntities context = new AccesoDatos.RGeronimoEstructuraEntities())
                {
                    var query = context.EmpleadoAdd(
                        empleado.Nombre,
                        empleado.Puesto.PuestoID,
                        empleado.Departamento.DepartamentoID
                        );

                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrio un problema al insertar";
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
        public static Result Delete(int EmpleadoID)
        {
            Result result = new Result();
            try
            {
                using (Telcel.R9.Estructura.AccesoDatos.RGeronimoEstructuraEntities context = new AccesoDatos.RGeronimoEstructuraEntities())
                {
                    var query = context.EmpleadoDelete(EmpleadoID);

                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrio un problema al eliminar";
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
        public static Result GetAll()
        {
            Result result = new Result();
            try
            {
                using (Telcel.R9.Estructura.AccesoDatos.RGeronimoEstructuraEntities context = new AccesoDatos.RGeronimoEstructuraEntities())
                {
                    var query = context.EmpleadoGetAll().ToList();
                    result.Objects = new List<object>();

                    if (query.Count > 0)
                    {
                        foreach (var obj in query)
                        {
                            Empleado empleado = new Empleado();
                            empleado.Puesto = new Puesto();
                            empleado.Departamento = new Departamento();

                            empleado.EmpleadoID = obj.EmpleadoID;
                            empleado.Nombre = obj.Nombre;

                            empleado.Puesto.PuestoID = obj.PuestoID.Value;
                            empleado.Puesto.Descripcion = obj.DescripcionPuesto;

                            empleado.Departamento.DepartamentoID = obj.DepartamentoID.Value;
                            empleado.Departamento.Descripcion = obj.DescripcionPuesto;

                            result.Objects.Add(empleado);
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
        public static Result GetAll(string Nombre)
        {
            Nombre = (Nombre != null) ? Nombre : "";

            Result result = new Result();
            try
            {
                using (Telcel.R9.Estructura.AccesoDatos.RGeronimoEstructuraEntities context = new AccesoDatos.RGeronimoEstructuraEntities())
                {
                    var query = context.EmpleadoGetByNombre(Nombre).ToList();
                    result.Objects = new List<object>();

                    if (query.Count > 0)
                    {
                        foreach (var obj in query)
                        {
                            Empleado empleado = new Empleado();
                            empleado.Puesto = new Puesto();
                            empleado.Departamento = new Departamento();

                            empleado.EmpleadoID = obj.EmpleadoID;
                            empleado.Nombre = obj.Nombre;

                            empleado.Puesto.PuestoID = obj.PuestoID.Value;
                            empleado.Puesto.Descripcion = obj.DescripcionPuesto;

                            empleado.Departamento.DepartamentoID = obj.DepartamentoID.Value;
                            empleado.Departamento.Descripcion = obj.DescripcionPuesto;

                            result.Objects.Add(empleado);
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
