using System;
using System.Linq;
using Microsoft.Data.SqlClient; // Asegúrate de usar Microsoft.Data.SqlClient
using System.Windows;
using System.Windows.Controls;
using System.Configuration;

namespace TP2_Prod_Quimicos_GrupoL
{
    public partial class EditProductWindow : Window
    {
        private ProductoQuimico producto;

        public EditProductWindow(ProductoQuimico producto)
        {
            InitializeComponent();
            this.producto = producto;
            CargarDatos();
        }

        private void CargarDatos()
        {
            NombreProducto.Text = producto.NombreProducto;
            Laboratorio.Text = producto.Laboratorio;

            // Asignar el ComboBoxItem correspondiente
            var especieItem = Especie.Items.Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == producto.Especie);
            Especie.SelectedItem = especieItem;

            PrecioVenta.Text = producto.Precio.ToString();
            CantidadStock.Text = producto.Stock.ToString();
            FechaVencimiento.SelectedDate = producto.FechaVencimiento;
            Muestras.IsChecked = producto.Muestras;
            Permiso.IsChecked = producto.Permiso;
            Licencia.IsChecked = producto.Licencia;
            AmbienteRefrigerado.IsChecked = producto.AmbienteRefrigerado;
            VentaLibre.IsChecked = producto.VentaLibre;
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
                MessageBox.Show("El nombre del producto no debe exceder los 10 caracteres.");
                return;
            }

            if (string.IsNullOrWhiteSpace(laboratorio))
            {
                MessageBox.Show("El nombre del laboratorio es obligatorio.");
                return;
            }

            if (laboratorio.Length > 25)
            {
                MessageBox.Show("El nombre del laboratorio no debe exceder los 25 caracteres.");
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

            if (precioVenta < 0)
            {
                MessageBox.Show("El precio de venta no puede ser negativo.");
                return;
            }

            if (string.IsNullOrWhiteSpace(CantidadStock.Text) || !int.TryParse(CantidadStock.Text, out int cantidadStock))
            {
                MessageBox.Show("La cantidad en stock es obligatoria y debe ser un número válido.");
                return;
            }

            if (cantidadStock < 0)
            {
                MessageBox.Show("La cantidad en stock no puede ser negativa.");
                return;
            }

            // No se realiza validación de fecha futura en la fecha de vencimiento
            DateTime fechaVencimiento = FechaVencimiento.SelectedDate ?? DateTime.MinValue;

            // Actualizar el objeto producto
            producto.NombreProducto = nombreProducto;
            producto.Laboratorio = laboratorio;
            producto.Especie = especie;
            producto.Precio = precioVenta;
            producto.Stock = cantidadStock;
            producto.FechaVencimiento = fechaVencimiento;
            producto.Muestras = Muestras.IsChecked ?? false;
            producto.Permiso = Permiso.IsChecked ?? false;
            producto.Licencia = Licencia.IsChecked ?? false;
            producto.AmbienteRefrigerado = AmbienteRefrigerado.IsChecked ?? false;
            producto.VentaLibre = VentaLibre.IsChecked ?? false;

            // Guardar los cambios en la base de datos
            ActualizarProducto(producto);
            MessageBox.Show("Datos actualizados correctamente.");
            Close(); // Cerrar la ventana después de guardar
        }

        private void ActualizarProducto(ProductoQuimico producto)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["MiConexion"].ConnectionString;

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                string query = "UPDATE dbo.Productos SET nombre = @Nombre, laboratorio = @Laboratorio, especie = @Especie, " +
                               "precio = @Precio, stock = @Stock, fechavenc = @FechaVencimiento, muestras = @Muestras, " +
                               "permiso = @Permiso, licencia = @Licencia, ambienterefrig = @AmbienteRefrigerado, ventalibre = @VentaLibre " +
                               "WHERE codigo = @Codigo";

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
                    comando.Parameters.AddWithValue("@Codigo", producto.Codigo); // Agregar el código del producto

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }
    }
}
