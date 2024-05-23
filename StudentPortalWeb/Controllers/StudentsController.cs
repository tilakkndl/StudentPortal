using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortalWeb.Data;
using StudentPortalWeb.Models;
using StudentPortalWeb.Models.Entities;

namespace StudentPortalWeb.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public StudentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Add(AddStudentViewModal viewModal)
        {
            var student = new Student
            {
                Name = viewModal.Name,
                Email = viewModal.Email,
                Phone = viewModal.Phone,
                Subscribed = viewModal.Subscribed,

            };
            await dbContext.Students.AddAsync(student);
            await dbContext.SaveChangesAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        { 
            var students = await dbContext.Students.ToListAsync();
            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var student = await dbContext.Students.FindAsync(id);
            return View(student);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(Student ViewModel)
        {
            var student = await dbContext.Students.FindAsync(ViewModel.Id);
            if(student is not null)
            {
                student.Name = ViewModel.Name;
                student.Email=ViewModel.Email;
                student.Subscribed = ViewModel.Subscribed;
                student.Phone=ViewModel.Phone;
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Students");
        }

        public async Task<IActionResult> Delete(Student ViewModel)
        {
            var student = await dbContext.Students.
                AsNoTracking().
                FirstOrDefaultAsync(x=>x.Id ==ViewModel.Id);
if( student is not null)
            {
                dbContext.Students.Remove(ViewModel);
                await dbContext.SaveChangesAsync();

            }
                return RedirectToAction("List", "Students");

        }
    }
}
