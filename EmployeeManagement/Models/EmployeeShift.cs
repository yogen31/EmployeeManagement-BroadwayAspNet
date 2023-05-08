namespace EmployeeManagement.Models
{
    public class EmployeeShift
    {
        public int Id { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int TotalTime { get; set; }
        public int EmployeeId1 { get; set; }
        public Employee Employee { get; set; }
        public List<EmployeeShiftLog> EmployeeShiftLogs { get; set; }
        
    }
}
