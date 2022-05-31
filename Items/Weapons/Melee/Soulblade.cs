using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;
using Trinity.Items.Items;
using Trinity.Projectiles.Melee;
using Terraria.DataStructures;

namespace Trinity.Items.Weapons.Melee
{
	public class Soulblade : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("The Soulbound weapon of a warrior.\nHolding down the attack button summons a Soulblade that circles the player.\nLetting go of the attack button releases it towards the mouse.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 102;
			Item.DamageType = DamageClass.Melee;
			Item.width = 56;
			Item.height = 56;
			Item.useTime = 60;
			Item.useAnimation = 0;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.channel = true;
			Item.knockBack = 4;
			Item.value = 2200;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SoulbladeProjectile>();
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}

		
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (TrinityPlayer.ModPlayer(player).circleProjCount == 0)
			{
				TrinityPlayer.ModPlayer(player).circleProjCount++;
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
			recipe.AddIngredient<SoulPearl>();
			recipe.AddIngredient(ItemID.LightsBane);
			recipe.AddIngredient(ItemID.HellstoneBar, 9);
			recipe.AddIngredient(ItemID.Obsidian, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient<SoulPearl>();
			recipe2.AddIngredient(ItemID.BloodButcherer);
			recipe2.AddIngredient(ItemID.HellstoneBar, 9);
			recipe2.AddIngredient(ItemID.Obsidian, 12);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}