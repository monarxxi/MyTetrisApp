using MyTetrisApp.Models;

namespace MyTetrisApp.Services;

public class TetrominoMover : IMovable
{
    private readonly Board _board;
    private readonly Tetromino _tetromino;

    public TetrominoMover(Board board, Tetromino tetromino)
    {
        _board = board;
        _tetromino = tetromino;
    }

    public bool MoveLeft()
    {
        return CanMove(-1, 0) && UpdatePosition(-1, 0);
    }

    public bool MoveRight()
    {
        return CanMove(1, 0) && UpdatePosition(1, 0);
    }

    public bool MoveDown()
    {
        return CanMove(0, 1) && UpdatePosition(0, 1);
    }

    public bool Rotate()
    {
        return _tetromino.Rotate(_board);
    }

    private bool CanMove(int dx, int dy)
    {
        foreach (var (x, y) in _tetromino.GetCells())
        {
            var newX = x + dx;
            var newY = y + dy;

            if (_board.IsCellOccupied(newX, newY))
            {
                return false;
            }
        }

        return true;
    }

    private bool UpdatePosition(int dx, int dy)
    {
        _tetromino.X += dx;
        _tetromino.Y += dy;
        return true;
    }
}
