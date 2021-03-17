using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SchoolSystemManagement.Data;
using SchoolSystemManagement.Models;

namespace SchoolSystemManagement.Controllers
{
    public class StudentLessonsController : Controller
    {
        private readonly SchoolContext _context;

        public StudentLessonsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: StudentLessons
        public async Task<IActionResult> Index()
        {
            return View(await _context.StudentLessons.ToListAsync());
        }

        // GET: StudentLessons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentLesson = await _context.StudentLessons
                .FirstOrDefaultAsync(m => m.ID == id);
            if (studentLesson == null)
            {
                return NotFound();
            }

            return View(studentLesson);
        }

        // GET: StudentLessons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudentLessons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Grade")] StudentLesson studentLesson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentLesson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studentLesson);
        }

        // GET: StudentLessons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var studentLesson = (from StudentLesson in _context.StudentLessons
                           join Student in _context.Students on StudentLesson.Student.ID equals Student.ID
                           join Lesson in _context.Lessons on StudentLesson.Lesson.ID equals Lesson.ID
                           where StudentLesson.ID == id
                           select new
                           {
                               StudentLesson = StudentLesson,
                               Student = Student,
                               Lesson = Lesson,
                           }).First();


            
            if (studentLesson == null)
            {
                return NotFound();
            }

            StudentLesson studentLesson1 = studentLesson.StudentLesson;
            studentLesson1.Student = studentLesson.Student;
            studentLesson1.Lesson = studentLesson.Lesson;
            return View(studentLesson1);
        }

        // POST: StudentLessons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Grade")] StudentLesson studentLesson)
        {
            if (id != studentLesson.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentLesson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentLessonExists(studentLesson.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var sl = (from StudentLesson in _context.StudentLessons
                                     join Lesson in _context.Lessons on StudentLesson.Lesson.ID equals Lesson.ID
                                     where StudentLesson.ID == id
                                     select new
                                     {
                                         StudentLesson = StudentLesson,
                                         Lesson = Lesson,
                                     }).First();

                return RedirectToAction("Details", new RouteValueDictionary(
                                    new { controller = "Lessons", action = "Details", id = sl.Lesson.ID}));
            }
            return View(studentLesson);
        }

        // GET: StudentLessons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentLesson = await _context.StudentLessons
                .FirstOrDefaultAsync(m => m.ID == id);
            if (studentLesson == null)
            {
                return NotFound();
            }

            return View(studentLesson);
        }

        // POST: StudentLessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentLesson = await _context.StudentLessons.FindAsync(id);
            _context.StudentLessons.Remove(studentLesson);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentLessonExists(int id)
        {
            return _context.StudentLessons.Any(e => e.ID == id);
        }
    }
}
