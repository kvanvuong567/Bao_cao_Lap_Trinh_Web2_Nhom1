using API_MPICTURE.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_MPICTURE.Models.DTO;

namespace MVC_MPICTURE.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CategoriesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: Categories
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            List<categoryDTO> categories = new List<categoryDTO>();
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("http://localhost:5102/api/Categories/get-all-categories");
                response.EnsureSuccessStatusCode();
                categories.AddRange(await response.Content.ReadFromJsonAsync<List<categoryDTO>>());
            }
            catch (Exception ex)
            {
                // Log error
                return View("Error");
            }
            return View(categories);
        }

        // GET: Categories/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(categoryDTO category)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsJsonAsync("http://localhost:5102/api/Categories/add-category", category);
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log error
                return View("Error");
            }
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"http://localhost:5102/api/Categories/get-category-by-id/{id}");
                response.EnsureSuccessStatusCode();
                var category = await response.Content.ReadFromJsonAsync<categoryDTO>();
                return View(category);
            }
            catch (Exception ex)
            {
                // Log error
                return View("Error");
            }
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, categoryDTO category)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.PutAsJsonAsync($"http://localhost:5102/api/Categories/update-category-by-id/{id}", category);
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log error
                return View("Error");
            }
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.DeleteAsync($"http://localhost:5102/api/Categories/delete-category-by-id/{id}");
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log error
                return View("Error");
            }
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"http://localhost:5102/api/Categories/get-category-by-id/{id}");
                response.EnsureSuccessStatusCode();
                var category = await response.Content.ReadFromJsonAsync<categoryDTO>();
                return View(category);
            }
            catch (Exception ex)
            {
                // Log error
                return View("Error");
            }
        }
    }
}
