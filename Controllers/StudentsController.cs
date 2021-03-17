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
    public class StudentsController : Controller
    {
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var students = (from Student in _context.Students
                           join Department in _context.Departments on Student.Department.ID equals Department.ID
                           join Faculty in _context.Faculties on Department.Faculty.ID equals Faculty.ID
                           join University in _context.Universities on Faculty.University.ID equals University.ID
                           select new
                           {
                               Student = Student,
                               Department = Department,
                               Faculty = Faculty,
                               University = University
                           }).ToList();

            List<Student> studentList = new List<Student>();

            students.ForEach(s =>
            {
                Student student = s.Student;
                student.Department = s.Department;
                student.Department.Faculty = s.Faculty;
                student.Department.Faculty.University = s.University;
                studentList.Add(student);
            });

            dynamic mymodel = new ExpandoObject();
            mymodel.Students = studentList;
            return View(mymodel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lessons = (from Student in _context.Students
                       join StudentLesson in _context.StudentLessons on Student.ID equals StudentLesson.Student.ID
                       join Lesson in _context.Lessons on StudentLesson.Lesson.ID equals Lesson.ID
                       join Lecturer in _context.Lecturers on Lesson.Lecturer.ID equals Lecturer.ID
                       where Student.ID == id
                       select new
                       {
                           Lesson = Lesson,
                           Lecturer = Lecturer,
                           StudentLesson = StudentLesson,
                       }).ToList();

            List<StudentLesson> studentLessons = new List<StudentLesson>();

            lessons.ForEach(lesson =>
            {
                StudentLesson studentLesson = lesson.StudentLesson;
                studentLesson.Lesson = lesson.Lesson;
                studentLesson.Lesson.Lecturer = lesson.Lecturer;
                studentLessons.Add(studentLesson);
            });

            

            var student = (from Student in _context.Students
                           join Department in _context.Departments on Student.Department.ID equals Department.ID
                           join Faculty in _context.Faculties on Department.Faculty.ID equals Faculty.ID
                           join University in _context.Universities on Faculty.University.ID equals University.ID
                           where Student.ID == id
                           select new
                           {
                               Student = Student,
                               Department = Department,
                               Faculty = Faculty,
                               University = University
                           }).First();

            
            if (student == null)
            {
                return NotFound();
            }

            Student student1 = student.Student;
            student1.Department = student.Department;
            student1.Department.Faculty = student.Faculty;
            student1.Department.Faculty.University = student.University;

            dynamic mymodel = new ExpandoObject();
            mymodel.Student = student1;
            mymodel.StudentLessons = studentLessons;
           

            return View(mymodel);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SchoolNumber,Class,Name,Surname,Birthday,Email,Phone")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.Department = await _context.Departments.FindAsync(1);
                _context.Add(student);
                await _context.SaveChangesAsync();
                User u = new User();
                String[] toUserName = student.Email.Split("@");
                u.UserName = toUserName[0];
                u.Password = student.SchoolNumber.ToString();
                u.UserType = 0;
                u.person = _context.Students.FirstOrDefault(m => m.Email == student.Email).ID;
                _context.Users.Add(u);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,SchoolNumber,Class,Name,Surname,Birthday,Email,Phone")] Student student)
        {
            if (id != student.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.ID))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
