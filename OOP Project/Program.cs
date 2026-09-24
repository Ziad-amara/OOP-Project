namespace ExaminationSystem
{
    /// <summary>
    /// Console entry point that interactively drives the Examination System:
    /// creates a subject and its exam, collects questions, records student
    /// answers, then shows the finished exam — using only the public API of
    /// <see cref="Subject"/>, <see cref="Exam"/> and its subclasses, and
    /// the various <see cref="Question"/> types.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Examination System ===\n");

            Subject subject = CreateSubject();
            Exam exam = CreateExamForSubject(subject);

            int numberOfQuestions = exam.NumberOfQuestions;
            for (int i = 0; i < numberOfQuestions; i++)
            {
                Console.Clear();
                Console.WriteLine($"-- Question {i + 1} of {numberOfQuestions} --\n");
                AddOneQuestion(exam);
            }

            Console.Clear();
            Console.WriteLine("-- Record Student Answers --\n");
            for (int i = 0; i < numberOfQuestions; i++)
            {
                RecordStudentAnswer(exam, i);
            }

            Console.WriteLine();
            Console.WriteLine(subject);
            Console.WriteLine();
            exam.ShowExam();
        }

        // ---------- Steps (each retries until it succeeds) ----------

        /// <summary>Prompts for and creates a <see cref="Subject"/>, re-asking on invalid input.</summary>
        private static Subject CreateSubject()
        {
            while (true)
            {
                try
                {
                    int id = ReadInt("Subject ID: ");
                    string name = ReadNonEmptyString("Subject Name: ");
                    return new Subject(id, name); // throws if id <= 0 or name blank
                }
                catch (Exception ex) when (ex is ArgumentException or ArgumentOutOfRangeException)
                {
                    Console.WriteLine($"Invalid input: {ex.Message}\nTry again.\n");
                }
            }
        }

        /// <summary>
        /// Prompts for the exam type, time, and question count, and calls
        /// <see cref="Subject.CreateExam"/>. The exam type is validated in its
        /// own loop, separately from time/question count.
        /// </summary>
        private static Exam CreateExamForSubject(Subject subject)
        {
            string examType = ReadExamType();
            int time = ReadInt("Exam time (minutes): ", min: 1);
            int numberOfQuestions = ReadInt("Number of questions: ", min: 1);

            return subject.CreateExam(examType, time, numberOfQuestions);
        }

        /// <summary>
        /// Loops until the user enters "Final" or "Practical" (case-insensitive,
        /// surrounding whitespace ignored), returning the canonical value.
        /// </summary>
        private static string ReadExamType()
        {
            while (true)
            {
                Console.Write("\nExam type (Final/Practical): ");
                string input = (Console.ReadLine() ?? string.Empty).Trim();

                if (input.Equals("Final", StringComparison.OrdinalIgnoreCase))
                    return "Final";
                if (input.Equals("Practical", StringComparison.OrdinalIgnoreCase))
                    return "Practical";

                Console.WriteLine($"'{input}' is not valid — enter 'Final' or 'Practical'.");
            }
        }

        /// <summary>Prompts for one question's details and adds it to <paramref name="exam"/>, retrying on validation failure.</summary>
        private static void AddOneQuestion(Exam exam)
        {
            while (true)
            {
                try
                {
                    string header = ReadNonEmptyString("Header: ");
                    string body = ReadNonEmptyString("Body: ");
                    int mark = ReadInt("Mark: ", min: 1);

                    Question question = exam is PracticalExam
                        ? BuildMcq(header, body, mark) // PracticalExam only accepts MCQQuestion
                        : BuildFinalQuestion(header, body, mark);

                    exam.AddQuestion(question); // throws if the exam rejects this question type or is full
                    return;
                }
                catch (Exception ex) when (ex is ArgumentException or ArgumentOutOfRangeException or InvalidOperationException)
                {
                    Console.WriteLine($"Invalid input: {ex.Message}\nLet's redo this question.\n");
                }
            }
        }

        /// <summary>Asks whether the question is True/False or MCQ, and builds it accordingly.</summary>
        private static Question BuildFinalQuestion(string header, string body, int mark)
        {
            while (true)
            {
                Console.Write("Type (TF/MCQ): ");
                string type = (Console.ReadLine() ?? string.Empty).Trim();

                if (type.Equals("TF", StringComparison.OrdinalIgnoreCase))
                {
                    bool correctIsTrue = ReadYesNo("Is 'True' correct? (y/n): ");
                    return new TrueFalseQuestion(header, body, mark, correctIsTrue);
                }

                if (type.Equals("MCQ", StringComparison.OrdinalIgnoreCase))
                {
                    return BuildMcq(header, body, mark);
                }

                Console.WriteLine("Please enter 'TF' or 'MCQ'.\n");
            }
        }

        /// <summary>
        /// Builds an <see cref="MCQQuestion"/> from console input. Answer IDs are
        /// auto-generated (1, 2, 3, ...) — only each answer's text is asked for.
        /// </summary>
        private static MCQQuestion BuildMcq(string header, string body, int mark)
        {
            int count = ReadInt("How many answer choices?: ", min: 2);

            var answers = new List<Answer>();
            for (int i = 0; i < count; i++)
            {
                string text = ReadNonEmptyString($"  Answer {i + 1} text: ");
                answers.Add(new Answer(i + 1, text));
            }

            Console.WriteLine();
            foreach (Answer a in answers)
                Console.WriteLine($"  {a.AnswerId}) {a.AnswerText}");

            while (true)
            {
                int rightId = ReadInt("Correct answer's number: ");
                Answer? rightAnswer = answers.FirstOrDefault(a => a.AnswerId == rightId);

                if (rightAnswer is not null)
                    return new MCQQuestion(header, body, mark, answers.ToArray(), rightAnswer);

                Console.WriteLine($"There's no answer numbered {rightId}. Pick one of 1-{answers.Count}.\n");
            }
        }

        /// <summary>Prompts for and records the student's answer to one question, retrying on validation failure.</summary>
        private static void RecordStudentAnswer(Exam exam, int questionIndex)
        {
            while (true)
            {
                try
                {
                    int answerId = ReadInt($"Question {questionIndex} - chosen answer ID: ");
                    exam.SetStudentAnswer(questionIndex, answerId); // throws if the ID doesn't belong to this question
                    return;
                }
                catch (Exception ex) when (ex is ArgumentException or ArgumentOutOfRangeException or InvalidOperationException)
                {
                    Console.WriteLine($"Invalid input: {ex.Message}\nTry again.\n");
                }
            }
        }

        // ---------- Input helpers: retry until the console text itself is well-formed ----------
        // (Domain rules enforced by the model's own setters/constructors are still the
        // authority — these only guard against non-numeric or empty text so int.Parse
        // never crashes, and optionally enforce a minimum via `min`.)

        /// <summary>Reads an integer, re-prompting until the text parses and (if given) meets <paramref name="min"/>.</summary>
        private static int ReadInt(string prompt, int? min = null)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int result) && (min is null || result >= min))
                    return result;

                string requirement = min is null ? "a whole number" : $"a whole number of at least {min}";
                Console.WriteLine($"'{input}' is not valid — enter {requirement}.");
            }
        }

        /// <summary>Reads a line of text, re-prompting until it's non-blank.</summary>
        private static string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                    return input;

                Console.WriteLine("This can't be empty.");
            }
        }

        /// <summary>Reads a yes/no answer, re-prompting until it's 'y' or 'n'.</summary>
        private static bool ReadYesNo(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = (Console.ReadLine() ?? string.Empty).Trim();

                if (input.Equals("y", StringComparison.OrdinalIgnoreCase)) return true;
                if (input.Equals("n", StringComparison.OrdinalIgnoreCase)) return false;

                Console.WriteLine("Please enter 'y' or 'n'.");
            }
        }
    }
}