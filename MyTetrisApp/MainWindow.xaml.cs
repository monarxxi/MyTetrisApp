using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MyTetrisApp.Services;

namespace MyTetrisApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private Game _game;
    private const int CellSize = 30; // Размер ячейки в пикселях

    public MainWindow(Game game)
    {
        _game = game;
        InitializeComponent();
        StartGame();
    }
    

    private void StartGame()
    {
        // Создаем экземпляр игры
        _game = new Game(10, 20);

        // Подписываемся на событие завершения игры
        _game.OnGameOver += GameOver;

        // Инициализируем доску
        DrawBoard();

        // Запускаем игровой процесс
        CompositionTarget.Rendering += UpdateGame;
    }

    private void DrawBoard()
    {
        // Очищаем канвас
        GameCanvas.Children.Clear();

        // Рисуем сетку игрового поля
        for (var y = 0; y < _game.Board.Height; y++)
        {
            for (var x = 0; x < _game.Board.Width; x++)
            {
                var cell = new Rectangle
                {
                    Width = CellSize,
                    Height = CellSize,
                    Stroke = Brushes.Gray,
                    Fill = Brushes.Black
                };
                Canvas.SetLeft(cell, x * CellSize);
                Canvas.SetTop(cell, y * CellSize);
                GameCanvas.Children.Add(cell);
            }
        }
    }

    private void UpdateGame(object? sender, EventArgs e)
    {
        // Обновляем логику игры
        _game.Update();

        // Перерисовываем игровое поле
        RenderGame();
    }

    private void RenderGame()
    {
        GameCanvas.Children.Clear();

        // Рисуем статические ячейки
        for (var y = 0; y < _game.Board.Height; y++)
        {
            for (var x = 0; x < _game.Board.Width; x++)
            {
                if (_game.Board.IsCellOccupied(x, y))
                {
                    DrawCell(x, y, Brushes.Blue);
                }
            }
        }

        // Рисуем текущую фигурку
        foreach (var (x, y) in _game.CurrentTetromino.GetCells())
        {
            DrawCell(x, y, Brushes.Red);
        }
    }

    private void DrawCell(int x, int y, Brush color)
    {
        var cell = new Rectangle
        {
            Width = CellSize,
            Height = CellSize,
            Fill = color
        };
        Canvas.SetLeft(cell, x * CellSize);
        Canvas.SetTop(cell, y * CellSize);
        GameCanvas.Children.Add(cell);
    }

    private void GameOver()
    {
        // Отображение сообщения о завершении игры
        MessageBox.Show("Game Over!");
        CompositionTarget.Rendering -= UpdateGame; // Останавливаем обновление игры
    }
}