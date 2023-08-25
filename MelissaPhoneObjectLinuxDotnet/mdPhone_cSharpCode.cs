using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;

namespace MelissaData {
	public class mdPhone : IDisposable {
		private IntPtr i;

		public enum ProgramStatus {
			ErrorNone = 0,
			ErrorOther = 1,
			ErrorOutOfMemory = 2,
			ErrorRequiredFileNotFound = 3,
			ErrorFoundOldFile = 4,
			ErrorDatabaseExpired = 5,
			ErrorLicenseExpired = 6
		}
		public enum AccessType {
			Local = 0,
			Remote = 1
		}
		public enum DiacriticsMode {
			Auto = 0,
			On = 1,
			Off = 2
		}
		public enum StandardizeMode {
			ShortFormat = 0,
			LongFormat = 1,
			AutoFormat = 2
		}
		public enum SuiteParseMode {
			ParseSuite = 0,
			CombineSuite = 1
		}
		public enum AliasPreserveMode {
			ConvertAlias = 0,
			PreserveAlias = 1
		}
		public enum AutoCompletionMode {
			AutoCompleteSingleSuite = 0,
			AutoCompleteRangedSuite = 1,
			AutoCompletePlaceHolderSuite = 2,
			AutoCompleteNoSuite = 3
		}
		public enum ResultCdDescOpt {
			ResultCodeDescriptionLong = 0,
			ResultCodeDescriptionShort = 1
		}
		public enum MailboxLookupMode {
			MailboxNone = 0,
			MailboxExpress = 1,
			MailboxPremium = 2
		}

		[SuppressUnmanagedCodeSecurity]
		private class mdPhoneUnmanaged {
			[DllImport("mdPhone", EntryPoint = "mdPhoneCreate", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneCreate();
			[DllImport("mdPhone", EntryPoint = "mdPhoneDestroy", CallingConvention = CallingConvention.Cdecl)]
			public static extern void mdPhoneDestroy(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneInitialize", CallingConvention = CallingConvention.Cdecl)]
			public static extern Int32 mdPhoneInitialize(IntPtr i, IntPtr p1);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetInitializeErrorString", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetInitializeErrorString(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneSetLicenseString", CallingConvention = CallingConvention.Cdecl)]
			public static extern Int32 mdPhoneSetLicenseString(IntPtr i, IntPtr p1);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetLicenseExpirationDate", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetLicenseExpirationDate(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetBuildNumber", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetBuildNumber(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetDatabaseDate", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetDatabaseDate(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneLookup", CallingConvention = CallingConvention.Cdecl)]
			public static extern Int32 mdPhoneLookup(IntPtr i, IntPtr phone, IntPtr zip);
			[DllImport("mdPhone", EntryPoint = "mdPhoneCorrectAreaCode", CallingConvention = CallingConvention.Cdecl)]
			public static extern Int32 mdPhoneCorrectAreaCode(IntPtr i, IntPtr phone, IntPtr zip);
			[DllImport("mdPhone", EntryPoint = "mdPhoneComputeDistance", CallingConvention = CallingConvention.Cdecl)]
			public static extern double mdPhoneComputeDistance(IntPtr i, double p1, double p2, double p3, double p4);
			[DllImport("mdPhone", EntryPoint = "mdPhoneComputeBearing", CallingConvention = CallingConvention.Cdecl)]
			public static extern double mdPhoneComputeBearing(IntPtr i, double p1, double p2, double p3, double p4);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetAreaCode", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetAreaCode(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetNewAreaCode", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetNewAreaCode(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetPrefix", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetPrefix(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetSuffix", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetSuffix(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetExtension", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetExtension(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetCity", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetCity(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetState", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetState(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetCountyFips", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetCountyFips(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetCountyName", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetCountyName(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetMsa", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetMsa(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetPmsa", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetPmsa(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetTimeZone", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetTimeZone(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetTimeZoneCode", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetTimeZoneCode(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetCountryCode", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetCountryCode(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetLatitude", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetLatitude(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetLongitude", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetLongitude(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetDistance", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetDistance(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetResults", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetResults(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetResultCodeDescription", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetResultCodeDescription(IntPtr i, IntPtr resultCode, Int32 opt);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetStatusCode", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetStatusCode(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdPhoneGetErrorCode", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdPhoneGetErrorCode(IntPtr i);
		}

		public mdPhone() {
			i = mdPhoneUnmanaged.mdPhoneCreate();
		}

		~mdPhone() {
			Dispose();
		}

		public virtual void Dispose() {
			lock (this) {
				mdPhoneUnmanaged.mdPhoneDestroy(i);
				GC.SuppressFinalize(this);
			}
		}

		public ProgramStatus Initialize(string p1) {
			Utf8String u_p1 = new Utf8String(p1);
			return (ProgramStatus)mdPhoneUnmanaged.mdPhoneInitialize(i, u_p1.GetUtf8Ptr());
		}

		public string GetInitializeErrorString() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetInitializeErrorString(i));
		}

		public bool SetLicenseString(string p1) {
			Utf8String u_p1 = new Utf8String(p1);
			return (mdPhoneUnmanaged.mdPhoneSetLicenseString(i, u_p1.GetUtf8Ptr()) != 0);
		}

		public string GetLicenseExpirationDate() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetLicenseExpirationDate(i));
		}

		public string GetBuildNumber() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetBuildNumber(i));
		}

		public string GetDatabaseDate() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetDatabaseDate(i));
		}

		public bool Lookup(string phone, string zip) {
			Utf8String u_phone = new Utf8String(phone);
			Utf8String u_zip = new Utf8String(zip);
			return (mdPhoneUnmanaged.mdPhoneLookup(i, u_phone.GetUtf8Ptr(), u_zip.GetUtf8Ptr()) != 0);
		}

		public bool Lookup(string phone) {
			Utf8String u_phone = new Utf8String(phone);
			Utf8String u_zip = new Utf8String("");
			return (mdPhoneUnmanaged.mdPhoneLookup(i, u_phone.GetUtf8Ptr(), u_zip.GetUtf8Ptr()) != 0);
		}

		public bool CorrectAreaCode(string phone, string zip) {
			Utf8String u_phone = new Utf8String(phone);
			Utf8String u_zip = new Utf8String(zip);
			return (mdPhoneUnmanaged.mdPhoneCorrectAreaCode(i, u_phone.GetUtf8Ptr(), u_zip.GetUtf8Ptr()) != 0);
		}

		public double ComputeDistance(double p1, double p2, double p3, double p4) {
			return mdPhoneUnmanaged.mdPhoneComputeDistance(i, p1, p2, p3, p4);
		}

		public double ComputeBearing(double p1, double p2, double p3, double p4) {
			return mdPhoneUnmanaged.mdPhoneComputeBearing(i, p1, p2, p3, p4);
		}

		public string GetAreaCode() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetAreaCode(i));
		}

