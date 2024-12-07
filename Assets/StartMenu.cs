using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartMenu : MonoBehaviour
{
    private UIDocument _document;
    private VisualElement Conveyor;
    private VisualElement LerpVis;
    private VisualElement Shaders;
    private VisualElement Splines;
    private GroupBox _EscapeMenu;
    private GroupBox _Group;

    private void Awake() {
        _document = GetComponent<UIDocument>();
        Conveyor = _document.rootVisualElement.Query<VisualElement>("Conveyor");
        Conveyor.RegisterCallback<ClickEvent>(LoadConveyor);
        LerpVis = _document.rootVisualElement.Query<VisualElement>("LerpVis");
        LerpVis.RegisterCallback<ClickEvent>(LoadLerpVis);
        Shaders = _document.rootVisualElement.Query<VisualElement>("Shaders");
        Shaders.RegisterCallback<ClickEvent>(LoadShaders);
        Splines = _document.rootVisualElement.Query<VisualElement>("Splines");
        Splines.RegisterCallback<ClickEvent>(LoadSplines);
    }

    public void LoadConveyor(EventBase evt) {
        SceneManager.LoadScene("Conveyor");
    }
    public void LoadLerpVis(EventBase evt) {
        SceneManager.LoadScene("LerpVis");
    }
    public void LoadShaders(EventBase evt) {
        SceneManager.LoadScene("Shaders");
    }
    public void LoadSplines(EventBase evt) {

        SceneManager.LoadScene("Splines");
    }
}
