namespace logoforprinter.Models
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
    static public int YMarginOffset { get; set; } = 0;
    static public char FilledSpace { get; set; } = 'X';
    static public char EmptySpace { get; set; } = '.';
    static public int Width { get; set; } = 1;

    //===============================================================
    // Adds the worksheet offset numbers at the start of the image.
    //------------------------------------------------------------
    public static List<string> AddWorkseetOffsets(List<string> worksheet)
    {
      worksheet.Insert(0, $"Margin {Margin}");
      worksheet.Insert(0, $"MinX {MinX}");
      worksheet.Insert(0, $"MinY {MinY}");

      return worksheet;
    }

    //===========================================
    // Extract worksheet offsets from image.
    // Returns a list without the offset lines.
    //---------------------------------------
    public static List<string> PickupWorksheetOffsets(List<string> worksheet)
    {
      bool done = false;
      while (!done)
      {
        var offsets = worksheet[0].Split(" ");
        switch (offsets[0])
        {
          case "MinY":
            MinY = Int32.Parse(offsets[1]);
            worksheet.Remove(worksheet[0]);
            break;

          case "MinX":
            MinX = Int32.Parse(offsets[1]);
            worksheet.Remove(worksheet[0]);
            break;

          case "Margin":
            Margin = Int32.Parse(offsets[1]);
            worksheet.Remove(worksheet[0]);
            break;

          default:
            SetOffsets();
            done = true;
            break;
        }
      }

      return worksheet;
    }

    //=====================================
    // Sets the offsets for the worksheet.
    //-----------------------------------
    public static void SetOffsets()
    {
      ReducedYBy = MinY;
      ReducedXBy = MinX;
      YMarginOffset = MinY - Margin;
    }

    //=======================================================
    // Returns a list of strings with the strings reversed.
    //----------------------------------------------------
    public static List<string> ReverseWorksheet(List<string> worksheet)
    {
      for (int i = 0; i < worksheet.Count(); i++)
      {
        worksheet[i] = new string(worksheet[i].ToCharArray().Reverse().ToArray());
      }

      return worksheet;
    }

    //====================================================
    // Calculate offset values for Worksheet.
    //-------------------------------------------------
    public static void CalculateOffsets(string[] lines)
    {
      foreach (string line in lines)
      {
        string[] lineArray = line.Split(" ");

        var yValue = Int32.Parse(lineArray[1]);
        var xValue1 = Int32.Parse(lineArray[2]);
        var xValue2 = Int32.Parse(lineArray[4]);

        MinY = Math.Min(yValue, MinY);
        MaxY = Math.Max(yValue, MaxY);

        MinX = Math.Min(xValue1, MinX);
        MaxX = Math.Max(xValue1, MaxX);

        MinX = Math.Min(xValue2, MinX);
        MaxX = Math.Max(xValue2, MaxX);
      }

      SetOffsets();
    }

    //================================================
    // Compares two values. Returns the lowest value.
    //----------------------------------------------
    public static int CompareLowestValue(int newValue, int presentValue)
    {
      return newValue < presentValue ? newValue : presentValue;
    }

    //================================================
    // Compares two values. Returns the highest value.
    //----------------------------------------------
    public static int CompareHighestValue(int newValue, int presentValue)
    {
      return newValue > presentValue ? newValue : presentValue;
    }
  }
}
