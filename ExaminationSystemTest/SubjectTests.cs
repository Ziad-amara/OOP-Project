using ExaminationSystem;

namespace ExaminationSystemTest
{
    public class SubjectTests
    {
        [Fact]
        public void Constructor_ValidValues_SetsProperties()
        {
            var subject = new Subject(1, "Data Structures");

            Assert.Equal(1, subject.SubjectId);
            Assert.Equal("Data Structures", subject.SubjectName);
            Assert.Null(subject.Exam);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_NonPositiveId_Throws(int invalidId)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Subject(invalidId, "name"));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Constructor_BlankName_Throws(string? invalidName)
        {
            Assert.Throws<ArgumentException>(() => new Subject(1, invalidName!));
        }

        [Fact]
        public void CreateExam_Final_CreatesFinalExamAndAssignsIt()
        {
            var subject = new Subject(1, "Math");

            Exam exam = subject.CreateExam("Final", 60, 2);

            Assert.IsType<FinalExam>(exam);
            Assert.Same(exam, subject.Exam);
        }

        [Fact]
        public void CreateExam_Practical_CreatesPracticalExam()
        {
            var subject = new Subject(1, "Math");

            Exam exam = subject.CreateExam("Practical", 60, 2);

            Assert.IsType<PracticalExam>(exam);
        }

        [Theory]
        [InlineData("final")]
        [InlineData("FINAL")]
        [InlineData("FiNaL")]
        public void CreateExam_ExamTypeIsCaseInsensitive(string examType)
        {
            var subject = new Subject(1, "Math");

            Exam exam = subject.CreateExam(examType, 60, 2);

            Assert.IsType<FinalExam>(exam);
        }

        [Fact]
        public void CreateExam_UnrecognizedType_Throws()
        {
            var subject = new Subject(1, "Math");

            Assert.Throws<ArgumentException>(() => subject.CreateExam("Midterm", 60, 2));
        }

        [Fact]
        public void CreateExam_NullType_Throws()
        {
            var subject = new Subject(1, "Math");

            Assert.Throws<ArgumentNullException>(() => subject.CreateExam(null!, 60, 2));
        }

        [Fact]
        public void ToString_IncludesIdNameAndExam()
        {
            var subject = new Subject(1, "Math");
            subject.CreateExam("Final", 60, 2);

            string result = subject.ToString();

            Assert.Contains("1", result);
            Assert.Contains("Math", result);
            Assert.Contains("Final", result);
        }
    }
}
