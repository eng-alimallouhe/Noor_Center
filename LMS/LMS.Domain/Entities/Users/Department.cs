using LMS.Domain.Entities.Orders;

namespace LMS.Domain.Entities.Users
{
    public class Department
    {
        //primary key
        public Guid DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        //soft delete:
        public bool IsActive { get; set; } = true;

        //Timestamp:
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //navigation property:
        public ICollection<EmployeeDepartment> EmployeeDepartments { get; set; } = [];
        public ICollection<Order> Orders { get; set; } = [];

        public Department()
        {
            DepartmentId = Guid.NewGuid();
        }
    }
}
