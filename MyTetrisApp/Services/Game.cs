using MyTetrisApp.Models;

namespace MyTetrisApp.Services;

public class Game(int boardWidth, int boardHeight)
{
    public Board Board { get; } = new(boardWidth, boardHeight); // Игровая доска

    public Tetromino CurrentTetromino { get; private set; } =
        TetrominoFactory.CreateRandomTetromino(boardWidth / 2 - boardWidth / 10, 0); // Текущая фигур ка

    private bool _isRunning = true; // Состояние игры
    
    public event Action? OnGameOver; // Событие завершения игры
    
    private int _clearedLines; // Счётчик очищенных линий
    
    public int ClearedLines => _clearedLines; // Открытое свойство для доступа
    
    public event Action? OnSpeedIncrease; // Событие для увеличения скорости


    public void Update()
    {
        if (!_isRunning)
            return;

        // Перемещение текущей фигурки вниз
        if (CurrentTetromino.MoveDown(Board)) return;

        // Фиксируем фигурку на доске
        Board.LockTetromino(CurrentTetromino);

        // Очищаем линии и обновляем счётчик
        var linesCleared = Board.ClearLines();
        _clearedLines += linesCleared;
        
        // Уведомляем об увеличении скорости каждые 3 линий
        if (_clearedLines >= 3)
        {
            OnSpeedIncrease?.Invoke();
            _clearedLines = 0; // Сбрасываем счётчик
        }

        // Создаем новую фигурку
        CurrentTetromino = TetrominoFactory.CreateRandomTetromino(Board.Width / 2 - boardWidth / 10, 0);

        // Проверяем, можно ли разместить новую фигурку
        if (CanPlaceTetromino(CurrentTetromino)) return;
        OnGameOver?.Invoke(); // Сигнализируем о завершении игры
        _isRunning = false;
    }

    private bool CanPlaceTetromino(Tetromino tetromino)
    {
        foreach (var (x, y) in tetromino.GetCells())
        {
            if (Board.IsCellOccupied(x, y))
            {
                return false;
            }
        }

        return true;
    }
}