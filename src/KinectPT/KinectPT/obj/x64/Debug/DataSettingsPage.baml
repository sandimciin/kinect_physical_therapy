   M S B A M L   `   `   `  ����C  ?KinectPT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:KinectPTKinectPT  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5          KinectPT.DataSettingsPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      (localclr-namespace:KinectPT     5      6sys&clr-namespace:System;assembly=mscorlib     5        N� Title$  DataSettingsPage��5
       � 	Resources 5      *   $KinectPT.EnumMatchToBooleanConverter   enumConverter  True 	 False��%j   &	        &	      &	 /       5   
   6,   ��5   
   True��6)   ��5   
   	False��6+   5      .��5      � .+�5   
   �  � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5      $��* �6#   .+�5      � $��0��6   .+�5      �� -   5        $
��1080��5      $��40��6'   
�������
�����p�$��Center=�6\   $
��Left=�6w   ��5      �� 5      $
��Back��6$   $��30(�60    � LayoutTransform 5      �� 5   "    �� Angle$	 -90Q�62   5       5"      5#      � 5%      $��1��6    � RowDefinitions 5&      �� 5'      $
��Auto �6$   �� 5(      $��* �6$   .+�5,      � $��0��6   .+�5-      �� -   5         ��Name$ 	pageTitle��5-       $��Data Collection Settings��63   $��60(�6S   $��Center=�6a   $��	0 10 0 10q�6~   5.      � 50      $��1��6    51      �� 52      $
��.25* �6+   �� 53      $
��.25* �6+   �� 54      $
��.25* �6+   .+�57      � $��0��6    58      �� 59      $
��.15* �6,   �� 5:      $
��.15* �6,   �� 5;      $
��.25* �6,   .+�5>      � $��0��6   .+�5?      �� $��50(�6(   $��Bottom=�66   $��Center=�6Q   Start6u   5@      � 5B      $��1��6   .+�5C      ��  �� Padding$ 10,10,10,10q�6(   $��20(�6>   $��WrapWithOverflow=�6L    �� TextAlignment$ Center=�6l   ,*Choose when to start collecting node data.6�   5D      � 5F      $��2��6   .+�5G      �� $	��Top=�6%   $��Center=�6=   .+�5H      	� $��Begin at Window open��6*   $��30(�6I   	 d� 	IsChecked	 5I   *   �� 
 �� Current �� Source# Z
  �� Path$&  Properties[beginAtExerciseStart]� �� Mode$ TwoWay=� �� 	Converter# [   �� ConverterParameter# [ 	� 5K      $��Begin at Exercise Start��6*   $��30(�6L   	 5L   *   �� # Z
 $&  Properties[beginAtExerciseStart]�$ TwoWay=�# [  # [ 5N      5O      5U      � 5W      $��1��6    5X      �� 5Y      $
��.15* �6,   �� 5Z      $
��.15* �6,   �� 5[      $
��.25* �6,   .+�5^      � $��0��6   .+�5_      �� $��50(�6$   $��Bottom=�62   $��Center=�6M   
Duration6t   5`      � 5b      $��1��6   .+�5c      �� $ 10,10,10,10q�6(   $��20(�6>   $��WrapWithOverflow=�6L   $ Center=�6l   ~|Set how long node data should be collected in seconds or minutes. Default setting collects data until exercise is completed.6  5d      � 5f      $��2��6   .+�5g      �� $��40��6t   $��70��6�   $��30(�6�   $	��Top=�6�   ��6"   �� # Z
 $ Properties[duration]�6�   �� 5h      -   5        $��durationBox��5h   #    �� Text$ seconds��66    �� 
IsEditable . $��40��6W   $
��Auto��6c   $��30(�6p   $	��Top=�6~   $��Right=�6�   .@�5i      �� -   5        $��minutes1��5i   +   .��6;   	minutes6D   �� 5j      -   5        $��seconds1��5j   +   .��6;   	seconds6D   5k      5l      5m      � 5o      $��2��6    5p      �� 5q      $
��.15* �6,   �� 5r      $
��.15* �6,   �� 5s      $
��.25* �6,   .+�5v      � $��0��6   .+�5w      �� $��50(�6(   $��Bottom=�66   $��Center=�6Q   	Frequency6y   5x      � 5z      $��1��6   .+�5{      �� $ 10,10,10,10q�6(   $��20(�6>   $��WrapWithOverflow=�6L   $ Center=�6l   ;9Specify how often to collect node data (every x seconds).6�   5|      � 5~      $��2��6   .+�5      �� $��30(�6u   $��40��6�   $��50��6�   $	��Top=�6�   ��6"   �� # Z
 $ Properties[frequency]�6�   �� 5�      $��30(�6$   $	��Top=�62   $��Right=�6J   	seconds6o   5�      5�      5�      5�      5�      5�      5�      