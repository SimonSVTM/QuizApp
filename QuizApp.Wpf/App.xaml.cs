using QuizApp.Core.Repositories;

using QuizApp.Wpf.ViewModels;

using QuizApp.Wpf.Views;
using System.Windows;


namespace QuizApp.Wpf
{

    public partial class App : Application

    {

        protected override void OnStartup(StartupEventArgs e)

        {

            base.OnStartup(e);

            var mainViewModel = new QuizViewModel(new FileQuestionRepository());

            var mainView = new MainWindow();

            mainView.DataContext = mainViewModel;

            mainView.Show();

        }

    }

}