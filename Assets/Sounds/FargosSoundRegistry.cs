using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;

namespace FargowiltasSouls.Assets.Sounds
{
    public static class FargosSoundRegistry
    {
        public const string SoundsPath = "FargowiltasSouls/Assets/Sounds/";

        public static readonly SoundStyle ThrowShort = new(SoundsPath + "ThrowShort");
        public static readonly SoundStyle RetinazerDeathray = new(SoundsPath + "VanillaEternity/Mechs/RetinazerDeathray");
        public static readonly SoundStyle ReticleBeep = new(SoundsPath + "ReticleBeep");
        public static readonly SoundStyle ReticleLockOn = new(SoundsPath + "ReticleLockOn");
        public static readonly SoundStyle StyxGazer = new(SoundsPath + "Siblings/Abominationn/StyxGazer");
        public static readonly SoundStyle Thunder = new(SoundsPath + "Thunder");
        public static readonly SoundStyle ZaWarudo = new(SoundsPath + "Accessories/ZaWarudo");
        public static readonly SoundStyle Zombie104 = new(SoundsPath + "Zombie_104");

        // Baron
        public static readonly SoundStyle BaronLaserTelegraph = new(SoundsPath + "Challengers/Baron/BaronLaserTelegraph");
        public static readonly SoundStyle BaronLaserSoundSlow = new(SoundsPath + "Challengers/Baron/BaronLaserSound_Slow");
        public static readonly SoundStyle BaronHit = new(SoundsPath + "Challengers/Baron/BaronHit");

    }
}
