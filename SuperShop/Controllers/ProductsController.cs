namespace SuperShop.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SuperShop.Data;
    using SuperShop.Data.Entities;
    using System.Threading.Tasks;

    public class ProductsController : Controller
    {
        private readonly IRepository _repository;

        public ProductsController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(_repository.GetProducts());
        }

        // GET: Products/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _repository.GetProduct(id.Value);                             // id.Value gives the value of itself and assures that the program doesn't crash if id is null

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
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _repository.AddProduct(product);                                               // Saves the product in memory
                await _repository.SaveAllAsync();                                             // Asynchronous saving
                return RedirectToAction(nameof(Index));
            }
            return View(product);                                                          // The product fields remains filled 
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int? id)                                                // int? means an id might or might not exist
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _repository.GetProduct(id.Value);                             // Checking the table if the product exists
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)                       // Called when we do the post from Edit
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.UpdateProduct(product);
                    await _repository.SaveAllAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_repository.ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public IActionResult Delete(int? id)                                                  // Only shows what's to delete    
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _repository.GetProduct(id.Value);
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
            var product = _repository.GetProduct(id);                                     // Check the table if the product is still there
            _repository.RemoveProduct(product);
            await _repository.SaveAllAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
