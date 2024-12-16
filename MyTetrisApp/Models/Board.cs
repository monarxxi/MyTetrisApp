using System.Windows.Media;

namespace MyTetrisApp.Models;

public class Board
{
    private readonly int[,] _cells; // Хранение состояния ячеек доски
    private readonly Brush[,] _cellColors; // Хранение цвета каждой ячейки
    public int Width { get; } // Ширина доски
    public int Height { get; } // Высота доски

    // Конструктор для инициализации доски
    public Board(int width, int height)
    {
        Width = width;
        Height = height;
        _cells = new int[height, width];
        _cellColors = new Brush[height, width];
    }

    // Проверка, занята ли ячейка
    public bool IsCellOccupied(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return true; // Ячейки за границей считаются занятыми
        return _cells[y, x] != 0;
    }

    public Brush GetCellColor(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return Brushes.Transparent; // Цвет для границ или пустых ячеек
        return _cellColors[y, x];
    }

    // Фиксация фигурки на доске
    public void LockTetromino(Tetromino tetromino)
    {
        foreach (var (x, y) in tetromino.GetCells())
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                _cells[y, x] = 1; // Помечаем ячейку как занятую
                _cellColors[y, x] = tetromino.Color; // Сохраняем цвет фигурки
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
                _cells[y, x] = _cells[y - 1, x]; // Сдвиг состояния ячейки вниз
                _cellColors[y, x] = _cellColors[y - 1, x]; // Сдвиг цвета ячейки вниз
            }
        }

        // Очистка верхней строки
        for (var x = 0; x < Width; x++)
        {
            _cells[0, x] = 0;
            _cellColors[0, x] = Brushes.Transparent; // Очистка цвета верхней строки
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