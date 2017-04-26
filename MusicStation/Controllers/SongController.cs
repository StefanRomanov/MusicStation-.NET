using Microsoft.AspNet.Identity;
using MusicStation.Data;
using MusicStation.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStation.Controllers
{
    public class SongController : Controller
    {
        [HttpGet]
        public ActionResult List()
        {
            using(var db = new MusicStationDbContext())
            {
                var songs = db.Songs
                    .Include(s => s.User)
                    .ToList();

                return View(songs);
            }
        }

        public ActionResult ListByUser(string user)
        {
            using (var db = new MusicStationDbContext())
            {
                if(user == null)
                {
                    return HttpNotFound()
;                }

                var songs = db.Songs
                    .Where(s => s.User.UserName == user)
                    .ToList();

                return View(songs);
            }
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }

            using (var db = new MusicStationDbContext())
            {
                var song = db.Songs
                    .Where(s => s.Id == id)
                    .Include(s => s.User)
                    .First();
                    

                if(song == null)
                {
                    return HttpNotFound();
                }

                return View(song);
            }
        }


        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Add(Song song, HttpPostedFileBase FilePath, HttpPostedFileBase image)
        {
            if(ModelState.IsValid)
            {
                using (var db = new MusicStationDbContext())
                {
                    var userId = User.Identity.GetUserId();

                    song.UserId = userId;

                    var filePath = "/Content/Songs/";
                    var imagesPath = "/Content/Images/";
                    var fileName = FilePath.FileName;
                    var uploadPath = filePath + fileName;
                    var physicalPath = Server.MapPath(uploadPath);

                    if(image != null)
                    {
                        var imageName = image.FileName;
                        var imageUploadPath = imagesPath + imageName;
                        var imagePhysicalPath = Server.MapPath(imageUploadPath);

                        image.SaveAs(imagePhysicalPath);

                        song.ImagePath = imageUploadPath;
                    }

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