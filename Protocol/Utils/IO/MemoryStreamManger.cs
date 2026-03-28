using Microsoft.IO;

namespace Protocol.Utils.IO;

public static class MemoryStreamManger
{
	public static RecyclableMemoryStreamManager stream { get; set; } = new();
}