using HCMS.Infrastructure.Entities;

namespace HCMS.Infrastructure.Data.SeedDb
{
    internal class SeedData
    {
        public Employee FirstEmployee { get; set; }
        public Employee SecondEmployee { get; set; }
        public Employee ThirdEmployee { get; set; }
        public Department FirstDepartment { get; set; }
        public Department SecondDepartment { get; set; }

        public SeedData()
        {
            SeedDepartments();
            SeedEmployees();
        }

        private void SeedEmployees()
        {
            FirstEmployee = new Employee()
            {
                Id = 1,
                FirstName = "Pesho",
                LastName = "Peshov",
                Email = "pesho@gmail.com",
                JobTitle = "Software Developer",
                Salary = 5000,
                DepartmentId = 1
            };

            SecondEmployee = new Employee()
            {
                Id = 2,
                FirstName = "Gosho",
                LastName = "Goshov",
                Email = "gosho@gmail.com",
                JobTitle = "AI Engineer",
                Salary = 6000,
                DepartmentId = 1
            };

            ThirdEmployee = new Employee()
            {
                Id = 3,
                FirstName = "Koko",
                LastName = "Kokov",
                Email = "koko@gmail.com",
                JobTitle = "HR Employee",
                Salary = 5000,
                DepartmentId = 2
            };
        }

        private void SeedDepartments()
        {
            FirstDepartment = new Department()
            {
                Id = 1,
                Name = "IT"
            };
            
            SecondDepartment = new Department()
            {
                Id = 2,
                Name = "HR"
            };
        }
    }
}
