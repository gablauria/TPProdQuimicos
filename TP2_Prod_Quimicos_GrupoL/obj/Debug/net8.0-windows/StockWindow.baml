   M S B A M L   `   `   `  ����  TP2_Prod_Quimicos_GrupoL*    $TP2_Prod_Quimicos_GrupoL.StockWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Stock de Productos��5   	   $	��400��6$   $	��600��61   .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5	      $
��Auto �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ DataGridStock��5         AutoGenerateColumns$ False��6*   $��10q�6F      SelectionMode$ Single=�6R   $��0��6i      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Código��6%      
IsReadOnly$
 True��6P   1  +System.Windows.Controls.DataGridBoundColumn   Binding 65   �� *Codigo+6P     5      $ Nombre��6%   $
 True��6X    65   �� *NombreProducto+6X     5      $ Laboratorio��6%   $
 True��6Z    6:   �� *Laboratorio+6Z     5      $ Especie��6%   $
 True��6R    66   �� *	Especie+6R     5      $ Precio��6%   $
 True��6P    65   �� *Precio+6P     5      $ Stock��6%   $
 True��6N    64   �� *Stock+6N     5      $ Fecha de Vencimiento��6%   $
 True��6�    6C   �� *FechaVencimiento+	 �� StringFormat$	 
dd/MM/yyyy��6�     5      $ Stock Valorizado��6%   $
 True��6c    6?   �� *StockValorizado+6c   4  .System.Windows.Controls.DataGridTemplateColumn  5      $ Muestras��6)   
   CellTemplate
 5      �� 5      .R�5      �� ${�False��6F    d� 	IsChecked 6'   �� *
Muestras+6F   5      5        5      $ Permiso��6)   
 5      �� 5       .R�5!      �� ${�False��6E    6'   �� *	Permiso+6E   5"      5$        5%      $ Licencia��6)   
 5&      �� 5'      .R�5(      �� ${�False��6F    6'   �� *
Licencia+6F   5)      5+        5,      $ Ambiente Refrigerado��6)   
 5-      �� 5.      .R�5/      �� ${�False��6Q    6'   �� *AmbienteRefrigerado+6Q   50      52        53      $ Venta Libre��6)   
 54      �� 55      .R�56      �� ${�False��6H    6'   �� *
VentaLibre+6H   57      59      5;      �� 5=   
    �� Orientation$ 
Horizontal=�6   $��Center=�6.   $	��Top=�6K   $��10q�6c   $��1��6o   .+�5>      �� -   5        $��Editar��5>      $��80��6C   $��5q�6N   �� 5?      -   5        $��Eliminar��5?      $��80��6G   $��5q�6R   5@      5A      5B      