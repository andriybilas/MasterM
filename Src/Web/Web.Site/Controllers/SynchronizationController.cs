using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Litium.Common;
using Litium.Domain.Entities.ECommerce;
using Litium.Domain.Entities.Media;
using Litium.Domain.Entities.ProductCatalog;
using Litium.Resources;
using Site.Infrastuctures.ModelHelpers.Synchronization;
using Site.Infrastuctures.Utility;

namespace Web.Site.Controllers
{
    public class SynchronizationController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadProductsAsync(IEnumerable<HttpPostedFileBase> attachments)
        {
            foreach (var file in attachments)
            {
                FileType fileType;
                Enum.TryParse(file.ContentType.Replace("text/", String.Empty), out fileType);

                if (fileType != FileType.other && fileType == FileType.xml)
                {
                    HttpPostedFileBase file1 = file;
                    var synch = new SynchService();
                    IEnumerable<Product> products = synch.CreateProductsFromXDocument(XDocument.Load(file1.InputStream));
                    var synchAction = new SynchActionService();
                    ActionHelper.TryExecute(() => synchAction.InsertProductToRepository(products), ModelState);
                }
                else
                {
                    ModelState.AddModelError("NotSupportedFormat", WebStroreResource.NotSupportedFormat);
					return Json(new { status = "Error", message = WebStroreResource.NotSupportedFormat }, "text/plain");
                }
            }
            return Content(String.Empty);
        }

        public FileResult DownloadOdersXml (SynchModel model)
        {
            IEnumerable<Order> orders;

            if (model.FromLastSynch)
                orders = Repository.Data.Get<Order>().Where(x => x.LastSynchDate == null).All();
            else
                orders = Repository.Data.Get<Order>().Where(x => x.CreateDate >= model.StartSynchDate &&
                        x.CreateDate <= model.EndSynchDate.Value.AddDays(1)).All();

            var service = new SynchService();
            XDocument xDocument = service.ConvertToXDocument(orders);
            Stream stream = new MemoryStream();
            xDocument.Save(stream);
            stream.Position = 0;
            return File(stream, "text/xml", "Orders.xml");
        }
    }
}