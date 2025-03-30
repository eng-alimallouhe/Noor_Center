namespace LMS.Domain.Entities.Users
{
    public class EmployeeDepartment
    {
        //primary key
        public Guid EmployeeDepartmentId { get; set; }

        //Foreign Key: EmployeeId ==> one(employee)-to-many(employeeDepartments) relationship
        public Guid EmployeeId { get; set; }

        //Foreign Key: DepartmentId ==> one(department)-to-many(employeeDepartments) relationship
        public Guid DepartmentId { get; set; }

        public string AppointmentDecisionUrl { get; set; } = string.Empty;

        //Soft Delete:
        public bool IsActive { get; set; } = true;

        //Timestamp:
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; } = DateTime.UtcNow;

        //Navigation Property:
        public Employee Employee { get; set; } = null!;
        public Department Department { get; set; } = null!;

        public EmployeeDepartment()
        {
            EmployeeDepartmentId = Guid.NewGuid();
        }
    }
}
