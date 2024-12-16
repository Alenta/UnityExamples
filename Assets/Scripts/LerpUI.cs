using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;


public class LerpUI : MonoBehaviour
{
    public UIDocument UI;
    public Vector2 Offset;
    private List<Label> followLabels = new List<Label>();
    private IReadOnlyList<LerpVisualizer.FollowTask> followTasks;
    public LerpVisualizer _lerpVis;



    private void Start()
    {
        followTasks = _lerpVis.GetFollowTasks();
        foreach (var item in followTasks)
        {
            print(item);
            Label newLabel = new Label();
            newLabel.AddToClassList("tags");
            newLabel.text = $"{Enum.GetName(typeof(InterpolationType), item.Interpolation)}";
            newLabel.style.color = item._Color;
            followLabels.Add(newLabel);
            UI.rootVisualElement.Add(newLabel);
        } 
    }

    private void LateUpdate()
    {
        for (int i = 0; i < followTasks.Count; i++)
        {
            UpdateLabelPosition(followLabels[i], followTasks[i]._Object.transform.position);
        }
    }
    

    void UpdateLabelPosition(Label label, Vector2 position)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
        label.style.left = screenPos.x + Offset.x; // Find an offset value here
        label.style.top = (Screen.height-screenPos.y) + Offset.y; // Find an offset value here
    }
}
