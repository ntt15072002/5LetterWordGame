using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

enum Validity { None, Valid, Potential, Invalid } 

public class KeyboardKey : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Image renderers;
    [SerializeField] private TextMeshProUGUI lettetText;

    [Header(" Settings ")]
    private Validity validity;

    [Header(" Events ")]
    public static Action<char> onKeyPressed;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SendKeyPressedEvent);
        Initialize();
    }

    void Update()
    {
        
    }

    private void SendKeyPressedEvent()
    {
        onKeyPressed?.Invoke(lettetText.text[0]);
    }

    public char GetLetter()
    {
        return lettetText.text[0];
    }

    public void Initialize()
    {
        renderers.color = Color.white;
        validity = Validity.None;
    }

    public void SetValid()
    {
        renderers.color = Color.green;
        validity = Validity.Valid;
    }

    public void SetPotential()
    {
        if (validity == Validity.Valid)
            return;

        renderers.color = Color.yellow;
        validity = Validity.Potential;

    }

    public void SetInvalid()
    {
        if (validity == Validity.Valid || validity == Validity.Potential)
            return;

        renderers.color = Color.gray;
        validity = Validity.Invalid;

    }

    public bool IsUntouched()
    {
        return validity == Validity.None;
    }
}
