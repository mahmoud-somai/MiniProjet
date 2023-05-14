using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniProjet.Data;
using MiniProjet.Models;
using MiniProjet.Models.Domain;

namespace MiniProjet.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MVCDemoDbContext mvcDemoDbContext;
        public EmployeesController(MVCDemoDbContext mvcDemoDbContext)
        { 
            this.mvcDemoDbContext=mvcDemoDbContext;
        }

        [HttpGet]
        public async Task<IActionResult>Index()
        {
          var employees=  await mvcDemoDbContext.employees.ToListAsync();
          return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View ();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                DateOfBirth = addEmployeeRequest.DateOfBirth,
                Department = addEmployeeRequest.Department,
            };
            await mvcDemoDbContext.employees.AddAsync(employee);
            await mvcDemoDbContext.SaveChangesAsync();
            return RedirectToAction("Add");
         }

        [HttpGet]
        public async Task <IActionResult> View(Guid id) 
        {
            var employee= await mvcDemoDbContext.employees.FirstOrDefaultAsync(x => x.Id == id);
            
            if(employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    DateOfBirth = employee.DateOfBirth,
                    Department = employee.Department,
                };
                return await Task.Run(()=>View("View",viewModel));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await mvcDemoDbContext.employees.FindAsync(model.Id);
            if(employee != null)
            {
                employee.Name=model.Name;
                employee.Email=model.Email;
                employee.Salary=model.Salary;
                employee.DateOfBirth=model.DateOfBirth;
                employee.Department=model.Department;

                await mvcDemoDbContext.SaveChangesAsync();
                RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee=await mvcDemoDbContext.employees.FindAsync(model.Id);
            if(employee != null)
            {
                mvcDemoDbContext.employees.Remove(employee);
                await mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
