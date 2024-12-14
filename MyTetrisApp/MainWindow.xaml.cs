using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using MyTetrisApp.Services;

namespace MyTetrisApp;

public partial class MainWindow
{
    private Game _game;
    private const int CellSize = 50; // Размер ячейки в пикселях
    private DispatcherTimer _gameTimer = new(); // Таймер для обновления игры
    private int _speed = 300; // Интервал в миллисекундах, скорость падения блока (начальная скорость)

    public MainWindow(Game game)
    {
        _game = game;
        InitializeComponent();
        
        // Подписываемся на событие изменения количества очищенных линий
        _game.OnLinesCleared += HandleLinesCleared;

        // Устанавливаем окно в полноэкранный режим
        WindowState = WindowState.Maximized;
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

        // Инициализация таймера
        _gameTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(_speed) // Устанавливаем интервал таймера
        };
        _gameTimer.Tick += UpdateGame; // Подписка на событие обновления игры
        _gameTimer.Start(); // Запускаем таймер
    }

    private void DrawBoard()
    {
        // Настраиваем размеры канвасов
        var canvasWidth = _game.Board.Width * CellSize;
        var canvasHeight = _game.Board.Height * CellSize;

        GridCanvas.Width = canvasWidth;
        GridCanvas.Height = canvasHeight;
        GameCanvas.Width = canvasWidth;
        GameCanvas.Height = canvasHeight;

        // Очищаем только сетку, чтобы избежать дублирования
        GridCanvas.Children.Clear();

        // Рисуем сетку игрового поля
        for (var y = 0; y < _game.Board.Height; y++)
        {
            for (var x = 0; x < _game.Board.Width; x++)
            {
                var cell = new Rectangle
                {
                    Width = CellSize,
                    Height = CellSize,
                    Stroke = Brushes.Gray, // Граница клеток
                    Fill = Brushes.Transparent // Заполнение ячейки прозрачным цветом
                };
                Canvas.SetLeft(cell, x * CellSize);
                Canvas.SetTop(cell, y * CellSize);
                GridCanvas.Children.Add(cell);
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
        // Очищаем только канвас для фигур
        GameCanvas.Children.Clear();

        // Рисуем статические ячейки
        for (var y = 0; y < _game.Board.Height; y++)
        {
            for (var x = 0; x < _game.Board.Width; x++)
            {
                if (_game.Board.IsCellOccupied(x, y))
                {
                    DrawCell(GameCanvas, x, y, _game.Board.GetCellColor(x, y));
                }
            }
        }

        // Рисуем текущую фигурку
        foreach (var (x, y) in _game.CurrentTetromino.GetCells())
        {
            DrawCell(GameCanvas, x, y, _game.CurrentTetromino.Color);
        }
    }

    private void DrawCell(Canvas canvas, int x, int y, Brush color)
    {
        var cell = new Rectangle
        {
            Width = CellSize,
            Height = CellSize,
            Fill = color
        };
        Canvas.SetLeft(cell, x * CellSize);
        Canvas.SetTop(cell, y * CellSize);
        canvas.Children.Add(cell);
    }

    private void GameOver()
    {
        // Отображение сообщения о завершении игры
        MessageBox.Show("Game Over!");
        CompositionTarget.Rendering -= UpdateGame; // Останавливаем обновление игры
    }
    
    private void HandleLinesCleared(int totalLinesCleared)
    {
        // Увеличиваем скорость каждые 10 линий
        if (totalLinesCleared % 10 == 0)
        {
            // Уменьшаем интервал таймера (увеличиваем скорость)
            if (_speed > 100) // Не даем скорости стать слишком быстрой
            {
                _speed -= 50; // Уменьшаем интервал на 50 мс
                _gameTimer.Interval = TimeSpan.FromMilliseconds(_speed); // Обновляем интервал таймера
            }
        }
    }
}