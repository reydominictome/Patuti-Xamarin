using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Reflection;
using System.IO;
using Xamarin.Essentials;

namespace Patuti
{
    public partial class MainPage : ContentPage
    {
        private string[][] animation = new string[5][]
        {
            new string[] { "Patuti.Media.idle-1.png", "Patuti.Media.idle-2.png" },
            new string[] { "Patuti.Media.left-1.png", "Patuti.Media.left-2.png", "Patuti.Media.left-3.png",
            "Patuti.Media.left-4.png", "Patuti.Media.left-6.png"},
            new string[] { "Patuti.Media.jump-1.png", "Patuti.Media.jump-2.png", "Patuti.Media.jump-3.png"
            , "Patuti.Media.jump-4.png", "Patuti.Media.jump-5.png", "Patuti.Media.jump-5.png", "Patuti.Media.jump-5.png"
            , "Patuti.Media.jump-5.png", "Patuti.Media.jump-5.png", "Patuti.Media.jump-5.png", "Patuti.Media.jump-6.png"
            , "Patuti.Media.jump-6.png", "Patuti.Media.jump-7.png", "Patuti.Media.jump-7.png"},
            new string[] { "Patuti.Media.dock-1.png", "Patuti.Media.dock-2.png", "Patuti.Media.dock-3.png",
            "Patuti.Media.dock-4.png", "Patuti.Media.dock-4.png", "Patuti.Media.dock-4.png", "Patuti.Media.dock-4.png"
            , "Patuti.Media.dock-4.png", "Patuti.Media.dock-4.png", "Patuti.Media.dock-4.png", "Patuti.Media.dock-4.png",
            "Patuti.Media.dock-4.png", "Patuti.Media.dock-5.png"},
            new string[] { "Patuti.Media.right-1.png", "Patuti.Media.right-2.png", "Patuti.Media.right-3.png"
            , "Patuti.Media.right-4.png", "Patuti.Media.right-5.png"}
        };
        private int patutiX, patutiY, patutiHeight, patutiWidth, animationKind, animationCursor;
        private SKBitmap patuti = null, area, background;
        private DisplayInfo screenInfo = DeviceDisplay.MainDisplayInfo;
        private Assembly assembly;
        private bool isMoving;
        public MainPage()
        {
            InitializeComponent();
            assembly = GetType().GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream("Patuti.Media.bg.png"))
            {
                background = SKBitmap.Decode(stream);
                if (background != null)
                {
                    var tempDisplayInfo = new SKImageInfo((int)screenInfo.Width, (int)screenInfo.Height);
                    background = background.Resize(tempDisplayInfo, SKFilterQuality.High);
                }
            }

            using (Stream stream = assembly.GetManifestResourceStream("Patuti.Media.area.png"))
            {
                area = SKBitmap.Decode(stream);
                if (area != null)
                {
                    var tempDisplayInfo = new SKImageInfo(350, 210);
                    area = area.Resize(tempDisplayInfo, SKFilterQuality.High);
                }
            }

            patutiWidth = 90;
            patutiHeight = 120;

            using (Stream stream = assembly.GetManifestResourceStream("Patuti.Media.idle-1.png"))
            {
                patuti = SKBitmap.Decode(stream);
                if (area != null)
                {
                    var tempDisplayInfo = new SKImageInfo(patutiWidth, patutiHeight);
                    patuti = patuti.Resize(tempDisplayInfo, SKFilterQuality.High);
                }
            }

            patutiX = ((int)screenInfo.Width / 2) - 60;
            patutiY = ((int)screenInfo.Height / 2) - 125;
            animationKind = 0;
            animationCursor = 0;

            Device.StartTimer(TimeSpan.FromSeconds(1f / 60), () =>
            {
                canvasView.InvalidateSurface();
                return true;
            });
        }

        private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.DrawBitmap(background, 0, 0);
            canvas.DrawBitmap(area, (info.Width/2)-150, info.Height/2);
            canvas.DrawBitmap(patuti, patutiX, patutiY);

            animationCursor = (animationCursor + 1) % animation[animationKind].Length;

            if (isMoving)
            {
                Console.WriteLine(animationKind);
                if(animationKind == 1)
                {
                    if(animationCursor > 2)
                    {
                        patutiX = patutiX - 25;
                    }
                    if(animationCursor == 0)
                    {
                        animationKind = 0;
                        animationCursor = 1;
                        isMoving = false;
                    }
                }

                if (animationKind == 2)
                {
                    if (animationCursor < 3)
                        patutiY = patutiY;
                    else if (animationCursor > 3)
                    {
                        if (animationCursor > 3 && animationCursor < 5)
                        {
                            patutiY = patutiY - 150;
                        }
                        else if (animationCursor < 11)
                        {
                            patutiY = patutiY;
                        }
                        else
                            patutiY = patutiY + 50;
                    }

                    if (animationCursor == 0)
                    {
                        animationKind = 0;
                        animationCursor = 1;
                        isMoving = false;
                    }
                }

                if (animationKind == 3)
                {
                    if (animationCursor > 2 && animationCursor < 5)
                    {
                        patutiY = patutiY + 20;
                        patutiHeight = patutiHeight - 15;
                    }
                    else if (animationCursor < 12)
                    {
                        patutiY = patutiY;
                        patutiHeight = patutiHeight;
                    }
                    else if (animationCursor == 12)
                    {
                        patutiY = patutiY - 40;
                        patutiHeight = patutiHeight + 30;
                    }

                    if (animationCursor == 0)
                    {
                        animationKind = 0;
                        animationCursor = 1;
                        isMoving = false;
                    }
                }

                if (animationKind == 4)
                {
                    if (animationCursor > 2)
                    {
                        patutiX = patutiX + 25;
                    }

                    if (animationCursor == 0)
                    {
                        animationKind = 0;
                        animationCursor = 1;
                        isMoving = false;
                    }
                }
            }
            else
            {
                animationKind = 0;
            }
            using (Stream stream = assembly.GetManifestResourceStream(animation[animationKind][animationCursor]))
            {
                patuti = SKBitmap.Decode(stream);
                if (area != null)
                {
                    var tempDisplayInfo = new SKImageInfo(patutiWidth, patutiHeight);
                    patuti = patuti.Resize(tempDisplayInfo, SKFilterQuality.High);
                }
            }
        }

        public void moveIt(int i)
        {
            animationKind = i;
            animationCursor = 0;
        }
        private async void ButtonClicked(object sender, EventArgs e)
        {
            if (!isMoving)
            {
                Button button = (Button)sender;
                string command = button.CommandParameter.ToString();

                switch (command)
                {
                    case "1":
                        // Patuti Moves to the Left
                        Console.WriteLine(command);
                        isMoving = true;
                        moveIt(1);
                        break;
                    case "2":
                        // Patuti Moves Jumps
                        Console.WriteLine(command);
                        isMoving = true;
                        moveIt(2);
                        break;
                    case "3":
                        // Patuti Moves Docks
                        Console.WriteLine(command);
                        isMoving = true;
                        moveIt(3);
                        break;
                    case "4":
                        // Patuti Moves to the Right
                        Console.WriteLine(command);
                        isMoving = true;
                        moveIt(4);
                        break;
                }
            }
        }
    }
}
