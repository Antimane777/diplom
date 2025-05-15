using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TextileCuttingOptimizer.Models;
using TextileCuttingOptimizer.Services;
using TextileCuttingOptimizer.ViewModels;

namespace TextileCuttingOptimizer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private readonly MainViewModel _viewModel;

        //public MainWindow()
        //{
        //    InitializeComponent();
        //    _viewModel = new MainViewModel();
        //    DataContext = _viewModel;
        //}

        //private void DrawPatterns(IEnumerable<Pattern> patterns)
        //{
        //    CuttingCanvas.Children.Clear();

        //    foreach (var pattern in patterns)
        //    {
        //        var polygon = new Polygon
        //        {
        //            Points = new PointCollection(pattern.Contour),
        //            Fill = Brushes.LightBlue,
        //            Stroke = Brushes.Black,
        //            StrokeThickness = 1
        //        };

        //        Canvas.SetLeft(polygon, pattern.Position.X);
        //        Canvas.SetTop(polygon, pattern.Position.Y);

        //        CuttingCanvas.Children.Add(polygon);
        //    }
        //}

        //private void Calculate_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var result = _viewModel.CalculateCutting();
        //        DrawPatterns(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        public MainWindow()
        {
            InitializeComponent();
        }
        

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            // Получаем ViewModel
            var viewModel = (MainViewWindow_mock)DataContext;
            try
            {
                var fabric = new Fabric { Width = 220, Length = 10 }; // Сатин 220см
                var patterns = new List<Pattern>();

                // Создаем лекала на основе выбранных параметров
                for (int i = 0; i < viewModel.PillowcaseCount; i++)
                {
                    patterns.Add(new Pattern
                    {
                        Name = "Наволочка " + viewModel.SelectedPillowcaseSize,
                        Contour = CreateRectangle(50, 70),
                        SeamAllowance = 1.5
                    });
                }

                for (int i = 0; i < viewModel.DuvetCoverCount; i++)
                {
                    patterns.Add(new Pattern
                    {
                        Name = "Пододеяльник " + viewModel.SelectedDuvetCoverSize,
                        Contour = CreateRectangle(180, 220),
                        SeamAllowance = 2.0
                    });
                }

                for (int i = 0; i < viewModel.SheetCount; i++)
                {
                    patterns.Add(new Pattern
                    {
                        Name = "Простыня",
                        Contour = CreateRectangle(200, 240),
                        SeamAllowance = 2.5
                    });
                }

                var result = new CuttingService().CalculateLayout(fabric, patterns);
                DrawPatterns(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void DrawPatterns(List<PlacedPattern> placedPatterns)
        {
            CuttingCanvas.Children.Clear();

            // Отрисовка рулона ткани
            var fabricVisual = new Rectangle
            {
                Width = 220, // Ширина сатина
                Height = 1000, // Длина для визуализации
                Fill = Brushes.Beige,
                Stroke = Brushes.Gray
            };
            CuttingCanvas.Children.Add(fabricVisual);

            // Отрисовка лекал
            foreach (var item in placedPatterns)
            {
                var polygon = new Polygon
                {
                    Points = OffsetPoints(item.Pattern.Contour, item.Position),
                    Fill = GetPatternColor(item.Pattern.Name),
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                CuttingCanvas.Children.Add(polygon);

                // Подпись
                var label = new TextBlock
                {
                    Text = item.Pattern.Name,
                    Margin = new Thickness(item.Position.X + 5, item.Position.Y + 5, 0, 0),
                    FontSize = 10,
                    Foreground = Brushes.DarkSlateGray
                };
                CuttingCanvas.Children.Add(label);
            }
        }

        private PointCollection OffsetPoints(List<Point> contour, Point offset)
        {
            var result = new PointCollection();
            foreach (var point in contour)
            {
                result.Add(new Point(point.X + offset.X, point.Y + offset.Y));
            }
            return result;
        }

        private Brush GetPatternColor(string name)
        {
            if (name.Contains("Наволочка")) return Brushes.LightBlue;
            if (name.Contains("Пододеяльник")) return Brushes.LightGreen;
            if (name.Contains("Простыня")) return Brushes.LightSalmon;
            return Brushes.LightGray;
        }
        // Вспомогательный метод для создания прямоугольного лекала
        private List<Point> CreateRectangle(double width, double height)
        {
            return new List<Point>
            {
                new Point(0, 0),
                new Point(width, 0),
                new Point(width, height),
                new Point(0, height)
            };
        }

        private void DrawPatterns(List<Pattern> patterns)
        {
            // Очищаем холст
            CuttingCanvas.Children.Clear();

            // Проверка на пустоту
            if (patterns == null || patterns.Count == 0)
            {
                MessageBox.Show("Нет лекал для отображения");
                return;
            }

            double currentY = 20; // Начальная позиция по Y
            double padding = 20;  // Отступ от краев

            // 1. Отрисовываем "рулон ткани"
            var fabricWidth = ((MainViewWindow_mock)DataContext).SelectedFabric?.Width ?? 150;
            var fabricVisual = new Rectangle
            {
                Width = fabricWidth + padding * 2,
                Height = CuttingCanvas.ActualHeight,
                Fill = Brushes.Beige,
                Stroke = Brushes.Gray,
                StrokeThickness = 1
            };
            CuttingCanvas.Children.Add(fabricVisual);

            // 2. Отрисовываем все лекала
            foreach (var pattern in patterns)
            {
                // Пропускаем некорректные лекала
                if (pattern.Contour == null || pattern.Contour.Count < 3)
                    continue;

                // Создаем полигон для лекала
                var polygon = new Polygon
                {
                    Points = new PointCollection(pattern.Contour),
                    Fill = GetPatternBrush(pattern.Name),
                    Stroke = Brushes.Black,
                    StrokeThickness = 0.5,
                    ToolTip = CreateToolTip(pattern)
                };

                // Позиционируем на холсте
                Canvas.SetLeft(polygon, padding);
                Canvas.SetTop(polygon, currentY);

                // Добавляем на холст
                CuttingCanvas.Children.Add(polygon);

                // Добавляем подпись
                var label = new TextBlock
                {
                    Text = pattern.Name,
                    Margin = new Thickness(padding + 5, currentY + 5, 0, 0),
                    FontSize = 10,
                    Foreground = Brushes.DarkSlateGray,
                    FontWeight = FontWeights.Bold
                };
                CuttingCanvas.Children.Add(label);

                // Смещаем позицию для следующего лекала
                currentY += pattern.Contour.Max(p => p.Y) + 15;
            }
        }

        private Brush GetPatternBrush(string patternName)
        {
            if (patternName.Contains("Наволочка")) return Brushes.LightBlue;
            if (patternName.Contains("Пододеяльник")) return Brushes.LightGreen;
            if (patternName.Contains("Простыня")) return Brushes.LightSalmon;
            return Brushes.LightGray;
        }

        private ToolTip CreateToolTip(Pattern pattern)
        {
            var tooltip = new ToolTip();
            var stack = new StackPanel();

            stack.Children.Add(new TextBlock
            {
                Text = pattern.Name,
                FontWeight = FontWeights.Bold
            });

            stack.Children.Add(new TextBlock
            {
                Text = $"Размер: {GetSizeString(pattern.Contour)}"
            });

            tooltip.Content = stack;
            return tooltip;
        }

        private string GetSizeString(List<Point> contour)
        {
            double width = contour.Max(p => p.X) - contour.Min(p => p.X);
            double height = contour.Max(p => p.Y) - contour.Min(p => p.Y);
            return $"{width} × {height} см";
        }

    }
}
