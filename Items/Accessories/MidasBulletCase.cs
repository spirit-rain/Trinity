using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace Trinity.Items.Accessories
{
	public class MidasBulletCase : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Raises the damage cap of the Lynchpin to 8,000.\nDoubles the effectiveness of crit chance in raising this cap.");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Red;
			Item.width = 16;
			Item.height = 16;
			Item.accessory = true;
			Item.value = 1000000;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			TrinityPlayer.ModPlayer(player).midasCase = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LunarBar, 5);
			recipe.AddIngredient(ItemID.GoldDust, 8);
			recipe.AddIngredient(ItemID.PlatinumCoin, 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}