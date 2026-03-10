using System;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using static ChessExample.CheckerBoardPosition;

namespace ChessExample;
// и так это начальный код
/// <summary>
/// Represents a position on a chess field (checkerboard).
/// </summary>
/// <param name="x">Horizontal coordinate</param>
/// <param name="y">Vertical coordinate</param>
public class CheckerBoardPosition(byte x, byte y) : IParsable<CheckerBoardPosition>
{
    /// <summary>
    /// Horizontal coordinate.
    /// </summary>
    [AllowedValues(1, 2, 3, 4, 5, 6, 7, 8)]
    public byte X { get; } = x is > 0 and <= 8 ? x : throw new ArgumentOutOfRangeException(nameof(x));

    /// <summary>
    /// Vertical coordinate.
    /// </summary>
    [AllowedValues(1, 2, 3, 4, 5, 6, 7, 8)]
    public byte Y { get; } = y is > 0 and <= 8 ? y : throw new ArgumentOutOfRangeException(nameof(y));

    private const char LetterOffset = '@'; // 'A' - 1
    /// <summary>
    /// An <see cref="X"/> as a letter
    /// </summary>
    public char XLetter => (char)(LetterOffset + X);

    public override string ToString() => $"{XLetter}{Y}";

    public static CheckerBoardPosition Parse(string s, IFormatProvider? provider)
        => TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Invalid {nameof(CheckerBoardPosition)}: {s}");

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out CheckerBoardPosition? result)
    {
        if (s is [var x and >= 'A' and <= 'H', var y and >= '1' and <= '8'])
        {
            result = new CheckerBoardPosition((byte)(x - LetterOffset), byte.Parse([y]));
            return true;
        }
        result = null;
        return false;
    }
    // отдельный класс с фигурами
    public abstract class ChessFigure
    {
        public string Name { get; }

        protected ChessFigure(string name) => Name = name;

        public abstract bool IsValidMove(CheckerBoardPosition from, CheckerBoardPosition to);
    }

    //Виды фигур

    public enum FigureType
    {
        Пешка, Ладья, Слон, Ферзь, Конь, Король
    }
    
    //проверка ходов
    public static class ChessRules
    {
        public static bool IsValidMove(FigureType figure, CheckerBoardPosition from, CheckerBoardPosition to)
        {
            int dx = Math.Abs(from.X - to.X);
            int dy = Math.Abs(from.Y - to.Y);

            return figure switch
            {
                FigureType.Пешка => from.X == to.X && to.Y == from.Y + 1,
                FigureType.Ладья => from.X == to.X || from.Y == to.Y,
                FigureType.Слон => dx == dy,
                FigureType.Ферзь => from.X == to.X || from.Y == to.Y || dx == dy,
                FigureType.Конь => (dx == 1 && dy == 2) || (dx == 2 && dy == 1),
                FigureType.Король => dx <= 1 && dy <= 1,
                _ => false
            };
        }
    }

  using System;

public enum FigureType
{
    Пешка, Ладья, Слон, Ферзь, Конь, Король
}

public static class ChessRules
{
    public static bool IsValidMove(FigureType figure, CheckerBoardPosition from, CheckerBoardPosition to)
    {
        int dx = Math.Abs(from.X - to.X);
        int dy = Math.Abs(from.Y - to.Y);

        return figure switch
        {
            // Пешка: только вперёд на 1 (упрощённо, без взятия и цвета)
            FigureType.Пешка => from.X == to.X && to.Y == from.Y + 1,
            FigureType.Ладья => from.X == to.X || from.Y == to.Y,
            FigureType.Слон => dx == dy,
            FigureType.Ферзь => from.X == to.X || from.Y == to.Y || dx == dy,
            FigureType.Конь => (dx == 1 && dy == 2) || (dx == 2 && dy == 1),
            FigureType.Король => dx <= 1 && dy <= 1,
            _ => false
        };
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Проверка шахматного хода");
        Console.WriteLine("Фигуры: Пешка, Ладья, Слон, Ферзь, Конь, Король");
        Console.WriteLine("Для выхода введите пустую строку или 'exit'.\n");

        while (true)
        {
            // ввод фигуры
            Console.Write("Введите фигуру: ");
            string? input = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(input) || input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Выход из программы.");
                break;
            }

            if (!Enum.TryParse(input, true, out FigureType figure))
            {
                Console.WriteLine("Неизвестная фигура! Попробуйте снова.\n");
                continue;
            }

            // ввод начальной позиции
            Console.Write("Начальная позиция (например, E2): ");
            string? fromInput = Console.ReadLine()?.Trim();
            if (!CheckerBoardPosition.TryParse(fromInput, null, out var from))
            {
                Console.WriteLine("Некорректная позиция! Попробуйте снова.\n");
                continue;
            }

            // ввод конечной позиции
            Console.Write("Конечная позиция (например, E4): ");
            string? toInput = Console.ReadLine()?.Trim();
            if (!CheckerBoardPosition.TryParse(toInput, null, out var to))
            {
                Console.WriteLine("Некорректная позиция! Попробуйте снова.\n");
                continue;
            }

            // проверка хода
            bool valid = ChessRules.IsValidMove(figure, from!, to!);
            Console.WriteLine($"\nХод {figure} из {from} в {to} — {(valid ? "допустим" : "недопустим")}\n");
        }
    }
}
