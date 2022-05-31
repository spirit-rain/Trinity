using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.Graphics.Shaders;
using Trinity.Projectiles.Ranger;

namespace Trinity.Items.Weapons.Ranger
{
    public class Lynchpin : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots a shot with damage equal to the square root of the amount of money you're holding to a max of 5,000.\nIn addition to this, this weapon cannot crit. Instead, critical strike chance raises maximum damage.\n\"Bullets are expensive!\"");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.rare = ItemRarityID.Lime;

            Item.useTime = 120;
            Item.useAnimation = 120;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item36;

            Item.DamageType = DamageClass.Ranged;
            Item.damage = 1;
            Item.knockBack = 7f;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.value = Item.buyPrice(silver: 666);

            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 11f;
        }


        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            ModifyTooltips(null);

            if (line.Mod == "Terraria" && line.Name == "ItemName")
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);
                GameShaders.Armor.Apply(GameShaders.Armor.GetShaderIdFromItemId(ItemID.BloodbathDye), Item, null);
                Utils.DrawBorderString(Main.spriteBatch, line.Text, new Vector2(line.X, line.Y), new Color(184, 170, 165), 1);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
                return false;
            }
            else
                return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            TrinityPlayer modPlayer = TrinityPlayer.ModPlayer(player);
            int damageCap = 5000;
            if (modPlayer.midasCase)
                damageCap = 8000;

            damage = (int)Math.Sqrt(Convert.ToInt32(Utils.CoinsCount(out bool flag, player.inventory, new int[] { 58, 57, 56, 55, 54 })));
            if (damage <= 0)
                damage = 1;
            if (damage > damageCap * ((player.GetCritChance(DamageClass.Ranged) * 1.5f + 50) / 50))
                damage = (int)(damageCap * ((player.GetCritChance(DamageClass.Ranged) * 1.5f + 50) / 50));

            type = ModContent.ProjectileType<LynchpinBullet>();
            Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Handgun);
            recipe.AddIngredient(ItemID.LuckyCoin);
            recipe.AddIngredient(ItemID.SoulofMight, 8);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}