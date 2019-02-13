using System;
using UnityEngine;
using UnityEngine.UI;
using Eclipse.Base;
using Eclipse.Backend;
using System.Collections.Generic;

namespace Eclipse.Components.Command
{
    [AddComponentMenu("Eclipse/Components/Command/Window")]
    public class CommandWindow : ComponentBase
    {
        public enum ScrollType
        {
            Single, Page, End
        }

        [SerializeField] private Text RecordText;
        [SerializeField] private Text InputText;

        private const int Show = 8;
        private int CurrentPosition = 0;

        private void OnValidate()
        {
            SetComponentName("黑盒子");
        }
        private void Start()
        {
            CurrentPosition = CommandBackend.CommandRecord.Length - Show;
        }
        private void Update()
        {
            DetectionKey();
            UIDisplayer();
        }

        private void DetectionKey()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) Scroll(false, ScrollType.Single);
            if (Input.GetKeyDown(KeyCode.DownArrow)) Scroll(true, ScrollType.Single);
            if (Input.GetKeyDown(KeyCode.PageUp)) Scroll(false, ScrollType.Page);
            if (Input.GetKeyDown(KeyCode.PageDown)) Scroll(true, ScrollType.Page);
            if (Input.GetKeyDown(KeyCode.Home)) Scroll(false, ScrollType.End);
            if (Input.GetKeyDown(KeyCode.End)) Scroll(true, ScrollType.End);
            if (Input.GetKeyDown(KeyCode.A)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "A" : "a");
            else if (Input.GetKeyDown(KeyCode.B)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "B" : "b");
            else if (Input.GetKeyDown(KeyCode.C)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "C" : "c");
            else if (Input.GetKeyDown(KeyCode.D)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "D" : "d");
            else if (Input.GetKeyDown(KeyCode.E)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "E" : "e");
            else if (Input.GetKeyDown(KeyCode.F)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "F" : "f");
            else if (Input.GetKeyDown(KeyCode.G)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "G" : "g");
            else if (Input.GetKeyDown(KeyCode.H)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "H" : "h");
            else if (Input.GetKeyDown(KeyCode.I)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "I" : "i");
            else if (Input.GetKeyDown(KeyCode.J)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "J" : "j");
            else if (Input.GetKeyDown(KeyCode.K)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "K" : "k");
            else if (Input.GetKeyDown(KeyCode.L)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "L" : "l");
            else if (Input.GetKeyDown(KeyCode.M)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "M" : "m");
            else if (Input.GetKeyDown(KeyCode.N)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "N" : "n");
            else if (Input.GetKeyDown(KeyCode.O)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "O" : "o");
            else if (Input.GetKeyDown(KeyCode.P)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "P" : "p");
            else if (Input.GetKeyDown(KeyCode.Q)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "Q" : "q");
            else if (Input.GetKeyDown(KeyCode.R)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "R" : "r");
            else if (Input.GetKeyDown(KeyCode.S)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "S" : "s");
            else if (Input.GetKeyDown(KeyCode.T)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "T" : "t");
            else if (Input.GetKeyDown(KeyCode.U)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "U" : "u");
            else if (Input.GetKeyDown(KeyCode.V)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "V" : "v");
            else if (Input.GetKeyDown(KeyCode.W)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "W" : "w");
            else if (Input.GetKeyDown(KeyCode.X)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "X" : "x");
            else if (Input.GetKeyDown(KeyCode.Y)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "Y" : "y");
            else if (Input.GetKeyDown(KeyCode.Z)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "Z" : "z");

            else if (Input.GetKeyDown(KeyCode.Minus)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "_" : "-");
            else if (Input.GetKeyDown(KeyCode.BackQuote)) EnterStringToCommand("`");
            else if (Input.GetKeyDown(KeyCode.Equals)) EnterStringToCommand(Input.GetKey(KeyCode.LeftShift) ? "+" : "=");
            else if (Input.GetKeyDown(KeyCode.Period)) EnterStringToCommand(".");
            else if (Input.GetKeyDown(KeyCode.DoubleQuote)) EnterStringToCommand("\"");

            else if (Input.GetKeyDown(KeyCode.Alpha0)) EnterStringToCommand("0");
            else if (Input.GetKeyDown(KeyCode.Alpha1)) EnterStringToCommand("1");
            else if (Input.GetKeyDown(KeyCode.Alpha2)) EnterStringToCommand("2");
            else if (Input.GetKeyDown(KeyCode.Alpha3)) EnterStringToCommand("3");
            else if (Input.GetKeyDown(KeyCode.Alpha4)) EnterStringToCommand("4");
            else if (Input.GetKeyDown(KeyCode.Alpha5)) EnterStringToCommand("5");
            else if (Input.GetKeyDown(KeyCode.Alpha6)) EnterStringToCommand("6");
            else if (Input.GetKeyDown(KeyCode.Alpha7)) EnterStringToCommand("7");
            else if (Input.GetKeyDown(KeyCode.Alpha8)) EnterStringToCommand("8");
            else if (Input.GetKeyDown(KeyCode.Alpha9)) EnterStringToCommand("9");

            else if (Input.GetKeyDown(KeyCode.Space)) CommandBackend.InputCommand += " ";
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                CommandBackend.CommandRecordEnter(CommandBackend.CommandEnter(false));
            }
            else if (Input.GetKeyDown(KeyCode.Backspace)) {
                if (CommandBackend.InputCommand.Length > 0)
                {
                    List<char> result = new List<char>(CommandBackend.InputCommand.ToCharArray());
                    result.RemoveAt(result.Count - 1);
                    CommandBackend.InputCommand = new string(result.ToArray());
                }
            }
        }

        private void Scroll(bool Up, ScrollType scrollType)
        {
            Vector2Int range = new Vector2Int(0, CommandBackend.CommandRecord.Length - Show);
            switch (scrollType)
            {
                case ScrollType.Single:
                    if (Up)
                    {
                        ScrollUp();
                    }
                    else
                    {
                        ScrollDown();
                    }
                    break;
                case ScrollType.Page:
                    if (Up)
                    {
                        for(int i = 0; i < 8; i++)
                        {
                            ScrollUp();
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            ScrollDown();
                        }
                    }
                    break;
                case ScrollType.End:
                    if (Up)
                        CurrentPosition = range.y;
                    else
                        CurrentPosition = range.x;
                    break;
            }
        }

        private void ScrollUp()
        {
            Vector2Int range = new Vector2Int(0, CommandBackend.CommandRecord.Length - Show);
            if (CurrentPosition < range.y)
                CurrentPosition++;
        }
        private void ScrollDown()
        {
            if (CurrentPosition > 0)
                CurrentPosition--;
        }

        private void UIDisplayer()
        {
            string stringBuffer = string.Empty;
            for(int i = CurrentPosition; i < CurrentPosition + Show; i++)
            {
                stringBuffer += (CommandBackend.CommandRecord[i] == null || CommandBackend.CommandRecord[i] == "" ? ".." : CommandBackend.CommandRecord[i]) + 
                    (i == CommandBackend.CommandRecord.Length ? "" : "\n");
            }
            RecordText.text = stringBuffer;

            if (InputText)
                InputText.text = ">" + CommandBackend.InputCommand + "_";
        }

        private void EnterStringToCommand(string bb)
        {
            CommandBackend.InputCommand = CommandBackend.InputCommand + bb;
        }

        private void OnDestroy()
        {
            CommandBackend.InputCommand = "";
        }
    }
}
