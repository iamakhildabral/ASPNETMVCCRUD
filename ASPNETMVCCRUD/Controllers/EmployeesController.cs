using ASPNETMVCCRUD.Data;
using ASPNETMVCCRUD.Models;
using ASPNETMVCCRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETMVCCRUD.Controllers
{
   
    public class EmployeesController : Controller
    {
        private readonly MVCDemoDBContext mVCDemoDBContext;

        public EmployeesController(MVCDemoDBContext mVCDemoDBContext)
        {
            this.mVCDemoDBContext = mVCDemoDBContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await mVCDemoDBContext.Employees.ToListAsync();
            return View(employees);
        }

        //for adding new employee
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employees
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                Department = addEmployeeRequest.Department,
                DateOfBirth = addEmployeeRequest.DateOfBirth
            };

            await mVCDemoDBContext.Employees.AddAsync(employee);
            await mVCDemoDBContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var tempEmployee = await mVCDemoDBContext.Employees.FirstOrDefaultAsync(x => x.Id == @id);
            if (tempEmployee != null) {
                var employeeViewModel = new UpdateEmployeeViewModel()
                {
                    Id = tempEmployee.Id,
                    Name = tempEmployee.Name,
                    Email = tempEmployee.Email,
                    Salary = tempEmployee.Salary,
                    Department = tempEmployee.Department,
                    DateOfBirth = tempEmployee.DateOfBirth
                };
                return await Task.Run(() => View("View",employeeViewModel));
            }
            else
            {
                return RedirectToAction("Index");
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await mVCDemoDBContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.Department = model.Department;
                employee.DateOfBirth = model.DateOfBirth;
                // we dont need to save the employee instance as this will save any changes made in the 
                // Db context in this method
                await mVCDemoDBContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel moded)
        {
            var employee = await mVCDemoDBContext.Employees.FindAsync(moded.Id);
            if (employee != null)
            {
                mVCDemoDBContext.Employees.Remove(employee);
                await mVCDemoDBContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}
