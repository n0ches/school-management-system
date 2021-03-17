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
    public class FacultiesController : Controller
    {
        private readonly SchoolContext _context;

        public FacultiesController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Faculties
        public async Task<IActionResult> Index()
        {
            var faculties = (from Faculty in _context.Faculties
                             join University in _context.Universities on Faculty.University.ID equals University.ID
                             select new
                             {
                                 Faculty = Faculty,
                                 University = University
                             }).ToList();

            List<Faculty> facultyList = new List<Faculty>();
            faculties.ForEach(f =>
            {
                Faculty faculty = f.Faculty;
                faculty.University = f.University;
                facultyList.Add(faculty);
            });
            dynamic mymodel = new ExpandoObject();
            mymodel.Faculties = facultyList;
            return View(mymodel);
        }

        // GET: Faculties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departments = (from Faculty in _context.Faculties
                            join Department in _context.Departments on Faculty.ID equals Department.Faculty.ID
                            where Faculty.ID == id
                            select new
                            {
                                Department = Department,
                            }).ToList();

            List<Department> departmentList = new List<Department>();

            departments.ForEach(d =>
            {
                Department department = d.Department;
                departmentList.Add(department);
            });


            var faculty = await _context.Faculties
                .FirstOrDefaultAsync(m => m.ID == id);
            if (faculty == null)
            {
                return NotFound();
            }

            dynamic mymodel = new ExpandoObject();
            mymodel.Faculty = faculty;
            mymodel.Departments = departmentList;

            return View(mymodel);
        }

        // GET: Faculties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Faculties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                _context.Add(faculty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(faculty);
        }

        // GET: Faculties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty == null)
            {
                return NotFound();
            }
            return View(faculty);
        }

        // POST: Faculties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] Faculty faculty)
        {
            if (id != faculty.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faculty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacultyExists(faculty.ID))
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
            return View(faculty);
        }

        // GET: Faculties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties
                .FirstOrDefaultAsync(m => m.ID == id);
            if (faculty == null)
            {
                return NotFound();
            }

            return View(faculty);
        }

        // POST: Faculties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faculty = await _context.Faculties.FindAsync(id);
            _context.Faculties.Remove(faculty);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacultyExists(int id)
        {
            return _context.Faculties.Any(e => e.ID == id);
        }
    }
}
