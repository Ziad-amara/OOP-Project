using ExaminationSystem;

namespace ExaminationSystemTest
{
    public class FinalExamTests
    {
        private static MCQQuestion CreateMcq(int mark = 5)
        {
            var a1 = new Answer(1, "Stack");
            var a2 = new Answer(2, "Queue");
            return new MCQQuestion("Q1", "Which is LIFO?", mark, new[] { a1, a2 }, a1);
        }

        private static TrueFalseQuestion CreateTf(int mark = 5, bool correctIsTrue = true)
        {
            return new TrueFalseQuestion("Q2", "A tree is linear.", mark, correctIsTrue);
        }

        [Fact]
        public void AddQuestion_AcceptsMcqAndTrueFalse()
        {
            var exam = new FinalExam(60, 2);

            exam.AddQuestion(CreateMcq());
            exam.AddQuestion(CreateTf());

            Assert.NotNull(exam.Questions[0]);
            Assert.NotNull(exam.Questions[1]);
        }

        [Fact]
        public void AddQuestion_NonMcqNonTfType_Throws()
        {
            // A locally defined Question subclass simulates an unsupported type.
            var exam = new FinalExam(60, 1);
            var unsupported = new UnsupportedQuestion("h", "b", 5);

            Assert.Throws<ArgumentException>(() => exam.AddQuestion(unsupported));
        }

        [Fact]
        public void CalculateGrade_SumsMarksOfCorrectlyAnsweredQuestions()
        {
            var exam = new FinalExam(60, 2);
            exam.AddQuestion(CreateMcq(mark: 5));   // right answer id 1
            exam.AddQuestion(CreateTf(mark: 3, correctIsTrue: true));

            int trueId = exam.Questions[1]!.Answers[0]!.AnswerId;

            exam.SetStudentAnswer(0, 1);     // correct -> +5
            exam.SetStudentAnswer(1, trueId); // correct -> +3

            Assert.Equal(8, exam.CalculateGrade());
        }

        [Fact]
        public void CalculateGrade_WrongAnswer_NotCounted()
        {
            var exam = new FinalExam(60, 1);
            exam.AddQuestion(CreateMcq(mark: 5)); // right id 1, wrong id 2

            exam.SetStudentAnswer(0, 2);

            Assert.Equal(0, exam.CalculateGrade());
        }

        [Fact]
        public void CalculateGrade_UnansweredQuestion_NotCounted()
        {
            var exam = new FinalExam(60, 1);
            exam.AddQuestion(CreateMcq(mark: 5));
            // no SetStudentAnswer call -> StudentAnswers[0] stays 0

            Assert.Equal(0, exam.CalculateGrade());
        }

        [Fact]
        public void ShowExam_PrintsGradeLine()
        {
            var exam = new FinalExam(60, 1);
            exam.AddQuestion(CreateMcq(mark: 5));
            exam.SetStudentAnswer(0, 1);

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

            Assert.Contains("Grade: 5", writer.ToString());
        }

        [Fact]
        public void ToString_StartsWithFinal()
        {
            var exam = new FinalExam(60, 2);

            Assert.StartsWith("Final", exam.ToString());
        }

        // A minimal Question subclass used only to test FinalExam's type guard.
        private sealed class UnsupportedQuestion : Question
        {
            public UnsupportedQuestion(string header, string body, int mark) : base(header, body, mark)
            {
                Answers = new Answer?[1];
            }
        }
    }
}
