using System.Windows.Media;

namespace MyTetrisApp.Models;

/// <summary>
/// Реализация фигурки "L" (форма в виде буквы "L").
/// </summary>
public class BlockL : Tetromino
{
    public BlockL(int startX, int startY) : base(startX, startY)
    {
        // Устанавливаем начальную форму фигурки "L" (вертикальная с базой внизу)
        Shape = new[,]
        {
            { 0, 0, 1 },
            { 1, 1, 1 },
            { 0, 0, 0 }
        };

        Color = Brushes.Orange;
    }
}