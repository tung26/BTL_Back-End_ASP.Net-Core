using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using YourHouse.Models;
using YourHouse.Models.Entities;

namespace YourHouse.Controllers
{
    public class MyArticleController : Controller
    {
        private readonly YourHouseContext _context;

        public MyArticleController(YourHouseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var articleList = _context.Articles.Select(a => new
            {
                a.ArticleId,
                a.Title,
                city = _context.Cities.FirstOrDefault(c => c.CityId == a.CityAr).CityName,
                district = _context.Districts.FirstOrDefault(c => c.DistrictId == a.DistrictAr).DistrictName,
                a.S,
                a.Price,
                a.CreateAt
            }).OrderByDescending(a => a.CreateAt).ToList();

            ViewData["articleList"] = articleList;

            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["City"] = _context.Cities.ToList();
            ViewData["District"] = _context.Districts.Select(d => new District { DistrictId = d.DistrictId, DistrictName = d.DistrictName, CityId = d.CityId }).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Add([FromForm] ModelAddArticle art)
        {
            ViewData["City"] = _context.Cities.ToList();
            ViewData["District"] = _context.Districts.Select(d => new District { DistrictId = d.DistrictId, DistrictName = d.DistrictName, CityId = d.CityId }).ToList();
            art.Status = 1;
            ModelState.Remove("Status");
            if (art.Type == "ChungCu")
            {
                ModelState.Remove("DoorDrt");
                ModelState.Remove("Floors");
                ModelState.Remove("DoorDrt");
            }
            else if (art.Type == "Office")
            {
                ModelState.Remove("Floors");
                ModelState.Remove("WaterPrice");
                ModelState.Remove("ElectricPrice");
                ModelState.Remove("BedRoom");
                ModelState.Remove("BathRoom");
                ModelState.Remove("MaxPerson");
            }
            else if (art.Type == "Tro")
            {
                ModelState.Remove("DoorDrt");
                ModelState.Remove("BedRoom");
                ModelState.Remove("BathRoom");
                ModelState.Remove("Floors");
            }
            else if (art.Type == "House")
            {
                ModelState.Remove("MaxPerson");
                ModelState.Remove("DoorDrt");
                ModelState.Remove("WaterPrice");
                ModelState.Remove("Floor");
                ModelState.Remove("ElectricPrice");
            }
            //foreach (var error in ModelState)
            //{
            //    foreach (var subError in error.Value.Errors)
            //    {
            //        Console.WriteLine($"Lỗi tại {error.Key}: {subError.ErrorMessage}");
            //    }
            //}
            if (ModelState.IsValid)
            {
                ImagesArticle imagesArticle = new ImagesArticle()
                {
                    ImageArticle = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSq8aVvwt226C1Kx4QqQzL5IibUHcRbb1XCtw&s",
                };

                Article article = new Article()
                {
                    AccountId = 1,
                    Title = art.Title,
                    DescAr = art.Desc,
                    Addr = art.Address,
                    CityAr = (int)art.City,
                    DistrictAr = (int)art.District,
                    S = (decimal)art.S,
                    Price = (decimal)art.Price,
                    TienCoc = (decimal)art.TienCoc,
                    TypeAr = art.Type,
                    ImagesArticles = new List<ImagesArticle>() { imagesArticle }
                };

                if (art.Type == "ChungCu")
                {
                    ChungCu chungCu = new ChungCu()
                    {
                        Floor = (int)art.Floor,
                        BedRoom = (int)art.BedRoom,
                        BathRoom = (int)art.BathRoom,
                        MaxPerson = (int)art.MaxPerson,
                        WaterPrice = (int)art.WaterPrice,
                        ElectricPrice = (int)art.ElectricPrice,
                        Article = article,
                    };
                    _context.ChungCus.Add(chungCu);
                    _context.SaveChanges();

                }
                else if (art.Type == "Office")
                {
                    Office office = new Office()
                    {
                        Floor = (int)art.Floor,
                        DoorDrt = (int)art.DoorDrt,
                        Article = article,
                    };
                    _context.Offices.Add(office);
                    _context.SaveChanges();
                }
                else if (art.Type == "Tro")
                {
                    Tro tro = new Tro()
                    {
                        Floor = (int)art.Floor,
                        MaxPerson = (int)art.MaxPerson,
                        WaterPrice= (int)art.WaterPrice,
                        ElectricPrice= (int)art.ElectricPrice,
                        Article = article,
                    };
                    _context.Tros.Add(tro);
                    _context.SaveChanges();
                }
                else if (art.Type == "House")
                {
                    House house = new House()
                    {
                        Floors = (int)art.Floors,
                        BathRoom = (int)art.BathRoom,
                        BedRoom = (int)art.BedRoom,
                        Article = article,
                    };
                    
                    _context.Houses.Add(house);
                    _context.SaveChanges();
                }

                return RedirectToAction("Index", "Home");
            }
            return View(art);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewData["id"] = id;
            ViewData["City"] = _context.Cities.ToList();
            ViewData["District"] = _context.Districts.Select(d => new District { DistrictId = d.DistrictId, DistrictName = d.DistrictName, CityId = d.CityId }).ToList();

            ModelAddArticle modelArticle = new ModelAddArticle();

            var art = _context.Articles.Where(a => a.ArticleId == id).FirstOrDefault();
            var tro = _context.Tros.Where(a => a.ArticleId == id).FirstOrDefault();
            var chungCu = _context.ChungCus.Where(a => a.ArticleId == id).FirstOrDefault();
            var house = _context.Houses.Where(a => a.ArticleId == id).FirstOrDefault();
            var office = _context.Offices.Where(a => a.ArticleId == id).FirstOrDefault();

            if(tro != null)
            {
                modelArticle.Floor = tro.Floor;
                modelArticle.MaxPerson = tro.MaxPerson;
                modelArticle.WaterPrice = (double)tro.WaterPrice;
                modelArticle.ElectricPrice = (double)tro.ElectricPrice;
            }
            else if(chungCu != null)
            {
                modelArticle.BedRoom = chungCu.BedRoom; 
                modelArticle.BathRoom = chungCu.BathRoom;
                modelArticle.Floor = chungCu.Floor;
                modelArticle.MaxPerson = chungCu.MaxPerson;
                modelArticle.WaterPrice = (double)chungCu.WaterPrice;
                modelArticle.ElectricPrice = (double)chungCu.ElectricPrice;
            }
            else if (house != null)
            {
                modelArticle.BedRoom = house.BedRoom;
                modelArticle.BathRoom = house.BathRoom;
                modelArticle.Floors = house.Floors;
            }
            else if(office != null)
            {
                modelArticle.Floor = office.Floor;
                modelArticle.DoorDrt = office.DoorDrt;
            }

            modelArticle.Title = art.Title;
            modelArticle.Desc = art.DescAr;
            modelArticle.Address = art.Addr;
            modelArticle.City = art.CityAr;
            modelArticle.District = art.DistrictAr;
            modelArticle.S = (double)art.S;
            modelArticle.Price = (double)art.Price;
            modelArticle.TienCoc = (double)art.TienCoc;
            modelArticle.Type = art.TypeAr;

            return View(modelArticle);
        }

        [HttpPost]
        public IActionResult Edit([FromForm]int id, [FromForm] ModelAddArticle art)
        {
            ViewData["City"] = _context.Cities.ToList();
            ViewData["District"] = _context.Districts.Select(d => new District { DistrictId = d.DistrictId, DistrictName = d.DistrictName, CityId = d.CityId }).ToList();
            
            ModelState.Remove("Status");
            if (art.Type == "ChungCu")
            {
                ModelState.Remove("DoorDrt");
                ModelState.Remove("Floors");
                ModelState.Remove("DoorDrt");
            }
            else if (art.Type == "Office")
            {
                ModelState.Remove("Floors");
                ModelState.Remove("WaterPrice");
                ModelState.Remove("ElectricPrice");
                ModelState.Remove("BedRoom");
                ModelState.Remove("BathRoom");
                ModelState.Remove("MaxPerson");
            }
            else if (art.Type == "Tro")
            {
                ModelState.Remove("DoorDrt");
                ModelState.Remove("BedRoom");
                ModelState.Remove("BathRoom");
                ModelState.Remove("Floors");
            }
            else if (art.Type == "House")
            {
                ModelState.Remove("MaxPerson");
                ModelState.Remove("DoorDrt");
                ModelState.Remove("WaterPrice");
                ModelState.Remove("Floor");
                ModelState.Remove("ElectricPrice");
            }

            var article = _context.Articles.FirstOrDefault(a => a.ArticleId == id);
            article.Title = art.Title;
            article.DescAr = art.Desc;
            article.Addr = art.Address;
            article.CityAr = (int)art.City;
            article.DistrictAr = (int)art.District;
            article.S = (decimal)art.S;
            article.Price = (decimal)art.Price;
            article.TienCoc = (decimal)art.TienCoc;
            article.TypeAr = art.Type;

            switch (art.Type)
            {
                case "Tro":
                    var tro = _context.Tros.FirstOrDefault(t => t.ArticleId == id);
                    if (tro != null)
                    {
                        tro.Floor = (int)art.Floor;
                        tro.MaxPerson = (int)art.MaxPerson;
                        tro.WaterPrice = (decimal)art.WaterPrice;
                        tro.ElectricPrice = (decimal)art.ElectricPrice;
                    }
                    break;
                case "ChungCu":
                    var cc = _context.ChungCus.FirstOrDefault(t => t.ArticleId == id);
                    if (cc != null)
                    {
                        cc.Floor = (int)art.Floor;
                        cc.MaxPerson = (int)art.MaxPerson;
                        cc.WaterPrice = (decimal)art.WaterPrice;
                        cc.ElectricPrice = (decimal)art.ElectricPrice;
                        cc.BedRoom = art.BedRoom ?? 0;
                        cc.BathRoom = art.BathRoom ?? 0;
                    }
                    break;
                case "House":
                    var house = _context.Houses.FirstOrDefault(h => h.ArticleId == id);
                    if (house != null)
                    {
                        house.Floors = (int)art.Floors;
                        house.BedRoom = (int)art.BedRoom;
                        house.BathRoom = (int)art.BathRoom;
                    }
                    break;
                case "Office":
                    var office = _context.Offices.FirstOrDefault(o => o.ArticleId == id);
                    if (office != null)
                    {
                        office.Floor = art.Floor ?? 0;
                        office.DoorDrt = (int)art.DoorDrt;
                    }
                    break;
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var art = _context.Articles
                .Include(a => a.Tro)
                .Include(a => a.ChungCu)
                .Include(a => a.Office)
                .Include(a => a.House)
                .Include(a => a.ImagesArticles)
                .Where(a => a.ArticleId == id).FirstOrDefault();
            Console.WriteLine(art);
            if (art.Tro != null)
            {
                _context.Tros.Remove(art.Tro);
            }
            if (art.ChungCu != null)
            {
                _context.ChungCus.Remove(art.ChungCu);
            }
            if (art.Office != null)
            {
                _context.Offices.Remove(art.Office);
            }
            if (art.House != null)
            {
                _context.Houses.Remove(art.House);
            }
            if (art.ImagesArticles != null && art.ImagesArticles.Any())
            {
                _context.ImagesArticles.RemoveRange(art.ImagesArticles);
            }
            if (art != null)
            {
                _context.Articles.Remove(art);
                _context.SaveChanges();
            }
            else
            {
                return Json(new { success = false, message = "Xóa bài đăng thất bại." });
            }

            return Json(new { success = true, message = "Xóa bài đăng thành công!" });
        }

        public IActionResult Type(string t)
        {
            var model = new ModelAddArticle { Type = t };

            if (t == "Tro")
            {
                return PartialView("_PartialAddTro", model);
            }
            else if (t == "ChungCu")
            {
                return PartialView("_PartialAddChungCu", model);
            }
            else if (t == "Office")
            {
                return PartialView("_PartialAddOffice", model);
            }
            else if (t == "House")
            {
                return PartialView("_PartialAddHouse", model);
            }

            return new EmptyResult();
        }
    }
}
