using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace HelperAndToolsForTest.IO
{

    /// <summary>
    ///     On Policy define a properties common for lenght
    ///     of elements well to be compose a path on your elements.
    /// </summary>
    public struct PolicyLengths
    {
        /// <summary>For convention use a length max for name of all directory</summary>
        public int NameOfDirectory;
        /// <summary>For convention use a length max for name of all FileName</summary>
        public int NameOfFile;
        /// <summary>For convention use a length max for string of path complete</summary>
        public int EntirePath;
    }

    /// <summary>
    ///     Options for Type conventions validation in use from this HostTypeConvention 
    ///     at the moment of the instance or Picked from list defaults or custom.
    /// </summary>
    public class OptionsConvention
    {

        #region ###             PUBLIC PROPERTIES           ###

        /// <summary>
        ///     Name and description of HOST and type conventions is in use of 
        ///     OS filesytem if resolved convention from host in execution, or Name of 
        ///     Host Picked on choice from use, or if custom name attribuite for ipotetic host.
        /// </summary>
        public string HostValidation { get; private set; }

        /// <summary>
        ///     Return type of validation setted for this instance of host convention.
        /// </summary>
        public TypeConvention ConventionType { get; private set; }

        /// <summary>
        ///     Return Host Convention in use.
        /// </summary>
        public HostType HostConvention { get; private set; }

        #endregion

        #region ###     All Properties Policy for this Host Convention      ###

        /// <summary>
        ///     Max lengths for Directory Name, File Name, Entire Path, etc.
        /// </summary>
        public PolicyLengths Lengths { get; private set; }

        /// <summary>
        ///     Return assignment in convention applied from runtime or customized from user, if do use end of path do not end with period or space.
        /// </summary>
        public bool CheckEndNameOnPath { get; private set; }

        #endregion

        #region ###     .ctors RUNTIME PICKED OR CUSTOM     ###

        /// <summary>
        ///     RUNTIME - Return a default conventions in use of current FileSystem at Runtime
        /// </summary>
        public OptionsConvention(
                    bool? useRestrictiveEndName = null                   // Optional to add other invalidate for default in win system if Filename ends with space or period is not convention to accept
            ) : this(true,null)
            {}

        /// <summary>
        ///     PICKED - Return a default conventions  from user to use in path and file system target.
        /// </summary>
        /// <param name="convention">Set convention predefineted from choice HOST managed from this class.</param>
        public OptionsConvention(
                    HostType convention                   // Picked from user type host
            ) : this(false, convention)
            {}

        /// <summary>
        ///     CUSTOM -Return a custom conventions definited from user to use in path and file system target.
        /// </summary>
        /// <param name="NameofCustomHost">Name of Host Convention well to be use this Options</param>
        /// <param name="customTypeConvention">Object TypeConvention with properties to apply for this object for alter changes</param>
        public OptionsConvention(
                    string NameofCustomHost,                        // Name of Custom HOST for definition.
                    OptionsConvention customTypeConvention      // On Custom picked this object is used for apply Variant on this Convention.
            ) : this(false, null, customTypeConvention)
            {}

        /// <summary>
        ///     Return a default options of convention in use of current FileSystem at Runtime if argument <paramref name="validateForCurrentHostSystem"/> is true. 
        ///     The Behavior is in case of true for Runtime Host in execution, is if Runtime in not resolved for 
        ///     cause of system os with filesystem custom, this object use other params to apply custom conventions in case of OTHER system.
        ///     Also in case of <paramref name="validateForCurrentHostSystem"/> is false the custom conventions in other arguments is 
        ///     applyed in mode esclusive without add internal clausole of system in execution.
        /// </summary>
        /// <param name="validateForCurrentHostSystem"> if True = check if chars based on current Host Type file system also not check and use only extraInvalidCharsForPath if filled or null, and if windows system check FullQualifiedPath for root</param>
        /// <param name="convention">Valid only if validateForCurrentHostSystem = false, set convention predefineted from choide.</param>
        /// <param name="customTypeConvention">Object TypeConvention with properies to apply on this object for alter changes</param>
        /// <param name="NameHostForCustomConvention">Optional for name attribuite to host with custom convention to apply.</param>
        private OptionsConvention(
                    bool validateForCurrentHostSystem,                      // True = check if chars based on current Host Type file system also not check and use only extraInvalidCharsForPath if filled or null
                    HostType? convention = null,              // Valid only if validateForCurrentHostSystem = false, set convention predefineted from choide.
                    OptionsConvention customTypeConvention = null,      // On Custom picked this object is used for apply Variant on this Convention.
                    string NameHostForCustomConvention = null               // Optional for name attribuite to host with custom convention to apply.
                )
            {

            string variant = "DEFAULTS";
            // RUNTIME
            if (validateForCurrentHostSystem) { 
                if(customTypeConvention != null) variant = "ALTERED";
            }
            // PICKED
            else if(!validateForCurrentHostSystem && convention.HasValue) {
                if (customTypeConvention != null) variant = "ALTERED";
            } else // CUSTOM
                variant = "CUSTOM";

            // :: :: Set Type Current Convention for Validations :: ::
            ConventionType = HostTypeConvention.GetTypeHostValidation(validateForCurrentHostSystem,variant,NameHostForCustomConvention, ref convention, out string hostValidation);
            HostValidation = hostValidation;

            // :: :: Apply Options for parent Convention :: ::
            // :: :: CONVENTION CHARS NOT VALID :: ::
            //
            // Fill Defaults Chars Not Valid which by convention are classified as not usable as a sequence in the
            // path of this host system, this from Path return from evneiroment OS list of default.
            if (ConventionType == TypeConvention.RUNTIME)
                // ( WIN | MAC | LINUX | FREEBSD | OTHER ) + Defaults extra of this class
                setRuntimeHostDefaultPolicyOptionsForPathOrFileName();
            else if (ConventionType == TypeConvention.PICKED)
                // ( WIN | MAC | LINUX | FREEBSD | OTHER ) + Defaults extra of this class
                setPickedHostDefaultPolicyOptionsForPathOrFileName(convention.Value);
            else  // Have TypeCurrentOptionsConvention == CUSTOM
            {
                // :: Apply custom properties if user has defined for every property differet value ::
                                
            }
            //

            // :: :: Now have convention :: ::
            HostConvention = convention.Value;

        }

        #endregion

        #region ###         Void Private to Set Policy Options Convention       ###

        private void setRuntimeHostDefaultPolicyOptionsForPathOrFileName()
        {
            // :: Add extra defaults Convention from this Concept Class ::            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                applyDefaultsOptionsPolicyForOSWindows();   // WINDOWS 
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                applyDefaultsOptionsPolicyForOSX();         // MAC 
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                applyDefaultsOptionsPolicyForOSLinux();     // LINUX 
            #if (!NETSTANDARD2_0)
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                applyDefaultsOptionsPolicyForOSFreeBSD();  // FREEBSD default 
            #endif
            else
                applyDefaultsOptionsPolicyForOSGeneral();  // OTHER general OS

        }

        private void setPickedHostDefaultPolicyOptionsForPathOrFileName(HostType convention) {

            // :: Add extra defaults Convention from this Concept Class ::            
            if (convention == HostType.WINDOWS)
                applyDefaultsOptionsPolicyForOSWindows();   // WINDOWS 
            else if (convention == HostType.IOS)
                applyDefaultsOptionsPolicyForOSX();         // MAC 
            else if (convention == HostType.LINUX)
                applyDefaultsOptionsPolicyForOSLinux();     // LINUX 
            else if (convention == HostType.FREEBSD)
                applyDefaultsOptionsPolicyForOSFreeBSD();  // FREEBSD default 
            else
                applyDefaultsOptionsPolicyForOSGeneral();  // FOR UKNOWED Set in OTHER for general OS
        }

        /****   POLICY DEFAULT FOR ALL MANAGED OS INTERNALLY OR GENERAL         ***/

        /*  List Of Policy with associated relative Property of this Object.:
         *  
         *      MaxLengthForElements        =   For all subelements well to be compe a path valuate lengths restrictions
         *      UseRestrictiveEndNameOnPath =   Check if Path end with space or period.
         * 
        * */

        /// <summary>Defaults for Systems base on FreeBSD</summary>
        private void applyDefaultsOptionsPolicyForOSGeneral(){
            this.Lengths = new PolicyLengths() { EntirePath = -1, NameOfDirectory = -1, NameOfFile = -1 };
            this.CheckEndNameOnPath = true;
        }

        /// <summary>Defaults for Systems base on Windows</summary>
        private void applyDefaultsOptionsPolicyForOSWindows() {
            this.Lengths = new PolicyLengths() { EntirePath = -1, NameOfDirectory = -1, NameOfFile = -1 };
            this.CheckEndNameOnPath = true;
        }

        /// <summary>Defaults for Systems base on OSX MAC</summary>
        private void applyDefaultsOptionsPolicyForOSX(){
            this.Lengths = new PolicyLengths() { EntirePath = -1, NameOfDirectory = -1, NameOfFile = -1 };
            this.CheckEndNameOnPath = true;
        }

        /// <summary>Defaults for Systems base on LINUX</summary>
        private void applyDefaultsOptionsPolicyForOSLinux(){
            this.Lengths = new PolicyLengths() { EntirePath = -1, NameOfDirectory = -1, NameOfFile = -1 };
            this.CheckEndNameOnPath = true;
        }

        /// <summary>Defaults for Systems base on FreeBSD</summary>
        private void applyDefaultsOptionsPolicyForOSFreeBSD(){
            this.Lengths = new PolicyLengths() { EntirePath = -1, NameOfDirectory = -1, NameOfFile = -1 };
            this.CheckEndNameOnPath = true;
        }

        #endregion
    }
}
