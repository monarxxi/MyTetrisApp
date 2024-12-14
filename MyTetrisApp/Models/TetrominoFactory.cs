namespace MyTetrisApp.Models;

public static class TetrominoFactory
{
    private static readonly Random Random = new Random();

    public static Tetromino CreateRandomTetromino(int startX, int startY)
    {
        // Генерация случайной фигурки
        var type = Random.Next(0, 2); // Пока ограничимся двумя типами
        return type switch
        {
            0 => new BlockI(startX, startY),
            1 => new BlockO(startX, startY),
            _ => new BlockI(startX, startY)
        };
    }
}