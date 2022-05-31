using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;
using Trinity.Projectiles.Melee;
using Terraria.DataStructures;

namespace Trinity.Items.Weapons.Melee
{
	public class SoulReaver : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("The Soulbound weapon of a warrior, ascended with the spirits of the dead.\nHolding down the attack button summons six Soulblades that circle the player.\nLetting go of the attack button release them towards the mouse.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 198;
			Item.DamageType = DamageClass.Melee;
			Item.width = 96;
			Item.height = 108;
			Item.useTime = 60;
			Item.useAnimation = 0;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.channel = true;
			Item.knockBack = 4;
			Item.value = 10000;
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SoulReaverProjectile>();
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}

		
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (TrinityPlayer.ModPlayer(player).circleProjCount == 0)
			{
				TrinityPlayer.ModPlayer(player).circleProjCount++;
				SoulReaverProjectile reaverProj = Main.projectile[Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI).identity].ModProjectile as SoulReaverProjectile;
				reaverProj.ProjectileAngleDegrees = 120;
				reaverProj = Main.projectile[Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI).identity].ModProjectile as SoulReaverProjectile;
				reaverProj.ProjectileAngleDegrees = 240;
				type = ModContent.ProjectileType<SoulReaverProjectileSmall>();
				SoulReaverProjectileSmall reaverProj2 = Main.projectile[Projectile.NewProjectileDirect(source, position, velocity, type, (int)(damage * 1.25f), knockback, player.whoAmI).identity].ModProjectile as SoulReaverProjectileSmall;
				reaverProj2.ProjectileAngleDegrees = 60;
				reaverProj2 = Main.projectile[Projectile.NewProjectileDirect(source, position, velocity, type, (int)(damage * 1.25f), knockback, player.whoAmI).identity].ModProjectile as SoulReaverProjectileSmall;
				reaverProj2.ProjectileAngleDegrees = 180;
				reaverProj2 = Main.projectile[Projectile.NewProjectileDirect(source, position, velocity, type, (int)(damage * 1.25f), knockback, player.whoAmI).identity].ModProjectile as SoulReaverProjectileSmall;
				reaverProj2.ProjectileAngleDegrees = 300;

				return base.Shoot(player, source, position, velocity, type, damage, knockback);
			}
			else
				return false;
        }

        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
		{
			ModifyTooltips(null);

			if (line.Mod == "Terraria" && line.Name == "ItemName")
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);
				GameShaders.Armor.Apply(GameShaders.Armor.GetShaderIdFromItemId(ItemID.VoidDye), Item, null);
				Utils.DrawBorderString(Main.spriteBatch, line.Text, new Vector2(line.X, line.Y), new Color(29, 25, 64), 1);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
				return false;
			}
			else
				return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient<TrueSoulblade>();
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddIngredient(ItemID.SpectreBar, 20);
			recipe.AddIngredient(ItemID.Ectoplasm, 50);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}