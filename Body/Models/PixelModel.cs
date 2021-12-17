using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Models
{
    public class PixelModel
    {
        public string Content { get; set; }
        public Point Position { get; set; }
        public Color Color { get; set; }

        public override string ToString() => Content;
        public PixelModel PixelClone => new PixelModel()
        {
            Content = Content,
            Color = Color.FromArgb(Color.R, Color.G, Color.B),
            Position = new Point(Position.X, Position.Y)
        };
    }
}
