namespace MyTetrisApp.Services;

public interface IMovable
{
    bool MoveLeft();
    bool MoveRight();
    bool MoveDown();
    bool Rotate();
}
