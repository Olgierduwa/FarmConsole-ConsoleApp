using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Models
{
    public class ViewModel
    {
        private Size Size { get; set; }
        private PixelModel[,] Pixels { get; set; }
        private List<PixelModel> Words { get; set; }
        private bool Edited { get; set; }

        public Size GetSize()
        {
            return Size;
        }
        public void SetView(int TopBorder = -1, int BotBorder = -1)
        {
            if (TopBorder < 0) TopBorder = 0;
            if (BotBorder < 0) BotBorder = Size.Height;
            Color Background = ColorService.BackgroundColor;
            PixelModel word = null;
            Words = new List<PixelModel>();
            for (int y = TopBorder; y < BotBorder; y++)
            {
                for (int x = 0; x < Size.Width; x++)
                {
                    if (word != null)
                        if (Pixels[x, y] != null && Pixels[x, y].Color == word.Color)
                            word.Content += Pixels[x, y].Content;
                        else { Words.Add(word); word = null; }

                    if (word == null && Pixels[x, y] != null) /* && Pixels[x, y].Color != Background)*/
                        word = new PixelModel()
                        {
                            Position = new Point(x, y),
                            Color = Pixels[x, y].Color,
                            Content = Pixels[x, y].Content
                        };
                }
                if (word != null) Words.Add(word); word = null;
            }
        }

        public void SetWord(int x, int y, string word, Color color)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (Pixels[x + i, y] == null) Pixels[x + i, y] = new PixelModel();
                Pixels[x + i, y].Content = word[i].ToString();
                Pixels[x + i, y].Color = color;
            }
        }
        public List<PixelModel> GetWords()
        {
            if (Words == null || Edited == true) SetView(); 
            return Words;
        }

        public void SetPixel(int x, int y, PixelModel Pixel)
        {
            if(x >= 0 && x < Size.Width && y >= 0 && y < Size.Height && Pixel != null)
                Pixels[x, y] = Pixel.PixelClone;
        }
        public PixelModel GetPixel(int x, int y)
        {
            if (x >= 0 && x < Size.Width && y >= 0 && y < Size.Height)
                return Pixels[x, y];
            else return null;
        }

        public ViewModel(PixelModel[,] _pixels, int _width, int _height)
        {
            Pixels = new PixelModel[_width, _height];
            for (int x = 0; x < _width; x++)
                for (int y = 0; y < _height; y++)
                    if (_pixels[x, y] != null)
                        Pixels[x, y] = _pixels[x, y].PixelClone;
            Size = new Size(_width, _height);
            Edited = false;
        }
        public ViewModel ViewClone()
        {
            return new ViewModel(Pixels, Size.Width, Size.Height);
        }
    }
}
