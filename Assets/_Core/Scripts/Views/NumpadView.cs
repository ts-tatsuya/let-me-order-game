using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class NumpadView : MonoBehaviour
{
    [Header("Numpad UI")]
    [Tooltip("Root container for the numpad. This GameObject will be shown when a target field is selected and hidden when Done is pressed.")]
    public GameObject numpadPanel;

    [Tooltip("Buttons used for numeric input. Each button can optionally provide an explicit value to insert.")]
    public Button[] numberButtons = new Button[10];

    [Tooltip("Optional explicit values for each numpad button. If provided, the value is inserted even when the button has no text.")]
    public string[] buttonValues = new string[10];

    [Tooltip("Button that removes the last character or the current selection.")]
    public Button deleteButton;

    [Tooltip("Button that closes the numpad when input is complete.")]
    public Button doneButton;

    [Header("Target Fields")]
    [Tooltip("TMP_InputFields that should open this numpad when clicked.")]
    public TMP_InputField[] targetFields;

    [Tooltip("Optional Unity InputFields that should also open this numpad when clicked.")]
    public InputField[] unityTargetFields;

    private TMP_InputField _currentTMPField;
    private InputField _currentUnityField;
    private int _currentTMPSelectionStart;
    private int _currentTMPSelectionEnd;
    private int _currentUnitySelectionStart;
    private int _currentUnitySelectionEnd;

    private void Awake()
    {
        if (numpadPanel == null)
        {
            numpadPanel = gameObject;
        }

        if (numpadPanel != null)
        {
            numpadPanel.SetActive(false);
        }

        BindNumpadButtons();
        RegisterTargetFields();
    }

    private static readonly string[] DefaultButtonValues = new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

    private void BindNumpadButtons()
    {
        if (numberButtons == null) return;

        for (int i = 0; i < numberButtons.Length; i++)
        {
            var button = numberButtons[i];
            if (button == null) continue;

            var value = GetButtonValue(button, i);
            if (string.IsNullOrEmpty(value))
            {
                continue;
            }

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnNumberButtonPressed(value));
        }

        if (deleteButton != null)
        {
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(DeleteCharacter);
        }

        if (doneButton != null)
        {
            doneButton.onClick.RemoveAllListeners();
            doneButton.onClick.AddListener(HideNumpad);
        }
    }

    private void RegisterTargetFields()
    {
        if (targetFields != null)
        {
            foreach (var field in targetFields)
            {
                if (field == null) continue;

                field.onSelect.RemoveAllListeners();
                field.onSelect.AddListener((eventData) => OnTargetSelected(field));
            }
        }

        if (unityTargetFields != null)
        {
            // foreach (var field in unityTargetFields)
            // {
            //     if (field == null) continue;

            //     field.onSelect.RemoveAllListeners();
            //     field.onSelect.AddListener((eventData) => OnTargetSelected(field));
            // }
        }
    }

    public void RegisterField(TMP_InputField field)
    {
        if (field == null) return;

        var list = new System.Collections.Generic.List<TMP_InputField>(targetFields ?? new TMP_InputField[0]);
        if (!list.Contains(field))
        {
            list.Add(field);
            targetFields = list.ToArray();
        }

        field.onSelect.AddListener((eventData) => OnTargetSelected(field));
    }

    public void RegisterField(InputField field)
    {
        // if (field == null) return;

        // var list = new System.Collections.Generic.List<InputField>(unityTargetFields ?? new InputField[0]);
        // if (!list.Contains(field))
        // {
        //     list.Add(field);
        //     unityTargetFields = list.ToArray();
        // }

        // field.onSelect.AddListener((eventData) => OnTargetSelected(field));
    }

    private void OnTargetSelected(TMP_InputField field)
    {
        _currentTMPField = field;
        _currentUnityField = null;
        _currentTMPSelectionStart = field.selectionStringAnchorPosition;
        _currentTMPSelectionEnd = field.selectionStringFocusPosition;

        if (_currentTMPSelectionStart == _currentTMPSelectionEnd)
        {
            _currentTMPSelectionStart = _currentTMPSelectionEnd = Mathf.Clamp(field.stringPosition, 0, (field.text ?? string.Empty).Length);
        }

        ShowNumpad(true);
        _currentTMPField.ActivateInputField();
    }

    private void OnTargetSelected(InputField field)
    {
        _currentUnityField = field;
        _currentTMPField = null;
        _currentUnitySelectionStart = field.selectionAnchorPosition;
        _currentUnitySelectionEnd = field.selectionFocusPosition;

        if (_currentUnitySelectionStart == _currentUnitySelectionEnd)
        {
            _currentUnitySelectionStart = _currentUnitySelectionEnd = Mathf.Clamp(field.caretPosition, 0, (field.text ?? string.Empty).Length);
        }

        ShowNumpad(true);
        _currentUnityField.ActivateInputField();
    }

    private void OnNumberButtonPressed(string value)
    {
        if (!EnsureActiveField()) return;

        if (_currentTMPField != null)
        {
            InsertIntoTMPField(_currentTMPField, value);
        }
        else if (_currentUnityField != null)
        {
            InsertIntoUnityField(_currentUnityField, value);
        }
    }

    private void DeleteCharacter()
    {
        if (!EnsureActiveField()) return;

        if (_currentTMPField != null)
        {
            DeleteFromTMPField(_currentTMPField);
        }
        else if (_currentUnityField != null)
        {
            DeleteFromUnityField(_currentUnityField);
        }
    }

    public void HideNumpad()
    {
        _currentTMPField = null;
        _currentUnityField = null;
        ShowNumpad(false);
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void ShowNumpad(bool show)
    {
        if (numpadPanel != null)
        {
            numpadPanel.SetActive(show);
        }
    }

    private static void SetCaretPosition(TMP_InputField field, int position)
    {
        field.stringPosition = position;
        field.caretPosition = position;
        field.selectionStringAnchorPosition = position;
        field.selectionStringFocusPosition = position;
    }

    private bool EnsureActiveField()
    {
        if (_currentTMPField != null || _currentUnityField != null)
        {
            return true;
        }

        if (targetFields != null)
        {
            foreach (var field in targetFields)
            {
                if (field != null && field.isFocused)
                {
                    _currentTMPField = field;
                    return true;
                }
            }
        }

        if (unityTargetFields != null)
        {
            foreach (var field in unityTargetFields)
            {
                if (field != null && field.isFocused)
                {
                    _currentUnityField = field;
                    return true;
                }
            }
        }

        return false;
    }

    private void InsertIntoTMPField(TMP_InputField field, string value)
    {
        var text = field.text ?? string.Empty;
        var start = Mathf.Min(_currentTMPSelectionStart, _currentTMPSelectionEnd);
        var end = Mathf.Max(_currentTMPSelectionStart, _currentTMPSelectionEnd);
        start = Mathf.Clamp(start, 0, text.Length);
        end = Mathf.Clamp(end, 0, text.Length);

        if (start != end)
        {
            text = text.Remove(start, end - start);
        }

        text = text.Insert(start, value);
        field.text = text;

        var newCaret = start + value.Length;
        _currentTMPSelectionStart = _currentTMPSelectionEnd = newCaret;
        SetCaretPosition(field, newCaret);
        field.ForceLabelUpdate();
        field.ActivateInputField();
    }

    private void DeleteFromTMPField(TMP_InputField field)
    {
        var text = field.text ?? string.Empty;
        var start = Mathf.Min(_currentTMPSelectionStart, _currentTMPSelectionEnd);
        var end = Mathf.Max(_currentTMPSelectionStart, _currentTMPSelectionEnd);
        start = Mathf.Clamp(start, 0, text.Length);
        end = Mathf.Clamp(end, 0, text.Length);
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

        field.text = text;
        _currentTMPSelectionStart = _currentTMPSelectionEnd = newCaret;
        SetCaretPosition(field, newCaret);
        field.ForceLabelUpdate();
        field.ActivateInputField();
    }

    private void InsertIntoUnityField(InputField field, string value)
    {
        var text = field.text ?? string.Empty;
        var start = Mathf.Min(_currentUnitySelectionStart, _currentUnitySelectionEnd);
        var end = Mathf.Max(_currentUnitySelectionStart, _currentUnitySelectionEnd);
        start = Mathf.Clamp(start, 0, text.Length);
        end = Mathf.Clamp(end, 0, text.Length);

        if (start != end)
        {
            text = text.Remove(start, end - start);
        }

        text = text.Insert(start, value);
        field.text = text;

        var newCaret = start + value.Length;
        _currentUnitySelectionStart = _currentUnitySelectionEnd = newCaret;
        field.caretPosition = newCaret;
        field.selectionAnchorPosition = newCaret;
        field.selectionFocusPosition = newCaret;
        field.ActivateInputField();
    }

    private void DeleteFromUnityField(InputField field)
    {
        var text = field.text ?? string.Empty;
        var start = Mathf.Min(_currentUnitySelectionStart, _currentUnitySelectionEnd);
        var end = Mathf.Max(_currentUnitySelectionStart, _currentUnitySelectionEnd);
        start = Mathf.Clamp(start, 0, text.Length);
        end = Mathf.Clamp(end, 0, text.Length);
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

        field.text = text;
        _currentUnitySelectionStart = _currentUnitySelectionEnd = newCaret;
        field.caretPosition = newCaret;
        field.selectionAnchorPosition = newCaret;
        field.selectionFocusPosition = newCaret;
        field.ActivateInputField();
    }

    private string GetButtonValue(Button button, int index)
    {
        if (button == null) return null;

        if (buttonValues != null && index >= 0 && index < buttonValues.Length)
        {
            var explicitValue = buttonValues[index];
            if (!string.IsNullOrEmpty(explicitValue))
            {
                return explicitValue;
            }
        }

        var tmpText = button.GetComponentInChildren<TMP_Text>();
        if (tmpText != null && !string.IsNullOrEmpty(tmpText.text))
        {
            return tmpText.text;
        }

        var uiText = button.GetComponentInChildren<Text>();
        if (uiText != null && !string.IsNullOrEmpty(uiText.text))
        {
            return uiText.text;
        }

        if (index >= 0 && index < DefaultButtonValues.Length)
        {
            return DefaultButtonValues[index];
        }

        return null;
    }
}
