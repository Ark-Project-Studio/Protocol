using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Minecraft
{
	internal class TranslatedItem : IEquatable<TranslatedItem>
	{
		public int Id { get; }
		public short Meta { get; }

		public TranslatedItem(int id, short meta)
		{
			Id = id;
			Meta = meta;
		}


#pragma warning disable CS8767
		public bool Equals(TranslatedItem other)
#pragma warning restore CS8767
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;

			return Id == other.Id && Meta == other.Meta;
		}


		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;

			return Equals((TranslatedItem)obj);
		}


		public override int GetHashCode()
		{
			return HashCode.Combine(Id, Meta);
		}
	}
}