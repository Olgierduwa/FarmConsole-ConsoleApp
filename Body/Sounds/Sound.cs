using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Media;
using NAudio.Wave;

namespace FarmConsole.Body.Sounds
{
    public static class S
    {
        private static string location = "../../../Body/Sounds/";

        public static void play(string select)
        {
            var waveOut = new WaveOutEvent();
            switch (select)
            {
                case "K1": waveOut.Init(new WaveFileReader(location + "klik1.wav")); break;
                case "K2": waveOut.Init(new WaveFileReader(location + "klik2.wav")); break;
                case "K3": waveOut.Init(new WaveFileReader(location + "klik3.wav")); break;
            }
            waveOut.Play();
        }
    }
}
