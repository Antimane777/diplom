using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TextileCuttingOptimizer.Models;

namespace TextileCuttingOptimizer.Services
{
    public class CuttingService
    {
        public List<PlacedPattern> CalculateLayout(Fabric fabric, List<Pattern> patterns)
        {
            // Сортируем лекала по убыванию площади
            var sortedPatterns = patterns.OrderByDescending(p => GetArea(p.Contour)).ToList();
            var placedPatterns = new List<PlacedPattern>();
            double fabricWidth = fabric.Width;
            double maxY = 0;

            foreach (var pattern in sortedPatterns)
            {
                var (bestX, bestY) = FindBestPosition(pattern, placedPatterns, fabricWidth);

                if (bestY + GetHeight(pattern.Contour) > fabric.Length * 100)
                    throw new Exception("Недостаточно длины ткани");

                placedPatterns.Add(new PlacedPattern
                {
                    Pattern = pattern,
                    Position = new Point(bestX, bestY)
                });

                maxY = Math.Max(maxY, bestY + GetHeight(pattern.Contour));
            }

            return placedPatterns;
        }

        private (double x, double y) FindBestPosition(Pattern pattern, List<PlacedPattern> placedPatterns, double fabricWidth)
        {
            double patternWidth = GetWidth(pattern.Contour);
            double patternHeight = GetHeight(pattern.Contour);

            // Пробуем разместить вплотную к левому краю
            for (double y = 0; ; y += 5) // Шаг 5 см для оптимизации
            {
                for (double x = 0; x <= fabricWidth - patternWidth; x += 5)
                {
                    if (CanPlace(pattern, x, y, placedPatterns, fabricWidth))
                    {
                        return (x, y);
                    }
                }

                // Защита от бесконечного цикла
                if (y > 10000) throw new Exception("Не удалось разместить лекало");
            }
        }

        private bool CanPlace(Pattern pattern, double x, double y,
                            List<PlacedPattern> placedPatterns, double fabricWidth)
        {
            double patternWidth = GetWidth(pattern.Contour);
            double patternHeight = GetHeight(pattern.Contour);

            // Проверка выхода за границы ткани
            if (x + patternWidth > fabricWidth)
                return false;

            // Проверка пересечений с другими лекалами
            foreach (var placed in placedPatterns)
            {
                if (CheckCollision(
                    x, y, patternWidth, patternHeight,
                    placed.Position.X, placed.Position.Y,
                    GetWidth(placed.Pattern.Contour),
                    GetHeight(placed.Pattern.Contour)))
                {
                    return false;
                }
            }

            return true;
        }

        // Вспомогательные методы
        private double GetWidth(List<Point> contour) => contour.Max(p => p.X) - contour.Min(p => p.X);
        private double GetHeight(List<Point> contour) => contour.Max(p => p.Y) - contour.Min(p => p.Y);
        private double GetArea(List<Point> contour) => GetWidth(contour) * GetHeight(contour);

        private bool CheckCollision(double x1, double y1, double w1, double h1,
                                  double x2, double y2, double w2, double h2)
        {
            return x1 < x2 + w2 && x1 + w1 > x2 &&
                   y1 < y2 + h2 && y1 + h1 > y2;
        }
    }

    public class PlacedPattern
    {
        public Pattern Pattern { get; set; }
        public Point Position { get; set; }
    }
}
