using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
//[Authorize(Roles = SD.Role_Admin)]
public class CompanyController : Controller
{
	private readonly IUnitOfWork unitOfWork;
	public CompanyController(IUnitOfWork unitOfWork)
	{
		this.unitOfWork = unitOfWork;
	}

	public IActionResult Index()
	{
		var CompanyList = unitOfWork.company.GetAll()
			.ToList();
		return View(CompanyList);
	}
	
	[HttpGet]
	// Update and Insert
	public IActionResult Upsert(int? id)
	{
		if (id is null || id == 0)
		{
			// Create
			return View(new Company());
		}
		else
		{
			// Update
			Company company = unitOfWork.company.Get(p => p.Id == id);
			return View(company);
		}

	}

	[HttpPost]
	public IActionResult Upsert(Company company)
	{
		if (ModelState.IsValid)
		{
			if (company.Id == 0)
				unitOfWork.company.Add(company);
			else
				unitOfWork.company.Update(company);

			unitOfWork.Save();
			TempData["success"] = "company Created Successfully";
			return RedirectToAction("Index");
		}
		else
		{
			return View(company);
		}
	}

	#region APICALLS

	[HttpGet]
    public IActionResult GetAll()
    {
        var companyList = unitOfWork.company
			.GetAll()
			.ToList();

        return Json(new { data = companyList });
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var companyToBeDeleted = unitOfWork.company.Get(p => p.Id == id);
        if (companyToBeDeleted is null)
        {
            return Json(new
            {
                success = false,
                message = "Error while deleting"
            });
        }
        else
        {
			unitOfWork.company.Remove(companyToBeDeleted);
			unitOfWork.Save();

            return Json(new
            {
                success = true,
                message = "Deleted Successfully"
            });
        }

    }
    #endregion
}