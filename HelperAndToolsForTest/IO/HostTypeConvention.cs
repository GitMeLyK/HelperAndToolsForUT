using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace HelperAndToolsForTest.IO
{

    /// <summary>
    ///     Type conventions validation in use from this HostTypeConvention 
    ///     at the moment of the instance or Picked from list defaults or custom.
    /// </summary>
    public enum TypeConvention
    {
        /// <summary>On constructor of HostConvention define to use HostConvention of Host in execution a runtime, if Runtime not resolved Host in execution well be setted OTHER</summary>
        RUNTIME,

        /// <summary>On constructor of HostConvention define to use HostConvention of Host choice from list of Host managed internally <see cref="HostType"/> a runtime</summary>
        PICKED,

        /// <summary>On construnctor of HostConvention as defined from user HostConvention of Host only from parameters custom of user</summary>
        CUSTOM
    }

    /// <summary>
    ///     For Conventions in use to Utils
    ///     set or found HOST with ricepts of
    ///     convention standard in use in host type.
    /// </summary>
    public enum HostType
    {
        /// <summary>Windows (All versions system)</summary>
        WINDOWS,
        /// <summary>MAC (All versions system)</summary>
        IOS,
        /// <summary>Linux (All of base system)</summary>
        LINUX,
        /// <summary>Unix Like (All of base to Free)</summary>
        FREEBSD,
        /// <summary>General OS base</summary>
        OTHER,
        /// <summary>In mode RUNTIME if system return string host not parsed from this procedure.</summary>
        UKNOWED
    }

    /// <summary>
    ///     Support to analyze path for file system on OS destination,
    /// </summary>
    public class HostTypeConvention
    {
        //public static Regex regexPathInvisibleUnix = new Regex(@"([\\\/]\.[^\.\\\/]|^\.)", RegexOptions.Compiled);

        OptionsConvention _customOptionsConvention = null;   // Optional to invalidate for default in win system if options for behavior for path and file is valid
        char[] _customExtraInvalidCharsForPath = null;           // Optional to considere a Negate chars for this Analyzer default : extraCharsNotValidPathWinSystem
        string[] _customExtraNamesDoNotUseInRootSystem = null;   // Optional to invalidate Path if a path in Root contains one of this name list used from system for convention internal, default is named of win system ntfs
        string[] _customExtraNamesDoNotUseForFileName = null;    // Optional to invalidate Path if a path in cas have filename final contains one of this name list used from system for convention internal, default is named internal of win system ntfs


        #region ###      TypePath and Convention Names CUSTOM extraChars we to don't Valid for HOSTS    ###

        /// <summary>Chars default extra not Valid for Other General System</summary>
        public static char[] defaultsExtraCharsNotValidPathGeneralSystem = new[] { Char.MinValue }; // NULL CHAR
        /// <summary>Chars default extra not Valid for Windows System</summary>
        public static char[] defaultsExtraCharsNotValidPathWinSystem = new[] { ':', ';', '*', '/', '?', ',', '^', '&' };
        /// <summary>Chars default extra not Valid for MAC System</summary>
        public static char[] defaultsExtraCharsNotValidPathMacSystem = new[] { ';', '*', '\\', '?', ',', '^', '&' };
        /// <summary>Chars default extra not Valid for Linux System</summary>
        public static char[] defaultsExtraCharsNotValidPathLinuxSystem = new[] { ';', '*', '\\', '?', ',', '^' };
        /// <summary>Chars default extra not Valid for Linux System</summary>
        public static char[] defaultsExtraCharsNotValidPathFreebsdSystem = new[] { '\\' };

        #endregion

        #region ###      TypePath and Convention Names RESEVED names for Files do not use for HOSTS     ###

        /// <summary>Reserved Name convention do not use in general OS (default check)</summary>
        public static string[] reservedNamesForFileGeneralSystem = new String[] { };

        /// <summary>Reserved Name convention do not use in Windows OS NTFS (default check)</summary>
        public static string[] reservedNamesForFileWinSystem = new String[] {
                "CON","PRN","AUX","NUL","COM1","COM2","COM3","COM4",
                "COM5", "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2", "LPT3",
                "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
        };

        /// <summary>Reserved Name convention do not use in Mac OS (default check)</summary>
        public static string[] reservedNamesForFileMacSystem = new String[] { };

        /// <summary>Reserved Name convention do not use in Linux OS (default check)</summary>
        public static string[] reservedNamesForFileLinuxSystem = new String[] { };

        /// <summary>Reserved Name convention do not use in FreeBsd OS (default check)</summary>
        public static string[] reservedNamesForFileFreebsdSystem = new String[] { };

        #endregion

        #region ###      TypePath and Convention Names RESEVED names for ROOT do not use for HOSTS      ###

        /// <summary>Reserved Name convention do not use in other generic for Root(default check)</summary>
        public static string[] reservedNamesPathRootGeneralSystem = new string[] { };

        /// <summary>Reserved Name convention do not use in Windows OS NTFS for Root(default check)</summary>
        public static string[] reservedNamesPathRootWinSystem = new string[]{
                "$AttrDef", "$BadClus", "$Bitmap", "$Boot", "$LogFile", "$MFT", "$MFTMirr",
                "pagefile.sys", "$Secure", "$UpCase", "$Volume", "$Extend", "$Extend\\$ObjId",
                "$Extend\\$Quota", "$Extend\\$Reparse"
        };

        /// <summary>Reserved Name convention do not use in MAC for Root(default check)</summary>
        public static string[] reservedNamesPathRootMacSystem = new string[] { };

        /// <summary>Reserved Name convention do not use in Linux for Root(default check)</summary>
        public static string[] reservedNamesPathRootLinuxSystem = new string[] { };

        /// <summary>Reserved Name convention do not use in Freebsd for Root(default check)</summary>
        public static string[] reservedNamesPathRootFreebsdSystem = new string[] { };

        #endregion

        #region ###             PUBLIC PROPERTIES           ###

        /// <summary>
        ///     Name and description of HOST and type conventions is in use of 
        ///     OS filesytem if resolved convention from host in execution, or Name of 
        ///     Host Picked on choice from use, or if custom name attribuite for ipotetic host.
        /// </summary>
        public string HostValidation { get; private set; }

        /// <summary>
        ///     Return Host Convention in use.
        /// </summary>
        public HostType HostFound { get; private set; }

        /// <summary>
        ///     Return type of validation setted for this instance of host convention.
        /// </summary>
        public TypeConvention ConventionType{ get; private set; }

        /// <summary>
        ///     Return List of Names reserved only in Root with extra names if user use a custom conventions extra.
        /// </summary>
        public List<string> ReservedNamesForRoot { get; private set; }

        /// <summary>
        ///     Return List of Names reserved only in FileName with extra names if user use a custom conventions extra.
        /// </summary>
        public List<string> ReservedNamesForFileName { get; private set; }

        /// <summary>
        ///     Return List of Chars with extra if user use a custom conventions extra fo not valid assignment in sequence path.
        /// </summary>
        public List<char> ListOfInvalidCharsForPath { get; private set; }

        /// <summary>
        ///     Return type of validation setted for this instance of host convention.
        /// </summary>
        public OptionsConvention Options { get; private set; }

        #endregion

        #region ###     .ctors RUNTIME PICKED OR CUSTOM     ###

        /// <summary>
        ///     RUNTIME - Return a default conventions in use of current FileSystem at Runtime
        /// </summary>
        /// <param name="extraInvalidCharsForPath">Extra vars chars to check if valid in this path. (For default use list of chars prohibited on system win NTFS) also use a array empty {} to not check validatation</param>
        /// <param name="extraNamesDoNotUseInRootSystem">Optional to invalidate Path if a path in Root contains one of this name list used from system for convention internal, default is named of win system ntfs</param>
        /// <param name="extraNamesDoNotUseForFileName">Optional to invalidate Path if a path in cas have filename final contains one of this name list used from system for convention internal, default is named internal of win system ntfs</param>
        /// <param name="alterOptionsConvention">Optional to invalidate for default in win system if options for behavior for path and file is valid.</param>
        public HostTypeConvention(
                    char[] extraInvalidCharsForPath = null,             // Optional to add other Negate chars for this Analyzer default : extraCharsNotValidPathWinSystem
                    string[] extraNamesDoNotUseInRootSystem = null,     // Optional to add other invalidate Path if a path in Root contains one of this name list used from system for convention internal, default is named of win system ntfs
                    string[] extraNamesDoNotUseForFileName = null,      // Optional to add other invalidate Path if a path in cas have filename final contains one of this name list used from system for convention internal, default is named internal of win system ntfs
                    OptionsConvention alterOptionsConvention = null // Optional to invalidate for default in win system if options for behavior for path and file is valid
            ) : this(true,null,
                    alterOptionsConvention,
                    extraInvalidCharsForPath,
                    extraNamesDoNotUseInRootSystem,
                    extraNamesDoNotUseForFileName
            ) {}

        /// <summary>
        ///     PICKED - Return a default conventions  from user to use in path and file system target.
        /// </summary>
        /// <param name="convention">Set convention predefineted from choice HOST managed from this class.</param>
        /// <param name="extraInvalidCharsForPath">Extra vars chars to check if valid in this path. (For default use list of chars prohibited on system win NTFS) also use a array empty {} to not check validatation</param>
        /// <param name="extraNamesDoNotUseInRootSystem">Optional to invalidate Path if a path in Root contains one of this name list used from system for convention internal, default is named of win system ntfs</param>
        /// <param name="extraNamesDoNotUseForFileName">Optional to invalidate Path if a path in cas have filename final contains one of this name list used from system for convention internal, default is named internal of win system ntfs</param>
        /// <param name="alterOptionsConvention">Optional to invalidate for default in win system if options for behavior for path and file is valid.</param>
        public HostTypeConvention(
                    HostType convention,                  // Picked from user type host
                    char[] extraInvalidCharsForPath = null,             // Optional to add other Negate chars for this Analyzer default : extraCharsNotValidPathWinSystem
                    string[] extraNamesDoNotUseInRootSystem = null,     // Optional to add other invalidate Path if a path in Root contains one of this name list used from system for convention internal, default is named of win system ntfs
                    string[] extraNamesDoNotUseForFileName = null,      // Optional to add other invalidate Path if a path in cas have filename final contains one of this name list used from system for convention internal, default is named internal of win system ntfs
                    OptionsConvention alterOptionsConvention = null // Optional to invalidate for default in win system if options for behavior for path and file is valid
            ) : this(false,convention,
                    alterOptionsConvention,
                    extraInvalidCharsForPath,
                    extraNamesDoNotUseInRootSystem,
                    extraNamesDoNotUseForFileName
                )
            { }

        /// <summary>
        ///     CUSTOM -Return a custom conventions definited from user to use in path and file system target.
        /// </summary>
        /// <param name="NameofCustomHost">Name strbuite to this Custom Host convention</param>
        /// <param name="customExtraInvalidCharsForPath">Custom vars chars to check if valid in this path. (For default use list of chars prohibited on system win NTFS) also use a array empty {} to not check validatation</param>
        /// <param name="customExtraNamesDoNotUseInRootSystem">Custom to invalidate Path if a path in Root contains one of this name list used from system for convention internal, default is named of win system ntfs</param>
        /// <param name="customExtraNamesDoNotUseForFileName">Custom to invalidate Path if a path in cas have filename final contains one of this name list used from system for convention internal, default is named internal of win system ntfs</param>
        /// <param name="customOptionsConvention">Optional to invalidate for default in win system if options for behavior for path and file is valid.</param>
        public HostTypeConvention(
                    string NameofCustomHost,                                // Name of Custom HOST for definition.
                    char[] customExtraInvalidCharsForPath = null,           // Optional to considere a Negate chars for this Analyzer default : extraCharsNotValidPathWinSystem
                    string[] customExtraNamesDoNotUseInRootSystem = null,   // Optional to invalidate Path if a path in Root contains one of this name list used from system for convention internal, default is named of win system ntfs
                    string[] customExtraNamesDoNotUseForFileName = null,    // Optional to invalidate Path if a path in cas have filename final contains one of this name list used from system for convention internal, default is named internal of win system ntfs
                    OptionsConvention customOptionsConvention = null    // Optional to invalidate for default in win system if options for behavior for path and file is valid
            ) : this(false, null,
                    customOptionsConvention,
                    customExtraInvalidCharsForPath,
                    customExtraNamesDoNotUseInRootSystem,
                    customExtraNamesDoNotUseForFileName
            ) { }

        /// <summary>
        ///     Return a default conventions in use of current FileSystem at Runtime if argument <paramref name="validateForCurrentHostSystem"/> is true. 
        ///     The Behavior is in case of true for Runtime Host in execution, is if Runtime in not resolved for 
        ///     cause of system os with filesystem custom, this object use other params to apply custom conventions in case of OTHER system.
        ///     Also in case of <paramref name="validateForCurrentHostSystem"/> is false the custom conventions in other arguments is 
        ///     applyed in mode esclusive without add internal clausole of system in execution.
        /// </summary>
        /// <param name="validateForCurrentHostSystem"> if True = check if chars based on current Host Type file system also not check and use only extraInvalidCharsForPath if filled or null, and if windows system check FullQualifiedPath for root</param>
        /// <param name="convention">Valid only if validateForCurrentHostSystem = false, set convention predefineted from choide.</param>
        /// <param name="customOptionsConvention">Optional to invalidate for default in win system if options for behavior for path and file is valid.</param>
        /// <param name="customExtraInvalidCharsForPath">Extra vars chars to check if valid in this path. (For default use list of chars prohibited on system win NTFS) also use a array empty {} to not check validatation</param>
        /// <param name="customExtraNamesDoNotUseInRootSystem">Optional to invalidate Path if a path in Root contains one of this name list used from system for convention internal, default is named of win system ntfs</param>
        /// <param name="customExtraNamesDoNotUseForFileName">Optional to invalidate Path if a path in cas have filename final contains one of this name list used from system for convention internal, default is named internal of win system ntfs</param>
        /// <param name="NameHostForCustomConvention">Optional for name attribuite to host with custom convention to apply.</param>
        private HostTypeConvention(
                    bool validateForCurrentHostSystem,                      // True = check if chars based on current Host Type file system also not check and use only extraInvalidCharsForPath if filled or null
                    HostType? convention = null,              // Valid only if validateForCurrentHostSystem = false, set convention predefineted from choide.
                    OptionsConvention customOptionsConvention = null,   // Optional to invalidate for default in win system if options for behavior for path and file is valid
                    char[] customExtraInvalidCharsForPath = null,           // Optional to considere a Negate chars for this Analyzer default : extraCharsNotValidPathWinSystem
                    string[] customExtraNamesDoNotUseInRootSystem = null,   // Optional to invalidate Path if a path in Root contains one of this name list used from system for convention internal, default is named of win system ntfs
                    string[] customExtraNamesDoNotUseForFileName = null,    // Optional to invalidate Path if a path in cas have filename final contains one of this name list used from system for convention internal, default is named internal of win system ntfs
                    string NameHostForCustomConvention = null               // Optional for name attribuite to host with custom convention to apply.
                ) {

            string variant = null;
            if (customExtraInvalidCharsForPath == null && customExtraNamesDoNotUseInRootSystem == null &&
                customExtraNamesDoNotUseForFileName == null) {
                if(customOptionsConvention == null)
                    variant = "WITH NO VARIANTS";
                else
                    variant = "WITH VARIANT CUSTOM OPTIONS";
                }
            else { 
                if(customOptionsConvention == null )
                    variant = "WITH VARIANTS AND OPTIONS DEFAULT";
                else
                    variant = "WITH VARIANTS AND CUSTOM OPTIONS";
            }

            // :: :: Set Type Current Convention for Validations :: ::
            ConventionType = GetTypeHostValidation(validateForCurrentHostSystem,variant, NameHostForCustomConvention, ref convention, out string hostValidation);
            HostValidation = hostValidation;

            // :: :: Set Options for this Convention applied from use for Variant or Default :: ::
            if (customOptionsConvention == null) {
                if (ConventionType == TypeConvention.RUNTIME) Options = new OptionsConvention();
                else if (ConventionType == TypeConvention.PICKED) Options = new OptionsConvention(convention.Value);
                else Options = new OptionsConvention(NameHostForCustomConvention, new OptionsConvention(HostType.OTHER )); // In this case use a general options
            } else {
                if (customOptionsConvention.ConventionType == TypeConvention.RUNTIME && ConventionType == TypeConvention.RUNTIME)
                    Options = customOptionsConvention;
                else if (customOptionsConvention.ConventionType == TypeConvention.PICKED && ConventionType == TypeConvention.PICKED) {
                    if (customOptionsConvention.HostConvention == HostFound)
                        // For some scope apply variant
                        Options = customOptionsConvention;
                    else
                        throw new ArgumentException("Options Convention must be of the same type " + HostFound.ToString());
                } else /* customOptionsConvention.TypeCurrentOptionsConvention == TypeConvention.CUSTOM*/ {
                    Options = customOptionsConvention;
                }
            }

            // Set temporary to apply polycy with custom variants
            _customOptionsConvention = customOptionsConvention;                             // Optional to invalidate for default in win system if options for behavior for path and file is valid
            _customExtraInvalidCharsForPath = customExtraInvalidCharsForPath;               // Optional to considere a Negate chars for this Analyzer default : extraCharsNotValidPathWinSystem
            _customExtraNamesDoNotUseInRootSystem = customExtraNamesDoNotUseInRootSystem;   // Optional to invalidate Path if a path in Root contains one of this name list used from system for convention internal, default is named of win system ntfs
            _customExtraNamesDoNotUseForFileName = customExtraNamesDoNotUseForFileName;     // Optional to invalidate Path if a path in cas have filename final contains one of this name list used from system for convention internal, default is named internal of win system ntfs

            // :: :: Apply Policy Validations with this variants    ::  ::
            ApplyPolicyConventions();

            // Null for invalidate applypolicy after
            _customOptionsConvention = null;                             
            _customExtraInvalidCharsForPath = null;               
            _customExtraNamesDoNotUseInRootSystem = null;   
            _customExtraNamesDoNotUseForFileName = null;     

            // :: :: Now have convention :: ::
            HostFound = convention.Value;
        }

        #endregion

        #region ###     Main to Apply Polycy Convention     ###

        /// <summary>
        ///     On course of alter conventions this method
        ///     reapply changes before use to check validity
        ///     of path or filename.
        /// </summary>
        private void ApplyPolicyConventions()
        {
            // :: :: CONVENTION CHARS NOT VALID :: ::
            //
            // Fill Defaults Chars Not Valid which by convention are classified as not usable as a sequence in the
            // path of this host system, this from Path return from evneiroment OS list of default.
            if (ConventionType == TypeConvention.RUNTIME)
                // ( WIN | MAC | LINUX | FREEBSD | OTHER ) + Defaults extra of this class
                setRuntimePlusExtraDefaultsListOfInvalidCharsForPath();
            else if (ConventionType == TypeConvention.PICKED)
                // ( WIN | MAC | LINUX | FREEBSD | OTHER ) + Defaults extra of this class
                setPickedPlusExtraDefaultsListOfInvalidCharsForPath(HostFound);
            else  // Have TypeCurrentConvention == CUSTOM
                // Start from Null Char + CUSTOM defined from parameteres user after.
                ListOfInvalidCharsForPath = new List<char>(); // [] { char.MinValue };
            //
            // :: Variants Always ADDed from parameter of this Convention Type. :: 
            if (_customExtraInvalidCharsForPath != null)
                ListOfInvalidCharsForPath = ListOfInvalidCharsForPath.Concat(_customExtraInvalidCharsForPath).ToList();                   // CUSTOM VARIANTS
            //

            // :: :: CONVENTION NAME RESERVED ON ROOT PATH :: ::

            // Fill List of names we For Convention on Root Winsystem or other not accept a name Spcial reserved
            // path of this host system, this from Path return from evneiroment OS list of default.
            if (ConventionType == TypeConvention.RUNTIME)
                // ( WIN | MAC | LINUX | FREEBSD | OTHER ) + Defaults extra of this class
                setRuntimePlusExtraDefaultsRestrictedNameOnRootForPath();
            else if (ConventionType == TypeConvention.PICKED)
                // ( WIN | MAC | LINUX | FREEBSD | OTHER ) + Defaults extra of this class
                setPickedPlusExtraDefaultsRestrictedNameOnRootForPath(HostFound);
            else // TypeCurrentConvention == CUSTOM
                // Start from List 0 element (all valid) + CUSTOM defined from parameteres user after.
                ReservedNamesForRoot = new List<string>(); // new[] { "" };
            //
            // :: Variants Always ADDed from parameter of this Convention Type. :: 
            if (_customExtraNamesDoNotUseInRootSystem != null)
                ReservedNamesForRoot = ReservedNamesForRoot.Concat(_customExtraNamesDoNotUseInRootSystem).ToList();     // CUSTOM VARIANTS
            //

            // :: :: CONVENTION NAME RESERVED FOR NAMED A FILE ON PATH :: ::

            // Fill List of names we For Convention on FileName in Winsystem or other not accept a name Special reserved
            // path of this host system, this from Path return from evneiroment OS list of filename reserved default.
            if (ConventionType == TypeConvention.RUNTIME)
                // ( WIN | MAC | LINUX | FREEBSD | OTHER ) + Defaults extra of this class
                setRuntimePlusExtraDefaultsRestrictedNamedForFileInPath();
            else if (ConventionType == TypeConvention.PICKED)
                // ( WIN | MAC | LINUX | FREEBSD | OTHER ) + Defaults extra of this class
                setPickedPlusExtraDefaultsRestrictedNamedForFileInPath(HostFound);
            else // TypeCurrentConvention == CUSTOM
                // Start from List 0 element (all valid) + CUSTOM defined from parameteres user after.
                ReservedNamesForFileName = new List<string>(); // new[] { "" };
            //
            // :: Variants Always ADDed from parameter of this Convention Type. :: 
            if (_customExtraNamesDoNotUseForFileName != null)
                ReservedNamesForFileName = ReservedNamesForFileName.Concat(_customExtraNamesDoNotUseForFileName).ToList();     // CUSTOM VARIANTS
            //

        }

        #endregion

        #region ###   Static for set conventions Type       ###

        /// <summary>
        ///     Set scope of this convention or options ofor this Convention in <see cref="TypeConvention"/>
        /// </summary>
        /// <param name="validateForCurrentHostSystem">If result from current RUNTIME host</param>
        /// <param name="convention">If use a Picked partiulcar HOST</param>
        /// <param name="signinAsVariant">If from defaults Convention as applied variants on default</param>
        /// <param name="HostValidation">Return string valid for catalog this conventions</param>
        /// <param name="CustomHostName">On Custom type defined a name to attribuite for catalog.</param>
        /// <returns>Type of Convention scope</returns>
        internal static TypeConvention GetTypeHostValidation(bool validateForCurrentHostSystem, string signinAsVariant,string CustomHostName, ref HostType? convention , out string HostValidation)
        {

            // If Runtime Host or Custom or Othet not found from Runtime 
            if (validateForCurrentHostSystem)
            {
                HostValidation = RuntimeInformation.OSDescription;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) convention = HostType.WINDOWS;
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) convention = HostType.IOS;
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) convention = HostType.LINUX;
#if (!NETSTANDARD2_0)
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD)) convention = HostType.FREEBSD;
#endif
                else if (HostValidation.Contains("OTHER")) convention = HostType.OTHER;
                else {
                    convention = HostType.UKNOWED;
                }
                //
                // FOUND from runtime Host applied Defaults (from class Path) + Defaults of this Class + extra
                HostValidation = $"RUNTIME {HostValidation} {signinAsVariant}";
                return TypeConvention.RUNTIME;
            }
            else
            {
                if (convention == null ){
                    // All values extra applied for conventions
                    HostValidation = $"CUSTOM USER DEFINED FOR HOST: {CustomHostName}";
                    convention = HostType.OTHER;
                    return TypeConvention.CUSTOM;
                }
                else
                {
                    // Defaults (from class Path) + Defaults (selected) of this Class + extra
                    HostValidation = $"PICKED {convention.ToString()} {signinAsVariant}";
                    return TypeConvention.PICKED;
                }
            }
        }

        #endregion

        #region ###  Methods to Set values for Conventions  ###

        /// <summary>Fill By RUNTIME DISCOVER ListOfInvalidCharsForPath with defaultsExtraCharsNotValidPathXXXSystem</summary>
        private void setRuntimePlusExtraDefaultsListOfInvalidCharsForPath()
        {
            // :: Get Invelid Chars from Defaults Framework Path.GetInvalidPathChars ::
            ListOfInvalidCharsForPath = Path.GetInvalidPathChars().ToList<char>();        // FOR THIS HOST GET OF PATH void    

            // :: Add extra defaults Convention from this Concept Class ::            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                setPickedPlusExtraDefaultsListOfInvalidCharsForPath(HostType.WINDOWS);    // WINDOWS default + extra
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                setPickedPlusExtraDefaultsListOfInvalidCharsForPath(HostType.IOS);        // MAC default + extra
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                setPickedPlusExtraDefaultsListOfInvalidCharsForPath(HostType.LINUX);      // LINUX default + extra
            #if (!NETSTANDARD2_0)
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                setPickedPlusExtraDefaultsListOfInvalidCharsForPath(HostType.FREEBSD);    // FREEBSD default + extra
            #endif
            else
                setPickedPlusExtraDefaultsListOfInvalidCharsForPath(HostType.OTHER);      // OTHER default + extra
        }

        /// <summary>Fill By PICKED USER ListOfInvalidCharsForPath with defaultsExtraCharsNotValidPathXXXSystem</summary>
        private void setPickedPlusExtraDefaultsListOfInvalidCharsForPath(HostType convention)
        {
            if (ListOfInvalidCharsForPath == null)
                ListOfInvalidCharsForPath = new List<char>();   // [] { char.MinValue };

            // :: Add extra defaults Convention from this Concept Class ::            
            if (convention == HostType.WINDOWS)
                ListOfInvalidCharsForPath = ListOfInvalidCharsForPath.Concat(defaultsExtraCharsNotValidPathWinSystem).ToList();        // WINDOWS default + extra
            else if (convention == HostType.IOS)
                ListOfInvalidCharsForPath = ListOfInvalidCharsForPath.Concat(defaultsExtraCharsNotValidPathMacSystem).ToList();        // MAC     default + extra
            else if (convention == HostType.LINUX)
                ListOfInvalidCharsForPath = ListOfInvalidCharsForPath.Concat(defaultsExtraCharsNotValidPathLinuxSystem).ToList();      // LINUX   default + extra
            else if (convention == HostType.FREEBSD)
                ListOfInvalidCharsForPath = ListOfInvalidCharsForPath.Concat(defaultsExtraCharsNotValidPathFreebsdSystem).ToList();      // FREEBSD default + extra
            else
                ListOfInvalidCharsForPath = ListOfInvalidCharsForPath.Concat(defaultsExtraCharsNotValidPathGeneralSystem).ToList();   // OTHER   default + extra
        }

        //

        /// <summary>Fill By RUNTIME DISCOVER values name not valid for root in reservedNamesPathRootXXXSystem</summary>
        private void setRuntimePlusExtraDefaultsRestrictedNameOnRootForPath()
        {

            // :: Add extra defaults Convention from this Concept Class ::            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                setPickedPlusExtraDefaultsRestrictedNameOnRootForPath(HostType.WINDOWS);    // WINDOWS default + extra
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                setPickedPlusExtraDefaultsRestrictedNameOnRootForPath(HostType.IOS);        // MAC default + extra
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                setPickedPlusExtraDefaultsRestrictedNameOnRootForPath(HostType.LINUX);      // LINUX default + extra
            #if (!NETSTANDARD2_0)
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                setPickedPlusExtraDefaultsRestrictedNameOnRootForPath(HostType.FREEBSD);    // FREEBSD default + extra
            #endif
            else
                setPickedPlusExtraDefaultsRestrictedNameOnRootForPath(HostType.OTHER);      // OTHER default + extra
        }

        /// <summary>Fill By PICKED USER List of name reserved in HOST with reservedNamesPathRootXXXSystem</summary>
        private void setPickedPlusExtraDefaultsRestrictedNameOnRootForPath(HostType convention)
        {
            if (ReservedNamesForRoot == null)
                ReservedNamesForRoot = new List<string>(); // new[] { "" };

            // :: Add extra defaults Convention from this Concept Class ::            
            if (convention == HostType.WINDOWS)
                ReservedNamesForRoot = ReservedNamesForRoot.Concat(reservedNamesPathRootWinSystem).ToList();        // WINDOWS default + extra
            else if (convention == HostType.IOS)
                ReservedNamesForRoot = ReservedNamesForRoot.Concat(reservedNamesPathRootMacSystem).ToList();        // MAC     default + extra
            else if (convention == HostType.LINUX)
                ReservedNamesForRoot = ReservedNamesForRoot.Concat(reservedNamesPathRootLinuxSystem).ToList();      // LINUX   default + extra
            else if (convention == HostType.FREEBSD)
                ReservedNamesForRoot = ReservedNamesForRoot.Concat(reservedNamesPathRootFreebsdSystem).ToList();    // FREEBSD default + extra
            else
                ReservedNamesForRoot = ReservedNamesForRoot.Concat(reservedNamesPathRootGeneralSystem).ToList();    // OTHER   default + extra
        }

        //

        /// <summary>Fill By RUNTIME DISCOVER values name not valid for use file name in reservedNamesForFileXXXSystem</summary>
        private void setRuntimePlusExtraDefaultsRestrictedNamedForFileInPath()
        {

            // :: Add extra defaults Convention from this Concept Class ::            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                setPickedPlusExtraDefaultsRestrictedNamedForFileInPath(HostType.WINDOWS);    // WINDOWS default + extra
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                setPickedPlusExtraDefaultsRestrictedNamedForFileInPath(HostType.IOS);        // MAC default + extra
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                setPickedPlusExtraDefaultsRestrictedNamedForFileInPath(HostType.LINUX);      // LINUX default + extra
            #if (!NETSTANDARD2_0)
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                setPickedPlusExtraDefaultsRestrictedNamedForFileInPath(HostType.FREEBSD);    // FREEBSD default + extra
            #endif
            else
                setPickedPlusExtraDefaultsRestrictedNamedForFileInPath(HostType.OTHER);      // OTHER default + extra
        }

        /// <summary>Fill By PICKED USER List of name reserved do not use for filename with reservedNamesForFileXXXSystem</summary>
        private void setPickedPlusExtraDefaultsRestrictedNamedForFileInPath(HostType convention)
        {
            if (ReservedNamesForFileName == null)
                ReservedNamesForFileName = new List<string>(); // new[] { "" };

            // :: Add extra defaults Convention from this Concept Class ::            
            if (convention == HostType.WINDOWS)
                ReservedNamesForFileName = ReservedNamesForFileName.Concat(reservedNamesForFileWinSystem).ToList();        // WINDOWS default + extra
            else if (convention == HostType.IOS)
                ReservedNamesForFileName = ReservedNamesForFileName.Concat(reservedNamesForFileMacSystem).ToList();        // MAC     default + extra
            else if (convention == HostType.LINUX)
                ReservedNamesForFileName = ReservedNamesForFileName.Concat(reservedNamesForFileLinuxSystem).ToList();      // LINUX   default + extra
            else if (convention == HostType.FREEBSD)
                ReservedNamesForFileName = ReservedNamesForFileName.Concat(reservedNamesForFileFreebsdSystem).ToList();    // FREEBSD default + extra
            else
                ReservedNamesForFileName = ReservedNamesForFileName.Concat(reservedNamesForFileGeneralSystem).ToList();    // OTHER   default + extra
        }

        #endregion
    }
}