		public string GetNewAreaCode() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetNewAreaCode(i));
		}

		public string GetPrefix() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetPrefix(i));
		}

		public string GetSuffix() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetSuffix(i));
		}

		public string GetExtension() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetExtension(i));
		}

		public string GetCity() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetCity(i));
		}

		public string GetState() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetState(i));
		}

		public string GetCountyFips() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetCountyFips(i));
		}

		public string GetCountyName() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetCountyName(i));
		}

		public string GetMsa() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetMsa(i));
		}

		public string GetPmsa() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetPmsa(i));
		}

		public string GetTimeZone() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetTimeZone(i));
		}

		public string GetTimeZoneCode() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetTimeZoneCode(i));
		}

		public string GetCountryCode() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetCountryCode(i));
		}

		public string GetLatitude() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetLatitude(i));
		}

		public string GetLongitude() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetLongitude(i));
		}

		public string GetDistance() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetDistance(i));
		}

		public string GetResults() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetResults(i));
		}

		public string GetResultCodeDescription(string resultCode, ResultCdDescOpt opt) {
			Utf8String u_resultCode = new Utf8String(resultCode);
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetResultCodeDescription(i, u_resultCode.GetUtf8Ptr(), (int)opt));
		}

		public string GetResultCodeDescription(string resultCode) {
			Utf8String u_resultCode = new Utf8String(resultCode);
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetResultCodeDescription(i, u_resultCode.GetUtf8Ptr(), (int)ResultCdDescOpt.ResultCodeDescriptionLong));
		}

		public string GetStatusCode() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetStatusCode(i));
		}

		public string GetErrorCode() {
			return Utf8String.GetUnicodeString(mdPhoneUnmanaged.mdPhoneGetErrorCode(i));
		}

		private class Utf8String : IDisposable {
			private IntPtr utf8String = IntPtr.Zero;

			public Utf8String(string str) {
				if (str == null)
					str = "";
				byte[] buffer = Encoding.UTF8.GetBytes(str);
				Array.Resize(ref buffer, buffer.Length + 1);
				buffer[buffer.Length - 1] = 0;
				utf8String = Marshal.AllocHGlobal(buffer.Length);
				Marshal.Copy(buffer, 0, utf8String, buffer.Length);
			}

			~Utf8String() {
				Dispose();
			}

			public virtual void Dispose() {
				lock (this) {
					Marshal.FreeHGlobal(utf8String);
					GC.SuppressFinalize(this);
				}
			}

			public IntPtr GetUtf8Ptr() {
				return utf8String;
			}

			public static string GetUnicodeString(IntPtr ptr) {
				if (ptr == IntPtr.Zero)
					return "";
				int len = 0;
				while (Marshal.ReadByte(ptr, len) != 0)
					len++;
				if (len == 0)
					return "";
				byte[] buffer = new byte[len];
				Marshal.Copy(ptr, buffer, 0, len);
				return Encoding.UTF8.GetString(buffer);
			}
		}
	}

	public class mdGlobalPhone : IDisposable {
		private IntPtr i;

		public enum ProgramStatus {
			ErrorNone = 0,
			ErrorOther = 1,
			ErrorOutOfMemory = 2,
			ErrorRequiredFileNotFound = 3,
			ErrorFoundOldFile = 4,
			ErrorDatabaseExpired = 5,
			ErrorLicenseExpired = 6
		}
		public enum AccessType {
			Local = 0,
			Remote = 1
		}
		public enum DiacriticsMode {
			Auto = 0,
			On = 1,
			Off = 2
		}
		public enum StandardizeMode {
			ShortFormat = 0,
			LongFormat = 1,
			AutoFormat = 2
		}
		public enum SuiteParseMode {
			ParseSuite = 0,
			CombineSuite = 1
		}
		public enum AliasPreserveMode {
			ConvertAlias = 0,
			PreserveAlias = 1
		}
		public enum AutoCompletionMode {
			AutoCompleteSingleSuite = 0,
			AutoCompleteRangedSuite = 1,
			AutoCompletePlaceHolderSuite = 2,
			AutoCompleteNoSuite = 3
		}
		public enum ResultCdDescOpt {
			ResultCodeDescriptionLong = 0,
			ResultCodeDescriptionShort = 1
		}
		public enum MailboxLookupMode {
			MailboxNone = 0,
			MailboxExpress = 1,
			MailboxPremium = 2
		}

		[SuppressUnmanagedCodeSecurity]
		private class mdGlobalPhoneUnmanaged {
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneCreate", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneCreate();
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneDestroy", CallingConvention = CallingConvention.Cdecl)]
			public static extern void mdGlobalPhoneDestroy(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneInitialize", CallingConvention = CallingConvention.Cdecl)]
			public static extern Int32 mdGlobalPhoneInitialize(IntPtr i, IntPtr p1);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneGetInitializeErrorString", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneGetInitializeErrorString(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneSetLicenseString", CallingConvention = CallingConvention.Cdecl)]
			public static extern Int32 mdGlobalPhoneSetLicenseString(IntPtr i, IntPtr p1);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneGetLicenseExpirationDate", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneGetLicenseExpirationDate(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneGetBuildNumber", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneGetBuildNumber(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneGetDatabaseDate", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneGetDatabaseDate(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneLookup", CallingConvention = CallingConvention.Cdecl)]
			public static extern Int32 mdGlobalPhoneLookup(IntPtr i, IntPtr phone, IntPtr country, IntPtr origcountry);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneLookupNext", CallingConvention = CallingConvention.Cdecl)]
			public static extern Int32 mdGlobalPhoneLookupNext(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneGetPhoneNumber", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneGetPhoneNumber(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneGetSubscriberNumber", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneGetSubscriberNumber(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneGetCountry", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneGetCountry(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneGetCountryCode", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneGetCountryCode(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneGetInternationalPrefix", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneGetInternationalPrefix(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneGetNationPrefix", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneGetNationPrefix(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneGetNationalDestinationCode", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneGetNationalDestinationCode(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneGetLanguage", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneGetLanguage(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneGetAdministrativeArea", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneGetAdministrativeArea(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneGetLocality", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneGetLocality(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneGetUTC", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneGetUTC(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneGetDST", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneGetDST(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneGetLatitude", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneGetLatitude(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneGetLongitude", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneGetLongitude(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneGetResults", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneGetResults(IntPtr i);
			[DllImport("mdPhone", EntryPoint = "mdGlobalPhoneGetResultCodeDescription", CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr mdGlobalPhoneGetResultCodeDescription(IntPtr i, IntPtr resultCode, Int32 opt);
		}

		public mdGlobalPhone() {
			i = mdGlobalPhoneUnmanaged.mdGlobalPhoneCreate();
		}

		~mdGlobalPhone() {
			Dispose();
		}

		public virtual void Dispose() {
			lock (this) {
				mdGlobalPhoneUnmanaged.mdGlobalPhoneDestroy(i);
				GC.SuppressFinalize(this);
			}
		}

		public ProgramStatus Initialize(string p1) {
			Utf8String u_p1 = new Utf8String(p1);
			return (ProgramStatus)mdGlobalPhoneUnmanaged.mdGlobalPhoneInitialize(i, u_p1.GetUtf8Ptr());
		}

		public string GetInitializeErrorString() {
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetInitializeErrorString(i));
		}

		public bool SetLicenseString(string p1) {
			Utf8String u_p1 = new Utf8String(p1);
			return (mdGlobalPhoneUnmanaged.mdGlobalPhoneSetLicenseString(i, u_p1.GetUtf8Ptr()) != 0);
		}

		public string GetLicenseExpirationDate() {
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetLicenseExpirationDate(i));
		}

		public string GetBuildNumber() {
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetBuildNumber(i));
		}

		public string GetDatabaseDate() {
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetDatabaseDate(i));
		}

		public bool Lookup(string phone, string country, string origcountry) {
			Utf8String u_phone = new Utf8String(phone);
			Utf8String u_country = new Utf8String(country);
			Utf8String u_origcountry = new Utf8String(origcountry);
			return (mdGlobalPhoneUnmanaged.mdGlobalPhoneLookup(i, u_phone.GetUtf8Ptr(), u_country.GetUtf8Ptr(), u_origcountry.GetUtf8Ptr()) != 0);
		}

		public bool Lookup(string phone, string country) {
			Utf8String u_phone = new Utf8String(phone);
			Utf8String u_country = new Utf8String(country);
			Utf8String u_origcountry = new Utf8String("");
			return (mdGlobalPhoneUnmanaged.mdGlobalPhoneLookup(i, u_phone.GetUtf8Ptr(), u_country.GetUtf8Ptr(), u_origcountry.GetUtf8Ptr()) != 0);
		}

		public bool Lookup(string phone) {
			Utf8String u_phone = new Utf8String(phone);
			Utf8String u_country = new Utf8String("");
			Utf8String u_origcountry = new Utf8String("");
			return (mdGlobalPhoneUnmanaged.mdGlobalPhoneLookup(i, u_phone.GetUtf8Ptr(), u_country.GetUtf8Ptr(), u_origcountry.GetUtf8Ptr()) != 0);
		}

		public bool LookupNext() {
			return (mdGlobalPhoneUnmanaged.mdGlobalPhoneLookupNext(i) != 0);
		}

		public string GetPhoneNumber() {
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetPhoneNumber(i));
		}

		public string GetSubscriberNumber() {
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetSubscriberNumber(i));
		}

		public string GetCountry() {
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetCountry(i));
		}

		public string GetCountryCode() {
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetCountryCode(i));
		}

		public string GetInternationalPrefix() {
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetInternationalPrefix(i));
		}

		public string GetNationPrefix() {
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetNationPrefix(i));
		}

		public string GetNationalDestinationCode() {
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetNationalDestinationCode(i));
		}

		public string GetLanguage() {
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetLanguage(i));
		}

		public string GetAdministrativeArea() {
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetAdministrativeArea(i));
		}

		public string GetLocality() {
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetLocality(i));
		}

		public string GetUTC() {
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetUTC(i));
		}

		public string GetDST() {
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetDST(i));
		}

		public string GetLatitude() {
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetLatitude(i));
		}

		public string GetLongitude() {
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetLongitude(i));
		}

		public string GetResults() {
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetResults(i));
		}

		public string GetResultCodeDescription(string resultCode, ResultCdDescOpt opt) {
			Utf8String u_resultCode = new Utf8String(resultCode);
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetResultCodeDescription(i, u_resultCode.GetUtf8Ptr(), (int)opt));
		}

		public string GetResultCodeDescription(string resultCode) {
			Utf8String u_resultCode = new Utf8String(resultCode);
			return Utf8String.GetUnicodeString(mdGlobalPhoneUnmanaged.mdGlobalPhoneGetResultCodeDescription(i, u_resultCode.GetUtf8Ptr(), (int)ResultCdDescOpt.ResultCodeDescriptionLong));
		}

		private class Utf8String : IDisposable {
			private IntPtr utf8String = IntPtr.Zero;

			public Utf8String(string str) {
				if (str == null)
					str = "";
				byte[] buffer = Encoding.UTF8.GetBytes(str);
				Array.Resize(ref buffer, buffer.Length + 1);
				buffer[buffer.Length - 1] = 0;
				utf8String = Marshal.AllocHGlobal(buffer.Length);
				Marshal.Copy(buffer, 0, utf8String, buffer.Length);
			}

			~Utf8String() {
				Dispose();
			}

			public virtual void Dispose() {
				lock (this) {
					Marshal.FreeHGlobal(utf8String);
					GC.SuppressFinalize(this);
				}
			}

			public IntPtr GetUtf8Ptr() {
				return utf8String;
			}

			public static string GetUnicodeString(IntPtr ptr) {
				if (ptr == IntPtr.Zero)
					return "";
				int len = 0;
				while (Marshal.ReadByte(ptr, len) != 0)
					len++;
				if (len == 0)
					return "";
				byte[] buffer = new byte[len];
				Marshal.Copy(ptr, buffer, 0, len);
				return Encoding.UTF8.GetString(buffer);
			}
		}
	}
}
