using Microsoft.AspNet.Identity;
using MusicStation.Data;
using MusicStation.Models.Songs;
using MusicStation.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace MusicStation.Controllers
{
    public class SongController : Controller
    {
        [HttpGet]
        public ActionResult List(int page = 1, string user = null, string genre = null, string search = null)
        {
            using (var db = new MusicStationDbContext())
            {
                var pageSize = 7;

                var songsQuery = db.Songs.AsQueryable();

                if (search != null)
                {
                    songsQuery = songsQuery
                        .Where(s => s.Artist.ToLower().Contains(search.ToLower()) || s.Title.ToLower().Contains(search.ToLower()));
                }

                if (user != null)
                {
                    songsQuery = songsQuery
                       .Where(s => s.User.UserName == user);
                }

                if (genre != null)
                {
                    songsQuery = songsQuery
                      .Where(s => s.Genre.ToString() == genre);

                }

                var songs = songsQuery
                    .OrderByDescending(s => s.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(s => new SongListModel
                    {
                        Id = s.Id,
                        Artist = s.Artist,
                        Title = s.Title,
                        Genre = s.Genre,
                        ImagePath = s.ImagePath,
                        FilePath = s.FilePath,
                        User = s.User

                    })
                    .ToList();

                ViewBag.CurrentPage = page;

                return View(songs);
            }
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            using (var db = new MusicStationDbContext())
            {
                var song = db.Songs
                    .Where(s => s.Id == id)
                    .Include(s => s.User)
                    .FirstOrDefault();

                if (song == null)
                {
                    return HttpNotFound();
                }

                var model = new SongDetailsModel
                {
                    Artist = song.Artist,
                    Title = song.Title,
                    Details = song.Details,
                    Genre = song.Genre,
                    ImagePath = song.ImagePath,
                    FilePath = song.FilePath,
                    Id = song.Id,
                    User = song.User
                };

                return View(model);
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
            if (ModelState.IsValid)
            {
                using (var db = new MusicStationDbContext())
                {
                    var userId = User.Identity.GetUserId();

                    song.UserId = userId;

                    if (image != null)
                    {
                        var allowedImageTypes = new[] { "image/jpg", "image/jpeg", "image/png" };

                        if (allowedImageTypes.Contains(image.ContentType))
                        {
                            var imagesPath = "/Content/Images/";
                            var imageName = userId.ToString() + image.FileName;
                            var imageUploadPath = imagesPath + imageName;
                            var imagePhysicalPath = Server.MapPath(imageUploadPath);

                            image.SaveAs(imagePhysicalPath);

                            song.ImagePath = imageUploadPath;
                        }
                    }

                    var allowedAudioTypes = new[] { "audio/mp3" };

                    if (allowedAudioTypes.Contains(FilePath.ContentType))
                    {
                        var filePath = "/Content/Songs/";
                        var fileName = userId.ToString() + FilePath.FileName;
                        var uploadPath = filePath + fileName;
                        var physicalPath = Server.MapPath(uploadPath);
                        FilePath.SaveAs(physicalPath);

                        song.FilePath = uploadPath;

                        db.Songs.Add(song);
                        db.SaveChanges();

                        return RedirectToAction("List");
                    }
                    else
                    {
                        return RedirectToAction("SongError");
                    }
                }
            }

            return View(song);
        }


        [Authorize]
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            using (var db = new MusicStationDbContext())
            {
                var song = db.Songs
                    .Where(s => s.Id == id)
                    .Include(s => s.User)
                    .FirstOrDefault();

                if (!IsAuthorizedToChange(song))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (song == null)
                {
                    return HttpNotFound();
                }

                return View(song);
            }
        }

        [Authorize]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            using (var db = new MusicStationDbContext())
            {
                var song = db.Songs
                    .Where(s => s.Id == id)
                    .Include(s => s.User)
                    .FirstOrDefault();

                if (song == null)
                {
                    return HttpNotFound();
                }

                var filePath = Request.MapPath(song.FilePath);
                var imagePath = Request.MapPath(song.ImagePath);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                db.Songs.Remove(song);
                db.SaveChanges();

                return RedirectToAction("List");
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            using (var db = new MusicStationDbContext())
            {
                var song = db.Songs
                    .Where(s => s.Id == id)
                    .Include(s => s.User)
                    .FirstOrDefault();

                if (!IsAuthorizedToChange(song))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (song == null)
                {
                    return HttpNotFound();
                }

                var model = new SongEditModel();

                model.Id = song.Id;
                model.Artist = song.Artist;
                model.Title = song.Title;
                model.Details = song.Details;
                model.Genre = song.Genre;

                return View(model);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(SongEditModel model, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MusicStationDbContext())
                {
                    var song = db.Songs
                        .Where(s => s.Id == model.Id)
                        .FirstOrDefault();

                    if (song == null)
                    {
                        return HttpNotFound();
                    }

                    var originalImageUploadPath = Server.MapPath(song.ImagePath);

                    if(image == null)
                    {
                        model.ImagePath = song.ImagePath;
                    }
                    else
                    {
                        var imagesPath = "/Content/Images/";
                        var newImageName = song.Id.ToString() + image.FileName;
                        var newImageUploadPath = imagesPath + newImageName;
                        var newImagePhysicalPath = Server.MapPath(newImageUploadPath);
                        if(song.ImagePath != null)
                        {
                            System.IO.File.Delete(originalImageUploadPath);
                        }
                        

                        image.SaveAs(newImagePhysicalPath);
                        model.ImagePath = newImageUploadPath;
                    }

                    song.Id = model.Id;
                    song.Artist = model.Artist;
                    song.Title = model.Title;
                    song.Details = model.Details;
                    song.Genre = model.Genre;
                    song.ImagePath = model.ImagePath;

                    db.Entry(song).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Details", new { id = song.Id });
                }
            }
            return View(model);
        }

        private bool IsAuthorizedToChange(Song song)
        {
            bool IsAdmin = this.User.IsInRole("Admin");
            bool IsAuthor = song.IsAuthor(this.User.Identity.GetUserName());

            return IsAdmin || IsAuthor;
        }

        [HttpGet]
        public ActionResult SongError()
        {
            return View();
        }
    }
}