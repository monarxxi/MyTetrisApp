using System.Windows.Media;

namespace MyTetrisApp.Models;

public abstract class Tetromino(int startX, int startY)
{
    // Координаты текущей позиции фигурки на доске
    private int X { get; set; } = startX;
    private int Y { get; set; } = startY;

    // Текущая форма фигурки
    protected int[,] Shape = new int[1, 1]; // Значение по умолчанию

    // Цвет фигурки
    public Brush Color { get; protected init; } = Brushes.Transparent; // Прозрачный цвет по умолчанию

    // Возвращает координаты ячеек фигурки относительно доски
    public IEnumerable<(int, int)> GetCells()
    {
        for (var row = 0; row < Shape.GetLength(0); row++)
        {
            for (var col = 0; col < Shape.GetLength(1); col++)
            {
                if (Shape[row, col] != 0)
                {
                    yield return (X + col, Y + row);
                }
            }
        }
    }

    public bool MoveLeft(Board board)
    {
        if (CanMove(board, -1, 0))
        {
            X--;
            return true;
        }

        return false;
    }

    public bool MoveRight(Board board)
    {
        if (CanMove(board, 1, 0))
        {
            X++;
            return true;
        }

        return false;
    }

    public bool MoveDown(Board board)
    {
        if (CanMove(board, 0, 1))
        {
            Y++;
            return true;
        }

        return false;
    }

    private bool CanMove(Board board, int dx, int dy)
    {
        foreach (var (x, y) in GetCells())
        {
            if (board.IsCellOccupied(x + dx, y + dy))
            {
                return false;
            }
        }

        return true;
    }

    public virtual void Rotate()
    {
        var size = Shape.GetLength(0); // Размер квадратной матрицы (3x3)
        var rotated = new int[size, size]; // Новая матрица для повернутой формы

        for (var row = 0; row < size; row++)
        {
            for (var col = 0; col < size; col++)
            {
                rotated[col, size - 1 - row] = Shape[row, col]; // Транспонирование и инверсия строк
            }
        }

        Shape = rotated; // Применяем новую форму
    }

    public bool CanRotate(Board board)
    {
        var size = Shape.GetLength(0);
        var rotated = new int[size, size];

        // Транспонирование и инверсия строк
        for (var row = 0; row < size; row++)
        {
            for (var col = 0; col < size; col++)
            {
                rotated[col, size - 1 - row] = Shape[row, col];
            }
        }

        // Проверяем, что после поворота координаты остаются валидными
        foreach (var (x, y) in GetRotatedCells(rotated))
        {
            if (x < 0 || x >= board.Width || y < 0 || y >= board.Height || board.IsCellOccupied(x, y))
            {
                return false;
            }
        }

        return true;
    }

// Получение координат ячеек после поворота
    private IEnumerable<(int x, int y)> GetRotatedCells(int[,] rotated)
    {
        var size = rotated.GetLength(0);

        for (var row = 0; row < size; row++)
        {
            for (var col = 0; col < size; col++)
            {
                if (rotated[row, col] == 1)
                {
                    yield return (X + col, Y + row);
                }
            }
        }
    }
}