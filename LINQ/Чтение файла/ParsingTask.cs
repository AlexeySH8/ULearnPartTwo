using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace linq_slideviews;

public class ParsingTask
{
    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
    /// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
    /// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
    public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
    {
        return lines
            .Skip(1)
            .Where(line => Regex.IsMatch(line, @"^\d+;(theory|quiz|exercise);[^;]+$"))
            .Select(line => line.Split(";"))
            .ToDictionary(line => int.Parse(line[0]),
            line => new SlideRecord(int.Parse(line[0]),
            (SlideType)Enum.Parse(typeof(SlideType), char.ToUpper(line[1][0]) +
            line[1].Substring(1)), line[2]));
    }

    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
    /// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
    /// Такой словарь можно получить методом ParseSlideRecords</param>
    /// <returns>Список информации о посещениях</returns>
    /// <exception cref="FormatException">Если среди строк есть некорректные</exception>
    public static IEnumerable<VisitRecord> ParseVisitRecords(
        IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
    {
        return lines
            .Skip(1)
            .Select(line => line.Split(";"))
            .Select(part =>
            {
                try
                {
                    return new VisitRecord(int.Parse(part[0]), int.Parse(part[1]),
                    DateTime.ParseExact(part[2] + " " + part[3], "yyyy-MM-dd HH:mm:ss",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None),
                    slides[int.Parse(part[1])].SlideType);
                }
                catch (Exception e)
                {
                    throw new FormatException($"Wrong line [{string.Join(";", part)}]", e);
                }
            });
    }
}