namespace MyTetrisApp.Models
{
    public abstract class Tetromino
    {
        // Координаты текущей позиции фигурки на доске
        public int X { get; protected set; }
        public int Y { get; protected set; }

        // Текущая форма фигурки (матрица 4x4)
        protected int[,] Shape;

        // Конструктор для установки стартовой позиции
        protected Tetromino(int startX, int startY)
        {
            X = startX;
            Y = startY;
        }

        // Возвращает координаты ячеек фигурки относительно доски
        public IEnumerable<(int, int)> GetCells()
        {
            for (int row = 0; row < Shape.GetLength(0); row++)
            {
                for (int col = 0; col < Shape.GetLength(1); col++)
                {
                    if (Shape[row, col] != 0)
                    {
                        yield return (X + col, Y + row);
                    }
                }
            }
        }

        // Метод для вращения фигурки (по умолчанию вращает на 90 градусов по часовой стрелке)
        public virtual void Rotate()
        {
            int size = Shape.GetLength(0);
            int[,] rotated = new int[size, size];

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    rotated[col, size - 1 - row] = Shape[row, col];
                }
            }

            Shape = rotated;
        }

        // Перемещение вниз
        public virtual bool MoveDown(Board board)
        {
            if (CanMove(board, 0, 1))
            {
                Y++;
                return true;
            }

            return false;
        }

        // Перемещение влево
        public virtual bool MoveLeft(Board board)
        {
            if (CanMove(board, -1, 0))
            {
                X--;
                return true;
            }

            return false;
        }

        // Перемещение вправо
        public virtual bool MoveRight(Board board)
        {
            if (CanMove(board, 1, 0))
            {
                X++;
                return true;
            }

            return false;
        }

        // Проверяет, можно ли двигаться в указанном направлении
        protected bool CanMove(Board board, int deltaX, int deltaY)
        {
            foreach (var (cellX, cellY) in GetCells())
            {
                int newX = cellX + deltaX;
                int newY = cellY + deltaY;

                if (board.IsCellOccupied(newX, newY))
                {
                    return false;
                }
            }

            return true;
        }
    }
}