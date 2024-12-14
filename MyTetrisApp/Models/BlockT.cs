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
            { 1, 1, 1 },
            { 0, 0, 0 }
        };

        Color = Brushes.Purple;
    }
}