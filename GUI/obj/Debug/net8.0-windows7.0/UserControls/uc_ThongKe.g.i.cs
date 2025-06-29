﻿#pragma checksum "..\..\..\..\UserControls\uc_ThongKe.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B8D6194FEDE80014A84E7EB3F5AF0C54A4C3F565"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using LiveCharts.Wpf;
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


namespace GUI.UserControls {
    
    
    /// <summary>
    /// uc_ThongKe
    /// </summary>
    public partial class uc_ThongKe : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 35 "..\..\..\..\UserControls\uc_ThongKe.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txbRevenuePhongThisMonth;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\..\UserControls\uc_ThongKe.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txbRevenueDVThisMonth;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\..\..\UserControls\uc_ThongKe.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtsoPhong;
        
        #line default
        #line hidden
        
        
        #line 124 "..\..\..\..\UserControls\uc_ThongKe.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox MonthSelector;
        
        #line default
        #line hidden
        
        
        #line 154 "..\..\..\..\UserControls\uc_ThongKe.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox YearSelector;
        
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
            System.Uri resourceLocater = new System.Uri("/GUI;component/usercontrols/uc_thongke.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\UserControls\uc_ThongKe.xaml"
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
            this.txbRevenuePhongThisMonth = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.txbRevenueDVThisMonth = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.txtsoPhong = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            
            #line 96 "..\..\..\..\UserControls\uc_ThongKe.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnBaoCao_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.MonthSelector = ((System.Windows.Controls.ComboBox)(target));
            
            #line 125 "..\..\..\..\UserControls\uc_ThongKe.xaml"
            this.MonthSelector.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.OnMonthYearChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.YearSelector = ((System.Windows.Controls.ComboBox)(target));
            
            #line 155 "..\..\..\..\UserControls\uc_ThongKe.xaml"
            this.YearSelector.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.OnMonthYearChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

