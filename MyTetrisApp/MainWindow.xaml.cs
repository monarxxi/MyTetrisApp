using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
    private bool _isFastDropActive; // Флаг для быстрого падения

    private bool _isPaused; // Флаг для паузы
    private bool _isGameRunning; // Флаг, указывающий, что игра идёт
    
    private int _score;
    private int _linesCleared;
    private int _level = 1;
    private const string HighScoreFilePath = "highscore.txt"; // Путь к файлу для хранения лучшего счёта
    private int _highScore; // Лучший счёт
    
    public MainWindow(Game game)
    {
        _game = game;
        InitializeComponent();
        
        // Загружаем лучший счёт
        LoadHighScore();
        HighScoreTextBlock.Text = _highScore.ToString();

        // Подписываемся на события клавиатуры
        KeyDown += OnKeyDown;
        KeyUp += OnKeyUp;

        // Настраиваем состояние по умолчанию
        _isPaused = false;
        _isGameRunning = false;

        // Устанавливаем окно в полноэкранный режим
        WindowState = WindowState.Maximized;
        
        // Обновляем таблицу
        UpdateStats();
    }

    private void StartGame()
    {
        // Инициализируем игру
        _game = new Game(10, 20);

        // Подписываемся на события игры
        _game.OnSpeedIncrease += IncreaseSpeed;
        _game.OnGameOver += GameOver;

        // Настраиваем таймер
        _gameTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(_speed)
        };
        _gameTimer.Tick += UpdateGame;

        // Настраиваем интерфейс
        DrawBoard();
        RenderGame();

        // Запускаем игру
        _gameTimer.Start();
        _isGameRunning = true;
        _isPaused = false;
        
        // Обновляем таблицу
        UpdateStats();
    }
    
    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
        SaveHighScore();
        base.OnClosing(e);
    }

    private void PauseGame()
    {
        if (_isGameRunning && !_isPaused)
        {
            _gameTimer.Stop();
            _isPaused = true;
        }
    }

    private void ResumeGame()
    {
        if (_isGameRunning && _isPaused)
        {
            _gameTimer.Start();
            _isPaused = false;
        }
    }

    private void RestartGame()
    {
        SaveHighScore();
        
        // Останавливаем текущую игру
        if (_isGameRunning)
        {
            _gameTimer.Stop();
        }

        // Перезапускаем игру
        StartGame();
    }

    private void IncreaseSpeed()
    {
        switch (_speed)
        {
            case >= 300:
                _speed -= 25;
                break;
            case >= 200:
                _speed -= 10;
                break;
            case >= 100:
                _speed -= 5;
                break;
            case >= 80:
                break;
        }
    }

    private void DrawBoard()
    {
        var canvasWidth = _game.Board.Width * CellSize;
        var canvasHeight = _game.Board.Height * CellSize;

        GridCanvas.Width = canvasWidth;
        GridCanvas.Height = canvasHeight;
        GameCanvas.Width = canvasWidth;
        GameCanvas.Height = canvasHeight;

        GridCanvas.Children.Clear();

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

    private void RenderGame()
    {
        GameCanvas.Children.Clear();

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

        foreach (var (x, y) in _game.CurrentTetromino.GetCells())
        {
            DrawCell(GameCanvas, x, y, _game.CurrentTetromino.Color);
        }
    }

    private static void DrawCell(Canvas canvas, int x, int y, Brush color)
    {
        var cell = new Rectangle
        {
            Width = CellSize,
            Height = CellSize,
            Fill = color,
            Stroke = Brushes.Black,
            StrokeThickness = 2
        };

        Canvas.SetLeft(cell, x * CellSize);
        Canvas.SetTop(cell, y * CellSize);
        canvas.Children.Add(cell);
    }

    private void UpdateGame(object? sender, EventArgs e)
    {
        // Перед обновлением логики сохраняем количество очищенных линий
        var linesBefore = _linesCleared;

        // Обновляем логику игры
        _game.Update();

        // Если количество очищенных линий изменилось
        if (_linesCleared != _game.ClearedLines)
        {
            var newLines = _game.ClearedLines - linesBefore;
            _linesCleared += newLines;

            // Увеличиваем счёт за каждую очищенную линию
            switch (newLines)
            {
                case 1:
                    _score +=  100;
                    break;
                case 2:
                    _score += 300;
                    break;
                case 3:
                    _score += 700;
                    break;
                case 4:
                    _score += 1500;
                    break;
            }
            
            // Проверяем уровень (каждые 3 линий)
            _level = 1 + (_linesCleared / 3);
            
            // Обновляем лучший счёт
            if (_score > _highScore)
            {
                _highScore = _score;
                HighScoreTextBlock.Text = _highScore.ToString();
            }

            // Обновляем табличку
            UpdateStats();
        }

        // Перерисовываем игровое поле
        RenderGame();
    }

    private void GameOver()
    {
        SaveHighScore();
        MessageBox.Show("Game Over!");
        _gameTimer.Stop();
        _isGameRunning = false;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (!_isGameRunning || _isPaused) return;

        switch (e.Key)
        {
            case Key.Left:
                _game.CurrentTetromino.MoveLeft(_game.Board);
                RenderGame();
                break;
            case Key.Right:
                _game.CurrentTetromino.MoveRight(_game.Board);
                RenderGame();
                break;
            case Key.Down:
                if (!_isFastDropActive)
                {
                    _isFastDropActive = true;
                    _gameTimer.Interval = TimeSpan.FromMilliseconds(_speed / 5.0);
                }

                break;
            case Key.Up:
                if (_game.CurrentTetromino.Rotate(_game.Board))
                {
                    RenderGame();
                }

                break;
        }
    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Down && _isFastDropActive)
        {
            _isFastDropActive = false;
            _gameTimer.Interval = TimeSpan.FromMilliseconds(_speed);
        }
    }

    private void StartButton_Click(object sender, RoutedEventArgs e)
    {
        if (!_isGameRunning)
        {
            StartGame();
        }
        else if (_isPaused)
        {
            ResumeGame();
        }
    }

    private void PauseButton_Click(object sender, RoutedEventArgs e)
    {
        PauseGame();
    }

    private void RestartButton_Click(object sender, RoutedEventArgs e)
    {
        RestartGame();
    }
    
    private void UpdateStats()
    {
        ScoreTextBlock.Text = _score.ToString();
        LinesTextBlock.Text = _linesCleared.ToString();
        LevelTextBlock.Text = _level.ToString();
    }
    
    private void LoadHighScore()
    {
        if (File.Exists(HighScoreFilePath))
        {
            var content = File.ReadAllText(HighScoreFilePath);
            if (int.TryParse(content, out var savedHighScore))
            {
                _highScore = savedHighScore;
            }
        }
    }

    private void SaveHighScore()
    {
        File.WriteAllText(HighScoreFilePath, _highScore.ToString());
    }
}