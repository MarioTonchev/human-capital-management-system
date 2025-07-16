using HCMS.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HCMS.Infrastructure.Data.SeedDb
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            var data = new SeedData();

            builder.HasData(new Employee[] {data.FirstEmployee, data.SecondEmployee, data.ThirdEmployee});
        }
    }
}
