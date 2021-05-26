using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PUtil.Plot;
using System.Linq; // don't need it, and have no clue what the fuck it is 
using System.Numerics; // can use maths like trigs 
using System;// has Func thing, that's what I need to inculude function as a variable

public class Calc : MonoBehaviour
{
    public float a; // initial point
    public float b;//end point 
    public int n; // number of integrating steps 
    public int m; // number of ceoffients or number of accuracy 

    // Start is called before the first frame update
    void Start()
    {
        //has more steps 

        float i= Integrater.TrapArray(a,b,Function,n); //we set variable i to traparray from integrator to output it's results. we can't do this in integrator because it's statics, so we can't print. 
        print(i);
        //uses only first and last value
        i = Integrater.Trap(a, b, Function);
        print(i);


        float c = (a + b )/ 2; //mid point
        float d = c - a; //distance between mid point and a or b

        float Symmetric(float t) //off setting a function to make it symmetric if it's not symmetric to start with. but works even if it's symmetric 
        {
            return Function(t+c);
        }

        //plotting it in 2d
        float[] x = Integrater.Linspace(a,b,n); // arrary of x

        float[] y = new float[n]; //empty arary of y 

        for (int j = 0; j < x.Length; j++)
        {
            y[j] = Function(x[j]); // array's of y

        }
        print("First y "+y[0]);
        PlotUtil.Plot2D(x,y, Color.magenta, 5f,10f);
        
        //Coefficient

        float[] an = Integrater.CoefficientA_n(d, Symmetric, m+1,n); //we need one extra an since bn starts at 1
        for (int k = 0; k <an.Length; k++)
        {
            if (Math.Abs(an[k])<0.001) // neglecting too small value 
            {
                an[k] = 0;
            }
            print("A" +k + " = " + an[k]);
        }

        float[] bn = Integrater.CoefficientB_n(d, Symmetric, m, n);
        for (int k = 0; k < bn.Length; k++)
        {
            if (Math.Abs(bn[k]) < 0.001)
            {
                bn[k] = 0;
            }
            print("B" + (k+1) + " = " + bn[k]);
        }

        
        Func<float, float> FourierFunction = Integrater.MakeFunction(an, bn, d); // d is the boundary 

        /*
        float sum = an[0] / 2;
        print("c = "+c);
        for (int l = 0; l < bn.Length; l++)
        {
            float v = an[l + 1] * (float)Math.Cos((Math.PI * l * 1) / c);
            float z = bn[l] * (float)Math.Sin((Math.PI * (l + 1) * 1) / c);
            print("an "+l +" "+v);
            print("bn "+l+ " "+z);
           sum += an[l + 1] * (float)Math.Cos((Math.PI * l * 1) / c) + bn[l] * (float)Math.Sin((Math.PI * (l + 1) * 1) / c); //need to l+1 for sin since it starts at l =0, so when we read bn[l] need to add one to it 
            
        }
        */

        x = Integrater.Linspace(-d,d,n);

        //x = Integrater.Linspace(d, -d, n);

        y = new float[n];
        for (int j = 0; j < x.Length; j++)
        {
            y[j] = FourierFunction(x[j]); // for the fou... 
        }
        print("second y "+ y[0]);

        PlotUtil.Plot2D(x, y, Color.yellow, 5f, 10f);

        for (int j = 0; j < x.Length; j++)
        {
            y[j] = Symmetric(x[j]); // for the fou... 
        }
        print("second y " + y[0]);

        PlotUtil.Plot2D(x, y, Color.white, 5f, 10f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float Function(float x)
    {
        return (float)(Math.Exp(x));
        //return x*x;
    }
}
