﻿/*
    Copyright 2013 Fred Bowker
    This file is part of WsdlUI.
    WsdlUI is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
    WsdlUI is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.Drawing;
using System.Windows.Forms;
using WsdlUI.App.UI.Windows.Properties;
using process = Drexyia.WebSvc.Process;
using utils = Drexyia.Utils;


namespace WsdlUI.App.UI.Windows {
    static class Program {
        static readonly object _locker = new object();
        static bool _exceptionOccurred;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {

            utils.Logger.Instance.XmlConfig();
            utils.Logger.Instance.Log.Info("Program Start");

            Application.ThreadException += Application_ThreadException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try {

                var mainForm = (args.Length > 0)
                    ? new UI.FormMain(args[0])
                    : new UI.FormMain();

                var settings = Settings.Default;
                if (settings.WindowPosition != Point.Empty)
                {
                    mainForm.StartPosition = FormStartPosition.Manual;
                    mainForm.Left = settings.WindowPosition.X;
                    mainForm.Top = settings.WindowPosition.Y;    
                }
                if (settings.WindowSize != Size.Empty)
                {
                    mainForm.Size = settings.WindowSize;
                }

                mainForm.OnException += mainForm_OnException;

                Application.Run(mainForm);

                if (!_exceptionOccurred) {
                    mainForm.CleanUp();

                    settings.WindowPosition = new Point(mainForm.Left, mainForm.Top);
                    settings.WindowSize = mainForm.Size;

                    settings.Save();
                }
            }
            catch (Exception ex) //handle exception on startup
            {
                HandleException(ex);
            }

        }

        static void mainForm_OnException(object sender, process.WebSvcAsync.EventParams.ExceptionAsyncArgs e) {
            HandleException(e.Ex);
        }

        //handle exception on ui thread
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
            HandleException(e.Exception);
        }

        static void HandleException(Exception ex) {

            utils.Logger.Instance.Log.Error(ex.ToString());

            lock (_locker) {
                if (!_exceptionOccurred) {
                    _exceptionOccurred = true;

                    WsdlUI.App.UI.Dialogs.dg_Error dialog = new WsdlUI.App.UI.Dialogs.dg_Error();
                    dialog.PopulateForm(ex);

                    dialog.ShowDialog();

                    if (System.Windows.Forms.Application.MessageLoop) {

                        // Use this since we are a WinForms app
                        Application.Exit();
                    }
                    else {

                        // Use this since we are a console app
                        Environment.Exit(1);
                    }

                }
            }
        }
    }
}
