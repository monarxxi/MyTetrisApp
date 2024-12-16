namespace MyTetrisApp.Models;

public static class TetrominoFactory
{
    private static readonly Random Random = new Random();

    public static Tetromino CreateRandomTetromino(int startX, int startY)
    {
        // Генерация случайной фигурки
        var type = Random.Next(0, 7);
        return type switch
        {
            0 => new BlockI(startX, startY),
            1 => new BlockJ(startX, startY),
            2 => new BlockL(startX, startY),
            3 => new BlockO(startX, startY),
            4 => new BlockS(startX, startY),
            5 => new BlockT(startX, startY),
            6 => new BlockZ(startX, startY),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}