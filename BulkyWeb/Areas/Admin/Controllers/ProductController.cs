using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
	private readonly IUnitOfWork unitOfWork;

	public ProductController(IUnitOfWork unitOfWork)
	{
		this.unitOfWork = unitOfWork;
	}

	public IActionResult Index()
	{
		var productList = unitOfWork.product.GetAll().ToList();
		return View(productList);
	}

	[HttpGet]
	// Update and Insert
	public IActionResult Upsert(int? id)
	{
		ProductVM productVm = new ProductVM
		{
			categoryList = unitOfWork.category
				.GetAll()
				.Select(e => new SelectListItem()
				{
					Text = e.Name,
					Value = e.Id.ToString()
				}) ,
			product = new Product()
		};

		if (id is null || id == 0)
		{
			// Create
			return View(productVm);
		}
		else
		{
			// Update
			productVm.product = unitOfWork.product.Get(p => p.Id == id);
			return View(productVm);
		}

	}

	[HttpPost]
	public IActionResult Upsert(Product product , IFormFile? file)
	{
		if (ModelState.IsValid)
		{
			unitOfWork.product.Add(product);
			unitOfWork.Save();
			TempData["success"] = "Product Created Successfully";
			return RedirectToAction("Index");
		}

		ProductVM productVm = new ProductVM
		{
			categoryList = unitOfWork.category
				.GetAll()
				.Select(e => new SelectListItem()
				{
					Text = e.Name,
					Value = e.Id.ToString()
				}),
			product = product
		};
		return View(productVm);
	}

	[HttpGet]
	public IActionResult Delete(int? id)
	{
		if (id == null || id == 0)
			return NotFound();

		var product = unitOfWork.product.Get(p => p.Id == id);

		if (product == null)
			return NotFound();

		return View(product);
	}

	[HttpPost]
	[ActionName("Delete")]
	public IActionResult DeletePOST(int? id)
	{
		var product = unitOfWork.product.Get(p => p.Id == id);

		if (product == null)
			return NotFound();

		unitOfWork.product.Remove(product);
		unitOfWork.Save();

		TempData["success"] = "Product Deleted Successfully";
		return RedirectToAction("Index");
	}
}