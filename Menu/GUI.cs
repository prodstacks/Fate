using System;
using BepInEx;
using UnityEngine;
using Photon.Pun;
using static NetworkSystem;
using static UnityEngine.EventSystems.EventTrigger;
using System.Collections.Generic;
using Unity.XR.CoreUtils;

namespace Fate.Menu
{
    [BepInPlugin("org.violet.gorillatag.gui", "Violet GUI", Fate.PluginInfo.Version)]
    public class VioletGUI : BaseUnityPlugin
    {
        #region Important Variables
        public Rect guiRect = new Rect(400f, 200f, 525f, 365f);
        public enum Page { Home, Room, Helpful, Movement, Entity, Player, Visuals, Master, Overpowered, Projectiles, Spammers, Settings }

        public static Category SettingsIndex = NXOTemplate.Mods.Category.Settings;
        public static Category RoomIndex = NXOTemplate.Mods.Category.Room;
        public static Category HelpfulIndex = NXOTemplate.Mods.Category.Helpful;
        public static Category MovementIndex = NXOTemplate.Mods.Category.Movement;
        public static Category EntityIndex = NXOTemplate.Mods.Category.Entity;
        public static Category PlayerIndex = NXOTemplate.Mods.Category.Player;
        public static Category VisualsIndex = NXOTemplate.Mods.Category.Visuals;
        public static Category MasterIndex = NXOTemplate.Mods.Category.Master;
        public static Category OverpoweredIndex = NXOTemplate.Mods.Category.Overpowered;
        public static Category ProjectilesIndex = NXOTemplate.Mods.Category.Projectiles;
        public static Category SpammersIndex = NXOTemplate.Mods.Category.Spammers;

        public static bool ArrayListShown = true;
        public static bool GUIShown = true;
        public static bool TexturesSet = false;

        public static string NameOfMenu = "Violet";
        public static string VersionOfMenu = NXOTemplate.Initialization.PluginInfo.menuVersion;

        public Color32 buttonColor = new Color32(28, 28, 28, byte.MaxValue);
        public Color32 buttonHoverColor = new Color32(48, 48, 48, byte.MaxValue);
        public Color32 buttonEnabledColor = new Color32(68, 68, 68, byte.MaxValue);


        public Color32 guiBackGroundColor = new Color32(18, 18, 18, byte.MaxValue);
        public Color32 containerColor = new Color32(88, 48, 128, byte.MaxValue);
        #endregion

        #region NonImportant Variables
        public Vector2 Scrolling = Vector2.zero;

        public static Texture2D buttonTexture = new Texture2D(2, 2);
        public static Texture2D buttonHoverTexture = new Texture2D(2, 2);
        public static Texture2D buttonClickTexture = new Texture2D(2, 2);
        public static Texture2D guiBackgroundTexture = new Texture2D(2, 2);
        public static Texture2D containerTexture = new Texture2D(2, 2);
        public static Texture2D arrayListTexture = new Texture2D(2, 2);
        public static Texture2D versionTexture = new Texture2D(2, 2);
        public static Texture2D timeTexture = new Texture2D(2, 2);
        public static Texture2D updatesTexture = new Texture2D(2, 2);

        public static Rect pageButtonRect;

        public static Page Category = Page.Home;

