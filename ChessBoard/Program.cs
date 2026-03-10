using System;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

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

    
}