using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

// namespace StudentManagementSystem.Controllers
namespace StudentManagementSystem.Data
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context; //allows the controller to interact with the database.

        public StudentsController(ApplicationDbContext context) //Constructor
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()  //Index is an action method , responds to HTTP GET requests. It retrieves a list of all Student records from the database.
        {
            return View(await _context.Students.ToListAsync()); //asynchronously fetch all the students from the Students table and convert them into a list.
        }  //It passes this list of students to a view


        // GET: Students/Create  render create view
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,DateOfBirth,Email")] Student student)
        {
            if (ModelState.IsValid)
            {
                if (student.DateOfBirth.Kind != DateTimeKind.Utc)
                {
                    student.DateOfBirth = DateTime.SpecifyKind(student.DateOfBirth, DateTimeKind.Utc);
                }

                _context.Add(student);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,DateOfBirth,Email")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (student.DateOfBirth.Kind != DateTimeKind.Utc)
                    {
                        student.DateOfBirth = DateTime.SpecifyKind(student.DateOfBirth, DateTimeKind.Utc);
                    }
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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

        // Helper method to check if a student exists
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }


        // GET: Students/View/5
        public async Task<IActionResult> Details(int? id)
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



        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }



    }
}
