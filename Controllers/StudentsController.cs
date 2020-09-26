using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LastProject.Models;
using System.Globalization;

namespace LastProject.Controllers
{
    public class StudentsController : Controller
    {
        // GET: Students
        public ActionResult Index()
        {
            //ViewBag.UniversityList = new SelectList(GetUniversitiesList(),"UniversityID", "UniversityName");
            return View();
        }

        public ActionResult GetData()
        {
            using (DBModel db = new DBModel())
            {
                db.Configuration.LazyLoadingEnabled = false;
                List<Student> students = db.Student.ToList();
                List<Supervisor> supervisors = db.Supervisor.ToList();
                List<University> universities = db.University.ToList();
                var studentList = from s in students
                                  join sv in supervisors on s.SupervisorID equals sv.SupervisorID 
                                  join u in universities on s.UniversityID equals u.UniversityID 
                                  
                                  select new ViewModel
                                  {
                                      StudentID = s.StudentID,
                                      StudentName = s.StudentName,
                                      Department = s.Department,
                                      Major = s.Major,
                                      StartDate = s.StartDate,
                                      SupervisorName = sv.SupervisorName,
                                      UniversityName = u.UniversityName


                                  };
                return Json(new { data = studentList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                using (DBModel db = new DBModel())
                {
                    ViewBag.UniversityList = new SelectList(GetUniversitiesList(), "UniversityID", "UniversityName");
                    return View(new Student());
                }
            }
                
            else
            {
                using (DBModel db = new DBModel())
                {
                    ViewBag.UniversityList = new SelectList(GetUniversitiesList(), "UniversityID", "UniversityName");
                    return View(db.Student.Where(x => x.StudentID == id).FirstOrDefault<Student>());
                }
            }
        }
        [HttpPost]
        public ActionResult AddOrEdit(Student student)
        {
            using (DBModel db = new DBModel())
            {

                if (student.StudentID == 0)
                {
                    ViewBag.UniversityList = new SelectList(GetUniversitiesList(), "UniversityID", "UniversityName");

                    db.Student.Add(student);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.UniversityList = new SelectList(GetUniversitiesList(), "UniversityID", "UniversityName");
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }

        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (DBModel db = new DBModel())
            {
                Student student = db.Student.Where(x => x.StudentID == id).FirstOrDefault<Student>();
                db.Student.Remove(student);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }

        public List<University> GetUniversitiesList()
        {
            DBModel db = new DBModel();
            List<University> universities = db.University.ToList();
            return universities;
        }
        public JsonResult GetSupervisorList(int UniversityID)
        {
            DBModel db = new DBModel();
            db.Configuration.ProxyCreationEnabled = false;
            List<Supervisor> SupervisorList = db.Supervisor.Where(x => x.UniversityID == UniversityID).ToList();
            return Json(SupervisorList, JsonRequestBehavior.AllowGet);

        }

    }

    internal class ViewModel
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string Department { get; set; }
        public string Major { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string SupervisorName { get; set; }
        public string UniversityName { get; set; }

    }
}