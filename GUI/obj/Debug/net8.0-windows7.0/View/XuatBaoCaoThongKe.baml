   M S B A M L   `   `   `  ����  GUI     GUI.View.XuatBaoCaoThongKe   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$  	Báo cáo��5   	   $	��600��6   $	��950��6&   $��13(�62    � WindowStartupLocation$ CenterScreen=�5   	   .��5
      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   ���#F5F5F5 �� Padding$ 10q�63    �� CornerRadius$ 5��6@   $��10q�6Q   ._�5      �  � ColumnDefinitions 5      �� 5      $��* �6'   �� 5      $
��Auto �6'   .+�5      ��  �� Orientation$ 
Horizontal=�6   $��Center=�66   $��0��6Q   .+�5      �� $ 
Horizontal=�6!   $��10q�6:   .+�5      �� $��Chọn Tháng:��6$   $��Center=�67   $
��Bold"�6R   $��0,0,5,0q�6d   �� 5       -   5         ��Name$ MonthSelector��5    #   $��80��6:   6E   5!      �� 5"      $ 
Horizontal=�6!   $��10q�6:   .+�5#      �� $��Chọn Năm:��6$   $��Center=�65   $
��Bold"�6P   $��0,0,5,0q�6b   �� 5$      -   5        $ YearSelector��5$   #   $	��120��69   6E   5%      5&      �� 5)      -   5        $��Xuất ra Excel��5)      $	��150��61   $��40��6=   $��Center=�5*      $��1��64   ���#478C105+   6   5,      5-      �� 50   
   $��1��6   ���#DDD$��1q�61   
�������$ 5��6X   $��10q�6i   ._�51      &   System.Windows.Controls.DataGrid  -   5          Name$ ReportDataGrid��51      	   AutoGenerateColumns$	 False��6/   
   HeadersVisibility$
 Column=�6K      
IsReadOnly$
 True��52      $��13(�6)      Columns 53      0  *System.Windows.Controls.DataGridTextColumn  54      ,  &System.Windows.Controls.DataGridColumn   Header$ Mã hóa đơn��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$	 150 6V   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6=   �� *MaHD+6V     55      $ Ngày lập��6)   $ * 6W    6;   �� *	NgayLap+6W     56      $ Tên nhân viên��6)   $ * 6c    6@   �� *NhanVien.HoTen+6c     57      $" Mã chi tiết phiếu thuê��6)   $ * 6d    6I   �� *MaCTPT+6d     58      $ Tổng tiền��6)   $ * 59   )    58   <   �� *
TongTien+ �� StringFormat$ 
{0:N0} VND��59   )   5;      5<      �� 5?   
   $��2��6   ���#F5F5F5$ 10q�63   $ 5��6@   $��10q�6Q   ._�5@      �� $ 
Horizontal=�6   $��Right=�62   .+�5A      �� $��Tổng doanh thu: ��6   $��Center=�64   $
��Bold"�6O   $��14(�6a   �� 5B      -   5         ��Name$ TotalRevenueText��5B      $��Center=�66   $��14(�6Q   
���  ��6_   5C      5D      5E      5F      