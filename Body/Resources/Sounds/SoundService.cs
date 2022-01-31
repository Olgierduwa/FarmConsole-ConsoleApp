using FarmConsole.Body.Models;
using NAudio.Wave;
using System;

namespace FarmConsole.Body.Services.MainServices
{
    public static class SoundService
    {
        private static readonly string location = "../../../Body/Resources/Sounds/";
        private static WaveOutEvent waveSound = new WaveOutEvent();
        private static WaveOutEvent waveMusic = new WaveOutEvent();
        private static float volumeSound;
        private static float volumeMusic;

        public static void Play(string select)
        {
            //waveSound = new WaveOutEvent
            //{
            //    Volume = volumeSound
            //};
            //switch (select)
            //{
            //    case "K1": waveSound.Init(new WaveFileReader(location + "klik1.wav")); break;
            //    case "K2": waveSound.Init(new WaveFileReader(location + "klik2.wav")); break;
            //    case "K3": waveSound.Init(new WaveFileReader(location + "klik3.wav")); break;
            //}
            //waveSound.Play();
        }
        public static void PlayMusic(string select)
        {
            waveMusic = new WaveOutEvent();
            waveSound.Volume = volumeMusic;
            switch (select)
            {
                case "M1": waveMusic.Init(new WaveFileReader(location + "music1.wav")); break;
                case "M2": waveMusic.Init(new WaveFileReader(location + "music2.wav")); break;
                case "M3": waveMusic.Init(new WaveFileReader(location + "music3.wav")); break;
            }
            waveMusic.Play();
        }
        public static void SetSoundVolume(int vol = -1)
        {
            SettingModel Setting = SettingsService.GetSetting("set effects volume");
            if (vol < 0) vol = Setting.GetRealValue;
            waveSound.Volume = volumeSound = Convert.ToSingle(vol) / Setting.GetMaxSliderValue;
        }
        public static void SetMusicVolume(int vol = -1)
        {
            SettingModel Setting = SettingsService.GetSetting("set music volume");
            if (vol < 0) vol = Setting.GetRealValue;
            waveMusic.Volume = volumeMusic = Convert.ToSingle(vol) / Setting.GetMaxSliderValue;
        }
    }
}
