using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using YourHouse.Application.DTOs;
using YourHouse.Application.Interfaces;
using YourHouse.Web.Infrastructure.Data;
using YourHouse.Web.Models;


namespace YourHouse.Web.Controllers
{
    public class MyArticleController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly ICityService _cityService;
        private readonly IDistrictService _districtService;
        public MyArticleController(IArticleService articleService, IDistrictService districtService, ICityService cityService)
        {
            _articleService = articleService;
            _cityService = cityService;
            _districtService = districtService;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsLogin)
            {
                return RedirectToAction("Login", "Account");
            }

            var articleList = await _articleService.GetAllArticleAsync();
            var cities = await _cityService.GetAllCityAsync();
            var districtes = await _districtService.GetAllDistrictAsync();

            var articleListUser = articleList.Where(a => a.AccountId == this.IdUser).Select(a => new
            {
                a.ArticleId,
                a.Title,
                city = cities.Where(e => e.CityId == a.CityAr).FirstOrDefault().CityName,
                district = districtes.Where(e => e.DistrictId == a.DistrictAr).FirstOrDefault().DistrictName,
                a.S,
                a.Price,
                a.CreateAt
            });

            ViewData["articleList"] = articleListUser.OrderByDescending(a => a.CreateAt).ToList();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if (!IsLogin)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewData["City"] = (await _cityService.GetAllCityAsync()).ToList();
            ViewData["District"] = (await _districtService.GetAllDistrictAsync()).Select(d => new DistrictDto { DistrictId = d.DistrictId, DistrictName = d.DistrictName, CityId = d.CityId }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] ModelAddArticle art)
        {
            if (!IsLogin)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewData["City"] = (await _cityService.GetAllCityAsync()).ToList();
            ViewData["District"] = (await _districtService.GetAllDistrictAsync()).Select(d => new DistrictDto { DistrictId = d.DistrictId, DistrictName = d.DistrictName, CityId = d.CityId }).ToList();
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
                ImagesArticleDto imagesArticle = new ImagesArticleDto()
                {
                    ImageArticle = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSq8aVvwt226C1Kx4QqQzL5IibUHcRbb1XCtw&s",
                };

                List<ImagesArticleDto> imagesArticleDtos = new() { imagesArticle };
                ArticleDto article = new ArticleDto()
                {
                    AccountId = this.IdUser,
                    Title = art.Title,
                    DescAr = art.Desc,
                    Addr = art.Address,
                    CityAr = (int)art.City,
                    DistrictAr = (int)art.District,
                    S = (decimal)art.S,
                    Price = (decimal)art.Price,
                    TienCoc = (decimal)art.TienCoc,
                    TypeAr = art.Type,
                    ImagesArticles = imagesArticleDtos
                };

                if (art.Type == "ChungCu")
                {
                    ChungCuDto chungCu = new ChungCuDto()
                    {
                        Floor = (int)art.Floor,
                        BedRoom = (int)art.BedRoom,
                        BathRoom = (int)art.BathRoom,
                        MaxPerson = (int)art.MaxPerson,
                        WaterPrice = (int)art.WaterPrice,
                        ElectricPrice = (int)art.ElectricPrice,
                    };
                    article.ChungCu = chungCu;


                }
                else if (art.Type == "Office")
                {
                    OfficeDto office = new OfficeDto()
                    {
                        Floor = (int)art.Floor,
                        DoorDrt = (int)art.DoorDrt,
                    };
                    article.Office = office;
                }
                else if (art.Type == "Tro")
                {
                    TroDto tro = new TroDto()
                    {
                        Floor = (int)art.Floor,
                        MaxPerson = (int)art.MaxPerson,
                        WaterPrice= (int)art.WaterPrice,
                        ElectricPrice= (int)art.ElectricPrice,
                    };
                    article.Tro = tro;
                }
                else if (art.Type == "House")
                {
                    HouseDto house = new HouseDto()
                    {
                        Floors = (int)art.Floors,
                        BathRoom = (int)art.BathRoom,
                        BedRoom = (int)art.BedRoom,
                    };
                    article.House = house;
                }

                _articleService.AddArticleAsync(article);

                return RedirectToAction("Index", "Home");
            }
            return View(art);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsLogin)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewData["id"] = id;
            ViewData["City"] = (await _cityService.GetAllCityAsync()).ToList();
            ViewData["District"] = (await _districtService.GetAllDistrictAsync()).Select(d => new DistrictDto { DistrictId = d.DistrictId, DistrictName = d.DistrictName, CityId = d.CityId }).ToList();

            ModelAddArticle modelArticle = new ModelAddArticle();

            var art = (await _articleService.GetArticleByIdAsync(id));

            if (art == null || art.AccountId != this.IdUser)
            {
                return RedirectToAction("Index");
            }

            if(art.TypeAr == "Tro")
            {
                var tro = art.Tro;
                modelArticle.Floor = tro.Floor;
                modelArticle.MaxPerson = tro.MaxPerson;
                modelArticle.WaterPrice = (double)tro.WaterPrice; 
                modelArticle.ElectricPrice = (double)tro.ElectricPrice;
            }
            else if(art.TypeAr == "ChungCu")
            {
                var chungCu = art.ChungCu;
                modelArticle.BedRoom = chungCu.BedRoom; 
                modelArticle.BathRoom = chungCu.BathRoom;
                modelArticle.Floor = chungCu.Floor;
                modelArticle.MaxPerson = chungCu.MaxPerson;
                modelArticle.WaterPrice = (double)chungCu.WaterPrice;
                modelArticle.ElectricPrice = (double)chungCu.ElectricPrice;
            }
            else if (art.TypeAr == "House")
            {
                
                var house = art.House;
                modelArticle.BedRoom = house.BedRoom;
                modelArticle.BathRoom = house.BathRoom;
                modelArticle.Floors = house.Floors;
            }
            else if(art.TypeAr == "Office")
            {
                var office = art.Office;
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
        public async Task<IActionResult> Edit([FromForm]int id, [FromForm] ModelAddArticle art)
        {
            ViewData["City"] = (await _cityService.GetAllCityAsync()).ToList();
            ViewData["District"] = (await _districtService.GetAllDistrictAsync()).Select(d => new DistrictDto { DistrictId = d.DistrictId, DistrictName = d.DistrictName, CityId = d.CityId }).ToList();

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

            var article = await _articleService.GetArticleByIdAsync(id);

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
                    var tro = article.Tro;
                    if (tro != null)
                    {
                        tro.Floor = (int)art.Floor;
                        tro.MaxPerson = (int)art.MaxPerson;
                        tro.WaterPrice = (decimal)art.WaterPrice;
                        tro.ElectricPrice = (decimal)art.ElectricPrice;
                    }
                    break;
                case "ChungCu":
                    var cc = article.ChungCu;
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
                    var house = article.House;
                    if (house != null)
                    {
                        house.Floors = (int)art.Floors;
                        house.BedRoom = (int)art.BedRoom;
                        house.BathRoom = (int)art.BathRoom;
                    }
                    break;
                case "Office":
                    var office = article.Office;
                    if (office != null)
                    {
                        office.Floor = art.Floor ?? 0;
                        office.DoorDrt = (int)art.DoorDrt;
                    }
                    break;
            }

            _articleService.UpdateArticle(article);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLogin)
            {
                return RedirectToAction("Login", "Account");
            }

            var art = await _articleService.GetArticleByIdAsync(id);

            if (art == null)
            {
                return RedirectToAction("Index");
            }

            if (art != null)
            {
                _articleService.DeleteArticleAsync(id);
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
