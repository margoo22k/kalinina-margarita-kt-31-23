namespace KalininaMargaritaKt_31_23.Dto
{

    public class GroupDisciplineAverageFilter { public int GroupId { get; set; } public int DisciplineId { get; set; } }
    public class StudentGradesFilter { public int StudentId { get; set; } public int? DisciplineId { get; set; } }
    public class YearAverageFilter { public int Course { get; set; } public int? DisciplineId { get; set; } }
    public class AddGradeRequest { public int StudentId { get; set; } public int DisciplineId { get; set; } public int Value { get; set; } }
    public class UpdateGradeRequest { public int Value { get; set; } }

    public record GradeDto(int GradeId, int StudentId, int DisciplineId, string? DisciplineName, int Value);
    public record StudentGradesDto(int StudentId, string FullName, double? Average, List<GradeDto> Grades);
    public record AverageGradeDto(double? Average, int GradesCount);
}