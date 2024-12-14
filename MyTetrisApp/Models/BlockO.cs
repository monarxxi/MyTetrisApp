using System.Windows.Media;

namespace MyTetrisApp.Models;

/// <summary>
/// Реализация фигурки "O" (квадрат 2x2).
/// </summary>
public class BlockO : Tetromino
{
    public BlockO(int startX, int startY) : base(startX, startY)
    {
        // Устанавливаем форму фигурки "O"
        Shape = new[,]
        {
            { 1, 1 },
            { 1, 1 }
        };

        Color = Brushes.Yellow;
    }

    /// <summary>
    /// Переопределение метода вращения.
    /// У "BlockO" форма не изменяется при вращении.
    /// </summary>
    public override void Rotate()
    {
        // Для BlockO ничего не делаем, так как форма не меняется
    }
}