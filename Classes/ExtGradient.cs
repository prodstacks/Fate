using Fate.Menu;
using System;
using System.Linq;
using UnityEngine;

namespace Fate.Classes
{
    public class ExtGradient
    {
        public static GradientColorKey[] GetSolidGradient(Color color) =>
            new GradientColorKey[] { new GradientColorKey(color, 0f), new GradientColorKey(color, 1f) };

        public static GradientColorKey[] GetSimpleGradient(Color a, Color b) =>
            new GradientColorKey[] { new GradientColorKey(a, 0f), new GradientColorKey(b, 0.5f), new GradientColorKey(a, 1f) };

        public GradientColorKey[] colors = GetSolidGradient(Color.magenta);

        public Color GetColor(int index)
        {
            if (epileptic)
                return Main.RandomColor();

            if (copyRigColor)
                return RigManager.GetPlayerColor(VRRig.LocalRig);

            if (transparent)
            {
                Color targetColor = colors[index].color;
                targetColor.a = 0f;

                return targetColor;
            }

            if (customColor != null)
                return customColor?.Invoke() ?? Color.magenta;

            return colors[index].color;
        }

        public void SetColor(int index, Color color, bool setMirror = true)
        {
            epileptic = false;
            copyRigColor = false;

            customColor = null;

            if (colors.Length <= 2)
                colors = GetSimpleGradient(colors[0].color, colors[^1].color);

            if (setMirror && index == 0)
            {
                colors[0].color = color;
                colors[^1].color = color;
            }
            else
                colors[index].color = color;
        }

        public void SetColors(Color color)
        {
            epileptic = false;
            copyRigColor = false;

            customColor = null;

            for (int i = 0; i < colors.Length; i++)
                colors[i].color = color;
        }

        public Color GetColorTime(float time)
        {
            if (epileptic)
                return Main.RandomColor();

            if (copyRigColor)
                return RigManager.GetPlayerColor(VRRig.LocalRig);

            if (transparent)
            {
                Color targetColor = new Gradient { colorKeys = colors }.Evaluate(time);
                targetColor.a = 0f;

                return targetColor;
            }

            if (customColor != null)
                return customColor?.Invoke() ?? Color.magenta;

            return new Gradient { colorKeys = colors }.Evaluate(time);
        }

        public Color GetCurrentColor(float offset = 0f) =>
            GetColorTime((offset + (Time.time * Settings.gradientSpeed)) % 1f);

        public bool IsFlat() =>
            !epileptic && !copyRigColor &&
            colors.Length > 0 && colors.All(key => key.color == colors[0].color);

        public ExtGradient Clone()
        {
            return new ExtGradient
            {
                epileptic = epileptic,
                copyRigColor = copyRigColor,
                customColor = customColor,
                colors = colors.Select(c => new GradientColorKey(c.color, c.time)).ToArray()
            };
        }

        public bool epileptic;
        public bool copyRigColor;

        public bool transparent;

        public Func<Color> customColor;
    }
}
