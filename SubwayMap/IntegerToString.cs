using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntegerToString : MonoBehaviour
{
    // static void Main(string[] args)
    // {
    //     int number = 42;
    //     string strNumber = ConvertIntToString(number);
    //     // Console.WriteLine("The string representation of " + number + " is: " + strNumber);
    // }

    public static string ConvertIntToString(int number)
    {
        switch (number)
        {
            case 0:
                return "zero";
            case 1:
                return "one";
            case 2:
                return "two";
            case 3:
                return "three";
            case 4:
                return "four";
            case 5:
                return "five";
            case 6:
                return "six";
            case 7:
                return "seven";
            case 8:
                return "eight";
            case 9:
                return "nine";
            default:
                return number.ToString();
        }
    }
}
