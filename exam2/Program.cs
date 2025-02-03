using System;
using System.Collections.Generic;

namespace ExaminationSystem
{
    // Base Question Class
    public abstract class Question : ICloneable, IComparable<Question>
    {
        public string Header { get; set; }
        public string Body { get; set; }
        public int Mark { get; set; }
        public List<Answer> AnswerList { get; set; } = new List<Answer>();
        public string UserAnswer { get; set; } //This To store the user answer
        public string RightAnswer { get; set; } //This To store the right answer

        public Question(string header, string body, int mark)
        {
            Header = header;
            Body = body;
            Mark = mark;
        }

        public abstract void DisplayQuestion();

        public void GetUserAnswer()
        {
            Console.Write("Your answer: ");
            UserAnswer = Console.ReadLine();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public int CompareTo(Question other)
        {
            return Mark.CompareTo(other.Mark);
        }

        public override string ToString()
        {
            return $"Header: {Header}, Body: {Body}, Mark: {Mark}";
        }
    }

    // True/False Question Class
    public class TrueFalseQuestion : Question
    {
        public TrueFalseQuestion(string header, string body, int mark) : base(header, body, mark) { }
        public override void DisplayQuestion()
        {
            Console.WriteLine($"{Header}: {Body} (True/False)");
        }
    }

    // MCq Question Claas
    public class MCQQuestion : Question
    {
        public MCQQuestion(string header, string body, int mark) : base(header, body, mark) { }
        public override void DisplayQuestion()
        {
            Console.WriteLine($"{Header}: {Body}");
            for (int i = 0; i < AnswerList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {AnswerList[i].AnswerText}");
            }
        }
    }

    // Answer Class
    public class Answer
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
        public Answer(int answerId, string answerText)
        {
            AnswerId = answerId;
            AnswerText = answerText;
        }
        public override string ToString()
        {
            return $"Answer ID: {AnswerId}, Text: {AnswerText}";
        }
    }

    // Base Exam Class
    public abstract class Exam
    {
        public int Time { get; set; }
        public int NumberOfQuestions { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
        public Exam(int time, int numberOfQuestions)
        {
            Time = time;
            NumberOfQuestions = numberOfQuestions;
        }
        public abstract void ShowExam();
        public void TakeExam()
        {
            Console.WriteLine($"Starting the exam... You have {Time} minutes.");
            int totalMarks = 0;
            int userMarks = 0;
            foreach (var question in Questions)
            {
                question.DisplayQuestion();
                question.GetUserAnswer();

                if (question.UserAnswer == question.RightAnswer)
                {
                    userMarks += question.Mark;
                }
                totalMarks += question.Mark;
            }

            Console.WriteLine("Exam finished. Here are your answers:");
            ShowExam();
            Console.WriteLine($"Your exam grade is {userMarks} from {totalMarks}");
        }
    }

    // Final Exam Class
    public class FinalExam : Exam
    {
        public FinalExam(int time, int numberOfQuestions) : base(time, numberOfQuestions) { }
        public override void ShowExam()
        {
            Console.WriteLine("Final Exam Results:");
            foreach (var question in Questions)
            {
                Console.WriteLine($"{question.Header}: {question.Body}");
                Console.WriteLine($"Your answer: {question.UserAnswer}");
                Console.WriteLine($"Right answer: {question.RightAnswer}");
            }
        }
    }

    // Practical Exam Class
    public class PracticalExam : Exam
    {
        public PracticalExam(int time, int numberOfQuestions) : base(time, numberOfQuestions) { }
        public override void ShowExam()
        {
            Console.WriteLine("Practical Exam Results:");
            foreach (var question in Questions)
            {
                Console.WriteLine($"{question.Header}: {question.Body}");
                Console.WriteLine($"Your answer: {question.UserAnswer}");
                Console.WriteLine("Correct answers:");
                foreach (var answer in question.AnswerList)
                {
                    Console.WriteLine(answer);
                }
            }
        }
    }

    // Subject Class
    public class Subject
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public Exam Exam { get; set; }
        public Subject(int subjectId, string subjectName)
        {
            SubjectId = subjectId;
            SubjectName = subjectName;
        }
        public void CreateExam(Exam exam)
        {
            Exam = exam;
        }
        public override string ToString()
        {
            return $"Subject ID: {SubjectId}, Name: {SubjectName}";
        }
    }

    // Main Program
    class Program
    {
        static void Main(string[] args)
        {
            // Create a Subject
            Subject subject = new Subject(1, "C# Programming");
            // choose the exam type
            Console.WriteLine("Choose the exam type:");
            Console.WriteLine("1. Final Exam");
            Console.WriteLine("2. Practical Exam");
            int examType = int.Parse(Console.ReadLine());
            // enter the exam time
            Console.Write("Enter the exam time (in minutes): ");
            int examTime = int.Parse(Console.ReadLine());
            // enter the number of questions
            Console.Write("Please enter the number of questions you wanted to create: ");
            int numberOfQuestions = int.Parse(Console.ReadLine());
            Exam exam;

            // Create the exam based on user input
            if (examType == 1)
            {
                exam = new FinalExam(examTime, numberOfQuestions);
            }
            else
            {
                exam = new PracticalExam(examTime, numberOfQuestions);
            }

            // Add questions to the exam
            for (int i = 0; i < exam.NumberOfQuestions; i++)
            {
                Console.WriteLine($"Enter details for Question {i + 1}:");
                Console.WriteLine("Choose the question type:");
                Console.WriteLine("1. True/False");
                Console.WriteLine("2. MCQ");
                int questionType = int.Parse(Console.ReadLine());
                Console.Write("Header: ");
                string header = Console.ReadLine();
                Console.Write("Body: ");
                string body = Console.ReadLine();
                Console.Write("Mark: ");
                int mark = int.Parse(Console.ReadLine());
                Question question;

                if (questionType == 1)
                {
                    question = new TrueFalseQuestion(header, body, mark);

                    // right answer for True/False questions
                    Console.Write("Right answer (1 for True, 2 for False): ");
                    int rightAnswer = int.Parse(Console.ReadLine());
                    question.RightAnswer = (rightAnswer == 1) ? "True" : "False";
                }
                else
                {
                    question = new MCQQuestion(header, body, mark);
                    // Add answers for MCQ
                    Console.Write("Enter the number of answers: ");
                    int numAnswers = int.Parse(Console.ReadLine());
                    for (int j = 0; j < numAnswers; j++)
                    {
                        Console.Write($"Answer {j + 1} Text: ");
                        string answerText = Console.ReadLine();
                        question.AnswerList.Add(new Answer(j + 1, answerText));
                    }
                    // right answer for MCQ questions
                    Console.Write("Right answer (enter the answer number): ");
                    question.RightAnswer = Console.ReadLine();
                }
                exam.Questions.Add(question);
            }
            subject.CreateExam(exam);
            // Take the exam
            exam.TakeExam();
        }
    }
}
