   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5       GUI#   GUI.UserControls.uc_PhieuThue   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6sys&clr-namespace:System;assembly=mscorlib     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5
      
�������   MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignFont#���  5      .��5      �  � RowDefinitions 5   
   �� 5      $��60 �6   �� 5      $��* �6   .+�5   
   � $��0��6    � ColumnDefinitions 5      �� 5      �� 5      .+�5      � $��1��6   .+�5      �� -   5        $	��150��5      $��38��6%   $��Đặt phòng��61   $��15(�6E   $��Right=�6S   $��Roboto)�5      $
��Bold"�6-   ���	#FF478C10$��	0 0 150 0q�5        J� Clip 5!      �� 5"       �� Rect$ 
0,0,150,38�6,    �� RadiusX$ 10Q�6>    �� RadiusY$ 10Q�6K   5$      5%      � 5&      $��0��6   .+�5(      (  "MaterialDesignThemes.Wpf.ColorZone     Mode$ Standard=�6+   	   CornerRadius$	 5��6;   $
��Left=�6L   $��	160,0,0,0q�5)   +   $��Center=�5*   +   $	��250��5+   +   .��5,      �  5-      �� 5.      $
��Auto �6/   �� 5/      $��* �6/   .+�51      �� $��0��6!     MaterialDesignToolButton#���  61   .��52      '  !MaterialDesignThemes.Wpf.PackIcon  
   Kind$
 Search=�66    J� Opacity$
 0.56Q�6D   53      �� 54      -   5         ��Name$ 
txbTimKiem��54   2   $��1��6"   $��0 0 10 0q�55   "   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$! Tìm theo tên khách hàng��56   "   .  (MaterialDesignThemes.Wpf.TextFieldAssist   DecorationVisibility$ Hidden=�57   "   $��0q�58   "   $	��200��59   "   $��LeftToRight=�5;   "   $��Center=�5<   "   5>      5?      5@      5A      � 5E   
   $��1��6   .+�5F      � $��Center=�6   $��Center=�60   $
��1000��6K   $	��500��6X   ���#F2F1F1 5G      �� 5H      $ 0,0,1000,500�6(   $ 10Q�6<   $ 10Q�6I   .+�5J      �� -   5         ��Name$ lvPhieuThue��5J      $��Disabled=�60    � 	Resources 5L      ��%    5M       �� View 5N      �� 5O      ! �� ColumnHeaderContainerStyle 5Q      �� 5R   "    �� 
TargetType" ��6(   .�5S   &   ��  �� Property � �� �� Value
 ��ح�6C   �� 5T   &    � ��
 �   �6C   �� 5U   &    � ��$
 Bold"�6C   5V   #   .P�5Z      �� $��Số phiếu thuê��6-    �� Width$	 190��6D    �� DisplayMemberBinding 5[   -   �� *MaPhieuThue+�� 5\      $��Tên khách hàng��6-   $	 230��6E    6Q   �� *TenKH+�� 5]      $��Ngày lập phiếu��6-   $	 230��6E    5^   -   �� *NgayLapPhieu+ �� StringFormat$ 	dd/MM/yyy���� 5_      $��Tên nhân viên��6-   $	 230��6D    6P   �� *TenNV+�� 5`      $ 55��6-   $��
Chi tiết��68    �� CellTemplate 5a   "   �� 5b   &   .R�5c   *   �� -   5        $��25��5c   1   $��25��6<   $��0q�6H   $��0q�5d   1   
������ .��5e   .     $

 More=�6F   
���   �6R   5f   +   5g   '   5i      �� 5j      $ 55��6-   $
��Xóa��68    5k   "   �� 5l   &   .R�5m   *   �� -   5        $��25��5m   1   $��25��6<   $��0q�6H   ��5n   .   �� 5o   2    �� ImageSource$# /GUI;component/Res/delete.png��6=   5q   +   5r   '   5t      5u      5w      5x      5y      5z      5{      