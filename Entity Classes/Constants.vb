Public Class Constants

  Public Const SELECT_PLACEHOLDER = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
  Public Const NOTAPPLICABLE_PLACEHOLDER = "- NA -"
  Public Const MAX_ASP_REPORT_LIMIT = 2000

  Public Shared EXCEL2003CHAR As String = Chr(160)

  Public Const cDymDataSeperator = "##"
  Public Const cSvrDataSeperator = "|"
  Public Const cSvrRecordSeperator = "!~!"
  Public Const cSvrStringSeperator = "!$!"
  Public Const cSvrStringCRLF = "^^"
  Public Const cSvrSORTSeperator = "&&"
  Public Const cSvrNotesSeperator = "!@!"
  Public Const cSvrElpsis = " ..."

  Public Const cMultiDelim = ", "
  Public Const cCommaDelim = ","
  Public Const cColonDelim = ":"
  Public Const cSemiColonDelim = ";"
  Public Const cWildCard = "*"
  Public Const cImbedComa = "_"
  Public Const cHyphen = "-"
  Public Const cSpaceDelim = " "
  Public Const cDot = "."

  Public Const cDollarSymbol = "$"
  Public Const cEuroSymbol = "€"
  Public Const cPoundSymbol = "£"

  Public Const cEmptyString = ""
  Public Const cSingleSpace = " "
  Public Const cSingleQuote = "'"

  Public Const cDoubleSingleQuote = "''"

  Public Const cValueSeperator = "','"
  Public Const cIsNot = " IS NOT "
  Public Const cNot = " NOT "
  Public Const cNull = " NULL "
  Public Const cAndClause = " AND "
  Public Const cOrClause = " OR "
  Public Const cLikeClause = " LIKE "
  Public Const cInClause = " IN "
  Public Const cBetweenClause = " BETWEEN "
  Public Const cConvertClause = " CONVERT "
  Public Const cWhereClause = " WHERE "
  Public Const cSQLWildCard = "%"

  Public Const cSingleOpen = "("
  Public Const cDoubleOpen = "(("
  Public Const cSingleClose = ")"
  Public Const cDoubleClose = "))"

  Public Const cSingleSlash = "\"
  Public Const cDoubleSlash = "\\"
  Public Const cSingleForwardSlash = "/"

  Public Const cEq = " = "
  Public Const cGt = " > "
  Public Const cLt = " < "
  Public Const cLtEq = " <= "
  Public Const cGtEq = " >= "
  Public Const cNotEq = " <> "

  Public Const cHTMLEncodeAMP = "&amp;"
  Public Const cHTMLEncodeGT = "&gt;"
  Public Const cHTMLEncodeLT = "&lt;"
  Public Const cHTMLnbsp = "&nbsp;"
  Public Const QUOTE = Chr(34)

  ' use for finding the user id and password for user SQL connection
  Public Const APPUSER_EVOLIVE = "EVOLIVE"
  Public Const APPUSER_EVOBETA = "EVOBETA"
  Public Const APPUSER_EVOTEST = "EVOTEST"
  Public Const APPUSER_JETLIVE = "EVOLIVE"
  Public Const APPUSER_JETTEST = "EVOTEST"
  Public Const APPUSER_LOCAL = "EVOLIVE"

  Public Const PRODUCT_TYPE_B = "B"  ' Business     ** check tier level for make/model selection
  Public Const PRODUCT_TYPE_H = "H"  ' Helicopters  ** ignore tier level
  Public Const PRODUCT_TYPE_C = "C"  ' Commercial   ** check tier level for make/model selection
  Public Const PRODUCT_TYPE_R = "R"  ' Regional     ** ignore tier level

  Public Const PRODUCT_TYPE_F = "F"  ' Fortune 1000 ** not used

  Public Const PRODUCT_TYPE_A = "A"  ' ABI          ** ignore tier level ** not used
  Public Const PRODUCT_TYPE_P = "P"  ' AirBP        ** ignore tier level ** not used
  Public Const PRODUCT_TYPE_S = "S"  ' STAR Reports ** ignore tier level
  Public Const PRODUCT_TYPE_I = "I"  ' SPI View     ** ignore tier level

  Public Const PRODUCT_TYPE_Y = "Y"  ' Yacht        ** ignore tier level

  Public Const PRODUCT_DEFAULT_TABS_B = "S,A,H,C,O,P,E,M,W"  ' Business      
  Public Const PRODUCT_DEFAULT_TABS_C = "S,A,H,C,P,E"  ' Commercial    
  Public Const PRODUCT_DEFAULT_TABS_H = "S,A,H,C,P,E,M,W"  ' Helicopters   
  Public Const PRODUCT_DEFAULT_TABS_R = "S,A,H,C,E"  ' Regional      
  Public Const PRODUCT_DEFAULT_TABS_P = "S"  ' AirBP      
  Public Const PRODUCT_DEFAULT_TABS_A = "S"  ' Aviation Business Index      
  Public Const PRODUCT_DEFAULT_TABS_S = "S"  ' STAR Reports View     
  Public Const PRODUCT_DEFAULT_TABS_I = "S"  ' SPI View      
  Public Const PRODUCT_DEFAULT_TABS_Y = "S"  ' Yacht      

  Public Const EVOAERO_VIEW_0 = 0   ' no view
  Public Const EVOAERO_VIEW_1 = 1   ' aircraft model view
  Public Const EVOAERO_VIEW_2 = 2   ' model compare view
  Public Const EVOAERO_VIEW_3 = 3   ' operator view
  Public Const EVOAERO_VIEW_4 = 4   ' financial document view
  Public Const EVOAERO_VIEW_5 = 5   ' start with ac tab view
  Public Const EVOAERO_VIEW_6 = 6   ' star reports view
  Public Const EVOAERO_VIEW_7 = 7   ' fractional view
  Public Const EVOAERO_VIEW_8 = 8   ' aircraft location view
  Public Const EVOAERO_VIEW_9 = 9   ' financial market view
  Public Const EVOAERO_VIEW_10 = 10 ' aircraft contact type view(charter)
  Public Const EVOAERO_VIEW_11 = 11 ' model forsale view
  Public Const EVOAERO_VIEW_12 = 12 ' sales price view
  Public Const EVOAERO_VIEW_13 = 13 ' lease view
  Public Const EVOAERO_VIEW_14 = 14 ' manufacturer view
  Public Const EVOAERO_VIEW_15 = 15 ' reminder view
  Public Const EVOAERO_VIEW_16 = 16 ' 
  Public Const EVOAERO_VIEW_17 = 17 ' 
  Public Const EVOAERO_VIEW_18 = 18 ' 

  Public Const PRODUCT_CODE_NONE = -1          ' Product Code NONE

  Public Const PRODUCT_CODE_ALL = 0          ' Product Code ALL
  Public Const PRODUCT_CODE_BUSINESS = 1     ' Product Code Business
  Public Const PRODUCT_CODE_COMMERCIAL = 2   ' Product Code Commercial
  Public Const PRODUCT_CODE_HELICOPTERS = 3  ' Product Code Helicopters
  Public Const PRODUCT_CODE_REGIONAL = 4     ' Product Code Regional
  Public Const PRODUCT_CODE_ABI = 5          ' Product Code ABI
  Public Const PRODUCT_CODE_AIRBP = 6        ' Product Code AirBP
  Public Const PRODUCT_CODE_STAR = 7         ' Product Code Star
  Public Const PRODUCT_CODE_SPI = 8          ' Product Code SPI
  Public Const PRODUCT_CODE_YACHT = 9        ' Product Code Yacht

  Public Const PRODUCT_CODE_COMBO = 99          ' Product Code COMBO

  Public Const COMMERCIAL_VIEW_MKMODEL = 0   ' Commercial View By make Model
  Public Const COMMERCIAL_VIEW_COMPANY = 1   ' Commercial View By Company
  Public Const COMMERCIAL_VIEW_OPERATOR = 2  ' Commercial View By Operator

  Public Const COMMERCIAL_SUMMARY_ALL = 0       ' Commercial Summary All
  Public Const COMMERCIAL_SUMMARY_INSERVICE = 1 ' Commercial Summary In Service
  Public Const COMMERCIAL_SUMMARY_ONORDER = 2   ' Commercial Summary On Order
  Public Const COMMERCIAL_SUMMARY_RETIRED = 3   ' Commercial Summary Retired

  Public Const LOCATION_VIEW_BASE = 0             ' Location View By Base
  Public Const LOCATION_VIEW_OWNER = 1            ' Location View By Owner
  Public Const LOCATION_VIEW_OPERATOR = 2         ' Location View By Operator
  Public Const LOCATION_VIEW_CHARTER_BASE = 3     ' Location View By Charter Aircraft Location
  Public Const LOCATION_VIEW_CHARTER_OPERATOR = 4 ' Location View By Charter Operator

  Public Const LOCATION_SUMMARY_STATE = 0    ' Location Summary By State
  Public Const LOCATION_SUMMARY_CITY = 1     ' Location Summary By US City
  Public Const LOCATION_SUMMARY_AIRFRAME = 2 ' Location Summary By Airframe
  Public Const LOCATION_SUMMARY_BASEIATA = 3 ' Location Summary By Base IATA
  Public Const LOCATION_SUMMARY_BASEICAO = 4 ' Location Summary By Base ICAO
  Public Const LOCATION_SUMMARY_AIRCRAFT = 5 ' Location Summary By Aircraft
  Public Const LOCATION_SUMMARY_MODEL = 6    ' Location Summary By Models
  Public Const LOCATION_SUMMARY_STAR = 7     ' Location Summary By Star Reports
  Public Const LOCATION_SUMMARY_SALES = 8    ' Location Summary By Sales

  Public Const LOCATION_SORT_STATE = 0     ' Location Sort By State
  Public Const LOCATION_SORT_CITY = 1    ' Location Sort By US City
  Public Const LOCATION_SORT_AIRFRAME = 2  ' Location Sort By Airframe
  Public Const LOCATION_SORT_BASEIATA = 3  ' Location Sort By Base IATA
  Public Const LOCATION_SORT_BASEICAO = 4  ' Location Sort By Base ICAO
  Public Const LOCATION_SORT_COUNTRY = 5   ' Location Sort By Country
  Public Const LOCATION_SORT_REGION = 6    ' Location Sort By Region
  Public Const LOCATION_SORT_CONTINENT = 7 ' Location Sort By Continent
  Public Const LOCATION_SORT_AIRCRAFT = 8 ' Location Sort By Aircraft

  Public Const VIEW_ALLAIRFRAME = 0  ' Market View All
  Public Const VIEW_EXECUTIVE = 1    ' Market View By Executive
  Public Const VIEW_JETS = 2         ' Market View By Jets
  Public Const VIEW_TURBOPROPS = 3   ' Market View By Turbo Props
  Public Const VIEW_PISTONS = 4      ' Market View By Pistons
  Public Const VIEW_HELICOPTERS = 5  ' Market View By Helicopters

  Public Const VIEW_ALLHULLTYPES = 0  ' Market View All
  Public Const VIEW_MOTORHULL = 1    ' Market View By Executive
  Public Const VIEW_SAILHULL = 2         ' Market View By Jets

  Public Const VIEW_6MONTHS = 6      ' Market View 6 month time span
  Public Const VIEW_12MONTHS = 12    ' Market View 12 month time span

  Public Const ACCONTACTTYPE_VIEW_6MONTHS = 6   ' Fractional View 6 month time span

  ' Not Used yet ...
  Public Const ACCONTACTTYPE_VIEW_CHARTER = 0   ' AC Contact Type View By CHARTER
  Public Const ACCONTACTTYPE_VIEW_EXCLUSIVE = 1 ' AC Contact Type View By EXCLUSIVE BROKER/REP
  Public Const ACCONTACTTYPE_VIEW_PILOT = 2     ' AC Contact Type View By PILOT

  Public Const ACCONTACTTYPE_SUMMARY_AIRFRAME = 0 ' AC Contact Type View By Airframe
  Public Const ACCONTACTTYPE_SUMMARY_MODEL = 1 ' AC Contact Type View By Model
  Public Const ACCONTACTTYPE_SUMMARY_COMPANY = 2 ' AC Contact Type View By Company

  Public Const EVOAERO_TAB_0 = "A"   ' Aircraft Tab
  Public Const EVOAERO_TAB_1 = "H"   ' History/Transaction Tab
  Public Const EVOAERO_TAB_2 = "C"   ' Company Tab
  Public Const EVOAERO_TAB_3 = "O"   ' Operating Costs Tab
  Public Const EVOAERO_TAB_4 = "P"   ' Performance Tab
  Public Const EVOAERO_TAB_5 = "E"   ' Events Tab
  Public Const EVOAERO_TAB_6 = "M"   ' Market Summary Tab
  Public Const EVOAERO_TAB_7 = "W"   ' Wanted Tab
  Public Const EVOAERO_TAB_8 = "S"   ' Home Tab
  Public Const EVOAERO_TAB_9 = "R"   ' Preferences Tab

  Public Const TAB_RESULTS_Y = "Y"   ' Return value to
  Public Const TAB_RESULTS_N = "N"   ' for display of tab
  Public Const TAB_RESULTS_F = "F"

  Public Const SMS_ACTIVATE_YES = "Y"
  Public Const SMS_ACTIVATE_NO = "N"
  Public Const SMS_ACTIVATE_PENDING = "A"
  Public Const SMS_ACTIVATE_WAIT = "W"
  Public Const SMS_ACTIVATE_TEST = "T"

  Public Const csALL_COMPANYTYPE = "All"
  Public Const csALL_OWNERS = "00, 97, 17, 08, 16"
  Public Const csALL_OPERATECOMP = "36, 94, 11, 35, 12, 89, 18, 39, 31"
  Public Const csALL_EXCLUSIVE = "93, 98, 99"
  Public Const csALL_OWNERSMINIMARKET = "00, 97, 17, 08, 56"

  Public Const MAX_USAGE_SELECTIONS = 6
  Public Const MAX_AVIONICS_SELECTIONS = 5
  Public Const MAX_FEATURE_SELECTIONS = 6
  Public Const MAX_DOCS_SELECTIONS = 1

  Public Const AIRCRAFT_BUSINESSPRODUCT = "ac_product_business_flag"
  Public Const AIRCRAFT_HELICOPTERPRODUCT = "ac_product_helicopter_flag"
  Public Const AIRCRAFT_COMMERCIALPRODUCT = "ac_product_commercial_flag"
  Public Const AIRCRAFT_REGIONALPRODUCT = "ac_product_regional_flag"     ' * not in use
  Public Const AIRCRAFT_AIRBPPRODUCT = "ac_product_airbp_flag"
  Public Const AIRCRAFT_ABIPRODUCT = "ac_product_abi_flag"               ' * not in use

  Public Const MODEL_BUSINESSPRODUCT = "amod_product_business_flag"
  Public Const MODEL_HELICOPTERPRODUCT = "amod_product_helicopter_flag"
  Public Const MODEL_COMMERCIALPRODUCT = "amod_product_commercial_flag"
  Public Const MODEL_REGIONALPRODUCT = "amod_product_regional_flag"     ' * not in use
  Public Const MODEL_AIRBPPRODUCT = "amod_product_airbp_flag"
  Public Const MODEL_ABIPRODUCT = "amod_product_abi_flag"               ' * not in use

  Public Const COMPANY_BUSINESSPRODUCT = "comp_product_business_flag"
  Public Const COMPANY_HELICOPTERPRODUCT = "comp_product_helicopter_flag"
  Public Const COMPANY_COMMERCIALPRODUCT = "comp_product_commercial_flag"
  Public Const COMPANY_REGIONALPRODUCT = "comp_product_regional_flag"     ' * not in use
  Public Const COMPANY_AIRBPPRODUCT = "comp_product_airbp_flag"
  Public Const COMPANY_ABIPRODUCT = "comp_product_abi_flag"               ' * not in use
  Public Const COMPANY_YACHTPRODUCT = "comp_product_yacht_flag"               ' * not in use

  Public Const FROMVIEW_A = "A"
  Public Const FROMVIEW_H = "H"
  Public Const FROMVIEW_HC = "HC"
  Public Const FROMVIEW_EC = "EC"
  Public Const FROMVIEW_DF = "DF"
  Public Const FROMVIEW_DE = "DE"
  Public Const FROMVIEW_DR = "DR"
  Public Const FROMVIEW_FP = "FP"
  Public Const FROMVIEW_FB = "FB"
  Public Const FROMVIEW_FF = "FF"
  Public Const FROMVIEW_AL = "AL"
  Public Const FROMVIEW_LD = "LD"
  Public Const FROMVIEW_LX = "LX"
  Public Const FROMVIEW_TD = "TD"
  Public Const FROMVIEW_FV = "FV"
  Public Const FROMVIEW_OP = "OP"

  Public Const _STARTCHARWIDTH As Double = 8.5
  Public Const _STARTMAXWIDTH As Double = (20 * _STARTCHARWIDTH)

  Public Const AIRFRAME_INDEX = 0
  Public Const AIRFRAME_TYPE = 1
  Public Const AIRFRAME_MAKE = 2
  Public Const AIRFRAME_MAKE_ABR = 3
  Public Const AIRFRAME_MODEL = 4
  Public Const AIRFRAME_MODEL_ID = 5
  Public Const AIRFRAME_USAGE = 6
  Public Const AIRFRAME_FRAME = 7
  Public Const AIRFRAME_MFRNAME = 8
  Public Const AIRFRAME_SIZE = 9

  Public Const AIRMAKETYPE_INDEX = 0
  Public Const AIRMAKETYPE_AFT = 1
  Public Const AIRMAKETYPE_AFMT = 2
  Public Const AIRMAKETYPE_CODE = 3
  Public Const AIRMAKETYPE_NAME = 4

  Public Const serverAIRFRAMEARRAY_DIM = 9
  Public Const serverAIRMAKELABLEARRAY_DIM = 4

  Public Const YACHT_LABEL_INDEX = 0
  Public Const YACHT_LABEL_MOTOR = 1
  Public Const YACHT_LABEL_CATEGORY = 2
  Public Const YACHT_LABEL_CODE = 3
  Public Const YACHT_LABEL_NAME = 4

  Public Const LOCYACHT_INDEX = 0
  Public Const LOCYACHT_CATEGORY = 1
  Public Const LOCYACHT_BRAND = 2
  Public Const LOCYACHT_BRAND_ABR = 3
  Public Const LOCYACHT_MODEL = 4
  Public Const LOCYACHT_MODEL_ID = 5
  Public Const LOCYACHT_MOTOR = 6

  Public Const serverYACHTARRAY_DIM = 6
  Public Const serverYACHTLABLEARRAY_DIM = 4

  Public Const serverRGNARRAY_DIM = 5
  Public Const serverTZARRAY_DIM = 2
  Public Const serverEVENTCATARRAY_DIM = 2

  Public Const serverMFRNAMESARRAY_DIM = 4
  Public Const serverSIZECATARRAY_DIM = 5

  Public Const AMOD_TYPE_AIRLINER = "E"
  Public Const AMOD_TYPE_JET = "J"
  Public Const AMOD_TYPE_TURBO = "T"
  Public Const AMOD_TYPE_PISTON = "P"

  Public Const AMOD_FIXED_AIRFRAME = "F"
  Public Const AMOD_ROTARY_AIRFRAME = "R"

  Public Const YMOD_TYPE_GIGA = "G"
  Public Const YMOD_TYPE_MEGA = "M"
  Public Const YMOD_TYPE_SUPER = "S"
  Public Const YMOD_TYPE_LUXURY = "L"

  Public Const YMOD_MOTOR_HULL = "M"
  Public Const YMOD_SAIL_HULL = "S"
  Public Const eValues_Refer_Name = "eValues"
  Public Const eValues_Descriptive_Text = "<p class=""evalue_blue"">The " & eValues_Refer_Name & " presented on this tab are estimates only based on services integrated thru Asset Insight's eValues platform. <a href=""javascript:void(0);"" onclick='javascript:openSmallWindowJS(""/help/documents/809.pdf"",""HelpWindow"");' class=""evalue_blue label"">Click to Learn More</a></p>"

  ' user input/output formatting types
  Public Const gtUSRNONE = 0
  Public Const gtUSRWILDCARD = 2
  Public Const gtUSRHTMLSELECT = 4
  Public Const gtUSRMULTISELECT = 8
  Public Const gtUSRRANGE = 16
  Public Const gtUSRPHONENUM = 32
  Public Const gtUSRCOMPNAME = 64
  Public Const gtUSRWILDSQL = 128
  Public Const gtUSRSAVEPROJECT = 256
  Public Const gtUSRHTMLSELECTNUM = 512
  Public Const gtUSRCOMPRANGE = 1024
  Public Const gtUSRNUMERICSTR = 2048
  Public Const gtUSRNUMRANGE = 4096

  ' sql query generation types
  Public Const gtSQLNONE = 0
  Public Const gtSQLAND = 100
  Public Const gtSQLOR = 200
  Public Const gtSQLIKE = 300
  Public Const gtSQLIN = 400
  Public Const gtSQLBET = 500
  Public Const gtSQLEQL = 600
  Public Const gtSQLLT = 610
  Public Const gtSQLGT = 620
  Public Const gtSQLGTEQL = 630
  Public Const gtSQLLTEQL = 640
  Public Const gtSQLGROUP = 700
  Public Const gtSQLORDER = 800
  Public Const gtSQLDATE = 900

  <System.Serializable(), FlagsAttribute()>
  Public Enum ApplicationVariable As Integer
    EVO = 0
    YACHT = 1
    CRM = 2
    CUSTOMER_CENTER = 3
    ABI = 4
    HOMEBASE = 5
  End Enum

End Class
