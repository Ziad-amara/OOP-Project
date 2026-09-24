namespace ExaminationSystem
{
    /// <summary>
    /// A question with exactly two fixed choices, "True" and "False".
    /// Usable only in <see cref="FinalExam"/>.
    /// </summary>
    public class TrueFalseQuestion : Question
    {
        /// <summary>
        /// Creates a True/False question and populates its two answers automatically.
        /// </summary>
        /// <param name="correctIsTrue">
        /// <c>true</c> if "True" is the correct answer; <c>false</c> if "False" is.
        /// </param>
        public TrueFalseQuestion(string header, string body, int mark, bool correctIsTrue)
            : base(header, body, mark)
        {
            Answers = new Answer?[2];

            Answer trueAnswer = new Answer(1, "True");
            Answer falseAnswer = new Answer(2, "False");

            AddAnswer(trueAnswer);
            AddAnswer(falseAnswer);

            SetRightAnswer(correctIsTrue ? trueAnswer : falseAnswer);
        }
    }
}
