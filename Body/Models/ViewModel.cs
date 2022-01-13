using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Models
{
    public class ViewModel
    {
        private Point _Position { get; set; }
        public Point GetPosition => _Position;
        private Size _Size { get; set; }
        public Size GetSize => _Size;
        private PixelModel[,] _Pixels { get; set; }
        public PixelModel[,] GetPixels => _Pixels;
        private List<PixelModel> _Words { get; set; }
        public List<PixelModel> GetWords
        {
            get
            {
                if (_Words == null || _Edited == true) SetWords();
                return _Words;
            }
        }
        private bool _Edited { get; set; }

        public ViewModel(PixelModel[,] _pixels, int _width, int _height, Point _pos = new Point())
        {
            _Position = new Point(_pos.X, _pos.Y);
            _Pixels = new PixelModel[_width, _height];
            for (int x = 0; x < _width; x++)
                for (int y = 0; y < _height; y++)
                    if (_pixels[x, y] != null)
                        _Pixels[x, y] = _pixels[x, y].PixelClone;
            _Size = new Size(_width, _height);
            _Edited = false;
        }
        public ViewModel ViewClone()
        {
            return new ViewModel(_Pixels, _Size.Width, _Size.Height);
        }
        public PixelModel GetPixel(int x, int y)
        {
            if (x >= 0 && x < _Size.Width && y >= 0 && y < _Size.Height)
                return _Pixels[x, y];
            else return null;
        }
        public void SetPixel(int x, int y, PixelModel Pixel)
        {
            if(x >= 0 && x < _Size.Width && y >= 0 && y < _Size.Height && Pixel != null)
                _Pixels[x, y] = Pixel.PixelClone;
            _Edited = true;
        }
        public void SetWord(int x, int y, string content, Color color)
        {
            for (int i = 0; i < content.Length; i++)
            {
                if (_Pixels[x + i, y] == null) _Pixels[x + i, y] = new PixelModel();
                _Pixels[x + i, y].Content = content[i].ToString();
                _Pixels[x + i, y].Color = color;
            }
        }
        public void SetWords()
        {
            Color Background = ColorService.BackgroundColor;
            PixelModel word = null;
            _Words = new List<PixelModel>();
            for (int y = 0; y < _Size.Height; y++)
            {
                for (int x = 0; x < _Size.Width; x++)
                {
                    if (word != null)
                        if (_Pixels[x, y] != null && _Pixels[x, y].Color == word.Color)
                            word.Content += _Pixels[x, y].Content;
                        else { _Words.Add(word); word = null; }

                    if (word == null && _Pixels[x, y] != null) /* && Pixels[x, y].Color != Background)*/
                        word = new PixelModel()
                        {
                            Position = new Point(x, y),
                            Color = _Pixels[x, y].Color,
                            Content = _Pixels[x, y].Content
                        };
                }
                if (word != null) _Words.Add(word); word = null;
            }
            _Edited = false;
        }
        public void DisplayPixels()
        {
            foreach (var word in GetWords)
                WindowService.Write(word.Position.X + GetPosition.X, word.Position.Y + GetPosition.Y, word.Content, word.Color);

            _Edited = false;
        }
        public void DisplayPixels(Point Position, Size Size)
        {
            int Y = Position.Y - GetPosition.Y < 0 ? 0 : Position.Y;
            int X = Position.X - GetPosition.X < 0 ? 0 : Position.X;
            int Height = Position.Y + Size.Height - GetPosition.Y > GetSize.Height ?
                GetSize.Height + GetPosition.Y : Position.Y + Size.Height;
            int Width = Position.X + Size.Width - GetPosition.X > GetSize.Width ?
                GetSize.Width + GetPosition.X : Position.X + Size.Width;
            Height -= Position.Y - GetPosition.Y < 0 && Height - Position.Y - GetPosition.Y > 0 ?
                Position.Y - GetPosition.Y : 0;
            Width -= Position.X - GetPosition.X < 0 && Width - Position.X - GetPosition.X > 0 ?
                Position.X - GetPosition.X : 0;
            if (Position.Y <= GetSize.Height && Position.X <= GetSize.Width)
            {
                PixelModel word = null;
                List<PixelModel> _words = new List<PixelModel>();

                for (int y = Y; y < Height; y++)
                {
                    for (int x = X; x < Width; x++)
                    {
                        var pixel = GetPixel(x - GetPosition.X, y - GetPosition.Y);
                        if (word != null)
                            if (pixel != null && pixel.Color == word.Color)
                                word.Content += pixel.Content;
                            else { _words.Add(word); word = null; }

                        if (word == null && pixel != null) /* && Pixels[x, y].Color != Background)*/
                            word = new PixelModel()
                            {
                                Position = new Point(x, y),
                                Color = pixel.Color,
                                Content = pixel.Content
                            };
                    }
                    if (word != null) _words.Add(word); word = null;
                }

                foreach(var pixel in _words)
                    WindowService.Write(pixel.Position.X, pixel.Position.Y, pixel.Content, pixel.Color);

                _Edited = false;
            }
        }
        public void ColorizeWords(string _Type, int _Procent)
        {
            foreach (var word in GetWords)
            {
                Color color = word.Color;
                if (_Procent < 0)
                    switch (_Type)
                    {
                        case "Darker": color = ColorService.Darker(color); break;
                        case "Brighter": color = ColorService.Brighter(color); break;
                        case "Bluer": color = ColorService.Bluer(color); break;
                        case "Greener": color = ColorService.Greener(color); break;
                        case "Yellower": color = ColorService.Yellower(color); break;
                    }
                else
                    switch (_Type)
                    {
                        case "Darker": color = ColorService.Darker(color, _Procent); break;
                        case "Brighter": color = ColorService.Brighter(color, _Procent); break;
                        case "Bluer": color = ColorService.Bluer(color, _Procent); break;
                        case "Greener": color = ColorService.Greener(color, _Procent); break;
                        case "Yellower": color = ColorService.Yellower(color, _Procent); break;
                    }
                word.Color = color;
            }
        }
        public void ColorizePixels(string _Type, int _Procent = -1)
        {
            Size size = _Size;
            for (int x = 0; x < size.Width; x++)
                for (int y = 0; y < size.Height; y++)
                    if (_Pixels[x, y] != null)
                    {
                        PixelModel pixel = _Pixels[x, y];
                        Color color = pixel.Color;
                        if (_Procent < 0)
                            switch (_Type)
                            {
                                case "Darker": color = ColorService.Darker(color); break;
                                case "Brighter": color = ColorService.Brighter(color); break;
                                case "Bluer": color = ColorService.Bluer(color); break;
                                case "Greener": color = ColorService.Greener(color); break;
                                case "Yellower": color = ColorService.Yellower(color); break;
                            }
                        else
                            switch (_Type)
                            {
                                case "Darker": color = ColorService.Darker(color, _Procent); break;
                                case "Brighter": color = ColorService.Brighter(color, _Procent); break;
                                case "Bluer": color = ColorService.Bluer(color, _Procent); break;
                                case "Greener": color = ColorService.Greener(color, _Procent); break;
                                case "Yellower": color = ColorService.Yellower(color, _Procent); break;
                            }
                        pixel.Color = color;
                        _Pixels[x, y] = pixel;
                    }
            SetWords();
        }
    }
}
