   M S B A M L   `   `   `  ����  GUI    GUI.View.DangNhap   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   -   5          �Name$  dangnhap��5   	      MaterialDesignBody#���   5   	   $��Regular"�5	   	   $��13(�5
   	   &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5   	      TextRenderingMode$
 Auto=�5   	     MaterialDesignFont#���  5   	    � Title$ Đăng nhập��5   	   $	��500��6   $	��900��6(    � WindowStyle$
 None=�5   	    � AllowsTransparency$
 True��5   	    � WindowStartupLocation$ CenterScreen=�5   	   ��5      �� 5   
    �� ImageSource$ ../Res/backGround_DN.png��6    J� Clip 5      �� 5   
   	 �� Rect$	 0,0,900,500�6   
 �� RadiusX$
 10Q�6/    �� RadiusY$ 10Q�6<   -   5        	boGoc��5   I   .��5      � .+�5   
   �� -   5         ��Name$ 	btn_Close��5   (   $��65��6   $��60��6   $��Right=�5      $	��Top=�6-   
������ $��0q�5       � ToolTip$ Close��6%    5      �� 5      $	 	0,0,65,50�6$   $
 20Q�65   $ 20Q�6B   .��5       '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ CloseOctagonOutline=�6&   $��35��6A   $��35��5!   &   
������� J� Opacity$	 0.7Q�5"   9   5#      #  MaterialDesignThemes.Wpf.Card  5%   
   $	��300��6   $	��325��6*   ���#FFFFFF$	��Top=�5&      $
��Left=�66   $��	65 80 0 0q�5'       5)      �� 5*      $	 0,0,300,325�6$   $
 10Q�67   $ 10Q�6D   .��5,      � .+�5.      �� $��35��6   $
��Bold"�6(   $��30(�6:   ���#0056FC$��Center=�5/      $	��Top=�69   $��0,25,0,0q�6Q   Đăng Nhập51        52      $ Account=�6*   $
��Left=�69   $	��Top=�6T   $��30��53   *   $��30��66   $��	50,90,0,0q�54   *     55      $
 Lock=�6*   $
��Left=�66   $	��Top=�6Q   $��30��56   *   $��30��66   $��
50,150,0,0q�57   *   �� 58      -   5         ��Name$ txtUsername��58   E   $��	85 90 0 0q�6   $��30��6-   $	��150��69   $
��Left=�59      $	��Top=�65   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Tên đăng nhập��5:      G� 5<      -   5         G�Name$ txtPasswordHidden��5<   H   $ Mật khẩu��6   $
��Left=�5=      $	��Top=�64   $��
85,150,0,0q�5>      $��30��6-   $	��150��69    � Cursor$
 Hand��6E   5?      �� 5@      -   5        $��txtPasswordVisible��5@   D   $ Mật khẩu��6   $
��Left=�5A      $	��Top=�64   $��
85,150,0,0q�5B      $��30��6-   $	��150��69   $y�	Collapsed=�6E     5D      -   5          Name$ iconEye��5D   +   $ EyeOff=�6<   $
��Left=�6J   $	��Top=�6e   $��30��5E      $��30��6&   $��245,150,0,0q�5F      6/   �� 5G      ���#0056FC$
��Left=�5H      $	��Top=�67   $��167,217,0,0q�5I      �� 5J      -   5        $��15(�5J   8   .�6F   Quên mật khẩu?6V   5K      �� 5M      -	   5        $��30��5M      $	��110��6%   ���#0056FC$��Center=�5N      $	��Top=�66   $��	0,265,0,0q�5O       5P      �� 5Q      $	 
0,0,110,30�6,   $
 10Q�6>   $ 10Q�6K   .��5S      �� $��Đăng nhập��6    $��15(�61   
�������6?   5T      �� 5U      -
   5         ��Name$ 
ckb_GhiNho��5U      $��	Ghi nhớ��6/   $��15(�6A   $
��Left=�5V      $��
50,210,0,0q�5W      $	��Top=�6/   -  'MaterialDesignThemes.Wpf.CheckBoxAssist   CheckBoxSize$ 30Q�5X      6h   5Y      5[      5\      5]      