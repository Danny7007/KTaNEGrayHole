using System.Collections;
using System.Collections.Generic;
using Rnd = UnityEngine.Random;

public class GrayHoleSystem : IEnumerable<GrayHoleScript>  
{
    public IEnumerator<GrayHoleScript> GetEnumerator()
    { return holes.GetEnumerator(); }
    IEnumerator IEnumerable.GetEnumerator()
    { return GetEnumerator(); }


    public List<GrayHoleScript> holes = new List<GrayHoleScript>();
    public int startX, startY;
    public List<int> displayedSequence = new List<int>();
    public bool alreadySet = false;

    public void GenPuzzle() 
    {
        alreadySet = true;
        startX = Rnd.Range(0, 10);
        startY = Rnd.Range(0, 10);

        for (int i = 0; i < 3 * holes.Count; i++)
        {

        }
    }


    /// <summary>
    ///     Contains every System loaded into the game. The serial number is used as a key.
    /// </summary>
    public static Dictionary<string, GrayHoleSystem> Systems = new Dictionary<string, GrayHoleSystem>();
}
