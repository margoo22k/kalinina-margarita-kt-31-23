namespace KalininaMargaritaKt_31_23.Dto
{
    public class StudentFilter
    {
        public string? GroupName { get; set; }
        public string? FullName { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
    public class StudentRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int GroupId { get; set; }
    }
    public record StudentDto(int StudentId, string? FirstName, string? LastName, int GroupId, string? GroupName, bool IsDeleted);
}