using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using MelissaData;

namespace MelissaPhoneObjectLinuxDotnet
{
  class Program
  {
    static void Main(string[] args)
    {
      // Variables
      string license = "";
      string testPhone = "";
      string dataPath = "";

      ParseArguments(ref license, ref testPhone, ref dataPath, args);
      RunAsConsole(license, testPhone, dataPath);
    }

    static void ParseArguments (ref string license, ref string testPhone, ref string dataPath, string[] args )
    {
      for (int i = 0; i < args.Length; i++)
      {
        if (args[i].Equals("--license") || args[i].Equals("-l"))
        {
          if (args[i + 1] != null)
          {
            license = args[i + 1];
          }
        }
        if (args[i].Equals("--phone") || args[i].Equals("-p"))
        {
          if (args[i + 1] != null)
          {
            testPhone = args[i + 1];
          }
        }
        if (args[i].Equals("--dataPath") || args[i].Equals("-d"))
        {
          if (args[i + 1] != null)
          {
            dataPath = args[i + 1];
          }
        }
      }
    }

    static void RunAsConsole(string license, string testPhone, string dataPath)
    {
      Console.WriteLine("\n\n============ WELCOME TO MELISSA PHONE OBJECT LINUX DOTNET ==========\n");
      
      PhoneObject phoneObject = new PhoneObject(license, dataPath);

      bool shouldContinueRunning = true;

      if (phoneObject.mdPhoneObj.GetInitializeErrorString() != "No error")
      {
        shouldContinueRunning = false;
      }

      while (shouldContinueRunning)
      {
        DataContainer dataContainer = new DataContainer();

        if (string.IsNullOrEmpty(testPhone))
        {
          Console.WriteLine("\nFill in each value to see the Phone Object results");
          Console.WriteLine("Phone:");

          Console.CursorTop -= 1;
          Console.CursorLeft = 7;
          dataContainer.Phone = Console.ReadLine();

        }
        else
        {
          dataContainer.Phone = testPhone;
        }

        // Print user input
        Console.WriteLine("\n============================== INPUTS ==============================\n");
        Console.WriteLine($"\t                Phone: {dataContainer.Phone}");

        // Execute Phone Object
        phoneObject.ExecuteObjectAndResultCodes(ref dataContainer);

        // Print output
        Console.WriteLine("\n============================== OUTPUT ==============================\n");
        Console.WriteLine("\n\tPhone Object Information:");

        Console.WriteLine($"\t       Area Code: {phoneObject.mdPhoneObj.GetAreaCode()}");
        Console.WriteLine($"\t          Prefix: {phoneObject.mdPhoneObj.GetPrefix()}");
        Console.WriteLine($"\t          Suffix: {phoneObject.mdPhoneObj.GetSuffix()}");
        Console.WriteLine($"\t            City: {phoneObject.mdPhoneObj.GetCity()}");
        Console.WriteLine($"\t           State: {phoneObject.mdPhoneObj.GetState()}");
        Console.WriteLine($"\t        Latitude: {phoneObject.mdPhoneObj.GetLatitude()}");
        Console.WriteLine($"\t       Longitude: {phoneObject.mdPhoneObj.GetLongitude()}");
        Console.WriteLine($"\t       Time Zone: {phoneObject.mdPhoneObj.GetTimeZone()}");
        Console.WriteLine($"\t    Result Codes: {dataContainer.ResultCodes}");

        //Console.WriteLine($"\t New Area Code: {phoneObject.mdPhoneObj.GetNewAreaCode()}");
        //Console.WriteLine($"\t     Extension: {phoneObject.mdPhoneObj.GetExtension()}");
        //Console.WriteLine($"\t    CountyFips: {phoneObject.mdPhoneObj.GetCountyFips()}");
        //Console.WriteLine($"\t    CountyName: {phoneObject.mdPhoneObj.GetCountyName()}");
        //Console.WriteLine($"\t           Msa: {phoneObject.mdPhoneObj.GetMsa()}");
        //Console.WriteLine($"\t          Pmsa: {phoneObject.mdPhoneObj.GetPmsa()}");
        //Console.WriteLine($"\tTime Zone Code: {phoneObject.mdPhoneObj.GetTimeZoneCode()}");
        //Console.WriteLine($"\t  Country Code: {phoneObject.mdPhoneObj.GetCountryCode()}");
        //Console.WriteLine($"\t      Distance: {phoneObject.mdPhoneObj.GetDistance()}");

        String[] rs = dataContainer.ResultCodes.Split(',');
        foreach (String r in rs)
          Console.WriteLine($"        {r}: {phoneObject.mdPhoneObj.GetResultCodeDescription(r, mdPhone.ResultCdDescOpt.ResultCodeDescriptionLong)}");

        bool isValid = false;
        if (!string.IsNullOrEmpty(testPhone))
        {
          isValid = true;
          shouldContinueRunning = false;
        }
        while (!isValid)
        {
          Console.WriteLine("\nTest another phone? (Y/N)");
          string testAnotherResponse = Console.ReadLine();

          if (!string.IsNullOrEmpty(testAnotherResponse))
          {
            testAnotherResponse = testAnotherResponse.ToLower();
            if (testAnotherResponse == "y")
            {
              isValid = true;
            }
            else if (testAnotherResponse == "n")
            {
              isValid = true;
              shouldContinueRunning = false;
            }
            else
            {
              Console.Write("Invalid Response, please respond 'Y' or 'N'");
            }
          }
        }
      }
      Console.WriteLine("\n============ THANK YOU FOR USING MELISSA DOTNET OBJECT ===========\n");
    }
  }

