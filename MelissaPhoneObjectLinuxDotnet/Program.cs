using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using MelissaData;

namespace MelissaPhoneObjectLinuxDotnet
{
  /// <summary>
  /// Phone Object allows websites and custom applications to verify phone numbers down 
  /// to 7 and 10 digits, update area codes, and append data about the phone number.
  /// </summary>
  /// <remarks>
  /// High-level flow of this sample:
  ///   1. SETUP     - create an mdPhone instance, hand it the license string and the
  ///                  path to the data files, then Initialize() (one time).
  ///   2. INPUT     - feed a phone number in.
  ///   3. PROCESS   - Lookup() validates the number and appends its data.
  ///   4. READ      - pull the results back out with the Get* getters
  ///                  (GetAreaCode, GetCity, GetState, GetTimeZone, ...).
  ///   5. INTERPRET - GetResults() returns comma-separated result codes describing
  ///                  what the object did/found; each code has a human description.
  ///
  /// The pieces in this file map onto that flow:
  ///   - Program        : console harness (argument parsing + the interactive loop).
  ///   - PhoneObject    : thin wrapper around mdPhone that owns setup + the call sequence.
  ///   - DataContainer  : plain holder for one record's input and output.
  ///
  /// Where mdPhone comes from:
  ///   The MelissaData namespace and its mdPhone class live in mdPhone_cSharpCode.cs,
  ///   a generated C# wrapper over libmdPhone.so that the accompanying
  ///   MelissaPhoneObjectLinuxDotnet.sh script downloads on every run.
  ///
  /// Reference:
  ///   Quickstart    : https://docs.melissa.com/on-premise-api/phone-object/phone-object-quickstart.html
  ///   Release notes : https://releasenotes.melissa.com/on-premise-api/phone-object/
  ///   Result codes  : https://docs.melissa.com/on-premise-api/phone-object/result-codes.html
  /// </remarks>
  class Program
  {
    /// <summary>
    /// Entry point. Reads the optional command-line arguments, then hands control to
    /// RunAsConsole, which performs the actual Phone Object setup and processing.
    /// </summary>
    /// <param name="args">The raw command-line arguments</param>
    static void Main(string[] args)
    {
      // Populated by ParseArguments below.
      string license = "";
      string testPhone = "";
      string dataPath = "";

      ParseArguments(ref license, ref testPhone, ref dataPath, args);
      RunAsConsole(license, testPhone, dataPath);
    }

    /// <summary>
    /// Reads the supported command-line options into the ref parameters.
    ///
    /// Recognized flags (each followed by its value, e.g. "--phone 8002356766"):
    ///   --license / -l   : the Melissa license string
    ///   --phone / -p     : a phone number to test in one-shot mode
    ///   --dataPath / -d  : path to the Phone Object data files
    /// </summary>
    /// <param name="license">Receives the Melissa license string.</param>
    /// <param name="testPhone">Receives the phone number to test in one-shot mode.</param>
    /// <param name="dataPath">Receives the path to the Phone Object data files.</param>
    /// <param name="args">The raw command-line arguments to parse.</param>
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

