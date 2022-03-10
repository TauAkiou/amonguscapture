﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.Reflection;
using AmongUsCapture.TextColorLibrary;

namespace AmongUsCapture_GTK.ConsoleTypes
{
    public class FormConsole : ConsoleInterface
    {
        private StreamWriter logFile;
        public MainGTKWindow form;
        private static object locker = new Object();

        private Dictionary<string, Color> ModuleColor = new Dictionary<string, Color>()
        {
            { "GameMemReader", Color.Purple } 
        };

    public FormConsole(MainGTKWindow mainWindow)
        {
            form = mainWindow;
            string directoryuri = null;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                directoryuri = Assembly.GetEntryAssembly().GetName().CodeBase.Substring(7);
            }
            else
            {
                directoryuri = Assembly.GetEntryAssembly().GetName().CodeBase.Substring(8);

            }
            logFile = File.CreateText(Path.Combine(Directory.GetParent(directoryuri).ToString(), "CaptureLog.txt"));
        }

        public void WriteTextFormatted(string text, bool acceptNewLines = true)
        {
            throw new NotImplementedException();
        }

        public void WriteColoredText(string ColoredText)
        {
            form.WriteColoredText(ColoredText);
            WriteToLog(ColoredText);
        }


        public void WriteLine(string s)
        {
            throw new NotImplementedException();
        }

        public void WriteModuleTextColored(string ModuleName, Color moduleColor, string text)
        {
            form.WriteConsoleLineFormatted(ModuleName, moduleColor, text);
            WriteToLog($"[{ModuleName}]: {text}");
        }
        
        public void WriteToLog(string textToLog)
        {
            WriteLogLine(DateTime.UtcNow, textToLog);
        }

        private string StripColor(string text)
        {
            return PangoColor.StripColor(text);
        }

        private void WriteLogLine(DateTime time, string textToLog)
        {
            lock (locker)
            {
                logFile.WriteLine($"{time.ToLongTimeString()} | {StripColor(textToLog)}");
                logFile.Flush();
            }
            
        }
    }
}
