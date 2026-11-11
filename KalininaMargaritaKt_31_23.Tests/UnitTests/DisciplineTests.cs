using Xunit;
using KalininaMargaritaKt_31_23.Models;

namespace KalininaMargaritaKt_31_23.Tests.UnitTests
{
    public class DisciplineTests
    {
        // ПОЗИТИВНЫЕ ТЕСТЫ

        [Fact]
        public void IsValidDisciplineName_ValidName_ReturnsTrue()
        {
            // Arrange
            string testName = "Базы данных";

            // Act
            bool result = Discipline.IsValidDisciplineName(testName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidDisciplineName_ValidNameWithHyphen_ReturnsTrue()
        {
            // Arrange
            string testName = "Объектно-ориентированное программирование";

            // Act
            bool result = Discipline.IsValidDisciplineName(testName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidDisciplineName_SingleWord_ReturnsTrue()
        {
            // Arrange
            string testName = "Физика";

            // Act
            bool result = Discipline.IsValidDisciplineName(testName);

            // Assert
            Assert.True(result);
        }

        // НЕГАТИВНЫЕ ТЕСТЫ

        [Fact]
        public void IsValidDisciplineName_LeadingSpace_ReturnsFalse()
        {
            // Arrange: Пробел в самом начале
            string testName = " Физика";

            // Act
            bool result = Discipline.IsValidDisciplineName(testName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidDisciplineName_MultipleLeadingSpaces_ReturnsFalse()
        {
            // Arrange: Множество пробелов в начале
            string testName = "     Физика";

            // Act
            bool result = Discipline.IsValidDisciplineName(testName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidDisciplineName_TrailingSpace_ReturnsFalse()
        {
            // Arrange: Пробел в конце
            string testName = "Физика ";

            // Act
            bool result = Discipline.IsValidDisciplineName(testName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidDisciplineName_MultipleTrailingSpaces_ReturnsFalse()
        {
            // Arrange: Множество пробелов в конце
            string testName = "Физика          ";

            // Act
            bool result = Discipline.IsValidDisciplineName(testName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidDisciplineName_MultipleConsecutiveSpaces_ReturnsFalse()
        {
            // Arrange: Двойной пробел внутри строки
            string testName = "Базы  данных";

            // Act
            bool result = Discipline.IsValidDisciplineName(testName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidDisciplineName_StartsWithHyphen_ReturnsFalse()
        {
            // Arrange: Начинается с дефиса
            string testName = "-Физика";

            // Act
            bool result = Discipline.IsValidDisciplineName(testName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidDisciplineName_EndsWithHyphen_ReturnsFalse()
        {
            // Arrange: Заканчивается дефисом
            string testName = "Физика-";

            // Act
            bool result = Discipline.IsValidDisciplineName(testName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidDisciplineName_ConsecutiveHyphens_ReturnsFalse()
        {
            // Arrange: Два дефиса подряд
            string testName = "Физика--математика";

            // Act
            bool result = Discipline.IsValidDisciplineName(testName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidDisciplineName_NullString_ReturnsFalse()
        {
            // Arrange: null значение
            string testName = null;

            // Act
            bool result = Discipline.IsValidDisciplineName(testName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidDisciplineName_EmptyString_ReturnsFalse()
        {
            // Arrange: Пустая строка
            string testName = "";

            // Act
            bool result = Discipline.IsValidDisciplineName(testName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidDisciplineName_NameWithDigits_ReturnsFalse()
        {
            // Arrange: Содержит цифры
            string testName = "Информатика 2";

            // Act
            bool result = Discipline.IsValidDisciplineName(testName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidDisciplineName_SpecialCharacters_ReturnsFalse()
        {
            // Arrange: Содержит недопустимые спецсимволы
            string testName = "Базы данных @#$";

            // Act
            bool result = Discipline.IsValidDisciplineName(testName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidDisciplineName_TooLongName_ReturnsFalse()
        {
            // Arrange: Превышение лимита в 150 символов
            string testName = new string('А', 151);

            // Act
            bool result = Discipline.IsValidDisciplineName(testName);

            // Assert
            Assert.False(result);
        }
    }
}