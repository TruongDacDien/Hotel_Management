   M S B A M L   `   `   `  ����  GUI    GUI.UserControls.uc_DDXQ   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      
�������   MaterialDesignBody#���   5
      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignFont#���  5      .��5      �  � RowDefinitions 5   
   �� 5      $��60 �6   �� 5      $��* �6   .+�5   
   � $��0��6    � ColumnDefinitions 5      �� 5      �� 5      .+�5      �� $��1��6    �� Orientation$ 
Horizontal=�6)   $��RightToLeft=�6B   .+�5      �� -   5        $	��150��5      $��38��6%   $��Thêm địa điểm��61   $��15(�6I   $��Roboto)�5      $
��Bold"�6-   ���	#FF478C10$��85 0 0 0q�5        J� Clip 5!      �� 5"       �� Rect$ 
0,0,150,38�6,    �� RadiusX$ 10Q�6>    �� RadiusY$ 10Q�6K   5$      5%      � 5'      $��0��6   .+�5)      (  "MaterialDesignThemes.Wpf.ColorZone  	   Mode$	 Standard=�6+   
   CornerRadius$
 5��6;   $
��Left=�5*   +   $��85,0,0,0q�6F   $��Center=�5+   +   $	��250��6F   .��5,      �  5-      �� 5.      $
��Auto �6/   �� 5/      $��* �6/   .+�51      �� $��0��6!     MaterialDesignToolButton#���  61   .��52      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Search=�66    J� Opacity$
 0.56Q�6D   53      �� 54      -   5         ��Name$ 
txbTimKiem��54   2   $��1��6"   $��0 0 10 0q�55   "   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Tìm địa điểm��56   "   .  (MaterialDesignThemes.Wpf.TextFieldAssist   DecorationVisibility$ Hidden=�57   "   $��0q�58   "   $	��200��59   "   $��LeftToRight=�5:   "   $��Center=�5<   "   5=      5>      5?      5@      � 5C   
   $��1��6    5D      �� 5E      $��* �6    �� 5G      $
��Auto �6    .+�5L      � $��0��6   $
��1220��6    $	��500��6-   ���#F2F1F1$��Center=�6O   $��Center=�6l    5M      �� 5N      $ 0,0,1220,500�6(   $ 10Q�6<   $ 10Q�6I   .+�5P      �� $
��Auto=�6   $
��Auto=�6D   .��5Q      �� -   5         ��Name$ lvDDXQ��5Q      $��Disabled=�5R      $��Disabled=�5S       �� View 5T      �� 5U      ! �� ColumnHeaderContainerStyle 5V   "   �� 5W   &    �� 
TargetType" ��6,   .�5X   *   ��  �� Property � �� �� Value
 ��ح�6G   �� 5Y   *    � ��
 �   �6G   �� 5Z   *    � ��$
 Bold"�6G   5[   '   .P�5^   "   �� $��Mã DD��61    �� Width$ 60��6@    �� DisplayMemberBinding 6K   �� *MaDD+�� 5_   "   $��Mã CN��61   $ 60��6@    6K   �� *MaCN+�� 5`   "   $��Tên địa điểm��61   $	 150��6G    6S   �� *TenDD+�� 5a   "   $��Loại địa điểm��61   $	 120��6H    6T   �� *LoaiDD+�� 5b   "   $��Địa chỉ��61   $	 250��6B    6N   �� *DiaChi+�� 5c   "   $��Đánh giá��61   $ 80��6C    6N   �� *	DanhGia+�� 5d   "   $��Khoảng cách (m)��61   $	 130��6J    6V   �� *
KhoangCach+�� 5e   "   $��Thời gian đi chuyển��61   $	 150��6N    6Z   �� *ThoiGianDiChuyen+�� 5f   "   $��Thời gian cập nhật��61   $	 150��6M    5g   )   �� *ThoiGianCapNhat+ �� StringFormat$ dd/MM/yyyy HH:mm:ss���� 5i   "   $ 60��61   $��Sửa��6<    �� CellTemplate 5j   &   �� 5k   *   .R�5l   .   �� -   5        $��25��5l   5   $��25��6@   $��0q�6L   ��5m   2   �� 5n   6    �� ImageSource$! /GUI;component/Res/edit.png��6A   5p   /   5q   +   5s   #   5�      5�      5�      5�      �� 5�      $��1��6   $ 
Horizontal=�6&   $��Center=�6?   $��0,10,0,0q�6\   .+�5�      �� -   5        $	��⏮��5�      $��5q�6?   �� 5�      -   5        $	��⏭��5�      $��5q�6?   5�      5�      5�      5�      