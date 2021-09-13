using System;
using System.Linq;
using System.Web.Mvc;
using University.BL.Data;
using University.BL.DTOs;
using PagedList;


namespace University.Web.Controllers
{
    public class OfficeAssignmentsController : Controller
    {
        private readonly UniversityContext context = new UniversityContext();

        // GET: OfficeAssignments
        public ActionResult Index(int? pageSize, int? page)
        {
            //  SELECT * FROM OfficeAssignment
            var query = context.OfficeAssignments.Include("Instructor").ToList();
            var offices = query.Select(x => new OfficeAssignmentDTO
            {
                InstructorID = x.InstructorID,
                Location = x.Location,
                Instructor = new InstructorDTO
                {
                    FirstMidName = x.Instructor.FirstMidName,
                    LastName = x.Instructor.LastName
                }
            });


            pageSize = (pageSize ?? 10);
            page = (page ?? 1);
            ViewBag.pageSize = pageSize;


            return View(offices.ToPagedList(page.Value, pageSize.Value));
        }

        [HttpGet]
        public ActionResult Create()
        {
            LoadData();

            return View();
        }

        [HttpPost]
        public ActionResult Create(OfficeAssignmentDTO office)
        {
            LoadData();

            if (!ModelState.IsValid)
                return View(ModelState);

            // INSERT INTO OfficeAssignments
            context.OfficeAssignments.Add(new BL.Models.officeAssignment
            {
                InstructorID = office.InstructorID,
                Location = office.Location
            });
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {


            var office = context.OfficeAssignments.Where(x => x.InstructorID == id)
                            .Select(x => new OfficeAssignmentDTO
                            {
                                InstructorID = x.InstructorID,
                                Location = x.Location
                            }).FirstOrDefault();


            return View(office);
        }

        [HttpPost]
        public ActionResult Edit(OfficeAssignmentDTO office)
        {

            try
            {
                if (!ModelState.IsValid)
                    return View(office);

                var officeModel = context.OfficeAssignments.FirstOrDefault(x => x.InstructorID == office.InstructorID);


                officeModel.Location = office.Location;

                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

            }
            return View(office);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var officeModel = context.OfficeAssignments.FirstOrDefault(x => x.InstructorID == id);
            context.OfficeAssignments.Remove(officeModel);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        private void LoadData()
        {
            var instructors = context.Instructors.Select(x => new InstructorDTO
            {
                ID = x.ID,
                FirstMidName = x.FirstMidName,
                LastName = x.LastName
            }).ToList();
            ViewData["Instructors"] = new SelectList(instructors, "ID", "FullName");


        }
    }
}