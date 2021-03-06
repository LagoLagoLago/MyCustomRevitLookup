#region Header
//
// Copyright 2003-2018 by Autodesk, Inc. 
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted, 
// provided that the above copyright notice appears in all copies and 
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting 
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC. 
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to 
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
#endregion // Header

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace RevitLookup
{

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class App : IExternalApplication
    {
        static AddInId m_appId = new AddInId(new Guid("356CDA5A-E6C5-4c2f-A9EF-B3222116B8C8"));

        // get the absolute path of this assembly
        static string ExecutingAssemblyPath = Assembly.GetExecutingAssembly().Location;

        private AppDocEvents m_appDocEvents;

        public Result OnStartup(UIControlledApplication application)
        {

            // Call this method explicitly in App.cs when Revit starts up because,
            // in .NET 4, the static variables will not be initialized until use them.
            Snoop.Collectors.CollectorObj.InitializeCollectors();
            AddMenu(application);
            AddAppDocEvents(application.ControlledApplication);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            RemoveAppDocEvents();

            return Result.Succeeded;
        }

        private void AddMenu(UIControlledApplication app)
        {
            //菜单栏
            RibbonPanel rvtRibbonPanel = app.CreateRibbonPanel("Revit Lookup");
            PulldownButtonData data = new PulldownButtonData("Options", "Revit Lookup");
            RibbonItem item = rvtRibbonPanel.AddItem(data);

            //下拉框
            PulldownButton optionsBtn = item as PulldownButton;
            // Add Icons to main RevitLookup Menu
            optionsBtn.Image = GetEmbeddedImage("RevitLookup.Resources.RLookup-16.png");
            optionsBtn.LargeImage = GetEmbeddedImage("RevitLookup.Resources.RLookup-32.png");
            //去掉HelloWorld的命令
            //optionsBtn.AddPushButton(new PushButtonData("HelloWorld", "Hello World...", ExecutingAssemblyPath, "RevitLookup.HelloWorld"));
            optionsBtn.AddPushButton(new PushButtonData("查看数据库...", "查看数据库...", ExecutingAssemblyPath, "RevitLookup.CmdSnoopDb"));
            optionsBtn.AddPushButton(new PushButtonData("查看当前选择...", "查看当前选择...", ExecutingAssemblyPath, "RevitLookup.CmdSnoopModScope"));
            optionsBtn.AddPushButton(new PushButtonData("查看活动视图...", "查看活动视图...", ExecutingAssemblyPath, "RevitLookup.CmdSnoopActiveView"));
            optionsBtn.AddPushButton(new PushButtonData("查看应用...", "查看应用...", ExecutingAssemblyPath, "RevitLookup.CmdSnoopApp"));
            optionsBtn.AddPushButton(new PushButtonData("按ID查看...", "按ID查看...", ExecutingAssemblyPath, "RevitLookup.CmdSearchBy"));
            //optionsBtn.AddPushButton(new PushButtonData("Test Framework...", "Test Framework...", ExecutingAssemblyPath, "RevitLookup.CmdTestShell"));

            var pushbutton = rvtRibbonPanel.AddItem(new PushButtonData("快速查看", "快速查看", ExecutingAssemblyPath, "RevitLookup.CmdSnoopModScope")) as PushButton;
            pushbutton.Image = GetEmbeddedImage("RevitLookup.Resources.RLookup-16.png");
            pushbutton.LargeImage = GetEmbeddedImage("RevitLookup.Resources.RLookup-32.png");

            var pushbuttonSearch = rvtRibbonPanel.AddItem(new PushButtonData("按ID查看", "按ID查看", ExecutingAssemblyPath, "RevitLookup.CmdSearchBy")) as PushButton;
            pushbuttonSearch.Image = GetEmbeddedImage("RevitLookup.Resources.RLookup-16.png");
            pushbuttonSearch.LargeImage = GetEmbeddedImage("RevitLookup.Resources.RLookup-32.png");
        }

        private void AddAppDocEvents(ControlledApplication app)
        {
            m_appDocEvents = new AppDocEvents(app);
            m_appDocEvents.EnableEvents();
        }

        private void RemoveAppDocEvents()
        {
            m_appDocEvents.DisableEvents();
        }

        /// <summary>
        /// 获取嵌入的图片
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static BitmapSource GetEmbeddedImage(string name)
        {
            try
            {
                Assembly a = Assembly.GetExecutingAssembly();
                Stream s = a.GetManifestResourceStream(name);
                return BitmapFrame.Create(s);
            }
            catch
            {
                return null;
            }
        }
    }
}
