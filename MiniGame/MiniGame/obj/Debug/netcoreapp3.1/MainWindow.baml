   M S B A M L   `   `   `  ����C  ?MiniGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:MiniGameMiniGame  5   	       MiniGame.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:MiniGame     5   	   -   5          � Title$  KillerBitch��5   	    � SizeToContent$ WidthAndHeight=�6   .��5	      � .+�5
   
   � $��40q�6   .+�5      �� $��5q�6   
���   �._�5      �� -   5         ��Name$ 	gameField��5      $
}�True��6,   $	��700��6@   $	��400��6L   $	��Top=�6Y   .+�5      � $
��auto��6   $
��auto��6)    � RowDefinitions 5      �� 5      $��* �6,   5      5      5      5      �� 5   
   -   5        $|�False��5      $��	Add enemy��6#   $��Center=�67   $��0,10,0,0q�6T   $	��Top=�6f   6~   �� 5   
   -   5         ��Name$ playerScore��5      $��Player score: 0��6%   $
��Left=�6?   $��	10,10,0,0q�6Z   $	��Top=�6m   �� 5   
   -   5        $|�False��5      $��
Reset Game��6#   $
��Left=�68   $��
445,10,0,0q�6S   $	��Top=�6g   6   �� 5   
   -   5        $|�False��5      $��New Game��6#   $
��Left=�66   $��
532,10,0,0q�6Q   $	��Top=�6e   6}   5      5      