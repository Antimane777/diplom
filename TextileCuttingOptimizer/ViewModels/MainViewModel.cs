//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TextileCuttingOptimizer.Data;
//using TextileCuttingOptimizer.Models;
//using TextileCuttingOptimizer.Services;

//namespace TextileCuttingOptimizer.ViewModels
//{
//    public class MainViewModel
//    {
//        public ObservableCollection<Fabric> Fabrics { get; set; }
//        public ObservableCollection<Pattern> Patterns { get; set; }

//        public Fabric SelectedFabric { get; set; }
//        public Pattern SelectedPattern { get; set; }

//        public int PillowcaseCount { get; set; } = 2;
//        public int DuvetCoverCount { get; set; } = 1;
//        public int SheetCount { get; set; } = 1;
//        public string SelectedDuvetCoverSize { get; internal set; }
//        public string SelectedPillowcaseSize { get; internal set; }

//        private readonly DatabaseService _db;
//        private readonly CuttingService _cuttingService;

//        public MainViewModel()
//        {
//            _db = new DatabaseService();
//            _cuttingService = new CuttingService();

//            // Загрузка данных из БД
//            Fabrics = new ObservableCollection<Fabric>(_db.Fabrics.ToList());
//            Patterns = new ObservableCollection<Pattern>(_db.Patterns.ToList());
//        }

//        public List<Pattern> CalculateCutting()
//        {
//            // Формируем список лекал с учетом количества
//            var patternsToCut = new List<Pattern>();

//            if (PillowcaseCount > 0)
//            {
//                var pillowcase = Patterns.First(p => p.Name.Contains("Наволочка"));
//                patternsToCut.AddRange(Enumerable.Repeat(pillowcase, PillowcaseCount));
//            }

//            // Аналогично для других элементов

//            return _cuttingService.CalculateLayout(SelectedFabric, patternsToCut);
//        }
//    }
//}
