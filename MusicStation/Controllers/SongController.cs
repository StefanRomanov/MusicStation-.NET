using Microsoft.AspNet.Identity;
using MusicStation.Data;
using MusicStation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStation.Controllers
{
    public class SongController : Controller
    {
        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Add(Song song, HttpPostedFileBase FilePath)
        {
            if(ModelState.IsValid)
            {
                using (var db = new MusicStationDbContext())
                {
                    var userId = User.Identity.GetUserId();

                    song.UserId = userId;

                    var filePath = "/Content/Songs/";

                    var fileName = FilePath.FileName;

                    var uploadPath = filePath + fileName;
                    var physicalPath = Server.MapPath(uploadPath);

                    FilePath.SaveAs(physicalPath);

                    song.FilePath = uploadPath;

                    db.Songs.Add(song);
                    db.SaveChanges();

                    return RedirectToAction("List");
                }
            }

            return View(song);
        }
    }
}