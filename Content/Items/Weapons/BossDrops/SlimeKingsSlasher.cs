using FargowiltasSouls.Content.Projectiles.BossWeapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using rail;
using Terraria.Audio;
using System;

namespace FargowiltasSouls.Content.Items.Weapons.BossDrops
{
    public class SlimeKingsSlasher : SoulsItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 44;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.scale = 1.5f;
        }
        // I tried to recreate the slash using predraw but i wasn't able to figure out how to get it to draw properly
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D slash = ModContent.Request<Texture2D>("FargowiltasSouls/Content/Items/Weapons/BossDrops/SlimeKingsSlasherSlash", AssetRequestMode.ImmediateLoad).Value;
            Vector2 slashloc;
            float slashrotation;
            slashloc = new Vector2(30, 110);
            slashrotation = 180;
            //float origin;

           
            //Main.spriteBatch.Draw(slash,  default, lightColor, slashrotation, Item.Center, 0.75f, SpriteEffects.None, 0);
            return false;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Slimed, 120);
            SoundEngine.PlaySound(SoundID.Item17);
            Projectile.NewProjectile(player.GetSource_FromThis(),target.Center, Vector2.Zero, ModContent.ProjectileType<Slimesplosion>(), damageDone, 1f, Item.whoAmI, 1, 1, 1);
        }
    }
}