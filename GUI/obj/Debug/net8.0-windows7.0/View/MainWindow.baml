   M S B A M L   `   `   `  ����>  :GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:GUI.UserControlsGUI.UserControls  5   	       GUI.View.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -ucclr-namespace:GUI.UserControls     5   	   _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	   	   -   5          �Name$  mainwindows��5   	      MaterialDesignBody#���   5
   	   $��Regular"�5   	   $��13(�5   	   &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5   	      TextRenderingMode$
 Auto=�5   	     MaterialDesignFont#���  5   	    � Title$ Quản lý khách sạn��5   	   $	��700��6#   $
��1300��60    � WindowStyle$
 None=�5   	    � AllowsTransparency$
 True��5   	    � WindowStartupLocation$ CenterScreen=�5   	    � 	Resources 5        	CloseMenu �� TargetProperty	 �� 
TargetName9  3System.Windows.Media.Animation.EasingDoubleKeyFrame
 O� KeyTime O� Value  OpenMenu��%  &	       &	     �� 5   
   .��5      U� $U O(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.X)�5      $	 grdMenu��5      .\�5        $
 0��6'   $	 250Q�63     5      $
 0:0:0.3��6'   $ 0Q�69   5      5      �� 5   
   .��5       U� $U O(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.X)�5!      $	 grdMenu��5"      .\�5#        $
 0��6'   $ 0Q�63     5$      $
 0:0:0.3��6'   $	 250Q�69   5%      5&      5'      ��5(      �� 5)   
    � Triggers 5,      :� 5-   
    :� RoutedEvent$ ButtonBase.Click��6    :� 
SourceName$ btnCloseLVMenu��66   .[�5.      �� -   5         ��Name$ CloseMenu_BeginStoryboard��5.      #��[ 6A   5/      :� 50   
   $ ButtonBase.Click��6   $ btnOpenLVMenu��66   .[�51      �� #��[ 6   52       J� Clip 55      �� 56   
    �� Rect$ 0,0,1300,700�6    �� RadiusX$ 10Q�60    �� RadiusY$ 10Q�6=   -   5        	boGoc��56   J   .��58      �  � RowDefinitions 59   
   �� 5:      $��26 �6   �� 5;      .+�5?   
   $   GUI.UserControls.uc_Controlbar  $��0��6   $��Center=�5A   J   ��5@      �� *MinHeight_ucControlbar+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�
 � Tag 5A      �� *Title+ �� ElementName$ mainwindows��6J   � 5C   
   $��1��6   .+�5F      � -   5         �Name$ GridBackground��5F       5G      �� 5H      $��30 �6$   �� 5I      .+�5L      (  "MaterialDesignThemes.Wpf.ColorZone  -   5          Name$ titleBar��5M   +   ���	#FFF4F4F4$��30��5L   B   $��0��6N   .��5N      �  � ColumnDefinitions 5O      �� 5P      $
��auto �6/   �� 5Q      $��1* �6/   �� 5R      $	��210 �6/   .+�5U      ��  �� Orientation$ 
Horizontal=�6%   $��0��6>   $��5 0 0 0 q�6N   .+�5W      �� -   5         ��Name$ btnOpenLVMenu��5X   %   $��30��5W   %   
������ $��0q�5X   U   $��0q�6i   .��5Y   "   '  !MaterialDesignThemes.Wpf.PackIcon     Kind$
 Menu=�6:   
���   �$��25��6Y   $��25��6d   5Z      5\      � 5^      $��1��6   .+�5_      �� $��Center=�6(   $��40 0 0 0q�6C   $��20(�5`   D   ��6(   �� *
Title_Main+6D   5a      � 5b      $��2��6   .+�5d      '  !MaterialDesignThemes.Wpf.PopupBox  $��	 0 0 40 0q�66   $��Right=�6I      PopupContent. 5e   "   �� .@�5f   &   �� -   5        $��Thông tin cá nhân��5f   2   6N   �� 5g   &   -   5        $��Thông tin tài khoản��5g   2   6P   �� 5h   &   -	   5        $��Đăng xuất��5h   2   6F   5i   #   5j      5k      5m      5o      � 5q      -
   5        $ displayMain��5q      $��1��5r      .+�5s      �� -   5          ��Name$  contenDisplayMain��5s   %   $
��auto��5t   %   $
��auto��5u   %   5v      5w      � 5y      -   5        $ grdMenu��5}      $	��250��5y      
�������$
��Left=�5z      $��
-250 0 0 0q�5{      ! J� RenderTransformOrigin$! 0.5,0.5&�5|      z�5~      V� 5      .~�5�      �� �� 5�      �� 5�      T� 5�      5�       5�      �� 5�      $
��Auto �6$   �� 5�      $��* �6$   .+�5�      � $��0��6   $	��170��6$   .+�5�      � ���	#FF142C2F.+�5�      A� -   5        " A�Name$" 	imgAvatar��5�   "   $	��130��65   $	��130��6A   $	��Top=�6N   $��0 10 0 0q�6f   ��5�      �� 5�   "   # �� ImageSource$"# /GUI;component/Res/image.png��6-   5�      � 5�      -   5        $ 	grdAvatar��5�      $	��130��62   $	��130��6>   $	��Top=�6K   $��0 10 0 0q�6c    5�      �� 5�   "   $ 0,0,130,130�64   $ 65Q�6G   $ 65Q�6T   .+�5�      �� -   5        $ ��Name$$ txblAvatarBinding��5�   (   5�      �� 5�      -   5        $$ 
txbHoTenNV��5�   $   $��Trương Đắc Điền��68   $��18(�6O   $��0 0 0 4q�6]   $��Center=�5�   $   $��Bottom=�6A   ���	#DDDFE9F7$��Oboto)�5�   ?   ��6$   Z� 6?   5�      �� 5�      -   5        $ btnCloseLVMenu��5�      $��30��5�      $��40��6)   $	��Top=�64   $��Right=�6L   
������ $��0q�5�   N   $��0q�6b   .��5�        $ ChevronTripleLeft=�62   ���	#FFE2D7D7$��40��6b   $��30��6m   5�      5�      �� 5�      -   5        % ��Name$% lisviewMenu��5�      $��1��5�      ���	#FF313131$��Champagne & Limousines)�6?   $��18(�6g   $��Hidden=�5�      $��Hidden=�5�      $
��True��5�      ��5�      �� 5�      .R�5�      �� $ 
Horizontal=�6)   $
��10 0q�6B   .+�5�   "     $��20��6U   $��20��6`   $��5q�5�   [   $��Center=�5�   :    5�   :   �� *	kind_Icon+��5�   :   �� *	foreColor+5�   :   �� 5�   "   $��10q�6B   ��6,   �� *name+6B   5�      5�      5�      5�      5�      5�      5�      