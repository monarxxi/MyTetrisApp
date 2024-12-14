using System.Windows;
using MyTetrisApp.Services;

namespace MyTetrisApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Создаем экземпляр игры
        var game = new Game(10, 20);

        // Создаем главное окно, передавая игру в конструктор
        var mainWindow = new MainWindow(game);
        mainWindow.Show();
    }
}