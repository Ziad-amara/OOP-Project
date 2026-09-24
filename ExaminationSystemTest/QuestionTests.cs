using ExaminationSystem;

namespace ExaminationSystemTest
{
    public class QuestionTests
    {
        private static MCQQuestion CreateMcq()
        {
            var a1 = new Answer(1, "Stack");
            var a2 = new Answer(2, "Queue");
            var a3 = new Answer(3, "Array");
            return new MCQQuestion("Q1", "Which is LIFO?", 5, new[] { a1, a2, a3 }, a1);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Header_Blank_Throws(string? invalidHeader)
        {
            Assert.Throws<ArgumentException>(() => new MCQQuestion(invalidHeader!, "body", 5));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Body_Blank_Throws(string? invalidBody)
        {
            Assert.Throws<ArgumentException>(() => new MCQQuestion("header", invalidBody!, 5));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Mark_NonPositive_Throws(int invalidMark)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new MCQQuestion("h", "b", invalidMark));
        }

        [Fact]
        public void FiveArgConstructor_ChainsAndSetsAnswersAndRightAnswer()
        {
            MCQQuestion mcq = CreateMcq();

            Assert.Equal("Q1", mcq.Header);
            Assert.Equal(3, mcq.Answers.Length);
            Assert.NotNull(mcq.RightAnswer);
            Assert.Equal(1, mcq.RightAnswer!.AnswerId);
        }

        [Fact]
        public void FiveArgConstructor_RightAnswerNotInList_Throws()
        {
            var a1 = new Answer(1, "Stack");
            var strangerAnswer = new Answer(99, "Not in list");

            Assert.Throws<ArgumentException>(() =>
                new MCQQuestion("h", "b", 5, new[] { a1 }, strangerAnswer));
        }

        [Fact]
        public void AddAnswer_NullAnswer_Throws()
        {
            var q = new MCQQuestion("h", "b", 5);

            Assert.Throws<ArgumentNullException>(() => q.AddAnswer(null!));
        }

        [Fact]
        public void AddAnswer_WhenFull_Throws()
        {
            var q = new MCQQuestion("h", "b", 5); // 4 slots

            q.AddAnswer(new Answer(1, "a"));
            q.AddAnswer(new Answer(2, "b"));
            q.AddAnswer(new Answer(3, "c"));
            q.AddAnswer(new Answer(4, "d"));

            Assert.Throws<InvalidOperationException>(() => q.AddAnswer(new Answer(5, "e")));
        }

        [Fact]
        public void RemoveAnswer_NullAnswer_Throws()
        {
            var q = new MCQQuestion("h", "b", 5);

            Assert.Throws<ArgumentNullException>(() => q.RemoveAnswer(null!));
        }

        [Fact]
        public void RemoveAnswer_ExistingAnswer_RemovesIt()
        {
            var q = new MCQQuestion("h", "b", 5);
            var a = new Answer(1, "Stack");
            q.AddAnswer(a);

            q.RemoveAnswer(a);

            Assert.DoesNotContain(q.Answers, x => x is not null && x.AnswerId == 1);
        }

        [Fact]
        public void RemoveAnswer_RightAnswer_ClearsRightAnswer()
        {
            var a1 = new Answer(1, "Stack");
            var q = new MCQQuestion("h", "b", 5, new[] { a1 }, a1);

            q.RemoveAnswer(a1);

            Assert.Null(q.RightAnswer);
        }

        [Fact]
        public void SetRightAnswer_AnswerNotInList_Throws()
        {
            var q = new MCQQuestion("h", "b", 5);
            q.AddAnswer(new Answer(1, "a"));

            Assert.Throws<ArgumentException>(() => q.SetRightAnswer(new Answer(99, "not added")));
        }

        [Fact]
        public void SetRightAnswer_ValidAnswer_UpdatesRightAnswer()
        {
            var q = new MCQQuestion("h", "b", 5);
            var a = new Answer(1, "a");
            q.AddAnswer(a);

            q.SetRightAnswer(a);

            Assert.Equal(1, q.RightAnswer!.AnswerId);
        }

        [Fact]
        public void IsCorrect_MatchingId_ReturnsTrue()
        {
            MCQQuestion mcq = CreateMcq();

            Assert.True(mcq.IsCorrect(1));
        }

        [Fact]
        public void IsCorrect_NonMatchingId_ReturnsFalse()
        {
            MCQQuestion mcq = CreateMcq();

            Assert.False(mcq.IsCorrect(2));
        }

        [Fact]
        public void IsCorrect_NoRightAnswerSet_ReturnsFalse()
        {
            var q = new MCQQuestion("h", "b", 5);

            Assert.False(q.IsCorrect(1));
        }

        [Fact]
        public void Clone_ProducesDeepCopy_IndependentAnswers()
        {
            MCQQuestion original = CreateMcq();

            var clone = (MCQQuestion)original.Clone();
            clone.Answers[0]!.AnswerText = "Changed";

            Assert.NotSame(original, clone);
            Assert.NotSame(original.Answers[0], clone.Answers[0]);
            Assert.Equal("Stack", original.Answers[0]!.AnswerText);
        }

        [Fact]
        public void Clone_PreservesRightAnswerAsCloneInstance()
        {
            MCQQuestion original = CreateMcq();

            var clone = (MCQQuestion)original.Clone();

            Assert.NotNull(clone.RightAnswer);
            Assert.Equal(original.RightAnswer!.AnswerId, clone.RightAnswer!.AnswerId);
            Assert.NotSame(original.RightAnswer, clone.RightAnswer);
            // The clone's RightAnswer should be the SAME object as the matching entry
            // in the clone's own Answers array, not a separately cloned duplicate.
            Assert.Same(clone.RightAnswer, clone.Answers[0]);
        }

        [Fact]
        public void CompareTo_ComparesByMark()
        {
            var lowMark = new MCQQuestion("h", "b", 5);
            var highMark = new MCQQuestion("h", "b", 10);

            Assert.True(lowMark.CompareTo(highMark) < 0);
            Assert.True(highMark.CompareTo(lowMark) > 0);
            Assert.Equal(0, lowMark.CompareTo(new MCQQuestion("h2", "b2", 5)));
        }

        [Fact]
        public void CompareTo_Null_ReturnsPositive()
        {
            var q = new MCQQuestion("h", "b", 5);

            Assert.True(q.CompareTo(null) > 0);
        }

        [Fact]
        public void CompareTo_NonQuestionObject_Throws()
        {
            var q = new MCQQuestion("h", "b", 5);

            Assert.Throws<ArgumentException>(() => q.CompareTo("not a question"));
        }

        [Fact]
        public void ToString_IncludesHeaderAndBody()
        {
            MCQQuestion mcq = CreateMcq();

            string result = mcq.ToString();

            Assert.Contains("Q1", result);
            Assert.Contains("Which is LIFO?", result);
        }
    }
}
