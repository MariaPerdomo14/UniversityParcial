using System;
using System.Linq;
using System.Web.Mvc;
using University.BL.Data;
using University.BL.DTOs;
using University.BL.Models;
using PagedList;
using System.Web.UI;

namespace University.Web.Controllers
{

    public class CoursesController : Controller
    {
        // GET: Course
        private readonly UniversityContext context = new UniversityContext();
        

        [HttpGet]
        public ActionResult Index(int? courseid, int? pageSize, int? page)
        {
            //  SELECT * FROM   Course
            var query = context.Courses.ToList();
            var courses = query.Select(x => new CourseDTO
            {
                CourseID = x.CourseID,
                Title = x.Title,
                Credits = x.Credits
            }).ToList();

            if (courseid != null)
            {
                var instructor = (from q in context.CourseInstructors
                                  join r in context.Courses on q.CourseID equals r.CourseID
                                  join s in context.Instructors on q.InstructorID equals s.ID
                                  where q.CourseID == courseid
                                  select new InstructorDTO
                                  {
                                      LastName = s.LastName,
                                      FirstMidName = s.FirstMidName,

                                  }).ToList();

                ViewBag.Instructores = instructor;
            }
            #region Paginacion
            //Si viene nulo dele 10 por defecto
            pageSize = (pageSize ?? 10);
            //si viene igual por defecto llevelo a la 1 
            page = (page ?? 1);
            ViewBag.pageSize = pageSize;
            #endregion
            return View(courses.ToPagedList(page.Value, pageSize.Value));
        }


        // Course/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(CourseDTO course)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(course);



                //INSERT INTO Course(Title,Credits,EnrollmentDate) VALUES(@Title, @Credits, @EnrollmentDate)
                context.Courses.Add(new Course
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Credits = course.Credits,
                });
                context.SaveChanges();



                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }



            return View(course);

        }

        [HttpGet]
        public ActionResult Edit(int courseid)
        {


            var course = context.Courses.Where(x => x.CourseID == courseid)
                    .Select(x => new CourseDTO
                    {
                        CourseID = x.CourseID,
                        Title = x.Title,
                        Credits = x.Credits

                    }).FirstOrDefault();

            return View(course);
            //CON ESTE METODO SE ATRAPA DESDE EL NAVEGADOR EL ID DEL CURSO A EDITAR
        }

        [HttpPost]
        public ActionResult Edit(CourseDTO course)
        {

            try
            {

                if (!ModelState.IsValid)
                    return View(course);
                var courseModel = context.Courses.FirstOrDefault(x => x.CourseID == course.CourseID);


                //camps a modificar
                courseModel.Title = course.Title; 
                courseModel.Credits = course.Credits;

               
                context.SaveChanges();

                return RedirectToAction("Index"); 
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);

            }

            return View(course);
        }

        [HttpGet]
        public ActionResult Delete(int courseid)
        {



            if (!context.CourseInstructors.Any(x => x.CourseID == courseid)) 
            {
                var courseModel = context.Courses.FirstOrDefault(x => x.CourseID == courseid); 
                context.Courses.Remove(courseModel); 
                context.SaveChanges(); 

                 
            }

            return RedirectToAction("Index");

        }
    }


}
