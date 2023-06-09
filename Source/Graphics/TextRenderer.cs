﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AstralAssault;

public static class TextRenderer
{
    private static Texture2D _font;

    private static Dictionary<char, int> _dict;

    public static void Init()
    {
        _font = AssetManager.LoadTexture("font");
        _dict = new Dictionary<char, int>
        {
            { 'A', 0 },
            { 'B', 1 },
            { 'C', 2 },
            { 'D', 3 },
            { 'E', 4 },
            { 'F', 5 },
            { 'G', 6 },
            { 'H', 7 },
            { 'I', 8 },
            { 'J', 9 },
            { 'K', 10 },
            { 'L', 11 },
            { 'M', 12 },
            { 'N', 13 },
            { 'O', 14 },
            { 'P', 15 },
            { 'Q', 16 },
            { 'R', 17 },
            { 'S', 18 },
            { 'T', 19 },
            { 'U', 20 },
            { 'V', 21 },
            { 'W', 22 },
            { 'X', 23 },
            { 'Y', 24 },
            { 'Z', 25 },
            { '0', 26 },
            { '1', 27 },
            { '2', 28 },
            { '3', 29 },
            { '4', 30 },
            { '5', 31 },
            { '6', 32 },
            { '7', 33 },
            { '8', 34 },
            { '9', 35 },
            { ':', 36 },
            { ';', 37 },
            { '.', 38 },
            { ',', 39 },
            { ' ', 40 }
        };
    }

    public static void Write(this SpriteBatch spriteBatch, string input, Vector2 position, Color color)
    {
        string text = input.ToUpper();
        
        for (int i = 0; i < text.Length; i++)
        {
            int x = _dict[text[i]] % 6;
            int y = _dict[text[i]] / 6;

            Rectangle source = new(x * 10, y * 10, 10, 10);

            spriteBatch.Draw(
                _font,
                new Vector2(position.X + i * 10, position.Y),
                source,
                color,
                0,
                Vector2.Zero,
                1,
                SpriteEffects.None,
                0);
        }
    }
}