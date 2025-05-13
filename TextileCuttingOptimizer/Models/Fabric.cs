using System.Collections.Generic;
using System.Windows;

namespace TextileCuttingOptimizer.Models
{
    public class Fabric
    {
        public int Id { get; set; }  // Уникальный идентификатор

        // Название ткани (хлопок, сатин и т.д.)
        public string Name { get; set; }

        // Ширина рулона в сантиметрах
        public double Width { get; set; }

        // Длина рулона в метрах
        public double Length { get; set; }

        // Дефекты ткани (координаты)
        public List<Point> Defects { get; set; } = new List<Point>();
    }
}