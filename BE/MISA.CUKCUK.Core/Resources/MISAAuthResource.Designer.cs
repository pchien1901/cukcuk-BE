﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MISA.CUKCUK.Core.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class MISAAuthResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MISAAuthResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MISA.CUKCUK.Core.Resources.MISAAuthResource", typeof(MISAAuthResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid access token or refresh token.
        /// </summary>
        public static string InvalidToken {
            get {
                return ResourceManager.GetString("InvalidToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Người dùng không tồn tại.
        /// </summary>
        public static string InvalidUserDevMsg {
            get {
                return ResourceManager.GetString("InvalidUserDevMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tên đăng nhập hoặc mật khẩu không chính xác.
        /// </summary>
        public static string InvalidUsernameOrPassword {
            get {
                return ResourceManager.GetString("InvalidUsernameOrPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Người dùng không tồn tại hoặc đã bị xóa.
        /// </summary>
        public static string InvalidUserUserMsg {
            get {
                return ResourceManager.GetString("InvalidUserUserMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to tokenModel không tồn tại.
        /// </summary>
        public static string NullToken {
            get {
                return ResourceManager.GetString("NullToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Không thể gia hạn đăng nhập.
        /// </summary>
        public static string NullTokenUser {
            get {
                return ResourceManager.GetString("NullTokenUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Vui lòng đăng nhập để sử dụng dịch vụ.
        /// </summary>
        public static string PleaseLoginToUseWeb {
            get {
                return ResourceManager.GetString("PleaseLoginToUseWeb", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Đăng ký người dùng thất bại.
        /// </summary>
        public static string RegisterFail {
            get {
                return ResourceManager.GetString("RegisterFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Người dùng đã tồn tại.
        /// </summary>
        public static string UserIsExist {
            get {
                return ResourceManager.GetString("UserIsExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Người dùng chưa được xác thực.
        /// </summary>
        public static string UserIsNotAuthenticated {
            get {
                return ResourceManager.GetString("UserIsNotAuthenticated", resourceCulture);
            }
        }
    }
}
