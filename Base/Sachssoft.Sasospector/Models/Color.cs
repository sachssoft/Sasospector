using System;

namespace Sachssoft.Sasospector.Models
{
    public readonly struct Color : IEquatable<Color>
    {
        public byte Alpha { get; }
        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }

        public Color(byte red, byte green, byte blue, byte alpha = 255)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        public override string ToString()
            => Alpha == 255
                ? $"#{Red:X2}{Green:X2}{Blue:X2}"
                : $"#{Alpha:X2}{Red:X2}{Green:X2}{Blue:X2}";

        public static bool TryParse(string? text, out Color color)
        {
            color = default;

            if (string.IsNullOrWhiteSpace(text))
                return false;

            text = text.Trim();

            if (!text.StartsWith("#"))
                return false;

            text = text[1..];

            return text.Length switch
            {
                6 => TryParseRgb(text, out color),
                8 => TryParseArgb(text, out color),
                _ => false
            };
        }

        private static bool TryParseRgb(string hex, out Color color)
        {
            color = default;

            if (!TryByte(hex, 0, out var r)) return false;
            if (!TryByte(hex, 2, out var g)) return false;
            if (!TryByte(hex, 4, out var b)) return false;

            color = new Color(r, g, b);
            return true;
        }

        private static bool TryParseArgb(string hex, out Color color)
        {
            color = default;

            if (!TryByte(hex, 0, out var a)) return false;
            if (!TryByte(hex, 2, out var r)) return false;
            if (!TryByte(hex, 4, out var g)) return false;
            if (!TryByte(hex, 6, out var b)) return false;

            color = new Color(r, g, b, a);
            return true;
        }

        private static bool TryByte(string hex, int index, out byte value)
        {
            value = 0;

            return byte.TryParse(
                hex.AsSpan(index, 2),
                System.Globalization.NumberStyles.HexNumber,
                null,
                out value);
        }

        public bool Equals(Color other)
            => Alpha == other.Alpha
            && Red == other.Red
            && Green == other.Green
            && Blue == other.Blue;

        public override bool Equals(object? obj)
            => obj is Color other && Equals(other);

        public override int GetHashCode()
            => HashCode.Combine(Alpha, Red, Green, Blue);
    }
}