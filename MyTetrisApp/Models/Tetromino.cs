using System.Windows.Media;

namespace MyTetrisApp.Models;

public abstract class Tetromino
{
    public int X { get; set; }
    public int Y { get; set; }

    public int[,] Shape { get; protected set; } = new int[1, 1];
    public Brush Color { get; protected init; } = Brushes.Transparent;

    protected Tetromino(int startX, int startY)
    {
        X = startX;
        Y = startY;
    }

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

    public virtual bool Rotate(Board board)
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

        // Проверяем возможность вращения без коррекции
        if (CanPlace(board, rotated, X, Y))
        {
            Shape = rotated;
            return true;
        }

        // Пробуем скорректировать смещение (kick correction)
        int[] xOffsets1 = { -1, 1 }; // Попробуем сдвинуть влево и вправо
        foreach (var xOffset in xOffsets1)
        {
            if (CanPlace(board, rotated, X + xOffset, Y))
            {
                X += xOffset; // Применяем сдвиг
                Shape = rotated;
                return true;
            }
        }

        // Пробуем скорректировать смещение (kick correction)
        int[] xOffsets2 = { -2, 2 }; // Попробуем сдвинуть влево и вправо
        foreach (var xOffset in xOffsets2)
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
}
