using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Input;
using Trinity.Items.Items;

namespace Trinity
{
	public class TrinityPlayer : ModPlayer
	{
		//accessories
		public bool umbralEmblem = false;
		public bool midasCase = false;

		//weapons
		public int circleProjCount = 0;

		public static TrinityPlayer ModPlayer(Player player)
		{
			return player.GetModPlayer<TrinityPlayer>();
		}

		public override void ResetEffects()
		{
			umbralEmblem = false;
			midasCase = false;
		}

        public override void PreUpdateMovement()
		{
			if (umbralEmblem && PlayerInput.GetPressedKeys().Contains(Keys.S))
				Player.velocity.Y = 0;

			base.PreUpdateMovement();
		}

		public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
		{
			if (!mediumCoreDeath)
			{
				Item item = new Item();
				item.SetDefaults(ModContent.ItemType<SoulPearl>());

				return new[]{ new Item(ModContent.ItemType<SoulPearl>()) };
			}
			else
				return base.AddStartingItems(mediumCoreDeath);
        }
	}
}