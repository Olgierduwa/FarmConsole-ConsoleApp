using System.Runtime.InteropServices;
using System;
using System.Drawing;
using Pastel;
using System.Collections.Generic;
using FarmConsole.Body.Models;

namespace FarmConsole.Body.Services.MainServices
{
    public static class WindowService
    {
        private static int windowWidth;
        private static int windowHeight;

        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        public static void PresetWindow()
        {
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, SC_CLOSE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }
            Console.Title = LS.Navigation("title");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            SetWindow();
        }

        public static void SetWindow()
        {
            Console.CursorVisible = false;
            windowWidth = SettingsService.GetSetting("set screen width").GetRealValue;
            windowHeight = SettingsService.GetSetting("set screen height").GetRealValue;
            SoundService.SetSoundVolume();
            SoundService.SetMusicVolume();
            Console.SetWindowSize(windowWidth, windowHeight);
            Console.SetBufferSize(windowWidth, windowHeight);
            Console.SetWindowSize(windowWidth, windowHeight);
        }

        public static void Write(int X, int Y, string Text, Color Color)
        {
            if (X >= 0 && X < Console.WindowWidth && Y >= 0 && Y < Console.WindowHeight)
            {
                Console.SetCursorPosition(X, Y);
                Console.Write(Text.Pastel(Color));
            }
        }
        public static void Write(int X, int Y, List<PixelModel> Words)
        {
            if (X >= 0 && X < Console.WindowWidth && Y >= 0 && Y < Console.WindowHeight)
            {
                int Lenght = 0;
                string Text = "";
                for (int i = 0; i < Words.Count; i++)
                {
                    if (Lenght % Console.WindowWidth == 0 && i != 0)
                    {
                        Text += "\n";
                        Lenght = 0;
                    }
                    Text += Words[i].Content.Pastel(Words[i].Color);
                    Lenght += Words[i].Content.Length;
                }

                Console.SetCursorPosition(X, Y);
                Console.Write(Text);
            }
        }
    }
}
