using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameColors
{
    public class KeyBindConfig
    {
        public static void WriteToConfig(KeyCode key)
        {
            using (StreamWriter streamWriter = new StreamWriter(KeyBindConfig.configLoc))
            {
                if (KeyBindConfig.keyBinds.Count > 0)
                {
                    bool flag = false;
                    foreach (string text in KeyBindConfig.keyBinds)
                    {
                        if (text.ToLower().Contains(KeyBindConfig.modName))
                        {
                            KeyBindConfig.keyBinds.Remove(text);
                            streamWriter.WriteLine(KeyBindConfig.modName + "-" + key.ToString());
                            flag = true;
                        }
                        else
                        {
                            streamWriter.WriteLine(text);
                        }
                    }

                    if (!flag)
                    {
                        streamWriter.WriteLine(KeyBindConfig.modName + "-" + key.ToString());
                        KeyBindConfig.menuKey = key;
                        return;
                    }
                }
                else
                {
                    streamWriter.WriteLine(KeyBindConfig.modName + "-" + key.ToString());
                }

                KeyBindConfig.menuKey = key;
            }
        }

        public static void LoadFromConfig()
        {
            if (!File.Exists(KeyBindConfig.configLoc))
            {
                KeyBindConfig.menuKey = DEFAULT_KEY;
                return;
            }

            string[] array = File.ReadAllLines(KeyBindConfig.configLoc);
            if (array.Length != 0)
            {
                foreach (string text in array)
                {
                    KeyBindConfig.keyBinds.Add(text);

                    if (text.ToLower().Contains(KeyBindConfig.modName))
                    {
                        KeyBindConfig.menuKey = (KeyCode)Enum.Parse(typeof(KeyCode), text.Split(new char[]
                        {
                            '-'
                        })[1]);
                    }
                }

                return;
            }

            KeyBindConfig.menuKey = DEFAULT_KEY;
        }

        public static bool keyBindAlreadyUsed = false;

        private static string configLoc = Environment.CurrentDirectory + "/Tweaks/Config/Keybinds.cfg";

        private static List<string> keyBinds = new List<string>();

        public static KeyCode menuKey = KeyCode.F2;

        private const KeyCode DEFAULT_KEY = KeyCode.F2;

        private static string modName = "gamecolors";
    }
}
