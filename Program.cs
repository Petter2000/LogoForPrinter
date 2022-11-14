//===================================================================================
// Logo for printer
//
// The program concists of two main functions:
// 1]. Text to image:
//     Translates the printer coordinates in a .cpcl file into a textbased image.
// 2]. Image to text.
//     Translates the textbased image into printer coordinates in a .cpcl file.
// 
// LIMITATION: Only supports horizontal 'L' lines in the .cpcl file.
//
// Language: C#
// OS: MS Windows
// Author Petter Berelin
// Created 2022-11-13
//=============================================================================

using logoforprinter.Models;

namespace logoforprinter
{
  public class Program
  {
    static public string Version { get; set; } = "1.0";
    static void Main(string[] args)
    {
      List<string> startupValues = new List<string>(args);

      //===============================================
      // For debugging. Text > Image | Image > Text.
      //-------------------------------------------
      if (startupValues.Count() == 0)
      {
        startupValues = new List<string> { "t2i", "MyRelease\\text_logo.cpcl", "MyRelease\\image_logo.prim" };
        //startupValues = new List<string> { "i2t", "MyRelease\\image_logo.prim", "MyRelease\\text_logo.cpcl" };
      }

      //===============================================================================
      // Makes sure that the program takes the correct amount of arguments to start
      // and populates the correct variables to accomplish the desired task
      //--------------------------------------------------------------------------
      Logo.PrintLogo(startupValues[0]);
      var direction = "";
      var inputFile = "";
      var outputFile = "";

      if (startupValues.Count() != 3)
      {
        direction = "missing startup values";
      }

      else
      {
        direction = startupValues[0];
        inputFile = startupValues[1];
        outputFile = startupValues[2];
      }

      //====================================================
      // Executes the requested task pending on direction.
      //------------------------------------------------
      switch (direction)
      {
        case "t2i":
          TextToImage.FromTextToImage(inputFile, outputFile);
          break;

        case "i2t":
          ImageToText.FromImageToText(inputFile, outputFile);
          break;

        default:
          Console.WriteLine("Command line syntax:");
          Console.WriteLine("logoforprinter.exe CMD [input file name] [output file name]");
          Console.WriteLine("CMD: t2i = text to image, i2t = image to text");
          break;
      }
    }
  }
}
