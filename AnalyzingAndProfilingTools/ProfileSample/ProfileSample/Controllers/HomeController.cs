using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ProfileSample.DAL;
using ProfileSample.Models;

namespace ProfileSample.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {

            //var sources = context.ImgSources.Take(20).Select(x => x.Id);

            //foreach (var id in sources)
            //{
            //    var item = context.ImgSources.Find(id);

            //    var obj = new ImageModel()
            //    {
            //        Name = item.Name,
            //        Data = item.Data
            //    };

            //    model.Add(obj);
            //} 

            var temp = await Convert(); // First for added images db

            var model = new List<ImageModel>();
            using (var context = new ProfileSampleEntities())
            {
                model = context.ImgSources.Take(24).AsNoTracking()
                .Select(x => new ImageModel()
                {
                    Name = x.Name,
                    Data = x.Data
                }).ToList();
            }




            return View(model);
        }

        public async Task<ActionResult> Convert()
        {
            var files = Directory.GetFiles(Server.MapPath("~/Content/Img"), "*.jpg");

            using (var context = new ProfileSampleEntities())
            {
                foreach (var file in files)
                {
                    using (var stream = new FileStream(file, FileMode.Open))
                    {
                        byte[] buff = new byte[stream.Length];

                        stream.ReadAsync(buff, 0, (int)stream.Length);

                        var entity = new ImgSource()
                        {
                            Name = Path.GetFileName(file),
                            Data = buff,
                        };

                        context.ImgSources.Add(entity);
                      await  context.SaveChangesAsync();
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your about page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}