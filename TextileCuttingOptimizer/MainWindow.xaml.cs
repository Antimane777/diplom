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
            var vm = (MainViewWindow_mock)DataContext;

            // Создаем лекала на основе выбора
            var patterns = new List<Pattern>();

            // Добавляем наволочки
            for (int i = 0; i < vm.PillowcaseCount; i++)
            {
                patterns.Add(CreatePillowcase(vm.SelectedPillowcaseSize));
            }

            // Добавляем пододеяльник
            if (vm.DuvetCoverCount > 0)
            {
                patterns.Add(CreateDuvetCover(vm.SelectedDuvetCoverSize));
            }

            // Визуализация
            DrawPatterns(patterns);
        }

        private Pattern CreatePillowcase(string size)
        {
            switch (size)
            {
                case "50×50 см":
                    return new Pattern { Name = "Наволочка 50×50", Contour = CreateRectangle(50, 50) };
                case "50×70 см":
                    return new Pattern { Name = "Наволочка 50×70", Contour = CreateRectangle(50, 70) };
                case "70×70 см":
                    return new Pattern { Name = "Наволочка 70×70", Contour = CreateRectangle(70, 70) };
                default:
                    throw new ArgumentException("Неизвестный размер");
            }
        }

        private Pattern CreateDuvetCover(string size)
        {
            switch (size)
            {
                case "1.5-спальный":
                    return new Pattern { Name = "Пододеяльник 1.5сп", Contour = CreateRectangle(150, 200) };
                case "2-спальный":
                    return new Pattern { Name = "Пододеяльник 2сп", Contour = CreateRectangle(180, 220) };
                case "Евро":
                    return new Pattern { Name = "Пододеяльник Евро", Contour = CreateRectangle(220, 240) };
                default:
                    throw new ArgumentException("Неизвестный размер");
            }
        }

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
            CuttingCanvas.Children.Clear();
            double currentY = 10;

            foreach (var pattern in patterns)
            {
                var polygon = new Polygon
                {
                    Points = new PointCollection(pattern.Contour),
                    Fill = Brushes.LightBlue,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };

                Canvas.SetLeft(polygon, 50);
                Canvas.SetTop(polygon, currentY);
                currentY += pattern.Contour.Max(p => p.Y) + 10;

                CuttingCanvas.Children.Add(polygon);

                // Подпись
                var label = new TextBlock
                {
                    Text = pattern.Name,
                    Margin = new Thickness(55, currentY - 20, 0, 0)
                };
                CuttingCanvas.Children.Add(label);
            }
        }
    }
}
