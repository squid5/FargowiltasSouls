using FargowiltasSouls.Core.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Content.Buffs.Masomode;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Localization;
using FargowiltasSouls.Content.Projectiles.Masomode;

namespace FargowiltasSouls.Core.ModPlayers
{
    public partial class EModePlayer : ModPlayer
    {
        public int MasomodeFreezeTimer;
        public int MasomodeSpaceBreathTimer;
        public int LightningCounter;
        public int LightLevelCounter;
        public int HallowFlipCheckTimer;

        public override void PreUpdate()
        {
            if (!WorldSavingSystem.EternityMode)
                return;

            if (!Player.Alive())
                return;

            FargoSoulsPlayer fargoSoulsPlayer = Player.FargoSouls();
            bool waterEffectImmune = fargoSoulsPlayer.BaronsBurden;

            //falling gives you dazed. wings save you
            /*if (Player.velocity.Y == 0f && Player.wingsLogic == 0 && !Player.noFallDmg && !Player.ghost && !Player.dead)
            {
                int num21 = 25;
                num21 += Player.extraFall;
                int num22 = (int)(Player.position.Y / 16f) - Player.fallStart;
                if (Player.mount.CanFly)
                {
                    num22 = 0;
                }
                if (Player.mount.Cart && Minecart.OnTrack(Player.position, Player.width, Player.height))
                {
                    num22 = 0;
                }
                if (Player.mount.Type == 1)
                {
                    num22 = 0;
                }
                Player.mount.FatigueRecovery();

                if (((Player.gravDir == 1f && num22 > num21) || (Player.gravDir == -1f && num22 < -num21)))
                {
                    Player.immune = false;
                    int dmg = (int)(num22 * Player.gravDir - num21) * 10;
                    if (Player.mount.Active)
                        dmg = (int)(dmg * Player.mount.FallDamage);

                    Player.Hurt(PlayerDeathReason.ByOther(0), dmg, 0);
                    Player.AddBuff(BuffID.Dazed, 120);
                }
                Player.fallStart = (int)(Player.position.Y / 16f);
            }*/

            if (!NPC.downedBoss3 && Player.ZoneDungeon && !NPC.AnyNPCs(NPCID.DungeonGuardian) && !Main.drunkWorld && !Main.zenithWorld)
            {
                NPC.SpawnOnPlayer(Player.whoAmI, NPCID.DungeonGuardian);
            }

            if (Player.ZoneUnderworldHeight)
            {
                bool anyAshwoodEffect = Player.HasEffect<AshWoodEffect>() || Player.HasEffect<ObsidianEffect>();
                if (anyAshwoodEffect || !(Player.fireWalk || fargoSoulsPlayer.PureHeart || Player.lavaMax > 0))
                    FargoSoulsUtil.AddDebuffFixedDuration(Player, BuffID.OnFire, 2);
            }

            if (Player.ZoneJungle)
            {
                if (WaterWet && !waterEffectImmune)
                    FargoSoulsUtil.AddDebuffFixedDuration(Player, BuffID.Poisoned, 2);
            }

            if (Player.ZoneSnow)
            {
                //if (!fargoSoulsPlayer.PureHeart && !Main.dayTime && Framing.GetTileSafely(Player.Center).WallType == WallID.None)
                //    Player.AddBuff(BuffID.Chilled, Main.expertMode && Main.expertDebuffTime > 1 ? 1 : 2);

                if (WaterWet && Player.chilled && !waterEffectImmune)
                {
                    Player.AddBuff(ModContent.BuffType<HypothermiaBuff>(), 2);
                    /*
                    MasomodeFreezeTimer++;
                    if (MasomodeFreezeTimer >= 600)
                    {
                        FargoSoulsUtil.AddDebuffFixedDuration(Player, BuffID.Frozen, 120);
                        MasomodeFreezeTimer = -300;
                    }
                    */
                }
                else
                {
                    MasomodeFreezeTimer = 0;
                }
            }
            else
            {
                MasomodeFreezeTimer = 0;
            }

            /*if (Player.wet && !fargoSoulsPlayer.MutantAntibodies)
            {
                if (Player.ZoneDesert)
                    FargoSoulsUtil.AddDebuffFixedDuration(Player, BuffID.Slow, 2);
                if (Player.ZoneDungeon)
                    FargoSoulsUtil.AddDebuffFixedDuration(Player, BuffID.Cursed, 2);
                Tile currentTile = Framing.GetTileSafely(Player.Center);
                if (currentTile.WallType == WallID.GraniteUnsafe)
                    FargoSoulsUtil.AddDebuffFixedDuration(Player, BuffID.Weak, 2);
                if (currentTile.WallType == WallID.MarbleUnsafe)
                    FargoSoulsUtil.AddDebuffFixedDuration(Player, BuffID.BrokenArmor, 2);
            }*/

            if (Player.ZoneDesert && Player.ZoneOverworldHeight && !fargoSoulsPlayer.PureHeart)
            {
                if (Main.dayTime)
                {
                    if (!Player.wet && !hasUmbrella())
                    {
                        FargoSoulsUtil.AddDebuffFixedDuration(Player, BuffID.Weak, 2);
                    }
                    
                }
                else
                {
                    if (!ItemID.Sets.Torches[Player.HeldItem.type])
                    {
                        FargoSoulsUtil.AddDebuffFixedDuration(Player, BuffID.Chilled, 2);
                    }
                    
                }
            }

            if (Player.ZoneCorrupt)
            {
                if (!fargoSoulsPlayer.PureHeart)
                    FargoSoulsUtil.AddDebuffFixedDuration(Player, BuffID.Darkness, 2);
                if (WaterWet && !waterEffectImmune)
                    FargoSoulsUtil.AddDebuffFixedDuration(Player, BuffID.CursedInferno, 2);
            }

            if (Player.ZoneCrimson)
            {
                if (!fargoSoulsPlayer.PureHeart)
                    FargoSoulsUtil.AddDebuffFixedDuration(Player, BuffID.Bleeding, 2);
                if (WaterWet && !waterEffectImmune)
                    FargoSoulsUtil.AddDebuffFixedDuration(Player, BuffID.Ichor, 2);
            }

            if (Player.ZoneHallow)
            {
                if (Player.ZoneRockLayerHeight && !fargoSoulsPlayer.PureHeart)
                {
                    if (++HallowFlipCheckTimer > 6) //reduce computation
                    {
                        HallowFlipCheckTimer = 0;

                        float playerAbove = Player.Center.Y - 16 * 50;
                        float playerBelow = Player.Center.Y + 16 * 50;
                        if (playerAbove / 16 < Main.maxTilesY && playerBelow / 16 < Main.maxTilesY
                            && !Collision.CanHitLine(new Vector2(Player.Left.X, playerAbove), 0, 0, new Vector2(Player.Left.X, playerBelow), 0, 0)
                            && !Collision.CanHitLine(new Vector2(Player.Right.X, playerAbove), 0, 0, new Vector2(Player.Right.X, playerBelow), 0, 0))
                        {
                            if (!Main.wallHouse[Framing.GetTileSafely(Player.Center).WallType]
                                && !Main.wallHouse[Framing.GetTileSafely(Player.TopLeft).WallType]
                                && !Main.wallHouse[Framing.GetTileSafely(Player.TopRight).WallType]
                                && !Main.wallHouse[Framing.GetTileSafely(Player.BottomLeft).WallType]
                                && !Main.wallHouse[Framing.GetTileSafely(Player.BottomRight).WallType])
                            {
                                Player.AddBuff(ModContent.BuffType<HallowIlluminatedBuff>(), 90);
                            }
                        }
                    }
                }

                if (WaterWet && !waterEffectImmune)
                    Player.AddBuff(ModContent.BuffType<SmiteBuff>(), 2);
            }

            Vector2 tileCenter = Player.Center;
            tileCenter.X /= 16;
            tileCenter.Y /= 16;
            Tile currentTile = Framing.GetTileSafely((int)tileCenter.X, (int)tileCenter.Y);


            if (!fargoSoulsPlayer.PureHeart) // Pure Heart-affected biome debuffs
            {
                Color light = Lighting.GetColor(Player.Center.ToTileCoordinates());
                float lightLevel = light.R + light.G + light.B;
                
                // underground deerclops hands
                if (Player.ZoneRockLayerHeight && !NPC.downedDeerclops && !Player.ZoneHallow)
                {
                    if (lightLevel < 500)
                    {
                        LightLevelCounter++;
                        if (LightLevelCounter > LumUtils.SecondsToFrames(20) && Main.rand.NextBool(600))
                        {
                            Vector2 pos = Player.Center + Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * 270;
                            bool failed = false;
                            for (int i = 0; i < 200; i++) // try to find a dark spot
                            {
                                pos = Player.Center + Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * 270;
                                Color lightAtPos = Lighting.GetColor(pos.ToTileCoordinates());
                                float lightLevelAtPos = lightAtPos.R + lightAtPos.G + lightAtPos.B;
                                if (lightLevelAtPos < 500)
                                    break;
                                if (i == 199) // failed
                                    failed = true;
                            }
                            if (!failed)
                            {
                                LightLevelCounter = 0;

                                int projType = ModContent.ProjectileType<DeerclopsDarknessHand>();
                                if (Main.netMode != NetmodeID.SinglePlayer)
                                {
                                    var netMessage = Mod.GetPacket();
                                    netMessage.Write((byte)FargowiltasSouls.PacketID.RequestEnvironmentalProjectile);
                                    netMessage.Write(projType);
                                    netMessage.WriteVector2(pos);
                                    netMessage.Send();
                                }
                                else
                                {
                                    int damage = (Main.hardMode ? 120 : 60) / 4;
                                    int p = Projectile.NewProjectile(Terraria.Entity.GetSource_NaturalSpawn(), pos, Vector2.Zero, projType, damage, 2f, Main.myPlayer);
                                    if (p.IsWithinBounds(Main.maxProjectiles))
                                    {
                                        Main.projectile[p].light = 1f;
                                    }
                                }
                                Lighting.AddLight(pos, 1f, 1f, 1f);
                            }
                        }
                    }
                }
                
                // hallow lifelight sparks
                if (Player.ZoneHallow && Player.ZoneRockLayerHeight && !WorldSavingSystem.DownedBoss[(int)WorldSavingSystem.Downed.Lifelight])
                {
                    if (lightLevel > 500)
                    {
                        LightLevelCounter++;
                        if (LightLevelCounter > LumUtils.SecondsToFrames(10) && Main.rand.NextBool(300))
                        {
                            LightLevelCounter = 0;
                            Vector2 pos = Player.Center;
                            int projType = ModContent.ProjectileType<LifelightEnvironmentStar>();
                            if (Main.netMode != NetmodeID.SinglePlayer)
                            {
                                var netMessage = Mod.GetPacket();
                                netMessage.Write((byte)FargowiltasSouls.PacketID.RequestEnvironmentalProjectile);
                                netMessage.Write(projType);
                                netMessage.WriteVector2(pos);
                                netMessage.Send();
                            }
                            else
                            {
                                int damage = (Main.hardMode ? 120 : 60) / 4;
                                Projectile.NewProjectile(Terraria.Entity.GetSource_NaturalSpawn(), pos, Vector2.Zero, projType, damage, 2f, Main.myPlayer, -120);
                            }
                        }
                    }
                }

                // rain lightning
                if (Main.raining && (Player.ZoneOverworldHeight)
                && !hasUmbrella())
                {
                    if (currentTile.WallType == WallID.None)
                    {
                        if (Player.ZoneSnow)
                            Player.AddBuff(ModContent.BuffType<HypothermiaBuff>(), 2);
                        else
                            Player.AddBuff(BuffID.Wet, 2);

                        LightningCounter++;

                        int lighntningMinSeconds = WorldSavingSystem.MasochistModeReal ? 10 : 17;
                        if (LightningCounter >= LumUtils.SecondsToFrames(lighntningMinSeconds))
                        {
                            Point tileCoordinates = Player.Top.ToTileCoordinates();

                            tileCoordinates.X += Main.rand.Next(-25, 25);
                            tileCoordinates.Y -= Main.rand.Next(4, 8);


                            bool foundMetal = false;
                            if (WorldSavingSystem.MasochistModeReal)
                                foundMetal = true;

                            /* TODO: make this work
                            for (int x = -5; x < 5; x++)
                            {
                                for (int y = -5; y < 5; y++)
                                {
                                    int testX = tileCoordinates.X + x;
                                    int testY = tileCoordinates.Y + y;
                                    Tile tile = Main.tile[testX, testY];
                                    if (IronTiles.Contains(tile.TileType) ||IronTiles.Contains(tile.WallType))
                                    {
                                        foundMetal = true;
                                        tileCoordinates.X = testX;
                                        tileCoordinates.Y = testY;
                                        Main.NewText("found metal");
                                        break;
                                    }
                                }
                            }
                            */

                            if (LumUtils.AnyBosses() && !WorldSavingSystem.MasochistModeReal)
                            {
                                LightningCounter = 0;
                            }
                            else if (Main.rand.NextBool(300) || foundMetal)
                            {
                                //tends to spawn in ceilings if the Player goes indoors/underground


                                //for (int index = 0; index < 10 && !WorldGen.SolidTile(tileCoordinates.X, tileCoordinates.Y) && tileCoordinates.Y > 10; ++index) 
                                //    tileCoordinates.Y -= 1;

                                float ai1 = Player.Center.Y;
                                LightningCounter = 0;
                                int projType = ModContent.ProjectileType<RainLightning>();
                                Vector2 pos = new(tileCoordinates.X * 16 + 8, tileCoordinates.Y * 16 + 17 - 900);
                                if (Main.netMode != NetmodeID.SinglePlayer)
                                {
                                    var netMessage = Mod.GetPacket();
                                    netMessage.Write((byte)FargowiltasSouls.PacketID.RequestEnvironmentalProjectile);
                                    netMessage.Write(projType);
                                    netMessage.WriteVector2(pos);
                                    netMessage.Write(ai1);
                                    netMessage.Send();
                                }
                                else
                                {
                                    int damage = (Main.hardMode ? 120 : 60) / 4;
                                    Projectile.NewProjectile(Terraria.Entity.GetSource_NaturalSpawn(), pos, Vector2.Zero, projType, damage, 2f, Main.myPlayer, Vector2.UnitY.ToRotation(), ai1);
                                }
                            }
                        }
                    }
                }

                // space breath
                if (!Player.buffImmune[BuffID.Suffocation] && Player.ZoneSkyHeight && Player.whoAmI == Main.myPlayer)
                {
                    bool immunity = !Player.armor[0].IsAir && (Player.armor[0].type == ItemID.FishBowl || Player.armor[0].type == ItemID.GoldGoldfishBowl);
                    if (Player.accDivingHelm)
                        immunity = true;

                    bool inLiquid = Collision.DrownCollision(Player.position, Player.width, Player.height, Player.gravDir);
                    if (!inLiquid || immunity)
                    {
                        Player.breath -= 3;
                        if (++MasomodeSpaceBreathTimer > 10)
                        {
                            MasomodeSpaceBreathTimer = 0;
                            Player.breath--;
                        }
                        if (Player.breath == 0)
                            SoundEngine.PlaySound(SoundID.Drown);
                        if (Player.breath <= 0)
                            Player.AddBuff(BuffID.Suffocation, 2);

                        if (Player.breath < -10) //don't stack far into negatives
                        {

                            Player.breath = -10;
                        }

                    }
                }

                // spider
                if (!Player.buffImmune[BuffID.Webbed] && Player.stickyBreak > 0)
                {

                    if (currentTile != null && currentTile.WallType == WallID.SpiderUnsafe)
                    {
                        Player.AddBuff(BuffID.Webbed, 30);
                        Player.AddBuff(BuffID.Slow, 90);
                        Player.stickyBreak = 0;

                        Vector2 vector = Collision.StickyTiles(Player.position, Player.velocity, Player.width, Player.height);
                        if (vector.X != -1 && vector.Y != -1)
                        {
                            int num3 = (int)vector.X;
                            int num4 = (int)vector.Y;
                            WorldGen.KillTile(num3, num4, false, false, false);
                            if (Main.netMode == NetmodeID.MultiplayerClient && !Main.tile[num3, num4].HasTile)
                                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, num3, num4, 0f, 0, 0, 0);
                        }
                    }
                }

                // blood moon
                if (Main.bloodMoon)
                    Player.AddBuff(BuffID.WaterCandle, 2);
            }

