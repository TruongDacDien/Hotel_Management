﻿#pragma checksum "..\..\..\..\View\Them_SuaTienNghi.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5CB06F9D628DFD175C8FE6B3828E31849D1136B2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
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
    /// Them_SuaTienNghi
    /// </summary>
    public partial class Them_SuaTienNghi : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 28 "..\..\..\..\View\Them_SuaTienNghi.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txbTitle;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\..\View\Them_SuaTienNghi.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtTenTN;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\..\View\Them_SuaTienNghi.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSoLuong;
        
        #line default
        #line hidden
        
        
        #line 99 "..\..\..\..\View\Them_SuaTienNghi.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCapNhat;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\..\..\View\Them_SuaTienNghi.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnThem;
        
        #line default
        #line hidden
        
        
        #line 127 "..\..\..\..\View\Them_SuaTienNghi.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnHuy;
        
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
            System.Uri resourceLocater = new System.Uri("/GUI;component/view/them_suatiennghi.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\Them_SuaTienNghi.xaml"
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
            this.txbTitle = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.txtTenTN = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.txtSoLuong = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.btnCapNhat = ((System.Windows.Controls.Button)(target));
            
            #line 99 "..\..\..\..\View\Them_SuaTienNghi.xaml"
            this.btnCapNhat.Click += new System.Windows.RoutedEventHandler(this.btnCapNhat_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnThem = ((System.Windows.Controls.Button)(target));
            
            #line 107 "..\..\..\..\View\Them_SuaTienNghi.xaml"
            this.btnThem.Click += new System.Windows.RoutedEventHandler(this.btnThem_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnHuy = ((System.Windows.Controls.Button)(target));
            
            #line 127 "..\..\..\..\View\Them_SuaTienNghi.xaml"
            this.btnHuy.Click += new System.Windows.RoutedEventHandler(this.btnHuy_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

