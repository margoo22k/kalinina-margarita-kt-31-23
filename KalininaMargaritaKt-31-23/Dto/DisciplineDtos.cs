namespace KalininaMargaritaKt_31_23.Dto
{
    public class DisciplineHas2GradeStudentFilter { public string? DisciplineName { get; set; } }
    public class DisciplineFilter
    {
        public string? Name { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
    public class DisciplineRequest
    {
        public string? Name { get; set; }
    }
    public record DisciplineDto(int DisciplineId, string? Name, bool IsDeleted);


}