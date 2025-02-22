using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TP2_Prod_Quimicos_GrupoL
{
    public partial class MainWindow : Window
    {
        private List<ProductoQuimico> productos = new List<ProductoQuimico>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string nombreProducto = NombreProducto.Text ?? string.Empty;
            string codigoProducto = CodigoProducto.Text ?? string.Empty;
            string laboratorio = Laboratorio.Text ?? string.Empty;

            if (string.IsNullOrWhiteSpace(nombreProducto))
            {
                MessageBox.Show("El nombre del producto es obligatorio.");
                return;
            }

            if (nombreProducto.Length > 10)
            {
                MessageBox.Show("El nombre del producto no debe exceder los 10 dígitos.");
                return;
            }

            if (string.IsNullOrWhiteSpace(codigoProducto))
            {
                MessageBox.Show("El código del producto es obligatorio.");
                return;
            }

            if (codigoProducto.Length > 7)
            {
                MessageBox.Show("El código del producto no debe exceder los 7 dígitos.");
                return;
            }

            if (string.IsNullOrWhiteSpace(laboratorio))
            {
                MessageBox.Show("El nombre del laboratorio es obligatorio.");
                return;
            }

            if (laboratorio.Length > 25)
            {
                MessageBox.Show("El nombre del laboratorio no debe exceder los 25 dígitos.");
                return;
            }

            string especie = (Especie.SelectedItem as ComboBoxItem)?.Content.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(especie))
            {
                MessageBox.Show("Debe seleccionar una especie.");
                return;
            }

            if (string.IsNullOrWhiteSpace(PrecioVenta.Text) || !decimal.TryParse(PrecioVenta.Text, out decimal precioVenta))
            {
                MessageBox.Show("El precio de venta es obligatorio y debe ser un número válido.");
                return;
            }

            if (string.IsNullOrWhiteSpace(CantidadStock.Text) || !int.TryParse(CantidadStock.Text, out int cantidadStock))
            {
                MessageBox.Show("La cantidad en stock es obligatoria y debe ser un número válido.");
                return;
            }

            DateTime fechaVencimiento = FechaVencimiento.SelectedDate ?? DateTime.MinValue;
            if (fechaVencimiento == DateTime.MinValue)
            {
                MessageBox.Show("Debe seleccionar una fecha de vencimiento válida.");
                return;
            }

            bool muestras = Muestras.IsChecked ?? false;
            bool permiso = Permiso.IsChecked ?? false;
            bool licencia = Licencia.IsChecked ?? false;
            bool ambienteRefrigerado = AmbienteRefrigerado.IsChecked ?? false;
            bool ventaLibre = VentaLibre.IsChecked ?? false;

            var producto = new ProductoQuimico
            {
                NombreProducto = nombreProducto,
                CodigoProducto = codigoProducto,
                Laboratorio = laboratorio,
                Especie = especie,
                PrecioVenta = precioVenta,
                CantidadStock = cantidadStock,
                FechaVencimiento = fechaVencimiento,
                Muestras = muestras,
                Permiso = permiso,
                Licencia = licencia,
                AmbienteRefrigerado = ambienteRefrigerado,
                VentaLibre = ventaLibre
            };

            productos.Add(producto);
            MessageBox.Show("Datos guardados correctamente.");

            // Limpiar los campos después de guardar los datos
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            NombreProducto.Clear();
            CodigoProducto.Clear();
            Laboratorio.Clear();
            Especie.SelectedIndex = -1;
            PrecioVenta.Clear();
            CantidadStock.Clear();
            FechaVencimiento.SelectedDate = null;
            Muestras.IsChecked = false;
            Permiso.IsChecked = false;
            Licencia.IsChecked = false;
            AmbienteRefrigerado.IsChecked = false;
            VentaLibre.IsChecked = false;
        }

        private void MostrarStock_Click(object sender, RoutedEventArgs e)
        {
            StockWindow stockWindow = new StockWindow(productos);
            stockWindow.Show();
        }
    }

    public class ProductoQuimico
    {
        public string NombreProducto { get; set; }
        public string CodigoProducto { get; set; }
        public string Laboratorio { get; set; }
        public string Especie { get; set; }
        public decimal PrecioVenta { get; set; }
        public int CantidadStock { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public bool Muestras { get; set; }
        public bool Permiso { get; set; }
        public bool Licencia { get; set; }
        public bool AmbienteRefrigerado { get; set; }
        public bool VentaLibre { get; set; }

        public decimal StockValorizado => CantidadStock * PrecioVenta;
    }
}
