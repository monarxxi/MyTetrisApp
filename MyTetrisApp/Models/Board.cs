namespace MyTetrisApp.Models
{
    public class Board
    {
        private readonly int[,] _cells; // Хранение состояния ячеек доски
        public int Width { get; } // Ширина доски
        public int Height { get; } // Высота доски

        // Конструктор для инициализации доски
        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            _cells = new int[height, width];
        }

        // Проверка, занята ли ячейка
        public bool IsCellOccupied(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return true; // Ячейки за границей считаются занятыми
            return _cells[y, x] != 0;
        }

        // Фиксация фигурки на доске
        public void LockTetromino(Tetromino tetromino)
        {
            foreach (var (x, y) in tetromino.GetCells())
            {
                if (x >= 0 && x < Width && y >= 0 && y < Height)
                {
                    _cells[y, x] = 1; // 1 означает занятую ячейку (можно заменить на цветовой код)
                }
            }
        }

        // Очистка заполненных линий
        public int ClearLines()
        {
            var linesCleared = 0;

            for (var y = 0; y < Height; y++)
            {
                if (IsLineFull(y))
                {
                    ClearLine(y);
                    linesCleared++;
                }
            }

            return linesCleared; // Возвращаем количество очищенных линий
        }

        // Проверка, заполнена ли линия полностью
        private bool IsLineFull(int y)
        {
            for (int x = 0; x < Width; x++)
            {
                if (_cells[y, x] == 0)
                    return false;
            }

            return true;
        }

        // Удаление линии и сдвиг оставшихся строк вниз
        private void ClearLine(int line)
        {
            for (var y = line; y > 0; y--)
            {
                for (var x = 0; x < Width; x++)
                {
                    _cells[y, x] = _cells[y - 1, x];
                }
            }

            // Очистка верхней строки
            for (int x = 0; x < Width; x++)
            {
                _cells[0, x] = 0;
            }
        }

        // Метод для отображения текущего состояния доски (для отладки)
        public void PrintBoard()
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    Console.Write(_cells[y, x] == 0 ? "." : "#");
                }

                Console.WriteLine();
            }
        }
    }
}