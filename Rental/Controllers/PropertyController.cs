using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Xml;
using Rental.Models;
using Rental.Services;

namespace Rental.Controllers
{
    public class PropertyController : Controller
    {
        private readonly AppContextdb _appContext;
        private readonly PropertyService _propertyService;
        public PropertyController(AppContextdb appContext, PropertyService service)
        {
            _propertyService = service;
            _appContext = appContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int userID)
        {
            var currentUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = await _appContext.User.FirstOrDefaultAsync(u => u.UserID == Convert.ToInt32(currentUser));
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var properties = await _appContext.RealEstate.ToListAsync();

            if (user.UserRole == "user")
            {
                return RedirectToAction("UserPage", "Property");
            }
            else return View(properties);

            //return View(properties);
        }

        [HttpPost]
        public async Task<IActionResult> AddEstate(IFormCollection form)
        {
            var model = new RealEstate()
            {
                Type = form["Type"],
                Address = form["Address"],
                Square = Convert.ToDouble(form["Square"]),
                Price = Convert.ToDecimal(form["Price"]),
                Description = form["Description"],
                OwnerID = Convert.ToInt32(form["OwnerID"]),
                Pledge = Convert.ToDecimal(form["Pledge"]),
            };
            await _propertyService.AddPropertyAsync(model);

            return RedirectToAction("Index", new { tenantId = Convert.ToInt32(form["TenantID"]) });
        }

        [HttpGet]
        public IActionResult FavoriteProperties()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Authorization");
            }

            int userId = int.Parse(userIdClaim.Value);
            ViewBag.UserID = userId;

            var favoriteProperties = _appContext.Favorite
                .Where(f => f.UserID == userId)
                .Join(_appContext.RealEstate, f => f.Code, r => r.Code, (f, r) => r)
                .ToList();

            return View(favoriteProperties);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToFavorites(int code)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Json(new { success = false, message = "Пользователь не аутентифицирован" });
                }

                int userId = int.Parse(userIdClaim.Value);

                var favorite = new Favorite()
                {
                    Code = code,
                    UserID = userId
                };

                await _propertyService.AddToFavoritesAsync(favorite);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(new { success = false, message = "Ошибка при добавлении в избранное" });
            }
        }
        [HttpPost("Property/DeleteProperty/{code:int}")]
        public async Task<IActionResult> DeleteProperty(int code)
        {
            var property = await _appContext.RealEstate.FindAsync(code);
            if (property == null)
            {
                return Json(new { success = false, message = "Property not found." });
            }
            _appContext.RealEstate.Remove(property);
            await _appContext.SaveChangesAsync();
            return Json(new { success = true });

        }
        [Authorize]
        public async Task<IActionResult> ClearFavorites()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Authorization");
            }

            int userId = int.Parse(userIdClaim.Value);

            await _propertyService.ClearFavoritesAsync(userId);

            return RedirectToAction("FavoriteProperties");
        }



        [HttpGet("Property/Edit/{code:int}")]
        public async Task<IActionResult> Edit(int code)
        {
            var property = await _appContext.RealEstate.FindAsync(code);
            if (property == null) return NotFound();
            return PartialView("Edit", property); 
        }

        [HttpPost("Property/Edit/{code:int}")]
        public async Task<IActionResult> Edit(RealEstate property)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var editedProperty = await _appContext.RealEstate.FindAsync(property.Code);

                    if (editedProperty != null)
                    {
                        editedProperty.Type = property.Type;
                        editedProperty.Address = property.Address;
                        editedProperty.Description = property.Description;
                        editedProperty.Price = property.Price;

                        await _propertyService.EditAsync(editedProperty);
                        return RedirectToAction("Index", "Property");
                    }
                    else return NotFound();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyExists(property.Code))
                    {
                        return Json(new { success = false, message = "Недвижимость не найдена." });
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"Ошибка: {ex.Message}" });
                }
            }
            return Json(new { success = false, message = "Некорректные данные." });
        }

        public async Task<IActionResult> UserPage()
        {
            var properties = await _appContext.RealEstate.ToListAsync();

            return View(properties);
        }
        private bool PropertyExists(int code)
        {
            return _appContext.RealEstate.Any(e => e.Code == code);
        }
    }
}
