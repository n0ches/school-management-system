using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolSystemManagement.Data;
using SchoolSystemManagement.Models;

namespace SchoolSystemManagement.Controllers
{
    public class LecturersController : Controller
    {
        private readonly SchoolContext _context;

        public LecturersController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Lecturers
        public async Task<IActionResult> Index()
        {
            var lecturers= (from Lecturer in _context.Lecturers
                           join Department in _context.Departments on Lecturer.Department.ID equals Department.ID
                           join Faculty in _context.Faculties on Department.Faculty.ID equals Faculty.ID
                           join University in _context.Universities on Faculty.University.ID equals University.ID
                           select new
                           {
                               Department = Department,
                               University = University,
                               Faculty = Faculty,
                               Lecturer = Lecturer
                           }).ToList();
            List<Lecturer> lecturerList = new List<Lecturer>();

            lecturers.ForEach(l =>
            {
                Lecturer lecturer = l.Lecturer;
                lecturer.Department = l.Department;
                lecturer.Department.Faculty = l.Faculty;
                lecturer.Department.Faculty.University = l.University;
                lecturerList.Add(lecturer);
            });

            dynamic mymodel = new ExpandoObject();
            mymodel.Lecturers = lecturerList;
            return View(mymodel);
        }

        // GET: Lecturers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lessons = (from Lecturer in _context.Lecturers
                           join Lesson in _context.Lessons on Lecturer.ID equals Lesson.Lecturer.ID
                           join Department in _context.Departments on Lesson.Department.ID equals Department.ID
                           where Lecturer.ID == id
                           select new
                           {
                               Department = Department,
                               Lesson = Lesson,
                               Lecturer = Lecturer
                           }).ToList();

            List<Lesson> lessonList = new List<Lesson>();

            lessons.ForEach(lesson =>
            {
                Lecturer lecturer = lesson.Lecturer;
                Lesson lesson1 = lesson.Lesson;
                lesson1.Department = lesson.Department;
                lesson1.Lecturer = lecturer;

                lessonList.Add(lesson1);

            });

            var lecturer = (from Lecturer in _context.Lecturers
                            join Department in _context.Departments on Lecturer.Department.ID equals Department.ID
                            join Faculty in _context.Faculties on Department.Faculty.ID equals Faculty.ID
                            join University in _context.Universities on Faculty.University.ID equals University.ID
                            where Lecturer.ID == id
                            select new
                            {
                                University = University,
                                Faculty = Faculty,
                                Department = Department,
                                Lecturer = Lecturer
                            }).First();
            Lecturer lecturer1 = lecturer.Lecturer;
            lecturer1.Department = lecturer.Department;
            lecturer1.Department.Faculty = lecturer.Faculty;
            lecturer1.Department.Faculty.University = lecturer.University;
            if (lecturer == null)
            {
                return NotFound();
            }

            dynamic mymodel = new ExpandoObject();
            mymodel.Lecturer = lecturer1;
            mymodel.Lessons = lessonList;

            return View(mymodel);
        }

        // GET: Lecturers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lecturers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Rank,Salary,Name,Surname,Birthday,Email,Phone")] Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                lecturer.Department = await _context.Departments.FindAsync(1);
                _context.Add(lecturer);
                await _context.SaveChangesAsync();
                User u = new User();
                String[] toUserName = lecturer.Email.Split("@");
                u.UserName = toUserName[0];
                u.Password = "123321";
                u.UserType = 1;
                u.person = _context.Lecturers.FirstOrDefault(m => m.Email == lecturer.Email).ID;
                _context.Users.Add(u);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lecturer);
        }

        // GET: Lecturers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return NotFound();
            }
            return View(lecturer);
        }

        // POST: Lecturers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Rank,Salary,Name,Surname,Birthday,Email,Phone")] Lecturer lecturer)
        {
            if (id != lecturer.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lecturer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LecturerExists(lecturer.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(lecturer);
        }

        // GET: Lecturers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _context.Lecturers
            .FirstOrDefaultAsync(m => m.ID == id);
            if (lecturer == null)
            {
                return NotFound();
            }

            return View(lecturer);
        }

        // POST: Lecturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            _context.Lecturers.Remove(lecturer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LecturerExists(int id)
        {
            return _context.Lecturers.Any(e => e.ID == id);
        }
    }
}