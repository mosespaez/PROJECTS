using System.Runtime.Serialization;

namespace BevChain.Integration.Portable
{
  [DataContract]
  public enum ConditionType
  {
    [EnumMember(Value = "0")] And,
    [EnumMember(Value = "1")] Or,
    [EnumMember(Value = "2")] AndNot,
    [EnumMember(Value = "3")] OrNot,
  }

  [DataContract]
  public enum ConnectionType
  {
    [EnumMember(Value = "0")] Connect,
    [EnumMember(Value = "1")] Email,
    [EnumMember(Value = "2")] Http,
    [EnumMember(Value = "3")] AS2,
    [EnumMember(Value = "4")] FTP,
    [EnumMember(Value = "5")] WebService,
    [EnumMember(Value = "6")] AzureStorage,
    }

  [DataContract]
  public enum Consumer
  {
    [EnumMember(Value = "0")] UnAssigned,
    [EnumMember(Value = "1")] FileTask,
    [EnumMember(Value = "2")] ScheduledTask,
    [EnumMember(Value = "3")] PriorityTask,
  }

  [DataContract]
  public enum DocumentFileType
  {
    [EnumMember(Value = "0")] Xls,
    [EnumMember(Value = "1")] Xlsx,
    [EnumMember(Value = "2")] Doc,
    [EnumMember(Value = "3")] Docx,
    [EnumMember(Value = "4")] Ppt,
    [EnumMember(Value = "5")] Pptx,
    [EnumMember(Value = "6")] Zip,
  }

  [DataContract]
  public enum EmailField
  {
    [EnumMember(Value = "0")] ReceivedFrom,
    [EnumMember(Value = "1")] NumberOfAttachments,
  }

  [DataContract]
  public enum ExecutionEnvironment
  {
    [EnumMember(Value = "0")] Development,
    [EnumMember(Value = "1")] Test,
    [EnumMember(Value = "2")] Staging,
    [EnumMember(Value = "3")] Production,
  }

  [DataContract]
  public enum FieldType
  {
    [EnumMember(Value = "0")] Filename,
    [EnumMember(Value = "1")] Tag,
    [EnumMember(Value = "2")] SourceFilename,
    [EnumMember(Value = "3")] FileType,
    [EnumMember(Value = "4")] From,
  }

  [DataContract]
  public enum FileField
  {
    [EnumMember(Value = "0")] FileName,
    [EnumMember(Value = "1")] CreatedOn,
  }

  [DataContract]
  public enum FileType
  {
    [EnumMember(Value = "0")] Text,
    [EnumMember(Value = "1")] Email,
    [EnumMember(Value = "2")] Xml,
    [EnumMember(Value = "3")] Binary,
    [EnumMember(Value = "4")] Document,
    [EnumMember(Value = "5")] Database,
    [EnumMember(Value = "6")] Sms,
  }

  [DataContract]
  public enum IOType
  {
    [EnumMember(Value = "0")] Inbound,
    [EnumMember(Value = "1")] Outbound,
  }

  [DataContract]
  public enum Notification
  {
    [EnumMember(Value = "0")] Error,
    [EnumMember(Value = "1")] Warningorerror,
    [EnumMember(Value = "2")] Always,
  }

  [DataContract]
  public enum OperatorType
  {
    [EnumMember(Value = "0")] Equals,
    [EnumMember(Value = "1")] NotEquals,
    [EnumMember(Value = "2")] IsGreaterThan,
    [EnumMember(Value = "3")] IsLessThan,
    [EnumMember(Value = "4")] IsGreaterThanOrEqualTo,
    [EnumMember(Value = "5")] IsLessThanOrEqualTo,
    [EnumMember(Value = "6")] Contains,
    [EnumMember(Value = "7")] DoesNotContain,
    [EnumMember(Value = "8")] IsBetween,
    [EnumMember(Value = "9")] IsNotBetween,
    [EnumMember(Value = "10")] BeginsWith,
    [EnumMember(Value = "11")] EndsWith,
    [EnumMember(Value = "12")] DoesNotBeginWith,
    [EnumMember(Value = "13")] DoesNotEndWith,
  }

  [DataContract]
  public enum ScheduleCycle
  {
    [EnumMember(Value = "0")] Months,
    [EnumMember(Value = "1")] Weeks,
    [EnumMember(Value = "2")] Days,
    [EnumMember(Value = "3")] Hours,
    [EnumMember(Value = "4")] Minutes,
    [EnumMember(Value = "5")] Seconds,
    [EnumMember(Value = "6")] Continuous,
    [EnumMember(Value = "7")] Files,
    [EnumMember(Value = "8")] OnceOff,
    [EnumMember(Value = "9")] FilesInSeries,
  }

