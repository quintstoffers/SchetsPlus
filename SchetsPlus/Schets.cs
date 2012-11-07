﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;

namespace SchetsPlus
{
    [Serializable]
    public class Schets
    {
        public Bitmap bitmap;

        public Size imageSize;
        public double imageRatio;
        public string imageName;
        public string imagePath;

        public int actionDrawLimit;
        public int actionEraseLimit = -1;

        public Color primaryColor;
        public Color secondaryColor;

        public ObservableCollection<Action> actions = new ObservableCollection<Action>();   // ObservableCollection to automatically update historyWindow

        public Graphics BitmapGraphics
        {
            get
            {
                if (bitmap == null)
                {
                    this.bitmap = new Bitmap(imageSize.Width, imageSize.Height);
                }
                return Graphics.FromImage(bitmap);
            }
        }

        public Schets(string name, Size imageSize)
        {
            this.imageName = name;
            this.imageSize = imageSize;
            this.imageRatio = (double) imageSize.Width / (double) imageSize.Height;
            this.bitmap = new Bitmap(imageSize.Width, imageSize.Height);

            primaryColor = Color.Black;
            secondaryColor = Color.White;
        }

        public void VeranderAfmeting(Size sz)
        {
            if (bitmap is Bitmap)
            {

            }
        }
        public void Teken(Graphics gr, int width, int height)
        {
            Debug.WriteLine("CONTROL: " + width + "::" + height);
            Debug.WriteLine("BMP    : " + bitmap.Width + "::" + bitmap.Height);
            gr.Clear(Color.White);
            gr.DrawImage(bitmap, 0, 0, width, height);
        }

        public void TekenFromActions(SchetsControl s)
        {
            Color tempPrimary = primaryColor;
            App.currentSchetsWindow.currentSchetsControl.Schoon();

            for (int i = 0; i < actions.Count; i++)
            {
                actions[i].drawAction = true;
            }

            for (int i = 0; i <= actionDrawLimit && i < actions.Count; i++)
            {
                if (actions[i] is FancyEraserAction)
                {
                    s.currentAction = actions[i];

                    s.schets.actionEraseLimit = i;
                    ((FancyEraserAction)actions[i]).draw(s);
                    s.schets.actionEraseLimit = -1;
                }
            }
            for (int i = actionDrawLimit + 1; i < actions.Count; i++)
            {
                if (actions[i] is FancyEraserAction)
                {
                    ((FancyEraserAction)actions[i]).erasedAction.drawAction = true;
                }
            }
            for (int i = 0; i <= actionDrawLimit && i < actions.Count; i++)
            {
                s.currentAction = actions[i];
                if (actions[i].drawAction && !(actions[i] is FancyEraserAction))
                {
                    actions[i].draw(s);
                }
            }
        }

        public void Schoon()
        {
            BitmapGraphics.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
        }
        public void Roteer()
        {
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }
    }
}
