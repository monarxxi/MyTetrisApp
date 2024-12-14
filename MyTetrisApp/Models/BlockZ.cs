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
            { 0, 1, 1 },
            { 0, 0, 0 }
        };

        Color = Brushes.Red;
    }
}