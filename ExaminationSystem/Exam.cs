namespace ExaminationSystem
{
    /// <summary>
    /// Base class for every exam type. Holds the exam's duration, its fixed-size
    /// array of questions, and the student's recorded answers. Cannot be instantiated
    /// directly — concrete exam types (<see cref="FinalExam"/>, <see cref="PracticalExam"/>)
    /// derive from it and supply their own <see cref="ShowExam"/> behavior.
    /// </summary>
    public abstract class Exam : ICloneable, IComparable
    {
        protected int _time;
        protected int _numberOfQuestions;
        protected Question?[] _questions = Array.Empty<Question?>();
        protected int[] _studentAnswers = Array.Empty<int>();

        /// <summary>Duration of the exam, in minutes. Must be greater than zero.</summary>
        public int Time
        {
            get
            {
                return _time;
            }

            set
            {
                if (value > 0)
                    _time = value;
                else
                    throw new ArgumentOutOfRangeException(nameof(value), "Time must be greater than zero.");
            }
        }

        /// <summary>
        /// The fixed number of questions this exam holds (and the size of <see cref="Questions"/>
        /// and <see cref="StudentAnswers"/>). Must be greater than zero.
        /// </summary>
        public int NumberOfQuestions
        {
            get
            {
                return _numberOfQuestions;
            }

            set
            {
                if (value > 0)
                    _numberOfQuestions = value;
                else
                    throw new ArgumentOutOfRangeException(nameof(value), "Number of questions must be greater than zero.");
            }
        }

        /// <summary>The exam's questions. Empty slots are <c>null</c> until filled via <see cref="AddQuestion"/>.</summary>
        public Question?[] Questions
        {
            get
            {
                return _questions;
            }

            protected set
            {
                _questions = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// The student's chosen answer ID for each question, by index (parallel to <see cref="Questions"/>).
        /// <c>0</c> means that question hasn't been answered yet.
        /// </summary>
        public int[] StudentAnswers
        {
            get
            {
                return _studentAnswers;
            }

            protected set
            {
                _studentAnswers = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Creates an exam with the given duration and question count, allocating
        /// empty <see cref="Questions"/> and <see cref="StudentAnswers"/> arrays.
        /// </summary>
        public Exam(int time, int numberOfQuestions)
        {
            Time = time;
            NumberOfQuestions = numberOfQuestions;

            _questions = new Question?[NumberOfQuestions];
            _studentAnswers = new int[NumberOfQuestions];
        }

        /// <summary>
        /// Adds a <b>deep clone</b> of the given question into the first free slot.
        /// Cloning keeps this exam's copy independent of the original question object
        /// (and of any other exam that was built from the same question bank).
        /// </summary>
        /// <exception cref="InvalidOperationException">The questions array is already full.</exception>
        public virtual void AddQuestion(Question question)
        {
            ArgumentNullException.ThrowIfNull(question);

            for (int i = 0; i < _questions.Length; i++)
            {
                if (_questions[i] is null)
                {
                    _questions[i] = (Question)question.Clone();
                    return;
                }
            }

            throw new InvalidOperationException("The questions array is full.");
        }

        /// <summary>
        /// Records the student's chosen answer for the question at <paramref name="questionIndex"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The index or answer ID is invalid.</exception>
        /// <exception cref="InvalidOperationException">There is no question at that index.</exception>
        /// <exception cref="ArgumentException">The answer ID doesn't belong to that question.</exception>
        public void SetStudentAnswer(int questionIndex, int answerId)
        {
            if (answerId <= 0)
                throw new ArgumentOutOfRangeException(nameof(answerId), "Answer ID must be greater than zero.");

            if (questionIndex < 0 || questionIndex >= _questions.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(questionIndex), "Question index is out of range");
            }

            Question? question = _questions[questionIndex];

            if (question is null)
                throw new InvalidOperationException("There is no question at this specified index.");

            bool answerExists = question.Answers.Any(a => a is not null && a.AnswerId == answerId);
            if (!answerExists)
                throw new ArgumentException("The specified answer does not belong to this question.", nameof(answerId));

            StudentAnswers[questionIndex] = answerId;
        }

        /// <summary>
        /// Displays the exam. Behavior differs per exam type — see the overriding
        /// implementations in <see cref="FinalExam"/> and <see cref="PracticalExam"/>.
        /// </summary>
        public abstract void ShowExam();

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Exam - Time: {Time} minutes, Questions: {NumberOfQuestions}";
        }

        /// <summary>
        /// Creates a deep copy of this exam: the questions array (and each question
        /// inside it) and the student answers array are cloned independently.
        /// </summary>
        public object Clone()
        {
            Exam clone = (Exam)MemberwiseClone();

            clone._questions = new Question?[_questions.Length];

            for (int i = 0; i < _questions.Length; i++)
            {
                if (_questions[i] is not null)
                {
                    clone._questions[i] = (Question)_questions[i]!.Clone();
                }
            }

            clone._studentAnswers = (int[])_studentAnswers.Clone();

            return clone;
        }

        /// <summary>
        /// Compares exams by <see cref="NumberOfQuestions"/>.
        /// </summary>
        public int CompareTo(object? obj)
        {
            if (obj is null)
                return 1;

            if (obj is not Exam other)
                throw new ArgumentException("Object is not an Exam.", nameof(obj));

            return NumberOfQuestions.CompareTo(other.NumberOfQuestions);
        }
    }
}