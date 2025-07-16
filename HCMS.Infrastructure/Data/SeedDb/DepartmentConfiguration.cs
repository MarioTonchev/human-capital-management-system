using HCMS.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HCMS.Infrastructure.Data.SeedDb
{
    internal class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            var data = new SeedData();

            builder.HasData(new Department[] {data.FirstDepartment, data.SecondDepartment});    
        }
    }
}
