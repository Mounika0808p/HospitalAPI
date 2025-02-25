namespace HospitalAPI.Models
{
    public class Patient
    {
	    public int PatientId { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string ContactNumber { get; set; }
    }
}
