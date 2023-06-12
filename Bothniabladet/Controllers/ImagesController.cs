using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bothniabladet.Data;
using Bothniabladet.Models.ImageModels;
using System.IO;
using Bothniabladet.Services;
using Microsoft.AspNetCore.Authorization;

namespace Bothniabladet.Controllers
{
    [Authorize]
    public class ImagesController : Controller
    {
        private readonly AppDbContext _context;
        public ImageService _service;

        public ImagesController(AppDbContext context, ImageService service)
        {
            _context = context;
            _service = service;
        }

        // GET: Images
        public IActionResult Index(string searchString)
        {
            // List all images if there is no search string specified
            if (String.IsNullOrEmpty(searchString))
            {
                var allModels = _service.GetImages();
                return View(allModels);
            }
            // Call the service to retrieve the image view models based on the search string
            var searchedModels = _service.GetSearchedImages(searchString);
            return View(searchedModels);
        }

        // GET: Images/BoxView
        public IActionResult BoxView(string searchString)
        {
            // List all images if there is no search string specified
            if (String.IsNullOrEmpty(searchString))
            {
                var allModels = _service.GetImages();
                return View(allModels);
            }
            // Call the service to retrieve the image view models based on the search string
            var searchedModels = _service.GetSearchedImages(searchString);
            return View(searchedModels);
        }

        // GET: Images/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the image view model using the service
            var imageViewModel = _service.GetImageDetail(id);

            if (imageViewModel == null)
            {
                return NotFound();
            }

            // Prepare the edited images data strings for display
            ICollection<GetEditedImageModel> editedImagesDataStrings = new List<GetEditedImageModel>();
            foreach (EditedImage editedImage in imageViewModel.EditedImages)
            {
                editedImagesDataStrings.Add(new GetEditedImageModel()
                {
                    EditedImageId = editedImage.EditedImageId,
                    ImageTitle = editedImage.ImageTitle,
                    Thumbnail = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(editedImage.Thumbnail))
                });
            }
            imageViewModel.getEditedImageModels = editedImagesDataStrings;

            // Add image data to ViewBag for display in the view
            ViewBag.ImageDataUrl = imageViewModel.ImageDataString;

            return View(imageViewModel);
        }

        // GET: Images/DetailsEdit/5/2 (get edited image)
        public async Task<IActionResult> DetailsEdit(int? id, int? editId)
        {
            if (id == null || editId == null)
            {
                return NotFound();
            }

            // Retrieve the image view model using the service
            var imageViewModel = _service.GetImageModel(id, editId);

            if (imageViewModel == null)
            {
                return NotFound();
            }

            // Add edited image data to ViewBag for display in the view
            ViewBag.ImageDataUrl = imageViewModel.ImageData;

            return View(imageViewModel);
        }

        // GET: Images/Create
        public IActionResult Create()
        {
            // Retrieve the section choices from the service and pass them to the view
            return View(new CreateImageCommand() { Sections = _service.GetSectionChoices() });
        }

        // POST: Images/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateImageCommand cmd)
        {
            // Handle image data upload
            using (var memoryStream = new MemoryStream())
            {
                await cmd.ImageData.FormFile.CopyToAsync(memoryStream);

                // Upload the file if it's less than 12MB
                if (memoryStream.Length < 12097152)
                {
                    cmd.ImageMemoryStream = memoryStream;
                    var id = _service.CreateImage(cmd);

                    return RedirectToAction(""); // Make this redirect to the added image's Details page
                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large.");
                }
            }

            // If we got here, something went wrong
            return View(cmd);
        }

        // GET: Images/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // POST: Images/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImageId,ImageTitle,ImageData,BasePrice,Issue,SectionPublished,Section,CreatedAt")] Image image)
        {
            if (id != image.ImageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(image);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageExists(image.ImageId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(image);
        }

        // POST: Images/AddEdited
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdited(ImageDetailViewModel viewModel)
        {
            // Refactor: I'm really not fond of how I'm doing this here, but don't really know how to pass only the command from the view
            AddEditedCommand cmd = viewModel.CreateEditedImage;

            // Handle edited image data upload
            using (var memoryStream = new MemoryStream())
            {
                await cmd.ImageData.FormFile.CopyToAsync(memoryStream);

                // Upload the file if it's less than 12MB
                if (memoryStream.Length < 12097152)
                {
                    var id = _service.CreateEditedImage(cmd);
                    return RedirectToAction(""); // Make this redirect to the added EditedImage's Details page
                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large.");
                }
            }

            // If we got here, something went wrong
            return View(cmd);
        }

        // GET: Images/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images.FirstOrDefaultAsync(m => m.ImageId == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _service.SoftDeleteImage(id);
            return RedirectToAction(nameof(Index));
        }

        // Private method to check if an image exists
        private bool ImageExists(int id)
        {
            return _context.Images.Any(e => e.ImageId == id);
        }

        // GET: Images/Details/Download/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Download(int id)
        {
            Image image = _service.GetImage(id);
            MemoryStream imageStream = new MemoryStream(image.ImageData);
            if (imageStream == null)
            {
                return NotFound(); // Returns a NotFoundResult with Status404NotFound response
            }

            return new FileContentResult(image.ImageData, "image/jpeg")
            {
                FileDownloadName = image.ImageTitle + ".jpeg"
            }; // Returns a FileStreamResult
        }
    }
}
