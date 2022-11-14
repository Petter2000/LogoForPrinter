namespace logoforprinter.Models
{
  public class ImageToText
  {
    public static void FromImageToText(string input, string output)
    {
      //============================
      // Read image pattern
      //-----------------------
      string[] inputFile = File.ReadAllLines(input);
      var worksheet = new List<string>(inputFile);

      //===========================================
      // Extract worksheet offsets from image.
      // Returns a list without the offset lines.
      //---------------------------------------
      worksheet = WorksheetOffsets.PickupWorksheetOffsets(worksheet);

      //=====================================================================
      // Reverse worksheet for accurate printer input.
      // Returns the list of strings with the strings reversed (again).
      //---------------------------------------------------------------
      worksheet = WorksheetOffsets.ReverseWorksheet(worksheet);

      //==========================================================
      // Extract all filled spaces from image.
      // Returns a list with the index for all the filled spaces.
      //-------------------------------------------------------
      var listOfCoordinates = GetAllFilledSpaces(worksheet);

      //==================================================================
      // Create fields of filled spaces with subsequent index.
      // Returns a string with fields to print in the form readable cpcl.
      //---------------------------------------------------------------
      var result = CreateFieldsToPrint(listOfCoordinates);

      //===================
      // Save to file
      //-----------------
      File.WriteAllText(output, result);
    }

    //==========================================================
    // Collects all filled spaces from image.
    // Returns a list with the index for all the filled spaces.
    //-------------------------------------------------------
    public static List<string> GetAllFilledSpaces(List<string> worksheet)
    {
      var listOfCoordinates = new List<string>();
      var lineOfCoordinates = "";

      foreach (var line in worksheet)
      {
        char[] charLine = line.ToCharArray();

        for (int i = 0; i < charLine.Count(); i++)
        {
          if (charLine[i] == WorksheetOffsets.FilledSpace)
          {
            lineOfCoordinates += i + ",";
          }
        }

        listOfCoordinates.Add(lineOfCoordinates);
        lineOfCoordinates = "";
      }

      return listOfCoordinates;
    }

    //==================================================================
    // Creates fields of filled spaces with subsequent index
    // Returns a string with fields to print in the form readable cpcl
    //---------------------------------------------------------------
    public static string CreateFieldsToPrint(List<string> listOfCoordinates)
    {
      var workList = new List<string>();
      string result = "";

      //==============================================
      // Unpacks every coordinate row for processing.
      //--------------------------------------------
      for (int i = 0; i < listOfCoordinates.Count(); i++)
      {
        string[] lineArray = listOfCoordinates[i].Split(",");

        int lastLineNumber = 0;
        int lastElement = 0;
        int coordinateInt = 0;
        int comparedValue = 0;
        var coordinateIntList = new List<int>();

        //========================================
        // Process one coordinate row at a time.
        //--------------------------------------
        foreach (var item in lineArray)
        {
          if (item != "")
          {
            coordinateInt = Int32.Parse(item);
          }

          //=================================================================================
          // Checks if there is a subsequent index which should be added to the field line
          // or if the field line is completed and is ready to have its values extracted.
          //------------------------------------------------------------------------- 
          if (lastElement == 0 || coordinateInt == lastElement + 1)
          {
            coordinateIntList.Add(coordinateInt);
            lastElement = coordinateInt;
          }

          else
          {
            comparedValue = coordinateInt;
            var min = coordinateIntList.Min();
            var max = coordinateIntList.Max();

            //==========================================================
            // Adjust the offset coordinates back to original context.
            //--------------------------------------------------------
            (min, max) = SetValuesForText(min, max);

            //========================================
            // Add the complete print line to result.
            //-------------------------------------
            if (i != lastLineNumber)
            {
              result = AddPrintLine(workList, result);

              workList.Clear();
              lastLineNumber = i;
            }

            //============================================
            // The form that the printer reads one field.
            //----------------------------------------- 
            workList.Add
            ($"L {i + WorksheetOffsets.YMarginOffset} {max} {i + WorksheetOffsets.YMarginOffset} {min} {WorksheetOffsets.Width}\n");

            lastElement = comparedValue;
            coordinateIntList.Clear();
            coordinateIntList.Add(comparedValue);
          }
        }
      }

      // =====================================
      // Save the last pending print line.
      // ----------------------------------
      if (workList.Count() > 0)
      {
        result = AddPrintLine(workList, result);
      }

      return result;
    }

    //================================================================================
    // Add the field lines to a string in the correct order for the printer to read.
    //----------------------------------------------------------------------------
    public static string AddPrintLine(List<string> workList, string result)
    {
      workList.Reverse();
      return result += String.Join<string>(String.Empty, workList);
    }

    //===================================================
    // Adjusts the coordinates back to original context.
    //-----------------------------------------------
    static public (int, int) SetValuesForText(int value1, int value2)
    {
      var min = Math.Min(value1, value2);
      var max = Math.Max(value1, value2);

      min += (WorksheetOffsets.ReducedXBy - WorksheetOffsets.Margin);
      max += (WorksheetOffsets.ReducedXBy - WorksheetOffsets.Margin);

      return (min, max);
    }
  }
}