﻿#pragma checksum "..\..\..\..\View\WebThanhToan.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "F19D7B70F688A6E930CD4F935DA614D9D27EDCC6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using GUI.UserControls;
using GUI.View;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace GUI.View {
    
    
    /// <summary>
    /// WebThanhToan
    /// </summary>
    public partial class WebThanhToan : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\..\View\WebThanhToan.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal GUI.View.WebThanhToan WebThanhToans;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\View\WebThanhToan.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.RectangleGeometry boGoc;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\View\WebThanhToan.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Web.WebView2.Wpf.WebView2 webView;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.5.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/GUI;component/view/webthanhtoan.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\WebThanhToan.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.5.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.WebThanhToans = ((GUI.View.WebThanhToan)(target));
            
            #line 14 "..\..\..\..\View\WebThanhToan.xaml"
            this.WebThanhToans.Loaded += new System.Windows.RoutedEventHandler(this.WebThanhToans_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.boGoc = ((System.Windows.Media.RectangleGeometry)(target));
            return;
            case 3:
            this.webView = ((Microsoft.Web.WebView2.Wpf.WebView2)(target));
            
            #line 22 "..\..\..\..\View\WebThanhToan.xaml"
            this.webView.NavigationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs>(this.WebView2_NavigationCompleted);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

