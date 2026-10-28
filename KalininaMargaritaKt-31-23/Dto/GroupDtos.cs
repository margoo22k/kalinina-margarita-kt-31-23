namespace KalininaMargaritaKt_31_23.Dto
{
    public class GroupFilter
    {
        public int? SpecialtyId { get; set; }
        public int? Course { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
    public class GroupRequest
    {
        public string? Name { get; set; }
        public int Course { get; set; }
        public int SpecialtyId { get; set; }
    }
    public record GroupDto(int GroupId, string? Name, int Course, int SpecialtyId, string? SpecialtyTitle, bool IsDeleted);
}