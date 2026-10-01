using CardMatching.Core.Settings;


namespace CardMatching.Core.Interfaces
{
    public interface IGridBoxCardItem
    {
        GridBoxCardData GridBoxData { get; }
        int CardIconIndex { get; }
        GridDimension GridLocation { get; set; }
        bool IsActive { get; } 
    }
}
