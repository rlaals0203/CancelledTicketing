using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class SeatGeneratorEditor : EditorWindow
{
    private const int Vert = 40;
    private const int Horizon = 50;
    private bool[,] selectedCells = new bool[Vert, Horizon];

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

    public void OnGUI()
    {
        
        
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
                button.name = "seat";
                button.clicked += OnSeatClicked;
                background.Add(button);
            }
        }
    }

    private void OnSeatClicked()
    {
        
    }
}