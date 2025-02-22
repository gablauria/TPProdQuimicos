using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Windows;

namespace TP2_Prod_Quimicos_GrupoL
{
    public partial class StockWindow : Window
    {
        private List<ProductoQuimico> productos;

        public StockWindow(List<ProductoQuimico> productos)
        {
            InitializeComponent();
            this.productos = productos;
            CargarProductos();
        }

        private void CargarProductos()
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["MiConexion"].ConnectionString;

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                string query = "SELECT codigo, nombre, laboratorio, especie, precio, stock, fechavenc, muestras, permiso, licencia, ambienterefrig, ventalibre " +
                               "FROM dbo.Productos";

                SqlCommand comando = new SqlCommand(query, conexion);
                conexion.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    List<ProductoQuimico> listaProductos = new List<ProductoQuimico>();
                    while (reader.Read())
                    {
                        listaProductos.Add(new ProductoQuimico
                        {
                            Codigo = reader.GetInt32(0),
                            NombreProducto = reader.GetString(1),
                            Laboratorio = reader.GetString(2),
                            Especie = reader.GetString(3),
                            Precio = (float)(reader.IsDBNull(4) ? 0 : reader.GetDouble(4)),
                            Stock = reader.GetInt32(5),
                            FechaVencimiento = reader.IsDBNull(6) ? DateTime.MinValue : reader.GetDateTime(6),
                            Muestras = reader.GetBoolean(7),
                            Permiso = reader.GetBoolean(8),
                            Licencia = reader.GetBoolean(9),
                            AmbienteRefrigerado = reader.GetBoolean(10),
                            VentaLibre = reader.GetBoolean(11),
                        });
                    }

                    DataGridStock.ItemsSource = listaProductos;
                }
            }
        }

        private void EditarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridStock.SelectedItem is ProductoQuimico productoSeleccionado)
            {
                EditProductWindow editWindow = new EditProductWindow(productoSeleccionado);
                editWindow.ShowDialog();

                // Recargar los productos después de la edición
                CargarProductos();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un producto para editar.");
            }
        }

        private void EliminarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridStock.SelectedItem is ProductoQuimico productoSeleccionado)
            {
                // Confirmación
                var result = MessageBox.Show("¿Estás seguro de que deseas eliminar este producto?",
                                             "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // Eliminar el producto de la base de datos
                    EliminarProductoDeBD(productoSeleccionado.Codigo);
                    MessageBox.Show("Producto eliminado correctamente.");

                    // Recargar los productos después de la eliminación
                    CargarProductos();
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un producto para eliminar.");
            }
        }

        private void EliminarProductoDeBD(int codigoProducto)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["MiConexion"].ConnectionString;

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                string query = "DELETE FROM dbo.Productos WHERE codigo = @Codigo";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Codigo", codigoProducto);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }
    }
}
