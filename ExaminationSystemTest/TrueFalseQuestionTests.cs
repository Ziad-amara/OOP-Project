using ExaminationSystem;

namespace ExaminationSystemTest
{
    public class TrueFalseQuestionTests
    {
        [Fact]
        public void Constructor_CreatesExactlyTwoAnswers_TrueAndFalse()
        {
            var q = new TrueFalseQuestion("Q1", "The sky is blue.", 5, true);

            Assert.Equal(2, q.Answers.Length);
            Assert.Contains(q.Answers, a => a is not null && a.AnswerText == "True");
            Assert.Contains(q.Answers, a => a is not null && a.AnswerText == "False");
        }

        [Fact]
        public void Constructor_CorrectIsTrue_SetsTrueAsRightAnswer()
        {
            var q = new TrueFalseQuestion("Q1", "body", 5, correctIsTrue: true);

            Assert.Equal("True", q.RightAnswer!.AnswerText);
        }

        [Fact]
        public void Constructor_CorrectIsFalse_SetsFalseAsRightAnswer()
        {
            var q = new TrueFalseQuestion("Q1", "body", 5, correctIsTrue: false);

            Assert.Equal("False", q.RightAnswer!.AnswerText);
        }

        [Fact]
        public void IsCorrect_MatchesTheConfiguredRightAnswer()
        {
            var q = new TrueFalseQuestion("Q1", "body", 5, correctIsTrue: true);
            int trueAnswerId = q.Answers[0]!.AnswerId;
            int falseAnswerId = q.Answers[1]!.AnswerId;

            Assert.True(q.IsCorrect(trueAnswerId));
            Assert.False(q.IsCorrect(falseAnswerId));
        }
    }
}
