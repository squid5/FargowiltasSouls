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
        public static readonly SoundStyle BaronRoar = new(SoundsPath + "Challengers/Baron/BaronRoar");
        public static readonly SoundStyle BaronYell = new(SoundsPath + "Challengers/Baron/BaronYell");
        public static readonly SoundStyle NukeBeep = new(SoundsPath + "Challengers/Baron/NukeBeep");

        // Lifelight
        public static readonly SoundStyle LifelightDash = new(SoundsPath + "Challengers/Lifelight/LifelightDash");
        public static readonly SoundStyle LifelightDeathray = new(SoundsPath + "Challengers/Lifelight/LifelightDeathray") ;
        public static readonly SoundStyle LifelightDeathrayShort = new(SoundsPath + "Challengers/Lifelight/LifelightDeathrayShort");
        public static readonly SoundStyle LifelightPixieDash = new(SoundsPath + "Challengers/Lifelight/LifelightPixieDash");
        public static readonly SoundStyle LifelightRuneSound = new(SoundsPath + "Challengers/Lifelight/LifelightRuneSound");
        public static readonly SoundStyle LifelightScreech1 = new(SoundsPath + "Challengers/Lifelight/LifelightScreech1");
        public static readonly SoundStyle LifelightShotPrep = new(SoundsPath + "Challengers/Lifelight/LifelightShotPrep");

        // Coffin
        public static readonly SoundStyle CoffinBigShot = new(SoundsPath + "Challengers/Coffin/CoffinBigShot");
        public static readonly SoundStyle CoffinHandCharge = new(SoundsPath + "Challengers/Coffin/CoffinHandCharge");
        public static readonly SoundStyle CoffinPhaseTransition = new (SoundsPath + "Challengers/Coffin/CoffinPhaseTransition");
        public static readonly SoundStyle CoffinShot = new(SoundsPath + "Challengers/Coffin/CoffinShot");
        public static readonly SoundStyle CoffinSlam = new(SoundsPath + "Challengers/Coffin/CoffinSlam");
        public static readonly SoundStyle CoffinSoulShot = new(SoundsPath + "Challengers/Coffin/CoffinSoulShot");
        public static readonly SoundStyle CoffinSpiritDrone = new(SoundsPath + "Challengers/Coffin/CoffinSpiritDrone");

        // Deviantt
        public static readonly SoundStyle DeviSwing = new(SoundsPath + "Siblings/Deviantt/DeviSwing");
        public static readonly SoundStyle DeviHeartExplosion = new(SoundsPath + "Siblings/Deviantt/DeviHeartExplosion");

        // Mutant
        public static readonly SoundStyle MutantUnpredictive = new(SoundsPath + "Siblings/Mutant/MutantUnpredictive");
        public static readonly SoundStyle MutantPredictive = new(SoundsPath + "Siblings/Mutant/MutantPredictive");

        // Abominationn
    }
}
