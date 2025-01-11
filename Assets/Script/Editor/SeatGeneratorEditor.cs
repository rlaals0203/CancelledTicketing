using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(SeatGroup))]
public class SeatGeneratorEditor : Editor
{
    public VisualTreeAsset VisualTree;

    private Button _seatBtn;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();
        VisualTree.CloneTree(root);

        _seatBtn = root.Q<Button>("Seat");

        return base.CreateInspectorGUI();

    }
}
