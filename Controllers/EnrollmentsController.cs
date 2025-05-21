using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Workshop.DbContext;
using Workshop.Models;

namespace Workshop.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly UniversityContext _context;

        public EnrollmentsController(UniversityContext context)
        {
            _context = context;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index(int? courseId, long? studentId, string semester, int? year)
        {
            var enrollments = _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .AsQueryable();

            if (courseId.HasValue)
                enrollments = enrollments.Where(e => e.CourseId == courseId.Value);

            if (studentId.HasValue)
                enrollments = enrollments.Where(e => e.StudentId == studentId.Value);

            if (!string.IsNullOrEmpty(semester))
                enrollments = enrollments.Where(e => e.Semester == semester);

            if (year.HasValue)
                enrollments = enrollments.Where(e => e.Year == year.Value);

            ViewBag.Courses = await _context.Courses
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Title
                }).ToListAsync();

            ViewBag.Students = await _context.Students
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.FirstName + " " + s.LastName
                }).ToListAsync();

            return View(await enrollments.ToListAsync());
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
                return NotFound();

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollment == null)
                return NotFound();

            return View(enrollment);
        }

        // GET: Enrollments/Create
        public IActionResult Create()
        {
            ViewBag.CourseId = new SelectList(_context.Courses, "Id", "Title");
            ViewBag.StudentId = new SelectList(_context.Students, "Id", "FirstName");
            return View();
        }

        // POST: Enrollments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseId,StudentId,Semester,Year,Grade,SeminalUrl,ProjectUrl,ExamPoints,SeminalPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CourseId = new SelectList(_context.Courses, "Id", "Title", enrollment.CourseId);
            ViewBag.StudentId = new SelectList(_context.Students, "Id", "FirstName", enrollment.StudentId);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
                return NotFound();

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
                return NotFound();

            ViewBag.CourseId = new SelectList(_context.Courses, "Id", "Title", enrollment.CourseId);
            ViewBag.StudentId = new SelectList(_context.Students, "Id", "FirstName", enrollment.StudentId);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,CourseId,StudentId,Semester,Year,Grade,SeminalUrl,ProjectUrl,ExamPoints,SeminalPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (id != enrollment.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CourseId = new SelectList(_context.Courses, "Id", "Title", enrollment.CourseId);
            ViewBag.StudentId = new SelectList(_context.Students, "Id", "FirstName", enrollment.StudentId);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
                return NotFound();

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollment == null)
                return NotFound();

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment != null)
                _context.Enrollments.Remove(enrollment);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(long id)
        {
            return _context.Enrollments.Any(e => e.Id == id);
        }
    }
}
