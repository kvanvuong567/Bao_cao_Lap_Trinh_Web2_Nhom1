using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_MPICTURE.Models.DTO;
using System.Net.Mime;
using System.Text.Json;
using System.Text;
using API_MPICTURE.Models.DTO;
using Microsoft.AspNetCore.Authorization;


namespace MVC_MPICTURE.Controllers
{
    [Authorize]
    public class ImagesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ImagesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        // GET: Images
        [AllowAnonymous]
        public async Task<IActionResult> Index(string filterOn = null, string filterQuery = null, string sortBy = null, bool isAscending = true)
        {
            List<ImageDTO> response = new List<ImageDTO>();
            try
            {
                var client = _httpClientFactory.CreateClient();
                var apiUrl = $"http://localhost:5102/api/Images/get-all-images?filterOn={filterOn}&filterQuery={filterQuery}&sortBy={sortBy}&isAscending={isAscending}";
                var httpResponseMessage = await client.GetAsync(apiUrl);
                httpResponseMessage.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<List<ImageDTO>>());
            }
            catch (Exception ex)
            {
                // Ghi lại log lỗi
                return View("Error");
            }
            return View(response);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // Lấy danh sách categories từ API
                var categoryResponse = await client.GetFromJsonAsync<List<categoryDTO>>("http://localhost:5102/api/Categories/get-all-categories");
                ViewBag.Categories = categoryResponse != null ? new SelectList(categoryResponse, "Id", "Name") : new SelectList(new List<categoryDTO>(), "Id", "Name");

                // Lấy danh sách tags từ API
                var tagResponse = await client.GetFromJsonAsync<List<tagDTO>>("http://localhost:5102/api/Tags/get-all-tags");
                ViewBag.Tags = tagResponse != null ? new SelectList(tagResponse, "Id", "Name") : new SelectList(new List<tagDTO>(), "Id", "Name");

                return View();
            }
            catch (Exception ex)
            {
                // Xử lý exception
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(addImageDTO addImageDTO)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var apiUrl = "http://localhost:5102/api/Images/add-image";

                // Gửi yêu cầu POST đến API
                var httpResponse = await client.PostAsJsonAsync(apiUrl, addImageDTO);
                httpResponse.EnsureSuccessStatusCode();

                // Đọc kết quả trả về từ API
                var response = await httpResponse.Content.ReadFromJsonAsync<addImageDTO>();

                // Xử lý kết quả
                if (response != null)
                {
                    // Chuyển hướng đến trang Index hoặc hiển thị thông báo thành công
                    return RedirectToAction("Index", "Images");
                }
                else
                {
                    // Xử lý trường hợp thất bại
                    ViewBag.Error = "Failed to create image. Server returned status code: " + httpResponse.StatusCode;
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Xử lý exception
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // Lấy thông tin hình ảnh để chỉnh sửa từ API
                var response = await client.GetAsync($"http://localhost:5102/api/Images/get-image-by-id/{id}");
                response.EnsureSuccessStatusCode();
                var image = await response.Content.ReadFromJsonAsync<EditImageDTO>();

                // Lấy danh sách categories từ API
                var categoryResponse = await client.GetFromJsonAsync<List<CategoryDTO>>("http://localhost:5102/api/Categories/get-all-categories");
                ViewBag.Categories = categoryResponse != null ? new SelectList(categoryResponse, "Id", "Name") : new SelectList(new List<CategoryDTO>(), "Id", "Name");

                // Lấy danh sách tags từ API
                var tagResponse = await client.GetFromJsonAsync<List<TagDTO>>("http://localhost:5102/api/Tags/get-all-tags");
                ViewBag.Tags = tagResponse != null ? new MultiSelectList(tagResponse, "Id", "Name", image.TagIds) : new MultiSelectList(new List<TagDTO>(), "Id", "Name");

                return View(image);
            }
            catch (Exception ex)
            {
                // Xử lý exception
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(EditImageDTO editImageDTO)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"http://localhost:5102/api/Images/update-image-by-id/{editImageDTO.Id}"),
                    Content = new StringContent(JsonSerializer.Serialize(editImageDTO), Encoding.UTF8, MediaTypeNames.Application.Json)
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Images");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var httpResponseMess = await client.DeleteAsync("http://localhost:5102/api/Images/delete-image-by-id/" + id);
                httpResponseMess.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Images");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Index");
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            ImageDTO imageDTO = null;
            try
            {
                var client = _httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync($"http://localhost:5102/api/Images/get-image-by-id/{id}");
                httpResponseMessage.EnsureSuccessStatusCode();
                imageDTO = await httpResponseMessage.Content.ReadFromJsonAsync<ImageDTO>();
            }
            catch (Exception ex)
            {
                // Ghi lại log lỗi
                return View("Error");
            }
            return View(imageDTO);
        }

        // Thêm phương thức tải ảnh
        public async Task<IActionResult> DownloadImage(string imageUrl)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(imageUrl);
            response.EnsureSuccessStatusCode();

            var fileBytes = await response.Content.ReadAsByteArrayAsync();
            var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";

            // Xác định phần mở rộng tệp dựa trên loại nội dung
            var fileExtension = contentType switch
            {
                "image/jpeg" => ".jpg",
                "image/png" => ".png",
                "image/gif" => ".gif",
                _ => ".bin"  
            };

            var fileName = Path.GetFileNameWithoutExtension(imageUrl) + fileExtension;

            return File(fileBytes, contentType, fileName);
        }
    }
}
