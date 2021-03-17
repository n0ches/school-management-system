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
    public class LessonsController : Controller
    {
        private readonly SchoolContext _context;

        public LessonsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Lessons
        public async Task<IActionResult> Index()
        {
            var lessons = (from Lesson in _context.Lessons
                            join Lecturer in _context.Lecturers on Lesson.Lecturer.ID equals Lecturer.ID
                            join Department in _context.Departments on Lesson.Department.ID equals Department.ID
                            select new
                            {
                                Department = Department,
                                Lecturer = Lecturer,
                                Lesson = Lesson
                            }).ToList();
            List<Lesson> lessonList = new List<Lesson>();

            lessons.ForEach(l =>
            {
                Lesson lesson = l.Lesson;
                lesson.Lecturer = l.Lecturer;
                lesson.Department = l.Department;
                lessonList.Add(lesson);
            });

            dynamic mymodel = new ExpandoObject();
            mymodel.Lessons = lessonList;
            return View(mymodel);
        }

        // GET: Lessons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var students = (from Lesson in _context.Lessons
                           join StudentLesson in _context.StudentLessons on Lesson.ID equals StudentLesson.Lesson.ID
                           join Student in _context.Students on StudentLesson.Student.ID equals Student.ID
                           join Department in _context.Departments on Student.Department.ID equals Department.ID
                           where Lesson.ID == id
                           select new
                           {
                               Department = Department,
                               Student = Student,
                               StudentLesson = StudentLesson
                           }).ToList();

            List<StudentLesson> studentLessons = new List<StudentLesson>();

            students.ForEach(s =>
            {
                Student student = s.Student;
                student.Department = s.Department;

                StudentLesson studentLesson = s.StudentLesson;
                studentLesson.Student = student;
                studentLessons.Add(studentLesson);

            });

            var lesson = (from Lesson in _context.Lessons
                          join Lecturer in _context.Lecturers on Lesson.Lecturer.ID equals Lecturer.ID
                          where Lesson.ID == id
                          select new
                          {
                              Lesson = Lesson,
                              Lecturer = Lecturer
                          }).First();

            //await _context.Lessons.FirstOrDefaultAsync(m => m.ID == id);
            Lesson lesson1 = lesson.Lesson;
            lesson1.Lecturer = lesson.Lecturer;

            if (lesson1 == null)
            {
                return NotFound();
            }

            dynamic mymodel = new ExpandoObject();
            mymodel.Lesson = lesson1;
            mymodel.StudentLessons = studentLessons;

            return View(mymodel);
        }

        // GET: Lessons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Code,LessonPerWeek,Credit")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                lesson.Lecturer = await _context.Lecturers.FindAsync(1);
                lesson.Department = await _context.Departments.FindAsync(1);
                _context.Add(lesson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lesson);
        }

        // GET: Lessons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Code,LessonPerWeek,Credit")] Lesson lesson)
        {
            if (id != lesson.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lesson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LessonExists(lesson.ID))
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
            return View(lesson);
        }

        // GET: Lessons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LessonExists(int id)
        {
            return _context.Lessons.Any(e => e.ID == id);
        }
    }
}
