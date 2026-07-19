using System;
using TMPro;
using UnityEngine;

public class InstructionController : MonoBehaviour
{
    public TextMeshProUGUI text;
    public String[] instructions;
    private int instructionIndex = 0;

    private void Start()
    {
        text.text = instructions[instructionIndex];
    }

    public void NextInstruction()
    {
        instructionIndex++;
        text.text = instructions[instructionIndex];
    }
}
