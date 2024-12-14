using System.Windows.Media;

namespace MyTetrisApp.Models
{
    public abstract class Tetromino
    {
        // Координаты текущей позиции фигурки на доске
        public int X { get; protected set; }
        public int Y { get; protected set; }

        // Текущая форма фигурки
        protected int[,] Shape;
        
        // Цвет фигурки
        public Brush Color { get; protected set; } 

        // Конструктор для установки стартовой позиции
        protected Tetromino(int startX, int startY)
        {
            X = startX;
            Y = startY;
        }

        // Возвращает координаты ячеек фигурки относительно доски
        public IEnumerable<(int, int)> GetCells()
        {
            for (var row = 0; row < Shape.GetLength(0); row++)
            {
                for (var col = 0; col < Shape.GetLength(1); col++)
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
            var size = Shape.GetLength(0);
            var rotated = new int[size, size];

            for (var row = 0; row < size; row++)
            {
                for (var col = 0; col < size; col++)
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
                var newX = cellX + deltaX;
                var newY = cellY + deltaY;

                if (board.IsCellOccupied(newX, newY))
                {
                    return false;
                }
            }

            return true;
        }
    }
}