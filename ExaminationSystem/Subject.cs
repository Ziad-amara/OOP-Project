namespace ExaminationSystem
{
    /// <summary>
    /// Represents a subject of study, which can create and hold a single exam
    /// (either <see cref="FinalExam"/> or <see cref="PracticalExam"/>) via <see cref="CreateExam"/>.
    /// </summary>
    public class Subject
    {
        private int _subjectId;
        private string _subjectName = string.Empty;
        private Exam? _exam;

        /// <summary>Unique identifier for the subject. Must be greater than zero.</summary>
        public int SubjectId
        {
            get
            {
                return _subjectId;
            }

            set
            {
                if (value > 0)
                    _subjectId = value;
                else
                    throw new ArgumentOutOfRangeException(nameof(value), "Subject Id must be greater than zero.");
            }
        }

        /// <summary>The subject's display name. Cannot be blank.</summary>
        public string SubjectName
        {
            get
            {
                return _subjectName;
            }

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _subjectName = value;
                else
                    throw new ArgumentException("Subject name cannot be null, empty, or whitespace.", nameof(value));
            }
        }

        /// <summary>
        /// The exam created for this subject, or <c>null</c> if <see cref="CreateExam"/>
        /// hasn't been called yet. Only settable internally, via <see cref="CreateExam"/>.
        /// </summary>
        public Exam? Exam
        {
            get
            {
                return _exam;
            }
            private set
            {
                _exam = value;
            }
        }

        /// <summary>
        /// Creates a subject with the given ID and name. No exam is created yet.
        /// </summary>
        public Subject(int id, string name)
        {
            SubjectId = id;
            SubjectName = name;
        }

        /// <summary>
        /// Creates the subject's exam — a <see cref="FinalExam"/> or <see cref="PracticalExam"/>
        /// depending on <paramref name="examType"/> — assigns it to <see cref="Exam"/>, and returns it.
        /// </summary>
        /// <param name="examType">"Final" or "Practical" (case-insensitive).</param>
        /// <exception cref="ArgumentException"><paramref name="examType"/> is neither "Final" nor "Practical".</exception>
        public Exam CreateExam(string examType, int time, int numberOfQuestions)
        {
            ArgumentNullException.ThrowIfNull(examType);

            if (examType.Equals("Final", StringComparison.OrdinalIgnoreCase))
                Exam = new FinalExam(time, numberOfQuestions);
            else if (examType.Equals("Practical", StringComparison.OrdinalIgnoreCase))
                Exam = new PracticalExam(time, numberOfQuestions);
            else
                throw new ArgumentException("Exam type must be either Final or Practical.", nameof(examType));

            return Exam;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Subject ID: {SubjectId}, Subject Name: {SubjectName}, Exam: {Exam}";
        }
    }
}