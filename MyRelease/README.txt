 LOGO FOR PRINTER VER 1.0

 This program is used for translating the printer coordinates in a .cpcl file
 into a textbased image for the purpose of editing and then update the .cpcl 
 file with the new coordinates. LIMITATION: Only supports horizontal lines.

 PREPARATIONS
 
 Copy the 'L' lines for the logo you intend to print from the original .cpcl file. 
 Paste into a file named text_logo.cpcl placed in the main folder.
 
 HOW TO USE IT 
 
 For Windows:
 To start, click the "logoforprinter.bat" file.

 In terminal:
 logoforprinter.exe CMD [input file name] [output file name].
 CMD: t2i = text to image, i2t = image to text.

 1. The program reads the 'L' coordinates from the .cpcl file and translates it into 
    a textbased image file called "graphic_logo.prim" which opens in a text editor 
    (notepad by default).

 2. Make the desired changes to your image. 
    This is done with 'X' for filled space and '.' for empty space. 

 3. When done, save the file in the text editor and close the editor window 
    in order to update your cpcl file with the new changes.

 For even more accurate viewing and editing i recommend using the font SQUARE.TTF provided in the folder.


