   M S B A M L   `   `   `  ����  GUI    GUI.View.DangNhap_QuenMK   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   -   5          �Name$  dangnhap_quenmk��5   	      MaterialDesignBody#���   5   	   $��Regular"�5	   	   $��13(�5
   	   &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5   	      TextRenderingMode$
 Auto=�5   	     MaterialDesignFont#���  5   	    � Title$ Quên mật khẩu��5   	   $	��400��5   	   $	��400��5   	    � WindowStyle$
 None=�5   	    � AllowsTransparency$
 True��5   	    � WindowStartupLocation$ CenterScreen=�5   	    J� Clip 5      �� 5   
    �� Rect$ 0,0,400,400�6   	 �� RadiusX$	 20Q�6/   
 �� RadiusY$
 20Q�6<   -   5        	boGoc��5   I   .��5      � .+�5   
   �� 
���   �$��2q�6%    �� CornerRadius$ 20��69   ._�5      � .+�5      �� -   5         ��Name$ 	btn_Close��5   0   $��65��6   $��60��6$   $��Right=�6[   $	��Top=�6w   
������ $��0q�6�    � ToolTip$ Close��6�    5      �� 5      $ 	0,0,65,50�6,   $	 20Q�6=   $
 20Q�6J   .��5!      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ CloseOctagonOutline=�6.   $��35��6I   $��35��6T   
���   � J� Opacity$	 0.7Q�6s   5"      � 5#      .+�5$      �� $��35��6    $
��Bold"�6,   $��30(�6>   ���#0056FC$��Center=�5%   	   $	��Top=�6&   $��0,20,0,0q�6>   Quên Mật Khẩu5'        5(      $ Account=�6.   $
��Left=�6=   $	��Top=�6X   $��30��5)      $��30��6#   $��
80,100,0,0q�5*        5+      $ CardAccountDetails=�6.   $
��Left=�6H   $	��Top=�6c   $��30��5,      $��30��6   $��
80,180,0,0q�5-        5.      $ Email=�6.   $
��Left=�6;   $	��Top=�6V   $��30��5/      $��30��6#   $��
80,260,0,0q�50      �� 51      -   5         ��Name$ txtUsername��51   K   $��120 100 0 0q�6   $��26��63   $	��200��6?   $
��Left=�52      $	��Top=�6"   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Tên đăng nhập��53      �� 54      -   5        $ txtCCCD��54   K   $��120 180 0 0q�6   $��26��63   $	��200��6?   $
��Left=�55      $	��Top=�6!   $ Căn cước công dân��56      �� 57      -   5        $��txtEmail��57   E   $ Email��6   $
��Left=�58      $	��Top=�6!   $��120,260,0,0q�59      $��26��6   $	��200��6'   �� 5;      -   5        $ btn_XacNhan��5;      $��30��62   $	��110��6>   ���#0056FC$��Center=�5<      $	��Top=�6#   $��	0,330,0,0q�6;    5=      �� 5>      $ 
0,0,110,30�60   $	 10Q�6B   $
 10Q�6O   .��5@      �� $��Xác nhận��6$   $��15(�64   
�������6B   5A      5B      5C      5D      5E      5F      