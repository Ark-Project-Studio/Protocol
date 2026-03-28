using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Protocol.Minecraft
{
	public class ItemBlock : Item
	{
		[JsonIgnore] public Block Block { get; protected set; }

		protected ItemBlock(string name, short id, short metadata = 0) : base(name, id, metadata)
		{
			Block = null;
		}

		public ItemBlock([NotNull] Block block, short metadata = 0) : base(block.Name,
			(short)(block.Id > 255 ? 255 - block.Id : block.Id), metadata)
		{
			Block = block ?? throw new ArgumentNullException(nameof(block));

			FuelEfficiency = Block.FuelEfficiency;
		}
	}
}