using QuizApp.Core;
using QuizApp.Core.Models;


namespace QuizApp.Core.Repositories
{ 
    public interface IQuestionRepository { 
        IEnumerable<Question> GetAll(); 
        Question GetById(int id); 
        void Add(Question question); 
    }
}
