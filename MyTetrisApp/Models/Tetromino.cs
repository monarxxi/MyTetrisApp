using System.Windows.Media;

namespace MyTetrisApp.Models;

public abstract class Tetromino(int startX, int startY)
{
    // Координаты текущей позиции фигурки на доске
    private int X { get; set; } = startX;
    private int Y { get; set; } = startY;

    // Текущая форма фигурки
    protected internal int[,] Shape = new int[1, 1]; // Значение по умолчанию

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

    public virtual bool Rotate(Board board)
    {
        var size = Shape.GetLength(0); // Размер квадратной матрицы 
        var rotated = new int[size, size];

        // Транспонирование и инверсия строк
        for (var row = 0; row < size; row++)
        {
            for (var col = 0; col < size; col++)
            {
                rotated[col, size - 1 - row] = Shape[row, col];
            }
        }

        // Проверяем возможность вращения без коррекции
        if (CanPlace(board, rotated, X, Y))
        {
            Shape = rotated;
            return true;
        }

        // Пробуем скорректировать смещение (kick correction)
        int[] xOffsets = {-1, 1 }; // Попробуем сдвинуть влево и вправо
        foreach (var xOffset in xOffsets)
        {
            if (CanPlace(board, rotated, X + xOffset, Y))
            {
                X += xOffset; // Применяем сдвиг
                Shape = rotated;
                return true;
            }
        }

        // Если ничего не помогло, поворот невозможен
        return false;
    }

    private static bool CanPlace(Board board, int[,] rotatedShape, int newX, int newY)
    {
        for (var row = 0; row < rotatedShape.GetLength(0); row++)
        {
            for (var col = 0; col < rotatedShape.GetLength(1); col++)
            {
                if (rotatedShape[row, col] == 1)
                {
                    var boardX = newX + col;
                    var boardY = newY + row;

                    // Проверяем границы доски
                    if (boardX < 0 || boardX >= board.Width || boardY < 0 || boardY >= board.Height)
                    {
                        return false;
                    }

                    // Проверяем, занята ли ячейка
                    if (board.IsCellOccupied(boardX, boardY))
                    {
                        return false;
                    }
                }
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