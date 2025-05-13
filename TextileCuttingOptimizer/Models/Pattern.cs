using System.Collections.Generic;
using System.Windows;

namespace TextileCuttingOptimizer.Models
{
    public class Pattern
    {
        public int Id { get; set; }

        // Тип элемента (пододеяльник, простыня и т.д.)
        public string Name { get; set; } 

        // Координаты контура лекала
        public List<Point> Contour { get; set; } = new List<Point>();

        // Припуск на шов в см
        public double SeamAllowance { get; set; }

        // Позиция на ткани (заполнится после расчета)
        public Point Position { get; set; } = new Point(0, 0);
    }
}