   M S B A M L   `   `   `  ����J  FImageService.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:ImageService.UI.ViewsImageService.UI.Views  5   	   G(clr-namespace:ImageService.UI.ConvertersImageService.UI.Converters  5   	   $    ImageService.UI.Views.LogsView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:ImageService.UI.Views     5   	   ?
converters(clr-namespace:ImageService.UI.Converters     5   	     � 	Resources  5	      6   0ImageService.UI.Converters.LogColorTypeConverter    LogTypeToBackgroundConverter��%   &	          5
   
   6+   5      .��5      ��  �� Orientation$ Vertical=�6   .+�5   
   �� $��40(�6   $
��Logs��5      $��Center=�5      $
��Bold"�5      
����  �5      &   System.Windows.Controls.DataGrid  5   
      AutoGenerateColumns .     CanUserAddRows .  "   VerticalScrollBarVisibility$
 Auto=�5      $
��Auto��5      $
��1000��5      ��. ��5      �� *VmLogEntries+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$
 Type��5         Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 5         
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 5      �� 
 �� Path$

 Type�   ElementStyle 5      �� 5        �� 
TargetType" ��6    .�5!      ��  �� Property � �� �� Value 5"   "   �� 5#   &   $

 Type�6.    �� 	Converter 5$   *   �� 5%   .    �� ResourceKey$" LogTypeToBackgroundConverter��6=   5'   '   5)      5*      5,        5-      $ Message��5.      $ * 5/       . 	 51      �� *	Message+53      54      55      