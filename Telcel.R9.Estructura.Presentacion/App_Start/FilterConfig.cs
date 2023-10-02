using System.Web;
using System.Web.Mvc;

namespace Telcel.R9.Estructura.Presentacion
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
