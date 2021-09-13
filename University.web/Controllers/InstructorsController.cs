using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.BL.Data;
using University.BL.DTOs;
using University.BL.Models;

namespace University.Web.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly UniversityContext context = new UniversityContext();
        // GET: Instructors
        public ActionResult Index(int? InstructorId, int? pageSize, int? page)
        {
            var query = context.Instructors.ToList();
            var instructors = query.Select(x => new InstructorDTO
            {
                ID = x.ID,
                LastName = x.LastName,
                FirstMidName = x.FirstMidName,
                HireDate = x.HireDate,
            });

            if (InstructorId != null)
            {
                var department = (from q in context.Departments
                                  join r in context.Instructors on q.InstructorID equals r.ID
                                  where q.DepartmentID == InstructorId
                                  select new DepartmentDTO
                                  {
                                      DepartmentID = q.DepartmentID,
                                      Name = q.Name,

                                  }).ToList();

                ViewBag.Department = department;
            }


            pageSize = (pageSize ?? 10);
            page = (page ?? 1);
            ViewBag.pageSize = pageSize;


            return View(instructors.ToPagedList(page.Value, pageSize.Value));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(InstructorDTO instructor)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(instructor);


                context.Instructors.Add(new Instructor
                {
                    ID = instructor.ID,
                    LastName = instructor.LastName,
                    FirstMidName = instructor.FirstMidName,
                    HireDate = instructor.HireDate,
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


            var instructor = context.Instructors.Where(x => x.ID == id)
                            .Select(x => new InstructorDTO
                            {

                                LastName = x.LastName,
                                FirstMidName = x.FirstMidName,
                                HireDate = x.HireDate,


                            }).FirstOrDefault();


            return View(instructor);
        }

        [HttpPost]
        public ActionResult Edit(InstructorDTO instructor)
        {

            try
            {
                if (!ModelState.IsValid)
                    return View(instructor);

                var instructorModel = context.Instructors.FirstOrDefault(x => x.ID == instructor.ID);


                instructorModel.LastName = instructor.LastName;
                instructorModel.FirstMidName = instructor.FirstMidName;
                instructorModel.HireDate = instructor.HireDate;

                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

            }
            return View(instructor);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var instructorModel = context.Instructors.FirstOrDefault(x => x.ID == id);
            context.Instructors.Remove(instructorModel);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}