        public static Material colorMaterial = new Material(Shader.Find("GUI/Text Shader")) { color = Color.Lerp(Color.gray * 1.3f, Color.gray * 1.6f, Mathf.PingPong(Time.time, 1.5f)) };
        #endregion
        public void Update()
        {
            if (UnityInput.Current.GetKeyDown(KeyCode.Insert))
                GUIShown = !GUIShown;
        }
        public void OnGUI()
        {
            buttonTexture = CreateTexture(buttonColor);
            buttonHoverTexture = CreateTexture(buttonHoverColor);
            buttonClickTexture = CreateTexture(buttonEnabledColor);


            guiBackgroundTexture = CreateTexture(guiBackGroundColor);
            containerTexture = CreateTexture(containerColor);

            versionTexture = CreateTexture(containerColor);
            timeTexture = CreateTexture(containerColor);
            updatesTexture = CreateTexture(containerColor);


            arrayListTexture = CreateRoundedTexture(17, containerColor);

            if (GUIShown)
            {
                guiRect = GUI.Window(1, guiRect, new GUI.WindowFunction(DrawGUI), "");
            }
            GUIStyle arrayLabelStyle = new GUIStyle(GUI.skin.box) { fontSize = 26, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter, wordWrap = false, normal = { background = arrayListTexture, textColor = colorMaterial.color } };
            GUI.Label(new Rect(750f, 10f, arrayLabelStyle.CalcSize(new GUIContent($"{NameOfMenu} v{VersionOfMenu} | " + "FPS : " + Mathf.Ceil(1f / Time.unscaledDeltaTime).ToString())).x + 13.5f, 33.5f), $"{NameOfMenu} v{VersionOfMenu} | " + "FPS : " + Mathf.Ceil(1f / Time.unscaledDeltaTime).ToString(), arrayLabelStyle);
            if (ArrayListShown)
            {
                GUIStyle arrayButtonStyle = new GUIStyle(GUI.skin.box) { fontSize = 21, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter, wordWrap = false, normal = { background = arrayListTexture, textColor = colorMaterial.color } };
                foreach (ButtonHandler.Button button in ModButtons.buttons)
                {
                    if (button.Enabled)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(5.5f);
                        GUILayout.BeginVertical();
                        GUILayout.Space(5.5f);
                        GUILayout.Label(button.buttonText, arrayButtonStyle, GUILayout.Width(arrayButtonStyle.CalcSize(new GUIContent(button.buttonText)).x + 13.5f), GUILayout.Height(33.5f));
                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                    }
                }
            }
        }
        public void DrawGUI(int id)
        {
            if (!TexturesSet)
            {
                SetTextures();

                TexturesSet = true;
            }
            else
            {
                DoTexture(new Rect(0f, 0f, guiRect.width, guiRect.height), guiBackgroundTexture, 12);
                DoTexture(new Rect(4f, 34f, 95f, guiRect.height - 40f), containerTexture, 12);

                GUIStyle titleStyle = new GUIStyle(GUI.skin.label) { fontSize = 26, fontStyle = FontStyle.BoldAndItalic };
                GUI.Label(new Rect(15f, 1f, guiRect.width, guiRect.height), NameOfMenu, titleStyle);

                GUILayout.BeginArea(new Rect(7.5f, 30f, 100f, guiRect.height));
                GUILayout.BeginVertical();
                GUILayout.Space(8f);

                foreach (Page page in Enum.GetValues(typeof(Page)))
                {
                    if (RoundedPageButton(EnumUtilExt.GetName<Page>(page), page, GUILayout.Width(89f)))
                    {
                        Category = page;
                        GorillaTagger.Instance.StartVibration(rightHandedMenu, GorillaTagger.Instance.tagHapticStrength / 2, GorillaTagger.Instance.tagHapticDuration / 2);
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, rightHandedMenu, 1f);
                    }
                    GUILayout.Space(2f);
                }

                GUILayout.EndVertical();
                GUILayout.EndArea();

                #region Categories
                switch (Category)
                {
                    case Page.Room:
                        DrawRoomPage();
                        break;

                    case Page.Helpful:
                        DrawHelpfulPage();
                        break;

                    case Page.Movement:
                        DrawMovementPage();
                        break;

                    case Page.Entity:
                        DrawEntityPage();
                        break;

                    case Page.Player:
                        DrawPlayerPage();
                        break;

                    case Page.Visuals:
                        DrawVisualsPage();
                        break;

                    case Page.Master:
                        DrawMasterPage();
                        break;

                    case Page.Overpowered:
                        DrawOverpoweredPage();
                        break;

                    case Page.Projectiles:
                        DrawProjectilesPage();
                        break;

                    case Page.Spammers:
                        DrawSpammersPage();
                        break;

                    case Page.Settings:
                        DrawSettingsPage();
                        break;

                    default:
                        DrawHomePage();
                        break;
                }
                #endregion

                GUI.DragWindow(new Rect(0f, 0f, 1000f, 1000f));
            }
        }
        #region Draw Pages Logic
        private void DrawRoomPage()
        {
            GUILayout.BeginArea(new Rect(115f, 30f, 370f, guiRect.height - 50f));
            GUILayout.BeginVertical();

            Scrolling = GUILayout.BeginScrollView(Scrolling);

            List<ButtonHandler.Button> list = new List<ButtonHandler.Button>();
            list.AddRange(ButtonHandler.GetButtonInfoByPage(RoomIndex));

            foreach (ButtonHandler.Button button in list)
            {
                if (RoundedButton(button.buttonText, button))
                {
                    ButtonHandler.Toggle(button);
                    GorillaTagger.Instance.StartVibration(rightHandedMenu, GorillaTagger.Instance.tagHapticStrength / 2, GorillaTagger.Instance.tagHapticDuration / 2);
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, rightHandedMenu, 1f);
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        private void DrawHelpfulPage()
        {
            GUILayout.BeginArea(new Rect(115f, 30f, 370f, guiRect.height - 50f));
            GUILayout.BeginVertical();

            Scrolling = GUILayout.BeginScrollView(Scrolling);

            List<ButtonHandler.Button> list = new List<ButtonHandler.Button>();
            list.AddRange(ButtonHandler.GetButtonInfoByPage(HelpfulIndex));

            foreach (ButtonHandler.Button button in list)
            {
                if (RoundedButton(button.buttonText, button))
                {
                    ButtonHandler.Toggle(button);
                    GorillaTagger.Instance.StartVibration(rightHandedMenu, GorillaTagger.Instance.tagHapticStrength / 2, GorillaTagger.Instance.tagHapticDuration / 2);
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, rightHandedMenu, 1f);
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        private void DrawMovementPage()
        {
            GUILayout.BeginArea(new Rect(115f, 30f, 370f, guiRect.height - 50f));
            GUILayout.BeginVertical();

            Scrolling = GUILayout.BeginScrollView(Scrolling);

            List<ButtonHandler.Button> list = new List<ButtonHandler.Button>();
            list.AddRange(ButtonHandler.GetButtonInfoByPage(MovementIndex));

            foreach (ButtonHandler.Button button in list)
            {
                if (RoundedButton(button.buttonText, button))
                {
                    ButtonHandler.Toggle(button);
                    GorillaTagger.Instance.StartVibration(rightHandedMenu, GorillaTagger.Instance.tagHapticStrength / 2, GorillaTagger.Instance.tagHapticDuration / 2);
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, rightHandedMenu, 1f);
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        private void DrawEntityPage()
        {
            GUILayout.BeginArea(new Rect(115f, 30f, 370f, guiRect.height - 50f));
            GUILayout.BeginVertical();

            Scrolling = GUILayout.BeginScrollView(Scrolling);

            List<ButtonHandler.Button> list = new List<ButtonHandler.Button>();
            list.AddRange(ButtonHandler.GetButtonInfoByPage(EntityIndex));

            foreach (ButtonHandler.Button button in list)
            {
                if (RoundedButton(button.buttonText, button))
                {
                    ButtonHandler.Toggle(button);
                    GorillaTagger.Instance.StartVibration(rightHandedMenu, GorillaTagger.Instance.tagHapticStrength / 2, GorillaTagger.Instance.tagHapticDuration / 2);
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, rightHandedMenu, 1f);
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        private void DrawPlayerPage()
        {
            GUILayout.BeginArea(new Rect(115f, 30f, 370f, guiRect.height - 50f));
            GUILayout.BeginVertical();

            Scrolling = GUILayout.BeginScrollView(Scrolling);

            List<ButtonHandler.Button> list = new List<ButtonHandler.Button>();
            list.AddRange(ButtonHandler.GetButtonInfoByPage(PlayerIndex));

            foreach (ButtonHandler.Button button in list)
            {
                if (RoundedButton(button.buttonText, button))
                {
                    ButtonHandler.Toggle(button);
                    GorillaTagger.Instance.StartVibration(rightHandedMenu, GorillaTagger.Instance.tagHapticStrength / 2, GorillaTagger.Instance.tagHapticDuration / 2);
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, rightHandedMenu, 1f);
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        private void DrawVisualsPage()
        {
            GUILayout.BeginArea(new Rect(115f, 30f, 370f, guiRect.height - 50f));
            GUILayout.BeginVertical();

            Scrolling = GUILayout.BeginScrollView(Scrolling);

            List<ButtonHandler.Button> list = new List<ButtonHandler.Button>();
            list.AddRange(ButtonHandler.GetButtonInfoByPage(VisualsIndex));

            foreach (ButtonHandler.Button button in list)
            {
                if (RoundedButton(button.buttonText, button))
                {
                    ButtonHandler.Toggle(button);
                    GorillaTagger.Instance.StartVibration(rightHandedMenu, GorillaTagger.Instance.tagHapticStrength / 2, GorillaTagger.Instance.tagHapticDuration / 2);
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, rightHandedMenu, 1f);
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        private void DrawMasterPage()
        {
            GUILayout.BeginArea(new Rect(115f, 30f, 370f, guiRect.height - 50f));
            GUILayout.BeginVertical();

            Scrolling = GUILayout.BeginScrollView(Scrolling);

            List<ButtonHandler.Button> list = new List<ButtonHandler.Button>();
            list.AddRange(ButtonHandler.GetButtonInfoByPage(MasterIndex));

            foreach (ButtonHandler.Button button in list)
            {
                if (RoundedButton(button.buttonText, button))
                {
                    ButtonHandler.Toggle(button);
                    GorillaTagger.Instance.StartVibration(rightHandedMenu, GorillaTagger.Instance.tagHapticStrength / 2, GorillaTagger.Instance.tagHapticDuration / 2);
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, rightHandedMenu, 1f);
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        private void DrawOverpoweredPage()
        {
            GUILayout.BeginArea(new Rect(115f, 30f, 370f, guiRect.height - 50f));
            GUILayout.BeginVertical();

            Scrolling = GUILayout.BeginScrollView(Scrolling);

            List<ButtonHandler.Button> list = new List<ButtonHandler.Button>();
            list.AddRange(ButtonHandler.GetButtonInfoByPage(OverpoweredIndex));

            foreach (ButtonHandler.Button button in list)
            {
                if (RoundedButton(button.buttonText, button))
                {
                    ButtonHandler.Toggle(button);
                    GorillaTagger.Instance.StartVibration(rightHandedMenu, GorillaTagger.Instance.tagHapticStrength / 2, GorillaTagger.Instance.tagHapticDuration / 2);
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, rightHandedMenu, 1f);
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        private void DrawProjectilesPage()
        {
            GUILayout.BeginArea(new Rect(115f, 30f, 370f, guiRect.height - 50f));
            GUILayout.BeginVertical();

            Scrolling = GUILayout.BeginScrollView(Scrolling);

            List<ButtonHandler.Button> list = new List<ButtonHandler.Button>();
            list.AddRange(ButtonHandler.GetButtonInfoByPage(ProjectilesIndex));

            foreach (ButtonHandler.Button button in list)
            {
                if (RoundedButton(button.buttonText, button))
                {
                    ButtonHandler.Toggle(button);
                    GorillaTagger.Instance.StartVibration(rightHandedMenu, GorillaTagger.Instance.tagHapticStrength / 2, GorillaTagger.Instance.tagHapticDuration / 2);
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, rightHandedMenu, 1f);
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        private void DrawSpammersPage()
        {
            GUILayout.BeginArea(new Rect(115f, 30f, 370f, guiRect.height - 50f));
            GUILayout.BeginVertical();

            Scrolling = GUILayout.BeginScrollView(Scrolling);

            List<ButtonHandler.Button> list = new List<ButtonHandler.Button>();
            list.AddRange(ButtonHandler.GetButtonInfoByPage(SpammersIndex));

            foreach (ButtonHandler.Button button in list)
            {
                if (RoundedButton(button.buttonText, button))
                {
                    ButtonHandler.Toggle(button);
                    GorillaTagger.Instance.StartVibration(rightHandedMenu, GorillaTagger.Instance.tagHapticStrength / 2, GorillaTagger.Instance.tagHapticDuration / 2);
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, rightHandedMenu, 1f);
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        private void DrawSettingsPage()
        {
            GUILayout.BeginArea(new Rect(115f, 30f, 370f, guiRect.height - 50f));
            GUILayout.BeginVertical();

            Scrolling = GUILayout.BeginScrollView(Scrolling);

            List<ButtonHandler.Button> list = new List<ButtonHandler.Button>();
            list.AddRange(ButtonHandler.GetButtonInfoByPage(SettingsIndex));

            foreach (ButtonHandler.Button button in list)
            {
                if (RoundedButton(button.buttonText, button))
                {
                    ButtonHandler.Toggle(button);
                    GorillaTagger.Instance.StartVibration(rightHandedMenu, GorillaTagger.Instance.tagHapticStrength / 2, GorillaTagger.Instance.tagHapticDuration / 2);
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, rightHandedMenu, 1f);
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        private void DrawHomePage()
        {
            DoTexture(new Rect(110f, 70f, 195f, 70f), versionTexture, 12);
            DoTexture(new Rect(314f, 70f, 195f, 70f), timeTexture, 12);
            DoTexture(new Rect(110f, 150f, guiRect.width - 125f, guiRect.height - 156f), updatesTexture, 12);

            GUI.Label(new Rect(115f, 2f, guiRect.width, 45f), $"We're glad your here, {PhotonNetwork.LocalPlayer.NickName.ToLower().FirstToUpper()}!", CreateLabelStyle(Color.white, 22, FontStyle.Bold, TextAnchor.MiddleLeft));
            GUI.Label(new Rect(115f, 29f, guiRect.width, 45f), "Press INSERT to toggle this GUI", CreateLabelStyle(Color.gray, 17, FontStyle.Italic, TextAnchor.MiddleLeft));

            GUI.Label(new Rect(122f, 68f, 165f, 30f), "Version", CreateLabelStyle(Color.grey, 20, FontStyle.Normal, TextAnchor.MiddleCenter));
            GUI.Label(new Rect(322f, 68f, 165f, 30f), "Current Time", CreateLabelStyle(Color.grey, 20, FontStyle.Normal, TextAnchor.MiddleCenter));
            GUI.Label(new Rect(323f, 95f, 165f, 30f), DateTime.Now.ToString("hh:mm tt"), CreateLabelStyle(Color.white, 22, FontStyle.Bold, TextAnchor.MiddleCenter));
            GUI.Label(new Rect(123f, 95f, 165f, 30f), VersionOfMenu, CreateLabelStyle(Color.white, 22, FontStyle.Bold, TextAnchor.MiddleCenter));

            GUI.Label(new Rect(265f, 152f, 400f, 30f), "Updates", CreateLabelStyle(Color.grey, 20, FontStyle.Normal, TextAnchor.MiddleLeft));
            GUI.Label(new Rect(115f, 172f, 400f, 150f), "Improved GUI\nOptimized Template\nAdded Input Library\nAdded Gun Library\nAdded Notification Library\n", CreateLabelStyle(Color.white, 19, FontStyle.Bold, TextAnchor.MiddleLeft));
        }
        #endregion

        #region GUI Utilities
        private GUIStyle CreateLabelStyle(Color color, int fontSize, FontStyle fontStyle, TextAnchor textAnchor)
        {
            return new GUIStyle(GUI.skin.label)
            {
                normal = { textColor = color },
                fontSize = fontSize,
                fontStyle = fontStyle,
                alignment = textAnchor,
            };
        }
        public static void SetTextures()
        {
            if (!TexturesSet)
            {
                GUI.skin.label.richText = true;
                GUI.skin.button.richText = true;
                GUI.skin.window.richText = true;
                GUI.skin.textField.richText = true;
                GUI.skin.box.richText = true;

                GUI.skin.window.border.bottom = 5;
                GUI.skin.window.border.left = 5;
                GUI.skin.window.border.top = 5;
                GUI.skin.window.border.right = 5;

                GUI.skin.window.active.background = null;
                GUI.skin.window.normal.background = null;
                GUI.skin.window.hover.background = null;
                GUI.skin.window.focused.background = null;
                GUI.skin.window.onFocused.background = null;
                GUI.skin.window.onActive.background = null;
                GUI.skin.window.onHover.background = null;
                GUI.skin.window.onNormal.background = null;

                GUI.skin.button.active.background = buttonClickTexture;
                GUI.skin.button.normal.background = buttonHoverTexture;
                GUI.skin.button.hover.background = buttonTexture;
                GUI.skin.button.onActive.background = buttonClickTexture;
                GUI.skin.button.onHover.background = buttonHoverTexture;
                GUI.skin.button.onNormal.background = buttonTexture;

                GUI.skin.verticalScrollbarThumb.active.background = containerTexture;
                GUI.skin.verticalScrollbarThumb.normal.background = containerTexture;
                GUI.skin.verticalScrollbarThumb.hover.background = containerTexture;
                GUI.skin.verticalScrollbarThumb.focused.background = containerTexture;
                GUI.skin.verticalScrollbarThumb.onFocused.background = containerTexture;
                GUI.skin.verticalScrollbarThumb.onActive.background = containerTexture;
                GUI.skin.verticalScrollbarThumb.onHover.background = containerTexture;
                GUI.skin.verticalScrollbarThumb.onNormal.background = containerTexture;

                GUI.skin.horizontalScrollbar.active.background = guiBackgroundTexture;
                GUI.skin.horizontalScrollbar.normal.background = guiBackgroundTexture;
                GUI.skin.horizontalScrollbar.hover.background = guiBackgroundTexture;
                GUI.skin.horizontalScrollbar.focused.background = guiBackgroundTexture;
                GUI.skin.horizontalScrollbar.onFocused.background = guiBackgroundTexture;
                GUI.skin.horizontalScrollbar.onActive.background = guiBackgroundTexture;
                GUI.skin.horizontalScrollbar.onHover.background = guiBackgroundTexture;
                GUI.skin.horizontalScrollbar.onNormal.background = guiBackgroundTexture;
                GUI.skin.horizontalSlider.active.background = guiBackgroundTexture;
                GUI.skin.horizontalSlider.normal.background = guiBackgroundTexture;
                GUI.skin.horizontalSlider.hover.background = guiBackgroundTexture;
                GUI.skin.horizontalSlider.focused.background = guiBackgroundTexture;
                GUI.skin.horizontalSlider.onFocused.background = guiBackgroundTexture;
                GUI.skin.horizontalSlider.onActive.background = guiBackgroundTexture;
                GUI.skin.horizontalSlider.onHover.background = guiBackgroundTexture;
                GUI.skin.horizontalSlider.onNormal.background = guiBackgroundTexture;

                GUI.skin.horizontalSliderThumb.active.background = guiBackgroundTexture;
                GUI.skin.horizontalSliderThumb.normal.background = guiBackgroundTexture;
                GUI.skin.horizontalSliderThumb.hover.background = guiBackgroundTexture;
                GUI.skin.horizontalSliderThumb.focused.background = guiBackgroundTexture;
                GUI.skin.horizontalSliderThumb.onFocused.background = guiBackgroundTexture;
                GUI.skin.horizontalSliderThumb.onActive.background = guiBackgroundTexture;
                GUI.skin.horizontalSliderThumb.onHover.background = guiBackgroundTexture;
                GUI.skin.horizontalSliderThumb.onNormal.background = guiBackgroundTexture;
                GUI.skin.horizontalScrollbarThumb.active.background = guiBackgroundTexture;
                GUI.skin.horizontalScrollbarThumb.normal.background = guiBackgroundTexture;
                GUI.skin.horizontalScrollbarThumb.hover.background = guiBackgroundTexture;
                GUI.skin.horizontalScrollbarThumb.focused.background = guiBackgroundTexture;
                GUI.skin.horizontalScrollbarThumb.onFocused.background = guiBackgroundTexture;
                GUI.skin.horizontalScrollbarThumb.onActive.background = guiBackgroundTexture;
                GUI.skin.horizontalScrollbarThumb.onHover.background = guiBackgroundTexture;
                GUI.skin.horizontalScrollbarThumb.onNormal.background = guiBackgroundTexture;

                GUI.skin.verticalScrollbar.border = new RectOffset(0, 0, 0, 0);
                GUI.skin.verticalScrollbar.fixedWidth = 0f;
                GUI.skin.verticalScrollbar.fixedHeight = 0f;
                GUI.skin.verticalScrollbarThumb.fixedHeight = 0f;
                GUI.skin.verticalScrollbarThumb.fixedWidth = 3f;

                GUI.skin.horizontalScrollbar.border = new RectOffset(0, 0, 0, 0);
                GUI.skin.horizontalScrollbar.fixedWidth = 0f;
                GUI.skin.horizontalScrollbar.fixedHeight = 0f;
                GUI.skin.horizontalScrollbarThumb.fixedHeight = 0f;
                GUI.skin.horizontalScrollbarThumb.fixedWidth = 3f;

                TexturesSet = true;
            }
            else
            {
                return;
            }
        }
        public static Texture2D CreateTexture(Color color)
        {
            Texture2D texture2D = new Texture2D(30, 30);
            Color[] array = new Color[900];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = color;
            }
            texture2D.SetPixels(array);
            texture2D.Apply();
            return texture2D;
        }
        public static Texture2D CreateRoundedTexture(int k, Color color)
        {
            Texture2D texture2D = new Texture2D(k, k);
            Color[] array = new Color[k * k];
            for (int i = 0; i < k * k; i++)
            {
                int j = i % k;
                int p = i / k;
                if (Mathf.Sqrt((j - k / 2) * (j - k / 2) + (p - k / 2) * (p - k / 2)) <= k / 2)
                {
                    array[i] = color;
                }
                else
                {
                    array[i] = Color.clear;
                }
            }
            texture2D.SetPixels(array);
            texture2D.Apply();
            return texture2D;
        }
        public static bool RoundedButton(string content, ButtonHandler.Button button, params GUILayoutOption[] options)
        {
            Texture2D texture2D = button.Enabled ? buttonClickTexture : buttonTexture;
            Rect rect = GUILayoutUtility.GetRect(new GUIContent(content), GUI.skin.button, options);
            if (rect.Contains(Event.current.mousePosition))
            {
                texture2D = buttonHoverTexture;
            }
            if (rect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
            {
                texture2D = buttonClickTexture;
                return true;
            }
            string text = button.buttonText;
            GUI.DrawTexture(rect, texture2D, 0, false, 0f, GUI.color, Vector4.zero, new Vector4(8f, 8f, 8f, 8f));
            GUIStyle guistyle = new GUIStyle(GUI.skin.label);
            guistyle.fontStyle = FontStyle.Bold;
            guistyle.fontSize = 13;
            guistyle.normal.textColor = Color.white;
            float num = rect.x + rect.width / 2f - guistyle.CalcSize(new GUIContent(text)).x / 2f;
            float num2 = rect.y + rect.height / 2f - guistyle.CalcSize(new GUIContent(text)).y / 2f;
            GUI.Label(new Rect(num, num2, rect.width, rect.height), new GUIContent(text), guistyle);
            return false;
        }
        public static bool RoundedPageButton(string content, Page i, params GUILayoutOption[] options)
        {
            if (Category == i)
            {
                Texture2D texture2D = buttonClickTexture;
                pageButtonRect = GUILayoutUtility.GetRect(new GUIContent(content), GUI.skin.button, options);
                if (pageButtonRect.Contains(Event.current.mousePosition))
                {
                    texture2D = buttonHoverTexture;
                }
                if (pageButtonRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                {
                    texture2D = buttonTexture;
                    return true;
                }
                GUI.DrawTexture(pageButtonRect, texture2D, 0, false, 0f, GUI.color, Vector4.zero, new Vector4(8f, 8f, 8f, 8f));
                GUIStyle guistyle = new GUIStyle(GUI.skin.label);
                guistyle.fontStyle = FontStyle.Bold;
                guistyle.fontSize = 13;
                guistyle.normal.textColor = Color.white;
                float num = pageButtonRect.x + pageButtonRect.width / 2f - guistyle.CalcSize(new GUIContent(content)).x / 2f;
                float num2 = pageButtonRect.y - 3f + 25f / 2f - guistyle.CalcSize(new GUIContent(content)).y / 2f;
                GUI.Label(new Rect(num, num2, pageButtonRect.width, 25f), new GUIContent(content), guistyle);
                return false;
            }
            else
            {
                Texture2D texture2D2 = buttonTexture;
                pageButtonRect = GUILayoutUtility.GetRect(new GUIContent(content), GUI.skin.button, options);
                if (pageButtonRect.Contains(Event.current.mousePosition))
                {
                    texture2D2 = buttonHoverTexture;
                }
                if (pageButtonRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                {
                    texture2D2 = buttonClickTexture;
                    return true;
                }
                GUI.DrawTexture(pageButtonRect, texture2D2, 0, false, 0f, GUI.color, Vector4.zero, new Vector4(8f, 8f, 8f, 8f));
                GUIStyle guistyle = new GUIStyle(GUI.skin.label);
                guistyle.fontStyle = FontStyle.Bold;
                guistyle.fontSize = 13;
                guistyle.normal.textColor = Color.white;
                float num = pageButtonRect.x + pageButtonRect.width / 2f - guistyle.CalcSize(new GUIContent(content)).x / 2f;
                float num2 = pageButtonRect.y - 3f + 25f / 2f - guistyle.CalcSize(new GUIContent(content)).y / 2f;
                GUI.Label(new Rect(num, num2, pageButtonRect.width, 25f), new GUIContent(content), guistyle);
                return false;
            }
        }
        public static void DoTexture(Rect rect, Texture2D texture, float borderRadius)
        {
            GUI.DrawTexture(rect, texture, ScaleMode.StretchToFill, false, 0f, GUI.color, Vector4.zero, new Vector4(borderRadius, borderRadius, borderRadius, borderRadius));
        }
        #endregion
    }
}