   M S B A M L   `   `   `  ����C  ?KinectPT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:KinectPTKinectPT  5          KinectPT.ReportsPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      R NOxyPlot.Wpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=75e952ba404cdbb0 oxyhttp://oxyplot.org/wpf  5      "localclr-namespace:KinectPT  5        N� Title$  ReportsPage��5
      .��5      � .+�5   
   �  � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5      $��* �6#   .+�5      � $��0��6   .+�5      �� -   5        $
��1080��5      $��40��6'   
�������
�����p�$��Center=�6\   $
��Left=�6w   ��5      �� 5      $
��Back��6$   $��30(�60    � LayoutTransform 5      �� 5   "    �� Angle$	 -90Q�62   5      5      5       � 5#      $��1��6    � RowDefinitions 5$      �� 5%      $
��Auto �6$   �� 5&      $��* �6$   �� 5'      $
��.08* �6$   .+�5+      � $��0��6   .+�5,      �� -   5         ��Name$ 	pageTitle��5,       $��Reports��63   $��60(�6B   $��Center=�6P   $��	0 10 0 10q�6m   5-      � 51      $��1��6   .+�52        OxyPlot.Wpf.PlotView  -   5          Name$ Plot1��52   #      Model 62   �� *Model+53      � 56      $��2��6    57      �� 58      $��* �6+   �� 59      $��* �6+   �� 5:      $��* �6+   .+�5<      �� -   5        $��
Arm Raises��5<      $��0��6D   $��20(�6T   $	��400��6b   $
��Left=�6n   
��� ���
�������6�   �� 5=      -   5        $��Walking��5=      $��1��6B   $��20(�6R   $	��400��6`   $��Center=�6l   
��� ���
�������6�   �� 5>      -   5        $��Sitting and Standing��5>      $��2��6N   $��20(�6^   $	��400��6l   $��Right=�6x   
��� ���
�������6�   5@      5A      5B      5C      5D      