    /// <summary>
    /// Sets up the Phone Object once, then drives the input -> process -> output cycle.
    ///
    /// In interactive mode (no --phone) it loops, asking for a new phone number each pass
    /// until the user answers "N". In one-shot mode (--phone supplied) it runs a single
    /// pass on testPhone and exits.
    /// </summary>
    /// <param name="license">The Melissa license string used to initialize the object.</param>
    /// <param name="testPhone">A phone number to process in one-shot mode; if empty, the program prompts interactively.</param>
    /// <param name="dataPath">Path to the Phone Object data files.</param>
    static void RunAsConsole(string license, string testPhone, string dataPath)
    {
      Console.WriteLine("\n\n============ WELCOME TO MELISSA PHONE OBJECT LINUX DOTNET ==========\n");
      
      // Construct the wrapper. This is where the object is licensed, pointed at the
      // data files, and initialized (see the PhoneObject constructor below).
      PhoneObject phoneObject = new PhoneObject(license, dataPath);

      bool shouldContinueRunning = true;

      // Gate the program on a successful initialization. If the data files could not
      // be loaded (bad/expired license, missing or wrong-path data files, ...),
      // GetInitializeErrorString() returns the reason instead of "No error" and we
      // skip the processing loop entirely.
      if (phoneObject.mdPhoneObj.GetInitializeErrorString() != "No error")
      {
        shouldContinueRunning = false;
      }

      while (shouldContinueRunning)
      {
        // Holder for this pass's input and result codes.
        DataContainer dataContainer = new DataContainer();

        if (string.IsNullOrEmpty(testPhone))
        {
          // Interactive mode: prompt the user for a phone number.
          Console.WriteLine("\nFill in each value to see the Phone Object results");
          Console.WriteLine("Phone:");

          Console.CursorTop -= 1;
          Console.CursorLeft = 7;
          dataContainer.Phone = Console.ReadLine();

        }
        else
        {
          // One-shot mode: use the phone number passed on the command line.
          dataContainer.Phone = testPhone;
        }

        // Print user input
        Console.WriteLine("\n============================== INPUTS ==============================\n");
        Console.WriteLine($"\t                Phone: {dataContainer.Phone}");

        // Execute Phone Object
        // Runs the Lookup and stores the result codes on dataContainer.
        phoneObject.ExecuteObjectAndResultCodes(ref dataContainer);

        // Print output
        // Each Get* getter below returns one component the object produced for the most
        // recently processed phone number. These read directly from the mdPhone instance,
        // which still holds the results from the Execute call above.
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

        // Other data the Phone Object can return - uncomment any you need:
        //Console.WriteLine($"\t New Area Code: {phoneObject.mdPhoneObj.GetNewAreaCode()}");
        //Console.WriteLine($"\t     Extension: {phoneObject.mdPhoneObj.GetExtension()}");
        //Console.WriteLine($"\t    CountyFips: {phoneObject.mdPhoneObj.GetCountyFips()}");
        //Console.WriteLine($"\t    CountyName: {phoneObject.mdPhoneObj.GetCountyName()}");
        //Console.WriteLine($"\t           Msa: {phoneObject.mdPhoneObj.GetMsa()}");
        //Console.WriteLine($"\t          Pmsa: {phoneObject.mdPhoneObj.GetPmsa()}");
        //Console.WriteLine($"\tTime Zone Code: {phoneObject.mdPhoneObj.GetTimeZoneCode()}");
        //Console.WriteLine($"\t  Country Code: {phoneObject.mdPhoneObj.GetCountryCode()}");
        //Console.WriteLine($"\t      Distance: {phoneObject.mdPhoneObj.GetDistance()}");

        // Result codes come back as a single comma-separated string (e.g. "PS01,PS08").
        // Split it and ask the object for a readable description of each code.
        // ResultCodeDescriptionLong requests the long-form text; a short form is also
        // available via ResultCodeDescriptionShort
        String[] rs = dataContainer.ResultCodes.Split(',');
        foreach (String r in rs)
          Console.WriteLine($"        {r}: {phoneObject.mdPhoneObj.GetResultCodeDescription(r, mdPhone.ResultCdDescOpt.ResultCodeDescriptionLong)}");

        bool isValid = false;

        // In one-shot mode there is nothing more to do after a single pass: mark the
        // input handled and stop the outer loop.
        if (!string.IsNullOrEmpty(testPhone))
        {
          isValid = true;
          shouldContinueRunning = false;
        }

        // Interactive mode: ask whether to process another phone number. Keep prompting
        // until we get a valid Y/N. "N" ends the program; "Y" falls through to another pass.
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

  /// <summary>
  /// Wrapper that owns a single Melissa Phone Object instance and encapsulates the two
  /// things every Melissa object needs: one-time setup (license + data files) and the
  /// per-record processing sequence. Reuse one instance across many numbers; do NOT
  /// re-initialize per number.
  /// </summary>
  class PhoneObject
  {
    // Path to the Phone Object data files.
    string dataFilePath; 

    // The underlying Melissa Phone Object instance.
    public mdPhone mdPhoneObj = new mdPhone();

    /// <summary>
    /// Performs the mandatory one-time setup, in this required order:
    ///   1. SetLicenseString - authorize the object.
    ///   2. Initialize       - point the object at the data files and load them.
    /// </summary>
    /// <param name="license">The Melissa license string used to authorize the object.</param>
    /// <param name="dataPath">Path to the folder containing the Phone Object data files.</param>
    public PhoneObject(string license, string dataPath)
    {
      // Set license string and set path to data files
      mdPhoneObj.SetLicenseString(license);
      dataFilePath = dataPath;

      // Point the object at the data files and load them. The returned ProgramStatus
      // reports whether initialization succeeded.
      // If you see a different date than expected, check your license string and either download the new data files
      // or use the Melissa Updater program to update your data files.
      mdPhone.ProgramStatus pStatus = mdPhoneObj.Initialize(dataFilePath);

      // If an issue occurred, please investigate the common causes.
      // Common causes: an invalid/expired license, or missing/wrong-path data files.
      if (pStatus != mdPhone.ProgramStatus.ErrorNone)
      {
        Console.WriteLine("Failed to Initialize Object.");
        Console.WriteLine(pStatus);
        return;
      }

      // Diagnostic information, handy for confirming the object loaded the data you expect:

      // Build date of the data files
      Console.WriteLine($"                DataBase Date: {mdPhoneObj.GetDatabaseDate()}");

      // When the license stops working
      Console.WriteLine($"              Expiration Date: {mdPhoneObj.GetLicenseExpirationDate()}");

      // This number should match with the file properties of the Melissa Object binary file.
      // If TEST appears with the build number, there may be a license key issue.
      Console.WriteLine($"               Object Version: {mdPhoneObj.GetBuildNumber()}\n");
    }

    /// <summary>
    /// Runs the full Phone Object processing sequence for one phone number and captures
    /// its result codes. This is the canonical per-record call pattern to copy into your
    /// own application:
    ///   Lookup -> GetResults
    /// </summary>
    /// <param name="data">
    /// The record to process. Its Phone is read as input, and ResultCodes is populated
    /// with this run's result codes.
    /// </param>
    public void ExecuteObjectAndResultCodes(ref DataContainer data)
    {

      // Validate the number and append its data
      mdPhoneObj.Lookup(data.Phone, data.ZipCode);

      // Other Phone Object operations, available if you need them:
      //mdPhoneObj.CorrectAreaCode(data.Phone, data.ZipCode);
      //mdPhoneObj.ComputeDistance(0.0, 0.0, 0.0, 0.0);
      //mdPhoneObj.ComputeBearing(0.0, 0.0, 0.0, 0.0);

      // Collect the result codes for this run
      // ResultsCodes explain any issues Phone Object has with the object.
      // List of result codes for Phone Object
      // https://docs.melissa.com/on-premise-api/phone-object/result-codes.html
      data.ResultCodes = mdPhoneObj.GetResults();
    }
  }

  /// <summary>
  /// Data holder for a single record: carries the input phone number in and the result
  /// codes out.
  /// </summary>
  public class DataContainer
  {
    // Input: the phone number to process.
    public string Phone { get; set; }

    // Input: optional ZIP code passed to Lookup to help disambiguate the number.
    public string ZipCode { get; set; }

    // Output: comma-separated result codes from GetResults().
    public string ResultCodes { get; set; } = "";
  }
}
