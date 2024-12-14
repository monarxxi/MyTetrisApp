using MyTetrisApp.Models;

namespace MyTetrisApp.Services;

public class Game(int boardWidth, int boardHeight)
{
    public Board Board { get; } = new(boardWidth, boardHeight); // Игровая доска

    public Tetromino CurrentTetromino { get; private set; } =
        TetrominoFactory.CreateRandomTetromino(boardWidth / 2, 0); // Текущая фигурка

    private bool _isRunning = true; // Состояние игры

    public event Action? OnGameOver; // Событие завершения игры

    // Создаем первую фигурку

    public void Update()
    {
        if (!_isRunning)
            return;

        // Перемещение текущей фигурки вниз
        if (CurrentTetromino.MoveDown(Board)) return;
        // Фиксируем фигурку на доске
        Board.LockTetromino(CurrentTetromino);

        // Очищаем линии
        Board.ClearLines();

        // Создаем новую фигурку
        CurrentTetromino = TetrominoFactory.CreateRandomTetromino(Board.Width / 2, 0);

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