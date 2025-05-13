using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace TextileCuttingOptimizer.Models
{
    public static class PointExtensions
    {
        // Метод для смещения контура
        public static List<Point> Shift(this List<Point> contour, Point delta)
        {
            return contour.Select(p => new Point(p.X + delta.X, p.Y + delta.Y)).ToList();
        }

        // Метод для вычисления габаритов
        public static Size GetBounds(this List<Point> contour)
        {
            double width = contour.Max(p => p.X) - contour.Min(p => p.X);
            double height = contour.Max(p => p.Y) - contour.Min(p => p.Y);
            return new Size(width, height);
        }
    }
}