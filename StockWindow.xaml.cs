using System.Collections.Generic;
using System.Windows;

namespace TP2_Prod_Quimicos_GrupoL
{
    public partial class StockWindow : Window
    {
        public StockWindow(List<ProductoQuimico> productos)
        {
            InitializeComponent();
            StockListBox.ItemsSource = productos;
        }
    }
}
