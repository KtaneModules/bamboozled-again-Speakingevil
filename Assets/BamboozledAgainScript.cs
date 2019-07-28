using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BamboozledAgainScript : MonoBehaviour {

    public KMAudio Audio;
    public KMBombInfo Bomb;
    public KMSelectable[] buttons;
    public KMSelectable[] leds;
    public Renderer[] objectID;
    public Material[] objectColours;
    public TextMesh displayText;
    public TextMesh[] buttonText;
    public Font[] boozle;
    public Material[] textID;
    public GameObject matStore;

    private int[][] answerKey = new int[2][] { new int[4], new int[4]};
    private int[][] inputs = new int[2][] { new int[4], new int[4]};
    private string[] ech = new string[15] { "WHITE", "RED", "ORANGE", "YELLOW", "LIME", "GREEN", "JADE", "GREY", "CYAN", "AZURE", "BLUE", "VIOLET", "MAGENTA", "ROSE", "BLACK" };
    private string[] location = new string[6] { "TL", "TM", "TR", "BL", "BM", "BR" };
    private int pressCount;
    private IEnumerator textCycle;
    private int[] textRandomiser = new int[8];
    private int[] screenVals = new int[8];
    private int[] buttonVals = new int[6];
    private int[] finalVals = new int[6];
    private int[] sortedVals = new int[6];
    private int[] xvals = new int[5];
    private string[][] buttonRandomiser = new string[2][] { new string[6], new string[6]};
    private string[][] message = new string[4][] { new string[8], new string[8], new string[8], new string[8]};
    private int[][] vals = new int[4][] { new int[8], new int[8], new int[8], new int[8] }; 
    private string[] textField = new string[80] {"THE#LETTER", "ONE#LETTER", "THE#COLOUR", "ONE#COLOUR", "THE#PHRASE", "ONE#PHRASE"
    ,"THEN", "NEXT", "ALPHA", "BRAVO", "CHARLIE", "DELTA", "ECHO", "GOLF", "KILO", "QUEBEC", "TANGO", "WHISKEY", "VICTOR", "YANKEE"
    ,"ECHO#ECHO", "E#THEN#E", "ALPHA#PAPA", "PAPA#ALPHA", "PAPHA#ALPA", "T#GOLF", "TANGOLF", "WHISKEE", "WHISKY", "CHARLIE#C"
    ,"C#CHARLIE", "YANGO", "DELTA#NEXT", "CUEBEQ", "MILO", "KI#LO", "HI-LO", "VVICTOR", "VICTORR", "LIME#BRAVO", "BLUE#BRAVO"
    ,"G#IN#JADE", "G#IN#ROSE", "BLUE#IN#RED", "YES#BUT#NO", "COLOUR", "MESSAGE", "CIPHER", "BUTTON", "TWO#BUTTONS", "SIX#BUTTONS"
    ,"I#GIVE#UP", "ONE#ELEVEN", "ONE#ONE#ONE", "THREE#ONES", "WHAT?", "THIS?", "THAT?", "BLUE!", "ECHO!", "BLANK", "BLANK?!", "NOTHING"
    ,"YELLOW#TEXT", "BLACK#TEXT?", "QUOTE#V", "END#QUOTE", "\"QUOTE#K\""
    ,"IN#RED", "ORANGE", "IN#YELLOW", "LIME", "IN#GREEN", "JADE", "IN#CYAN", "AZURE", "IN#BLUE", "VIOLET", "IN#MAGENTA", "ROSE"};
    private string[] puncField = new string[8] { "#", "'", "\"", "?", "-", "*", "~", "!" };
    private int[] valField = new int[80] { 40, 24, 32, 39, 20, 15, 0, 0, 70, 84, 83, 61, 66, 46, 68, 56, 80, 54, 65, 41, 84, 60,
    56, 86, 69, 50, 62, 78, 64, 43, 41, 51, 47, 57, 45, 46, 86, 84, 82, 78, 47, 59, 63, 42, 89, 77, 70, 55, 67, 79, 71, 58, 86,
    58, 88, 49, 78, 68, 75, 45, 44, 56, 72, 70, 46, 73, 66, 52, 48, 69, 41, 58, 84, 47, 45, 55, 83, 74, 51, 67};
    int finalpunc;
    int step;
    bool pause;

    //Logging
    static int moduleCounter = 1;
    int moduleID;
    private bool moduleSolved;

    private void Awake()
    {
        moduleID = moduleCounter++;
        foreach (KMSelectable led in leds)
        {
            int l = leds.ToList().IndexOf(led);
            led.OnInteract += delegate () { LEDPress(l); return false; };
        }
        foreach (KMSelectable button in buttons)
        {
            int b = buttons.ToList().IndexOf(button);
            button.OnInteract += delegate () { ButtonOn(b); return false; };
        }
    }

    void Start () {
        matStore.SetActive(false);
        textCycle = TextCycle();
        if (moduleID == 1)
        {
            GetComponent<KMAudio>().PlaySoundAtTransform("Klaxon", transform);
        }
        Reset();
	}

    private void Reset()
    {
        pause = false;
        for (int i = 0; i < 6; i++)
        {
            buttonText[i].fontSize = 144;
            if(i < 4)
            {
                objectID[i + 6].material = objectColours[14];
            }
        }
        string[][] strvals = new string[4][] { new string[8], new string[8], new string[8], new string[8] };
        List<string> strv = new List<string> { };
        List<string> striv = new List<string> { };
        List<string> strscreen = new List<string> { };
        List<string> textcol = new List<string> { };
        int[] intermediary = new int[8];
        for (int i = 0; i < 8; i++)
        {
            textRandomiser[i] = UnityEngine.Random.Range(0, 14);
            switch (i)
            {
                case 0:
                case 2:
                case 4:
                    message[0][i] = textField[UnityEngine.Random.Range(0, 6)];
                    break;
                case 1:
                case 3:
                    message[0][i] = textField[UnityEngine.Random.Range(6, 8)];
                    break;
                default:
                    message[0][i] = textField[UnityEngine.Random.Range(8, 80)];
                    break;
            }
            vals[0][i] = UnityEngine.Random.Range(0, message[0][i].Length);
            vals[1][i] = UnityEngine.Random.Range(0, 8);
            vals[2][i] = UnityEngine.Random.Range(1, 27);
            vals[3][i] = UnityEngine.Random.Range(11, 17);
            for(int j = 0; j < 4; j++)
            {
                strvals[j][i] = vals[j][i].ToString();
            }
            List<char> rot = new List<char> { };
            for (int j = 0; j < message[0][i].Length; j++)
            {
                char symbol = message[0][i][j];
                rot.Add(message[0][i][(j + vals[0][i]) % message[0][i].Length]);
            }           
            message[1][i] = new string(rot.ToArray());
            message[2][i] = puncField[vals[1][i]] + message[1][i] + puncField[vals[1][i]];
            List<string> ciph = new List<string> { };
            for (int j = 0; j < message[2][i].Length; j++)
            {
                char symbol = message[2][i][j];
                bool punc = puncField.Contains(symbol.ToString());
                if (punc == true)
                {
                    ciph.Add(puncField[(puncField.ToList().IndexOf(symbol.ToString()) + vals[2][i]) % 8]);
                }
                else
                {
                    ciph.Add("ABCDEFGHIJKLMNOPQRSTUVWXYZ"[("ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(message[2][i][j]) + vals[2][i]) % 26].ToString());
                }
            }
            message[3][i] = String.Join(String.Empty, ciph.ToArray());
            int v = valField[textField.ToList().IndexOf(message[0][i])];
            int[] digit = new int[2];
            digit[0] = (int)Math.Floor(v / 10f);
            digit[1] = v - digit[0] * 10;
            switch (textRandomiser[i])
            {
                case 0:
                    intermediary[i] = v;
                    break;
                case 1:
                    intermediary[i] = v - digit[0];
                    break;
                case 2:
                    intermediary[i] = digit[0] * 11;
                    break;
                case 3:
                    intermediary[i] = v + digit[1];
                    break;
                case 4:
                    intermediary[i] = v - Math.Max(digit[0], digit[1]);
                    break;
                case 5:
                    intermediary[i] = v - digit[0] - digit[1];
                    break;
                case 6:
                    intermediary[i] = v - 2 * digit[0];
                    break;
                case 7:
                    intermediary[i] = digit[1] * 10 + digit[0];
                    break;
                case 8:
                    intermediary[i] = digit[0] * 10;
                    break;
                case 9:
                    intermediary[i] = digit[1] * 11;
                    break;
                case 10:
                    intermediary[i] = v + digit[0];
                    break;
                case 11:
                    intermediary[i] = v - Math.Min(digit[0], digit[1]);
                    break;
                case 12:
                    intermediary[i] = v - Math.Abs(digit[0] - digit[1]);
                    break;
                case 13:
                    intermediary[i] = v - 2 * digit[1];
                    break;
            }
            textcol.Add(ech[textRandomiser[i]]);
            screenVals[i] = intermediary[i] + 5 * vals[0][i] + 2 * (vals[2][i] + vals[3][i]);
            if (i != 1 && i != 3)
            {
                strv.Add(v.ToString());
                striv.Add(intermediary[i].ToString());
                strscreen.Add(screenVals[i].ToString());
            }
            else
            {
                strv.Add("/");
                striv.Add("/");
                strscreen.Add("/");
            }
        }
        Debug.LogFormat("[Bamboozled Again #{0}] The unencrypted message reads:\n[Bamboozled Again #{0}]{1}", moduleID, String.Join(" - ", message[0]));
        Debug.LogFormat("[Bamboozled Again #{0}] The A values are: {1}", moduleID, String.Join(", ", strvals[0]));
        Debug.LogFormat("[Bamboozled Again #{0}] The rotated message reads:\n[Bamboozled Again #{0}]{1}", moduleID, String.Join(" - ", message[1]));
        Debug.LogFormat("[Bamboozled Again #{0}] The appended message reads:\n[Bamboozled Again #{0}]{1}", moduleID, String.Join(" - ", message[2]));
        Debug.LogFormat("[Bamboozled Again #{0}] The B values are: {1}", moduleID, String.Join(", ", strvals[2]));
        Debug.LogFormat("[Bamboozled Again #{0}] The encrypted message reads:\n[Bamboozled Again #{0}]{1}", moduleID, String.Join(" - ", message[3]));
        Debug.LogFormat("[Bamboozled Again #{0}] The C values are: {1}", moduleID, String.Join(", ", strvals[3]));
        Debug.LogFormat("[Bamboozled Again #{0}] The raw text values are: {1}", moduleID, String.Join(", ", strv.ToArray()));
        Debug.LogFormat("[Bamboozled Again #{0}] The text colours are: {1}", moduleID, String.Join(", ", textcol.ToArray()));
        Debug.LogFormat("[Bamboozled Again #{0}] The modified text values are: {1}", moduleID, String.Join(", ", striv.ToArray()));
        Debug.LogFormat("[Bamboozled Again #{0}] The final text values are: {1}", moduleID, String.Join(", ", strscreen.ToArray()));
        StartCoroutine(textCycle);
        for(int i = 0; i < 6; i++)
        {
            ButtonSet(i);
        }
        AnswerOrder();
    }

    void ButtonSet(int b)
    {
        int q = 0;
        int p = UnityEngine.Random.Range(0, 15);
        while (true)
        {
            q = UnityEngine.Random.Range(0, 80);
            if(q != 6 && q != 7)
            {
                break;
            }
        }
        buttonRandomiser[0][b] = ech[p];
        buttonRandomiser[1][b] = textField[q];
        Debug.LogFormat("[Bamboozled Again #{0}] The {1} button has the colour: {2}\n[Bamboozled Again #{0}] and the text: {3}", moduleID, location[b], buttonRandomiser[0][b], buttonRandomiser[1][b]);
        objectID[b].material = objectColours[p];
        buttonText[b].text = buttonRandomiser[1][b].Replace('#', '\n');
        if(p == 14)
        {
            buttonVals[b] = 30;
            buttonText[b].color = new Color32(255, 255, 255, 255);
        }
        else if (p == 0 || p == 7)
        {
            buttonVals[b] = 20;
            buttonText[b].color = new Color32(0, 0, 0, 255);
        }
        else
        {
            buttonVals[b] = 0;
            buttonText[b].color = new Color32(0, 0, 0, 255);
        }
        for(int i = 0; i < 8; i++)
        {          
            if(p == textRandomiser[i])
            {
                buttonVals[b] += 15;
            }
            if(q == textField.ToList().IndexOf(message[0][i]))
            {
                buttonVals[b] += 60;
            }
            if((p == 14 && textRandomiser[i] == 0) || (p > 0 && p < 7 && p + 7 == textRandomiser[i]) || (p > 7 && p < 14 && p - 7 == textRandomiser[i]))
            {
               buttonVals[b] += 5;               
            }
        }
        switch (b)
        {
            case 0:
            case 3:
                finalVals[b] = 3 * buttonVals[b] + 2 * (screenVals[0] + screenVals[5]);
                break;
            case 1:
            case 4:
                finalVals[b] = 3 * buttonVals[b] + 2 * (screenVals[2] + screenVals[6]);
                break;
            case 2:
            case 5:
                finalVals[b] = 3 * buttonVals[b] + 2 * (screenVals[4] + screenVals[7]);
                break;
        }
    }

    void AnswerOrder()
    {
        Debug.Log("[Bamboozled Again #" + moduleID + "] The initial button values are: " + buttonVals[0] + ", " + buttonVals[1] + ", " + buttonVals[2] + ", " + buttonVals[3] + ", " + buttonVals[4] + ", " + buttonVals[5]);
        Debug.Log("[Bamboozled Again #" + moduleID + "] The final button values are: " + finalVals[0] + ", " + finalVals[1] + ", " + finalVals[2] + ", " + finalVals[3] + ", " + finalVals[4] + ", " + finalVals[5]);
        for (int i = 0; i < 6; i++)
        {
            sortedVals[i] = finalVals[i];
        }
        Array.Sort(sortedVals);
        answerKey[0][pressCount] = finalVals.ToList().IndexOf(sortedVals[pressCount + 2]);
        Debug.LogFormat("[Bamboozled Again #{0}] After {2} presses, the correct button to press is the {1} button", moduleID, location[answerKey[0][pressCount]], pressCount);
        if(pressCount < 3)
        { 
            int v = valField[textField.ToList().IndexOf(buttonRandomiser[1][answerKey[0][pressCount]])];
            int[] digit = new int[2];
            digit[0] = (int)Math.Floor(v / 10f);
            digit[1] = v - digit[0] * 10;
            switch (ech.ToList().IndexOf(buttonRandomiser[0][answerKey[0][pressCount]]))
            {
                case 0:
                   v = Math.Max(digit[0], digit[1]);
                   break;
                case 1:
                   v = digit[0] - digit[1];
                   break;
                case 2:
                   v = v % 9;
                   if (v == 0)
                   {
                       v = 9;
                   }
                   break;
                case 3:
                   v = digit[0];
                   break;
                case 4:
                   v = v % 9;
                   if (v == 0)
                   {
                       v = 9;
                   }
                   v = digit[0] - v;
                   break;
                case 5:
                   v = digit[0] + digit[1];
                   break;
                case 6:
                   v = 2 * digit[0];
                   break;
                case 7:
                   v = v % 9;
                   if (v == 0)
                   {
                       v = 9;
                   }
                   v = digit[0] + digit[1] - v;
                   break;
                case 8:
                   v = digit[1] - digit[0];
                   break;
                case 9:
                   v = -(v % 9);
                   if (v == 0)
                   {
                       v = -9;
                   }
                   break;
                case 10:
                   v = digit[1];
                   break;
               case 11:
                   v = v % 9;
                   if (v == 0)
                   {
                       v = 9;
                   }
                   v = digit[1] - v;
                   break;
               case 12:
                   v = 10 - digit[0] - digit[1];
                   break;
               case 13:
                   v = 2 * digit[1];
                   break;
               case 14:
                   v = Math.Min(digit[0], digit[1]);
                   break;
            }
            if (pressCount == 0)
            {
               xvals[0] = v;
            }
            else
            {
                if (message[0][(2 * pressCount) - 1] == "THEN")
                {
                    xvals[pressCount] = v + xvals[pressCount - 1];
                }
                else
                {
                    xvals[pressCount] = v - xvals[pressCount - 1];
                }
            }
            Debug.LogFormat("[Bamboozled Again #{0}] The X value of the {1} button is {2}", moduleID, location[answerKey[0][pressCount]], v);
            Debug.LogFormat("[Bamboozled Again #{0}] The Y value of the {1} button is {2}", moduleID, location[answerKey[0][pressCount]], xvals[pressCount]);   
            switch (vals[1][7 - pressCount])
            {
                case 0:
                    answerKey[1][pressCount] = (xvals[pressCount] + 90) % 10;
                    Debug.LogFormat("[Bamboozled Again #{0}] The {1} button should be pressed when the last digit of the timer is {2}", moduleID, location[answerKey[0][pressCount]], answerKey[1][pressCount]);
                    break;
                case 1:
                    answerKey[1][pressCount] = ((xvals[pressCount] + 90) % 9) + 3;
                    Debug.LogFormat("[Bamboozled Again #{0}] The {1} button should be pressed when the sum of the last two digits of the timer is {2}", moduleID, location[answerKey[0][pressCount]], answerKey[1][pressCount]);
                    break;
                case 2:
                    answerKey[1][pressCount] = (2 * (xvals[pressCount] + 45) % 9) + 3;
                    Debug.LogFormat("[Bamboozled Again #{0}] The {1} button should be pressed when the sum of the last two digits of the timer is {2}", moduleID, location[answerKey[0][pressCount]], answerKey[1][pressCount]);
                    break;
                case 3:
                    answerKey[1][pressCount] = (xvals[pressCount] + 45) % 5;
                    Debug.LogFormat("[Bamboozled Again #{0}] The {1} button should be pressed when the difference between the last two digits of the timer is {2}", moduleID, location[answerKey[0][pressCount]], answerKey[1][pressCount]);
                    break;
                case 4:
                    answerKey[1][pressCount] = 9 - ((xvals[pressCount] + 90) % 10);
                    Debug.LogFormat("[Bamboozled Again #{0}] The {1} button should be pressed when the last digit of the timer is {2}", moduleID, location[answerKey[0][pressCount]], answerKey[1][pressCount]);
                    break;
                case 5:
                    answerKey[1][pressCount] = 11 - ((xvals[pressCount] + 90) % 9);
                    Debug.LogFormat("[Bamboozled Again #{0}] The {1} button should be pressed when the sum of the last two digits of the timer is {2}", moduleID, location[answerKey[0][pressCount]], answerKey[1][pressCount]);
                    break;
                case 6:
                    answerKey[1][pressCount] = 11 - (2 * (xvals[pressCount] + 45) % 9);
                    Debug.LogFormat("[Bamboozled Again #{0}] The {1} button should be pressed when the sum of the last two digits of the timer is {2}", moduleID, location[answerKey[0][pressCount]], answerKey[1][pressCount]);
                    break;
                case 7:
                    answerKey[1][pressCount] = 2 * (xvals[pressCount] + 45) % 5;
                    Debug.LogFormat("[Bamboozled Again #{0}] The {1} button should be pressed when the difference between the last two digits of the timer is {2}", moduleID, location[answerKey[0][pressCount]], answerKey[1][pressCount]);
                    break;
            }
        }
        else
        {
            int[] x = new int[2];
            x[0] = valField[textField.ToList().IndexOf(buttonRandomiser[1][answerKey[0][3]])];
            x[1] = x[0];
            int[] digit = new int[2];
            digit[0] = (int)Math.Floor(x[0] / 10f);
            digit[1] = x[0] - digit[0] * 10;
            for(int i = 0; i < 2; i++)
            {
                switch (textRandomiser[2 * i + 1])
                {
                    case 1:
                        x[i] = x[i] - digit[0];
                        break;
                    case 2:
                        x[i] = digit[0] * 11;
                        break;
                    case 3:
                        x[i] = x[i] + digit[1];
                        break;
                    case 4:
                        x[i] = x[i] - Math.Max(digit[0], digit[1]);
                        break;
                    case 5:
                        x[i] = x[i] - digit[0] - digit[1];
                        break;
                    case 6:
                        x[i] = x[i] - 2 * digit[0];
                        break;
                    case 7:
                        x[i] = digit[1] * 10 + digit[0];
                        break;
                    case 8:
                        x[i] = digit[0] * 10;
                        break;
                    case 9:
                        x[i] = digit[1] * 11;
                        break;
                    case 10:
                        x[i] = x[i] + digit[0];
                        break;
                    case 11:
                        x[i] = x[i] - Math.Min(digit[0], digit[1]);
                        break;
                    case 12:
                        x[i] = x[i] - Math.Abs(digit[0] - digit[1]);
                        break;
                    case 13:
                        x[i] = x[i] - 2 * digit[1];
                        break;
                }
                Debug.LogFormat("[Bamboozled Again #{0}] S{1} = {2}", moduleID, i + 1, x[i]);
            }
            for (int i = 0; i < 2; i++)
            {
                if (x[i] != 0)
                {
                    digit[0] = (int)Math.Floor(x[i] / 10f);
                    digit[1] = x[i] - digit[0] * 10;
                    switch (ech.ToList().IndexOf(buttonRandomiser[0][answerKey[0][3]]))
                    {
                        case 0:
                            x[i] = Math.Max(digit[0], digit[1]);
                            break;
                        case 1:
                            x[i] = digit[0] - digit[1];
                            break;
                        case 2:
                            x[i] = x[i] % 9;
                            if (x[i] == 0)
                            {
                                x[i] = 9;
                            }
                            break;
                        case 3:
                            x[i] = digit[0];
                            break;
                        case 4:
                            x[i] = x[i] % 9;
                            if (x[i] == 0)
                            {
                                x[i] = 9;
                            }
                            x[i] = digit[0] - x[i];
                            break;
                        case 5:
                            x[i] = digit[0] + digit[1];
                            break;
                        case 6:
                            x[i] = 2 * digit[0];
                            break;
                        case 7:
                            x[i] = x[i] % 9;
                            if (x[i] == 0)
                            {
                                x[i] = 9;
                            }
                            x[i] = digit[0] + digit[1] - x[i];
                            break;
                        case 8:
                            x[i] = digit[1] - digit[0];
                            break;
                        case 9:
                            x[i] = -(x[i] % 9);
                            if (x[i] == 0)
                            {
                                x[i] = -9;
                            }
                            break;
                        case 10:
                            x[i] = digit[1];
                            break;
                        case 11:
                            x[i] = x[i] % 9;
                            if (x[i] == 0)
                            {
                                x[i] = 9;
                            }
                            x[i] = digit[1] - x[i];
                            break;
                        case 12:
                            x[i] = 10 - digit[0] - digit[1];
                            break;
                        case 13:
                            x[i] = 2 * digit[1];
                            break;
                        case 14:
                            x[i] = Math.Min(digit[0], digit[1]);
                            break;
                    }
                }
                Debug.LogFormat("[Bamboozled Again #{0}] X{1} = {2}", moduleID, i + 1, x[i]);
            }
            int z = 0;
            if(message[0][1] == message[0][3])
            {
                z = x[0] + x[1];
            }
            else
            {
                z = Math.Abs(x[0] - x[1]);
            }
            Debug.LogFormat("[Bamboozled Again #{0}] The Y value of the {1} button is {2}", moduleID, location[answerKey[0][pressCount]], z);
            if (vals[1][1] == vals[1][3])
            {
                finalpunc = vals[1][1];
            }
            else
            {
                finalpunc = (vals[1][1] + vals[1][3]) % 8;
            }
            switch (finalpunc)
            {
                case 0:
                    answerKey[1][pressCount] = (z + 90) % 10;
                    Debug.LogFormat("[Bamboozled Again #{0}] The {1} button should be pressed when the last digit of the timer is {2}", moduleID, location[answerKey[0][pressCount]], answerKey[1][pressCount]);
                    break;
                case 1:
                    answerKey[1][pressCount] = ((z + 90) % 9) + 3;
                    Debug.LogFormat("[Bamboozled Again #{0}] The {1} button should be pressed when the sum of the last two digits of the timer is {2}", moduleID, location[answerKey[0][pressCount]], answerKey[1][pressCount]);
                    break;
                case 2:
                    answerKey[1][pressCount] = (2 * (z + 45) % 9) + 3;
                    Debug.LogFormat("[Bamboozled Again #{0}] The {1} button should be pressed when the sum of the last two digits of the timer is {2}", moduleID, location[answerKey[0][pressCount]], answerKey[1][pressCount]);
                    break;
                case 3:
                    answerKey[1][pressCount] = (z + 45) % 5;
                    Debug.LogFormat("[Bamboozled Again #{0}] The {1} button should be pressed when the difference between the last two digits of the timer is {2}", moduleID, location[answerKey[0][pressCount]], answerKey[1][pressCount]);
                    break;
                case 4:
                    answerKey[1][pressCount] = 9 - ((z + 90) % 10);
                    Debug.LogFormat("[Bamboozled Again #{0}] The {1} button should be pressed when the last digit of the timer is {2}", moduleID, location[answerKey[0][pressCount]], answerKey[1][pressCount]);
                    break;
                case 5:
                    answerKey[1][pressCount] = 11 - ((z + 90) % 9);
                    Debug.LogFormat("[Bamboozled Again #{0}] The {1} button should be pressed when the sum of the last two digits of the timer is {2}", moduleID, location[answerKey[0][pressCount]], answerKey[1][pressCount]);
                    break;
                case 6:
                    answerKey[1][pressCount] = 11 - (2 * (z + 45) % 9);
                    Debug.LogFormat("[Bamboozled Again #{0}] The {1} button should be pressed when the sum of the last two digits of the timer is {2}", moduleID, location[answerKey[0][pressCount]], answerKey[1][pressCount]);
                    break;
                case 7:
                    answerKey[1][pressCount] = 2 * (z + 45) % 5;
                    Debug.LogFormat("[Bamboozled Again #{0}] The {1} button should be pressed when the difference between the last two digits of the timer is {2}", moduleID, location[answerKey[0][pressCount]], answerKey[1][pressCount]);
                    break;
            }
        }
    }

    void LEDPress(int l)
    {
        if (moduleSolved == false)
        {
            GetComponent<KMSelectable>().AddInteractionPunch(0.2f);
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            switch (l)
            {
                case 0:
                    if (pause == true)
                    {
                        if (step > 0)
                        {
                            step--;
                            switch (textRandomiser[step])
                            {
                                case 0:
                                    displayText.color = new Color32(192, 192, 192, 255);
                                    break;
                                case 1:
                                    displayText.color = new Color32(192, 0, 0, 255);
                                    break;
                                case 2:
                                    displayText.color = new Color32(192, 96, 0, 255);
                                    break;
                                case 3:
                                    displayText.color = new Color32(192, 192, 0, 255);
                                    break;
                                case 4:
                                    displayText.color = new Color32(96, 192, 0, 255);
                                    break;
                                case 5:
                                    displayText.color = new Color32(0, 192, 0, 255);
                                    break;
                                case 6:
                                    displayText.color = new Color32(0, 192, 96, 255);
                                    break;
                                case 7:
                                    displayText.color = new Color32(96, 96, 96, 255);
                                    break;
                                case 8:
                                    displayText.color = new Color32(0, 192, 192, 255);
                                    break;
                                case 9:
                                    displayText.color = new Color32(0, 96, 192, 255);
                                    break;
                                case 10:
                                    displayText.color = new Color32(0, 0, 192, 255);
                                    break;
                                case 11:
                                    displayText.color = new Color32(96, 0, 192, 255);
                                    break;
                                case 12:
                                    displayText.color = new Color32(192, 0, 192, 255);
                                    break;
                                case 13:
                                    displayText.color = new Color32(192, 0, 96, 255);
                                    break;
                            }
                            displayText.GetComponent<Renderer>().material = textID[vals[3][step] - 11];
                            displayText.font = boozle[vals[3][step] - 11];
                            displayText.text = message[3][step];
                        }
                    }
                    break;
                case 1:
                    if(pause == true)
                    {
                        pause = false;
                        StartCoroutine(textCycle);
                    }
                    break;
                case 2:
                    if (pause == false)
                    {
                        pause = true;
                        StopCoroutine(textCycle);
                    }
                    break;
                case 3:
                    if (pause == true)
                    {
                        if (step < 7)
                        {
                            step++;
                            switch (textRandomiser[step])
                            {
                                case 0:
                                    displayText.color = new Color32(192, 192, 192, 255);
                                    break;
                                case 1:
                                    displayText.color = new Color32(192, 0, 0, 255);
                                    break;
                                case 2:
                                    displayText.color = new Color32(192, 96, 0, 255);
                                    break;
                                case 3:
                                    displayText.color = new Color32(192, 192, 0, 255);
                                    break;
                                case 4:
                                    displayText.color = new Color32(96, 192, 0, 255);
                                    break;
                                case 5:
                                    displayText.color = new Color32(0, 192, 0, 255);
                                    break;
                                case 6:
                                    displayText.color = new Color32(0, 192, 96, 255);
                                    break;
                                case 7:
                                    displayText.color = new Color32(96, 96, 96, 255);
                                    break;
                                case 8:
                                    displayText.color = new Color32(0, 192, 192, 255);
                                    break;
                                case 9:
                                    displayText.color = new Color32(0, 96, 192, 255);
                                    break;
                                case 10:
                                    displayText.color = new Color32(0, 0, 192, 255);
                                    break;
                                case 11:
                                    displayText.color = new Color32(96, 0, 192, 255);
                                    break;
                                case 12:
                                    displayText.color = new Color32(192, 0, 192, 255);
                                    break;
                                case 13:
                                    displayText.color = new Color32(192, 0, 96, 255);
                                    break;
                            }
                            displayText.GetComponent<Renderer>().material = textID[vals[3][step] - 11];
                            displayText.font = boozle[vals[3][step] - 11];
                            displayText.text = message[3][step];
                        }
                    }
                    break;
            }
        }
    }

    void ButtonOn(int b)
    {
        if (moduleSolved == false)
        {
            GetComponent<KMSelectable>().AddInteractionPunch(0.5f);
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            inputs[0][pressCount] = b;
            int a = 0;
            int s = ((int)GetComponent<KMBombInfo>().GetTime() % 60) % 10;
            int t = (((int)GetComponent<KMBombInfo>().GetTime() % 60) - (((int)GetComponent<KMBombInfo>().GetTime() % 60) % 10)) / 10;
            if (pressCount < 3)
            {
                a = vals[1][7 - pressCount];
            }
            else
            {
                a = finalpunc;
                displayText.text = String.Empty;
                StopCoroutine(textCycle);
            }
            switch (a)
            {
                case 0:
                case 4:
                    inputs[1][pressCount] = s;
                    Debug.LogFormat("[Bamboozled Again #{0}] The {1} button was pressed when the last digit of the timer was {2}", moduleID, location[inputs[0][pressCount]], inputs[1][pressCount]);
                    break;
                case 1:
                case 2:
                case 5:
                case 6:
                    inputs[1][pressCount] = s + t;
                    Debug.LogFormat("[Bamboozled Again #{0}] The {1} button was pressed when the sum of the last two digits of the timer was {2}", moduleID, location[inputs[0][pressCount]], inputs[1][pressCount]);
                    break;
                case 3:
                case 7:
                    inputs[1][pressCount] = Math.Abs(s - t);
                    Debug.LogFormat("[Bamboozled Again #{0}] The {1} button was pressed when the difference between the last two digits of the timer was {2}", moduleID, location[inputs[0][pressCount]], inputs[1][pressCount]);
                    break;
            }
            int[] correct = new int[2];
            if (pressCount < 3)
            {
                pressCount++;
                objectID[5 + pressCount].material = objectColours[0];
                ButtonSet(b);
                AnswerOrder();
            }
            else
            {
                pressCount = 0;
                for(int i = 0; i < 6; i++)
                {
                    buttonText[i].color = new Color32(0, 0, 0, 255);
                }
                displayText.color = new Color32(255, 255, 255, 255);
                displayText.GetComponent<Renderer>().material = textID[6];
                displayText.font = boozle[6];
                for (int i = 0; i < 4; i++)
                {
                    if (answerKey[0][i] == inputs[0][i])
                    {
                        correct[0] += 1;
                        if (answerKey[1][i] == inputs[1][i])
                        {
                            correct[1] += 1;
                            objectID[i + 6].material = objectColours[5];
                        }
                        else
                        {
                            objectID[i + 6].material = objectColours[3];
                        }
                    }
                    else
                    {
                        objectID[i + 6].material = objectColours[1];
                    }
                }
                if (correct[0] == 4)
                {
                    if (correct[1] == 4)
                    {
                        moduleSolved = true;
                        StartCoroutine(Submit(2));
                    }
                    else
                    {
                        StartCoroutine(Submit(1));                 
                    }
                }
                else
                {
                    GetComponent<KMBombModule>().HandleStrike();
                    StartCoroutine(Submit(0));
                }
            }
        }
    }

    private IEnumerator TextCycle()
    {
        for (int i = 0; i < 9; i++)
        {
            step = i;
            if (i < 8)
            {
                switch (textRandomiser[i])
                {
                    case 0:
                        displayText.color = new Color32(192, 192, 192, 255);
                        break;
                    case 1:
                        displayText.color = new Color32(192, 0, 0, 255);
                        break;
                    case 2:
                        displayText.color = new Color32(192, 96, 0, 255);
                        break;
                    case 3:
                        displayText.color = new Color32(192, 192, 0, 255);
                        break;
                    case 4:
                        displayText.color = new Color32(96, 192, 0, 255);
                        break;
                    case 5:
                        displayText.color = new Color32(0, 192, 0, 255);
                        break;
                    case 6:
                        displayText.color = new Color32(0, 192, 96, 255);
                        break;
                    case 7:
                        displayText.color = new Color32(96, 96, 96, 255);
                        break;
                    case 8:
                        displayText.color = new Color32(0, 192, 192, 255);
                        break;
                    case 9:
                        displayText.color = new Color32(0, 96, 192, 255);
                        break;
                    case 10:
                        displayText.color = new Color32(0, 0, 192, 255);
                        break;
                    case 11:
                        displayText.color = new Color32(96, 0, 192, 255);
                        break;
                    case 12:
                        displayText.color = new Color32(192, 0, 192, 255);
                        break;
                    case 13:
                        displayText.color = new Color32(192, 0, 96, 255);
                        break;
                }
                displayText.GetComponent<Renderer>().material = textID[vals[3][i] - 11];
                displayText.font = boozle[vals[3][i] - 11];
                displayText.text = message[3][i];
            }
            else
            {
                i = -1;
                displayText.text = String.Empty;
            }
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator Submit(int x)
    {
        moduleSolved = true;
        switch (x)
        {
            case 0:
                for (int i = 0; i < 6; i++)
                {
                    buttonText[i].fontSize = 500;
                    objectID[i].material = objectColours[1];
                }
                switch (UnityEngine.Random.Range(0, 4))
                {
                    case 0:
                        displayText.text = "YOU HAD";
                        buttonText[0].text = "O";
                        buttonText[1].text = "N";
                        buttonText[2].text = "E";
                        buttonText[3].text = "J";
                        buttonText[4].text = "O";
                        buttonText[5].text = "B";
                        break;
                    case 1:
                        displayText.text = "GET REKT";
                        buttonText[0].text = "T";
                        buttonText[1].text = "O";
                        buttonText[2].text = "P";
                        buttonText[3].text = "K";
                        buttonText[4].text = "E";
                        buttonText[5].text = "K";
                        break;
                    case 2:
                        displayText.text = "A WORD OF ADVICE:";
                        buttonText[0].text = "G";
                        buttonText[1].text = "I";
                        buttonText[2].text = "T";
                        buttonText[3].text = "G";
                        buttonText[4].text = "U";
                        buttonText[5].text = "D";
                        break;
                    case 3:
                        displayText.text = "BAMBOOZLED AGAIN";
                        buttonText[0].text = "G";
                        buttonText[1].text = "O";
                        buttonText[2].text = "T";
                        buttonText[3].text = "Y";
                        buttonText[4].text = "O";
                        buttonText[5].text = "U";
                        break;
                }
                yield return new WaitForSeconds(4);
                Debug.LogFormat("[Bamboozled Again #{0}] Incorrect button pressed: Resetting", moduleID);
                moduleSolved = false;
                Reset();
                StopCoroutine(Submit(0));
                yield break;
            case 1:
                for (int i = 0; i < 6; i++)
                {
                    buttonText[i].fontSize = 500;
                    objectID[i].material = objectColours[3];
                }
                switch (UnityEngine.Random.Range(0, 3))
                {
                    case 0:
                        displayText.text = "IT'S NOT OVER";
                        buttonText[0].text = "N";
                        buttonText[1].text = "O";
                        buttonText[2].text = "T";
                        buttonText[3].text = "Y";
                        buttonText[4].text = "E";
                        buttonText[5].text = "T";
                        break;
                    case 1:
                        displayText.text = "SO CLOSE";
                        buttonText[0].text = "A";
                        buttonText[1].text = "L";
                        buttonText[2].text = "M";
                        buttonText[3].text = "O";
                        buttonText[4].text = "S";
                        buttonText[5].text = "T";
                        break;
                    case 2:
                        displayText.text = "NOT QUITE";
                        buttonText[0].text = "N";
                        buttonText[1].text = "E";
                        buttonText[2].text = "A";
                        buttonText[3].text = "R";
                        buttonText[4].text = "L";
                        buttonText[5].text = "Y";
                        break;
                }
                yield return new WaitForSeconds(4);
                Debug.LogFormat("[Bamboozled Again #{0}] Button pressed at incorrect time: Resetting", moduleID);
                moduleSolved = false;
                Reset();
                StopCoroutine(Submit(1));
                yield break;
            case 2:
                GetComponent<KMAudio>().PlaySoundAtTransform("Win", transform);
                for (int i = 0; i < 6; i++)
                {
                    objectID[i].material = objectColours[0];
                    buttonText[i].text = String.Empty;
                }
                for (int i = 0; i < 15; i++)
                {
                    objectID[i % 2].material = objectColours[i];
                    objectID[(i % 2) + 2].material = objectColours[i];
                    objectID[(i % 2) + 4].material = objectColours[i];
                    yield return new WaitForSeconds(0.4f);
                }
                objectID[1].material = objectColours[0];
                objectID[3].material = objectColours[0];
                objectID[5].material = objectColours[0];
                yield return new WaitForSeconds(0.4f);
                for (int i = 0; i < 16; i++)
                {
                    objectID[i % 2].material = objectColours[Math.Min(i, 14)];
                    objectID[(i % 2) + 2].material = objectColours[Math.Min(i, 14)];
                    objectID[(i % 2) + 4].material = objectColours[Math.Min(i, 14)];
                    yield return new WaitForSeconds(0.4f);
                }
                for (int i = 0; i < 17; i++)
                {
                    objectID[0].material = objectColours[14 * (i % 2)];
                    objectID[1].material = objectColours[14 * (1 - (i % 2))];
                    objectID[2].material = objectColours[14 * (i % 2)];
                    objectID[3].material = objectColours[14 * (1 - (i % 2))];
                    objectID[4].material = objectColours[14 * (i % 2)];
                    objectID[5].material = objectColours[14 * (1 - (i % 2))];
                    yield return new WaitForSeconds(0.2f);
                }
                displayText.text = "CONGRATULATIONS";
                for (int i = 0; i < 6; i++)
                {
                    objectID[i].material = objectColours[5];
                    yield return new WaitForSeconds(0.125f);
                }
                GetComponent<KMBombModule>().HandlePass();
                yield break;
        }
    }
}
