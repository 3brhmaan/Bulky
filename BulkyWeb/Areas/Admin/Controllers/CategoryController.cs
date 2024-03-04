using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly IUnitOfWork unitOfWork;
    public CategoryController(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        var categoryList = unitOfWork.category.GetAll().ToList();
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
            unitOfWork.category.Add(category);
            unitOfWork.Save();
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

        Category category = unitOfWork.category.Get(c => c.Id == id);

        if (category == null)
            return NotFound();

        return View(category);
    }

    [HttpPost]
    public IActionResult Edit(Category category)
    {
        if (ModelState.IsValid)
        {
            unitOfWork.category.Update(category);
            unitOfWork.Save();
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

        Category category = unitOfWork.category.Get(c => c.Id == id);

        if (category == null)
            return NotFound();

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {
        Category category = unitOfWork.category.Get(c => c.Id == id);

        if (category == null)
            return NotFound();

        unitOfWork.category.Remove(category);
        unitOfWork.Save();

        TempData["success"] = "Category Deleted Successfully";
        return RedirectToAction("Index");
    }
}