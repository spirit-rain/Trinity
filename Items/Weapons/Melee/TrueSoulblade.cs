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
	public class TrueSoulblade : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("The Soulbound weapon of a warrior, raised to it's physical limits.\nHolding down the attack button summons a Soulblade that circles the player.\nLetting go of the attack button releases it towards the mouse.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 136;
			Item.DamageType = DamageClass.Melee;
			Item.width = 62;
			Item.height = 62;
			Item.useTime = 60;
			Item.useAnimation = 0;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.channel = true;
			Item.knockBack = 4;
			Item.value = 2200;
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<TrueSoulbladeProjectile>();
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
			recipe.AddIngredient<Soulblade>();
			recipe.AddIngredient(ItemID.SoulofFright, 20);
			recipe.AddIngredient(ItemID.SoulofMight, 20);
			recipe.AddIngredient(ItemID.SoulofSight, 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}