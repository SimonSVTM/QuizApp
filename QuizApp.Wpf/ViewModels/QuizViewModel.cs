using QuizApp.Core.Models;
using QuizApp.Core.Repositories;
using QuizApp.Wpf.ViewModels;
using System.Collections.Generic;
using System.Windows.Input;
using QuizApp.Core;
using System.Diagnostics;

namespace QuizApp.Wpf
{
    public class QuizViewModel : ViewModelBase
    {
        private readonly IQuestionRepository _repo;
        private List<Question> _questions;
        private int _currentIndex;

        // ── Properties med PropertyChanged ────────────────────────────
        private Question _currentQuestion;
        public Question CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                OnPropertyChanged();
            }
        }

        private int _currentScore;
        public int CurrentScore
        {
            get => _currentScore;
            set
            {
                _currentScore = value;
                OnPropertyChanged();
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        private bool _isQuizFinished;
        public bool IsQuizFinished
        {
            get => _isQuizFinished;
            set
            {
                _isQuizFinished = value;
                OnPropertyChanged();
            }
        }

        private double _progressValue;
        public double ProgressValue
        {
            get => _progressValue;
            set
            {
                _progressValue = value;
                OnPropertyChanged();
            }
        }

        public ICommand AnswerCommand { get; private set; }

        // ── Constructor ───────────────────────────────────────────────
        public QuizViewModel(IQuestionRepository repo)
        {
            _repo = repo;

            // Initialiser AnswerCommand med RelayCommand
            AnswerCommand = new RelayCommand(CheckAnswer);
            // Indlæs spørgsmålene ved opstart
            LoadQuestions();
        }

        // ── Metoder ───────────────────────────────────────────────────
        private void LoadQuestions()
        {
            // Hent spørgsmål fra repository og nulstil tilstand
            _questions = _repo.GetAll().ToList();
            _currentIndex = 0;
            CurrentScore = 0;
            IsQuizFinished = false;

            if (_questions != null && _questions.Count > 0)
            {
                CurrentQuestion = _questions[_currentIndex];
                UpdateStatusAndProgress();
            }
        }

        private void CheckAnswer(object parameter)
        {
            if (parameter == null || IsQuizFinished) return;

            // Konverter parameter (fx "0", "1", "2", "3") til int
            if (int.TryParse(parameter.ToString(), out int selectedIndex))
            {
                // Sammenlign med det korrekte svar-indeks
                if (CurrentQuestion != null && selectedIndex == CurrentQuestion.CorrectOptionIndex)
                {
                    CurrentScore++;
                }

                // Gå videre til næste spørgsmål
                NextQuestion();
            }
        }

        private void NextQuestion()
        {
            _currentIndex++;

            if (_questions != null && _currentIndex < _questions.Count)
            {
                // Flyt til næste spørgsmål
                CurrentQuestion = _questions[_currentIndex];
                UpdateStatusAndProgress();
            }
            else
            {
                // Nået til slutningen af quizzen
                IsQuizFinished = true;
                StatusMessage = $"Quiz afsluttet! Du fik {CurrentScore} af {_questions?.Count ?? 0} rigtige.";
                ProgressValue = 100;
            }
        }

        private void UpdateStatusAndProgress()
        {
            if (_questions == null || _questions.Count == 0) return;

            // Opdater statusbesked (fx 'Spørgsmål 1 af 10')
            StatusMessage = $"Spørgsmål {_currentIndex + 1} af {_questions.Count}";

            // Beregn fremgang i procent til ProgressBar (0 - 100)
            ProgressValue = ((double)(_currentIndex + 1) / _questions.Count) * 100;
        }
    }
}