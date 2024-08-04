namespace SuperShop.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SuperShop.Data;
    using SuperShop.Data.Entities;
    using SuperShop.Helpers;
    using SuperShop.Models;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public ProductsController(IProductRepository productRepository, IUserHelper userHelper, IImageHelper imageHelper, IConverterHelper converterHelper)
        {
            _productRepository = productRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(_productRepository.GetAll().OrderBy(p => p.Name));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);                       // id.Value gives the value of itself and assures that the program doesn't crash if id is null

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {              
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0) 
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "products");                  
                }

                var product = _converterHelper.ToProduct(model, path, true);

                //TODO: Change to the user that is logged in
                product.User = await _userHelper.GetUserByEmailAsync("rafaasfs@gmail.com");
                await _productRepository.CreateAsync(product);                                                                                                         
                return RedirectToAction(nameof(Index));
            }
            return View(model);                                                                     // The fields remains filled 
        }       

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)                                                // int? means an id might or might not exist
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);                             // Checking the table if the product exists

            if (product == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToProductViewModel(product);

            return View(model);
        }       

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)                                  // Called when we do the post from Edit
        {
            if (ModelState.IsValid)
            {
                try
                { 
                    var path = model.ImageUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0) 
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "products");
                    }

                    var product = _converterHelper.ToProduct(model, path, false);
                
                    //TODO: change to the user that is logged in
                    product.User = await _userHelper.GetUserByEmailAsync("rafaasfs@gmail.com");
                    await _productRepository.UpdateAsync(product);                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _productRepository.ExistAsync(model.Id))
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
            return View(model);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)                                              // Only shows what's to delete    
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)                            // Post is required to make sure that the action performed is to actually delete and not just
                                                                                            // showing what's to delete
        {
            var product = await _productRepository.GetByIdAsync(id);                        // Check the table if the product is still there
            await _productRepository.DeleteAsync(product);
           
            return RedirectToAction(nameof(Index));
        }
    }
}
