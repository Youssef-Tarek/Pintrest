﻿#pragma checksum "..\..\..\..\UserControls\boardUserControl.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "609EEE2B44F1BB317F926B88AB1E25180352DF8E828C061CE816F4E259E9ED1D"
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
using RootLibrary.WPF.Localization;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


namespace Pinterest_V2.UserControls {
    
    
    /// <summary>
    /// boardUserControl
    /// </summary>
    public partial class boardUserControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\..\UserControls\boardUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button boardButton;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\UserControls\boardUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button EditMyBoard_btn;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\UserControls\boardUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.PackIcon EditMyBoard_Icon;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\UserControls\boardUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button DeleteMyBoard_btn;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\UserControls\boardUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.PackIcon DeleteMyBoard_btn_Icon;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Pinterest V2;component/usercontrols/boardusercontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\UserControls\boardUserControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 7 "..\..\..\..\UserControls\boardUserControl.xaml"
            ((Pinterest_V2.UserControls.boardUserControl)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.UserControl_MouseEnter);
            
            #line default
            #line hidden
            
            #line 7 "..\..\..\..\UserControls\boardUserControl.xaml"
            ((Pinterest_V2.UserControls.boardUserControl)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.UserControl_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 2:
            this.boardButton = ((System.Windows.Controls.Button)(target));
            
            #line 9 "..\..\..\..\UserControls\boardUserControl.xaml"
            this.boardButton.Click += new System.Windows.RoutedEventHandler(this.boardButton_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.EditMyBoard_btn = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\..\..\UserControls\boardUserControl.xaml"
            this.EditMyBoard_btn.Click += new System.Windows.RoutedEventHandler(this.EditMyBoard_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.EditMyBoard_Icon = ((MaterialDesignThemes.Wpf.PackIcon)(target));
            return;
            case 5:
            this.DeleteMyBoard_btn = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\..\..\UserControls\boardUserControl.xaml"
            this.DeleteMyBoard_btn.Click += new System.Windows.RoutedEventHandler(this.DeleteMyBoard_btn_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.DeleteMyBoard_btn_Icon = ((MaterialDesignThemes.Wpf.PackIcon)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
