using System.Text;

namespace logoforprinter.Models
{
  public class TextToImage
  {
    public static void FromTextToImage(string input, string output)
    {
      //=========================
      // Read print file commands
      //------------------------
      string[] lines = File.ReadAllLines(input);

      //====================================================
      // Calculate offset values for Worksheet.
      //-------------------------------------------------
      WorksheetOffsets.CalculateOffsets(lines);

      //===============================================
      // Create empty worksheet. 
      // Returns a list of strings with empty spaces.
      //---------------------------------------------
      var worksheet = CreateWorksheet();

      //===========================================================
      // Populate worsksheet using coordinates.
      // Returns the list of strings with added filled spaces.
      // This needs to be done seperatly due to one printede line 
      // is made out of several lines of text.
      //--------------------------------------------------------
      worksheet = PopulateWorksheet(worksheet, lines);

      //=====================================================================
      // Reverse worksheet for accurate viewing.
      // Returns the list of strings with the strings reversed.
      //---------------------------------------------------------------
      worksheet = WorksheetOffsets.ReverseWorksheet(worksheet);

      //======================================================
      // Add line number (using offset) for accurate editing.
      // Returns the list of strings with added line numbers. 
      //---------------------------------------------------
      worksheet = AddLineNumber(worksheet, lines);

      //====================================================
      // Add the worksheet offset numbers to the image.
      //-------------------------------------------------
      worksheet = WorksheetOffsets.AddWorkseetOffsets(worksheet);

      //================================================
      // Convert the worksheet list to a savable item.
      //----------------------------------------------
      string result = string.Join(Environment.NewLine, worksheet.ToArray());

      //======================
      // Save to file.
      //--------------------
      File.WriteAllText(output, result);
    }

    //===============================================
    // Creates empty worksheet. 
    // Returns a list of strings with blank spaces.
    //---------------------------------------------
    public static List<string> CreateWorksheet()
    {
      var worksheet = new List<string>();
      string worksheetLine = "";

      for (int i = WorksheetOffsets.MinY - WorksheetOffsets.Margin; i <= WorksheetOffsets.MaxY + WorksheetOffsets.Margin; i++)
      {
        for (int j = WorksheetOffsets.MinX - WorksheetOffsets.Margin; j <= WorksheetOffsets.MaxX + WorksheetOffsets.Margin; j++)
        {
          worksheetLine += WorksheetOffsets.EmptySpace;
        }

        worksheet.Add(worksheetLine);
        worksheetLine = "";
      }

      return worksheet;
    }

    //=======================================================
    // Adjusts offset coordinates for focused image of logo.
    //----------------------------------------------------
    public static (int, int) SetValuesForImage(int value1, int value2)
    {
      var min = Math.Min(value1, value2);
      var max = Math.Max(value1, value2);

      min -= (WorksheetOffsets.ReducedXBy - WorksheetOffsets.Margin);
      max -= (WorksheetOffsets.ReducedXBy - WorksheetOffsets.Margin);

      return (min, max);
    }

    //===========================================================
    // Populates worsksheet with coordinates.
    // Returns the list of strings with added filled spaces.
    //-----------------------------------------------------
    public static List<string> PopulateWorksheet(List<string> worksheet, string[] lines)
    {
      var worksheetListLine = new List<String>();

      //==========================================
      // Adjust offset coordinates for focused image of logo.
      // Creates fields to be printed for each line.
      // Populate the worksheet with lines to be printed.
      foreach (var line in lines)
      {
        string[] lineArray = line.Split(" ");
        int currentLine = Int32.Parse(lineArray[1]);
        currentLine -= (WorksheetOffsets.ReducedYBy - WorksheetOffsets.Margin);
        (int lowValue, int highValue) = SetValuesForImage(Int32.Parse(lineArray[2]), Int32.Parse(lineArray[4]));

        int fieldLength = highValue - lowValue + 1;
        string field = new string(WorksheetOffsets.FilledSpace, fieldLength);

        var aStringBuilder = new StringBuilder(worksheet[currentLine]);
        aStringBuilder.Remove(lowValue, fieldLength);
        aStringBuilder.Insert(lowValue, field);
        worksheet[currentLine] = aStringBuilder.ToString();
      }

      return worksheet;
    }

    //======================================================
    // Add line number (using offset) for accurate editing.
    // Returns the list of strings with added line number. 
    //---------------------------------------------------
    public static List<string> AddLineNumber(List<string> worksheet, string[] lines)
    {
      for (int i = 0; i < worksheet.Count(); i++)
      {
        var aStringBuilder = new StringBuilder(worksheet[i]);
        aStringBuilder.Remove(0, (i + WorksheetOffsets.YMarginOffset).ToString().Count());
        aStringBuilder.Insert(0, i + WorksheetOffsets.YMarginOffset);

        worksheet[i] = aStringBuilder.ToString();
      }

      return worksheet;
    }
  }
}