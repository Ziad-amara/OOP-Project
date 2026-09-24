namespace ExaminationSystem
{
    /// <summary>
    /// An exam that accepts <see cref="MCQQuestion"/> and <see cref="TrueFalseQuestion"/>.
    /// Showing it prints every question along with the student's grade.
    /// </summary>
    public class FinalExam : Exam
    {
        /// <inheritdoc/>
        public FinalExam(int time, int numberOfQuestions) : base(time, numberOfQuestions)
        {
        }

        /// <summary>
        /// Prints every question in the exam, followed by the student's <see cref="CalculateGrade"/> result.
        /// </summary>
        public override void ShowExam()
        {
            for (int i = 0; i < _questions.Length; i++)
            {
                if (_questions[i] is not null)
                {
                    Console.WriteLine(_questions[i]);
                }
            }

            Console.WriteLine($"Grade: {CalculateGrade()}");
        }

        /// <summary>
        /// Sums the <see cref="Question.Mark"/> of every question the student answered correctly.
        /// Unanswered questions (<see cref="Exam.StudentAnswers"/> value of 0) are skipped.
        /// </summary>
        public int CalculateGrade()
        {
            int grade = 0;

            for (int i = 0; i < _questions.Length; i++)
            {
                if (_questions[i] is Question question)
                {
                    if (StudentAnswers[i] != 0)
                    {
                        if (question.IsCorrect(StudentAnswers[i]))
                            grade += question.Mark;
                    }
                }
            }

            return grade;
        }

        /// <summary>
        /// Adds a question after confirming it's an <see cref="MCQQuestion"/> or <see cref="TrueFalseQuestion"/>.
        /// </summary>
        /// <exception cref="ArgumentException">The question is of an unsupported type.</exception>
        public override void AddQuestion(Question question)
        {
            ArgumentNullException.ThrowIfNull(question);

            if (question is not MCQQuestion && question is not TrueFalseQuestion)
            {
                throw new ArgumentException("Final Exam accepts only MCQQuestion or TrueFalseQuestion.", nameof(question));
            }

            base.AddQuestion(question);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Final {base.ToString()}";
        }
    }
}