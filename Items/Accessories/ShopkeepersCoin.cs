using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;

namespace Trinity.Items.Accessories
{
	public class ShopkeepersCoin : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Your maximum HP is set to the amount of gold coins you are carrying\nThis can go up to a maximum of 1.25x your base max HP, and has a minimum of 10.\nIn addition to this, your damage is reduced by 20%.\nHowever, it increases by 1% per 5 gold coins you're carrying, to a maximum of an extra 50% damage.\n\"Live to sell and sell to live\"");
			DisplayName.SetDefault("Shopkeeper's Coin");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Green;
			Item.width = 16;
			Item.height = 16;
			Item.accessory = true;
			Item.value = 10000;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.statLifeMax2 = (int)(Convert.ToInt32(Utils.CoinsCount(out bool flag, player.inventory, new int[] { 58, 57, 56, 55, 54 })) / 10000);
			if (player.statLifeMax2 < 10)
				player.statLifeMax2 = 10;
			if (player.statLifeMax2 > player.statLifeMax * 1.25f)
				player.statLifeMax2 = (int)(player.statLifeMax * 1.25f);

			float damageAdd = Convert.ToInt32(Utils.CoinsCount(out bool flag2, player.inventory, new int[] { 58, 57, 56, 55, 54 })) / 5000000;
			if (damageAdd > 0.7f)
				damageAdd = 0.7f;
			player.GetDamage(DamageClass.Generic) -= 0.2f;
			player.GetDamage(DamageClass.Generic) += damageAdd;
		}

		public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
		{
			ModifyTooltips(null);

			if (line.Mod == "Terraria" && line.Name == "ItemName")
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);
				GameShaders.Armor.Apply(GameShaders.Armor.GetShaderIdFromItemId(ItemID.ReflectiveGoldDye), Item, null);
				Utils.DrawBorderString(Main.spriteBatch, line.Text, new Vector2(line.X, line.Y), new Color(184, 170, 165), 1);
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
			recipe.AddIngredient(ItemID.GoldCoin);
			recipe.AddIngredient(ItemID.GoldBar, 3);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.GoldCoin);
			recipe2.AddIngredient(ItemID.PlatinumBar, 3);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}