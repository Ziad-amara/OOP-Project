using ExaminationSystem;

namespace ExaminationSystemTest
{
    public class AnswerTests
    {
        [Fact]
        public void Constructor_ValidValues_SetsProperties()
        {
            var answer = new Answer(1, "Stack");

            Assert.Equal(1, answer.AnswerId);
            Assert.Equal("Stack", answer.AnswerText);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void Constructor_NonPositiveId_Throws(int invalidId)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Answer(invalidId, "text"));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Constructor_BlankText_Throws(string? invalidText)
        {
            Assert.Throws<ArgumentException>(() => new Answer(1, invalidText!));
        }

        [Fact]
        public void ToString_ContainsIdAndText()
        {
            var answer = new Answer(3, "Queue");

            string result = answer.ToString();

            Assert.Contains("3", result);
            Assert.Contains("Queue", result);
        }

        [Fact]
        public void Clone_ReturnsEqualButDistinctInstance()
        {
            var original = new Answer(1, "Stack");

            var clone = (Answer)original.Clone();

            Assert.NotSame(original, clone);
            Assert.Equal(original.AnswerId, clone.AnswerId);
            Assert.Equal(original.AnswerText, clone.AnswerText);
        }

        [Fact]
        public void Clone_MutatingCloneDoesNotAffectOriginal()
        {
            var original = new Answer(1, "Stack");
            var clone = (Answer)original.Clone();

            clone.AnswerText = "Changed";

            Assert.Equal("Stack", original.AnswerText);
        }
    }
}
