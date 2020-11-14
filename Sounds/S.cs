using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Media;
using NAudio.Wave;

namespace FarmConsole.Model
{
    public static class S
    {
        public static void play(string select)
        {
            var waveOut = new WaveOutEvent();
            switch (select)
            {
                case "K1": waveOut.Init(new WaveFileReader("../../../Sounds/klik1.wav")); break;
                case "K2": waveOut.Init(new WaveFileReader("../../../Sounds/klik2.wav")); break;
                case "K3": waveOut.Init(new WaveFileReader("../../../Sounds/klik3.wav")); break;
            }
            waveOut.Play();
        }
    }
}
