using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Numerics;

public static class Integrater // static in memory -can't change-stay as it is so no need update and void the usual thing unity has 

{
    public static float Trap(float a, float b, Func<float, float> f) // <> creates types of object that can work with any type 
    {
        //height -- |initial point - end point|
        //initial point
        //end point

        float dx = b - a; //the distance
        float approx = 0.5f * dx * (f(b) + f(a)); //just the simpla case of trapizum rule, extimating using first and last point
        return approx;
    }

    public static float[] Linspace(float a, float b, int n) //integrating steps 
    {
        float fraction = (b - a) / (n-1); //the distance 
        float points = a;// initial point
        float[] linspace = new float[n]; // creating the array 
        linspace[0] = a;//assinging the first value array

        int i = 1; // setting the first point to iterate
        while (points < b) //as long as a != b proceed with this while loop 
        {
            points = points + fraction; //next step of the integration

            linspace[i] = points; // filling the empty array created above
            i++; //incrementing the wile loop value by 1
            if (i == n) //if i has reached the number of integrating steps we want then break, we don't want more points 
            {
                break;
            }
        }
        return linspace;
    }
    //adding all the y values at every x 
    public static float Sum(float[] array, Func<float, float> f) //this returns a float, which takes an array and function 
    {
        float sum = 0; //initial sum 
        for (int i = 0; i <array.Length; i++)
        {
            sum += f(array[i]);
        }
        return sum;
    }

    public static float TrapArray(float a, float b, Func<float, float> f, int n)
    {
        //height -- |initial point - end point|
        //initial point
        //end point
        //f(x1) +f(x2)  
        float[] output = Linspace(a, b, n);
        float wholeSum = Sum(output,f);

        float p = wholeSum - (f(b) + f(a));

        float dx = (b - a) / (n - 1);
        float approx = 0.5f * dx * (f(b) + f(a) + 2f*(wholeSum-(f(b) + f(a))));
        return approx;
    }


    public static float[] CoefficientA_n(float c, Func<float, float> f,  int m, int n) // m is how many coefficient we want, n is how many in between steps we want or integration steps
    {

        float[] An = new float[m]; //float[] that is a array type 
        for (int i = 0; i < m; i++)
        {
            float Function(float x) //anything like this creates a function, in this case takes a float value and returns a float 
            {
            return f(x)*(float)Math.Cos((i*Math.PI*x)/c); // we enter the x values, all the parameters is entered by someone so no need to worry about it 
            } 

             An[i] = (1/c)*TrapArray(-c, c, Function, n); //{} defining the body, () the funcrtion [] array          
        }
        return An;                
     }

    public static float[] CoefficientB_n(float c, Func<float, float> f, int m, int n) // m is how many coefficient we want, n is how many in between steps we want or integration steps
    {
        float[] Bn = new float[m]; //float[] that is a array type 
        for (int i = 1; i < m+1; i++) //sin(0) = 0 so no need to start at 0, and to have the same lenght as an we need m+1
        {
            float Function(float x)
            {
                return f(x) * (float)Math.Sin((i * Math.PI * x) / c); //(float) function sin, beacuse by default it returns a double
            }

            Bn[i-1] = (1 / c) * TrapArray(-c, c, Function, n); //{} defining the body, () the funcrtion [] array
        }
        return Bn;
    }
    //this outputs a function which takes an arrays of an,ba, and the boundary
    public static Func<float,float> MakeFunction(float[] An, float[] Bn, float boundary)
    {        
        if (boundary < 0) //taking the positive value for the boundary 
        { 
            boundary = -boundary; 
        }

        float g(float t) //creating a function that outputs a float and takes in a float 
        {
            float sum = An[0]/2; //seeting the initial value for the sum 

            for (int l = 0; l <Bn.Length; l++) //going upto bn's length since bn has one more vlue then an in the summation, as an losses a0 
            {
                
                sum += An[l + 1] * (float)Math.Cos((Math.PI * (l+1) * t) / boundary) + Bn[l] * (float)Math.Sin((Math.PI * (l+1) * t) / boundary); //need to l+1 for sin since it starts at l =0, so when we read bn[l] need to add one to it 
            }

            return sum;
        }
        
        return g; // fourier series f(x)
    }

}
