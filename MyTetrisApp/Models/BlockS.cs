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
            { 1, 1, 0 },
            { 0, 0, 0 }
        };

        Color = Brushes.Green;
    }
}