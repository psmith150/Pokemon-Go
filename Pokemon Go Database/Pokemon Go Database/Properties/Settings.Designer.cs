﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Pokemon_Go_Database.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\patricks\\Documents\\GitHub\\Pokemon-Go\\Pokemon Go Database\\Data")]
        public string DefaultDirectory {
            get {
                return ((string)(this["DefaultDirectory"]));
            }
            set {
                this["DefaultDirectory"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection LastFiles {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["LastFiles"]));
            }
            set {
                this["LastFiles"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<Species><IV%>")]
        public string SuggestedName {
            get {
                return ((string)(this["SuggestedName"]));
            }
            set {
                this["SuggestedName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("D:\\GitHub\\Pokemon-Go\\Pokemon Go Database\\Pokemon Go Database\\Resources\\BaseData.x" +
            "ml")]
        public string BaseDataDirectory {
            get {
                return ((string)(this["BaseDataDirectory"]));
            }
            set {
                this["BaseDataDirectory"] = value;
            }
        }
    }
}
