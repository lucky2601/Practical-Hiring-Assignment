namespace PracticalHiring.Models
{
    public class AttendanceSummaryViewModel
    {
        public string EmployeeName { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int LeaveCount { get; set; }
    }
}