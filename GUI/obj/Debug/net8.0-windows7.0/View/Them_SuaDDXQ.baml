   M S B A M L   `   `   `  ����  GUI    GUI.View.Them_SuaDDXQ   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   $	��600��5   	   $	��800��6     � WindowStyle$
  None=�5	   	    � AllowsTransparency$
 True��5
   	    � WindowStartupLocation$ CenterScreen=�5   	   
�������   MaterialDesignFont#���   5   	     MaterialDesignBody#���  5   	   $��Regular"�5   	   $��13(�5   	   &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5   	      TextRenderingMode$
 Auto=�5   	    J� Clip 5      �� 5   
    �� Rect$ 0,0,800,600�6    �� RadiusX$ 10Q�6/    �� RadiusY$ 10Q�6<   .��5      �� $��1q�6   ���	#FF807D7D	 �� CornerRadius$	 10��69   ._�5   
   � 
 � RowDefinitions
 5      �� 5      $	��100 �6    �� 5      $��* �6    �� 5      $��80 �6    .+�5!      �� $��0��6   $��Thông tin địa điểm��6%   $��28(�6?   $
��Bold"�6M   $��Center=�5"      $��Center=�65   ���#00A3FF6P   � 5$      $��1��6   .+�5%      � $��40q�6   ���#F2F1F1$
}�True��68    5&      �� 5'      $ 0,0,700,400�6,   $ 10Q�6?   $ 10Q�6L    � ColumnDefinitions 5)      �� 5*      �� 5+      $��2 �6+   �� 5,      .+�50      �� $��0��6!   $��30q�61   .+�51      �� -   5         ��Name$ txbTenDD��51   "    % !MaterialDesignFloatingHintTextBox#��[ 64   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Tên địa điểm��52   "   $
��0 10q�6P   �� 53      -   5        $ 	txbLoaiDD��53   "   #��[ 65   $ Loại địa điểm��54   "   $
��0 10q�6Q   �� 55      -   5        $ 	txbDiaChi��55   "   #��[ 65   $ Địa chỉ��56   "   $
��0 10q�6K   �� 57      -   5        $ 
txbDanhGia��57   "   #��[ 66   $ Đánh giá (VD: 4.5)��58   "   $
��0 10q�6V   �� 59      -   5        $ txbViDo��59   "   #��[ 63   $ 	Vĩ độ��5:   "   $
��0 10q�6I   5;      � 5>      $��1��6   ���#9A8A8A$��2��6@   $
��10 0q�6J   �� 5A      $��2��6!   $��30q�61   .+�5B      �� -   5        $ 	txbKinhDo��5B   "   #��[ 65   $ 
Kinh độ��5C   "   $
��0 10q�6K   �� 5D      -   5        $ txbKhoangCach��5D   "   #��[ 69   $ Khoảng cách (m)��5E   "   $
��0 10q�6S   �� 5F      -   5        $ txbThoiGianDiChuyen��5F   "   #��[ 6?   $! Thời gian di chuyển (s)��5G   "   $
��0 10q�6[   (  "System.Windows.Controls.DatePicker  5H      -	   5          Name$ dtThoiGianCapNhat��5H   %    ( $MaterialDesignFloatingHintDatePicker#��[ 6@   $ Thời gian cập nhật��5I   %   $
��0 10q�6Y   �� 5J      -
   5        $ txbMaCN��5J   "   #��[ 63   $ Mã chi nhánh��5K   "   $
��0 10q�6P   5L      5M      5N      � 5Q      $��2��6   .+�5R      �� -   5        $
��Lưu��5R      $	��100��6'   $��36��63   ���#478C10$��Right=�5S      $��Center=�65   $��	0 0 120 0q�6P    5U      �� 5V      $ 
0,0,100,36�6,   $ 10Q�6>   $ 10Q�6K   5X      �� 5Y      -   5        $��Hủy��5Y      $	��100��6'   $��36��63   ���#9D9999$��Right=�5Z      $��Center=�65   $��0 0 20 0q�6P    5\      �� 5]      $ 
0,0,100,36�6,   $ 10Q�6>   $ 10Q�6K   5_      5`      5a      5b      5c      