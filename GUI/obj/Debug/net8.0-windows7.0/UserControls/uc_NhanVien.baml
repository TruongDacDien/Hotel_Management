   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5       GUI"   GUI.UserControls.uc_NhanVien   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6sys&clr-namespace:System;assembly=mscorlib     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5
      
�������   MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignFont#���  5      .��5      �  � RowDefinitions 5   
   �� 5      $��60 �6   �� 5      $��* �6   .+�5   
   � $��0��6    � ColumnDefinitions 5      �� 5      �� 5      .+�5      �� $��1��6    �� Orientation$ 
Horizontal=�6)   $��RightToLeft=�6B   .+�5      �� -   5        $	��150��5      $��38��6%   $��Thêm nhân viên��61   $��15(�6J   $��Roboto)�5      $
��Bold"�6-   ���	#FF478C10$��85 0 0 0q�5        J� Clip 5!      �� 5"       �� Rect$ 
0,0,150,38�6,    �� RadiusX$ 10Q�6>    �� RadiusY$ 10Q�6K   5$      5&      � 5'      $��0��6   .+�5)      (  "MaterialDesignThemes.Wpf.ColorZone  	   Mode$	 Standard=�6+   
   CornerRadius$
 5��6;   $
��Left=�6L   $��85,0,0,0q�6g   $��Center=�5*   +   $	��250��5+   +   .��5,      �  5-      �� 5.      $
��Auto �6/   �� 5/      $��* �6/   .+�51      �� $��0��6!     MaterialDesignToolButton#���  61   .��52      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Search=�66    J� Opacity$
 0.56Q�6D   53      �� 54      -   5         ��Name$ 
txbTimKiem��54   2   $��1��6"   $��0 0 10 0q�55   "   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Tìm nhân viên ��56   "   .  (MaterialDesignThemes.Wpf.TextFieldAssist   DecorationVisibility$ Hidden=�57   "   $��0q�58   "   $	��200��59   "   $��LeftToRight=�5:   "   $��Center=�5<   "   5>      5?      �� 5A      $��	100 0 0 0q�6   $��60��5B      $��50��6$   
������ $��0q�5D       5E      �� 5F      $ 	0,0,60,50�6,   $ 50Q�6=   $ 60Q�6J   .��5H        $
 Send=�6.   $��30��6:   $��30��5I   .   
���   �$	 0.7Q�5J   A   5K      5L      5M      � 5Q   
   $��1��6   .+�5R      � $
��1220��6   $	��500��6    ���#F2F1F1$��Center=�6B   $��Center=�6_    5S      �� 5T      $ 0,0,1220,500�6(   $ 10Q�6<   $ 10Q�6I   .+�5V      �� -   5         ��Name$ 
lvNhanVien��5V      $��Disabled=�6/    � 	Resources 5X      ��%    5Y       �� View 5Z      �� 5[      ! �� ColumnHeaderContainerStyle 5]      �� 5^   "    �� 
TargetType" ��6(   .�5_   &   ��  �� Property � �� �� Value
 ��ح�6C   �� 5`   &    � ��
 �   �6C   �� 5a   &    � ��$
 Bold"�6C   5b   #   .P�5f      �� $��Mã NV��6-    �� Width$ 60��6<    �� DisplayMemberBinding 6G   �� *MaNV+�� 5g      $��Tên Nhân Viên��6-   $	 150��6D    6P   �� *HoTen+�� 5h      $
��NTNS��6-   $	 100��6;    5i   -   ��  �� Path$
 NTNS� �� StringFormat$ 
dd/MM/yyyy���� 5j      $��Chức vụ��6-   $	 120��6>    6J   �� *ChucVu+�� 5k      $��Giới tính��6-   $ 70��6@    6K   �� *
GioiTinh+�� 5l      $��Dịa chỉ��6-   $	 240��6>    6J   �� *DiaChi+�� 5m      $
��SĐT��6-   $	 110��6:    6F   �� *SDT+�� 5n      $
��CCCD��6-   $	 100��6;    6G   �� *CCCD+�� 5o      $��Lương��6-   $	 150��6<    5p   -   �� *Luong+$ 
{0:N0} VND���� 5r      $ 60��6-   $��Sửa��68    �� CellTemplate 5s   "   �� 5t   &   .R�5u   *   �� -   5        $��25��5u   1   $��25��6<   $��0q�6H   ��5v   .   �� 5w   2    �� ImageSource$! /GUI;component/Res/edit.png��6=   5y   +   5z   '   5|      �� 5}      $ 60��6-   $
��Xóa��68    5~   "   �� 5   &   .R�5�   *   �� -   5        $��25��5�   1   $��25��6<   $��0q�6H   ��5�   .   �� 5�   2   $# /GUI;component/Res/delete.png��6=   5�   +   5�   '   5�      5�      5�      5�      5�      5�      5�      