  [DataContract]
  public enum SendNotification
  {
    [EnumMember(Value = "0")] Error,
    [EnumMember(Value = "1")] WarningOrError,
    [EnumMember(Value = "2")] Always,
    [EnumMember(Value = "3")] Never,
  }

  [DataContract]
  public enum TimeZoneIndex
  {
    [EnumMember(Value = "0")] DatelineStandardTime = 0,
    [EnumMember(Value = "2")] HawaiianStandardTime = 2,
    [EnumMember(Value = "3")] AlaskanStandardTime = 3,
    [EnumMember(Value = "4")] PacificStandardTimeMexico = 4,
    [EnumMember(Value = "5")] PacificStandardTime = 5,
    [EnumMember(Value = "6")] UnitatedStatesMountainStandardTime = 6,
    [EnumMember(Value = "7")] MountainStandardTimeMexico = 7,
    [EnumMember(Value = "8")] MountainStandardTime = 8,
    [EnumMember(Value = "9")] CentralAmericaStandardTime = 9,
    [EnumMember(Value = "10")] CentralStandardTime = 10,
    [EnumMember(Value = "11")] CentralStandardTimeMexico = 11,
    [EnumMember(Value = "12")] CanadaCentralStandardTime = 12,
    [EnumMember(Value = "13")] SouthAmericaPacificStandardTime = 13,
    [EnumMember(Value = "14")] EasternStandardTime = 14,
    [EnumMember(Value = "15")] UnitedStatesEasternStandardTime = 15,
    [EnumMember(Value = "16")] VenezuelaStandardTime = 16,
    [EnumMember(Value = "17")] ParaguayStandardTime = 17,
    [EnumMember(Value = "18")] AtlanticStandardTime = 18,
    [EnumMember(Value = "19")] CentralBrazilianStandardTime = 19,
    [EnumMember(Value = "20")] SouthAmericaWesternStandardTime = 20,
    [EnumMember(Value = "21")] PacificSouthAmericaStandardTime = 21,
    [EnumMember(Value = "22")] NewfoundlandStandardTime = 22,
    [EnumMember(Value = "23")] EastSouthAmericaStandardTime = 23,
    [EnumMember(Value = "24")] ArgentinaStandardTime = 24,
    [EnumMember(Value = "25")] SouthAmericaEasternStandardTime = 25,
    [EnumMember(Value = "26")] GreenlandStandardTime = 26,
    [EnumMember(Value = "27")] MontevideoStandardTime = 27,
    [EnumMember(Value = "28")] BahiaStandardTime = 28,
    [EnumMember(Value = "30")] MidAtlanticStandardTime = 30,
    [EnumMember(Value = "31")] AzoresStandardTime = 31,
    [EnumMember(Value = "32")] CapeVerdeStandardTime = 32,
    [EnumMember(Value = "33")] MoroccoStandardTime = 33,
    [EnumMember(Value = "34")] UTC = 34,
    [EnumMember(Value = "35")] GMTStandardTime = 35,
    [EnumMember(Value = "36")] GreenwichStandardTime = 36,
    [EnumMember(Value = "37")] WestEuropeStandardTime = 37,
    [EnumMember(Value = "38")] CentralEuropeStandardTime = 38,
    [EnumMember(Value = "39")] RomanceStandardTime = 39,
    [EnumMember(Value = "40")] CentralEuropeanStandardTime = 40,
    [EnumMember(Value = "41")] WestCentralAfricaStandardTime = 41,
    [EnumMember(Value = "42")] NamibiaStandardTime = 42,
    [EnumMember(Value = "43")] JordanStandardTime = 43,
    [EnumMember(Value = "44")] GTBStandardTime = 44,
    [EnumMember(Value = "45")] MiddleEastStandardTime = 45,
    [EnumMember(Value = "46")] EgyptStandardTime = 46,
    [EnumMember(Value = "47")] SyriaStandardTime = 47,
    [EnumMember(Value = "48")] EastEuropeStandardTime = 48,
    [EnumMember(Value = "49")] SouthAfricaStandardTime = 49,
    [EnumMember(Value = "50")] FLEStandardTime = 50,
    [EnumMember(Value = "51")] TurkeyStandardTime = 51,
    [EnumMember(Value = "52")] IsraelStandardTime = 52,
    [EnumMember(Value = "53")] ArabicStandardTime = 53,
    [EnumMember(Value = "54")] KaliningradStandardTime = 54,
    [EnumMember(Value = "55")] ArabStandardTime = 55,
    [EnumMember(Value = "56")] EastAfricaStandardTime = 56,
    [EnumMember(Value = "57")] IranStandardTime = 57,
    [EnumMember(Value = "58")] ArabianStandardTime = 58,
    [EnumMember(Value = "59")] AzerbaijanStandardTime = 59,
    [EnumMember(Value = "60")] RussianStandardTime = 60,
    [EnumMember(Value = "61")] MauritiusStandardTime = 61,
    [EnumMember(Value = "62")] GeorgianStandardTime = 62,
    [EnumMember(Value = "63")] CaucasusStandardTime = 63,
    [EnumMember(Value = "64")] AfghanistanStandardTime = 64,
    [EnumMember(Value = "65")] PakistanStandardTime = 65,
    [EnumMember(Value = "66")] WestAsiaStandardTime = 66,
    [EnumMember(Value = "67")] IndiaStandardTime = 67,
    [EnumMember(Value = "68")] SriLankaStandardTime = 68,
    [EnumMember(Value = "69")] NepalStandardTime = 69,
    [EnumMember(Value = "70")] CentralAsiaStandardTime = 70,
    [EnumMember(Value = "71")] BangladeshStandardTime = 71,
    [EnumMember(Value = "72")] EkaterinburgStandardTime = 72,
    [EnumMember(Value = "73")] MyanmarStandardTime = 73,
    [EnumMember(Value = "74")] SouthEastAsiaStandardTime = 74,
    [EnumMember(Value = "75")] NorthCentralAsiaStandardTime = 75,
    [EnumMember(Value = "76")] ChinaStandardTime = 76,
    [EnumMember(Value = "77")] NorthAsiaStandardTime = 77,
    [EnumMember(Value = "78")] SingaporeStandardTime = 78,
    [EnumMember(Value = "79")] WesternAustraliaStandardTime = 79,
    [EnumMember(Value = "80")] TaipeiStandardTime = 80,
    [EnumMember(Value = "81")] UlaanbaatarStandardTime = 81,
    [EnumMember(Value = "82")] NorthAsiaEastStandardTime = 82,
    [EnumMember(Value = "83")] TokyoStandardTime = 83,
    [EnumMember(Value = "84")] KoreaStandardTime = 84,
    [EnumMember(Value = "85")] CentreAustraliaStandardTime = 85,
    [EnumMember(Value = "86")] AustraliaCentralStandardTime = 86,
    [EnumMember(Value = "87")] EastAustraliaStandardTime = 87,
    [EnumMember(Value = "88")] AustraliaEasternStandardTime = 88,
    [EnumMember(Value = "89")] WestPacificStandardTime = 89,
    [EnumMember(Value = "90")] TasmaniaStandardTime = 90,
    [EnumMember(Value = "91")] YakutskStandardTime = 91,
    [EnumMember(Value = "92")] CentralPacificStandardTime = 92,
    [EnumMember(Value = "93")] VladivostokStandardTime = 93,
    [EnumMember(Value = "94")] NewZealandStandardTime = 94,
    [EnumMember(Value = "96")] FijiStandardTime = 96,
    [EnumMember(Value = "97")] MagadanStandardTime = 97,
    [EnumMember(Value = "98")] KamchatkaStandardTime = 98,
    [EnumMember(Value = "99")] TongaStandardTime = 99,
    [EnumMember(Value = "100")] SamoaStandardTime = 100,
  }

  [DataContract]
  public enum Weekday
  {
    [EnumMember(Value = "0")] Sunday,
    [EnumMember(Value = "1")] Monday,
    [EnumMember(Value = "2")] Tuesday,
    [EnumMember(Value = "3")] Wednesday,
    [EnumMember(Value = "4")] Thursday,
    [EnumMember(Value = "5")] Friday,
    [EnumMember(Value = "6")] Saturday,
  }

  public enum BinaryFileType
  {
    jpg,
    gif,
    bmp,
    pdf,
    tiff,
    png,
  }

 public enum LogType
  {
    Log,
    Important,
    Warning,
    Error,
    Break,
    Debug,
    Caption,
  }
}
