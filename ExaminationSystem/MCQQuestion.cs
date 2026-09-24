namespace ExaminationSystem
{
    /// <summary>
    /// A multiple-choice question with up to four answer slots (when built empty),
    /// or a caller-supplied set of choices. Usable in both <see cref="FinalExam"/>
    /// and <see cref="PracticalExam"/>.
    /// </summary>
    public class MCQQuestion : Question
    {
        /// <summary>
        /// Creates an MCQ question with four empty answer slots, to be filled in
        /// later via <see cref="Question.AddAnswer"/> and <see cref="Question.SetRightAnswer"/>.
        /// </summary>
        public MCQQuestion(string header, string body, int mark) : base(header, body, mark)
        {
            Answers = new Answer?[4];
        }

        /// <summary>
        /// Creates an MCQ question with its answer choices and correct answer supplied up front.
        /// </summary>
        /// <param name="answersList">The full set of answer choices.</param>
        /// <param name="rightAnswer">Must be one of the answers in <paramref name="answersList"/>.</param>
        public MCQQuestion(string header, string body, int mark, Answer[] answersList, Answer rightAnswer)
            : base(header, body, mark, answersList, rightAnswer)
        {
        }
    }
}