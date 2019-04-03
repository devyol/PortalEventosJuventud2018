using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EVE01.UI.Controllers
{
    public class EventoController : Controller
    {
        //
        // GET: /Evento/

        public ActionResult AsignacionEvento()
        {
            return View();
        }

        public ActionResult ListarEventosMantenimiento()
        {
            return View();
        }

    }
}
