namespace KalininaMargaritaKt_31_23.Dto
{
    public class SpecialtyFilter { public string? Title { get; set; } public string? Code { get; set; } }
    public class SpecialtyRequest { public string? Title { get; set; } public string? Code { get; set; } }
    public record SpecialtyDto(int SpecialtyId, string? Title, string? Code);
}