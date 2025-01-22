using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class SeatData
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
public class SeatGeneratorEditor : EditorWindow
{
    private const int Vert = 40;
    private const int Horizon = 50;

    private int vert;
    private int horizon;
    
    private SeatData[,] datas = new SeatData[Vert, Horizon];
    private SeatData currentData;
    private SeatData prevData;

    private Button seat;
    private Button generateSeat;
    private VisualElement background;
    private TextField prefabTextField;
    
    [MenuItem("Tools/SeatGenerator")]
    public static void ShowWindow()
    {
        SeatGeneratorEditor window = GetWindow<SeatGeneratorEditor>();
        window.titleContent = new GUIContent("SeatGeneratorEditor");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        VisualTreeAsset asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/CustomSeatGenerator.uxml");
        VisualElement tree = asset.Instantiate();
        root.Add(tree);

        #region AssignElements
        seat = tree.Q<Button>("seat");
        generateSeat = tree.Q<Button>("generate-btn");
        background = tree.Q<VisualElement>("seat-background");
        prefabTextField = tree.Q<TextField>("seat-name");
        #endregion
        
        for (int y = 0; y < Vert; y++)
        {   
            for (int x = 0; x < Horizon; x++)
            {
                Button button = new Button();
                datas[y, x] = new SeatData(false, button);
                button.name = "seat";
                button.userData = datas[y, x];
                
                button.clicked += () => OnSeatClicked(button);
                background.Add(button);
            }
        }
        
        generateSeat.clicked += () => GenerateSeat(generateSeat);
    }

    private void OnSeatClicked(Button button)
    { 
        Debug.Log("멀티");

        SeatData data = button.userData as SeatData;
        prevData = data;
        data.Position = new Vector2(vert, horizon);
        data.IsChecked = !data.IsChecked;
        currentData = data;

        if (Event.current.isKey)
            OnMultiSelect();
        else
            SetSeatSelect(currentData);
    }

    private void SetSeatSelect(SeatData data, bool isMulti = false)   
    {
        if(data.IsChecked)
            data.Seat.style.backgroundColor = Color.green;
        else
            data.Seat.style.backgroundColor = Color.white;

        if (isMulti)
        {
            data.Seat.style.backgroundColor = Color.green;
            data.IsChecked = true;
        }
    }

    private void OnMultiSelect()
    {
        for (int i = (int)prevData.Position.y; i < (int)currentData.Position.y; i++)
        {
            for (int j = (int)prevData.Position.x; j < (int)currentData.Position.x; j++)
            {
                SetSeatSelect(datas[j, i], true);
            }
        }
    }

    private void GenerateSeat(Button button)
    {
        GameObject prefab = new GameObject();
        prefab.name = prefabTextField.value;

        for (int i = 0; i < Vert; i++)
        {
            for (int j = 0; j < Horizon; j++)
            {
                
            }
        }
    }
}