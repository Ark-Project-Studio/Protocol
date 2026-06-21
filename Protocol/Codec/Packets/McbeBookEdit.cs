using Protocol.Network;
using System;

namespace Protocol.Codec.Packets;
/// <summary>
/// Represents the type of book editing action
/// </summary>
public enum BookActionType : uint
{
    /// <summary>
    /// Replace the content of an existing page
    /// </summary>
    ReplacePage = 0,
    /// <summary>
    /// Add a new page to the book
    /// </summary>
    AddPage = 1,
    /// <summary>
    /// Delete a page from the book
    /// </summary>
    DeletePage = 2,
    /// <summary>
    /// Swap the positions of two pages
    /// </summary>
    SwapPages = 3,
    /// <summary>
    /// Sign the book with title and author
    /// </summary>
    Finalize = 4
}

public class McbeBookEdit : Packet
{
    /// <summary>
    /// The slot in which the book that was edited may be found. Typically, the server should
    /// check if this slot matches the held item slot of the player.
    /// </summary>
    public int InventorySlot { get; set; }
    /// <summary>
    /// The type of the book edit action. The data obtained depends on what type this is.
    /// The action type is one of the constants defined for book actions.
    /// </summary>
    public BookActionType ActionType { get; set; }
    /// <summary>
    /// The number of the page that the book edit action concerns. It applies for all actions
    /// but the BookActionSign. In BookActionSwapPages, it is one of the pages that was swapped.
    /// </summary>
    public int PageNumber { get; set; }
    /// <summary>
    /// The page number of the second page that the action concerned. It is only set for
    /// the BookActionSwapPages action, in which case it is the other page that is swapped.
    /// </summary>
    public int SecondaryPageNumber { get; set; }
    /// <summary>
    /// The text that was written in a particular page of the book. It applies for the
    /// BookActionAddPage and BookActionReplacePage only.
    /// </summary>
    public string Text { get; set; } = string.Empty;
    /// <summary>
    /// The name of the photo on the page in the book. It applies for the BookActionAddPage and
    /// BookActionReplacePage only.
    /// Unfortunately, the functionality of this field was removed from the default Minecraft Bedrock Edition.
    /// It is still available on Education Edition.
    /// </summary>
    public string PhotoName { get; set; } = string.Empty;
    /// <summary>
    /// The title that the player has given the book. It applies only for the BookActionSign action.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// The author that the player has given the book. It applies only for the BookActionSign action.
    /// Note that the author may be freely changed, so no assumptions can be made on if the author is actually
    /// the name of a player.
    /// </summary>
    public string Author { get; set; } = string.Empty;
    /// <summary>
    /// The XBOX Live User ID of the player that edited the book. The field is rather pointless, as the
    /// server is already aware of the XUID of the player anyway.
    /// </summary>
    public string XUID { get; set; } = string.Empty;

    public McbeBookEdit()
    {
        Id = 0x61;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        WriteVarInt(InventorySlot);
        WriteUnsignedVarInt((uint)ActionType);
        switch (ActionType)
        {
            case BookActionType.ReplacePage:
            case BookActionType.AddPage:
                WriteVarInt(PageNumber);
                Write(Text);
                Write(PhotoName);
                break;
            case BookActionType.DeletePage:
                WriteVarInt(PageNumber);
                break;
            case BookActionType.SwapPages:
                WriteVarInt(PageNumber);
                WriteVarInt(SecondaryPageNumber);
                break;
            case BookActionType.Finalize:
                Write(Title);
                Write(Author);
                Write(XUID);
                break;
        }

        base.EncodePacket();
    }

    protected override void DecodePacket()
    {
        // 璇诲彇鍙嶅簭鍒楀寲浠ｇ爜
        InventorySlot = ReadVarInt(); // 瀵瑰簲 io.Varint32(&pk.InventorySlot)
        ActionType = (BookActionType)ReadUnsignedVarInt(); // 瀵瑰簲 io.Varuint32(&pk.ActionType)
        switch (ActionType)
        {
            case BookActionType.ReplacePage:
            case BookActionType.AddPage:
                PageNumber = ReadVarInt(); // 瀵瑰簲 io.Varint32(&pk.PageNumber)
                Text = ReadString(); // 瀵瑰簲 io.String(&pk.Text)
                PhotoName = ReadString(); // 瀵瑰簲 io.String(&pk.PhotoName)
                break;
            case BookActionType.DeletePage:
                PageNumber = ReadVarInt(); // 瀵瑰簲 io.Varint32(&pk.PageNumber)
                break;
            case BookActionType.SwapPages:
                PageNumber = ReadVarInt(); // 瀵瑰簲 io.Varint32(&pk.PageNumber)
                SecondaryPageNumber = ReadVarInt(); // 瀵瑰簲 io.Varint32(&pk.SecondaryPageNumber)
                break;
            case BookActionType.Finalize:
                Title = ReadString(); // 瀵瑰簲 io.String(&pk.Title)
                Author = ReadString(); // 瀵瑰簲 io.String(&pk.Author)
                XUID = ReadString(); // 瀵瑰簲 io.String(&pk.XUID)
                break;
        }

        base.DecodePacket();
    }
}