  class PhoneObject
  {
    // Path to Phone Object data files (.dat, etc)
    string dataFilePath; 

    // Create instance of Melissa Phone Object
    public mdPhone mdPhoneObj = new mdPhone();

    public PhoneObject(string license, string dataPath)
    {
      // Set license string and set path to data files  (.dat, etc)
      mdPhoneObj.SetLicenseString(license);
      dataFilePath = dataPath;

      // If you see a different date than expected, check your license string and either download the new data files or use the Melissa Updater program to update your data files.  
      mdPhone.ProgramStatus pStatus = mdPhoneObj.Initialize(dataFilePath);

      if (pStatus != mdPhone.ProgramStatus.ErrorNone)
      {
        Console.WriteLine("Failed to Initialize Object.");
        Console.WriteLine(pStatus);
        return;
      }

      Console.WriteLine($"                DataBase Date: {mdPhoneObj.GetDatabaseDate()}");
      Console.WriteLine($"              Expiration Date: {mdPhoneObj.GetLicenseExpirationDate()}");

      /**
       * This number should match with file properties of the Melissa Object binary file.
       * If TEST appears with the build number, there may be a license key issue.
       */
      Console.WriteLine($"               Object Version: {mdPhoneObj.GetBuildNumber()}\n");
    }

    //This will call the lookup function to process the input phone as well as generate the result codes
    public void ExecuteObjectAndResultCodes(ref DataContainer data)
    {
      mdPhoneObj.Lookup(data.Phone, data.ZipCode);

      //mdPhoneObj.CorrectAreaCode(data.Phone, data.ZipCode);
      //mdPhoneObj.ComputeDistance(0.0, 0.0, 0.0, 0.0); 
      //mdPhoneObj.ComputeBearing(0.0, 0.0, 0.0, 0.0);

      data.ResultCodes = mdPhoneObj.GetResults();

      // ResultsCodes explain any issues Phone Object has with the object.
      // List of result codes for Phone Object
      // https://wiki.melissadata.com/?title=Result_Code_Details#Phone_Object
    }
  }

  public class DataContainer
  {
    public string Phone { get; set; }
    public string ZipCode { get; set; }
    public string ResultCodes { get; set; } = "";
  }
}
