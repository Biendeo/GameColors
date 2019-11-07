using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace GameColors
{
    public class TweakMain : MonoBehaviour
    {
        private bool configExists()
        {
            return File.Exists(this.configLoc);
        }

        private bool isStarPower
        {
            get
            {
                return this.sideLGlow.color.a > 0;
            }
        }

        private void OnStarPower()
        {
            bool flag = this.spRan;
        }

        private Type GetNoteField()
        {
            byte[] bytes = Encoding.Unicode.GetBytes("̙̗̘̜̎̔̍̓̑̚̕");
            foreach (Type type in this.asm.GetTypes())
            {
                if (type.Namespace == null && BitConverter.ToString(Encoding.Unicode.GetBytes(type.Name)) == BitConverter.ToString(bytes))
                {
                    return type;
                }
            }

            return null;
        }

        private FieldInfo FindField(string fieldStr, string debugName = "")
        {
            byte[] bytes = Encoding.Unicode.GetBytes(fieldStr);
            foreach (FieldInfo fieldInfo in this.noteType.GetFields())
            {
                if (BitConverter.ToString(Encoding.Unicode.GetBytes(fieldInfo.Name)) == BitConverter.ToString(bytes))
                {
                    return fieldInfo;
                }
            }
            Debug.Log("Could not find " + debugName);
            return null;
        }

        private void InitNoteColors()
        {
            this.noteType = this.GetNoteField();
            if (this.noteType != null)
            {
                this.noteColorField = this.FindField(this.noteColorSig, "Notes");
                this.animColorField = this.FindField(this.animColorsSig, "Note Anims");
                this.sustainColorField = this.FindField(this.sustainColorSig, "Sustain");
                this.cyanColorField = this.FindField(this.cyanColorSig, "Cyan");
                this.cyanAnimField = this.FindField(this.cyanAnimSig, "Cyan Anim");
                this.purpleColorField = this.FindField(this.purpleColorSig, "Purple Color");
                this.openSustainField = this.FindField(this.openSustainSig, "Open Sustain");
                return;
            }
            Debug.Log("Fatal Error: Note field is null!");
        }

        private void SetOpenNotesColor(Color color)
        {
            //Color color2 = (Color)this.openSustainField.GetValue(null);
            this.openSustainField.SetValue(null, color);

            //Color color3 = (Color)this.purpleColorField.GetValue(null);
            this.purpleColorField.SetValue(null, color);
        }

        private void SetStarPowerNotes(Color color)
        {
            this.cyanColorField.SetValue(null, color);
            this.cyanAnimField.SetValue(null, color);
        }

        private void SetNoteColor(int index, Color color)
        {
            if (index <= 5)
            {
                Color[] array = this.noteColorField.GetValue(null) as Color[];
                array[index] = color;
                this.noteColorField.SetValue(null, array);

                if (index < 5)
                {
                    Color[] array2 = this.animColorField.GetValue(null) as Color[];
                    array2[index] = color;
                    this.animColorField.SetValue(null, array2);
                }
            }
        }

        private void SetSustainColor(int index, Color color)
        {
            if (index <= 4)
            {
                Color[] array = this.sustainColorField.GetValue(null) as Color[];
                array[index] = color;
                this.sustainColorField.SetValue(null, array);
            }
        }

        private Sprite CreateSpriteFromTex(string filePath)
        {
            Texture2D texture2D = new Texture2D(2, 2);
            byte[] array = File.ReadAllBytes(filePath);

            if (ImageConversion.LoadImage(texture2D, array))
            {
                return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0), 100);
            }

            return null;
        }

        private Texture2D TexFromFile(string texPath)
        {
            if (!File.Exists(texPath))
            {
                return null;
            }

            byte[] array = File.ReadAllBytes(texPath);
            Texture2D texture2D = new Texture2D(2, 2);
            if (ImageConversion.LoadImage(texture2D, array))
            {
                return texture2D;
            }

            return null;
        }

        private bool isPracticeEnabled()
        {
            return GameObject.Find("SPBar/lowerbar") == null;
        }

        private bool isBotEnabled()
        {
            return GameObject.Find("Multiplier") == null;
        }

        private void LoadBG(string pName)
        {
            try
            {
                string[] array = File.ReadAllLines(Environment.CurrentDirectory + "/Tweaks/Config/GC Profiles/" + pName);
                if (array[34] != "empty")
                {
                    this.bgSprite = this.CreateSpriteFromTex(array[34]);
                    this.currBgFile = array[34];
                    this.bgSaveProfile = true;
                    this.bgPersist = true;
                }

                if (array[35] != "empty")
                {
                    this.hwTex = this.TexFromFile(array[35]);
                    this.currHwFile = array[35];
                    this.hwSaveProfile = true;
                    this.hwPersist = true;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Error occurred loading BG: " + ex.Message);
            }
        }

        private void LoadConfig(string pName)
        {
            string[] array = File.ReadAllLines(Environment.CurrentDirectory + "/Tweaks/Config/GC Profiles/" + pName);
            this.LoadBG(pName);

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Contains("RB"))
                {
                    switch (i)
                    {
                        case 0:
                            this.greenRB = true;
                            break;
                        case 1:
                            this.redRB = true;
                            break;
                        case 2:
                            this.yellowRB = true;
                            break;
                        case 3:
                            this.blueRB = true;
                            break;
                        case 4:
                            this.orangeRB = true;
                            break;
                        case 5:
                            this.spRB = true;
                            break;
                        case 6:
                            this.gfRB = true;
                            break;
                        case 7:
                            this.rfRB = true;
                            break;
                        case 8:
                            this.yfRB = true;
                            break;
                        case 9:
                            this.bfRB = true;
                            break;
                        case 10:
                            this.ofRB = true;
                            break;
                        case 11:
                            this.glRB = true;
                            break;
                        case 12:
                            this.rlRB = true;
                            break;
                        case 13:
                            this.ylRB = true;
                            break;
                        case 14:
                            this.blRB = true;
                            break;
                        case 15:
                            this.olRB = true;
                            break;
                        case 16:
                            this.spblRB = true;
                            break;
                        case 17:
                            this.spbuRB = true;
                            break;
                        case 18:
                            this.spbLtRB = true;
                            break;
                    }
                }
                else if (!(array[i] == "empty") && i != 35 && i != 34)
                {
                    string[] array2 = array[i].Split(new char[]
                    {
                        '|'
                    });
                    float[] array3 = new float[array2.Length];

                    for (int j = 0; j < array2.Length; j++)
                    {
                        array3[j] = float.Parse(array2[j]);
                    }

                    switch (i)
                    {
                        case 0:
                            this.greenR = array3[0];
                            this.greenG = array3[1];
                            this.greenB = array3[2];
                            this.greenA = array3[3];
                            this.greenActive = true;
                            break;
                        case 1:
                            this.redR = array3[0];
                            this.redG = array3[1];
                            this.redB = array3[2];
                            this.redA = array3[3];
                            this.redActive = true;
                            break;
                        case 2:
                            this.yellowR = array3[0];
                            this.yellowG = array3[1];
                            this.yellowB = array3[2];
                            this.yellowA = array3[3];
                            this.yellowActive = true;
                            break;
                        case 3:
                            this.blueR = array3[0];
                            this.blueG = array3[1];
                            this.blueB = array3[2];
                            this.blueA = array3[3];
                            this.blueActive = true;
                            break;
                        case 4:
                            this.orangeR = array3[0];
                            this.orangeG = array3[1];
                            this.orangeB = array3[2];
                            this.orangeA = array3[3];
                            this.orangeActive = true;
                            break;
                        case 5:
                            this.spR = array3[0];
                            this.spG = array3[1];
                            this.spB = array3[2];
                            this.spA = array3[3];
                            this.spActive = true;
                            break;
                        case 6:
                            this.gfR = array3[0];
                            this.gfG = array3[1];
                            this.gfB = array3[2];
                            this.gfA = array3[3];
                            this.gfActive = true;
                            break;
                        case 7:
                            this.rfR = array3[0];
                            this.rfG = array3[1];
                            this.rfB = array3[2];
                            this.rfA = array3[3];
                            this.rfActive = true;
                            break;
                        case 8:
                            this.yfR = array3[0];
                            this.yfG = array3[1];
                            this.yfB = array3[2];
                            this.yfA = array3[3];
                            this.yfActive = true;
                            break;
                        case 9:
                            this.bfR = array3[0];
                            this.bfG = array3[1];
                            this.bfB = array3[2];
                            this.bfA = array3[3];
                            this.bfActive = true;
                            break;
                        case 10:
                            this.ofR = array3[0];
                            this.ofG = array3[1];
                            this.ofB = array3[2];
                            this.ofA = array3[3];
                            this.ofActive = true;
                            break;
                        case 11:
                            this.glR = array3[0];
                            this.glG = array3[1];
                            this.glB = array3[2];
                            this.glA = array3[3];
                            this.glActive = true;
                            break;
                        case 12:
                            this.rlR = array3[0];
                            this.rlG = array3[1];
                            this.rlB = array3[2];
                            this.rlA = array3[3];
                            this.rlActive = true;
                            break;
                        case 13:
                            this.ylR = array3[0];
                            this.ylG = array3[1];
                            this.ylB = array3[2];
                            this.ylA = array3[3];
                            this.ylActive = true;
                            break;
                        case 14:
                            this.blR = array3[0];
                            this.blG = array3[1];
                            this.blB = array3[2];
                            this.blA = array3[3];
                            this.blActive = true;
                            break;
                        case 15:
                            this.olR = array3[0];
                            this.olG = array3[1];
                            this.olB = array3[2];
                            this.olA = array3[3];
                            this.olActive = true;
                            break;
                        case 16:
                            this.spblR = array3[0];
                            this.spblG = array3[1];
                            this.spblB = array3[2];
                            this.spblA = array3[3];
                            this.spblActive = true;
                            break;
                        case 17:
                            this.spbuR = array3[0];
                            this.spbuG = array3[1];
                            this.spbuB = array3[2];
                            this.spbuA = array3[3];
                            this.spbuActive = true;
                            break;
                        case 18:
                            this.spbLtR = array3[0];
                            this.spbLtG = array3[1];
                            this.spbLtB = array3[2];
                            this.spbLtA = array3[3];
                            this.spbLtActive = true;
                            break;
                        case 19:
                            this.gcR = array3[0];
                            this.gcG = array3[1];
                            this.gcB = array3[2];
                            this.gcA = array3[3];
                            this.gcActive = true;
                            break;
                        case 20:
                            this.rcR = array3[0];
                            this.rcG = array3[1];
                            this.rcB = array3[2];
                            this.rcA = array3[3];
                            this.rcActive = true;
                            break;
                        case 21:
                            this.ycR = array3[0];
                            this.ycG = array3[1];
                            this.ycB = array3[2];
                            this.ycA = array3[3];
                            this.ycActive = true;
                            break;
                        case 22:
                            this.bcR = array3[0];
                            this.bcG = array3[1];
                            this.bcB = array3[2];
                            this.bcA = array3[3];
                            this.bcActive = true;
                            break;
                        case 23:
                            this.ocR = array3[0];
                            this.ocG = array3[1];
                            this.ocB = array3[2];
                            this.ocA = array3[3];
                            this.ocActive = true;
                            break;
                        case 24:
                            this.ghR = array3[0];
                            this.ghG = array3[1];
                            this.ghB = array3[2];
                            this.ghA = array3[3];
                            this.ghActive = true;
                            break;
                        case 25:
                            this.rhR = array3[0];
                            this.rhG = array3[1];
                            this.rhB = array3[2];
                            this.rhA = array3[3];
                            this.rhActive = true;
                            break;
                        case 26:
                            this.yhR = array3[0];
                            this.yhG = array3[1];
                            this.yhB = array3[2];
                            this.yhA = array3[3];
                            this.yhActive = true;
                            break;
                        case 27:
                            this.bhR = array3[0];
                            this.bhG = array3[1];
                            this.bhB = array3[2];
                            this.bhA = array3[3];
                            this.bhActive = true;
                            break;
                        case 28:
                            this.ohR = array3[0];
                            this.ohG = array3[1];
                            this.ohB = array3[2];
                            this.ohA = array3[3];
                            this.ohActive = true;
                            break;
                        case 29:
                            this.ghlR = array3[0];
                            this.ghlG = array3[1];
                            this.ghlB = array3[2];
                            this.ghlA = array3[3];
                            this.ghlActive = true;
                            break;
                        case 30:
                            this.rhlR = array3[0];
                            this.rhlG = array3[1];
                            this.rhlB = array3[2];
                            this.rhlA = array3[3];
                            this.rhlActive = true;
                            break;
                        case 31:
                            this.yhlR = array3[0];
                            this.yhlG = array3[1];
                            this.yhlB = array3[2];
                            this.yhlA = array3[3];
                            this.yhlActive = true;
                            break;
                        case 32:
                            this.bhlR = array3[0];
                            this.bhlG = array3[1];
                            this.bhlB = array3[2];
                            this.bhlA = array3[3];
                            this.bhlActive = true;
                            break;
                        case 33:
                            this.ohlR = array3[0];
                            this.ohlG = array3[1];
                            this.ohlB = array3[2];
                            this.ohlA = array3[3];
                            this.ohlActive = true;
                            break;
                        case 36:
                            this.mnR = array3[0];
                            this.mnG = array3[1];
                            this.mnB = array3[2];
                            this.mnA = array3[3];
                            this.mnActive = true;
                            break;
                        case 37:
                            this.mcR = array3[0];
                            this.mcG = array3[1];
                            this.mcB = array3[2];
                            this.mcA = array3[3];
                            this.mcActive = true;
                            break;
                        case 38:
                            this.songpbR = array3[0];
                            this.songpbG = array3[1];
                            this.songpbB = array3[2];
                            this.songpbA = array3[3];
                            this.songpbActive = true;
                            break;
                        case 39:
                            this.starpbR = array3[0];
                            this.starpbG = array3[1];
                            this.starpbB = array3[2];
                            this.starpbA = array3[3];
                            this.starpbActive = true;
                            break;
                        case 40:
                            this.osR = array3[0];
                            this.osG = array3[1];
                            this.osB = array3[2];
                            this.osA = array3[3];
                            this.osActive = true;
                            break;
                        case 41:
                            this.isR = array3[0];
                            this.isG = array3[1];
                            this.isB = array3[2];
                            this.isA = array3[3];
                            this.isActive = true;
                            break;
                        case 42:
                            this.scTextR = array3[0];
                            this.scTextG = array3[1];
                            this.scTextB = array3[2];
                            this.scTextA = array3[3];
                            this.scTextActive = true;
                            break;
                        case 43:
                            this.fcR = array3[0];
                            this.fcG = array3[1];
                            this.fcB = array3[2];
                            this.fcA = array3[3];
                            this.fcActive = true;
                            break;
                        case 44:
                            this.onR = array3[0];
                            this.onG = array3[1];
                            this.onB = array3[2];
                            this.onA = array3[3];
                            this.onActive = true;
                            break;
                        case 45:
                            this.spGlowR = array3[0];
                            this.spGlowG = array3[1];
                            this.spGlowB = array3[2];
                            this.spGlowActive = true;
                            break;
                        case 46:
                            this.particleGR = array3[0];
                            this.particleGG = array3[1];
                            this.particleGB = array3[2];
                            this.particleGA = array3[3];
                            this.pgActive = true;
                            break;
                        case 47:
                            this.particleRR = array3[0];
                            this.particleRG = array3[1];
                            this.particleRB = array3[2];
                            this.particleRA = array3[3];
                            this.prActive = true;
                            break;
                        case 48:
                            this.particleYR = array3[0];
                            this.particleYG = array3[1];
                            this.particleYB = array3[2];
                            this.particleYA = array3[3];
                            this.pyActive = true;
                            break;
                        case 49:
                            this.particleBR = array3[0];
                            this.particleBG = array3[1];
                            this.particleBB = array3[2];
                            this.particleBA = array3[3];
                            this.pbActive = true;
                            break;
                        case 50:
                            this.particleOR = array3[0];
                            this.particleOG = array3[1];
                            this.particleOB = array3[2];
                            this.particleOA = array3[3];
                            this.poActive = true;
                            break;
                        case 51:
                            this.SetTheme((int)array3[0]);
                            this.currentThemeIndex = (int)array3[0];
                            this.saveMenuTheme = true;
                            break;
                        case 52:
                            this.particleSize = array3[0];
                            this.particleSpeed = array3[1];
                            this.particleGravity = array3[2];
                            this.particleMax = array3[3];
                            this.particleSettingsActive = true;
                            break;
                        case 53:
                            this.scoreFontR = array3[0];
                            this.scoreFontG = array3[1];
                            this.scoreFontB = array3[2];
                            this.scoreFontActive = true;
                            break;
                        case 54:
                            this.comboFontR = array3[0];
                            this.comboFontG = array3[1];
                            this.comboFontB = array3[2];
                            this.comboFontActive = true;
                            break;
                        case 55:
                            this.strikelGR = array3[0];
                            this.strikelGG = array3[1];
                            this.strikelGB = array3[2];
                            this.strikelGA = array3[3];
                            this.strikeGActive = true;
                            break;
                        case 56:
                            this.strikelRR = array3[0];
                            this.strikelRG = array3[1];
                            this.strikelRB = array3[2];
                            this.strikelRA = array3[3];
                            this.strikeRActive = true;
                            break;
                        case 57:
                            this.strikelYR = array3[0];
                            this.strikelYG = array3[1];
                            this.strikelYB = array3[2];
                            this.strikelYA = array3[3];
                            this.strikeYActive = true;
                            break;
                        case 58:
                            this.strikelBR = array3[0];
                            this.strikelBG = array3[1];
                            this.strikelBB = array3[2];
                            this.strikelBA = array3[3];
                            this.strikeBActive = true;
                            break;
                        case 59:
                            this.strikelOR = array3[0];
                            this.strikelOG = array3[1];
                            this.strikelOB = array3[2];
                            this.strikelOA = array3[3];
                            this.strikeOActive = true;
                            break;
                        case 60:
                            this.sR = array3[0];
                            this.sG = array3[1];
                            this.sB = array3[2];
                            this.sActive = true;
                            break;
                    }
                }
            }
        }

        private void SaveConfig(string fileName)
        {
            using (StreamWriter streamWriter = new StreamWriter(Environment.CurrentDirectory + "/Tweaks/Config/GC Profiles/" + fileName + ".cfg"))
            {
                if (!this.greenRB && this.greenActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.greenR,
                        "|",
                        this.greenG,
                        "|",
                        this.greenB,
                        "|",
                        this.greenA
                    }));
                }
                else if (this.greenRB)
                {
                    streamWriter.WriteLine("RB");
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (!this.redRB && this.redActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.redR,
                        "|",
                        this.redG,
                        "|",
                        this.redB,
                        "|",
                        this.redA
                    }));
                }
                else if (this.redRB)
                {
                    streamWriter.WriteLine("RB");
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (!this.yellowRB && this.yellowActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.yellowR,
                        "|",
                        this.yellowG,
                        "|",
                        this.yellowB,
                        "|",
                        this.yellowA
                    }));
                }
                else if (this.yellowRB)
                {
                    streamWriter.WriteLine("RB");
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (!this.blueRB && this.blueActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.blueR,
                        "|",
                        this.blueG,
                        "|",
                        this.blueB,
                        "|",
                        this.blueA
                    }));
                }
                else if (this.blueRB)
                {
                    streamWriter.WriteLine("RB");
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (!this.orangeRB && this.orangeActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.orangeR,
                        "|",
                        this.orangeG,
                        "|",
                        this.orangeB,
                        "|",
                        this.orangeA
                    }));
                }
                else if (this.orangeRB)
                {
                    streamWriter.WriteLine("RB");
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (!this.spRB && this.spActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.spR,
                        "|",
                        this.spG,
                        "|",
                        this.spB,
                        "|",
                        this.spA
                    }));
                }
                else if (this.spRB)
                {
                    streamWriter.WriteLine("RB");
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (!this.gfRB && this.gfActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.gfR,
                        "|",
                        this.gfG,
                        "|",
                        this.gfB,
                        "|",
                        this.gfA
                    }));
                }
                else if (this.gfRB)
                {
                    streamWriter.WriteLine("RB");
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (!this.rfRB && this.rfActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.rfR,
                        "|",
                        this.rfG,
                        "|",
                        this.rfB,
                        "|",
                        this.rfA
                    }));
                }
                else if (this.rfRB)
                {
                    streamWriter.WriteLine("RB");
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (!this.yfRB && this.yfActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.yfR,
                        "|",
                        this.yfG,
                        "|",
                        this.yfB,
                        "|",
                        this.yfA
                    }));
                }
                else if (this.yfRB)
                {
                    streamWriter.WriteLine("RB");
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (!this.bfRB && this.bfActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.bfR,
                        "|",
                        this.bfG,
                        "|",
                        this.bfB,
                        "|",
                        this.bfA
                    }));
                }
                else if (this.bfRB)
                {
                    streamWriter.WriteLine("RB");
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (!this.ofRB && this.ofActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.ofR,
                        "|",
                        this.ofG,
                        "|",
                        this.ofB,
                        "|",
                        this.ofA
                    }));
                }
                else if (this.ofRB)
                {
                    streamWriter.WriteLine("RB");
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (!this.glRB && this.glActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.glR,
                        "|",
                        this.glG,
                        "|",
                        this.glB,
                        "|",
                        this.glA
                    }));
                }
                else if (this.glRB)
                {
                    streamWriter.WriteLine("RB");
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (!this.rlRB && this.rlActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.rlR,
                        "|",
                        this.rlG,
                        "|",
                        this.rlB,
                        "|",
                        this.rlA
                    }));
                }
                else if (this.rlRB)
                {
                    streamWriter.WriteLine("RB");
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (!this.ylRB && this.ylActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.ylR,
                        "|",
                        this.ylG,
                        "|",
                        this.ylB,
                        "|",
                        this.ylA
                    }));
                }
                else if (this.ylRB)
                {
                    streamWriter.WriteLine("RB");
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (!this.blRB && this.blActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.blR,
                        "|",
                        this.blG,
                        "|",
                        this.blB,
                        "|",
                        this.blA
                    }));
                }
                else if (this.blRB)
                {
                    streamWriter.WriteLine("RB");
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (!this.olRB && this.olActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.olR,
                        "|",
                        this.olG,
                        "|",
                        this.olB,
                        "|",
                        this.olA
                    }));
                }
                else if (this.olRB)
                {
                    streamWriter.WriteLine("RB");
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (!this.spblRB && this.spblActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.spblR,
                        "|",
                        this.spblG,
                        "|",
                        this.spblB,
                        "|",
                        this.spblA
                    }));
                }
                else if (this.spblRB)
                {
                    streamWriter.WriteLine("RB");
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (!this.spbuRB && this.spbuActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.spbuR,
                        "|",
                        this.spbuG,
                        "|",
                        this.spbuB,
                        "|",
                        this.spbuA
                    }));
                }
                else if (this.spbuRB)
                {
                    streamWriter.WriteLine("RB");
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (!this.spbLtRB && this.spbLtActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.spbLtR,
                        "|",
                        this.spbLtG,
                        "|",
                        this.spbLtB,
                        "|",
                        this.spbLtA
                    }));
                }
                else if (this.spbLtRB)
                {
                    streamWriter.WriteLine("RB");
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.gcActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.gcR,
                        "|",
                        this.gcG,
                        "|",
                        this.gcB,
                        "|",
                        this.gcA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.rcActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.rcR,
                        "|",
                        this.rcG,
                        "|",
                        this.rcB,
                        "|",
                        this.rcA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.ycActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.ycR,
                        "|",
                        this.ycG,
                        "|",
                        this.ycB,
                        "|",
                        this.ycA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.bcActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.bcR,
                        "|",
                        this.bcG,
                        "|",
                        this.bcB,
                        "|",
                        this.bcA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.ocActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.ocR,
                        "|",
                        this.ocG,
                        "|",
                        this.ocB,
                        "|",
                        this.ocA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.ghActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.ghR,
                        "|",
                        this.ghG,
                        "|",
                        this.ghB,
                        "|",
                        this.ghA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.rhActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.rhR,
                        "|",
                        this.rhG,
                        "|",
                        this.rhB,
                        "|",
                        this.rhA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.yhActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.yhR,
                        "|",
                        this.yhG,
                        "|",
                        this.yhB,
                        "|",
                        this.yhA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.bhActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.bhR,
                        "|",
                        this.bhG,
                        "|",
                        this.bhB,
                        "|",
                        this.bhA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.ohActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.ohR,
                        "|",
                        this.ohG,
                        "|",
                        this.ohB,
                        "|",
                        this.ohA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.ghlActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.ghlR,
                        "|",
                        this.ghlG,
                        "|",
                        this.ghlB,
                        "|",
                        this.ghlA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.rhlActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.rhlR,
                        "|",
                        this.rhlG,
                        "|",
                        this.rhlB,
                        "|",
                        this.rhlA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.yhlActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.yhlR,
                        "|",
                        this.yhlG,
                        "|",
                        this.yhlB,
                        "|",
                        this.yhlA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.bhlActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.bhlR,
                        "|",
                        this.bhlG,
                        "|",
                        this.bhlB,
                        "|",
                        this.bhlA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.ohlActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.ohlR,
                        "|",
                        this.ohlG,
                        "|",
                        this.ohlB,
                        "|",
                        this.ohlA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.bgSaveProfile)
                {
                    streamWriter.WriteLine(this.currBgFile);
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.hwSaveProfile)
                {
                    streamWriter.WriteLine(this.currHwFile);
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.mnActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.mnR,
                        "|",
                        this.mnG,
                        "|",
                        this.mnB,
                        "|",
                        this.mnA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.mcActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.mcR,
                        "|",
                        this.mcG,
                        "|",
                        this.mcB,
                        "|",
                        this.mcA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.songpbActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.songpbR,
                        "|",
                        this.songpbG,
                        "|",
                        this.songpbB,
                        "|",
                        this.songpbA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.starpbActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.starpbR,
                        "|",
                        this.starpbG,
                        "|",
                        this.starpbB,
                        "|",
                        this.starpbA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.osActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.osR,
                        "|",
                        this.osG,
                        "|",
                        this.osB,
                        "|",
                        this.osA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.isActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.isR,
                        "|",
                        this.isG,
                        "|",
                        this.isB,
                        "|",
                        this.isA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.scTextActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.scTextR,
                        "|",
                        this.scTextG,
                        "|",
                        this.scTextB,
                        "|",
                        this.scTextA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.fcActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.fcR,
                        "|",
                        this.fcG,
                        "|",
                        this.fcB,
                        "|",
                        this.fcA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.onActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.onR,
                        "|",
                        this.onG,
                        "|",
                        this.onB,
                        "|",
                        this.onA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.spGlowActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.spGlowR,
                        "|",
                        this.spGlowG,
                        "|",
                        this.spGlowB,
                        "|",
                        0
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.pgActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.particleGR,
                        "|",
                        this.particleGG,
                        "|",
                        this.particleGB,
                        "|",
                        this.particleGA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.prActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.particleRR,
                        "|",
                        this.particleRG,
                        "|",
                        this.particleRB,
                        "|",
                        this.particleRA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.pyActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.particleYR,
                        "|",
                        this.particleYG,
                        "|",
                        this.particleYB,
                        "|",
                        this.particleYA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.pbActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.particleBR,
                        "|",
                        this.particleBG,
                        "|",
                        this.particleBB,
                        "|",
                        this.particleBA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.poActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.particleOR,
                        "|",
                        this.particleOG,
                        "|",
                        this.particleOB,
                        "|",
                        this.particleOA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }
                if (this.saveMenuTheme)
                {
                    streamWriter.WriteLine(this.currentThemeIndex);
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.particleSettingsActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.particleSize,
                        "|",
                        this.particleSpeed,
                        "|",
                        this.particleGravity,
                        "|",
                        this.particleMax
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.scoreFontActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.scoreFontR,
                        "|",
                        this.scoreFontG,
                        "|",
                        this.scoreFontB
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.comboFontActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.comboFontR,
                        "|",
                        this.comboFontG,
                        "|",
                        this.comboFontB
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.strikeGActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.strikelGR,
                        "|",
                        this.strikelGG,
                        "|",
                        this.strikelGB,
                        "|",
                        this.strikelGA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.strikeRActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.strikelRR,
                        "|",
                        this.strikelRG,
                        "|",
                        this.strikelRB,
                        "|",
                        this.strikelRA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.strikeYActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.strikelYR,
                        "|",
                        this.strikelYG,
                        "|",
                        this.strikelYB,
                        "|",
                        this.strikelYA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.strikeBActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.strikelBR,
                        "|",
                        this.strikelBG,
                        "|",
                        this.strikelBB,
                        "|",
                        this.strikelBA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.strikeOActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.strikelOR,
                        "|",
                        this.strikelOG,
                        "|",
                        this.strikelOB,
                        "|",
                        this.strikelOA
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }

                if (this.sActive)
                {
                    streamWriter.WriteLine(string.Concat(new object[]
                    {
                        this.sR,
                        "|",
                        this.sG,
                        "|",
                        this.sB,
                    }));
                }
                else
                {
                    streamWriter.WriteLine("empty");
                }
            }

            using (StreamWriter streamWriter2 = new StreamWriter(Environment.CurrentDirectory + "/Tweaks/Config/GC-Default.txt"))
            {
                if (this.saveAsDefault)
                {
                    streamWriter2.WriteLine(this.profileName);
                }
                else
                {
                    streamWriter2.WriteLine("empty");
                }
            }
        }

        private void Start()
        {
            Debug.Delete();

            UnityEngine.Object.DontDestroyOnLoad(this);
            new DirectoryInfo(Environment.CurrentDirectory + "/Tweaks/Config/").Create();
            new DirectoryInfo(Environment.CurrentDirectory + "/Tweaks/Config/GC Profiles/").Create();
            SceneManager.activeSceneChanged += new UnityAction<Scene, Scene>(this.GatherObjs);

            this.greenEx = new GUIStyle();
            this.redEx = new GUIStyle();
            this.yellowEx = new GUIStyle();
            this.blueEx = new GUIStyle();
            this.orangeEx = new GUIStyle();
            this.spEx = new GUIStyle();
            this.gfEx = new GUIStyle();
            this.rfEx = new GUIStyle();
            this.yfEx = new GUIStyle();
            this.bfEx = new GUIStyle();
            this.ofEx = new GUIStyle();
            this.glEx = new GUIStyle();
            this.rlEx = new GUIStyle();
            this.ylEx = new GUIStyle();
            this.blEx = new GUIStyle();
            this.olEx = new GUIStyle();
            this.spblEx = new GUIStyle();
            this.spbuEx = new GUIStyle();
            this.spbLtEx = new GUIStyle();
            this.centerText = new GUIStyle();
            this.gcEx = new GUIStyle();
            this.rcEx = new GUIStyle();
            this.ycEx = new GUIStyle();
            this.bcEx = new GUIStyle();
            this.ocEx = new GUIStyle();
            this.ghEx = new GUIStyle();
            this.rhEx = new GUIStyle();
            this.yhEx = new GUIStyle();
            this.bhEx = new GUIStyle();
            this.ohEx = new GUIStyle();
            this.ghlEx = new GUIStyle();
            this.rhlEx = new GUIStyle();
            this.yhlEx = new GUIStyle();
            this.bhlEx = new GUIStyle();
            this.ohlEx = new GUIStyle();
            this.mnEx = new GUIStyle();
            this.mcEx = new GUIStyle();
            this.songpbEx = new GUIStyle();
            this.starpbEx = new GUIStyle();
            this.osEx = new GUIStyle();
            this.isEx = new GUIStyle();
            this.scTextEx = new GUIStyle();
            this.onEx = new GUIStyle();
            this.spGlowEx = new GUIStyle();
            this.pgEx = new GUIStyle();
            this.prEx = new GUIStyle();
            this.pyEx = new GUIStyle();
            this.pbEx = new GUIStyle();
            this.poEx = new GUIStyle();
            this.scoreFontEx = new GUIStyle();
            this.comboFontEx = new GUIStyle();
            this.strikelREx = new GUIStyle();
            this.strikelGEx = new GUIStyle();
            this.strikelYEx = new GUIStyle();
            this.strikelBEx = new GUIStyle();
            this.strikelOEx = new GUIStyle();
            this.fcEx = new GUIStyle();
            this.sEx = new GUIStyle();

            this.fpsCount.normal.textColor = Color.white;
            this.fpsCount.fontSize = 24;
            this.fpsCount.fontStyle = FontStyle.Bold;
            this.centerText.alignment = TextAnchor.UpperCenter;
            this.centerText.normal.textColor = Color.white;

            this.asm = Assembly.Load("Assembly-CSharp");

            if (File.Exists(Environment.CurrentDirectory + "/Tweaks/Config/GC-Default.txt"))
            {
                string[] array = File.ReadAllLines(Environment.CurrentDirectory + "/Tweaks/Config/GC-Default.txt");
                if (array[0] != "empty" && array[0] != string.Empty)
                {
                    this.profileName = array[0];
                    this.LoadConfig(this.profileName + ".cfg");
                    this.saveAsDefault = true;
                }
            }

            this.grayTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(45, 45, 45, 255));
            this.buttonTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(0, 152, 204, 255));
            this.buttonPressTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(0, 122, 163, 255));
            this.buttonHoverTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(0, 176, 235, 255));
            this.greenScreen = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(0, 255, 0, 255));
            this.clearScreen = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(0, 0, 0, 0));

            KeyBindConfig.LoadFromConfig();
            new Thread(new ThreadStart(this.RainbowThread)).Start();

            /*Debug.Log("Object list:");
            foreach (UnityEngine.Object obj in GameObject.FindObjectsOfType(typeof(UnityEngine.Object)))
            {
                Debug.Log(string.Format("Name: {0}, Type: {1}", obj.name, obj.GetType()));

                if (obj is GameObject)
                {
                    GameObject gameObj = (GameObject)obj;

                    ListComponents(gameObj);
                }
            }*/
        }

        private void ListComponents(UnityEngine.Object gameObject, int tabs = 1)
        {
            string tabString = "";
            StringBuilder sb = new StringBuilder(tabString);
            for (int i = 1; i < tabs; i++)
            {
                sb.Append("\t");
            }
            tabString = sb.ToString();

            Component[] components = new Component[0];

            if (gameObject is GameObject)
            {
                GameObject go = (GameObject)gameObject;
                components = go.GetComponents(typeof(Component));
            }
            else if (gameObject is Component)
            {
                Component co = (Component)gameObject;
                components = co.GetComponents(typeof(Component));
            }

            if (components.Length > 0)
            {
                Debug.Log(tabString + "Components:");
                foreach (Component component in components)
                {
                    Debug.Log(string.Format("\t{0}Name: {1}, Type: {2}, Tag: {3}", tabString, component.name, component.GetType(), component.tag));
                }
            }
        }

        private void RainbowThread()
        {
            for (; ; )
            {
                if (GameStatus.gameState == GameStatus.GameState.PlayingSong)
                {
                    Color color = Helpers.RGBtoUnity(255, 25, 25, 255);
                    Color color2 = Helpers.RGBtoUnity(255, 162, 0, 255);
                    Color color3 = Helpers.RGBtoUnity(246, 255, 0, 255);
                    Color color4 = Helpers.RGBtoUnity(19, 240, 7, 255);
                    Color color5 = Helpers.RGBtoUnity(0, 195, 255, 255);
                    Color color6 = Helpers.RGBtoUnity(102, 0, 255, 255);
                    Color color7 = Helpers.RGBtoUnity(242, 0, 255, 255);
                    Color[] array = new Color[]
                    {
                        color,
                        color2,
                        color3,
                        color4,
                        color5,
                        color6,
                        color7
                    };

                    if (this.greenRB)
                    {
                        Color color8 = array[Random.Range(0, 6)];
                        this.SetNoteColor(0, color8);
                        this.SetSustainColor(0, color8);
                        this.greenActive = false;
                    }

                    if (this.redRB)
                    {
                        Color color9 = array[Random.Range(0, 6)];
                        this.SetNoteColor(1, color9);
                        this.SetSustainColor(1, color9);
                        this.redActive = false;
                    }

                    if (this.yellowRB)
                    {
                        Color color10 = array[Random.Range(0, 6)];
                        this.SetNoteColor(2, color10);
                        this.SetSustainColor(2, color10);
                        this.yellowActive = false;
                    }

                    if (this.blueRB)
                    {
                        Color color11 = array[Random.Range(0, 6)];
                        this.SetNoteColor(3, color11);
                        this.SetSustainColor(3, color11);
                        this.blueActive = false;
                    }

                    if (this.orangeRB)
                    {
                        Color color12 = array[Random.Range(0, 6)];
                        this.SetNoteColor(4, color12);
                        this.SetSustainColor(4, color12);
                        this.orangeActive = false;
                    }

                    if (this.spRB)
                    {
                        Color starPowerNotes = array[Random.Range(0, 6)];
                        this.SetStarPowerNotes(starPowerNotes);
                        this.spActive = false;
                    }
                }
                Thread.Sleep(65);
            }
        }

        private void LoadDefaultValues()
        {
            if (!this.particleSettingsActive)
            {
                ParticleSystem.MainModule main = this.particles[10].main;
                this.particleSize = main.startSize.constant;
                this.particleSpeed = main.startSpeed.constant;
                this.particleGravity = main.gravityModifier.constant;
                this.particleMax = main.maxParticles;
            }

            if (!this.pgActive)
            {
                ParticleSystem.MainModule main = this.particles[14].main;
                this.particleGR = main.startColor.color.r;
                this.particleGG = main.startColor.color.g;
                this.particleGB = main.startColor.color.b;
                this.particleGA = main.startColor.color.a;
            }

            if (!this.prActive)
            {
                ParticleSystem.MainModule main = this.particles[13].main;
                this.particleRR = main.startColor.color.r;
                this.particleRG = main.startColor.color.g;
                this.particleRB = main.startColor.color.b;
                this.particleRA = main.startColor.color.a;
            }

            if (!this.pyActive)
            {
                ParticleSystem.MainModule main = this.particles[12].main;
                this.particleYR = main.startColor.color.r;
                this.particleYG = main.startColor.color.g;
                this.particleYB = main.startColor.color.b;
                this.particleYA = main.startColor.color.a;
            }

            if (!this.pbActive)
            {
                ParticleSystem.MainModule main = this.particles[11].main;
                this.particleBR = main.startColor.color.r;
                this.particleBG = main.startColor.color.g;
                this.particleBB = main.startColor.color.b;
                this.particleBA = main.startColor.color.a;
            }

            if (!this.poActive)
            {
                ParticleSystem.MainModule main = this.particles[14].main;
                this.particleOR = main.startColor.color.r;
                this.particleOG = main.startColor.color.g;
                this.particleOB = main.startColor.color.b;
                this.particleOA = main.startColor.color.a;
            }
        }

        private void LoadNotes()
        {
            try
            {
                if (this.spActive)
                {
                    this.SetStarPowerNotes(this.spPrint);
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Error occurred setting star power notes: " + ex.Message);
            }

            try
            {
                if (this.greenActive)
                {
                    this.SetNoteColor(0, this.greenPrint);
                    this.SetSustainColor(0, this.greenPrint);
                    this.greenRB = false;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Error occurred setting green notes: " + ex.Message);
            }

            try
            {
                if (this.redActive)
                {
                    this.SetNoteColor(1, this.redPrint);
                    this.SetSustainColor(1, this.redPrint);
                    this.redRB = false;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Error occurred setting red notes: " + ex.Message);
            }

            try
            {
                if (this.yellowActive)
                {
                    this.SetNoteColor(2, this.yellowPrint);
                    this.SetSustainColor(2, this.yellowPrint);
                    this.yellowRB = false;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Error occurred setting yellow notes: " + ex.Message);
            }

            try
            {
                if (this.blueActive)
                {
                    this.SetNoteColor(3, this.bluePrint);
                    this.SetSustainColor(3, this.bluePrint);
                    this.blueRB = false;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Error occurred setting blue notes: " + ex.Message);
            }

            try
            {
                if (this.orangeActive)
                {
                    this.SetNoteColor(4, this.orangePrint);
                    this.SetSustainColor(4, this.orangePrint);
                    this.orangeRB = false;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Error occurred setting orange notes: " + ex.Message);
            }

            try
            {
                if (this.onActive)
                {
                    this.SetOpenNotesColor(this.onPrint);
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Error occurred setting open notes: " + ex.Message);
            }
        }

        private void GatherObjs(Scene scene, Scene next)
        {
            if (scene.buildIndex != 3)
            {
                reloaded = false;
            }

            if (GameObject.Find("GHL Player 1(Clone)") == null)
            {
                this.ghlEnabled = false;
            }
            else
            {
                this.ghlEnabled = true;
            }

            if (GameStatus.gameState == GameStatus.GameState.PlayingSong)
            {
                Debug.Log("Preparing to gather objects...");
                int num = 0;

                try
                {
                    this.ClearArrays();
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred clearing arrays: " + ex.Message);
                }

                try
                {
                    if (!this.ghlEnabled)
                    {
                        this.InitNoteColors();
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred initializing note colors: " + ex.Message);
                }

                /*Debug.Log("Object list in " + next.name + ":");
                foreach (GameObject gameObj in next.GetRootGameObjects())
                {
                    Debug.Log(string.Format("Name: {0}, Type: {1}", gameObj.name, gameObj.GetType()));
                    ListComponents(gameObj);
                }*/

                Debug.Log("Begin loading main objects...");
                foreach (GameObject gameObject in UnityEngine.Object.FindObjectsOfType<GameObject>())
                {
                    try
                    {
                        switch (gameObject.name.ToLower())
                        {
                            case "flames_green":
                                Debug.Log("Green flame loaded");
                                this.flames[0] = gameObject;
                                this.flameRenders[0] = gameObject.GetComponent<SpriteRenderer>();
                                break;
                            case "flames_red":
                                Debug.Log("Red flame loaded");
                                this.flames[1] = gameObject;
                                this.flameRenders[1] = gameObject.GetComponent<SpriteRenderer>();
                                break;
                            case "flames_yellow":
                                Debug.Log("Yellow flame loaded");
                                this.flames[2] = gameObject;
                                this.flameRenders[2] = gameObject.GetComponent<SpriteRenderer>();
                                break;
                            case "flames_blue":
                                Debug.Log("Blue flame loaded");
                                this.flames[3] = gameObject;
                                this.flameRenders[3] = gameObject.GetComponent<SpriteRenderer>();
                                break;
                            case "flames_orange":
                                Debug.Log("Orange flame loaded");
                                this.flames[4] = gameObject;
                                this.flameRenders[4] = gameObject.GetComponent<SpriteRenderer>();
                                break;
                            case "open_hitflames_oval":
                                Debug.Log("open oval flame loaded");
                                this.onOval = gameObject.GetComponent<SpriteRenderer>();
                                break;
                            case "open_hitflames":
                                Debug.Log("Open flame loaded");
                                this.onOvalFlames = gameObject.GetComponent<SpriteRenderer>();
                                break;
                            case "lightning_green":
                                Debug.Log("Green lightning loaded");
                                this.lightning[0] = gameObject;
                                this.lightningRenders[0] = gameObject.GetComponent<SpriteRenderer>();
                                break;
                            case "lightning_red":
                                Debug.Log("Red lightning loaded");
                                this.lightning[1] = gameObject;
                                this.lightningRenders[1] = gameObject.GetComponent<SpriteRenderer>();
                                break;
                            case "lightning_yellow":
                                Debug.Log("Yellow lightning loaded");
                                this.lightning[2] = gameObject;
                                this.lightningRenders[2] = gameObject.GetComponent<SpriteRenderer>();
                                break;
                            case "lightning_blue":
                                Debug.Log("Blue lightning loaded");
                                this.lightning[3] = gameObject;
                                this.lightningRenders[3] = gameObject.GetComponent<SpriteRenderer>();
                                break;
                            case "lightning_orange":
                                Debug.Log("Orange lightning loaded");
                                this.lightning[4] = gameObject;
                                this.lightningRenders[4] = gameObject.GetComponent<SpriteRenderer>();
                                break;
                            case "multiplier":
                                Debug.Log("Multiplier loaded");
                                this.mn = gameObject.GetComponent<SpriteRenderer>();
                                this.multiRender = gameObject.GetComponent<Renderer>();
                                break;
                            case "multiplierglow":
                                Debug.Log("Multiplier glow loaded");
                                this.mg = gameObject.GetComponent<SpriteRenderer>();
                                this.multiGlowRender = gameObject.GetComponent<Renderer>();
                                break;
                            case "combocount":
                                Debug.Log("Combo count loaded");
                                this.cc = gameObject.GetComponent<SpriteRenderer>();
                                this.comboCountRender = gameObject.GetComponent<Renderer>();
                                break;
                            case "progressbar":
                                if (num == 0)
                                {
                                    Debug.Log("Progress bar (top) loaded");
                                    this.pBar1 = gameObject.GetComponent<SpriteRenderer>();
                                    num++;
                                }
                                else if (num == 1)
                                {
                                    Debug.Log("Progress bar (bottom) loaded");
                                    this.pBar2 = gameObject.GetComponent<SpriteRenderer>();
                                }
                                break;
                            case "star_outer":
                                Debug.Log("Outer star loaded");
                                this.outerStar = gameObject.GetComponent<SpriteRenderer>();
                                this.oStarRender = gameObject.GetComponent<Renderer>();
                                break;
                            case "star_inner":
                                Debug.Log("Inner star loaded");
                                this.innerStar = gameObject.GetComponent<SpriteRenderer>();
                                this.iStarRender = gameObject.GetComponent<Renderer>();
                                break;
                            case "starcount":
                                Debug.Log("Star count loaded");
                                this.starCount = gameObject.GetComponent<SpriteRenderer>();
                                break;
                            case "fc_rotate":
                                Debug.Log("FC loaded");
                                this.fcRotate = gameObject.GetComponent<SpriteRenderer>();
                                this.fcRotateRender = gameObject.GetComponent<Renderer>();
                                break;
                            case "fc_bg":
                                Debug.Log("FC background loaded");
                                this.fcBg = gameObject.GetComponent<SpriteRenderer>();
                                this.fcBgRender = gameObject.GetComponent<Renderer>();
                                break;
                            case "backgroundimage":
                                Debug.Log("Background image loaded");
                                this.imageComps[0] = gameObject.GetComponent<Image>();
                                this.renders[0] = null;
                                break;
                            default:
                                //Debug.Log(string.Format("Unknown object: Name: {0}, Tag: {1}", gameObject.name, gameObject.tag));
                                //ListComponents(gameObject);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("Error occurred loading " + gameObject.name.ToLower() + ": " + ex.Message);
                    }
                }

                Debug.Log("Begin loading fonts...");
                try
                {
                    this.scoreFonts[0] = GameObject.Find("Score/1").GetComponent<SpriteRenderer>();
                    this.scoreFonts[1] = GameObject.Find("Score/2").GetComponent<SpriteRenderer>();
                    this.scoreFonts[2] = GameObject.Find("Score/3").GetComponent<SpriteRenderer>();
                    this.scoreFonts[3] = GameObject.Find("Score/4").GetComponent<SpriteRenderer>();
                    this.scoreFonts[4] = GameObject.Find("Score/5").GetComponent<SpriteRenderer>();
                    this.scoreFonts[5] = GameObject.Find("Score/6").GetComponent<SpriteRenderer>();
                    this.scoreFonts[6] = GameObject.Find("Score/7").GetComponent<SpriteRenderer>();
                    this.scoreFonts[7] = GameObject.Find("Score/8").GetComponent<SpriteRenderer>();
                    this.scoreFonts[8] = GameObject.Find("Score/9").GetComponent<SpriteRenderer>();
                    this.scoreFonts[9] = GameObject.Find("Score/comma1").GetComponent<SpriteRenderer>();
                    this.scoreFonts[10] = GameObject.Find("Score/comma2").GetComponent<SpriteRenderer>();

                    this.comboFonts[0] = GameObject.Find("Combo/1").GetComponent<SpriteRenderer>();
                    this.comboFonts[1] = GameObject.Find("Combo/2").GetComponent<SpriteRenderer>();
                    this.comboFonts[2] = GameObject.Find("Combo/3").GetComponent<SpriteRenderer>();
                    this.comboFonts[3] = GameObject.Find("Combo/4").GetComponent<SpriteRenderer>();
                    this.comboFonts[4] = GameObject.Find("Combo/5").GetComponent<SpriteRenderer>();
                    this.comboFonts[5] = GameObject.Find("Combo/6").GetComponent<SpriteRenderer>();
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred loading fonts: " + ex.Message);
                }

                Debug.Log("Begin loading frets...");
                try
                {
                    TweakMain.FretCover fretCover = new TweakMain.FretCover();
                    TweakMain.FretHead fretHead = new TweakMain.FretHead();
                    fretCover.frontCover = GameObject.Find("Fret_Green/cover");
                    fretCover.backCover = GameObject.Find("Fret_Green/cover (2)");
                    fretCover.headCover = GameObject.Find("Fret_Green/head/headcover");
                    fretHead.headLight = GameObject.Find("Fret_Green/head/headlight");
                    fretHead.head = GameObject.Find("Fret_Green/head");

                    TweakMain.FretCover fretCover2 = new TweakMain.FretCover();
                    TweakMain.FretHead fretHead2 = new TweakMain.FretHead();
                    fretCover2.frontCover = GameObject.Find("Fret_Red/cover");
                    fretCover2.backCover = GameObject.Find("Fret_Red/halfcover");
                    fretCover2.headCover = GameObject.Find("Fret_Red/head/headcover");
                    fretHead2.headLight = GameObject.Find("Fret_Red/head/headlight");
                    fretHead2.head = GameObject.Find("Fret_Red/head");

                    TweakMain.FretCover fretCover3 = new TweakMain.FretCover();
                    TweakMain.FretHead fretHead3 = new TweakMain.FretHead();
                    fretCover3.frontCover = GameObject.Find("Fret_Yellow/cover");
                    fretCover3.backCover = GameObject.Find("Fret_Yellow/cover (2)");
                    fretCover3.headCover = GameObject.Find("Fret_Yellow/head/headcover");
                    fretHead3.headLight = GameObject.Find("Fret_Yellow/head/headlight");
                    fretHead3.head = GameObject.Find("Fret_Yellow/head");

                    TweakMain.FretCover fretCover4 = new TweakMain.FretCover();
                    TweakMain.FretHead fretHead4 = new TweakMain.FretHead();
                    fretCover4.frontCover = GameObject.Find("Fret_Blue/cover");
                    fretCover4.backCover = GameObject.Find("Fret_Blue/cover (2)");
                    fretCover4.headCover = GameObject.Find("Fret_Blue/head/headcover");
                    fretHead4.headLight = GameObject.Find("Fret_Blue/head/headlight");
                    fretHead4.head = GameObject.Find("Fret_Blue/head");

                    TweakMain.FretCover fretCover5 = new TweakMain.FretCover();
                    TweakMain.FretHead fretHead5 = new TweakMain.FretHead();
                    fretCover5.frontCover = GameObject.Find("Fret_Orange/cover");
                    fretCover5.backCover = GameObject.Find("Fret_Orange/cover (2)");
                    fretCover5.headCover = GameObject.Find("Fret_Orange/head/headcover");
                    fretHead5.headLight = GameObject.Find("Fret_Orange/head/headlight");
                    fretHead5.head = GameObject.Find("Fret_Orange/head");
                    this.fretCovers[0] = fretCover;
                    this.fretCovers[1] = fretCover2;
                    this.fretCovers[2] = fretCover3;
                    this.fretCovers[3] = fretCover4;
                    this.fretCovers[4] = fretCover5;

                    this.fretHeads[0] = fretHead;
                    this.fretHeads[1] = fretHead2;
                    this.fretHeads[2] = fretHead3;
                    this.fretHeads[3] = fretHead4;
                    this.fretHeads[4] = fretHead5;
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred loading frets: " + ex.Message);
                }
                
                Debug.Log("Begin loading stars...");
                try
                {
                    this.star1 = GameObject.Find("star").GetComponent<SpriteRenderer>();
                    this.star2 = GameObject.Find("star (1)").GetComponent<SpriteRenderer>();
                    this.star3 = GameObject.Find("star (2)").GetComponent<SpriteRenderer>();
                    this.star4 = GameObject.Find("star (3)").GetComponent<SpriteRenderer>();
                    this.star5 = GameObject.Find("star (4)").GetComponent<SpriteRenderer>();
                    this.star6 = GameObject.Find("star (5)").GetComponent<SpriteRenderer>();
                    this.star7 = GameObject.Find("star (6)").GetComponent<SpriteRenderer>();
                }
                catch (Exception ex)
                {
                    if (!isBotEnabled())
                    {
                        Debug.Log("Error occurred loading stars: " + ex.Message);
                    }
                }

                Debug.Log("Begin loading highway sides...");
                try
                {
                    this.sideL = GameObject.Find("sidebar_l").GetComponent<SpriteRenderer>();
                    this.sideR = GameObject.Find("sidebar_r").GetComponent<SpriteRenderer>();
                    this.sideLGlow = GameObject.Find("sidebar_l_glow").GetComponent<SpriteRenderer>();
                    this.sideRGlow = GameObject.Find("sidebar_r_glow").GetComponent<SpriteRenderer>();
                    this.highwayGlow = GameObject.Find("highway_glow").GetComponent<SpriteRenderer>();
                    this.glowL = GameObject.Find("highway_glow (1)").GetComponent<SpriteRenderer>();
                    this.glowR = GameObject.Find("highway_glow (2)").GetComponent<SpriteRenderer>();
                }
                catch (Exception ex)
                {
                    if (!isBotEnabled())
                    {
                        Debug.Log("Error occurred loading highway sides: " + ex.Message);
                    }
                }

                Debug.Log("Begin loading particles...");
                try
                {
                    int num3 = 1;

                    foreach (GameObject gameObject3 in UnityEngine.Object.FindObjectsOfType<GameObject>())
                    {
                        if (gameObject3.GetComponent<ParticleSystem>() != null)
                        {
                            num3++;
                            this.particles.Add(gameObject3.GetComponent<ParticleSystem>());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred loading particles: " + ex.Message);
                }

                Debug.Log("Begin loading strike line...");
                try
                {
                    if (this.spGlowActive)
                    {
                        this.defaultSpColor = Helpers.RGBtoUnity(this.spGlowR, this.spGlowG, this.spGlowB, 255);
                    }

                    foreach (GameObject gameObject4 in UnityEngine.Object.FindObjectsOfType<GameObject>())
                    {
                        if (gameObject4.name.ToLower().Contains("string"))
                        {
                            this.strikeStrings.Add(gameObject4.GetComponent<SpriteRenderer>());
                        }
                    }

                    this.LoadNotes();
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred loading strike line: " + ex.Message);
                }

                Debug.Log("Begin loading font colours...");
                try
                {
                    if (this.scoreFontActive)
                    {
                        SpriteRenderer[] array2 = this.scoreFonts;
                        for (int i = 0; i < array2.Length; i++)
                        {
                            array2[i].color = this.scoreFontPrint;
                        }
                    }

                    if (this.comboFontActive)
                    {
                        SpriteRenderer[] array2 = this.comboFonts;
                        for (int i = 0; i < array2.Length; i++)
                        {
                            array2[i].color = this.comboFontPrint;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred loading font colours: " + ex.Message);
                }
            }

            Debug.Log("Begin loading background/renders...");
            try
            {
                if (this.bgPersist && this.imageComps[0] != null)
                {
                    this.imageComps[0].gameObject.SetActive(true);
                    this.imageComps[0].enabled = true;
                    this.imageComps[0].sprite = this.bgSprite;
                }

                if (this.hwPersist && this.renders[0] != null)
                {
                    this.renders[0].gameObject.SetActive(true);
                    this.renders[0].material.mainTexture = this.hwTex;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Error occurred loading background/renders: " + ex.Message);
            }

            Debug.Log("Finished gathering objects");
        }

        private void ClearArrays()
        {
            this.fretCovers = new TweakMain.FretCover[5];
            this.fretHeads = new TweakMain.FretHead[5];
            this.flames = new GameObject[5];
            this.flameRenders = new SpriteRenderer[5];
            this.lightning = new GameObject[5];
            this.lightningRenders = new SpriteRenderer[5];
            this.renders = new Renderer[2];
            this.imageComps = new Image[1];
            this.strikeStrings.Clear();
            this.spBars = new SpriteRenderer[7];
            this.scoreFonts = new SpriteRenderer[11];
            this.comboFonts = new SpriteRenderer[6];
            this.particles.Clear();
        }

        private void LateUpdate()
        {
            if (this.ghlEnabled)
            {
                return;
            }

            if (GameStatus.gameState == GameStatus.GameState.PlayingSong && !this.isPracticeEnabled())
            {
                if (this.spGlowActive)
                {
                    this.defaultSpColor = Helpers.RGBtoUnity(this.spGlowR, this.spGlowG, this.spGlowB, 255);
                    this.star1.color = (this.star2.color = (this.star3.color = (this.star4.color = (this.star5.color = (this.star6.color = (this.star7.color = Helpers.RGBtoUnity(this.spGlowR, this.spGlowG, this.spGlowB, Helpers.UnityToRGB(this.star7.color.a))))))));
                    this.glowL.color = (this.glowR.color = Helpers.RGBtoUnity(this.spGlowR, this.spGlowG, this.spGlowB, Helpers.UnityToRGB(this.glowR.color.a)));
                    this.sideLGlow.color = Helpers.RGBtoUnity(this.spGlowR, this.spGlowG, this.spGlowB, Helpers.UnityToRGB(this.sideLGlow.color.a));
                    this.sideRGlow.color = Helpers.RGBtoUnity(this.spGlowR, this.spGlowG, this.spGlowB, Helpers.UnityToRGB(this.sideRGlow.color.a));
                    this.highwayGlow.color = Helpers.RGBtoUnity(this.spGlowR, this.spGlowG, this.spGlowB, Helpers.UnityToRGB(this.highwayGlow.color.a));
                }

                if (this.mnActive)
                {
                    if (this.isStarPower)
                    {
                        this.multiRender.material.SetColor("_Color", Color.white);
                        this.multiGlowRender.material.SetColor("_Color", Color.white);
                        this.mn.color = this.defaultSpColor;
                        this.mg.color = this.defaultSpColor;
                    }
                    else
                    {
                        this.multiRender.material.SetColor("_Color", this.mnPrint);
                        this.multiGlowRender.material.SetColor("_Color", this.mnPrint);
                        this.mn.color = this.mnPrint;
                        this.mg.color = this.mnPrint;
                    }
                }

                if (this.mcActive)
                {
                    if (this.isStarPower)
                    {
                        this.comboCountRender.material.SetColor("_Color", Color.white);
                        this.cc.color = this.defaultSpColor;
                        return;
                    }

                    this.comboCountRender.material.SetColor("_Color", this.mcPrint);
                    this.cc.color = this.mcPrint;
                }
            }
        }

        private void Update()
        {
            if (!this.isSettingKey)
            {
                this.keyButtonText = "Change Open Keybind";
                if (Input.GetKeyDown(KeyBindConfig.menuKey))
                {
                    this.isInEditMode = !this.isInEditMode;
                    this.menuLoaded = !this.menuLoaded;
                    Cursor.visible = this.menuLoaded;
                    this.SetTheme(this.currentThemeIndex);
                }
            }
            else
            {
                this.keyButtonText = "Press any key...";
            }

            if (this.ghlEnabled)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                this.windowRect = new Rect(Screen.width / 3f, Screen.height / 3f, 300, 400);
            }

            #region Colors
            this.greenPrint = Helpers.RGBtoUnity(this.greenR, this.greenG, this.greenB, this.greenA);
            this.redPrint = Helpers.RGBtoUnity(this.redR, this.redG, this.redB, this.redA);
            this.yellowPrint = Helpers.RGBtoUnity(this.yellowR, this.yellowG, this.yellowB, this.yellowA);
            this.bluePrint = Helpers.RGBtoUnity(this.blueR, this.blueG, this.blueB, this.blueA);
            this.orangePrint = Helpers.RGBtoUnity(this.orangeR, this.orangeG, this.orangeB, this.orangeA);

            this.spPrint = Helpers.RGBtoUnity(this.spR, this.spG, this.spB, this.spA);

            this.gfPrint = Helpers.RGBtoUnity(this.gfR, this.gfG, this.gfB, this.gfA);
            this.rfPrint = Helpers.RGBtoUnity(this.rfR, this.rfG, this.rfB, this.rfA);
            this.yfPrint = Helpers.RGBtoUnity(this.yfR, this.yfG, this.yfB, this.yfA);
            this.bfPrint = Helpers.RGBtoUnity(this.bfR, this.bfG, this.bfB, this.bfA);
            this.ofPrint = Helpers.RGBtoUnity(this.ofR, this.ofG, this.ofB, this.ofA);

            this.glPrint = Helpers.RGBtoUnity(this.glR, this.glG, this.glB, this.glA);
            this.rlPrint = Helpers.RGBtoUnity(this.rlR, this.rlG, this.rlB, this.rlA);
            this.ylPrint = Helpers.RGBtoUnity(this.ylR, this.ylG, this.ylB, this.ylA);
            this.blPrint = Helpers.RGBtoUnity(this.blR, this.blG, this.blB, this.blA);
            this.olPrint = Helpers.RGBtoUnity(this.olR, this.olG, this.olB, this.olA);

            this.spblPrint = Helpers.RGBtoUnity(this.spblR, this.spblG, this.spblB, this.spblA);
            this.spbuPrint = Helpers.RGBtoUnity(this.spbuR, this.spbuG, this.spbuB, this.spbuA);
            this.spbLtPrint = Helpers.RGBtoUnity(this.spbLtR, this.spbLtG, this.spbLtB, this.spbLtA);

            this.gcPrint = Helpers.RGBtoUnity(this.gcR, this.gcG, this.gcB, this.gcA);
            this.rcPrint = Helpers.RGBtoUnity(this.rcR, this.rcG, this.rcB, this.rcA);
            this.ycPrint = Helpers.RGBtoUnity(this.ycR, this.ycG, this.ycB, this.ycA);
            this.bcPrint = Helpers.RGBtoUnity(this.bcR, this.bcG, this.bcB, this.bcA);
            this.ocPrint = Helpers.RGBtoUnity(this.ocR, this.ocG, this.ocB, this.ocA);

            this.ghPrint = Helpers.RGBtoUnity(this.ghR, this.ghG, this.ghB, this.ghA);
            this.rhPrint = Helpers.RGBtoUnity(this.rhR, this.rhG, this.rhB, this.rhA);
            this.yhPrint = Helpers.RGBtoUnity(this.yhR, this.yhG, this.yhB, this.yhA);
            this.bhPrint = Helpers.RGBtoUnity(this.bhR, this.bhG, this.bhB, this.bhA);
            this.ohPrint = Helpers.RGBtoUnity(this.ohR, this.ohG, this.ohB, this.ohA);

            this.ghlPrint = Helpers.RGBtoUnity(this.ghlR, this.ghlG, this.ghlB, this.ghlA);
            this.rhlPrint = Helpers.RGBtoUnity(this.rhlR, this.rhlG, this.rhlB, this.rhlA);
            this.yhlPrint = Helpers.RGBtoUnity(this.yhlR, this.yhlG, this.yhlB, this.yhlA);
            this.bhlPrint = Helpers.RGBtoUnity(this.bhlR, this.bhlG, this.bhlB, this.bhlA);
            this.ohlPrint = Helpers.RGBtoUnity(this.ohlR, this.ohlG, this.ohlB, this.ohlA);

            this.mnPrint = Helpers.RGBtoUnity(this.mnR, this.mnG, this.mnB, this.mnA);
            this.mcPrint = Helpers.RGBtoUnity(this.mcR, this.mcG, this.mcB, this.mcA);

            this.songpbPrint = Helpers.RGBtoUnity(this.songpbR, this.songpbG, this.songpbB, this.songpbA);
            this.starpbPrint = Helpers.RGBtoUnity(this.starpbR, this.starpbG, this.starpbB, this.starpbA);

            this.osPrint = Helpers.RGBtoUnity(this.osR, this.osG, this.osB, this.osA);
            this.isPrint = Helpers.RGBtoUnity(this.isR, this.isG, this.isB, this.isA);

            this.scTextPrint = Helpers.RGBtoUnity(this.scTextR, this.scTextG, this.scTextB, this.scTextA);
            this.fcPrint = Helpers.RGBtoUnity(this.fcR, this.fcG, this.fcB, this.fcA);
            this.onPrint = Helpers.RGBtoUnity(this.onR, this.onG, this.onB, this.onA);
            this.spGlowPrint = Helpers.RGBtoUnity(this.spGlowR, this.spGlowG, this.spGlowB, 255);

            this.pgPrint = Helpers.RGBtoUnity(this.particleGR, this.particleGG, this.particleGB, this.particleGA);
            this.prPrint = Helpers.RGBtoUnity(this.particleRR, this.particleRG, this.particleRB, this.particleRA);
            this.pyPrint = Helpers.RGBtoUnity(this.particleYR, this.particleYG, this.particleYB, this.particleYA);
            this.pbPrint = Helpers.RGBtoUnity(this.particleBR, this.particleBG, this.particleBB, this.particleBA);
            this.poPrint = Helpers.RGBtoUnity(this.particleOR, this.particleOG, this.particleOB, this.particleOA);

            this.scoreFontPrint = Helpers.RGBtoUnity(this.scoreFontR, this.scoreFontG, this.scoreFontB, 255);
            this.comboFontPrint = Helpers.RGBtoUnity(this.comboFontR, this.comboFontG, this.comboFontB, 255);

            this.strikelGPrint = Helpers.RGBtoUnity(this.strikelGR, this.strikelGG, this.strikelGB, this.strikelGA);
            this.strikelRPrint = Helpers.RGBtoUnity(this.strikelRR, this.strikelRG, this.strikelRB, this.strikelRA);
            this.strikelYPrint = Helpers.RGBtoUnity(this.strikelYR, this.strikelYG, this.strikelYB, this.strikelYA);
            this.strikelBPrint = Helpers.RGBtoUnity(this.strikelBR, this.strikelBG, this.strikelBB, this.strikelBA);
            this.strikelOPrint = Helpers.RGBtoUnity(this.strikelOR, this.strikelOG, this.strikelOB, this.strikelOA);

            this.sPrint = Helpers.RGBtoUnity(this.sR, this.sG, this.sB, 255);

            if (this.isInEditMode)
            {
                this.greenEx.normal.textColor = this.greenPrint;
                this.redEx.normal.textColor = this.redPrint;
                this.yellowEx.normal.textColor = this.yellowPrint;
                this.blueEx.normal.textColor = this.bluePrint;
                this.orangeEx.normal.textColor = this.orangePrint;

                this.spEx.normal.textColor = this.spPrint;

                this.gfEx.normal.textColor = this.gfPrint;
                this.rfEx.normal.textColor = this.rfPrint;
                this.yfEx.normal.textColor = this.yfPrint;
                this.bfEx.normal.textColor = this.bfPrint;
                this.ofEx.normal.textColor = this.ofPrint;

                this.glEx.normal.textColor = this.glPrint;
                this.rlEx.normal.textColor = this.rlPrint;
                this.ylEx.normal.textColor = this.ylPrint;
                this.blEx.normal.textColor = this.blPrint;
                this.olEx.normal.textColor = this.olPrint;

                this.spblEx.normal.textColor = this.spblPrint;
                this.spbuEx.normal.textColor = this.spbuPrint;
                this.spbLtEx.normal.textColor = this.spbLtPrint;

                this.gcEx.normal.textColor = this.gcPrint;
                this.rcEx.normal.textColor = this.rcPrint;
                this.ycEx.normal.textColor = this.ycPrint;
                this.bcEx.normal.textColor = this.bcPrint;
                this.ocEx.normal.textColor = this.ocPrint;

                this.ghEx.normal.textColor = this.ghPrint;
                this.rhEx.normal.textColor = this.rhPrint;
                this.yhEx.normal.textColor = this.yhPrint;
                this.bhEx.normal.textColor = this.bhPrint;
                this.ohEx.normal.textColor = this.ohPrint;

                this.ghlEx.normal.textColor = this.ghlPrint;
                this.rhlEx.normal.textColor = this.rhlPrint;
                this.yhlEx.normal.textColor = this.yhlPrint;
                this.bhlEx.normal.textColor = this.bhlPrint;
                this.ohlEx.normal.textColor = this.ohlPrint;

                this.mnEx.normal.textColor = this.mnPrint;
                this.mcEx.normal.textColor = this.mcPrint;

                this.songpbEx.normal.textColor = this.songpbPrint;
                this.starpbEx.normal.textColor = this.starpbPrint;

                this.osEx.normal.textColor = this.osPrint;
                this.isEx.normal.textColor = this.isPrint;
                this.scTextEx.normal.textColor = this.scTextPrint;
                this.fcEx.normal.textColor = this.fcPrint;
                this.onEx.normal.textColor = this.onPrint;
                this.spGlowEx.normal.textColor = this.spGlowPrint;

                this.pgEx.normal.textColor = this.pgPrint;
                this.prEx.normal.textColor = this.prPrint;
                this.pyEx.normal.textColor = this.pyPrint;
                this.pbEx.normal.textColor = this.pbPrint;
                this.poEx.normal.textColor = this.poPrint;

                this.scoreFontEx.normal.textColor = this.scoreFontPrint;
                this.comboFontEx.normal.textColor = this.comboFontPrint;

                this.strikelGEx.normal.textColor = this.strikelGPrint;
                this.strikelREx.normal.textColor = this.strikelRPrint;
                this.strikelYEx.normal.textColor = this.strikelYPrint;
                this.strikelBEx.normal.textColor = this.strikelBPrint;
                this.strikelOEx.normal.textColor = this.strikelOPrint;

                this.sEx.normal.textColor = this.sPrint;

                this.LoadNotes();

                if (this.scoreFontActive)
                {
                    SpriteRenderer[] array = this.scoreFonts;
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i].color = this.scoreFontPrint;
                    }
                }

                if (this.comboFontActive)
                {
                    SpriteRenderer[] array = this.comboFonts;
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i].color = this.comboFontPrint;
                    }
                }
            }
            #endregion

            if (GameStatus.gameState == GameStatus.GameState.PlayingSong)
            {
                if (!reloaded)
                {
                    GatherObjs(SceneManager.GetSceneByBuildIndex(3), SceneManager.GetActiveScene());
                    reloaded = true;
                }

                try
                {
                    if (this.sActive)
                    {
                        this.sideL.color = this.sideR.color = sPrint;                        
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating highway sides: " + ex.Message);
                }

                try
                {
                    if (this.spBars[0] == null && !this.isPracticeEnabled())
                    {
                        this.spBars[0] = GameObject.Find("SPBar/lowerbar").GetComponent<SpriteRenderer>();
                        this.spBars[1] = GameObject.Find("SPBar/upperbar").GetComponent<SpriteRenderer>();
                        this.spBars[2] = GameObject.Find("SPBar/Electricity").GetComponent<SpriteRenderer>();
                        this.spBars[3] = GameObject.Find("SPBar/FillArrow").GetComponent<SpriteRenderer>();
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating SP bar: " + ex.Message);
                }

                try
                {
                    if (this.isStarPower && !this.gfActive)
                    {
                        this.flameRenders[0].color = this.defaultSpColor;
                    }

                    if (this.gfActive)
                    {
                        if (this.isStarPower)
                        {
                            this.flameRenders[0].color = this.defaultSpColor;
                            this.gfRB = false;
                        }
                        else
                        {
                            this.flameRenders[0].color = this.gfPrint;
                            this.gfRB = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating green flames: " + ex.Message);
                }

                try
                {
                    if (this.isStarPower && !this.rfActive)
                    {
                        this.flameRenders[1].color = this.defaultSpColor;
                    }

                    if (this.rfActive)
                    {
                        if (this.isStarPower)
                        {
                            this.flameRenders[1].color = this.defaultSpColor;
                            this.rfRB = false;
                        }
                        else
                        {
                            this.flameRenders[1].color = this.rfPrint;
                            this.rfRB = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating red flames: " + ex.Message);
                }

                try
                {
                    if (this.isStarPower && !this.yfActive)
                    {
                        this.flameRenders[2].color = this.defaultSpColor;
                    }

                    if (this.yfActive)
                    {
                        if (this.isStarPower)
                        {
                            this.flameRenders[2].color = this.defaultSpColor;
                            this.yfRB = false;
                        }
                        else
                        {
                            this.flameRenders[2].color = this.yfPrint;
                            this.yfRB = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating yellow flames: " + ex.Message);
                }

                try
                {
                    if (this.isStarPower && !this.bfActive)
                    {
                        this.flameRenders[3].color = this.defaultSpColor;
                    }

                    if (this.bfActive)
                    {
                        if (this.isStarPower)
                        {
                            this.flameRenders[3].color = this.defaultSpColor;
                            this.bfRB = false;
                        }
                        else
                        {
                            this.flameRenders[3].color = this.bfPrint;
                            this.bfRB = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating blue flames: " + ex.Message);
                }

                try
                {
                    if (this.isStarPower && !this.ofActive)
                    {
                        this.flameRenders[4].color = this.defaultSpColor;
                    }

                    if (this.ofActive)
                    {
                        if (this.isStarPower)
                        {
                            this.flameRenders[4].color = this.defaultSpColor;
                            this.ofRB = false;
                        }
                        else
                        {
                            this.flameRenders[4].color = this.ofPrint;
                            this.ofRB = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating orange flames: " + ex.Message);
                }

                try
                {
                    if (!this.isPracticeEnabled())
                    {
                        if (this.glActive)
                        {
                            this.lightningRenders[0].color = this.glPrint;
                            this.glRB = false;
                        }

                        if (this.rlActive)
                        {
                            this.lightningRenders[1].color = this.rlPrint;
                            this.rlRB = false;
                        }

                        if (this.ylActive)
                        {
                            this.lightningRenders[2].color = this.ylPrint;
                            this.ylRB = false;
                        }

                        if (this.blActive)
                        {
                            this.lightningRenders[3].color = this.blPrint;
                            this.blRB = false;
                        }

                        if (this.olActive)
                        {
                            this.lightningRenders[4].color = this.olPrint;
                            this.olRB = false;
                        }

                        if (this.spblActive && this.spBars[0] != null)
                        {
                            this.spBars[0].color = this.spblPrint;
                        }

                        if (this.spbuActive && this.spBars[1] != null)
                        {
                            this.spBars[1].color = this.spbuPrint;
                        }

                        if (this.spbLtActive && this.spBars[2] != null)
                        {
                            this.spBars[2].color = this.spbLtPrint;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating lightning: " + ex.Message);
                }

                try
                {
                    if (this.gcActive)
                    {
                        this.fretCovers[0].backCoverRenderer.color = this.gcPrint;
                        this.fretCovers[0].frontCoverRenderer.color = this.gcPrint;
                        this.fretCovers[0].headCoverRenderer.color = this.gcPrint;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating green cover: " + ex.Message);
                }

                try
                {
                    if (this.rcActive)
                    {
                        this.fretCovers[1].backCoverRenderer.color = this.rcPrint;
                        this.fretCovers[1].frontCoverRenderer.color = this.rcPrint;
                        this.fretCovers[1].headCoverRenderer.color = this.rcPrint;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating red cover: " + ex.Message);
                }

                try
                {
                    if (this.ycActive)
                    {
                        this.fretCovers[2].backCoverRenderer.color = this.ycPrint;
                        this.fretCovers[2].frontCoverRenderer.color = this.ycPrint;
                        this.fretCovers[2].headCoverRenderer.color = this.ycPrint;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating yellow cover: " + ex.Message);
                }

                try
                {
                    if (this.bcActive)
                    {
                        this.fretCovers[3].backCoverRenderer.color = this.bcPrint;
                        this.fretCovers[3].frontCoverRenderer.color = this.bcPrint;
                        this.fretCovers[3].headCoverRenderer.color = this.bcPrint;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating blue cover: " + ex.Message);
                }

                try
                {
                    if (this.ocActive)
                    {
                        this.fretCovers[4].backCoverRenderer.color = this.ocPrint;
                        this.fretCovers[4].frontCoverRenderer.color = this.ocPrint;
                        this.fretCovers[4].headCoverRenderer.color = this.ocPrint;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating orange covers: " + ex.Message);
                }

                try
                {
                    if (this.ghActive)
                    {
                        this.fretHeads[0].headRenderer.color = this.ghPrint;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating green heads: " + ex.Message);
                }

                try
                {
                    if (this.rhActive)
                    {
                        this.fretHeads[1].headRenderer.color = this.rhPrint;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating red heads: " + ex.Message);
                }

                try
                {
                    if (this.yhActive)
                    {
                        this.fretHeads[2].headRenderer.color = this.yhPrint;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating yellow heads: " + ex.Message);
                }

                try
                {
                    if (this.bhActive)
                    {
                        this.fretHeads[3].headRenderer.color = this.bhPrint;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating blue heads: " + ex.Message);
                }

                try
                {
                    if (this.ohActive)
                    {
                        this.fretHeads[4].headRenderer.color = this.ohPrint;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating orange heads: " + ex.Message);
                }

                try
                {
                    if (this.ghlActive)
                    {
                        this.fretHeads[0].headLightRenderer.color = this.ghlPrint;
                    }

                    if (this.rhlActive)
                    {
                        this.fretHeads[1].headLightRenderer.color = this.rhlPrint;
                    }

                    if (this.yhlActive)
                    {
                        this.fretHeads[2].headLightRenderer.color = this.yhlPrint;
                    }

                    if (this.bhlActive)
                    {
                        this.fretHeads[3].headLightRenderer.color = this.bhlPrint;
                    }

                    if (this.ohlActive)
                    {
                        this.fretHeads[4].headLightRenderer.color = this.ohlPrint;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating fret head lights: " + ex.Message);
                }

                try
                {
                    if (this.songpbActive)
                    {
                        this.pBar1.color = this.songpbPrint;
                    }

                    if (this.starpbActive)
                    {
                        this.pBar2.color = this.starpbPrint;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating progress bar: " + ex.Message);
                }

                try
                {
                    if (this.osActive)
                    {
                        this.outerStar.color = this.osPrint;
                        this.oStarRender.material.SetColor("_Color", this.osPrint);
                    }

                    if (this.isActive)
                    {
                        this.innerStar.color = this.isPrint;
                        this.iStarRender.material.SetColor("_Color", this.isPrint);
                    }

                    if (this.scTextActive)
                    {
                        this.starCount.color = this.scTextPrint;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating stars: " + ex.Message);
                }

                try
                {
                    if (!isBotEnabled())
                    {
                        if (this.fcActive)
                        {
                            this.fcBg.color = this.fcPrint;
                            this.fcBgRender.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                            this.fcRotate.GetComponent<SpriteRenderer>().color = this.fcPrint;
                            this.fcRotateRender.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating FC indicator: " + ex.Message);
                }

                try
                {
                    if (this.isStarPower && !this.mnActive)
                    {
                        this.multiRender.material.SetColor("_Color", Color.white);
                        this.multiGlowRender.material.SetColor("_Color", Color.white);
                        this.mn.color = this.defaultSpColor;
                        this.mg.color = this.defaultSpColor;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating multiplier (SP active): " + ex.Message);
                }

                try
                {
                    if (this.isStarPower && !this.mcActive)
                    {
                        this.comboCountRender.material.SetColor("_Color", Color.white);
                        this.cc.color = this.defaultSpColor;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating combo counter (SP active): " + ex.Message);
                }

                try
                {
                    if (!isBotEnabled())
                    {
                        if (this.mnActive)
                        {
                            if (this.isStarPower)
                            {
                                this.multiRender.material.SetColor("_Color", Color.white);
                                this.multiGlowRender.material.SetColor("_Color", Color.white);
                                this.mn.color = this.defaultSpColor;
                                this.mg.color = this.defaultSpColor;
                            }
                            else
                            {
                                this.multiRender.material.SetColor("_Color", this.mnPrint);
                                this.multiGlowRender.material.SetColor("_Color", this.mnPrint);
                                this.mn.color = this.mnPrint;
                                this.mg.color = this.mnPrint;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating multiplier: " + ex.Message);
                }

                try
                {
                    if (!isBotEnabled())
                    {
                        if (this.mcActive)
                        {
                            if (this.isStarPower)
                            {
                                this.comboCountRender.material.SetColor("_Color", Color.white);
                                this.cc.color = this.defaultSpColor;
                            }
                            else
                            {
                                this.comboCountRender.material.SetColor("_Color", this.mcPrint);
                                this.cc.color = this.mcPrint;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating combo counter: " + ex.Message);
                }

                try
                {
                    if (this.onActive)
                    {
                        if (this.isStarPower)
                        {
                            this.onOval.color = this.spPrint;
                            this.onOvalFlames.color = this.spPrint;
                        }
                        else
                        {
                            this.onOval.color = this.onPrint;
                            this.onOvalFlames.color = this.onPrint;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating oval flames: " + ex.Message);
                }

                try
                {
                    if (this.pgActive)
                    {
                        ParticleSystem.MainModule main14 = this.particles[14].main;
                        ParticleSystem.MainModule main9 = this.particles[9].main;
                        if (this.isStarPower)
                        {
                            main14.startColor = this.defaultSpColor;
                            main9.startColor = this.defaultSpColor;
                        }
                        else
                        {
                            main14.startColor = this.pgPrint;
                            main9.startColor = this.pgPrint;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating green particle system: " + ex.Message);
                }

                try
                {
                    if (this.prActive)
                    {
                        ParticleSystem.MainModule main13 = this.particles[13].main;
                        ParticleSystem.MainModule main8 = this.particles[8].main;
                        if (this.isStarPower)
                        {
                            main13.startColor = this.defaultSpColor;
                            main8.startColor = this.defaultSpColor;
                        }
                        else
                        {
                            main13.startColor = this.prPrint;
                            main8.startColor = this.prPrint;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating red particle system: " + ex.Message);
                }

                try
                {
                    if (this.pyActive)
                    {
                        ParticleSystem.MainModule main12 = this.particles[12].main;
                        ParticleSystem.MainModule main7 = this.particles[7].main;
                        if (this.isStarPower)
                        {
                            main12.startColor = this.defaultSpColor;
                            main7.startColor = this.defaultSpColor;
                        }
                        else
                        {
                            main12.startColor = this.pyPrint;
                            main7.startColor = this.pyPrint;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating yellow particle system: " + ex.Message);
                }

                try
                {
                    if (this.pbActive)
                    {
                        ParticleSystem.MainModule main11 = this.particles[11].main;
                        ParticleSystem.MainModule main6 = this.particles[6].main;
                        if (this.isStarPower)
                        {
                            main11.startColor = this.defaultSpColor;
                            main6.startColor = this.defaultSpColor;
                        }
                        else
                        {
                            main11.startColor = this.pbPrint;
                            main6.startColor = this.pbPrint;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating blue particle system: " + ex.Message);
                }

                try
                {
                    if (this.poActive)
                    {
                        ParticleSystem.MainModule main10 = this.particles[10].main;
                        ParticleSystem.MainModule main5 = this.particles[5].main;
                        if (this.isStarPower)
                        {
                            main10.startColor = this.defaultSpColor;
                            main5.startColor = this.defaultSpColor;
                        }
                        else
                        {
                            main10.startColor = this.poPrint;
                            main5.startColor = this.poPrint;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating orange particle system: " + ex.Message);
                }

                try
                {
                    if (this.isStarPower && this.spGlowActive)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            ParticleSystem.MainModule main = this.particles[j].main;
                            main.startColor = this.defaultSpColor;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating SP particle systems: " + ex.Message);
                }

                try
                {
                    if (this.strikeGActive)
                    {
                        this.strikeStrings[4].color = this.strikelGPrint;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating green strike string: " + ex.Message);
                }

                try
                {
                    if (this.strikeRActive)
                    {
                        this.strikeStrings[3].color = this.strikelRPrint;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating red strike string: " + ex.Message);
                }

                try
                {
                    if (this.strikeYActive)
                    {
                        this.strikeStrings[2].color = this.strikelYPrint;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating yellow strike string: " + ex.Message);
                }

                try
                {
                    if (this.strikeBActive)
                    {
                        this.strikeStrings[1].color = this.strikelBPrint;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating blue strike string: " + ex.Message);
                }

                try
                {
                    if (this.strikeOActive)
                    {
                        this.strikeStrings[0].color = this.strikelOPrint;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating orange strike string: " + ex.Message);
                }

                try
                {
                    if (this.particleSettingsActive)
                    {
                        foreach (ParticleSystem particleSystem in this.particles)
                        {
                            ParticleSystem.MainModule main = particleSystem.main;
                            main.startSize = this.particleSize;
                            main.startSpeed = this.particleSpeed;
                            main.gravityModifier = this.particleGravity;
                            main.maxParticles = (int)this.particleMax;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating particle system settings: " + ex.Message);
                }

                try
                {
                    if (this.fpsCounter)
                    {
                        this.timeleft -= Time.deltaTime;
                        this.accum += Time.timeScale / Time.deltaTime;
                        this.frames++;

                        if (timeleft <= 0)
                        {
                            float num = this.accum / frames;
                            if (num >= 30)
                            {
                                this.fpsCount.normal.textColor = Color.green;
                            }
                            else if (num < 30 && num >= 15)
                            {
                                this.fpsCount.normal.textColor = Color.yellow;
                            }
                            else
                            {
                                this.fpsCount.normal.textColor = Color.red;
                            }

                            this.debug = string.Format("{0:N0} FPS", num);
                            this.timeleft = 0.5f;
                            this.accum = 0;
                            this.frames = 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error occurred updating FPS counter: " + ex.Message);
                }
            }
        }

        private void OnGUI()
        {
            try
            {
                if (this.fpsCounter && GameStatus.gameState == GameStatus.GameState.PlayingSong)
                {
                    GUI.Label(new Rect(25, 25, 400, 100), this.debug, this.fpsCount);
                }

                if (this.menuLoaded)
                {
                    this.SetSkins();
                    this.windowRect = GUILayout.Window(0, this.windowRect, new GUI.WindowFunction(this.OnWindow), this.menuTitle, new GUILayoutOption[0]);
                }
            }
            catch
            {
            }
        }

        private void OnWindow(int ID)
        {
            try
            {
                if (ID == 0)
                {
                    switch (this.menuIndex)
                    {
                        case 0:
                            this.menuTitle = "Game Colors [Main]";

                            if (GUILayout.Button("Highway Modifications", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 75;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("UI Modifications", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 76;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Save/Load Profiles", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 52;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Menu Settings", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 99;
                            }

                            GUILayout.Space(2);
                            this.fpsCounter = GUILayout.Toggle(this.fpsCounter, "Enable FPS Counter", new GUILayoutOption[0]);
                            GUILayout.Space(10);
                            GUILayout.Label("(Huge thanks to kurt2467 for v.21 Note Fixes!)", this.centerText, new GUILayoutOption[0]);
                            GUILayout.Space(10);
                            GUILayout.Label("Press " + KeyBindConfig.menuKey.ToString() + " to Close", this.centerText, new GUILayoutOption[0]);

                            break;
                        case 1:
                            this.menuTitle = "Game Colors [Notes]";

                            if (GUILayout.Button("Green Note", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 2;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Red Note", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 3;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Yellow Note", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 4;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Blue Note", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 5;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Orange Note", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 6;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("All Notes", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 7;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Open Notes", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 64;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Highway Modifications", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 75;
                            }

                            break;
                        case 2:
                            this.menuTitle = "Game Colors [Green Note]";
                            GUILayout.Label("Red Value [" + this.greenR + "]", new GUILayoutOption[0]);

                            this.greenR = GUILayout.HorizontalSlider(this.greenR, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Green Value [" + this.greenG + "]", new GUILayoutOption[0]);

                            this.greenG = GUILayout.HorizontalSlider(this.greenG, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Blue Value [" + this.greenB + "]", new GUILayoutOption[0]);

                            this.greenB = GUILayout.HorizontalSlider(this.greenB, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Alpha Value [" + this.greenA + "]", new GUILayoutOption[0]);

                            this.greenA = GUILayout.HorizontalSlider(this.greenA, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Current Color Selection", this.greenEx, new GUILayoutOption[0]);

                            if (GUILayout.Button("Enable Green Note", new GUILayoutOption[0]))
                            {
                                this.greenActive = true;
                                this.LoadNotes();
                            }

                            if (GUILayout.Button("Disable Green Note", new GUILayoutOption[0]))
                            {
                                this.greenActive = false;
                                this.LoadNotes();
                            }

                            if (GUILayout.Button("Enable Rainbow", new GUILayoutOption[0]))
                            {
                                this.greenRB = true;
                                this.greenActive = false;
                            }

                            if (GUILayout.Button("Disable Rainbow", new GUILayoutOption[0]))
                            {
                                this.greenRB = false;
                            }

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Notes", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 1;
                            }

                            break;
                        case 3:
                            this.menuTitle = "Game Colors [Red Note]";
                            GUILayout.Label("Red Value [" + this.redR + "]", new GUILayoutOption[0]);

                            this.redR = GUILayout.HorizontalSlider(this.redR, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Green Value [" + this.redG + "]", new GUILayoutOption[0]);

                            this.redG = GUILayout.HorizontalSlider(this.redG, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Blue Value [" + this.redB + "]", new GUILayoutOption[0]);

                            this.redB = GUILayout.HorizontalSlider(this.redB, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Alpha Value [" + this.redA + "]", new GUILayoutOption[0]);

                            this.redA = GUILayout.HorizontalSlider(this.redA, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Current Color Selection", this.redEx, new GUILayoutOption[0]);

                            if (GUILayout.Button("Enable Red Note", new GUILayoutOption[0]))
                            {
                                this.redActive = true;
                                this.LoadNotes();
                            }

                            if (GUILayout.Button("Disable Red Note", new GUILayoutOption[0]))
                            {
                                this.redActive = false;
                                this.LoadNotes();
                            }

                            if (GUILayout.Button("Enable Rainbow", new GUILayoutOption[0]))
                            {
                                this.redRB = true;
                                this.redActive = false;
                            }

                            if (GUILayout.Button("Disable Rainbow", new GUILayoutOption[0]))
                            {
                                this.redRB = false;
                            }

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Notes", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 1;
                            }

                            break;
                        case 4:
                            this.menuTitle = "Game Colors [Yellow Note]";
                            GUILayout.Label("Red Value [" + this.yellowR + "]", new GUILayoutOption[0]);

                            this.yellowR = GUILayout.HorizontalSlider(this.yellowR, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Green Value [" + this.yellowG + "]", new GUILayoutOption[0]);

                            this.yellowG = GUILayout.HorizontalSlider(this.yellowG, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Blue Value [" + this.yellowB + "]", new GUILayoutOption[0]);

                            this.yellowB = GUILayout.HorizontalSlider(this.yellowB, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Alpha Value [" + this.yellowA + "]", new GUILayoutOption[0]);

                            this.yellowA = GUILayout.HorizontalSlider(this.yellowA, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Current Color Selection", this.yellowEx, new GUILayoutOption[0]);

                            if (GUILayout.Button("Enable Yellow Note", new GUILayoutOption[0]))
                            {
                                this.yellowActive = true;
                                this.LoadNotes();
                            }

                            if (GUILayout.Button("Disable Yellow Note", new GUILayoutOption[0]))
                            {
                                this.yellowActive = false;
                                this.LoadNotes();
                            }

                            if (GUILayout.Button("Enable Rainbow", new GUILayoutOption[0]))
                            {
                                this.yellowRB = true;
                                this.yellowActive = false;
                            }

                            if (GUILayout.Button("Disable Rainbow", new GUILayoutOption[0]))
                            {
                                this.yellowRB = false;
                            }

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Notes", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 1;
                            }

                            break;
                        case 5:
                            this.menuTitle = "Game Colors [Blue Note]";
                            GUILayout.Label("Red Value [" + this.blueR + "]", new GUILayoutOption[0]);

                            this.blueR = GUILayout.HorizontalSlider(this.blueR, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Green Value [" + this.blueG + "]", new GUILayoutOption[0]);

                            this.blueG = GUILayout.HorizontalSlider(this.blueG, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Blue Value [" + this.blueB + "]", new GUILayoutOption[0]);

                            this.blueB = GUILayout.HorizontalSlider(this.blueB, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Alpha Value [" + this.blueA + "]", new GUILayoutOption[0]);

                            this.blueA = GUILayout.HorizontalSlider(this.blueA, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Current Color Selection", this.blueEx, new GUILayoutOption[0]);

                            if (GUILayout.Button("Enable Blue Note", new GUILayoutOption[0]))
                            {
                                this.blueActive = true;
                                this.LoadNotes();
                            }

                            if (GUILayout.Button("Disable Blue Note", new GUILayoutOption[0]))
                            {
                                this.blueActive = false;
                                this.LoadNotes();
                            }

                            if (GUILayout.Button("Enable Rainbow", new GUILayoutOption[0]))
                            {
                                this.blueRB = true;
                                this.blueActive = false;
                            }

                            if (GUILayout.Button("Disable Rainbow", new GUILayoutOption[0]))
                            {
                                this.blueRB = false;
                            }

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Notes", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 1;
                            }

                            break;
                        case 6:
                            this.menuTitle = "Game Colors [Orange Note]";
                            GUILayout.Label("Red Value [" + this.orangeR + "]", new GUILayoutOption[0]);

                            this.orangeR = GUILayout.HorizontalSlider(this.orangeR, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Green Value [" + this.orangeG + "]", new GUILayoutOption[0]);

                            this.orangeG = GUILayout.HorizontalSlider(this.orangeG, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Blue Value [" + this.orangeB + "]", new GUILayoutOption[0]);

                            this.orangeB = GUILayout.HorizontalSlider(this.orangeB, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Alpha Value [" + this.orangeA + "]", new GUILayoutOption[0]);

                            this.orangeA = GUILayout.HorizontalSlider(this.orangeA, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Current Color Selection", this.orangeEx, new GUILayoutOption[0]);

                            if (GUILayout.Button("Enable Orange Note", new GUILayoutOption[0]))
                            {
                                this.orangeActive = true;
                                this.LoadNotes();
                            }

                            if (GUILayout.Button("Disable Orange Note", new GUILayoutOption[0]))
                            {
                                this.orangeActive = false;
                                this.LoadNotes();
                            }

                            if (GUILayout.Button("Enable Rainbow", new GUILayoutOption[0]))
                            {
                                this.orangeRB = true;
                                this.orangeActive = false;
                            }

                            if (GUILayout.Button("Disable Rainbow", new GUILayoutOption[0]))
                            {
                                this.orangeRB = false;
                            }

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Notes", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 1;
                            }

                            break;
                        case 7:
                            this.menuTitle = "Game Colors [All Notes]";
                            GUILayout.Label("Red Value [" + this.orangeR + "]", new GUILayoutOption[0]);

                            this.greenR = (this.redR = (this.yellowR = (this.blueR = (this.orangeR = GUILayout.HorizontalSlider(this.orangeR, 0, 255, new GUILayoutOption[0])))));
                            GUILayout.Label("Green Value [" + this.orangeG + "]", new GUILayoutOption[0]);

                            this.greenG = (this.redG = (this.yellowG = (this.blueG = (this.orangeG = GUILayout.HorizontalSlider(this.orangeG, 0, 255, new GUILayoutOption[0])))));
                            GUILayout.Label("Blue Value [" + this.orangeB + "]", new GUILayoutOption[0]);

                            this.greenB = (this.redB = (this.yellowB = (this.blueB = (this.orangeB = GUILayout.HorizontalSlider(this.orangeB, 0, 255, new GUILayoutOption[0])))));
                            GUILayout.Label("Alpha Value [" + this.orangeA + "]", new GUILayoutOption[0]);

                            this.greenA = (this.redA = (this.yellowA = (this.blueA = (this.orangeA = GUILayout.HorizontalSlider(this.orangeA, 0, 255, new GUILayoutOption[0])))));
                            GUILayout.Label("Current Color Selection", this.orangeEx, new GUILayoutOption[0]);

                            GUILayout.Space(2);

                            if (GUILayout.Button("Enable All Colors", new GUILayoutOption[0]))
                            {
                                this.greenActive = (this.redActive = (this.yellowActive = (this.blueActive = (this.orangeActive = true))));
                                this.LoadNotes();
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Disable All Colors", new GUILayoutOption[0]))
                            {
                                this.greenActive = (this.redActive = (this.yellowActive = (this.blueActive = (this.orangeActive = false))));
                                this.LoadNotes();
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Enable Rainbow", new GUILayoutOption[0]))
                            {
                                this.greenRB = (this.redRB = (this.yellowRB = (this.blueRB = (this.orangeRB = true))));
                                this.greenActive = (this.redActive = (this.yellowActive = (this.blueActive = (this.orangeActive = false))));
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Disable Rainbow", new GUILayoutOption[0]))
                            {
                                this.greenRB = (this.redRB = (this.yellowRB = (this.blueRB = (this.orangeRB = false))));
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Notes", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 1;
                            }

                            break;
                        case 8:
                            this.menuTitle = "Game Colors [Star Power]";

                            if (GUILayout.Button("Star Power Notes", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 65;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Star Power Highway", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 66;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Highway Modifications", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 75;
                            }

                            break;
                        case 9:
                            this.menuTitle = "Game Colors [Note Flame Colors]";

                            if (GUILayout.Button("Green Note Flame", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 10;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Red Note Flame", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 11;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Yellow Note Flame", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 12;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Blue Note Flame", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 13;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Orange Note Flame", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 14;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("All Note Flames", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 15;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Highway Modifications", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 75;
                            }

                            break;
                        case 10:
                            this.menuTitle = "Game Colors [Green Note Flame]";
                            GUILayout.Label("Red Value [" + this.gfR + "]", new GUILayoutOption[0]);

                            this.gfR = GUILayout.HorizontalSlider(this.gfR, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Green Value [" + this.gfG + "]", new GUILayoutOption[0]);

                            this.gfG = GUILayout.HorizontalSlider(this.gfG, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Blue Value [" + this.gfB + "]", new GUILayoutOption[0]);

                            this.gfB = GUILayout.HorizontalSlider(this.gfB, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Alpha Value [" + this.gfA + "]", new GUILayoutOption[0]);

                            this.gfA = GUILayout.HorizontalSlider(this.gfA, 0, 255, new GUILayoutOption[0]);
                            GUILayout.Label("Current Color Selection", this.gfEx, new GUILayoutOption[0]);

                            this.gfActive = GUILayout.Toggle(this.gfActive, "Color Enabled", new GUILayoutOption[0]);
                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Note Flames", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 9;
                            }

                            break;
                        case 11:
                            this.menuTitle = "Game Colors [Red Note Flame]";

                            GUILayout.Label("Red Value [" + this.rfR + "]", new GUILayoutOption[0]);
                            this.rfR = GUILayout.HorizontalSlider(this.rfR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.rfG + "]", new GUILayoutOption[0]);
                            this.rfG = GUILayout.HorizontalSlider(this.rfG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.rfB + "]", new GUILayoutOption[0]);
                            this.rfB = GUILayout.HorizontalSlider(this.rfB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.rfA + "]", new GUILayoutOption[0]);
                            this.rfA = GUILayout.HorizontalSlider(this.rfA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.rfEx, new GUILayoutOption[0]);
                            this.rfActive = GUILayout.Toggle(this.rfActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Note Flames", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 9;
                            }

                            break;
                        case 12:
                            this.menuTitle = "Game Colors [Yellow Note Flame]";

                            GUILayout.Label("Red Value [" + this.yfR + "]", new GUILayoutOption[0]);
                            this.yfR = GUILayout.HorizontalSlider(this.yfR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.yfG + "]", new GUILayoutOption[0]);
                            this.yfG = GUILayout.HorizontalSlider(this.yfG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.yfB + "]", new GUILayoutOption[0]);
                            this.yfB = GUILayout.HorizontalSlider(this.yfB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.yfA + "]", new GUILayoutOption[0]);
                            this.yfA = GUILayout.HorizontalSlider(this.yfA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.yfEx, new GUILayoutOption[0]);
                            this.yfActive = GUILayout.Toggle(this.yfActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Note Flames", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 9;
                            }

                            break;
                        case 13:
                            this.menuTitle = "Game Colors [Blue Note Flame]";

                            GUILayout.Label("Red Value [" + this.bfR + "]", new GUILayoutOption[0]);
                            this.bfR = GUILayout.HorizontalSlider(this.bfR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.bfG + "]", new GUILayoutOption[0]);
                            this.bfG = GUILayout.HorizontalSlider(this.bfG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.bfB + "]", new GUILayoutOption[0]);
                            this.bfB = GUILayout.HorizontalSlider(this.bfB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.bfA + "]", new GUILayoutOption[0]);
                            this.bfA = GUILayout.HorizontalSlider(this.bfA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.bfEx, new GUILayoutOption[0]);
                            this.bfActive = GUILayout.Toggle(this.bfActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Note Flames", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 9;
                            }

                            break;
                        case 14:
                            this.menuTitle = "Game Colors [Orange Note Flame]";

                            GUILayout.Label("Red Value [" + this.ofR + "]", new GUILayoutOption[0]);
                            this.ofR = GUILayout.HorizontalSlider(this.ofR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.ofG + "]", new GUILayoutOption[0]);
                            this.ofG = GUILayout.HorizontalSlider(this.ofG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.ofB + "]", new GUILayoutOption[0]);
                            this.ofB = GUILayout.HorizontalSlider(this.ofB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.ofA + "]", new GUILayoutOption[0]);
                            this.ofA = GUILayout.HorizontalSlider(this.ofA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.ofEx, new GUILayoutOption[0]);
                            this.ofActive = GUILayout.Toggle(this.ofActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Note Flames", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 9;
                            }

                            break;
                        case 15:
                            this.menuTitle = "Game Colors [All Note Flames]";

                            GUILayout.Label("Red Value [" + this.ofR + "]", new GUILayoutOption[0]);
                            this.gfR = (this.rfR = (this.yfR = (this.bfR = (this.ofR = GUILayout.HorizontalSlider(this.ofR, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Green Value [" + this.ofG + "]", new GUILayoutOption[0]);
                            this.gfG = (this.rfG = (this.yfG = (this.bfG = (this.ofG = GUILayout.HorizontalSlider(this.ofG, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Blue Value [" + this.ofB + "]", new GUILayoutOption[0]);
                            this.gfB = (this.rfB = (this.yfB = (this.bfB = (this.ofB = GUILayout.HorizontalSlider(this.ofB, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Alpha Value [" + this.ofA + "]", new GUILayoutOption[0]);
                            this.gfA = (this.rfA = (this.yfA = (this.bfA = (this.ofA = GUILayout.HorizontalSlider(this.ofA, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Current Color Selection", this.ofEx, new GUILayoutOption[0]);
                            GUILayout.Space(2);

                            if (GUILayout.Button("Enable All Colors", new GUILayoutOption[0]))
                            {
                                this.gfActive = (this.rfActive = (this.yfActive = (this.bfActive = (this.ofActive = true))));
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Disable All Colors", new GUILayoutOption[0]))
                            {
                                this.gfActive = (this.rfActive = (this.yfActive = (this.bfActive = (this.ofActive = false))));
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Note Flames", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 9;
                            }

                            break;
                        case 16:
                            this.menuTitle = "Game Colors [Lightning Colors]";

                            if (GUILayout.Button("Green Note Lightning", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 17;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Red Note Lightning", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 18;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Yellow Note Lightning", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 19;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Blue Note Lightning", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 20;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Orange Note Lightning", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 21;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("All Note Lightning", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 22;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Highway Modifications", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 75;
                            }

                            break;
                        case 17:
                            this.menuTitle = "Game Colors [Green Note Lightning]";

                            GUILayout.Label("Red Value [" + this.glR + "]", new GUILayoutOption[0]);
                            this.glR = GUILayout.HorizontalSlider(this.glR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.glG + "]", new GUILayoutOption[0]);
                            this.glG = GUILayout.HorizontalSlider(this.glG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.glB + "]", new GUILayoutOption[0]);
                            this.glB = GUILayout.HorizontalSlider(this.glB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.glA + "]", new GUILayoutOption[0]);
                            this.glA = GUILayout.HorizontalSlider(this.glA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.glEx, new GUILayoutOption[0]);
                            this.glActive = GUILayout.Toggle(this.glActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Lightning Colors", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 16;
                            }

                            break;
                        case 18:
                            this.menuTitle = "Game Colors [Red Note Lightning]";

                            GUILayout.Label("Red Value [" + this.rlR + "]", new GUILayoutOption[0]);
                            this.rlR = GUILayout.HorizontalSlider(this.rlR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.rlG + "]", new GUILayoutOption[0]);
                            this.rlG = GUILayout.HorizontalSlider(this.rlG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.rlB + "]", new GUILayoutOption[0]);
                            this.rlB = GUILayout.HorizontalSlider(this.rlB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.rlA + "]", new GUILayoutOption[0]);
                            this.rlA = GUILayout.HorizontalSlider(this.rlA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.rlEx, new GUILayoutOption[0]);
                            this.rlActive = GUILayout.Toggle(this.rlActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Lightning Colors", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 16;
                            }

                            break;
                        case 19:
                            this.menuTitle = "Game Colors [Yellow Note Lightning]";

                            GUILayout.Label("Red Value [" + this.ylR + "]", new GUILayoutOption[0]);
                            this.ylR = GUILayout.HorizontalSlider(this.ylR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.ylG + "]", new GUILayoutOption[0]);
                            this.ylG = GUILayout.HorizontalSlider(this.ylG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.ylB + "]", new GUILayoutOption[0]);
                            this.ylB = GUILayout.HorizontalSlider(this.ylB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.ylA + "]", new GUILayoutOption[0]);
                            this.ylA = GUILayout.HorizontalSlider(this.ylA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.ylEx, new GUILayoutOption[0]);
                            this.ylActive = GUILayout.Toggle(this.ylActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Lightning Colors", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 16;
                            }

                            break;
                        case 20:
                            this.menuTitle = "Game Colors [Blue Note Lightning]";

                            GUILayout.Label("Red Value [" + this.blR + "]", new GUILayoutOption[0]);
                            this.blR = GUILayout.HorizontalSlider(this.blR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.blG + "]", new GUILayoutOption[0]);
                            this.blG = GUILayout.HorizontalSlider(this.blG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.blB + "]", new GUILayoutOption[0]);
                            this.blB = GUILayout.HorizontalSlider(this.blB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.blA + "]", new GUILayoutOption[0]);
                            this.blA = GUILayout.HorizontalSlider(this.blA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.blEx, new GUILayoutOption[0]);
                            this.blActive = GUILayout.Toggle(this.blActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Lightning Colors", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 16;
                            }

                            break;
                        case 21:
                            this.menuTitle = "Game Colors [Orange Note Lightning]";

                            GUILayout.Label("Red Value [" + this.olR + "]", new GUILayoutOption[0]);
                            this.olR = GUILayout.HorizontalSlider(this.olR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.olG + "]", new GUILayoutOption[0]);
                            this.olG = GUILayout.HorizontalSlider(this.olG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.olB + "]", new GUILayoutOption[0]);
                            this.olB = GUILayout.HorizontalSlider(this.olB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.olA + "]", new GUILayoutOption[0]);
                            this.olA = GUILayout.HorizontalSlider(this.olA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.olEx, new GUILayoutOption[0]);
                            this.olActive = GUILayout.Toggle(this.olActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Lightning Colors", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 16;
                            }

                            break;
                        case 22:
                            this.menuTitle = "Game Colors [All Notes Lightning]";

                            GUILayout.Label("Red Value [" + this.olR + "]", new GUILayoutOption[0]);
                            this.glR = (this.rlR = (this.ylR = (this.blR = (this.olR = GUILayout.HorizontalSlider(this.olR, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Green Value [" + this.olG + "]", new GUILayoutOption[0]);
                            this.glG = (this.rlG = (this.ylG = (this.blG = (this.olG = GUILayout.HorizontalSlider(this.olG, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Blue Value [" + this.olB + "]", new GUILayoutOption[0]);
                            this.glB = (this.rlB = (this.ylB = (this.blB = (this.olB = GUILayout.HorizontalSlider(this.olB, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Alpha Value [" + this.olA + "]", new GUILayoutOption[0]);
                            this.glA = (this.rlA = (this.ylA = (this.blA = (this.olA = GUILayout.HorizontalSlider(this.olA, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Current Color Selection", this.olEx, new GUILayoutOption[0]);
                            GUILayout.Space(2);

                            if (GUILayout.Button("Enable All Colors", new GUILayoutOption[0]))
                            {
                                this.glActive = (this.rlActive = (this.ylActive = (this.blActive = (this.olActive = true))));
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Disable All Colors", new GUILayoutOption[0]))
                            {
                                this.glActive = (this.rlActive = (this.ylActive = (this.blActive = (this.olActive = false))));
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Lightning Colors", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 16;
                            }

                            break;
                        case 23:
                            this.menuTitle = "Game Colors [SP Bar Colors]";

                            if (GUILayout.Button("Lower Bar Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 24;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Upper Bar Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 25;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Lightning Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 26;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Highway Modifications", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 75;
                            }

                            break;
                        case 24:
                            this.menuTitle = "Game Colors [Lower Bar Color]";

                            GUILayout.Label("Red Value [" + this.spblR + "]", new GUILayoutOption[0]);
                            this.spblR = GUILayout.HorizontalSlider(this.spblR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.spblG + "]", new GUILayoutOption[0]);
                            this.spblG = GUILayout.HorizontalSlider(this.spblG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.spblB + "]", new GUILayoutOption[0]);
                            this.spblB = GUILayout.HorizontalSlider(this.spblB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.spblA + "]", new GUILayoutOption[0]);
                            this.spblA = GUILayout.HorizontalSlider(this.spblA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.spblEx, new GUILayoutOption[0]);
                            this.spblActive = GUILayout.Toggle(this.spblActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to SP Bar Colors", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 23;
                            }

                            break;
                        case 25:
                            this.menuTitle = "Game Colors [Upper Bar Color]";

                            GUILayout.Label("Red Value [" + this.spbuR + "]", new GUILayoutOption[0]);
                            this.spbuR = GUILayout.HorizontalSlider(this.spbuR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.spbuG + "]", new GUILayoutOption[0]);
                            this.spbuG = GUILayout.HorizontalSlider(this.spbuG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.spbuB + "]", new GUILayoutOption[0]);
                            this.spbuB = GUILayout.HorizontalSlider(this.spbuB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.spbuA + "]", new GUILayoutOption[0]);
                            this.spbuA = GUILayout.HorizontalSlider(this.spbuA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.spbuEx, new GUILayoutOption[0]);
                            this.spbuActive = GUILayout.Toggle(this.spbuActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to SP Bar Colors", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 23;
                            }

                            break;
                        case 26:
                            this.menuTitle = "Game Colors [SP Bar Lightning Color]";

                            GUILayout.Label("Red Value [" + this.spbLtR + "]", new GUILayoutOption[0]);
                            this.spbLtR = GUILayout.HorizontalSlider(this.spbLtR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.spbLtG + "]", new GUILayoutOption[0]);
                            this.spbLtG = GUILayout.HorizontalSlider(this.spbLtG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.spbLtB + "]", new GUILayoutOption[0]);
                            this.spbLtB = GUILayout.HorizontalSlider(this.spbLtB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.spbLtA + "]", new GUILayoutOption[0]);
                            this.spbLtA = GUILayout.HorizontalSlider(this.spbLtA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.spbLtEx, new GUILayoutOption[0]);
                            this.spbLtActive = GUILayout.Toggle(this.spbLtActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to SP Bar Colors", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 23;
                            }

                            break;
                        case 27:
                            this.menuTitle = "Game Colors [Strikeline]";

                            if (GUILayout.Button("Green Strike", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 28;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Red Strike", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 32;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Yellow Strike", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 36;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Blue Strike", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 40;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Orange Strike", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 44;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("All Strikes", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 48;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Highway Modifications", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 75;
                            }

                            break;
                        case 28:
                            this.menuTitle = "Game Colors [Green Strike]";

                            if (GUILayout.Button("Cover Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 29;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Head Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 30;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Head Light Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 31;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Strikes", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 27;
                            }

                            break;
                        case 29:
                            this.menuTitle = "Game Colors [Green Strike Cover]";

                            GUILayout.Label("Red Value [" + this.gcR + "]", new GUILayoutOption[0]);
                            this.gcR = GUILayout.HorizontalSlider(this.gcR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.gcG + "]", new GUILayoutOption[0]);
                            this.gcG = GUILayout.HorizontalSlider(this.gcG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.gcB + "]", new GUILayoutOption[0]);
                            this.gcB = GUILayout.HorizontalSlider(this.gcB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.gcA + "]", new GUILayoutOption[0]);
                            this.gcA = GUILayout.HorizontalSlider(this.gcA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.gcEx, new GUILayoutOption[0]);
                            this.gcActive = GUILayout.Toggle(this.gcActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Green Strike", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 28;
                            }

                            break;
                        case 30:
                            this.menuTitle = "Game Colors [Green Strike Head]";

                            GUILayout.Label("Red Value [" + this.ghR + "]", new GUILayoutOption[0]);
                            this.ghR = GUILayout.HorizontalSlider(this.ghR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.ghG + "]", new GUILayoutOption[0]);
                            this.ghG = GUILayout.HorizontalSlider(this.ghG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.ghB + "]", new GUILayoutOption[0]);
                            this.ghB = GUILayout.HorizontalSlider(this.ghB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.ghA + "]", new GUILayoutOption[0]);
                            this.ghA = GUILayout.HorizontalSlider(this.ghA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.ghEx, new GUILayoutOption[0]);
                            this.ghActive = GUILayout.Toggle(this.ghActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Green Strike", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 28;
                            }

                            break;
                        case 31:
                            this.menuTitle = "Game Colors [Green Strike Head Light]";

                            GUILayout.Label("Red Value [" + this.ghlR + "]", new GUILayoutOption[0]);
                            this.ghlR = GUILayout.HorizontalSlider(this.ghlR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.ghlG + "]", new GUILayoutOption[0]);
                            this.ghlG = GUILayout.HorizontalSlider(this.ghlG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.ghlB + "]", new GUILayoutOption[0]);
                            this.ghlB = GUILayout.HorizontalSlider(this.ghlB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.ghlA + "]", new GUILayoutOption[0]);
                            this.ghlA = GUILayout.HorizontalSlider(this.ghlA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.ghlEx, new GUILayoutOption[0]);
                            this.ghlActive = GUILayout.Toggle(this.ghlActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Green Strike", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 28;
                            }

                            break;
                        case 32:
                            this.menuTitle = "Game Colors [Red Strike]";

                            if (GUILayout.Button("Cover Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 33;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Head Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 34;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Head Light Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 35;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Strikes", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 27;
                            }

                            break;
                        case 33:
                            this.menuTitle = "Game Colors [Red Strike Cover]";

                            GUILayout.Label("Red Value [" + this.rcR + "]", new GUILayoutOption[0]);
                            this.rcR = GUILayout.HorizontalSlider(this.rcR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.rcG + "]", new GUILayoutOption[0]);
                            this.rcG = GUILayout.HorizontalSlider(this.rcG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.rcB + "]", new GUILayoutOption[0]);
                            this.rcB = GUILayout.HorizontalSlider(this.rcB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.rcA + "]", new GUILayoutOption[0]);
                            this.rcA = GUILayout.HorizontalSlider(this.rcA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.rcEx, new GUILayoutOption[0]);
                            this.rcActive = GUILayout.Toggle(this.rcActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Red Strike", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 32;
                            }

                            break;
                        case 34:
                            this.menuTitle = "Game Colors [Red Strike Head]";

                            GUILayout.Label("Red Value [" + this.rhR + "]", new GUILayoutOption[0]);
                            this.rhR = GUILayout.HorizontalSlider(this.rhR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.rhG + "]", new GUILayoutOption[0]);
                            this.rhG = GUILayout.HorizontalSlider(this.rhG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.rhB + "]", new GUILayoutOption[0]);
                            this.rhB = GUILayout.HorizontalSlider(this.rhB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.rhA + "]", new GUILayoutOption[0]);
                            this.rhA = GUILayout.HorizontalSlider(this.rhA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.rhEx, new GUILayoutOption[0]);
                            this.rhActive = GUILayout.Toggle(this.rhActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Red Strike", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 32;
                            }

                            break;
                        case 35:
                            this.menuTitle = "Game Colors [Red Strike Head Light]";

                            GUILayout.Label("Red Value [" + this.rhlR + "]", new GUILayoutOption[0]);
                            this.rhlR = GUILayout.HorizontalSlider(this.rhlR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.rhlG + "]", new GUILayoutOption[0]);
                            this.rhlG = GUILayout.HorizontalSlider(this.rhlG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.rhlB + "]", new GUILayoutOption[0]);
                            this.rhlB = GUILayout.HorizontalSlider(this.rhlB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.rhlA + "]", new GUILayoutOption[0]);
                            this.rhlA = GUILayout.HorizontalSlider(this.rhlA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.rhlEx, new GUILayoutOption[0]);
                            this.rhlActive = GUILayout.Toggle(this.rhlActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Red Strike", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 32;
                            }

                            break;
                        case 36:
                            this.menuTitle = "Game Colors [Yellow Strike]";

                            if (GUILayout.Button("Cover Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 37;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Head Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 38;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Head Light Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 39;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Strikes", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 27;
                            }

                            break;
                        case 37:
                            this.menuTitle = "Game Colors [Yellow Strike Cover]";

                            GUILayout.Label("Red Value [" + this.ycR + "]", new GUILayoutOption[0]);
                            this.ycR = GUILayout.HorizontalSlider(this.ycR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.ycG + "]", new GUILayoutOption[0]);
                            this.ycG = GUILayout.HorizontalSlider(this.ycG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.ycB + "]", new GUILayoutOption[0]);
                            this.ycB = GUILayout.HorizontalSlider(this.ycB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.ycA + "]", new GUILayoutOption[0]);
                            this.ycA = GUILayout.HorizontalSlider(this.ycA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.ycEx, new GUILayoutOption[0]);
                            this.ycActive = GUILayout.Toggle(this.ycActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Yellow Strike", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 36;
                            }

                            break;
                        case 38:
                            this.menuTitle = "Game Colors [Yellow Strike Head]";

                            GUILayout.Label("Red Value [" + this.yhR + "]", new GUILayoutOption[0]);
                            this.yhR = GUILayout.HorizontalSlider(this.yhR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.yhG + "]", new GUILayoutOption[0]);
                            this.yhG = GUILayout.HorizontalSlider(this.yhG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.yhB + "]", new GUILayoutOption[0]);
                            this.yhB = GUILayout.HorizontalSlider(this.yhB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.yhA + "]", new GUILayoutOption[0]);
                            this.yhA = GUILayout.HorizontalSlider(this.yhA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.yhEx, new GUILayoutOption[0]);
                            this.yhActive = GUILayout.Toggle(this.yhActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Yellow Strike", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 36;
                            }

                            break;
                        case 39:
                            this.menuTitle = "Game Colors [Yellow Strike Head Light]";

                            GUILayout.Label("Red Value [" + this.yhlR + "]", new GUILayoutOption[0]);
                            this.yhlR = GUILayout.HorizontalSlider(this.yhlR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.yhlG + "]", new GUILayoutOption[0]);
                            this.yhlG = GUILayout.HorizontalSlider(this.yhlG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.yhlB + "]", new GUILayoutOption[0]);
                            this.yhlB = GUILayout.HorizontalSlider(this.yhlB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.yhlA + "]", new GUILayoutOption[0]);
                            this.yhlA = GUILayout.HorizontalSlider(this.yhlA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.yhlEx, new GUILayoutOption[0]);
                            this.yhlActive = GUILayout.Toggle(this.yhlActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Yellow Strike", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 36;
                            }

                            break;
                        case 40:
                            this.menuTitle = "Game Colors [Blue Strike]";

                            if (GUILayout.Button("Cover Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 41;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Head Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 42;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Head Light Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 43;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Strikes", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 27;
                            }

                            break;
                        case 41:
                            this.menuTitle = "Game Colors [Blue Strike Cover]";

                            GUILayout.Label("Red Value [" + this.bcR + "]", new GUILayoutOption[0]);
                            this.bcR = GUILayout.HorizontalSlider(this.bcR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.bcG + "]", new GUILayoutOption[0]);
                            this.bcG = GUILayout.HorizontalSlider(this.bcG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.bcB + "]", new GUILayoutOption[0]);
                            this.bcB = GUILayout.HorizontalSlider(this.bcB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.bcA + "]", new GUILayoutOption[0]);
                            this.bcA = GUILayout.HorizontalSlider(this.bcA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.bcEx, new GUILayoutOption[0]);
                            this.bcActive = GUILayout.Toggle(this.bcActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Blue Strike", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 40;
                            }

                            break;
                        case 42:
                            this.menuTitle = "Game Colors [Blue Strike Head]";

                            GUILayout.Label("Red Value [" + this.bhR + "]", new GUILayoutOption[0]);
                            this.bhR = GUILayout.HorizontalSlider(this.bhR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.bhG + "]", new GUILayoutOption[0]);
                            this.bhG = GUILayout.HorizontalSlider(this.bhG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.bhB + "]", new GUILayoutOption[0]);
                            this.bhB = GUILayout.HorizontalSlider(this.bhB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.bhA + "]", new GUILayoutOption[0]);
                            this.bhA = GUILayout.HorizontalSlider(this.bhA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.bhEx, new GUILayoutOption[0]);
                            this.bhActive = GUILayout.Toggle(this.bhActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Blue Strike", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 40;
                            }

                            break;
                        case 43:
                            this.menuTitle = "Game Colors [Blue Strike Head Light]";

                            GUILayout.Label("Red Value [" + this.bhlR + "]", new GUILayoutOption[0]);
                            this.bhlR = GUILayout.HorizontalSlider(this.bhlR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.bhlG + "]", new GUILayoutOption[0]);
                            this.bhlG = GUILayout.HorizontalSlider(this.bhlG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.bhlB + "]", new GUILayoutOption[0]);
                            this.bhlB = GUILayout.HorizontalSlider(this.bhlB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.bhlA + "]", new GUILayoutOption[0]);
                            this.bhlA = GUILayout.HorizontalSlider(this.bhlA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.bhlEx, new GUILayoutOption[0]);
                            this.bhlActive = GUILayout.Toggle(this.bhlActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Blue Strike", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 40;
                            }

                            break;
                        case 44:
                            this.menuTitle = "Game Colors [Orange Strike]";

                            if (GUILayout.Button("Cover Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 45;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Head Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 46;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Head Light Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 47;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Strikes", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 27;
                            }

                            break;
                        case 45:
                            this.menuTitle = "Game Colors [Orange Strike Cover]";

                            GUILayout.Label("Red Value [" + this.ocR + "]", new GUILayoutOption[0]);
                            this.ocR = GUILayout.HorizontalSlider(this.ocR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.ocG + "]", new GUILayoutOption[0]);
                            this.ocG = GUILayout.HorizontalSlider(this.ocG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.ocB + "]", new GUILayoutOption[0]);
                            this.ocB = GUILayout.HorizontalSlider(this.ocB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.ocA + "]", new GUILayoutOption[0]);
                            this.ocA = GUILayout.HorizontalSlider(this.ocA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.ocEx, new GUILayoutOption[0]);
                            this.ocActive = GUILayout.Toggle(this.ocActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Orange Strike", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 44;
                            }

                            break;
                        case 46:
                            this.menuTitle = "Game Colors [Orange Strike Head]";

                            GUILayout.Label("Red Value [" + this.ohR + "]", new GUILayoutOption[0]);
                            this.ohR = GUILayout.HorizontalSlider(this.ohR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.ohG + "]", new GUILayoutOption[0]);
                            this.ohG = GUILayout.HorizontalSlider(this.ohG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.ohB + "]", new GUILayoutOption[0]);
                            this.ohB = GUILayout.HorizontalSlider(this.ohB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.ohA + "]", new GUILayoutOption[0]);
                            this.ohA = GUILayout.HorizontalSlider(this.ohA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.ohEx, new GUILayoutOption[0]);
                            this.ohActive = GUILayout.Toggle(this.ohActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Orange Strike", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 44;
                            }

                            break;
                        case 47:
                            this.menuTitle = "Game Colors [Orange Strike Head Light]";

                            GUILayout.Label("Red Value [" + this.ohlR + "]", new GUILayoutOption[0]);
                            this.ohlR = GUILayout.HorizontalSlider(this.ohlR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.ohlG + "]", new GUILayoutOption[0]);
                            this.ohlG = GUILayout.HorizontalSlider(this.ohlG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.ohlB + "]", new GUILayoutOption[0]);
                            this.ohlB = GUILayout.HorizontalSlider(this.ohlB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.ohlA + "]", new GUILayoutOption[0]);
                            this.ohlA = GUILayout.HorizontalSlider(this.ohlA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.ohlEx, new GUILayoutOption[0]);
                            this.ohlActive = GUILayout.Toggle(this.ohlActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Orange Strike", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 44;
                            }

                            break;
                        case 48:
                            this.menuTitle = "Game Colors [All Strikes]";

                            if (GUILayout.Button("Cover Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 49;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Head Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 50;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Head Light Color", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 51;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Strikes", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 27;
                            }

                            break;
                        case 49:
                            this.menuTitle = "Game Colors [All Strike Covers]";

                            GUILayout.Label("Red Value [" + this.ocR + "]", new GUILayoutOption[0]);
                            this.gcR = (this.rcR = (this.ycR = (this.bcR = (this.ocR = GUILayout.HorizontalSlider(this.ocR, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Green Value [" + this.ocG + "]", new GUILayoutOption[0]);
                            this.gcG = (this.rcG = (this.ycG = (this.bcG = (this.ocG = GUILayout.HorizontalSlider(this.ocG, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Blue Value [" + this.ocB + "]", new GUILayoutOption[0]);
                            this.gcB = (this.rcB = (this.ycB = (this.bcB = (this.ocB = GUILayout.HorizontalSlider(this.ocB, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Alpha Value [" + this.ocA + "]", new GUILayoutOption[0]);
                            this.gcA = (this.rcA = (this.ycA = (this.bcA = (this.ocA = GUILayout.HorizontalSlider(this.ocA, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Current Color Selection", this.ocEx, new GUILayoutOption[0]);
                            GUILayout.Space(2);

                            if (GUILayout.Button("Enable All Colors", new GUILayoutOption[0]))
                            {
                                this.gcActive = (this.bcActive = (this.ycActive = (this.rcActive = (this.ocActive = true))));
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Disable All Colors", new GUILayoutOption[0]))
                            {
                                this.gcActive = (this.bcActive = (this.ycActive = (this.rcActive = (this.ocActive = false))));
                            }

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to All Strikes", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 48;
                            }

                            break;
                        case 50:
                            this.menuTitle = "Game Colors [All Strike Heads]";

                            GUILayout.Label("Red Value [" + this.ohR + "]", new GUILayoutOption[0]);
                            this.ghR = (this.rhR = (this.yhR = (this.bhR = (this.ohR = GUILayout.HorizontalSlider(this.ohR, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Green Value [" + this.ohG + "]", new GUILayoutOption[0]);
                            this.ghG = (this.rhG = (this.yhG = (this.bhG = (this.ohG = GUILayout.HorizontalSlider(this.ohG, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Blue Value [" + this.ohB + "]", new GUILayoutOption[0]);
                            this.ghB = (this.rhB = (this.yhB = (this.bhB = (this.ohB = GUILayout.HorizontalSlider(this.ohB, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Alpha Value [" + this.ohA + "]", new GUILayoutOption[0]);
                            this.ghA = (this.rhA = (this.yhA = (this.bhA = (this.ohA = GUILayout.HorizontalSlider(this.ohA, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Current Color Selection", this.ohEx, new GUILayoutOption[0]);
                            GUILayout.Space(2);

                            if (GUILayout.Button("Enable All Colors", new GUILayoutOption[0]))
                            {
                                this.ghActive = (this.bhActive = (this.yhActive = (this.rhActive = (this.ohActive = true))));
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Disable All Colors", new GUILayoutOption[0]))
                            {
                                this.ghActive = (this.bhActive = (this.yhActive = (this.rhActive = (this.ohActive = false))));
                            }

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to All Strikes", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 48;
                            }

                            break;
                        case 51:
                            this.menuTitle = "Game Colors [All Strike Head Lights]";

                            GUILayout.Label("Red Value [" + this.ohlR + "]", new GUILayoutOption[0]);
                            this.ghlR = (this.rhlR = (this.yhlR = (this.bhlR = (this.ohlR = GUILayout.HorizontalSlider(this.ohlR, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Green Value [" + this.ohlG + "]", new GUILayoutOption[0]);
                            this.ghlG = (this.rhlG = (this.yhlG = (this.bhlG = (this.ohlG = GUILayout.HorizontalSlider(this.ohlG, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Blue Value [" + this.ohlB + "]", new GUILayoutOption[0]);
                            this.ghlB = (this.rhlB = (this.yhlB = (this.bhlB = (this.ohlB = GUILayout.HorizontalSlider(this.ohlB, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Alpha Value [" + this.ohlA + "]", new GUILayoutOption[0]);
                            this.ghlA = (this.rhlA = (this.yhlA = (this.bhlA = (this.ohlA = GUILayout.HorizontalSlider(this.ohlA, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Current Color Selection", this.ohlEx, new GUILayoutOption[0]);
                            GUILayout.Space(2);

                            if (GUILayout.Button("Enable All Colors", new GUILayoutOption[0]))
                            {
                                this.ghlActive = (this.bhlActive = (this.yhlActive = (this.rhlActive = (this.ohlActive = true))));
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Disable All Colors", new GUILayoutOption[0]))
                            {
                                this.ghlActive = (this.bhlActive = (this.yhlActive = (this.rhlActive = (this.ohlActive = false))));
                            }

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to All Strikes", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 48;
                            }

                            break;
                        case 52:
                            this.menuTitle = "Save/Load Profiles";

                            GUILayout.Label("All Profiles", new GUILayoutOption[0]);
                            this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, new GUILayoutOption[0]);

                            foreach (string path in Directory.GetFiles(Environment.CurrentDirectory + "/Tweaks/Config/GC Profiles/"))
                            {
                                if (GUILayout.Button("Load " + Path.GetFileName(path), new GUILayoutOption[0]))
                                {
                                    this.profileName = Path.GetFileName(path).Remove(Path.GetFileName(path).Length - 4);
                                    this.LoadConfig(Path.GetFileName(path));
                                }

                                GUILayout.Space(2);
                            }

                            GUILayout.Label("Save Profile As", new GUILayoutOption[0]);
                            this.profileName = GUILayout.TextField(this.profileName, new GUILayoutOption[0]);

                            GUILayout.Space(2);

                            this.saveAsDefault = GUILayout.Toggle(this.saveAsDefault, "Load as default profile", new GUILayoutOption[0]);

                            GUILayout.Space(2);

                            if (GUILayout.Button("Save Profile", new GUILayoutOption[0]))
                            {
                                this.SaveConfig(this.profileName);
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Main Menu", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 0;
                            }

                            GUILayout.EndScrollView();
                            break;
                        case 53:
                            this.menuTitle = "Background Options";

                            GUILayout.Label("All Images", new GUILayoutOption[0]);
                            this.bgScrollPos = GUILayout.BeginScrollView(this.bgScrollPos, new GUILayoutOption[0]);

                            foreach (string text in Directory.GetFiles(Environment.CurrentDirectory + "/Custom/Image Backgrounds/"))
                            {
                                if (Path.GetFileName(text).ToLower().Contains(".jpg") || Path.GetFileName(text).ToLower().Contains(".png"))
                                {
                                    if (Path.GetFileName(text).ToLower().Contains(".jpg"))
                                    {
                                        if (GUILayout.Button("Apply " + Path.GetFileName(text), new GUILayoutOption[0]))
                                        {
                                            this.currBgFile = text;
                                            this.bgSprite = this.CreateSpriteFromTex(text);
                                            this.bgPersist = true;

                                            if (this.bgPersist && this.imageComps[0] != null)
                                            {
                                                this.imageComps[0].gameObject.SetActive(true);
                                                this.imageComps[0].enabled = true;
                                                this.imageComps[0].sprite = this.bgSprite;
                                            }
                                        }
                                    }
                                    else if (GUILayout.Button("Apply " + Path.GetFileName(text), new GUILayoutOption[0]))
                                    {
                                        this.currBgFile = text;
                                        this.bgSprite = this.CreateSpriteFromTex(text);
                                        this.bgPersist = true;

                                        if (this.bgPersist && this.imageComps[0] != null)
                                        {
                                            this.imageComps[0].gameObject.SetActive(true);
                                            this.imageComps[0].enabled = true;
                                            this.imageComps[0].sprite = this.bgSprite;
                                        }
                                    }

                                    GUILayout.Space(2);
                                }
                            }

                            this.bgSaveProfile = GUILayout.Toggle(this.bgSaveProfile, "Save to Profile", new GUILayoutOption[0]);

                            GUILayout.Space(2);

                            if (GUILayout.Button("Save Profile (" + this.profileName + ")", new GUILayoutOption[0]))
                            {
                                this.SaveConfig(this.profileName);
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to UI Modifications", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 76;
                            }

                            GUILayout.EndScrollView();
                            break;
                        case 54:
                            this.menuTitle = "Highway Options";

                            GUILayout.Label("All Images", new GUILayoutOption[0]);
                            this.hwScrollPos = GUILayout.BeginScrollView(this.hwScrollPos, new GUILayoutOption[0]);

                            foreach (string text2 in Directory.GetFiles(Environment.CurrentDirectory + "/Custom/Highways/"))
                            {
                                if (Path.GetFileName(text2).ToLower().Contains(".jpg") || Path.GetFileName(text2).ToLower().Contains(".png"))
                                {
                                    if (Path.GetFileName(text2).ToLower().Contains(".jpg"))
                                    {
                                        if (GUILayout.Button("Apply " + Path.GetFileName(text2), new GUILayoutOption[0]))
                                        {
                                            this.currHwFile = text2;
                                            this.hwTex = this.TexFromFile(text2);
                                            this.hwPersist = true;

                                            if (this.hwPersist && this.renders[0] != null)
                                            {
                                                this.renders[0].gameObject.SetActive(true);
                                                this.renders[0].material.mainTexture = this.hwTex;
                                            }
                                        }
                                    }
                                    else if (GUILayout.Button("Apply " + Path.GetFileName(text2), new GUILayoutOption[0]))
                                    {
                                        this.currHwFile = text2;
                                        this.hwTex = this.TexFromFile(text2);
                                        this.hwPersist = true;

                                        if (this.hwPersist && this.renders[0] != null)
                                        {
                                            this.renders[0].gameObject.SetActive(true);
                                            this.renders[0].material.mainTexture = this.hwTex;
                                        }
                                    }

                                    GUILayout.Space(2);
                                }
                            }

                            this.hwSaveProfile = GUILayout.Toggle(this.hwSaveProfile, "Save to Profile", new GUILayoutOption[0]);

                            GUILayout.Space(2);

                            if (GUILayout.Button("Save Profile (" + this.profileName + ")", new GUILayoutOption[0]))
                            {
                                this.SaveConfig(this.profileName);
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to UI Modifications", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 76;
                            }

                            GUILayout.EndScrollView();
                            break;
                        case 55:
                            this.menuTitle = "Game Colors [UI Elements]";

                            if (GUILayout.Button("Multiplier Number", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 56;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Multiplier Counter", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 57;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Song Progress Bar", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 58;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Star Progress Bar", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 59;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Outer Star", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 60;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Inner Star", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 61;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Star Count Number", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 62;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("FC Indicator", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 63;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Score & Combo Text", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 84;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to UI Modifications", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 76;
                            }

                            break;
                        case 56:
                            this.menuTitle = "Game Colors [Multiplier Number]";

                            GUILayout.Label("Red Value [" + this.mnR + "]", new GUILayoutOption[0]);
                            this.mnR = GUILayout.HorizontalSlider(this.mnR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.mnG + "]", new GUILayoutOption[0]);
                            this.mnG = GUILayout.HorizontalSlider(this.mnG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.mnB + "]", new GUILayoutOption[0]);
                            this.mnB = GUILayout.HorizontalSlider(this.mnB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.mnA + "]", new GUILayoutOption[0]);
                            this.mnA = GUILayout.HorizontalSlider(this.mnA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.mnEx, new GUILayoutOption[0]);
                            this.mnActive = GUILayout.Toggle(this.mnActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Various UI Elements", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 55;
                            }

                            break;
                        case 57:
                            this.menuTitle = "Game Colors [Multiplier Counter]";

                            GUILayout.Label("Red Value [" + this.mcR + "]", new GUILayoutOption[0]);
                            this.mcR = GUILayout.HorizontalSlider(this.mcR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.mcG + "]", new GUILayoutOption[0]);
                            this.mcG = GUILayout.HorizontalSlider(this.mcG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.mcB + "]", new GUILayoutOption[0]);
                            this.mcB = GUILayout.HorizontalSlider(this.mcB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.mcA + "]", new GUILayoutOption[0]);
                            this.mcA = GUILayout.HorizontalSlider(this.mcA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.mcEx, new GUILayoutOption[0]);
                            this.mcActive = GUILayout.Toggle(this.mcActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Various UI Elements", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 55;
                            }

                            break;
                        case 58:
                            this.menuTitle = "Game Colors [Song Progress Bar]";

                            GUILayout.Label("Red Value [" + this.songpbR + "]", new GUILayoutOption[0]);
                            this.songpbR = GUILayout.HorizontalSlider(this.songpbR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.songpbG + "]", new GUILayoutOption[0]);
                            this.songpbG = GUILayout.HorizontalSlider(this.songpbG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.songpbB + "]", new GUILayoutOption[0]);
                            this.songpbB = GUILayout.HorizontalSlider(this.songpbB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.songpbA + "]", new GUILayoutOption[0]);
                            this.songpbA = GUILayout.HorizontalSlider(this.songpbA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.songpbEx, new GUILayoutOption[0]);
                            this.songpbActive = GUILayout.Toggle(this.songpbActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Various UI Elements", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 55;
                            }

                            break;
                        case 59:
                            this.menuTitle = "Game Colors [Star Progress Bar]";

                            GUILayout.Label("Red Value [" + this.starpbR + "]", new GUILayoutOption[0]);
                            this.starpbR = GUILayout.HorizontalSlider(this.starpbR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.starpbG + "]", new GUILayoutOption[0]);
                            this.starpbG = GUILayout.HorizontalSlider(this.starpbG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.starpbB + "]", new GUILayoutOption[0]);
                            this.starpbB = GUILayout.HorizontalSlider(this.starpbB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.starpbA + "]", new GUILayoutOption[0]);
                            this.starpbA = GUILayout.HorizontalSlider(this.starpbA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.starpbEx, new GUILayoutOption[0]);
                            this.starpbActive = GUILayout.Toggle(this.starpbActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Various UI Elements", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 55;
                            }

                            break;
                        case 60:
                            this.menuTitle = "Game Colors [Outer Star]";

                            GUILayout.Label("Red Value [" + this.osR + "]", new GUILayoutOption[0]);
                            this.osR = GUILayout.HorizontalSlider(this.osR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.osG + "]", new GUILayoutOption[0]);
                            this.osG = GUILayout.HorizontalSlider(this.osG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.osB + "]", new GUILayoutOption[0]);
                            this.osB = GUILayout.HorizontalSlider(this.osB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.osA + "]", new GUILayoutOption[0]);
                            this.osA = GUILayout.HorizontalSlider(this.osA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.osEx, new GUILayoutOption[0]);
                            this.osActive = GUILayout.Toggle(this.osActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Various UI Elements", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 55;
                            }

                            break;
                        case 61:
                            this.menuTitle = "Game Colors [Inner Star]";

                            GUILayout.Label("Red Value [" + this.isR + "]", new GUILayoutOption[0]);
                            this.isR = GUILayout.HorizontalSlider(this.isR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.isG + "]", new GUILayoutOption[0]);
                            this.isG = GUILayout.HorizontalSlider(this.isG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.isB + "]", new GUILayoutOption[0]);
                            this.isB = GUILayout.HorizontalSlider(this.isB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.isA + "]", new GUILayoutOption[0]);
                            this.isA = GUILayout.HorizontalSlider(this.isA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.isEx, new GUILayoutOption[0]);
                            this.isActive = GUILayout.Toggle(this.isActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Various UI Elements", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 55;
                            }

                            break;
                        case 62:
                            this.menuTitle = "Game Colors [Star Count Number]";

                            GUILayout.Label("Red Value [" + this.scTextR + "]", new GUILayoutOption[0]);
                            this.scTextR = GUILayout.HorizontalSlider(this.scTextR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.scTextG + "]", new GUILayoutOption[0]);
                            this.scTextG = GUILayout.HorizontalSlider(this.scTextG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.scTextB + "]", new GUILayoutOption[0]);
                            this.scTextB = GUILayout.HorizontalSlider(this.scTextB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.scTextA + "]", new GUILayoutOption[0]);
                            this.scTextA = GUILayout.HorizontalSlider(this.scTextA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.scTextEx, new GUILayoutOption[0]);
                            this.scTextActive = GUILayout.Toggle(this.scTextActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Various UI Elements", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 55;
                            }

                            break;
                        case 63:
                            this.menuTitle = "Game Colors [FC Indicator]";

                            GUILayout.Label("Red Value [" + this.fcR + "]", new GUILayoutOption[0]);
                            this.fcR = GUILayout.HorizontalSlider(this.fcR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.fcG + "]", new GUILayoutOption[0]);
                            this.fcG = GUILayout.HorizontalSlider(this.fcG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.fcB + "]", new GUILayoutOption[0]);
                            this.fcB = GUILayout.HorizontalSlider(this.fcB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.fcA + "]", new GUILayoutOption[0]);
                            this.fcA = GUILayout.HorizontalSlider(this.fcA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.fcEx, new GUILayoutOption[0]);
                            this.fcActive = GUILayout.Toggle(this.fcActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Various UI Elements", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 55;
                            }

                            break;
                        case 64:
                            this.menuTitle = "Game Colors [Open Notes]";

                            GUILayout.Label("Red Value [" + this.onR + "]", new GUILayoutOption[0]);
                            this.onR = GUILayout.HorizontalSlider(this.onR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.onG + "]", new GUILayoutOption[0]);
                            this.onG = GUILayout.HorizontalSlider(this.onG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.onB + "]", new GUILayoutOption[0]);
                            this.onB = GUILayout.HorizontalSlider(this.onB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.onA + "]", new GUILayoutOption[0]);
                            this.onA = GUILayout.HorizontalSlider(this.onA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.onEx, new GUILayoutOption[0]);
                            this.onActive = GUILayout.Toggle(this.onActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Highway Modifications", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 75;
                            }

                            break;
                        case 65:
                            this.menuTitle = "Game Colors [Star Power Notes]";

                            GUILayout.Label("Red Value [" + this.spR + "]", new GUILayoutOption[0]);
                            this.spR = GUILayout.HorizontalSlider(this.spR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.spG + "]", new GUILayoutOption[0]);
                            this.spG = GUILayout.HorizontalSlider(this.spG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.spB + "]", new GUILayoutOption[0]);
                            this.spB = GUILayout.HorizontalSlider(this.spB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.spA + "]", new GUILayoutOption[0]);
                            this.spA = GUILayout.HorizontalSlider(this.spA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.spEx, new GUILayoutOption[0]);

                            if (GUILayout.Button("Enable Star Power Notes", new GUILayoutOption[0]))
                            {
                                this.spActive = true;
                                this.spRB = false;
                                this.LoadNotes();
                            }

                            if (GUILayout.Button("Disable Star Power Notes", new GUILayoutOption[0]))
                            {
                                this.spActive = false;
                                this.LoadNotes();
                            }

                            if (GUILayout.Button("Enable Star Power Rainbow", new GUILayoutOption[0]))
                            {
                                this.spRB = true;
                                this.spActive = false;
                            }

                            if (GUILayout.Button("Disable Star Power Rainbow", new GUILayoutOption[0]))
                            {
                                this.spRB = false;
                            }

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Star Power", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 8;
                            }

                            break;
                        case 66:
                            this.menuTitle = "Game Colors [Star Power Highway]";

                            GUILayout.Label("Red Value [" + this.spGlowR + "]", new GUILayoutOption[0]);
                            this.spGlowR = GUILayout.HorizontalSlider(this.spGlowR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.spGlowG + "]", new GUILayoutOption[0]);
                            this.spGlowG = GUILayout.HorizontalSlider(this.spGlowG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.spGlowB + "]", new GUILayoutOption[0]);
                            this.spGlowB = GUILayout.HorizontalSlider(this.spGlowB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.spGlowEx, new GUILayoutOption[0]);
                            this.spGlowActive = GUILayout.Toggle(this.spGlowActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Star Power", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 8;
                            }

                            break;
                        case 67:
                            this.menuTitle = "Game Colors [Green Note Particles]";

                            GUILayout.Label("Red Value [" + this.particleGR + "]", new GUILayoutOption[0]);
                            this.particleGR = GUILayout.HorizontalSlider(this.particleGR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.particleGG + "]", new GUILayoutOption[0]);
                            this.particleGG = GUILayout.HorizontalSlider(this.particleGG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.particleGB + "]", new GUILayoutOption[0]);
                            this.particleGB = GUILayout.HorizontalSlider(this.particleGB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.particleGA + "]", new GUILayoutOption[0]);
                            this.particleGA = GUILayout.HorizontalSlider(this.particleGA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.pgEx, new GUILayoutOption[0]);
                            this.pgActive = GUILayout.Toggle(this.pgActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Particles", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 73;
                            }

                            break;
                        case 68:
                            this.menuTitle = "Game Colors [Red Note Particles]";

                            GUILayout.Label("Red Value [" + this.particleRR + "]", new GUILayoutOption[0]);
                            this.particleRR = GUILayout.HorizontalSlider(this.particleRR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.particleRG + "]", new GUILayoutOption[0]);
                            this.particleRG = GUILayout.HorizontalSlider(this.particleRG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.particleRB + "]", new GUILayoutOption[0]);
                            this.particleRB = GUILayout.HorizontalSlider(this.particleRB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.particleRA + "]", new GUILayoutOption[0]);
                            this.particleRA = GUILayout.HorizontalSlider(this.particleRA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.prEx, new GUILayoutOption[0]);
                            this.prActive = GUILayout.Toggle(this.prActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Particles", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 73;
                            }

                            break;
                        case 69:
                            this.menuTitle = "Game Colors [Yellow Note Particles]";

                            GUILayout.Label("Red Value [" + this.particleYR + "]", new GUILayoutOption[0]);
                            this.particleYR = GUILayout.HorizontalSlider(this.particleYR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.particleYG + "]", new GUILayoutOption[0]);
                            this.particleYG = GUILayout.HorizontalSlider(this.particleYG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.particleYB + "]", new GUILayoutOption[0]);
                            this.particleYB = GUILayout.HorizontalSlider(this.particleYB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.particleYA + "]", new GUILayoutOption[0]);
                            this.particleYA = GUILayout.HorizontalSlider(this.particleYA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.pyEx, new GUILayoutOption[0]);
                            this.pyActive = GUILayout.Toggle(this.pyActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Particles", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 73;
                            }

                            break;
                        case 70:
                            this.menuTitle = "Game Colors [Blue Note Particles]";

                            GUILayout.Label("Red Value [" + this.particleBR + "]", new GUILayoutOption[0]);
                            this.particleBR = GUILayout.HorizontalSlider(this.particleBR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.particleBG + "]", new GUILayoutOption[0]);
                            this.particleBG = GUILayout.HorizontalSlider(this.particleBG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.particleBB + "]", new GUILayoutOption[0]);
                            this.particleBB = GUILayout.HorizontalSlider(this.particleBB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.particleBA + "]", new GUILayoutOption[0]);
                            this.particleBA = GUILayout.HorizontalSlider(this.particleBA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.pbEx, new GUILayoutOption[0]);
                            this.pbActive = GUILayout.Toggle(this.pbActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Particles", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 73;
                            }

                            break;
                        case 71:
                            this.menuTitle = "Game Colors [Orange Note Particles]";

                            GUILayout.Label("Red Value [" + this.particleOR + "]", new GUILayoutOption[0]);
                            this.particleOR = GUILayout.HorizontalSlider(this.particleOR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.particleOG + "]", new GUILayoutOption[0]);
                            this.particleOG = GUILayout.HorizontalSlider(this.particleOG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.particleOB + "]", new GUILayoutOption[0]);
                            this.particleOB = GUILayout.HorizontalSlider(this.particleOB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.particleOA + "]", new GUILayoutOption[0]);
                            this.particleOA = GUILayout.HorizontalSlider(this.particleOA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.poEx, new GUILayoutOption[0]);
                            this.poActive = GUILayout.Toggle(this.poActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Particles", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 73;
                            }

                            break;
                        case 72:
                            this.menuTitle = "Game Colors [All Note Particles]";

                            GUILayout.Label("Red Value [" + this.particleOR + "]", new GUILayoutOption[0]);
                            this.particleGR = (this.particleRR = (this.particleYR = (this.particleBR = (this.particleOR = GUILayout.HorizontalSlider(this.particleOR, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Green Value [" + this.particleOG + "]", new GUILayoutOption[0]);
                            this.particleGG = (this.particleRG = (this.particleYG = (this.particleBG = (this.particleOG = GUILayout.HorizontalSlider(this.particleOG, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Blue Value [" + this.particleOB + "]", new GUILayoutOption[0]);
                            this.particleGB = (this.particleRB = (this.particleYB = (this.particleBB = (this.particleOB = GUILayout.HorizontalSlider(this.particleOB, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Alpha Value [" + this.particleOA + "]", new GUILayoutOption[0]);
                            this.particleGA = (this.particleRA = (this.particleYA = (this.particleBA = (this.particleOA = GUILayout.HorizontalSlider(this.particleOA, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Current Color Selection", this.poEx, new GUILayoutOption[0]);
                            this.pgActive = (this.prActive = (this.pyActive = (this.pbActive = (this.poActive = GUILayout.Toggle(this.poActive, "Color Enabled", new GUILayoutOption[0])))));

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Particles", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 73;
                            }

                            break;
                        case 73:
                            this.menuTitle = "Game Colors [Note Particles]";

                            if (GUILayout.Button("Green Note Particles", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 67;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Red Note Particles", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 68;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Yellow Note Particles", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 69;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Blue Note Particles", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 70;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Orange Note Particles", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 71;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("All Note Particles", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 72;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Note Particle Settings", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 74;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Highway Modifications", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 75;
                            }

                            break;
                        case 74:
                            this.menuTitle = "Game Colors [Note Particles Settings]";

                            GUILayout.Label("Particle Size [" + this.particleSize + "]", new GUILayoutOption[0]);
                            this.particleSize = GUILayout.HorizontalSlider(this.particleSize, 0, 1, new GUILayoutOption[0]);

                            GUILayout.Label("Particle Gravity [" + this.particleGravity + "]", new GUILayoutOption[0]);
                            this.particleGravity = GUILayout.HorizontalSlider(this.particleGravity, 0, 1, new GUILayoutOption[0]);

                            GUILayout.Label("Particle Speed [" + this.particleSpeed + "]", new GUILayoutOption[0]);
                            this.particleSpeed = GUILayout.HorizontalSlider(this.particleSpeed, 0, 5, new GUILayoutOption[0]);

                            GUILayout.Label("Max Particles [" + this.particleMax + "]", new GUILayoutOption[0]);
                            this.particleMax = GUILayout.HorizontalSlider(this.particleMax, 0, 1000000, new GUILayoutOption[0]);
                            this.particleMax = (float)Math.Round(particleMax);

                            this.particleSettingsActive = GUILayout.Toggle(this.particleSettingsActive, "Settings Enabled", new GUILayoutOption[0]);
                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Particles", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 73;
                            }

                            break;
                        case 75:
                            this.menuTitle = "Game Colors [Highway Modifications]";

                            if (GUILayout.Button("Note Colors", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 1;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Star Power Colors", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 8;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Flame Colors", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 9;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Lightning Colors", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 16;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("SP Bar Colors", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 23;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Strikeline Colors", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 27;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Strikeline Strings", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 77;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Note Particles", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 73;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Highway Sides", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 101;
                            }

                            GUILayout.Space(2);
                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Main Menu", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 0;
                            }

                            break;
                        case 76:
                            this.menuTitle = "Game Colors [UI Modifications]";

                            GUILayout.Space(2);

                            if (GUILayout.Button("UI Elements", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 55;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Main Menu", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 0;
                            }

                            break;
                        case 77:
                            this.menuTitle = "Game Colors [Strikeline Strings]";

                            if (GUILayout.Button("Green Note String", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 78;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Red Note String", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 79;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Yellow Note String", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 80;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Blue Note String", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 81;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Orange Note String", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 82;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("All Note Strings", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 83;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Highway Modifications", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 75;
                            }

                            break;
                        case 78:
                            this.menuTitle = "Game Colors [Green Strikeline String]";

                            GUILayout.Label("Red Value [" + this.strikelGR + "]", new GUILayoutOption[0]);
                            this.strikelGR = GUILayout.HorizontalSlider(this.strikelGR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.strikelGG + "]", new GUILayoutOption[0]);
                            this.strikelGG = GUILayout.HorizontalSlider(this.strikelGG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.strikelGB + "]", new GUILayoutOption[0]);
                            this.strikelGB = GUILayout.HorizontalSlider(this.strikelGB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.strikelGA + "]", new GUILayoutOption[0]);
                            this.strikelGA = GUILayout.HorizontalSlider(this.strikelGA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.strikelGEx, new GUILayoutOption[0]);
                            this.strikeGActive = GUILayout.Toggle(this.strikeGActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Strikeline Strings", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 77;
                            }

                            break;
                        case 79:
                            this.menuTitle = "Game Colors [Red Strikeline String]";

                            GUILayout.Label("Red Value [" + this.strikelRR + "]", new GUILayoutOption[0]);
                            this.strikelRR = GUILayout.HorizontalSlider(this.strikelRR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.strikelRG + "]", new GUILayoutOption[0]);
                            this.strikelRG = GUILayout.HorizontalSlider(this.strikelRG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.strikelRB + "]", new GUILayoutOption[0]);
                            this.strikelRB = GUILayout.HorizontalSlider(this.strikelRB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.strikelRA + "]", new GUILayoutOption[0]);
                            this.strikelRA = GUILayout.HorizontalSlider(this.strikelRA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.strikelREx, new GUILayoutOption[0]);
                            this.strikeRActive = GUILayout.Toggle(this.strikeRActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Strikeline Strings", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 77;
                            }

                            break;
                        case 80:
                            this.menuTitle = "Game Colors [Yellow Strikeline String]";

                            GUILayout.Label("Red Value [" + this.strikelYR + "]", new GUILayoutOption[0]);
                            this.strikelYR = GUILayout.HorizontalSlider(this.strikelYR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.strikelYG + "]", new GUILayoutOption[0]);
                            this.strikelYG = GUILayout.HorizontalSlider(this.strikelYG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.strikelYB + "]", new GUILayoutOption[0]);
                            this.strikelYB = GUILayout.HorizontalSlider(this.strikelYB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.strikelYA + "]", new GUILayoutOption[0]);
                            this.strikelYA = GUILayout.HorizontalSlider(this.strikelYA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.strikelYEx, new GUILayoutOption[0]);
                            this.strikeYActive = GUILayout.Toggle(this.strikeYActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Strikeline Strings", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 77;
                            }

                            break;
                        case 81:
                            this.menuTitle = "Game Colors [Blue Strikeline String]";

                            GUILayout.Label("Red Value [" + this.strikelBR + "]", new GUILayoutOption[0]);
                            this.strikelBR = GUILayout.HorizontalSlider(this.strikelBR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.strikelBG + "]", new GUILayoutOption[0]);
                            this.strikelBG = GUILayout.HorizontalSlider(this.strikelBG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.strikelBB + "]", new GUILayoutOption[0]);
                            this.strikelBB = GUILayout.HorizontalSlider(this.strikelBB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.strikelBA + "]", new GUILayoutOption[0]);
                            this.strikelBA = GUILayout.HorizontalSlider(this.strikelBA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.strikelBEx, new GUILayoutOption[0]);
                            this.strikeBActive = GUILayout.Toggle(this.strikeBActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Strikeline Strings", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 77;
                            }

                            break;
                        case 82:
                            this.menuTitle = "Game Colors [Orange Strikeline String]";

                            GUILayout.Label("Red Value [" + this.strikelOR + "]", new GUILayoutOption[0]);
                            this.strikelOR = GUILayout.HorizontalSlider(this.strikelOR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.strikelOG + "]", new GUILayoutOption[0]);
                            this.strikelOG = GUILayout.HorizontalSlider(this.strikelOG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.strikelOB + "]", new GUILayoutOption[0]);
                            this.strikelOB = GUILayout.HorizontalSlider(this.strikelOB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Alpha Value [" + this.strikelOA + "]", new GUILayoutOption[0]);
                            this.strikelOA = GUILayout.HorizontalSlider(this.strikelOA, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.strikelOEx, new GUILayoutOption[0]);
                            this.strikeOActive = GUILayout.Toggle(this.strikeOActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Strikeline Strings", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 77;
                            }

                            break;
                        case 83:
                            this.menuTitle = "Game Colors [All Strikeline Strings]";

                            GUILayout.Label("Red Value [" + this.strikelGR + "]", new GUILayoutOption[0]);
                            this.strikelGR = (this.strikelRR = (this.strikelYR = (this.strikelBR = (this.strikelOR = GUILayout.HorizontalSlider(this.strikelGR, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Green Value [" + this.strikelGG + "]", new GUILayoutOption[0]);
                            this.strikelGG = (this.strikelRG = (this.strikelYG = (this.strikelBG = (this.strikelOG = GUILayout.HorizontalSlider(this.strikelGG, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Blue Value [" + this.strikelGB + "]", new GUILayoutOption[0]);
                            this.strikelGB = (this.strikelRB = (this.strikelYB = (this.strikelBB = (this.strikelOB = GUILayout.HorizontalSlider(this.strikelGB, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Alpha Value [" + this.strikelGA + "]", new GUILayoutOption[0]);
                            this.strikelGA = (this.strikelRA = (this.strikelYA = (this.strikelBA = (this.strikelOA = GUILayout.HorizontalSlider(this.strikelGA, 0, 255, new GUILayoutOption[0])))));

                            GUILayout.Label("Current Color Selection", this.strikelGEx, new GUILayoutOption[0]);

                            if (GUILayout.Button("Enable All Colors", new GUILayoutOption[0]))
                            {
                                this.strikeGActive = (this.strikeRActive = (this.strikeYActive = (this.strikeBActive = (this.strikeOActive = true))));
                            }

                            if (GUILayout.Button("Disable All Colors", new GUILayoutOption[0]))
                            {
                                this.strikeGActive = (this.strikeRActive = (this.strikeYActive = (this.strikeBActive = (this.strikeOActive = false))));
                            }

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Strikeline Strings", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 77;
                            }

                            break;
                        case 84:
                            this.menuTitle = "Game Colors [Score & Combo Text]";

                            if (GUILayout.Button("Score Text Colors", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 85;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Combo Text Colors", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 86;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Back to Various UI Elements", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 55;
                            }

                            break;
                        case 85:
                            this.menuTitle = "Game Colors [Score Text Colors]";

                            GUILayout.Label("Red Value [" + this.scoreFontR + "]", new GUILayoutOption[0]);
                            this.scoreFontR = GUILayout.HorizontalSlider(this.scoreFontR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.scoreFontG + "]", new GUILayoutOption[0]);
                            this.scoreFontG = GUILayout.HorizontalSlider(this.scoreFontG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.scoreFontB + "]", new GUILayoutOption[0]);
                            this.scoreFontB = GUILayout.HorizontalSlider(this.scoreFontB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.scoreFontEx, new GUILayoutOption[0]);
                            this.scoreFontActive = GUILayout.Toggle(this.scoreFontActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Score & Combo Text", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 84;
                            }

                            break;
                        case 86:
                            this.menuTitle = "Game Colors [Combo Text Colors]";

                            GUILayout.Label("Red Value [" + this.comboFontR + "]", new GUILayoutOption[0]);
                            this.comboFontR = GUILayout.HorizontalSlider(this.comboFontR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.comboFontG + "]", new GUILayoutOption[0]);
                            this.comboFontG = GUILayout.HorizontalSlider(this.comboFontG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.comboFontB + "]", new GUILayoutOption[0]);
                            this.comboFontB = GUILayout.HorizontalSlider(this.comboFontB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.comboFontEx, new GUILayoutOption[0]);
                            this.comboFontActive = GUILayout.Toggle(this.comboFontActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Score & Combo Text", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 84;
                            }

                            break;
                        case 99:
                            this.menuTitle = "Game Colors [Menu Settings]";

                            if (GUILayout.Button("Menu Themes", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 100;
                            }

                            if (GUILayout.Button(this.keyButtonText, new GUILayoutOption[0]))
                            {
                                this.isSettingKey = !this.isSettingKey;
                            }

                            if (this.isSettingKey)
                            {
                                GUILayout.Label("Press the button when done.", new GUILayoutOption[0]);

                                if (Event.current.isKey)
                                {
                                    KeyBindConfig.WriteToConfig(Event.current.keyCode);
                                }
                            }

                            GUILayout.Label("Current Key: " + KeyBindConfig.menuKey.ToString(), new GUILayoutOption[0]);

                            if (GUILayout.Button("Back to Main Menu", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 0;
                            }

                            break;
                        case 100:
                            if (GUILayout.Button("Set Default Menu Theme", new GUILayoutOption[0]))
                            {
                                this.SetTheme(0);
                                this.currentThemeIndex = 0;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Set Green Menu Theme", new GUILayoutOption[0]))
                            {
                                this.SetTheme(1);
                                this.currentThemeIndex = 1;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Set Purple Menu Theme", new GUILayoutOption[0]))
                            {
                                this.SetTheme(2);
                                this.currentThemeIndex = 2;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Set Orange Menu Theme", new GUILayoutOption[0]))
                            {
                                this.SetTheme(3);
                                this.currentThemeIndex = 3;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Set Red Menu Theme", new GUILayoutOption[0]))
                            {
                                this.SetTheme(4);
                                this.currentThemeIndex = 4;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Set Pink Menu Theme", new GUILayoutOption[0]))
                            {
                                this.SetTheme(5);
                                this.currentThemeIndex = 5;
                            }

                            GUILayout.Space(2);

                            if (GUILayout.Button("Set Gray Menu Theme", new GUILayoutOption[0]))
                            {
                                this.SetTheme(6);
                                this.currentThemeIndex = 6;
                            }

                            GUILayout.Space(2);

                            this.saveMenuTheme = GUILayout.Toggle(this.saveMenuTheme, "Set as Default", new GUILayoutOption[0]);
                            GUILayout.Label("Default menu theme saves to your profile on save!", new GUILayoutOption[0]);

                            if (GUILayout.Button("Back to Menu Settings", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 99;
                            }

                            break;
                        case 101:
                            this.menuTitle = "Game Colors [Highway Sides]";

                            GUILayout.Label("Red Value [" + this.sR + "]", new GUILayoutOption[0]);
                            this.sR = GUILayout.HorizontalSlider(this.sR, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Green Value [" + this.sG + "]", new GUILayoutOption[0]);
                            this.sG = GUILayout.HorizontalSlider(this.sG, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Blue Value [" + this.sB + "]", new GUILayoutOption[0]);
                            this.sB = GUILayout.HorizontalSlider(this.sB, 0, 255, new GUILayoutOption[0]);

                            GUILayout.Label("Current Color Selection", this.sEx, new GUILayoutOption[0]);
                            this.sActive = GUILayout.Toggle(this.sActive, "Color Enabled", new GUILayoutOption[0]);

                            GUILayout.Space(5);

                            if (GUILayout.Button("Back to Highway Modifications", new GUILayoutOption[0]))
                            {
                                this.menuIndex = 75;
                            }

                            break;
                    }
                }
            }
            catch
            {
            }

            GUI.DragWindow();
        }

        private void SetTheme(int id)
        {
            switch (id)
            {
                case 0:
                    this.grayTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(45, 45, 45, 255));
                    this.buttonTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(0, 152, 204, 255));
                    this.buttonPressTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(0, 122, 163, 255));
                    this.buttonHoverTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(0, 176, 235, 255));

                    return;
                case 1:
                    this.grayTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(45, 45, 45, 255));
                    this.buttonTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(0, 168, 28, 255));
                    this.buttonPressTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(0, 161, 24, 255));
                    this.buttonHoverTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(56, 201, 56, 255));

                    return;
                case 2:
                    this.grayTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(45, 45, 45, 255));
                    this.buttonTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(108, 0, 196, 255));
                    this.buttonPressTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(77, 0, 140, 255));
                    this.buttonHoverTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(117, 57, 247, 255));

                    return;
                case 3:
                    this.grayTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(45, 45, 45, 255));
                    this.buttonTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(255, 77, 0, 255));
                    this.buttonPressTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(207, 62, 0, 255));
                    this.buttonHoverTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(255, 119, 61, 255));

                    return;
                case 4:
                    this.grayTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(45, 45, 45, 255));
                    this.buttonTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(217, 0, 0, 255));
                    this.buttonPressTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(163, 0, 0, 255));
                    this.buttonHoverTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(255, 0, 0, 255));

                    return;
                case 5:
                    this.grayTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(45, 45, 45, 255));
                    this.buttonTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(255, 0, 195, 255));
                    this.buttonPressTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(207, 0, 158, 255));
                    this.buttonHoverTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(252, 76, 211, 255));

                    return;
                case 6:
                    this.grayTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(45, 45, 45, 255));
                    this.buttonTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(95, 95, 95, 255));
                    this.buttonPressTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(80, 80, 80, 255));
                    this.buttonHoverTex = Helpers.MakeTex(2, 2, Helpers.RGBtoUnity(110, 110, 110, 255));

                    return;
                default:
                    return;
            }
        }

        private void SetSkins()
        {
            GUI.skin.window.onNormal.background = this.grayTex;
            GUI.skin.window.onNormal.textColor = Color.white;

            GUI.skin.window.onFocused.background = this.grayTex;
            GUI.skin.window.onFocused.textColor = Color.white;

            GUI.skin.window.onActive.background = this.grayTex;
            GUI.skin.window.onActive.textColor = Color.white;

            GUI.skin.window.onHover.background = this.grayTex;
            GUI.skin.window.onHover.textColor = Color.white;

            GUI.skin.window.focused.background = this.grayTex;
            GUI.skin.window.focused.textColor = Color.white;

            GUI.skin.window.hover.background = this.grayTex;
            GUI.skin.window.hover.textColor = Color.white;

            GUI.skin.window.active.background = this.grayTex;
            GUI.skin.window.active.textColor = Color.white;

            GUI.skin.window.normal.background = this.grayTex;
            GUI.skin.window.normal.textColor = Color.white;

            GUI.skin.button.normal.background = this.buttonTex;
            GUI.skin.button.normal.textColor = Color.white;

            GUI.skin.button.onNormal.background = this.buttonTex;
            GUI.skin.button.onNormal.textColor = Color.white;

            GUI.skin.button.hover.background = this.buttonHoverTex;
            GUI.skin.button.hover.textColor = Color.white;

            GUI.skin.button.onHover.background = this.buttonHoverTex;
            GUI.skin.button.onHover.textColor = Color.white;

            GUI.skin.button.focused.background = this.buttonTex;
            GUI.skin.button.focused.textColor = Color.white;

            GUI.skin.button.onFocused.background = this.buttonTex;
            GUI.skin.button.onFocused.textColor = Color.white;

            GUI.skin.button.active.background = this.buttonPressTex;
            GUI.skin.button.active.textColor = Color.white;

            GUI.skin.button.onActive.background = this.buttonPressTex;
            GUI.skin.button.onActive.textColor = Color.white;

            GUI.skin.button.fixedHeight = 25;
            GUI.skin.textField.fixedWidth = this.windowRect.width;
        }

        private readonly string configLoc = Environment.CurrentDirectory + "/Tweaks/Config/cfg-GameColors.txt";

        private Rect windowRect = new Rect(Screen.width / 3f, Screen.height / 3f, 300, 400);

        private int menuIndex;

        private bool menuLoaded;

        private string menuTitle = "Game Colors [Main]";

        private Texture2D grayTex;

        private Texture2D buttonTex;

        private Texture2D buttonPressTex;

        private Texture2D buttonHoverTex;

        private Texture2D greenScreen;

        private Texture2D clearScreen;

        private float timeleft;

        private float accum;

        private int frames = 60;

        private bool fpsCounter;

        private GUIStyle fpsCount = new GUIStyle();

        private bool isInEditMode;

        private string debug = "";

        private string profileName = "default";

        private bool saveAsDefault;

        private string keyButtonText = "Change Open Keybind";

        private int currentThemeIndex;

        private Vector2 scrollPos = Vector2.zero;

        private Vector2 bgScrollPos = Vector2.zero;

        private Vector2 hwScrollPos = Vector2.zero;

        private string currBgFile;

        private string currHwFile;

        private Sprite bgSprite;

        private Texture2D hwTex;

        private bool bgPersist;

        private bool hwPersist;

        private bool bgSaveProfile;

        private bool hwSaveProfile;

        private Assembly asm;

        private GameObject[] flames = new GameObject[5];

        private GameObject[] lightning = new GameObject[5];

        private SpriteRenderer[] flameRenders = new SpriteRenderer[5];

        private SpriteRenderer[] lightningRenders = new SpriteRenderer[5];

        private Image[] imageComps = new Image[1];

        private Renderer[] renders = new Renderer[2];

        private List<SpriteRenderer> strikeStrings = new List<SpriteRenderer>();

        private SpriteRenderer[] spBars = new SpriteRenderer[7];

        private TweakMain.FretCover[] fretCovers = new TweakMain.FretCover[5];

        private TweakMain.FretHead[] fretHeads = new TweakMain.FretHead[5];

        private Color defaultSpColor = new Color(0, 1, 1, 1);

        private GUIStyle greenEx;

        private GUIStyle redEx;

        private GUIStyle yellowEx;

        private GUIStyle blueEx;

        private GUIStyle orangeEx;

        private GUIStyle spEx;

        private GUIStyle gfEx;

        private GUIStyle rfEx;

        private GUIStyle yfEx;

        private GUIStyle bfEx;

        private GUIStyle ofEx;

        private GUIStyle glEx;

        private GUIStyle rlEx;

        private GUIStyle ylEx;

        private GUIStyle blEx;

        private GUIStyle olEx;

        private GUIStyle centerText;

        private GUIStyle spblEx;

        private GUIStyle spbuEx;

        private GUIStyle spbLtEx;

        private GUIStyle onEx;

        private GUIStyle sEx;

        private bool redActive;

        private bool greenActive;

        private bool yellowActive;

        private bool blueActive;

        private bool orangeActive;

        private bool spActive;

        private bool gfActive;

        private bool rfActive;

        private bool yfActive;

        private bool bfActive;

        private bool ofActive;

        private bool glActive;

        private bool rlActive;

        private bool ylActive;

        private bool blActive;

        private bool olActive;

        private bool onActive;

        private bool spblActive;

        private bool spbuActive;

        private bool spbLtActive;

        private bool spblRB;

        private bool spbuRB;

        private bool spbLtRB;

        private bool greenRB;

        private bool redRB;

        private bool yellowRB;

        private bool blueRB;

        private bool orangeRB;

        private bool spRB;

        private bool gfRB;

        private bool rfRB;

        private bool yfRB;

        private bool bfRB;

        private bool ofRB;

        private bool glRB;

        private bool rlRB;

        private bool ylRB;

        private bool blRB;

        private bool olRB;

        private float greenR;

        private float greenG;

        private float greenB;

        private float greenA = 255;

        private float redR;

        private float redG;

        private float redB;

        private float redA = 255;

        private float yellowR;

        private float yellowG;

        private float yellowB;

        private float yellowA = 255;

        private float blueR;

        private float blueG;

        private float blueB;

        private float blueA = 255;

        private float orangeR;

        private float orangeG;

        private float orangeB;

        private float orangeA = 255;

        private float spR;

        private float spG;

        private float spB;

        private float spA = 255;

        private float gfR;

        private float gfG;

        private float gfB;

        private float gfA = 255;

        private float rfR;

        private float rfG;

        private float rfB;

        private float rfA = 255;

        private float yfR;

        private float yfG;

        private float yfB;

        private float yfA = 255;

        private float bfR;

        private float bfG;

        private float bfB;

        private float bfA = 255;

        private float ofR;

        private float ofG;

        private float ofB;

        private float ofA = 255;

        private float glR;

        private float glG;

        private float glB;

        private float glA = 255;

        private float rlR;

        private float rlG;

        private float rlB;

        private float rlA = 255;

        private float ylR;

        private float ylG;

        private float ylB;

        private float ylA = 255;

        private float blR;

        private float blG;

        private float blB;

        private float blA = 255;

        private float olR;

        private float olG;

        private float olB;

        private float olA = 255;

        private float spblR;

        private float spblG;

        private float spblB;

        private float spblA = 255;

        private float spbuR;

        private float spbuG;

        private float spbuB;

        private float spbuA = 255;

        private float spbLtR;

        private float spbLtG;

        private float spbLtB;

        private float spbLtA = 255;

        private GUIStyle gcEx;

        private GUIStyle rcEx;

        private GUIStyle ycEx;

        private GUIStyle bcEx;

        private GUIStyle ocEx;

        private GUIStyle ghEx;

        private GUIStyle rhEx;

        private GUIStyle yhEx;

        private GUIStyle bhEx;

        private GUIStyle ohEx;

        private GUIStyle ghlEx;

        private GUIStyle rhlEx;

        private GUIStyle yhlEx;

        private GUIStyle bhlEx;

        private GUIStyle ohlEx;

        private GUIStyle spGlowEx;

        private bool gcActive;

        private bool rcActive;

        private bool ycActive;

        private bool bcActive;

        private bool ocActive;

        private float gcR;

        private float gcG;

        private float gcB;

        private float gcA = 255;

        private float rcR;

        private float rcG;

        private float rcB;

        private float rcA = 255;

        private float ycR;

        private float ycG;

        private float ycB;

        private float ycA = 255;

        private float bcR;

        private float bcG;

        private float bcB;

        private float bcA = 255;

        private float ocR;

        private float ocG;

        private float ocB;

        private float ocA = 255;

        private bool ghActive;

        private bool rhActive;

        private bool yhActive;

        private bool bhActive;

        private bool ohActive;

        private float ghR;

        private float ghG;

        private float ghB;

        private float ghA = 255;

        private float rhR;

        private float rhG;

        private float rhB;

        private float rhA = 255;

        private float yhR;

        private float yhG;

        private float yhB;

        private float yhA = 255;

        private float bhR;

        private float bhG;

        private float bhB;

        private float bhA = 255;

        private float ohR;

        private float ohG;

        private float ohB;

        private float ohA = 255;

        private bool ghlActive;

        private bool rhlActive;

        private bool yhlActive;

        private bool bhlActive;

        private bool ohlActive;

        private float ghlR;

        private float ghlG;

        private float ghlB;

        private float ghlA = 255;

        private float rhlR;

        private float rhlG;

        private float rhlB;

        private float rhlA = 255;

        private float yhlR;

        private float yhlG;

        private float yhlB;

        private float yhlA = 255;

        private float bhlR;

        private float bhlG;

        private float bhlB;

        private float bhlA = 255;

        private float ohlR;

        private float ohlG;

        private float ohlB;

        private float ohlA = 255;

        private bool mnActive;

        private bool mcActive;

        private bool songpbActive;

        private bool starpbActive;

        private bool osActive;

        private bool isActive;

        private bool scTextActive;

        private bool fcActive;

        private GUIStyle mnEx;

        private GUIStyle mcEx;

        private GUIStyle songpbEx;

        private GUIStyle starpbEx;

        private GUIStyle osEx;

        private GUIStyle isEx;

        private GUIStyle scTextEx;

        private GUIStyle fcEx;

        private float mnR;

        private float mnG;

        private float mnB;

        private float mnA = 255;

        private float mcR;

        private float mcG;

        private float mcB;

        private float mcA = 255;

        private float songpbR;

        private float songpbG;

        private float songpbB;

        private float songpbA = 255;

        private float starpbR;

        private float starpbG;

        private float starpbB;

        private float starpbA = 255;

        private float osR;

        private float osG;

        private float osB;

        private float osA = 255;

        private float isR;

        private float isG;

        private float isB;

        private float isA = 255;

        private float scTextR;

        private float scTextG;

        private float scTextB;

        private float scTextA = 255;

        private float fcR;

        private float fcG;

        private float fcB;

        private float fcA = 255;

        private float onR;

        private float onG;

        private float onB;

        private float onA = 255;

        private List<ParticleSystem> particles = new List<ParticleSystem>();

        private float particleRR;

        private float particleRG;

        private float particleRB;

        private float particleRA = 255;

        private float particleGR;

        private float particleGG;

        private float particleGB;

        private float particleGA = 255;

        private float particleYR;

        private float particleYG;

        private float particleYB;

        private float particleYA = 255;

        private float particleBR;

        private float particleBG;

        private float particleBB;

        private float particleBA = 255;

        private float particleOR;

        private float particleOG;

        private float particleOB;

        private float particleOA = 255;

        private float particleSize;

        private float particleGravity;

        private float particleMax;

        private float particleSpeed;

        private float sR;

        private float sG;

        private float sB;

        private bool pgActive;

        private bool prActive;

        private bool pyActive;

        private bool pbActive;

        private bool poActive;

        private bool particleSettingsActive;

        private bool saveMenuTheme;

        private bool isSettingKey;

        private Color pgPrint;

        private Color prPrint;

        private Color pyPrint;

        private Color pbPrint;

        private Color poPrint;

        private GUIStyle pgEx;

        private GUIStyle prEx;

        private GUIStyle pyEx;

        private GUIStyle pbEx;

        private GUIStyle poEx;

        private SpriteRenderer onOval;

        private SpriteRenderer onOvalFlames;

        private SpriteRenderer[] scoreFonts = new SpriteRenderer[11];

        private SpriteRenderer[] comboFonts = new SpriteRenderer[6];

        private Color greenPrint;

        private Color redPrint;

        private Color yellowPrint;

        private Color bluePrint;

        private Color orangePrint;

        private Color spPrint;

        private Color gfPrint;

        private Color rfPrint;

        private Color yfPrint;

        private Color bfPrint;

        private Color ofPrint;

        private Color glPrint;

        private Color rlPrint;

        private Color ylPrint;

        private Color blPrint;

        private Color olPrint;

        private Color spblPrint;

        private Color spbuPrint;

        private Color spbLtPrint;

        private Color gcPrint;

        private Color rcPrint;

        private Color ycPrint;

        private Color bcPrint;

        private Color ocPrint;

        private Color ghPrint;

        private Color rhPrint;

        private Color yhPrint;

        private Color bhPrint;

        private Color ohPrint;

        private Color ghlPrint;

        private Color rhlPrint;

        private Color yhlPrint;

        private Color bhlPrint;

        private Color ohlPrint;

        private Color mnPrint;

        private Color mcPrint;

        private Color songpbPrint;

        private Color starpbPrint;

        private Color osPrint;

        private Color isPrint;

        private Color scTextPrint;

        private Color fcPrint;

        private Color onPrint;

        private Color spGlowPrint;

        private Color comboFontPrint;

        private Color scoreFontPrint;

        private Color sPrint;

        private SpriteRenderer mn;

        private SpriteRenderer mg;

        private SpriteRenderer cc;

        private SpriteRenderer pBar1;

        private SpriteRenderer pBar2;

        private SpriteRenderer outerStar;

        private SpriteRenderer innerStar;

        private SpriteRenderer starCount;

        private SpriteRenderer fcRotate;

        private SpriteRenderer fcBg;

        private Renderer multiRender;

        private Renderer multiGlowRender;

        private Renderer comboCountRender;

        private Renderer oStarRender;

        private Renderer iStarRender;

        private Renderer fcRotateRender;

        private Renderer fcBgRender;

        private SpriteRenderer star1;

        private SpriteRenderer star2;

        private SpriteRenderer star3;

        private SpriteRenderer star4;

        private SpriteRenderer star5;

        private SpriteRenderer star6;

        private SpriteRenderer star7;

        private SpriteRenderer glowL;

        private SpriteRenderer glowR;

        private bool spGlowActive;

        private bool sActive;

        private float strikelGR;

        private float strikelGG;

        private float strikelGB;

        private float strikelGA = 255;

        private float strikelRR;

        private float strikelRG;

        private float strikelRB;

        private float strikelRA = 255;

        private float strikelYR;

        private float strikelYG;

        private float strikelYB;

        private float strikelYA = 255;

        private float strikelBR;

        private float strikelBG;

        private float strikelBB;

        private float strikelBA = 255;

        private float strikelOR;

        private float strikelOG;

        private float strikelOB;

        private float strikelOA = 255;

        private Color strikelRPrint;

        private Color strikelGPrint;

        private Color strikelYPrint;

        private Color strikelBPrint;

        private Color strikelOPrint;

        private GUIStyle strikelREx;

        private GUIStyle strikelGEx;

        private GUIStyle strikelYEx;

        private GUIStyle strikelBEx;

        private GUIStyle strikelOEx;

        private bool strikeRActive;

        private bool strikeGActive;

        private bool strikeYActive;

        private bool strikeBActive;

        private bool strikeOActive;

        private float spGlowR;

        private float spGlowG = 1;

        private float spGlowB = 1;

        private SpriteRenderer sideLGlow;

        private SpriteRenderer sideRGlow;

        private SpriteRenderer sideL;

        private SpriteRenderer sideR;

        private SpriteRenderer highwayGlow;

        private float comboFontR;

        private float comboFontG;

        private float comboFontB;

        private float scoreFontR;

        private float scoreFontG;

        private float scoreFontB;

        private bool comboFontActive;

        private bool scoreFontActive;

        private GUIStyle comboFontEx;

        private GUIStyle scoreFontEx;

        private bool ghlEnabled;

        private bool spRan;

        private string animColorsSig = "̛̜̘̔̐̑̏̓̕̚̕";

        private string noteColorSig = "̗̖̖̎̓̔̑̔̚̚̚";

        private string sustainColorSig = "̛̛̛̜̙̘̙̜̎̒̕";

        private string purpleColorSig = "̛̗̜̗̗̖̐̔̍̕̚";

        private string cyanColorSig = "̙̘̘̖̗̖̎̑̕̚̚";

        private string cyanAnimSig = "̛̛̗̗̙̜̏̒̐̓̚";

        private string openSustainSig = "̖̘̗̔̔̎̓̑̏̚̚";

        private Type noteType;

        private FieldInfo animColorField;

        private FieldInfo noteColorField;

        private FieldInfo sustainColorField;

        private FieldInfo cyanColorField;

        private FieldInfo cyanAnimField;

        private FieldInfo purpleColorField;

        private FieldInfo openSustainField;

        private bool reloaded = false;

        public class FretHead
        {
            public GameObject headLight
            {
                get
                {
                    return this.hLight;
                }
                set
                {
                    this.hLight = value;
                }
            }

            public SpriteRenderer headLightRenderer
            {
                get
                {
                    if (this.hLightRender == null)
                    {
                        this.hLightRender = this.headLight.GetComponent<SpriteRenderer>();
                    }

                    return this.hLightRender;
                }
            }

            public GameObject head
            {
                get
                {
                    return this.h;
                }
                set
                {
                    this.h = value;
                }
            }

            public SpriteRenderer headRenderer
            {
                get
                {
                    if (this.hRender == null)
                    {
                        this.hRender = this.head.GetComponent<SpriteRenderer>();
                    }
                    return this.hRender;
                }
            }

            private GameObject hLight;

            private GameObject h;

            private SpriteRenderer hLightRender;

            private SpriteRenderer hRender;
        }

        public class FretCover
        {
            public GameObject frontCover
            {
                get
                {
                    return this.fCover;
                }
                set
                {
                    this.fCover = value;
                }
            }

            public SpriteRenderer frontCoverRenderer
            {
                get
                {
                    if (this.fCoverRender == null)
                    {
                        this.fCoverRender = this.frontCover.GetComponent<SpriteRenderer>();
                    }

                    return this.fCoverRender;
                }
            }

            public GameObject backCover
            {
                get
                {
                    return this.bCover;
                }
                set
                {
                    this.bCover = value;
                }
            }

            public SpriteRenderer backCoverRenderer
            {
                get
                {
                    if (this.bCoverRender == null)
                    {
                        this.bCoverRender = this.backCover.GetComponent<SpriteRenderer>();
                    }

                    return this.bCoverRender;
                }
            }

            public GameObject headCover
            {
                get
                {
                    return this.hCover;
                }
                set
                {
                    this.hCover = value;
                }
            }

            public SpriteRenderer headCoverRenderer
            {
                get
                {
                    if (this.hCoverRender == null)
                    {
                        this.hCoverRender = this.headCover.GetComponent<SpriteRenderer>();
                    }

                    return this.hCoverRender;
                }
            }

            public GameObject[] covers
            {
                get
                {
                    return new GameObject[]
                    {
                        this.frontCover,
                        this.backCover,
                        this.headCover
                    };
                }
            }

            private GameObject fCover;

            private GameObject bCover;

            private GameObject hCover;

            private SpriteRenderer fCoverRender;

            private SpriteRenderer bCoverRender;

            private SpriteRenderer hCoverRender;
        }
    }
}
