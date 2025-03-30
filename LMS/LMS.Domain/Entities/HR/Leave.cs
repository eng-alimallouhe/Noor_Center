using LMS.Domain.Entities.Users;
using LMS.Domain.Enums.HR;

namespace LMS.Domain.Entities.HR
{
    public class Leave
    {
        //primary key
        public Guid LeaveId { get; set; }

        //Foreign Key: EmployeeId ==> one(employee)-to-many(leaves) relationship
        public Guid EmployeeId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveType LeaveType { get; set; }
        public string Reason { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
        public bool IsPaid { get; set; }

        //Soft Delete
        public bool IsActive { get; set; }

        //Navigation Property:
        public Employee Employee { get; set; }

        public Leave()
        {
            LeaveId = Guid.NewGuid();
            IsActive = true;
            Employee = null!;
        }
    }
}
