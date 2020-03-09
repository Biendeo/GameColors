using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GameColors
{
    public static class GameObjects
    {
        public static FieldInfo noteColourField;

        public static FieldInfo animColourField;

        public static FieldInfo sustainColourField;

        public static SpriteRenderer[] defCol;

        public static List<Color> allColours = new List<Color>();

        public static ParticleSystem[] allParticles;

        public static class Frets
        {
            public static void setAllColours(Color RGBcol)
            {
                Color colour = Helpers.Colour_RGB2Unity(RGBcol);
                GameObjects.Frets.green.setColour(colour);
                GameObjects.Frets.red.setColour(colour);
                GameObjects.Frets.yellow.setColour(colour);
                GameObjects.Frets.blue.setColour(colour);
                GameObjects.Frets.orange.setColour(colour);
            }

            public static void resetColours()
            {
                GameObjects.Frets.green.reset();
                GameObjects.Frets.red.reset();
                GameObjects.Frets.yellow.reset();
                GameObjects.Frets.blue.reset();
                GameObjects.Frets.orange.reset();
                TweakMain.setColours(GameObjects.Frets.defColours);
            }

            public static void updateRainbow()
            {
                Color color = Helpers.Colour_RGB2Unity(GameObjects.allColours[TweakMain.rainbowIndex[0]]);
                Color color2 = Helpers.Colour_RGB2Unity(GameObjects.allColours[TweakMain.rainbowIndex[1]]);
                Color color3 = Helpers.Colour_RGB2Unity(GameObjects.allColours[TweakMain.rainbowIndex[2]]);
                Color color4 = Helpers.Colour_RGB2Unity(GameObjects.allColours[TweakMain.rainbowIndex[3]]);
                Color color5 = Helpers.Colour_RGB2Unity(GameObjects.allColours[TweakMain.rainbowIndex[4]]);

                GameObjects.Frets.green.setColour(color);
                GameObjects.Frets.red.setColour(color2);
                GameObjects.Frets.yellow.setColour(color3);
                GameObjects.Frets.blue.setColour(color4);
                GameObjects.Frets.orange.setColour(color5);

                TweakMain.setColours(new Color[]
                {
                    color,
                    color2,
                    color3,
                    color4,
                    color5,
                    color3
                });

                string[] array = new string[]
                {
                    "G",
                    "R",
                    "Y",
                    "B",
                    "O"
                };

                Color[] array2 = new Color[]
                {
                    color,
                    color2,
                    color3,
                    color4,
                    color5
                };

                string text = "";
                text += "Starting rainbow setting...\n";

                for (int i = 0; i < GameObjects.allParticles.Length; i++)
                {
                    text = string.Concat(new string[]
                    {
                        text,
                        array[i % 5],
                        " : ",
                        GameObjects.allParticles[i].name,
                        "\n"
                    });

                    ParticleSystem.MainModule main = GameObjects.allParticles[i].main;
                    if (GameObjects.allParticles[i].name.Contains("Green"))
                    {
                        main.startColor = color;
                    }
                    else if (GameObjects.allParticles[i].name.Contains("Red"))
                    {
                        main.startColor = color2;
                    }
                    else if (GameObjects.allParticles[i].name.Contains("Yellow"))
                    {
                        main.startColor = color3;
                    }
                    else if (GameObjects.allParticles[i].name.Contains("Blue"))
                    {
                        main.startColor = color4;
                    }
                    else if (GameObjects.allParticles[i].name.Contains("Orange"))
                    {
                        main.startColor = color5;
                    }

                    text += "Finished rainbow setting...\n";
                    Debug.Log(text);
                }
            }


            public static GameObjects.Fret green;

            public static GameObjects.Fret red;

            public static GameObjects.Fret yellow;

            public static GameObjects.Fret blue;

            public static GameObjects.Fret orange;

            public static Color[] defColours;
        }

        public class Fret
        {
            public Fret(SpriteRenderer[] spriteRenderers)
            {
                this.renderers = spriteRenderers;
                this.sprites = new Sprite[this.renderers.Length];
                this.defaultColours = new Color[this.renderers.Length];

                for (int i = 0; i < this.renderers.Length; i++)
                {
                    this.defaultColours[i] = this.renderers[i].color;
                    this.sprites[i] = this.renderers[i].sprite;
                }
            }

            public void setColour(Color col)
            {
                for (int i = 0; i < this.renderers.Length; i++)
                {
                    if (this.renderers[i].name != "head")
                    {
                        this.renderers[i].color = col;
                    }
                }

                this.flames.color = col;
            }

            public void reset()
            {
                for (int i = 0; i < this.renderers.Length; i++)
                {
                    this.renderers[i].color = this.defaultColours[i];
                    this.renderers[i].sprite = this.sprites[i];
                }
            }

            public SpriteRenderer flames;

            public List<ParticleSystem> particles = new List<ParticleSystem>();

            public SpriteRenderer[] renderers;

            public Color[] defaultColours;

            public Sprite[] sprites;
        }
    }
}
