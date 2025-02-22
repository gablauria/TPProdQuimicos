using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;
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
            // Validaciones de entrada
            string nombreProducto = NombreProducto.Text ?? string.Empty;
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

            if (string.IsNullOrWhiteSpace(PrecioVenta.Text) || !float.TryParse(PrecioVenta.Text, out float precioVenta))
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
                Laboratorio = laboratorio,
                Especie = especie,
                Precio = precioVenta,
                Stock = cantidadStock,
                FechaVencimiento = fechaVencimiento,
                Muestras = muestras,
                Permiso = permiso,
                Licencia = licencia,
                AmbienteRefrigerado = ambienteRefrigerado,
                VentaLibre = ventaLibre
            };

            // Guardar el producto en la base de datos
            GuardarProducto(producto);
            productos.Add(producto); // Agregar el producto a la lista
            MessageBox.Show("Datos guardados correctamente.");

            // Limpiar los campos después de guardar los datos
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            NombreProducto.Clear();
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
            stockWindow.ShowDialog(); // Cambiado a ShowDialog para esperar a que se cierre
            // Aquí podrías recargar los datos si es necesario
        }

        private void GuardarProducto(ProductoQuimico producto)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["MiConexion"].ConnectionString;

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                string query = "INSERT INTO dbo.Productos (nombre, laboratorio, especie, precio, stock, fechavenc, muestras, permiso, licencia, ambienterefrig, ventalibre) " +
                               "VALUES (@Nombre, @Laboratorio, @Especie, @Precio, @Stock, @FechaVencimiento, @Muestras, @Permiso, @Licencia, @AmbienteRefrigerado, @VentaLibre)";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Nombre", producto.NombreProducto);
                    comando.Parameters.AddWithValue("@Laboratorio", producto.Laboratorio);
                    comando.Parameters.AddWithValue("@Especie", producto.Especie);
                    comando.Parameters.AddWithValue("@Precio", producto.Precio);
                    comando.Parameters.AddWithValue("@Stock", producto.Stock);
                    comando.Parameters.AddWithValue("@FechaVencimiento", producto.FechaVencimiento);
                    comando.Parameters.AddWithValue("@Muestras", producto.Muestras);
                    comando.Parameters.AddWithValue("@Permiso", producto.Permiso);
                    comando.Parameters.AddWithValue("@Licencia", producto.Licencia);
                    comando.Parameters.AddWithValue("@AmbienteRefrigerado", producto.AmbienteRefrigerado);
                    comando.Parameters.AddWithValue("@VentaLibre", producto.VentaLibre);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }
    }

    public class ProductoQuimico
    {
        public int Codigo { get; set; } // PK, se asigna automáticamente
        public string NombreProducto { get; set; }
        public string Laboratorio { get; set; }
        public string Especie { get; set; }
        public float Precio { get; set; }
        public int Stock { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public bool Muestras { get; set; }
        public bool Permiso { get; set; }
        public bool Licencia { get; set; }
        public bool AmbienteRefrigerado { get; set; }
        public bool VentaLibre { get; set; }

        // Nueva propiedad para el stock valorizado
        public float StockValorizado => Precio * Stock;
    }
}
