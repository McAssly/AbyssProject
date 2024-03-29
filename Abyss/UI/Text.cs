﻿using Abyss.Globals;
using Microsoft.Xna.Framework;
using System;
using System.Reflection.Emit;
using System.Text;

namespace Abyss.UI
{
    internal class Text
    {
        private protected Vector2 position;
        private protected float width;
        private protected float height;
        private protected float scale;
        private protected string text;

        public Text(string text, int x, int y, float scale)
        {
            this.text = text;
            this.position = new Vector2(x, y);
            this.scale = scale;
        }

        public Text(string text, int x, int y, int width, float height, float scale) 
        {
            this.text = text;
            this.position = new Vector2(x, y);
            this.width = width;
            this.height = height;
            this.scale = scale;
        }

        public Text(string text, Vector2 position, float scale)
        {
            this.text = text;
            this.position = position;
            this.scale = scale;
        }

        public float GetPixelWidth()
        {
            return CalculatePixelWidth(text, scale);
        }

        public static float CalculatePixelWidth(string text, float scale)
        {
            int perfect = PerfectWidth(text);
            int imperfect = ImperfectWidth(text);
            return (perfect + imperfect) * scale;
        }

        private static int ImperfectWidth(string text)
        {
            StringBuilder imperfect_width = new StringBuilder();
            foreach (char c in text)
                if (!char.IsLetter(c) || !AcceptableWidth(c) || !char.IsDigit(c))
                    imperfect_width.Append(c);
            string imperfect = imperfect_width.ToString();
            int width = 0;
            foreach (char c in imperfect)
                width += PxWidth(c);
            return width;
        }

        private static int PerfectWidth(string text)
        {
            StringBuilder perfect_width = new StringBuilder();
            foreach (char c in text)
                if (char.IsLetter(c) || AcceptableWidth(c) || char.IsDigit(c))
                    perfect_width.Append(c);
            return perfect_width.Length * (14 + Variables.TextSpacing);
        }

        private static int PxWidth(char c)
        {
            switch (c)
            {
                case ' ': return 1 + Variables.TextSpacing;
                case '!': case '|': case '\'': return 3 + Variables.TextSpacing;
                case '*': case '-': case '"': return 9 + Variables.TextSpacing;
                case '(': case ')': case '[': case ']': case '{': case '}': return 11 + Variables.TextSpacing;
                case '.': case ',': case ';': case ':': return 6 + Variables.TextSpacing;
                default: return 0;
            }
        }

        private static bool AcceptableWidth(char c)
        {
            switch (c)
            {
                case '@': case '~': case '#': case '$': case '%': case '/': case '\\': case '^': case '&':
                case '=': case '_': case '+': case '?': case '<': case '>':
                    return true;
                default: return false;
            }
        }


        public static string FormatInWidth(string text, int width, float scale)
        {
            string[] lines = text.Split('\n');
            StringBuilder formatted = new StringBuilder();

            foreach (string line in lines)
            {
                string formatted_line = FormatLineInWidth(line, width, scale);
                formatted.AppendLine(formatted_line);
            }
            return formatted.ToString().Trim();
        }


        private static string FormatLineInWidth(string line, int width, float scale)
        {
            StringBuilder formatted = new StringBuilder();
            string[] words = line.Split(' ');

            int length = 0;

            foreach (string word in words)
            {
                float word_length = CalculatePixelWidth(word, scale);

                if (length + word_length + 1 <= width) formatted.Append(word + ' ');
                else formatted.Append('\n' + word + ' ');
                length += (int)word_length + 1;
            }

            return formatted.ToString().Trim();
        }



        public void Append(string text)
        {
            this.text += text;
        }

        public void Delete()
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Remove(text.Length - 1);
            }
        }

        public void Update(string new_text)
        {
            if (text != new_text)
                text = new_text;
        }

        public void SetPosition(float x, float y)
        {
            this.position = new Vector2(x, y);
        }
        public float GetWidth() { return width; }
        public float GetHeight() { return height; }
        public float GetX() { return position.X; }
        public float GetY() { return position.Y; }
        public Vector2 GetPosition() { return position; }
        public float GetScale() { return scale; }
        public string GetText() { return text; }

        internal Text CreateAtOffset(float x_offset, float y_offset)
        {
            return new Text(this.text, (int)(this.position.X + x_offset), (int)(this.position.Y + y_offset), this.scale);
        }
    }
}
