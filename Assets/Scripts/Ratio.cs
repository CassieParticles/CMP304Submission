using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.UIElements;

public class Ratio
{

    public Ratio(int count)
    {
        weights = new float[count];
        sum = 0;
    }
    public Ratio(float[] initialWeights)
    {
        weights = initialWeights;
        sum = 0;
        for (int i = 0; i < weights.Length; ++i)
        {
            sum += weights[i];
        }
    }

    public float[] weights;

    public float this[int index]
    {
        get=>weights[index];
        set 
        {
            sum-=weights[index];
            weights[index] = value;
            sum += value;
        }
        
    }

    public float ratio(int index)
    {
        if(sum==0)
        {
            return 0;
        }
        return weights[index]/sum;
    }
    


    float sum;
}
