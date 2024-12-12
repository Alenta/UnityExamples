using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EscapeMenu : MonoBehaviour
{
    private UIDocument _document;
    private Label _continueButton;
    private Label _sceneSelection;
    private GroupBox _EscapeMenu;
    private GroupBox _Group;
    bool visible = false;
    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _EscapeMenu = _document.rootVisualElement.Query<GroupBox>("EscapeMenu");
        _continueButton = _document.rootVisualElement.Query<Label>("Continue");
        _continueButton.RegisterCallback<ClickEvent>(Continue);
        _sceneSelection = _document.rootVisualElement.Query<Label>("SceneSelection");
        _sceneSelection.RegisterCallback<ClickEvent>(SceneSelection);
        _Group = _document.rootVisualElement.Query<GroupBox>("Group");
        
        /*
        _audioSource = GetComponent<AudioSource>();
        */
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ToggleMenu();      
        }
    }

    public void Continue(ClickEvent e)
    {
        ToggleMenu();
    }

    public void SceneSelection(ClickEvent e) {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("SceneSelection");
    }

    public void ToggleMenu()
    {
        if (visible)
        {
            Time.timeScale = 1;
            visible = false;
            _Group.style.visibility = Visibility.Hidden;
            _EscapeMenu.style.visibility = Visibility.Hidden;
        }
        else
        {
            Time.timeScale = 0;
            _EscapeMenu.style.visibility = Visibility.Visible;
            _Group.style.visibility = Visibility.Visible;
            visible = true;
        }
    }
}
