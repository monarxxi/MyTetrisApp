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
            { 0, 1, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 1, 0, 0 }
        };

        Color = Brushes.Cyan;
    }
}