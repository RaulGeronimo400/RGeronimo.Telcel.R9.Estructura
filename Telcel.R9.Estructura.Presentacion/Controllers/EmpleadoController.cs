using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Telcel.R9.Estructura.Presentacion.Controllers
{
    public class EmpleadoController : Controller
    {
        [HttpGet]
        public ActionResult EmpleadoBuscar()
        {
            Telcel.R9.Estructura.Negocio.Empleado empleado = new Telcel.R9.Estructura.Negocio.Empleado();
            empleado.Puesto = new Negocio.Puesto();
            empleado.Departamento = new Negocio.Departamento();
            empleado.Empleados = new List<Object>();

            Telcel.R9.Estructura.Negocio.Result result = Telcel.R9.Estructura.Negocio.Empleado.GetAll();

            if (result.Correct)
            {
                empleado.Empleados = result.Objects;
            }
            else
            {
                ViewBag.Message = result.ErrorMessage;
            }
            return View(empleado);
        }

        [HttpPost]
        public ActionResult EmpleadoBuscar(string Nombre)
        {
            Telcel.R9.Estructura.Negocio.Empleado empleado = new Telcel.R9.Estructura.Negocio.Empleado();
            empleado.Puesto = new Negocio.Puesto();
            empleado.Departamento = new Negocio.Departamento();
            empleado.Empleados = new List<Object>();

            Telcel.R9.Estructura.Negocio.Result result = Telcel.R9.Estructura.Negocio.Empleado.GetAll(Nombre);

            if (result.Correct)
            {
                empleado.Empleados = result.Objects;
            }
            else
            {
                ViewBag.Message = result.ErrorMessage;
            }
            return View(empleado);
        }

        [HttpGet]
        public ActionResult EmpleadoEliminar(int EmpleadoID)
        {
            Telcel.R9.Estructura.Negocio.Result result = Telcel.R9.Estructura.Negocio.Empleado.Delete(EmpleadoID);
            if (result.Correct)
            {
                ViewBag.Message = "Se elimino Correctamente el empleado";
            }
            else
            {
                ViewBag.Message = "Ocurrio un problema al eliminar. Error: " + result.ErrorMessage;
            }
            return PartialView("Modal");
        }

        [HttpGet]
        public ActionResult CargaMasiva()
        {
            Telcel.R9.Estructura.Negocio.Empleado empleado = new Telcel.R9.Estructura.Negocio.Empleado();
            return View(empleado);
        }

        [HttpPost]
        public ActionResult CargaMasiva(Telcel.R9.Estructura.Negocio.Empleado empleado, HttpPostedFileBase txtEmpleado)
        {
            if (Session["ListEmpleados"] == null)
            {
                if (txtEmpleado == null)
                {
                    ViewBag.Message = "No se selecciono un archivo";
                    return PartialView("Modal");
                }
                else
                {
                    string extension = Path.GetExtension(txtEmpleado.FileName);
                    if (extension == ".txt")
                    {
                        var fecha = DateTime.Now.ToString("dd-MM-yyyy HHmmss");
                        string direccionTxt = Server.MapPath("~/CargaMasiva/TXT/" + Path.GetFileNameWithoutExtension(txtEmpleado.FileName) + "-" + fecha + ".txt");
                        txtEmpleado.SaveAs(direccionTxt);

                        empleado = Previsualizar(empleado, txtEmpleado);

                        Session["ListEmpleados"] = empleado.Empleados;
                    }
                    else
                    {
                        ViewBag.Message = "Archivo no valido";
                        return PartialView("Modal");
                    }
                }
            }
            else
            {
                if (txtEmpleado != null)
                {
                    Session["ListEmpleados"] = null;
                    CargaMasiva(empleado, txtEmpleado);
                }
                else
                {
                    var x = (List<object>)Session["ListEmpleados"];
                    empleado.Empleados = x;

                    CargarDatos(empleado);

                    Session["ListEmpleados"] = null;

                    ViewBag.Message = "Se han insertado los registros";
                    return PartialView("Modal");

                }
            }
            return View(empleado);
        }
        //TXT
        public static Telcel.R9.Estructura.Negocio.Empleado Previsualizar(Telcel.R9.Estructura.Negocio.Empleado empleado, HttpPostedFileBase txtEmpleado)
        {
            empleado.Empleados = new List<object>();
            using (StreamReader file = new StreamReader(txtEmpleado.InputStream, System.Text.Encoding.Default))
            {
                int counter = 0;
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    if (counter > 0)
                    {
                        var datos = ln.Split(',');

                        Telcel.R9.Estructura.Negocio.Empleado empleadoItem = new Negocio.Empleado();
                        empleadoItem.Puesto = new Negocio.Puesto();
                        empleadoItem.Departamento = new Negocio.Departamento();

                        empleadoItem.Nombre = datos[0];
                        empleadoItem.Departamento.DepartamentoID = (datos[1] != null) ? int.Parse(datos[1]) : int.Parse("0");
                        empleadoItem.Puesto.PuestoID = (datos[2] != null) ? int.Parse(datos[2]) : int.Parse("0");

                        empleado.Empleados.Add(empleadoItem);
                    }
                    counter++;
                }
            }
            return empleado;
        }
        public void CargarDatos(Telcel.R9.Estructura.Negocio.Empleado empleado)
        {
            List<string> list = new List<string>();
            int counter = 1;
            foreach (Telcel.R9.Estructura.Negocio.Empleado empleadoItem in empleado.Empleados)
            {
                Telcel.R9.Estructura.Negocio.Result result = Telcel.R9.Estructura.Negocio.Empleado.Add(empleadoItem);

                if (result.Correct)
                {
                    list.Add("La insersion del empleado: " + empleadoItem.Nombre + " fue exitosa.");
                }
                else
                {
                    list.Add("Hubo un error al insertar el empleado: " + empleadoItem.Nombre + " Error: ." + result.ErrorMessage);
                }
                counter++;
            }

            var fecha = DateTime.Now.ToString("dd-MM-yyyy HHmmss");
            string path = Server.MapPath("~/CargaMasiva/Reportes/" + "CargaMasiva_" + fecha + ".txt");

            using (StreamWriter archivo = System.IO.File.CreateText(path))
            {
                foreach (string msj in list)
                {
                    archivo.WriteLine(msj);
                }
            }
        }
    }
}