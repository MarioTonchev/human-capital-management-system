using HCMS.Infrastructure.Entities;

namespace HCMS.Infrastructure.Data.SeedDb
{
    internal class SeedData
    {
        public ApplicationUser FirstApplicationUser { get; set; }
        public ApplicationUser SecondApplicationUser { get; set; }
        public ApplicationUser ThirdApplicationUser { get; set; }
        public Employee FirstEmployee { get; set; }
        public Employee SecondEmployee { get; set; }
        public Employee ThirdEmployee { get; set; }
        public Department FirstDepartment { get; set; }
        public Department SecondDepartment { get; set; }

        public SeedData()
        {
            SeedDepartments();
            SeedEmployees();
            SeedApplicationUsers();
        }

        private void SeedEmployees()
        {
            FirstEmployee = new Employee()
            {
                FirstName = "Pesho",
                LastName = "Peshov",
                Email = "pesho@gmail.com",
                JobTitle = "Software Developer",
                Salary = 5000,
                DepartmentId = 1
            };

            SecondEmployee = new Employee()
            {
                FirstName = "Gosho",
                LastName = "Goshov",
                Email = "gosho@gmail.com",
                JobTitle = "AI Engineer",
                Salary = 6000,
                DepartmentId = 1
            };

            ThirdEmployee = new Employee()
            {
                FirstName = "Koko",
                LastName = "Kokov",
                Email = "koko@gmail.com",
                JobTitle = "HR Employee",
                Salary = 5000,
                DepartmentId = 2
            };
        }

        private void SeedApplicationUsers()
        {
            FirstApplicationUser = new ApplicationUser()
            {
                UserName = "pesho",
                Email = "pesho@gmail.com",
                EmployeeId = 1
            };

            SecondApplicationUser = new ApplicationUser()
            {
                UserName = "gosho",
                Email = "gosho@gmail.com",
                EmployeeId = 2
            };
            ThirdApplicationUser = new ApplicationUser()
            {
                UserName = "koko",
                Email = "koko@gmail.com",
                EmployeeId = 3
            };
        }

        private void SeedDepartments()
        {
            FirstDepartment = new Department()
            {
                Name = "IT"
            };
            
            SecondDepartment = new Department()
            {
                Name = "HR"
            };
        }
    }
}
