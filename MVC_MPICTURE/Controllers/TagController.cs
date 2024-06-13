using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_MPICTURE.Models.DTO;

namespace MVC_MPICTURE.Controllers
{
    [Authorize]
    public class TagsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TagsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: Tags
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            List<tagDTO> tags = new List<tagDTO>();
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("http://localhost:5102/api/Tags/get-all-tags");
                response.EnsureSuccessStatusCode();
                tags.AddRange(await response.Content.ReadFromJsonAsync<List<tagDTO>>());
            }
            catch (Exception ex)
            {
                // Log error
                return View("Error");
            }
            return View(tags);
        }

        // GET: Tags/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tags/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(tagDTO tag)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsJsonAsync("http://localhost:5102/api/Tags/add-tag", tag);
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log error
                return View("Error");
            }
        }

        // GET: Tags/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"http://localhost:5102/api/Tags/get-tag-by-id/{id}");
                response.EnsureSuccessStatusCode();
                var tag = await response.Content.ReadFromJsonAsync<tagDTO>();
                return View(tag);
            }
            catch (Exception ex)
            {
                // Log error
                return View("Error");
            }
        }

        // POST: Tags/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, tagDTO tag)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.PutAsJsonAsync($"http://localhost:5102/api/Tags/update-tag-by-id/{id}", tag);
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log error
                return View("Error");
            }
        }

        // GET: Tags/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.DeleteAsync($"http://localhost:5102/api/Tags/delete-tag-by-id/{id}");
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log error
                return View("Error");
            }
        }

        // GET: Tags/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"http://localhost:5102/api/Tags/get-tag-by-id/{id}");
                response.EnsureSuccessStatusCode();
                var tag = await response.Content.ReadFromJsonAsync<tagDTO>();
                return View(tag);
            }
            catch (Exception ex)
            {
                // Log error
                return View("Error");
            }
        }
    }
}
