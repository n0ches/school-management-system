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
    public class DepartmentsController : Controller
    {
        private readonly SchoolContext _context;

        public DepartmentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var departments = (from Department in _context.Departments
                             join Faculty in _context.Faculties on Department.Faculty.ID equals Faculty.ID
                             join University in _context.Universities on Faculty.University.ID equals University.ID
                             select new
                             {
                                 Department = Department,
                                 Faculty = Faculty,
                                 University = University
                             }).ToList();

            List<Department> departmentList = new List<Department>();
            departments.ForEach(d =>
            {
                Department department = d.Department;
                department.Faculty = d.Faculty;
                department.Faculty.University = d.University;
                departmentList.Add(department);
            });

            dynamic mymodel = new ExpandoObject();
            mymodel.Departments = departmentList;
            return View(mymodel);
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturers = (from Department in _context.Departments
                               join Lecturer in _context.Lecturers on Department.ID equals Lecturer.Department.ID
                               where Department.ID == id
                               select new
                               {
                                   Lecturer = Lecturer,
                               }).ToList();

            List<Lecturer> lecturerList = new List<Lecturer>();

            lecturers.ForEach(l =>
            {
                Lecturer lecturer = l.Lecturer;
                lecturerList.Add(lecturer);
            });



            var department = (from Department in _context.Departments
                              join Faculty in _context.Faculties on Department.Faculty.ID equals Faculty.ID
                              where Department.ID == id
                              select new
                              {
                                  Faculty = Faculty,
                                  Department= Department
                              }).First();

            Department department1 = department.Department;
            department1.Faculty = department.Faculty;
            if (department == null)
            {
                return NotFound();
            }

            dynamic mymodel = new ExpandoObject();
            mymodel.Department = department1;
            mymodel.Lecturers = lecturerList;

            return View(mymodel);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Language,PrimaryEducation,SecondaryEducation")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Language,PrimaryEducation,SecondaryEducation")] Department department)
        {
            if (id != department.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.ID))
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
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.ID == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.ID == id);
        }
    }
}
