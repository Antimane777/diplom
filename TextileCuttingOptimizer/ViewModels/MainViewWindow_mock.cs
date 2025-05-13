using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextileCuttingOptimizer.Models;

namespace TextileCuttingOptimizer.ViewModels
{
    public class MainViewWindow_mock
    {
        public ObservableCollection<Fabric> Fabrics { get; set; }
        public ObservableCollection<string> PillowcaseSizes { get; set; }
        public ObservableCollection<string> DuvetCoverSizes { get; set; }

        public Fabric SelectedFabric { get; set; }
        public string SelectedPillowcaseSize { get; set; }
        public string SelectedDuvetCoverSize { get; set; }
        public int PillowcaseCount { get; set; } = 2;
        public int DuvetCoverCount { get; set; } = 1;
        public int SheetCount { get; set; } = 1;

        public MainViewWindow_mock()
        {
            // Заглушки для тканей
            Fabrics = new ObservableCollection<Fabric>
            {
                new Fabric { Name = "Хлопок (150см)", Width = 150 },
                new Fabric { Name = "Сатин (220см)", Width = 220 }
            };

            // Заглушки для размеров
            PillowcaseSizes = new ObservableCollection<string>
            {
                "50×50 см",
                "50×70 см",
                "70×70 см"
            };

            DuvetCoverSizes = new ObservableCollection<string>
            {
                "1.5-спальный",
                "2-спальный",
                "Евро"
            };

            SelectedPillowcaseSize = PillowcaseSizes[1]; // 50×70 по умолчанию
            SelectedDuvetCoverSize = DuvetCoverSizes[0]; // 1.5-спальный по умолчанию
        }
    }
}
