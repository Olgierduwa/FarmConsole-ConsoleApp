using FarmConsole.Body.Model.Helpers;
using NAudio.Wave;
using System;

namespace FarmConsole.Body.Sounds
{
    public static class S
    {
        private static readonly string location = "../../../Body/Sounds/";
        private static float volume = Convert.ToSingle(OPTIONS.GetOptionById(2)) / 6.0f;
        private static WaveOutEvent waveOut = new WaveOutEvent();

        public static void Play(string select)
        {
            waveOut = new WaveOutEvent();
            switch (select)
            {
                case "K1": waveOut.Init(new WaveFileReader(location + "klik1.wav")); break;
                case "K2": waveOut.Init(new WaveFileReader(location + "klik2.wav")); break;
                case "K3": waveOut.Init(new WaveFileReader(location + "klik3.wav")); break;
            }
            waveOut.Play();
        }
        public static void SetVolume(int vol)
        {
            volume = Convert.ToSingle(vol) / 6.0f;
            waveOut.Volume = volume;
        }
    }
}
