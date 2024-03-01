using Bulky.DataAccess.Data;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers;

public class CategoryController : Controller
{
	private readonly ApplicationDbContext _db;

	public CategoryController(ApplicationDbContext db)
	{
		_db = db;
	}

	public IActionResult Index()
	{
		var categoryList = _db.Categories.ToList();
		return View(categoryList);
	}

	[HttpGet]
	public IActionResult Create()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Create(Category category)
	{
		if (category.Name == category.DisplayOrder.ToString())
		{
			ModelState.AddModelError("Name", "The DisplayOrder Can't Match Exactly The Name.");
		}

		if (ModelState.IsValid)
		{
			_db.Categories.Add(category);
			_db.SaveChanges();
			TempData["success"] = "Category Created Successfully";
			return RedirectToAction("Index");
		}
		else
		{
			return View(category);
		}
	}
	[HttpGet]
	public IActionResult Edit(int? id)
	{
		if (id == null || id == 0)
			return NotFound();

		Category category = _db.Categories
			.SingleOrDefault(c => c.Id == id);

		if (category == null)
			return NotFound();

		return View(category);
	}

	[HttpPost]
	public IActionResult Edit(Category category)
	{
		if (ModelState.IsValid)
		{
			_db.Categories.Update(category);
			_db.SaveChanges();
			TempData["success"] = "Category Updated Successfully";
			return RedirectToAction("Index");
		}

		return View(category);
	}

	[HttpGet]
	public IActionResult Delete(int? id)
	{
		if (id == null || id == 0)
			return NotFound();

		Category category = _db.Categories
			.SingleOrDefault(c => c.Id == id);

		if (category == null)
			return NotFound();

		return View(category);
	}

	[HttpPost , ActionName("Delete")]
	public IActionResult DeletePOST(int? id)
	{
		Category category = _db.Categories
			.SingleOrDefault(c => c.Id == id);

		if (category == null)
			return NotFound();

		_db.Categories.Remove(category);
		_db.SaveChanges();

		TempData["success"] = "Category Deleted Successfully";
		return RedirectToAction("Index");
	}
}