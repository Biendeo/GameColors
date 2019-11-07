using System.IO;
using UnityEngine;

namespace GameColors
{
    public class Helpers
    {
        public static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] array = new Color[width * height];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = col;
            }

            Texture2D texture2D = new Texture2D(width, height);
            texture2D.SetPixels(array);
            texture2D.Apply();

            return texture2D;
        }

        public static Sprite CreateSpriteFromTex(string filePath, Vector2 pivot)
        {
            Texture2D texture2D = new Texture2D(1, 1);
            byte[] array = File.ReadAllBytes(filePath);

            if (ImageConversion.LoadImage(texture2D, array))
            {
                return Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), pivot);
            }

            return null;
        }

        public static Color RGBtoUnity(float r, float g, float b, float alpha)
        {
            return new Color(r / 255f, g / 255f, b / 255f, alpha / 255f);
        }

        public static float UnityToRGB(float val)
        {
            return val * 255f;
        }

        public static T GetComponent<T>()
        {
            foreach (GameObject gameObject in Object.FindObjectsOfType<GameObject>())
            {
                if (gameObject.GetComponent<T>() != null)
                {
                    return gameObject.GetComponent<T>();
                }
            }

            return default;
        }
    }
}
