using UnityEngine;
using UnityEngine.UIElements;

internal class SeatData
{
    public Button Seat {get; set;}
    public Vector2 Position {get; set;}
    public bool IsChecked {get; set;}

    public SeatData(bool isChecked, Button seat)
    {
        IsChecked = isChecked;
        Seat = seat;
    }
}
