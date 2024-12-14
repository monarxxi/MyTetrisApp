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
            { 1, 1, 1 },
            { 0, 0, 0 }
        };

        Color = Brushes.Blue;
    }
}