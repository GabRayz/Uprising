using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//public enum Control
//{
//    forward,
//    backward,
//    right,
//    left,
//    jump,
//    select1,
//    select2,
//    select3,
//    select4,
//}

public class InputManager
{
    private Dictionary<string, string> inputs;

    public InputManager()
    {
        inputs = new Dictionary<string, string>();
        LoadControlsFromConfig();
    }

    private void LoadControlsFromConfig()
    {
        if (File.Exists("/Users/gabriel/Desktop/Uprising/controls.config"))
        {
            using (StreamReader myReader = new StreamReader("/Users/gabriel/Desktop/Uprising/controls.config"))
            {
                string line = myReader.ReadLine();
                while (line != null)
                {
                    string[] PairCommandeInput = line.Split('=');
                    inputs.Add(PairCommandeInput[0], PairCommandeInput[1]);
                    Debug.Log("Added control : " + PairCommandeInput[0]);
                    try
                    {
                        line = myReader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        line = null;
                    }
                }
            }
        }
        else Debug.LogError("File not found");
    }

    public void SetControlFromString(string control, string input)
    {

    }

    public void SaveControls()
    {
        using (StreamWriter myWriter = new StreamWriter("/Users/gabriel/Desktop/Uprising/controls.config"))
        {
            foreach(var input in inputs)
            {
                myWriter.WriteLine("{0}={1}", input.Key, input.Value);
            }
        }
    }

    public bool GetButtonDown(string button)
    {
        if (Input.anyKeyDown)
            return inputs[button] == Input.inputString;
        return false;
    }

    public bool GetButton(string button)
    {
        return inputs[button] == Input.inputString;
    }

    //public Control Get(string controlName)
    //{
    //    Control control = Control.forward;
    //    switch (controlName)
    //    {
    //        case "forward":
    //            control = Control.forward;
    //            break;
    //        case "backward":
    //            control = Control.backward;
    //            break;
    //        case "left":
    //            control = Control.left;
    //            break;
    //        case "right":
    //            control = Control.right;
    //            break;
    //        case "jump":
    //            control = Control.jump;
    //            break;
    //        case "select1":
    //            control = Control.select1;
    //            break;
    //        case "select2":
    //            control = Control.select2;
    //            break;
    //        case "select3":
    //            control = Control.select3;
    //            break;
    //        case "select4":
    //            control = Control.select4;
    //            break;
    //    }

    //    return control;
    //}
}
