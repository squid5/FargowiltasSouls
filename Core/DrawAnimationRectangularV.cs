using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;

namespace FargowiltasSouls.Core
{
    public class DrawAnimationRectangularV : DrawAnimation
    {
        public bool NotActuallyAnimating;
        public int Width;
        public int Height;

        public DrawAnimationRectangularV(int ticksperframe, int horizontalFrames, int verticalFrames)
        {
            Frame = 0;
            FrameCounter = 0;
            Width = horizontalFrames;
            Height = verticalFrames;
            TicksPerFrame = ticksperframe;
            FrameCount = Width * Height;
        }

        public override void Update()
        {
            if (NotActuallyAnimating || ++FrameCounter < TicksPerFrame)
                return;

            FrameCounter = 0;
            if (++Frame >= FrameCount)
            {
                Frame = 0;
            }
        }

        public override Rectangle GetFrame(Texture2D texture, int frameCounterOverride = -1)
        {
            if (frameCounterOverride != -1)
            {
                int frameOverride = frameCounterOverride / TicksPerFrame;

                int frameXOverride = frameOverride / Height;
                int frameYOverride = frameOverride % Height;

                Rectangle result = texture.Frame(Width, Height, frameXOverride, frameYOverride);
                result.Height -= 2;
                return result;
            }

            int frameX = Frame / Height;
            int frameY = Frame % Height;

            Rectangle result2 = texture.Frame(Width, Height, frameX, frameY);
            result2.Height -= 2;
            return result2;
        }
    }
}