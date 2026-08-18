using QuizApp.Core;
using QuizApp.Core.Models;
using System.Diagnostics;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.IO;
namespace QuizApp.Core.Repositories { 
    public class FileQuestionRepository : IQuestionRepository {
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Repositories", "questions.txt");
        public FileQuestionRepository() { 
            if (!File.Exists(_filePath)) 
                File.Create(_filePath).Close(); 
        }

        public IEnumerable<Question> GetAll() { 
            try { 
                string[] lines = File.ReadAllLines(_filePath); 
                List<Question> result = new List<Question>(); 
                foreach (string line in lines) { 
                    Debug.WriteLine(line);
                    if (!string.IsNullOrWhiteSpace(line)) result.Add(Question.FromFileString(line)); 
                } 
                return result; 
            } 
            catch (IOException ex) 
            { 
                Debug.WriteLine($"IO-fejl: {ex.Message}"); 
                return new List<Question>(); 
            } 
        }
        public Question GetById(int id) { foreach (Question q in GetAll()) { if (q.Id == id) return q; } return null; }
        public void Add(Question question) { 
            List<Question> all = GetAll().ToList(); 
            int maxId = 0; 
            foreach (Question q in all) { 
                if (q.Id > maxId) maxId = q.Id; 
            } 
            question.Id = maxId + 1;
            try
            {
                StreamWriter sw = File.AppendText(_filePath);
                sw.WriteLine(question.ToFileString());
                sw.Close();
            }
            catch (IOException ex) { 
                Debug.WriteLine($"Skrivefejl: {ex.Message}"); 
            } 
        }
    }
}