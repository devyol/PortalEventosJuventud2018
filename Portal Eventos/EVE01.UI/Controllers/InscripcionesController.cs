using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EVE01.DO.DATA;
using EVE01.UI.Clases;
using EVE01.UI.Models;
using Microsoft.Reporting.WebForms;

namespace EVE01.UI.Controllers
{
    public class InscripcionesController : Controller
    {
        //
        // GET: /Inscripciones/

        public ActionResult Inscripciones()
        {
            return View();
        }

        public ActionResult registroParticipante()
        {
            return View();
        }

        public FileResult recibo(decimal idmov)
        {
            Recibo impresion = new Recibo();
            impresion.idMovimiento = idmov;
            List<Recibo> servicio = impresion.impresionRecibo();

            ReportViewer rv = new ReportViewer();
            rv.ProcessingMode = ProcessingMode.Local;
            rv.LocalReport.ReportPath = Server.MapPath("~/Reportes/rptReciboImpresion.rdlc");
            rv.LocalReport.DataSources.Clear();
            ReportDataSource dsEncabezado = new ReportDataSource("dts_recibo", servicio);
            rv.LocalReport.DataSources.Add(dsEncabezado);
            rv.LocalReport.Refresh();

            byte[] streamBytes = null;
            string mimeType = "";
            string enconding = "";
            string filenameExtension = "";
            string[] streamids = null;
            Warning[] warnings = null;

            streamBytes = rv.LocalReport.Render("PDF", null, out mimeType, out enconding, out filenameExtension, out streamids, out warnings);
            return File(streamBytes, mimeType);
        }

        

    }
}
