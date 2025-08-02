using HCMS.Infrastructure.Entities;
using HCMS.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HCMS.Infrastructure.Data.SeedDb
{
    public static class DbSeeder
    {
        private static readonly SeedData data = new SeedData();

        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var repository = serviceProvider.GetRequiredService<IRepository>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (repository.AllAsReadOnly<Employee>().Any())
            {
                return; 
            }

            await SeedRolesAsync(roleManager);
            await SeedDepartmentsAsync(repository);
            await SeedEmployeesAsync(repository);
            await SeedHRAdminUserAsync(repository, userManager);
            await SeedApplicationUsersAsync(userManager);
        }

        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = ["Employee", "Manager", "HRAdmin"];

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public static async Task SeedHRAdminUserAsync(IRepository repository, UserManager<ApplicationUser> userManager)
        {
            var adminEmail = "admin@gmail.com";

            var existingUser = await userManager.Users
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.Email == adminEmail);

            if (existingUser == null)
            {
                var employee = new Employee
                {
                    FirstName = "Admin",
                    LastName = "Adminov",
                    Email = adminEmail,
                    JobTitle = "HR Administrator",
                    Salary = 7000,
                    DepartmentId = 2
                };

                await repository.AddAsync(employee);
                await repository.SaveChangesAsync();

                var user = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = adminEmail,
                    EmployeeId = employee.Id
                };

                var result = await userManager.CreateAsync(user, "a12345");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "HRAdmin");
                }
                else
                {
                    throw new Exception("Failed to seed HR Admin user");
                }
            }
        }

        private static async Task SeedDepartmentsAsync(IRepository repository)
        {
            if (!repository.AllAsReadOnly<Department>().Any())
            {
                await repository.AddAsync(data.FirstDepartment);
                await repository.AddAsync(data.SecondDepartment);
                await repository.SaveChangesAsync();
            }
        }

        private static async Task SeedEmployeesAsync(IRepository repository)
        {
            if (!repository.AllAsReadOnly<Employee>().Any())
            {
                await repository.AddAsync(data.FirstEmployee);
                await repository.AddAsync(data.SecondEmployee);
                await repository.AddAsync(data.ThirdEmployee);
                await repository.SaveChangesAsync();
            }
        }

        private static async Task SeedApplicationUsersAsync(UserManager<ApplicationUser> userManager)
        {
            var usersToSeed = new[]
            {
                (data.FirstApplicationUser, "Employee", "a12345"),
                (data.SecondApplicationUser, "Employee", "a12345"),
                (data.ThirdApplicationUser, "Manager", "a12345")
            };

            foreach (var (user, role, password) in usersToSeed)
            {
                var existingUser = await userManager.FindByEmailAsync(user.Email);

                if (existingUser == null)
                {
                    var result = await userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                    else
                    {
                        throw new Exception($"Failed to seed user {user.Email}");
                    }
                }

            }
        }
    }
}