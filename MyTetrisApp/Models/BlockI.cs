using System.Windows.Media;

namespace MyTetrisApp.Models;

/// <summary>
/// Реализация фигурки "I" (линия длиной 4 клетки).
/// </summary>
public class BlockI : Tetromino
{
    public BlockI(int startX, int startY) : base(startX, startY)
    {
        // Устанавливаем начальную форму фигурки "I" (вертикальная линия)
        Shape = new[,]
        {
            { 1 },
            { 1 },
            { 1 },
            { 1 }
        };

        Color = Brushes.Cyan;
    }

    /// <summary>
    /// Переопределение метода вращения.
    /// Фигура "I" меняет форму между вертикальной и горизонтальной.
    /// </summary>
    public override void Rotate()
    {
        // Проверяем текущую форму (ширина больше высоты — горизонтальная)
        if (Shape.GetLength(0) > Shape.GetLength(1))
        {
            // Меняем на горизонтальную
            Shape = new[,]
            {
                { 1, 1, 1, 1 }
            };
        }
        else
        {
            // Меняем на вертикальную
            Shape = new[,]
            {
                { 1 },
                { 1 },
                { 1 },
                { 1 }
            };
        }
    }
}