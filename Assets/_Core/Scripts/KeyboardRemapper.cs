using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class KeyboardRemapper : MonoBehaviour
{
    private TMP_InputField inputField;

    private const string Numbers = "1234567890";
    private const string Row1 = "qwertyuiop";
    private const string Row2 = "asdfghjkl";
    private const string Row3 = "zxcvbnm";

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onValidateInput = ValidateInput;
    }

    private char ValidateInput(string text, int charIndex, char addedChar)
    {
        bool isUpper = char.IsUpper(addedChar);
        char lower = char.ToLower(addedChar);

        char mapped = ShiftRight(lower);

        if (isUpper)
            mapped = char.ToUpper(mapped);

        return mapped;
    }

    private char ShiftRight(char c)
    {
        if (TryShift(c, Numbers, out char result)) return result;
        if (TryShift(c, Row1, out result)) return result;
        if (TryShift(c, Row2, out result)) return result;
        if (TryShift(c, Row3, out result)) return result;

        return c;
    }

    private bool TryShift(char c, string row, out char result)
    {
        int index = row.IndexOf(c);

        if (index == -1)
        {
            result = c;
            return false;
        }

        // Wrap around to the beginning of the row
        result = row[(index + 1) % row.Length];
        return true;
    }
}