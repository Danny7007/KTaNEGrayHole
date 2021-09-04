using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dir
{
    N,
    NE,
    E,
    SE,
    S,
    SW,
    W,
    NW,
    X = -1 //Represents no movement
}
public static class Data 
{
    public static int[,] table = new int[,]
    {
        {3,4,1,0,2,3,1,2,0,4},
        {1,3,0,2,4,1,2,3,4,0},
        {3,2,4,2,1,3,0,0,1,4},
        {4,0,0,1,3,4,2,2,1,3},
        {1,2,1,3,0,0,4,3,4,2},
        {4,0,2,3,4,1,3,0,2,1},
        {2,1,3,1,3,0,4,4,0,2},
        {2,4,4,0,0,2,1,1,3,3},
        {0,1,3,4,2,2,0,4,3,1},
        {0,3,2,4,1,4,3,1,2,0},
    };
    public static Dir[,] dirTable = new Dir[,]
    {
        //   N      E       S       W        Row represents the held section
    /*N*/{Dir.X , Dir.SE, Dir.S , Dir.SW },//Column represents the released section.
    /*e*/{Dir.NW, Dir.X , Dir.SW, Dir.W  },
    /*s*/{Dir.N , Dir.NE, Dir.X , Dir.NW },
    /*w*/{Dir.NE, Dir.E , Dir.SE, Dir.X  },
    };  
}
