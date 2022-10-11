using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LogoForPrinter.Model
{
  public class WorksheetOffsets
  {
    static public int MinX { get; set; } = 99999;
    static public int MaxX { get; set; } = 0;
    static public int MinY { get; set; } = 99999;
    static public int MaxY { get; set; } = 0;
    static public int ReducedXBy { get; set; }
    static public int ReducedYBy { get; set; }
    static public int Margin { get; set; } = 5;
    static public char FilledSpace { get; set; } = 'X';
    static public char EmptySpace { get; set; } = '.';


    public static void FromTextToImage()
    {
      //=========================
      // Read print file commands
      //------------------------
      string o = (@$"C:\Users\Petter\source\repos\logoforprinter\Logo\text_logo.ORG.cpcl");
      string n = (@$"C:\Users\Petter\source\repos\logoforprinter\Logo\text_logo.cpcl");
      string[] lines = File.ReadAllLines(o);

      //======================================
      // Calculate offset values for Worksheet
      //-------------------------------------
      WorksheetOffsets.CalculateOffsets(lines);

      //============================
      // Create unpopulated worksheet 
      //-----------------------------
      var worksheet = WorksheetOffsets.CreateWorksheet();

      //====================================
      // Populate worsksheet with coordinate
      //-----------------------------------
      worksheet = WorksheetOffsets.PopulateWorksheet(worksheet, lines);

      //=======================================
      // Reverse worksheet for accurate viewing
      //-------------------------------------
      worksheet = ReverseWorksheet(worksheet);

      //=====================================
      // Add line number for accurate editing
      //------------------------------------
      worksheet = AddLineNumber(worksheet, lines);

      //================================================
      // Convert the worksheet list to a savable item.
      //----------------------------------------------
      string itemToSave = ListToString(worksheet);

      //======================
      // Save item to file.
      //--------------------
      File.WriteAllText(@"C:\Users\Petter\source\repos\logoforprinter\Logo\graphic_logo.cpcl", itemToSave);

    }
    public static string CreateFieldsToPrint(List<string> listOfCoordinates)
    {
      string result = "";
      for (int i = 0; i < listOfCoordinates.Count(); i++)
      {

        // }
        // foreach (var line in listOfCoordinates)
        // {
        string[] lineArray = listOfCoordinates[i].Split(",");



        int lastElement = 0;
        int coordinateInt = 0;
        var coordinateIntList = new List<int>();

        foreach (var item in lineArray)
        {
          if (item != "")
          {
            coordinateInt = Int32.Parse(item);



          }

          if (lastElement == 0 || coordinateInt == lastElement + 1)
          {
            coordinateIntList.Add(coordinateInt);
            lastElement = coordinateInt;
          }

          else
          {
            var min = coordinateIntList.Min();
            var max = coordinateIntList.Max();
            (min, max) = SetValues(min, max);

            //L 8 773 8 765 1
            result += $"L {i} {min} {i} {max} 1 \n";
            lastElement = 0;
            coordinateIntList.Clear();




            //Console.WriteLine(result);
          }
        }

      }

      return result;
    }
    public static List<string> GetAllFilledSpaces(string[] inputFile)
    {
      var listOfCoordinates = new List<string>();
      var lineOfCoordinates = "";

      for (int i = 0; i < inputFile.Count(); i++)
      {


        string[] lineArray = inputFile[i].Split(" ");
        char[] lines = string.Join(string.Empty, lineArray).ToCharArray();

        for (int j = 0; j < lines.Count(); j++)
        {
          if (lines[j] == FilledSpace)
          {
            lineOfCoordinates += j + ",";
          }
          // else
          // {
          //   lineOfCoordinates += '.' + ",";
          // }
        }

        listOfCoordinates.Add(lineOfCoordinates);
        lineOfCoordinates = "";

      }
      return listOfCoordinates;
    }
    public static void FromImageToText()
    {
      //============================
      // Read image pattern
      //-----------------------
      string o = (@"C:\Users\Petter\source\repos\logoforprinter\Logo\graphic_logo.ORG.cpcl");
      string n = (@"C:\Users\Petter\source\repos\logoforprinter\Logo\graphic_logo.cpcl");
      string[] inputFile = File.ReadAllLines(o);




      //===================================================
      // Collect all filled spaces from image.
      // Returns a list with the index for all the filled space.
      //------------------------------------
      var listOfCoordinates = GetAllFilledSpaces(inputFile);


      // var listOfCoordinates = new List<string>();
      // var lineOfCoordinates = "";

      // for (int i = 0; i < inputFile.Count(); i++)
      // {


      //   string[] lineArray = inputFile[i].Split(" ");
      //   char[] lines = string.Join(string.Empty, lineArray).ToCharArray();

      //   for (int j = 0; j < lines.Count(); j++)
      //   {
      //     if (lines[j] == FilledSpace)
      //     {
      //       lineOfCoordinates += j + ",";
      //     }
      //     // else
      //     // {
      //     //   lineOfCoordinates += '.' + ",";
      //     // }
      //   }

      //   listOfCoordinates.Add(lineOfCoordinates);
      //   lineOfCoordinates = "";

      // }
      //=====================================
      // Creates fields of filled spaces with subsequent index
      // Returns a string with fields to print in the form readable cpcl
      //=======================================
      var result = CreateFieldsToPrint(listOfCoordinates);
      // for (int i = 0; i < listOfCoordinates.Count(); i++)
      // {

      //   // }
      //   // foreach (var line in listOfCoordinates)
      //   // {
      //   string[] lineArray = listOfCoordinates[i].Split(",");



      //   int lastElement = 0;
      //   int coordinateInt = 0;
      //   var coordinateIntList = new List<int>();

      //   foreach (var item in lineArray)
      //   {
      //     if (item != "")
      //     {
      //       coordinateInt = Int32.Parse(item);



      //     }

      //     if (lastElement == 0 || coordinateInt == lastElement + 1)
      //     {
      //       coordinateIntList.Add(coordinateInt);
      //       lastElement = coordinateInt;
      //     }

      //     else
      //     {
      //       var min = coordinateIntList.Min();
      //       var max = coordinateIntList.Max();
      //       (min, max) = SetValues(min, max);

      //       //L 8 773 8 765 1
      //       result += $"L {i} {min} {i} {max} 1 \n";
      //       lastElement = 0;
      //       coordinateIntList.Clear();




      //       //Console.WriteLine(result);
      //     }
      //   }

      // }
      File.WriteAllText(@"C:\Users\Petter\source\repos\logoforprinter\Logo\text_logo.cpcl", result);


    }

    public static string ListToString(List<string> worksheet)
    {
      string itemToSave = "";
      foreach (var line in worksheet)
      {
        itemToSave += line + "\n";
      }

      return itemToSave;
    }

    public static List<string> ReverseWorksheet(List<string> worksheet)
    {


      for (int i = 0; i < worksheet.Count(); i++)
      {
        worksheet[i] = new string(worksheet[i].ToCharArray().Reverse().ToArray());
      }

      return worksheet;
    }



    public static void CalculateOffsets(string[] lines)
    {
      int xValue1 = 0;
      int xValue2 = 0;
      int yValue = 0;


      foreach (string line in lines)
      {
        string[] lineArray = line.Split(" ");

        yValue = Int32.Parse(lineArray[1]);
        xValue1 = Int32.Parse(lineArray[2]);
        xValue2 = Int32.Parse(lineArray[4]);

        MinY = CompareLowestValue(yValue, MinY);
        MaxY = CompareHighestValue(yValue, MaxY);

        MinX = CompareLowestValue(xValue1, MinX);
        MaxX = CompareHighestValue(xValue1, MaxX);

        MinX = CompareLowestValue(xValue2, MinX);
        MaxX = CompareHighestValue(xValue2, MaxX);
      }

      ReducedYBy = MinY;
      ReducedXBy = MinX;
    }

    // public static void ReduceXYValue()
    // {
    //   MinX -= ReducedXBy;
    //   MaxX -= ReducedYBy;
    // }

    public static int CompareLowestValue(int newValue, int presentValue)
    {
      return newValue < presentValue ? newValue : presentValue;

    }

    public static int CompareHighestValue(int newValue, int presentValue)
    {
      return newValue > presentValue ? newValue : presentValue;

    }

    public static List<string> CreateWorksheet()
    {
      var worksheet = new List<string>();
      string worksheetLine = "";
      for (int i = MinY - Margin; i <= MaxY + Margin; i++)
      {
        for (int j = MinX - Margin; j <= MaxX + Margin; j++)
        {
          worksheetLine += EmptySpace;
        }
        worksheet.Add(worksheetLine);
        worksheetLine = "";

      }

      return worksheet;

    }

    public static (int, int) SetValues(int value1, int value2)
    {
      if (value1 > value2)
      {
        int tempValue = value2;
        value2 = value1;
        value1 = tempValue;
      }
      value1 -= (ReducedXBy - Margin);
      value2 -= (ReducedXBy - Margin);
      return (value1, value2);
    }
    public static List<string> PopulateWorksheet(List<string> worksheet, string[] lines)
    {
      var worksheetListLine = new List<String>();

      int currentLine = 0;

      //==========================================
      // Adjust offset coordinates for focused image of logo.
      // Creates fields to be printed for each line.
      // Populate the worksheet with lines to be printed.
      foreach (var line in lines)
      {
        string[] lineArray = line.Split(" ");
        currentLine = Int32.Parse(lineArray[1]);
        currentLine -= (ReducedYBy - Margin);
        (int lowValue, int highValue) = SetValues(Int32.Parse(lineArray[2]), Int32.Parse(lineArray[4]));

        int fieldValue = Math.Abs(highValue) - (lowValue);
        string field = new string(FilledSpace, fieldValue);

        var aStringBuilder = new StringBuilder(worksheet[currentLine]);
        aStringBuilder.Remove(lowValue, fieldValue);
        aStringBuilder.Insert(lowValue, field);

        worksheet[currentLine] = aStringBuilder.ToString();

      }

      return worksheet;
    }
    public static List<string> ReverseLines(List<string> worksheet)
    {

      for (int i = 0; i < worksheet.Count(); i++)
      {
        string stringToReverse = new string(worksheet[i].ToCharArray().Reverse().ToArray());
        worksheet[i] = stringToReverse;
      }


      return worksheet;
    }
    public static List<string> AddLineNumber(List<string> worksheet, string[] lines)
    {
      var worksheetWithNumber = new List<string>();
      int currentLine = 0;
      foreach (var line in lines)
      {
        string[] lineArray = line.Split(" ");
        currentLine = Int32.Parse(lineArray[1]);

        var aStringBuilder = new StringBuilder(worksheet[currentLine]);

        aStringBuilder.Remove(0, lineArray[1].Length);
        aStringBuilder.Insert(0, lineArray[1]);


        worksheet[currentLine] = aStringBuilder.ToString();
      }

      return worksheet;

    }



  }
}
