namespace QuizApp.Core.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
        public int CorrectOptionIndex { get; set; }

        // Filformat:Id;SpørgsmålsTekst;Svar1,Svar2,Svar3,Svar4;KorrektIndex 
        public static Question FromFileString(string line)
        {
            string[] parts = line.Split(';');
            return new Question
            {
                Id = int.Parse(parts[0]),
                QuestionText = parts[1],
                Options = parts[2].Split(',').ToList(),
                CorrectOptionIndex = int.Parse(parts[3])
            };
        }

        public string ToFileString()
        {
            return $"{Id};{QuestionText};{string.Join(',', Options)};{CorrectOptionIndex}";
        }

    }


}