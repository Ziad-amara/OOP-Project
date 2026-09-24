using ExaminationSystem;

namespace ExaminationSystemTest
{
    public class ExamTests
    {
        private static MCQQuestion CreateMcq(int mark = 5)
        {
            var a1 = new Answer(1, "Stack");
            var a2 = new Answer(2, "Queue");
            return new MCQQuestion("Q1", "Which is LIFO?", mark, new[] { a1, a2 }, a1);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Constructor_NonPositiveTime_Throws(int invalidTime)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new FinalExam(invalidTime, 2));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_NonPositiveQuestionCount_Throws(int invalidCount)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new FinalExam(60, invalidCount));
        }

        [Fact]
        public void Constructor_ValidValues_AllocatesArraysOfCorrectSize()
        {
            var exam = new FinalExam(60, 3);

            Assert.Equal(3, exam.Questions.Length);
            Assert.Equal(3, exam.StudentAnswers.Length);
        }

        [Fact]
        public void AddQuestion_Null_Throws()
        {
            var exam = new FinalExam(60, 2);

            Assert.Throws<ArgumentNullException>(() => exam.AddQuestion(null!));
        }

        [Fact]
        public void AddQuestion_StoresAClone_NotTheOriginalReference()
        {
            var exam = new FinalExam(60, 1);
            MCQQuestion original = CreateMcq();

            exam.AddQuestion(original);
            original.Header = "Mutated after adding";

            Assert.NotEqual("Mutated after adding", exam.Questions[0]!.Header);
        }

        [Fact]
        public void AddQuestion_WhenFull_Throws()
        {
            var exam = new FinalExam(60, 1);
            exam.AddQuestion(CreateMcq());

            Assert.Throws<InvalidOperationException>(() => exam.AddQuestion(CreateMcq()));
        }

        [Fact]
        public void SetStudentAnswer_NonPositiveAnswerId_Throws()
        {
            var exam = new FinalExam(60, 1);
            exam.AddQuestion(CreateMcq());

            Assert.Throws<ArgumentOutOfRangeException>(() => exam.SetStudentAnswer(0, 0));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(5)]
        public void SetStudentAnswer_IndexOutOfRange_Throws(int invalidIndex)
        {
            var exam = new FinalExam(60, 1);
            exam.AddQuestion(CreateMcq());

            Assert.Throws<ArgumentOutOfRangeException>(() => exam.SetStudentAnswer(invalidIndex, 1));
        }

        [Fact]
        public void SetStudentAnswer_NoQuestionAtIndex_Throws()
        {
            var exam = new FinalExam(60, 2); // only 1 question added below
            exam.AddQuestion(CreateMcq());

            Assert.Throws<InvalidOperationException>(() => exam.SetStudentAnswer(1, 1));
        }

        [Fact]
        public void SetStudentAnswer_AnswerIdNotBelongingToQuestion_Throws()
        {
            var exam = new FinalExam(60, 1);
            exam.AddQuestion(CreateMcq()); // answer IDs 1 and 2 only

            Assert.Throws<ArgumentException>(() => exam.SetStudentAnswer(0, 999));
        }

        [Fact]
        public void SetStudentAnswer_ValidAnswer_RecordsIt()
        {
            var exam = new FinalExam(60, 1);
            exam.AddQuestion(CreateMcq());

            exam.SetStudentAnswer(0, 1);

            Assert.Equal(1, exam.StudentAnswers[0]);
        }

        [Fact]
        public void Clone_ProducesDeepCopy_IndependentQuestionsAndAnswers()
        {
            var exam = new FinalExam(60, 1);
            exam.AddQuestion(CreateMcq());
            exam.SetStudentAnswer(0, 1);

            var clone = (FinalExam)exam.Clone();
            clone.Questions[0]!.Header = "Changed in clone";
            clone.StudentAnswers[0] = 2;

            Assert.NotEqual("Changed in clone", exam.Questions[0]!.Header);
            Assert.Equal(1, exam.StudentAnswers[0]);
        }

        [Fact]
        public void CompareTo_ComparesByNumberOfQuestions()
        {
            var small = new FinalExam(60, 1);
            var large = new FinalExam(60, 5);

            Assert.True(small.CompareTo(large) < 0);
            Assert.True(large.CompareTo(small) > 0);
        }

        [Fact]
        public void CompareTo_Null_ReturnsPositive()
        {
            var exam = new FinalExam(60, 1);

            Assert.True(exam.CompareTo(null) > 0);
        }

        [Fact]
        public void CompareTo_NonExamObject_Throws()
        {
            var exam = new FinalExam(60, 1);

            Assert.Throws<ArgumentException>(() => exam.CompareTo("not an exam"));
        }

        [Fact]
        public void ToString_IncludesTimeAndQuestionCount()
        {
            var exam = new FinalExam(45, 3);

            string result = exam.ToString();

            Assert.Contains("45", result);
            Assert.Contains("3", result);
        }
    }
}
