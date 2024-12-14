using System.Windows.Media;

namespace MyTetrisApp.Models;

/// <summary>
/// Реализация фигурки "T" (форма в виде буквы "T").
/// </summary>
public class BlockT : Tetromino
{
    public BlockT(int startX, int startY) : base(startX, startY)
    {
        // Устанавливаем начальную форму фигурки "T" (горизонтальная база с центром наверху)
        Shape = new[,]
        {
            { 0, 1, 0 },
            { 1, 1, 1 }
        };

        Color = Brushes.Purple;
    }

    /// <summary>
    /// Переопределение метода вращения.
    /// Фигура "T" имеет 4 состояния (вращение на 90°).
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