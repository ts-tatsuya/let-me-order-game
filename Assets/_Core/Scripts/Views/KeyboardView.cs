using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyboardView : MonoBehaviour
{
    // Assign Button references in QWERTY order in the inspector.
    // Order string: Q W E R T Y U I O P A S D F G H J K L Z X C V B N M
    [Tooltip("Assign 26 Buttons in QWERTY order: Q W E R T Y U I O P A S D F G H J K L Z X C V B N M")]
    public Button[] qwertyButtons = new Button[26];
    public Button shiftButton;
    public Button modeToggleButton;
    public Button deleteButton;

    [Header("Text Field Targets")]
    public InputField unityInput;
    public TMP_InputField tmpInput;
    [Tooltip("Optional remapper to apply to typed characters from the keyboard view.")]
    public KeyboardRemapper remapper;

    private readonly Dictionary<char, Button> buttonMap = new Dictionary<char, Button>(26);
    private static readonly string[] LetterLayout = new[]
    {
        "Q","W","E","R","T","Y","U","I","O","P",
        "A","S","D","F","G","H","J","K","L",
        "Z","X","C","V","B","N","M"
    };
    private static readonly string[] NumberLayout = new[]
    {
        "1","2","3","4","5","6","7","8","9","0",
        "-","/",";",":","(",")","$","&","@","\"",
        ".",",","?","!","'","#"
    };
    private bool _isUpper = true;
    private bool _isNumberMode = false;

    void Awake()
    {
        ResolveRemapper();
        BuildMapFromArray();
        BindModeButtons();
    }

    private void BindModeButtons()
    {
        if (shiftButton != null)
        {
            shiftButton.onClick.RemoveAllListeners();
            shiftButton.onClick.AddListener(ToggleShift);
        }

        if (modeToggleButton != null)
        {
            modeToggleButton.onClick.RemoveAllListeners();
            modeToggleButton.onClick.AddListener(ToggleNumberMode);
            UpdateModeToggleLabel();
        }

        if (deleteButton != null)
        {
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(DeleteChar);
        }
    }

    // Keep editor assignments in sync when changed in inspector
    void OnValidate()
    {
        BuildMapFromArray();
        UpdateKeyLabels();
        UpdateModeToggleLabel();
    }

    private void BuildMapFromArray()
    {
        buttonMap.Clear();
        if (qwertyButtons == null) return;
        int len = Mathf.Min(qwertyButtons.Length, LetterLayout.Length);
        for (int i = 0; i < len; i++)
        {
            var btn = qwertyButtons[i];
            if (btn == null) continue;
            buttonMap[(char)('A' + i)] = btn;

            // Ensure button types its character into the configured text field when clicked.
            var index = i; // capture index for closure
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => TypeChar(index));
        }

        UpdateKeyLabels();
    }

    void Update()
    {

    }

    private KeyboardRemapper ResolveRemapper()
    {
        if (remapper != null)
        {
            return remapper;
        }

        remapper = GetComponent<KeyboardRemapper>();
        if (remapper == null)
        {
            remapper = GetComponentInParent<KeyboardRemapper>();
        }

        return remapper;
    }

    private void ToggleShift()
    {
        _isUpper = !_isUpper;
        UpdateKeyLabels();
    }

    private void ToggleNumberMode()
    {
        _isNumberMode = !_isNumberMode;
        UpdateModeToggleLabel();
        UpdateKeyLabels();
    }

    private void UpdateModeToggleLabel()
    {
        if (modeToggleButton == null) return;

        var label = modeToggleButton.GetComponentInChildren<Text>();
        if (label != null)
        {
            label.text = _isNumberMode ? "ABC" : "123";
            return;
        }

        var tmpLabel = modeToggleButton.GetComponentInChildren<TMP_Text>();
        if (tmpLabel != null)
        {
            tmpLabel.text = _isNumberMode ? "ABC" : "123";
        }
    }

    private void UpdateKeyLabels()
    {
        if (qwertyButtons == null) return;

        for (int i = 0; i < qwertyButtons.Length && i < LetterLayout.Length; i++)
        {
            var btn = qwertyButtons[i];
            if (btn == null) continue;

            var label = _isNumberMode ? NumberLayout[i] : LetterLayout[i];
            var isUpper = _isUpper && !_isNumberMode;
            var displayedLabel = _isNumberMode ? label : (isUpper ? label : label.ToLowerInvariant());

            var text = btn.GetComponentInChildren<Text>();
            if (text != null)
            {
                text.text = displayedLabel;
            }
            else
            {
                var tmpText = btn.GetComponentInChildren<TMP_Text>();
                if (tmpText != null)
                {
                    tmpText.text = displayedLabel;
                }
            }
        }
    }

    private void TypeChar(int index)
    {
        if (index < 0 || index >= LetterLayout.Length) return;

        var value = _isNumberMode ? NumberLayout[index] : LetterLayout[index];
        var mappedValue = ResolveRemapper()?.Remap(value[0]) ?? value[0];
        var finalValue = _isNumberMode ? mappedValue.ToString() : (_isUpper ? mappedValue.ToString().ToUpperInvariant() : mappedValue.ToString().ToLowerInvariant());

        if (tmpInput != null)
        {
            tmpInput.text = tmpInput.text + finalValue;
            tmpInput.caretPosition = tmpInput.text.Length;
            tmpInput.ForceLabelUpdate();
        }
        else if (unityInput != null)
        {
            unityInput.text = unityInput.text + finalValue;
            unityInput.caretPosition = unityInput.text.Length;
        }
    }

    public void DeleteChar()
    {
        if (tmpInput != null)
        {
            DeleteFromTMPInput();
        }
        else if (unityInput != null)
        {
            DeleteFromUnityInput();
        }
    }

    private void DeleteFromTMPInput()
    {
        var text = tmpInput.text ?? string.Empty;
        var anchor = tmpInput.selectionStringAnchorPosition;
        var focus = tmpInput.selectionStringFocusPosition;
        var start = Mathf.Min(anchor, focus);
        var end = Mathf.Max(anchor, focus);
        var newCaret = start;

        if (start != end)
        {
            text = text.Remove(start, end - start);
        }
        else if (start > 0)
        {
            text = text.Remove(start - 1, 1);
            newCaret = start - 1;
        }
        else
        {
            return;
        }

        tmpInput.text = text;
        tmpInput.stringPosition = newCaret;
        tmpInput.selectionStringAnchorPosition = newCaret;
        tmpInput.selectionStringFocusPosition = newCaret;
        tmpInput.caretPosition = newCaret;
        tmpInput.ForceLabelUpdate();
    }

    private void DeleteFromUnityInput()
    {
        var text = unityInput.text ?? string.Empty;
        var anchor = unityInput.selectionAnchorPosition;
        var focus = unityInput.selectionFocusPosition;
        var start = Mathf.Min(anchor, focus);
        var end = Mathf.Max(anchor, focus);
        var newCaret = start;

        if (start != end)
        {
            text = text.Remove(start, end - start);
        }
        else if (start > 0)
        {
            text = text.Remove(start - 1, 1);
            newCaret = start - 1;
        }
        else
        {
            return;
        }

        unityInput.text = text;
        unityInput.caretPosition = newCaret;
        unityInput.selectionAnchorPosition = newCaret;
        unityInput.selectionFocusPosition = newCaret;
    }

    // Optional helper to get the mapped Button for a letter
    public Button GetButtonForLetter(char ch)
    {
        buttonMap.TryGetValue(char.ToUpperInvariant(ch), out var btn);
        return btn;
    }
}
