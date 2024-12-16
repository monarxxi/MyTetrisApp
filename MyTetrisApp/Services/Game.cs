using MyTetrisApp.Models;

namespace MyTetrisApp.Services;

public class Game(int boardWidth, int boardHeight)
{
    public Board Board { get; } = new(boardWidth, boardHeight); // Игровая доска

    public Tetromino CurrentTetromino { get; private set; } =
        TetrominoFactory.CreateRandomTetromino(boardWidth / 2 - boardWidth / 10, 0); // Текущая фигура

    public Tetromino NextTetromino { get; private set; } =
        TetrominoFactory.CreateRandomTetromino(boardWidth / 2 - boardWidth / 10, 0); // Следующая фигура

    private bool _isRunning = true; // Состояние игры

    public event Action? OnGameOver; // Событие завершения игры

    private int _clearedLines; // Счётчик очищенных линий

    public int ClearedLines => _clearedLines; // Открытое свойство для доступа

    public event Action? OnSpeedIncrease; // Событие для увеличения скорости


    public void Update()
    {
        Board.PrintBoard();

        if (!_isRunning)
            return;

        // Перемещение текущей фигурки вниз
        if (CurrentTetromino.MoveDown(Board)) return;

        // Фиксируем фигурку на доске
        Board.LockTetromino(CurrentTetromino);

        // Подсчитываем количество очищенных линий
        var oldClearedLines = _clearedLines;
        _clearedLines += Board.ClearLines();

        // Уведомляем об увеличении скорости каждые 3 линии
        var linesToCheck = _clearedLines / 3 - (oldClearedLines / 3);
        for (var i = 0; i < linesToCheck; i++)
        {
            OnSpeedIncrease?.Invoke();
        }

        // Переносим следующую фигуру в текущую
        CurrentTetromino = NextTetromino;

        // Создаем новую фигурку
        NextTetromino = TetrominoFactory.CreateRandomTetromino(Board.Width / 2 - boardWidth / 10, 0);

        // Проверяем, можно ли разместить новую фигурку
        if (!CanPlaceTetromino(CurrentTetromino))
        {
            OnGameOver?.Invoke(); // Сигнализируем о завершении игры
            _isRunning = false;
        }
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