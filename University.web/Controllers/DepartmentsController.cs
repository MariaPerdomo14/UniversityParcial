using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.BL.Data;
using University.BL.DTOs;
using University.BL.Models;
using PagedList;

namespace University.Web.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly UniversityContext context = new UniversityContext();

        // GET: Departments
        public ActionResult Index(int? pageSize, int? page)
        {
            var query = context.Departments.Include("Instructor").ToList();
            var departments = query.Select(x => new DepartmentDTO
            {
                DepartmentID = x.DepartmentID,
                Name = x.Name,
                Budget = x.Budget,
                StartDate = x.StartDate,
                InstructorID = x.InstructorID,
                Instructor = new InstructorDTO
                {
                    FirstMidName = x.Instructor.FirstMidName,
                    LastName = x.Instructor.LastName
                }

            });


            pageSize = (pageSize ?? 10);
            page = (page ?? 1);
            ViewBag.pageSize = pageSize;



            return View(departments.ToPagedList(page.Value, pageSize.Value));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(DepartmentDTO department)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(department);


                context.Departments.Add(new Department
                {
                    DepartmentID = department.DepartmentID,
                    Name = department.Name,
                    Budget = department.Budget,
                    StartDate = department.StartDate,
                    InstructorID = department.InstructorID,

                });

                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {


            var department = context.Departments.Where(x => x.DepartmentID == id)
                            .Select(x => new DepartmentDTO
                            {
                                DepartmentID = x.DepartmentID,
                                Name = x.Name,
                                Budget = x.Budget,
                                StartDate = x.StartDate,
                                InstructorID = x.InstructorID,

                            }).FirstOrDefault();


            return View(department);
        }

        [HttpPost]
        public ActionResult Edit(DepartmentDTO department)
        {

            try
            {
                if (!ModelState.IsValid)
                    return View(department);

                var departmentModel = context.Departments.FirstOrDefault(x => x.DepartmentID == department.DepartmentID);


                departmentModel.Name = department.Name;
                departmentModel.Budget = department.Budget;
                departmentModel.StartDate = department.StartDate;

                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

            }
            return View(department);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (!context.Departments.Any(x => x.InstructorID == id))
            {
                var departmentModel = context.Departments.FirstOrDefault(x => x.DepartmentID == id);
                context.Departments.Remove(departmentModel);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }




    }


}