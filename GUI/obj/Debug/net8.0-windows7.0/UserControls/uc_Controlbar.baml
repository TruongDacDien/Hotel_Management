   M S B A M L   `   `   `  ����  GUI$    GUI.UserControls.uc_Controlbar   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	      -   5          >�Name$  
controlbar��5      
 � Tag$ Title��5         MaterialDesignBody#���   5
      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5         TextRenderingMode$
 Auto=�5      
������   MaterialDesignFont#���  5      $��0��6@   .��5      � .+�5   
   (  "MaterialDesignThemes.Wpf.ColorZone  -   5          Name$ 	backGroum��5   #   
�������$��2q�5   #      CornerRadius$ 	10 10 0 0��5   #   $��0 2 0 0q�5   #   .��5      � -   5         � ColumnDefinitions 5      �� 5      �� 5      $	��110 �6'   .+�5      �� $��0��6   $��Center=�5       $��20 0 0 0q�5!      ��5"      �� *Tag+ �� ElementName$ 
controlbar���� 5%       �� Orientation$ 
Horizontal=�6   $
��Auto��5&      $��1��5'      
������ .+�5*      �� -   5        $��7 0 7 0q�5+      $��20��5,      $��20��5-      ���	#FF2BD655 & "MaterialDesignFloatingActionButton#��[ 50      	 � ToolTip$	 Minimize��51      .��52      �� $	����6$   $��Segoe MDL2 Assets)�53   $   $��12(�54   $   $��Center=�55   $   $��Center=�56   $   
�������57   $   58      �� 5;      -   5        
 ��Name$
 btn_maximize��5<      $��20��5=      $��20��5>      $��7 0 7 0q�5?      ���	#FFFFCA32#��[ 5B      $	 Maximize��5C      .��5D      �� $	����6$   $��Segoe MDL2 Assets)�5E   $   $��12(�5F   $   $��Center=�5G   $   $��Center=�5H   $   
�������5I   $   5J      �� 5M      -   5        $��20��5N      $��20��5O      $��7 0 7 0q�5P      ���	#FFE17365#��[ 5S      $	 Close��5T      .��5U      �� $	����6$   $��Segoe MDL2 Assets)�5V   $   $��12(�5W   $   $��Center=�5X   $   $��Center=�5Y   $   
�������5Z   $   5[      5\      5]      5^      5_      5`      