using ExaminationSystem;

namespace ExaminationSystemTest
{
    public class MCQQuestionTests
    {
        [Fact]
        public void ThreeArgConstructor_CreatesFourEmptySlots()
        {
            var q = new MCQQuestion("Q1", "body", 5);

            Assert.Equal(4, q.Answers.Length);
            Assert.All(q.Answers, a => Assert.Null(a));
        }

        [Fact]
        public void FiveArgConstructor_SetsSuppliedAnswersAndRightAnswer()
        {
            var a1 = new Answer(1, "Stack");
            var a2 = new Answer(2, "Queue");

            var q = new MCQQuestion("Q1", "body", 5, new[] { a1, a2 }, a1);

            Assert.Equal(2, q.Answers.Length);
            Assert.Equal(1, q.RightAnswer!.AnswerId);
        }

        [Fact]
        public void ThreeArgConstructor_AllowsAddingUpToFourAnswers()
        {
            var q = new MCQQuestion("Q1", "body", 5);

            q.AddAnswer(new Answer(1, "a"));
            q.AddAnswer(new Answer(2, "b"));
            q.AddAnswer(new Answer(3, "c"));
            q.AddAnswer(new Answer(4, "d"));

            Assert.DoesNotContain(q.Answers, a => a is null);
        }
    }
}
