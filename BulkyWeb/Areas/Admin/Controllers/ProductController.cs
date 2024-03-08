﻿using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
	private readonly IUnitOfWork unitOfWork;
	private readonly IWebHostEnvironment webHostEnvironment;
	public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
	{
		this.unitOfWork = unitOfWork;
		this.webHostEnvironment = webHostEnvironment;
	}

	public IActionResult Index()
	{
		var productList = unitOfWork.product.GetAll("Category").ToList();
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
			// C:\Users\aaaa\source\repos\Bulky\BulkyWeb\wwwroot
			string wwwRoot = webHostEnvironment.WebRootPath;

			if (file is not null)
			{
				// C:\Users\aaaa\source\repos\Bulky\BulkyWeb\wwwroot\Images\Product
				string productPath = Path.Combine(wwwRoot, @"Images\Product");

				//RandomName.JPG
				string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

				// delete the old image if exsist
				if (!string.IsNullOrEmpty(product.ImageUrl))
				{
					var oldImagePath = Path.Combine(wwwRoot, product.ImageUrl.TrimStart('\\'));

					if (System.IO.File.Exists(oldImagePath))
					{
						System.IO.File.Delete(oldImagePath);
					}
				}

				// File Path : C:\Users\aaaa\source\repos\Bulky\BulkyWeb\wwwroot\Images\Product\RandomName.JPG
				using (var fileStream = new FileStream(Path.Combine(productPath , fileName) , FileMode.Create))
				{
					file.CopyTo(fileStream);
				}
				
				product.ImageUrl = @"\Images\Product\" + fileName;
			}

			if (product.Id == 0)
				unitOfWork.product.Add(product);
			else
				unitOfWork.product.Update(product);

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