            if (WaterWet && !waterEffectImmune && !(Player.GetJumpState(ExtraJump.Flipper).Enabled || Player.gills || fargoSoulsPlayer.MutantAntibodies))
                Player.AddBuff(ModContent.BuffType<LethargicBuff>(), 2);

            if (currentTile != null && currentTile.TileType == TileID.Cactus && currentTile.HasUnactuatedTile && !fargoSoulsPlayer.CactusImmune && !Player.cactusThorns)
            {
                int damage = 10;
                if (WorldSavingSystem.MasochistModeReal)
                {
                    if (Player.ZoneCorrupt)
                    {
                        damage *= 2;
                        Player.AddBuff(BuffID.CursedInferno, 150);
                    }
                    if (Player.ZoneCrimson)
                    {
                        damage *= 2;
                        Player.AddBuff(BuffID.Ichor, 150);
                    }
                    if (Player.ZoneHallow)
                    {
                        damage *= 2;
                        Player.AddBuff(BuffID.Confused, 150);
                    }
                }

                if (Main.hardMode)
                    damage *= 2;

                if (Player.hurtCooldowns[0] <= 0) //same i-frames as spike tiles
                    Player.Hurt(PlayerDeathReason.ByCustomReason(Language.GetTextValue("Mods.FargowiltasSouls.DeathMessage.Cactus", Player.name)), damage, 0, false, false, 0, false);
            }


        }


        private bool hasUmbrella()
        {
            return Player.HeldItem.type == ItemID.Umbrella || Player.HeldItem.type == ItemID.TragicUmbrella
                || Player.armor[0].type == ItemID.UmbrellaHat || Player.armor[0].type == ItemID.Eyebrella
                || !Player.HasEffect<RainUmbrellaEffect>();
        }

    }
}
