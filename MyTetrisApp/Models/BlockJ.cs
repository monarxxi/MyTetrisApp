using System.Windows.Media;

namespace MyTetrisApp.Models;

/// <summary>
/// Реализация фигурки "J" (зеркальная форма буквы "L").
/// </summary>
public class BlockJ : Tetromino
{
    public BlockJ(int startX, int startY) : base(startX, startY)
    {
        // Устанавливаем начальную форму фигурки "J" (вертикальная с базой внизу)
        Shape = new[,]
        {
            { 1, 0, 0 },
            { 1, 1, 1 }
        };

        Color = Brushes.Blue;
    }

    /// <summary>
    /// Переопределение метода вращения.
    /// Фигура "J" имеет 4 состояния (вращение на 90°).
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