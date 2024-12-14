using System.Windows.Media;

namespace MyTetrisApp.Models;

/// <summary>
/// Реализация фигурки "Z" (зигзагообразная форма).
/// </summary>
public class BlockZ : Tetromino
{
    public BlockZ(int startX, int startY) : base(startX, startY)
    {
        // Устанавливаем начальную форму фигурки "Z" (горизонтальная зигзагообразная форма)
        Shape = new[,]
        {
            { 1, 1, 0 },
            { 0, 1, 1 }
        };

        Color = Brushes.Red;
    }

    /// <summary>
    /// Переопределение метода вращения.
    /// Фигура "Z" имеет 2 состояния (горизонтальное и вертикальное).
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