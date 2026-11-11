using System.Text.RegularExpressions;

namespace KalininaMargaritaKt_31_23.Models
{
    public class Discipline
    {
        public int DisciplineId { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<Grade> Grades { get; set; }


        // Метод для юнит-тестирования: проверка корректности названия дисциплины
        public static bool IsValidDisciplineName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            if (name.Length > 150)
                return false;

            var pattern = @"^[А-Яа-я]+(?:[\s\-][А-Яа-я]+)*$";
            return Regex.IsMatch(name, pattern);
        }
    }
}
