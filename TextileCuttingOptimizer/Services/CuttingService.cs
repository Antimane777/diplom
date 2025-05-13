using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TextileCuttingOptimizer.Models;

namespace TextileCuttingOptimizer.Services
{
    public class CuttingService
    {
        public List<Pattern> CalculateLayout(Fabric fabric, List<Pattern> patterns)
        {
            double currentY = 0;

            foreach (var pattern in patterns)
            {
                // Простейший алгоритм - размещение друг под другом
                pattern.Position = new Point(0, currentY);

                // Учет припусков на швы
                double patternHeight = pattern.Contour.Max(p => p.Y) + pattern.SeamAllowance * 2;
                currentY += patternHeight;

                // Проверка на выход за границы ткани
                if (currentY > fabric.Length * 100) // Конвертация метров в см
                {
                    throw new Exception("Недостаточно длины ткани");
                }
            }

            return patterns;
        }
    }
}
