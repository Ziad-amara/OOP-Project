namespace ExaminationSystem
{
    /// <summary>
    /// An exam that accepts only <see cref="MCQQuestion"/>. Showing it prints every
    /// question along with the right answer — no grade is calculated.
    /// </summary>
    public class PracticalExam : Exam
    {
        /// <inheritdoc/>
        public PracticalExam(int time, int numberOfQuestions) : base(time, numberOfQuestions)
        {
        }

        /// <summary>
        /// Adds a question after confirming it's an <see cref="MCQQuestion"/>.
        /// </summary>
        /// <exception cref="ArgumentException">The question is not an MCQQuestion.</exception>
        public override void AddQuestion(Question question)
        {
            ArgumentNullException.ThrowIfNull(question);

            if (question is not MCQQuestion)
            {
                throw new ArgumentException("Practical Exam accepts only MCQQuestion.", nameof(question));
            }

            base.AddQuestion(question);
        }

        /// <summary>
        /// Prints every question in the exam along with its <see cref="Question.RightAnswer"/>.
        /// </summary>
        public override void ShowExam()
        {
            for (int i = 0; i < _questions.Length; i++)
            {
                if (_questions[i] is Question question)
                {
                    Console.WriteLine(question);

                    if (question.RightAnswer is not null)
                    {
                        Console.WriteLine($"Right Answer: {question.RightAnswer}");
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Practical {base.ToString()}";
        }
    }
}