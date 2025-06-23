using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using YourHouse.Application.DTOs;
using YourHouse.Application.Interfaces;
using YourHouse.Application.Services;
using YourHouse.Web.Infrastructure;
using YourHouse.Web.Infrastructure.Data;
using YourHouse.Web.Models;


namespace YourHouse.Web.Controllers
{
    public class MyArticleController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly ITroService _troService;
        private readonly IChungCuService _chungCuService;
        private readonly IHouseService _houseService;
        private readonly IOfficeService _officeService;
        private readonly IImageArticleService _imageArticleService;

        private readonly ICityService _cityService;
        private readonly IDistrictService _districtService;

        public MyArticleController(IArticleService articleService,
            IDistrictService districtService,
            ICityService cityService,
            ITroService troService,
            IChungCuService chungCuService,
            IHouseService houseService,
            IOfficeService officeService,
            IImageArticleService imageArticleService
            )
        {
            _articleService = articleService;
            _cityService = cityService;
            _districtService = districtService;
            _troService = troService;
            _chungCuService = chungCuService;
            _houseService = houseService;
            _officeService = officeService;
            _imageArticleService = imageArticleService;
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
                    AccountId = (int)this.IdUser,
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

                await _articleService.AddArticleAsync(article);

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

            var art = await _articleService.GetArticleByIdAsync(id);

            if (art == null || art.AccountId != this.IdUser)
            {
                return RedirectToAction("Index");
            }

            if(art.TypeAr == "Tro")
            {
                var tro = await _troService.GetTroByIdAsync(id);
                if(tro == null)
                {
                    return RedirectToAction("Index");
                }
                modelArticle.Floor = tro.Floor;
                modelArticle.MaxPerson = tro.MaxPerson;
                modelArticle.WaterPrice = (double)tro.WaterPrice; 
                modelArticle.ElectricPrice = (double)tro.ElectricPrice;
            }
            else if(art.TypeAr == "ChungCu")
            {
                var chungCu = await _chungCuService.GetChungCuByIdAsync(id);
                if(chungCu == null)
                {
                    return RedirectToAction("Index");
                }
                modelArticle.BedRoom = chungCu.BedRoom; 
                modelArticle.BathRoom = chungCu.BathRoom;
                modelArticle.Floor = chungCu.Floor;
                modelArticle.MaxPerson = chungCu.MaxPerson;
                modelArticle.WaterPrice = (double)chungCu.WaterPrice;
                modelArticle.ElectricPrice = (double)chungCu.ElectricPrice;
            }
            else if (art.TypeAr == "House")
            {
                var house = await _houseService.GetHouseByIdAsync(id);
                if(house == null)
                {
                    return RedirectToAction("Index");
                }
                modelArticle.BedRoom = house.BedRoom;
                modelArticle.BathRoom = house.BathRoom;
                modelArticle.Floors = house.Floors;
            }
            else if(art.TypeAr == "Office")
            {
                var office = await _officeService.GetOfficeByIdAsync(id);
                if(office == null)
                {
                    return RedirectToAction("Index");
                }
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
                    var tro = await _troService.GetTroByIdAsync(id);
                    if (tro != null)
                    {
                        tro.Floor = (int)art.Floor;
                        tro.MaxPerson = (int)art.MaxPerson;
                        tro.WaterPrice = (decimal)art.WaterPrice;
                        tro.ElectricPrice = (decimal)art.ElectricPrice;

                        await _troService.UpdateTro(tro);
                    }
                    break;
                case "ChungCu":
                    var cc = await _chungCuService.GetChungCuByIdAsync(id);
                    if (cc != null)
                    {
                        cc.Floor = (int)art.Floor;
                        cc.MaxPerson = (int)art.MaxPerson;
                        cc.WaterPrice = (decimal)art.WaterPrice;
                        cc.ElectricPrice = (decimal)art.ElectricPrice;
                        cc.BedRoom = art.BedRoom ?? 0;
                        cc.BathRoom = art.BathRoom ?? 0;

                        await _chungCuService.UpdateChungCu(cc);
                    }
                    break;
                case "House":
                    var house = await _houseService.GetHouseByIdAsync(id);
                    if (house != null)
                    {
                        house.Floors = (int)art.Floors;
                        house.BedRoom = (int)art.BedRoom;
                        house.BathRoom = (int)art.BathRoom;

                        await _houseService.UpdateHouse(house);
                    }
                    break;
                case "Office":
                    var office = await _officeService.GetOfficeByIdAsync(id);
                    if (office != null)
                    {
                        office.Floor = art.Floor ?? 0;
                        office.DoorDrt = (int)art.DoorDrt;

                        await _officeService.UpdateOffice(office);
                    }
                    break;
            }

            await _articleService.UpdateArticle(article);

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
                switch (art.TypeAr)
                {
                    case "Tro":
                        await _troService.DeleteTroAsync(id);
                        break;
                    case "ChungCu":
                        await _chungCuService.DeleteChungCuAsync(id);
                        break;
                    case "House":
                        await _houseService.DeleteHouseAsync(id);
                        break;
                    case "Office":
                        await _officeService.DeleteOfficeAsync(id);
                        break;
                }

                var images = await _imageArticleService.GetAllImageArticleAsync();
                foreach(var image in images)
                {
                    if(image.ArticleId == id)
                    {
                        await _imageArticleService.DeleteImageArticleAsync(image.ImageId);
                    }
                }

                await _articleService.DeleteArticleAsync(id);
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
