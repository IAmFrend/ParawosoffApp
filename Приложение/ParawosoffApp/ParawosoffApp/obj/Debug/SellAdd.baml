   M S B A M L   `   `   `  ����H  DParawosoffApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:ParawosoffAppParawosoffApp  5   	       ParawosoffApp.SellAdd   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:ParawosoffApp     5   	   -   5          � Title$!  Добавить заказ��5   	   $	��604��6    $	��592��6-   .��5	      �  � ColumnDefinitions 5
   
   �� 5      6!   �� 5      6!    � RowDefinitions 5   
   �� 5      $��25 �6   6*   �� 5      $	��148 �6   6+   �� 5      $��35 �6   6*   �� 5      $��25 �6   6*   �� 5      $	��148 �6   6+   �� 5      $��25 �6   6*   �� 5      $��35 �6   6*   �� 5      $��35 �6   6*   �� 5      $��25 �6   6*   �� 5      $��27 �6   6*   �� 5      $��35 �6   6*   .+�5   
   �� -   5        $��
lblProduct��5   !   $��Товары��6   $��0��63   $��0��6C   $��TimesNewRoman)�6P   $��14(�6k   
���   �6�   �� 5   
   -   5        $��lblCheck��5   &   $��Номер чека:��6   $��1��66   $��0��6F   $��TimesNewRoman)�6S   $��14(�6n   
���   �6�   &   System.Windows.Controls.DataGrid  5   
   -   5        $��	dgProduct��5      $��0��6$   $��1��64   $��3��6A   $��TimesNewRoman)�6U   $��14(�6p   
���   �   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 auto 6�      AutoGenerateColumns .    
IsReadOnly . 6	  �� 5   
   -   5        $��btAdd��5      $��Добавить��6   $��0��61   $��2��6A   $��3��6N   $��TimesNewRoman)�6b   $��14(�6}   
���   �{�.  6�   �� 5   
   -   5        $��lblSell��5   (   $��Текущий заказ��6   $��0��67   $��3��6G   $��TimesNewRoman)�6T   $��14(�6o   
���   �6�     5    
   -   5        $��dgSell��5       $��0��6!   $��4��61   $��3��6>   $��TimesNewRoman)�6R   $��14(�6m   
���   �$
 auto 6�    .  . 6  �� 5!   
   -   5        $��
tbReamount��5!      $��0��6$   $��5��64   $��3��6A   $��TimesNewRoman)�6U   $��14(�6p   
���   �6�   �� 5"   
   -	   5        $��
btReamount��5"      $+��%Изменить количество��6#   $��0��6A   $��6��6Q   $��3��6^   $��TimesNewRoman)�6r   $��14(�6�   
���   �{�.  6�   �� 5#   
   -
   5        $��btDelete��5#      $��Убрать��6!   $��0��62   $��7��6B   $��3��6O   $��TimesNewRoman)�6c   $��14(�6~   
���   �{�.  6�   �� 5$   
   -   5        $��lblSum��5$       $��
Итого��6   $��0��6.   $��8��6>   $��TimesNewRoman)�6K   $��14(�6f   
���   �6�   �� 5%   
   -   5        $��	lblCurSum��5%   %   $��000 рублей��6   $��0��66   $��2��6F   $��9��6Z   $��TimesNewRoman)�6g   $��16(�6�   
���   �6�   �� 5&   
   -   5        $��btInsert��5&      $��Подтвердить��6!   $��0��67   $	��10 ��6G   $��3��6V   $��TimesNewRoman)�6j   $��14(�6�   
���   �{�.  6�   �� 5'   
   -   5        $��tbProductID��5'      $y�	Collapsed=�6d   ��6%   �� *SelectedItem.Артикул+ �� ElementName$ 	dgProduct��6�   �� 5(   
   -   5        $��tbProductName��5(      $y�	Collapsed=�6g   ��6'   �� *SelectedItem.Название+$ 	dgProduct��6�   �� 5)   
   -   5        $��tbProductPrice��5)      $y�	Collapsed=�6d   ��6(   �� *SelectedItem.Цена+$ 	dgProduct��6�   �� 5*   
   -   5        $��tbCurSellID��5*      $y�	Collapsed=�6a   ��6%   �� *SelectedItem.Артикул+$ dgSell��6�   �� 5+   
   -   5        $��tbCurSellName��5+      $y�	Collapsed=�6d   ��6'   �� *SelectedItem.Название+$ dgSell��6�   �� 5,   
   -   5        $��tbCurSellPrice��5,      $y�	Collapsed=�6a   ��6(   �� *SelectedItem.Цена+$ dgSell��6�   5-      5.      