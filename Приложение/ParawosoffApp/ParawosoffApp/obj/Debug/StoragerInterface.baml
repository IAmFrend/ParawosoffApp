   M S B A M L   `   `   `  ����H  DParawosoffApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:ParawosoffAppParawosoffApp  5   	   %    ParawosoffApp.StoragerInterface   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:ParawosoffApp     5   	   -   5          � Title$-  'Интерфейс завскладом��5   	   $	��942��6&   $	��774��63   .��5	      �  � ColumnDefinitions 5
   
   �� 5      6!   �� 5      6!   �� 5      6!    � RowDefinitions 5   
   �� 5      $��25 �6   6*   �� 5      $	��180 �6   6+   �� 5      $��60 �6   6*   �� 5      $��60 �6   6*   �� 5      $��25 �6   6*   �� 5      $��29 �6   6*   �� 5      $��25 �6   6*   �� 5      $��29 �6   6*   �� 5      $��25 �6   6*   �� 5      $	��180 �6   6+   �� 5      $��60 �6   6*   �� 5      $��25 �6   6*   �� 5      $��29 �6   6*   �� 5      $��25 �6   6*   �� 5      $��29 �6   6*   �� 5      $��35 �6   6*   �� 5       $��35 �6   6*   �� 5!      $��35 �6   6*   .+�5#   
   �� -   5        $��
lblProduct��5#   !   $��Товары��6   $��0��63   $��0��6C   $��TimesNewRoman)�6P   $��14(�6k   
���   �6�   &   System.Windows.Controls.DataGrid  5$   
   -   5        $��	dgProduct��5$      $��0��6$   $��1��64   $��3��6A   $��TimesNewRoman)�6U   $��14(�6p   
���   �   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 auto 6�      AutoGenerateColumns .    
IsReadOnly .    CanUserSortColumns . 6#  �� 5%   
   $-��'Изменение количества��6   $��2��61   $��0��6>   $��3��6N   $��TimesNewRoman)�6b   .��5&      A�  A� Columns$ 3��6   .+�5'      �� -   5        $��btAdd��5'      $��Добавить��6&   $��TimesNewRoman)�69   $��14(�6T   
���   �{�.  6�   �� 5(      -   5        $��tbAmount��5(      $��TimesNewRoman)�6*   $��14(�6E   
���   �6�   �� 5)      -   5        $��btRemove��5)      $��Убрать��6)   $��TimesNewRoman)�6:   $��14(�6U   
���   �{�.  6�   5*      5+      �� 5,   
   -   5        $��	gdPSearch��5,   "   $��
Поиск��6   $��0��63   $��3��6C   $��2��6P   $��TimesNewRoman)�6d   .��5-      �� -   5        $��	tbPSearch��5-      $��TimesNewRoman)�6'   $��14(�6B   
���   �6e   5.      �� 5/   
   -	   5        $��	btPSearch��5/      $��
Найти��6"   $��3��62   $��3��6B   $��TimesNewRoman)�6O   $��14(�6j   
���   �6�   �� 50   
   -
   5        $��lblName��50   #   $��Название��6   $��0��62   $��4��6B   $��TimesNewRoman)�6O   $��14(�6j   
���   �6�   �� 51   
   -   5        $��tbName��51      $��0��6    $��5��60   $��3��6=   $��TimesNewRoman)�6Q   $��14(�6l   
���   ���6�   �� *SelectedItem.Название+ �� ElementName$ 	dgProduct��6�   �� 52   
   -   5        $��lblPrice��52   +   $"��Стоимость (руб.)��6   $��0��6;   $��6��6K   $��TimesNewRoman)�6X   $��14(�6s   
���   �6�   �� 53   
   -   5        $��tbPrice��53      $��0��6!   $��7��61   $��3��6>   $��TimesNewRoman)�6R   $��14(�6m   
���   ���6�   �� *SelectedItem.Цена+$ 	dgProduct��6  �� 54   
   -   5        $��lblType��54   %   $��Тип товара��6   $��0��64   $��8��6D   $��TimesNewRoman)�6Q   $��14(�6l   
���   �6�     55   
   -   5        $��dgType��55      $��0��6!   $��9��61   $��3��6>   $��TimesNewRoman)�6R   $��14(�6m   
���   �$
 auto 6�    .  .  . 6  �� 56   
   -   5        $��	gdTSearch��56   "   $��
Поиск��6   $��0��63   $��10��6C   $��2��6Q   .��57      �� -   5        $��	tbTSearch��57      $��TimesNewRoman)�6'   $��14(�6B   
���   �6e   58      �� 59   
   -   5        $��	btTSearch��59      $��
Найти��6"   $��3��62   $��10��6B   $��TimesNewRoman)�6P   $��14(�6k   
���   �6�   �� 5:   
   -   5        $��lblTypeName��5:   #   $��Название��6   $��0��66   $��11��6F   $��TimesNewRoman)�6T   $��14(�6o   
���   �6�   �� 5;   
   -   5        $��
tbTypeName��5;      $��0��6$   $��12��64   $��3��6B   $��TimesNewRoman)�6V   $��14(�6q   
���   ���6�   �� *SelectedItem.Название+$ dgType��6�   �� 5<   
   -   5        $��lblTerms��5<   +   $%��Правила хранения��6   $��0��6;   $��13��6K   $��TimesNewRoman)�6Y   $��14(�6t   
���   �6�   �� 5=   
   -   5        $��tbTerms��5=      $��0��6!   $��14��61   $��3��6?   $��TimesNewRoman)�6S   $��14(�6n   
���   ���6�   �� *SelectedItem.Условия+$ dgType��6�   �� 5>   
   -   5        $��btTypeInsert��5>      $��Добавить тип��6%   $��0��6<   $��15��6L   $��TimesNewRoman)�6Z   $��14(�6u   
���   �{�.  6�   �� 5?   
   -   5        $��btTypeUpdate��5?      $��Изменить тип��6%   $��1��6<   $��15��6L   $��TimesNewRoman)�6Z   $��14(�6u   
���   �{�.  6�   �� 5@   
   -   5        $��btTypeDelete��5@      $��Удалить тип��6%   $��2��6;   $��15��6K   $��TimesNewRoman)�6Y   $��14(�6t   
���   �{�.  6�   �� 5A   
   -   5        $��btInsert��5A      $��Добавить��6!   $��0��64   $��16��6D   $��TimesNewRoman)�6R   $��14(�6m   
���   �{�.  6�   �� 5B   
   -   5        $��btUpdate��5B      $��Изменить��6!   $��1��64   $��16��6D   $��TimesNewRoman)�6R   $��14(�6m   
���   �{�.  6�   �� 5C   
   -   5        $��btDelete��5C      $��Удалить��6!   $��2��63   $��16��6C   $��TimesNewRoman)�6Q   $��14(�6l   
���   �{�.  6�   �� 5D   
   -   5        $��btSelect��5D      $��Вывести отчёт��6!   $��0��69   $��17��6I   $��3��6W   $��TimesNewRoman)�6k   $��14(�6�   
���   �6�   �� 5E   
   -   5        $��btOwnerSettings��5E      $��Личные данные��6(   $��3��6@   $��0��6P   $��TimesNewRoman)�6]   $��14(�6x   
���   �6�   �� 5F   
   -   5        $��tbTypeID��5F      $y�	Collapsed=�6�   ��6"   �� *SelectedItem.Номер+$ dgType��6�   �� 5G   
   -    5        $��tbProductID��5G      $y�	Collapsed=�6�   ��6%   �� *SelectedItem.Артикул+$ 	dgProduct��6�   �� 5H   
   -!   5        $��tbProductCurAmount��5H      $y�	Collapsed=�6�   ��6,   �� *#!SelectedItem.Количество+$ 	dgProduct��6�   5I      5J      