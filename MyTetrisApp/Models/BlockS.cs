using System.Windows.Media;

namespace MyTetrisApp.Models;

/// <summary>
/// Реализация фигурки "S" (зеркальная зигзагообразная форма).
/// </summary>
public class BlockS : Tetromino
{
    public BlockS(int startX, int startY) : base(startX, startY)
    {
        // Устанавливаем начальную форму фигурки "S" (горизонтальная зеркальная зигзагообразная форма)
        Shape = new[,]
        {
            { 0, 1, 1 },
            { 1, 1, 0 }
        };

        Color = Brushes.Green;
    }

    /// <summary>
    /// Переопределение метода вращения.
    /// Фигура "S" имеет 2 состояния (горизонтальное и вертикальное).
    /// </summary>
    public override void Rotate()
    {
        var size = Shape.GetLength(0);
        var rotated = new int[size, size];

        // Транспонирование и инверсия для вращения
        for (var row = 0; row < size; row++)
        {
            for (var col = 0; col < size; col++)
            {
                rotated[col, size - 1 - row] = Shape[row, col];
            }
        }

        Shape = rotated;
    }
}