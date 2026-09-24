using ExaminationSystem;

namespace ExaminationSystemTest
{
    public class PracticalExamTests
    {
        private static MCQQuestion CreateMcq(int mark = 5)
        {
            var a1 = new Answer(1, "Stack");
            var a2 = new Answer(2, "Queue");
            return new MCQQuestion("Q1", "Which is LIFO?", mark, new[] { a1, a2 }, a1);
        }

        [Fact]
        public void AddQuestion_AcceptsMcq()
        {
            var exam = new PracticalExam(60, 1);

            exam.AddQuestion(CreateMcq());

            Assert.NotNull(exam.Questions[0]);
        }

        [Fact]
        public void AddQuestion_TrueFalseQuestion_Throws()
        {
            var exam = new PracticalExam(60, 1);
            var tf = new TrueFalseQuestion("Q", "body", 5, true);

            Assert.Throws<ArgumentException>(() => exam.AddQuestion(tf));
        }

        [Fact]
        public void ShowExam_PrintsRightAnswer_NoGradeLine()
        {
            var exam = new PracticalExam(60, 1);
            exam.AddQuestion(CreateMcq());

            var writer = new StringWriter();
            TextWriter original = Console.Out;
            Console.SetOut(writer);
            try
            {
                exam.ShowExam();
            }
            finally
            {
                Console.SetOut(original);
            }

            string output = writer.ToString();
            Assert.Contains("Right Answer:", output);
            Assert.DoesNotContain("Grade:", output);
        }

        [Fact]
        public void ToString_StartsWithPractical()
        {
            var exam = new PracticalExam(60, 2);

            Assert.StartsWith("Practical", exam.ToString());
        }
    }
}
