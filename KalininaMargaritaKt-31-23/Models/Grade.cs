namespace KalininaMargaritaKt_31_23.Models
{
    public class Grade
    {
        public int GradeId { get; set; }

        public int Value { get; set; }

        public int StudentId { get; set; }

        public Student Student { get; set; }

        public int DisciplineId { get; set; }

        public Discipline Discipline { get; set; }
    }
}
