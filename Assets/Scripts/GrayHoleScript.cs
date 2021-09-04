using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;

public class GrayHoleScript : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMBombModule Module;

    public KMSelectable[] sections;
    public TextMesh text;

    public Texture[] swirlTextures;
    public MeshRenderer imageTemplate;
    public GameObject containerTemplate;
    public Transform swirlContainer;

    private Transform[] _swirlsVisible;
    private Transform[] _swirlsActive;

    private GrayHoleSystem system;
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    private Dir? highlighted = null;
    private Dir? selected = null;

    void Awake () {
        moduleId = moduleIdCounter++;
        for (int i = 0; i < 4; i++)
        {
            int ix = i;
            sections[ix].OnInteract += delegate () { Hold(); return false; };
            sections[ix].OnInteractEnded += delegate () { Release(); };
            sections[ix].OnHighlight += delegate () { highlighted = (Dir)(ix * 2); };
            sections[ix].OnHighlightEnded += delegate () { highlighted = null; };
        }
    }

    void Hold()
    {
        selected = highlighted;
        Audio.PlaySoundAtTransform("BlackHoleInput2", transform);
    }
    void Release()
    {
        Dir resultingMovement = Data.dirTable[(int)selected / 2, (int)highlighted / 2];
        text.text = resultingMovement.ToString();
        text.gameObject.SetActive(true);
    }

    void Start ()
    {
        ConnectSystem();
        StartCoroutine(GeneratePuzzle());
        CreateSwirls();
    }
    void ConnectSystem()
    {
        string sn = Bomb.GetSerialNumber();
        if (!GrayHoleSystem.Systems.ContainsKey(sn))
        {
            GrayHoleSystem.Systems.Add(sn, new GrayHoleSystem());
            Debug.LogFormat("<Gray Hole #{0}> Created system {1}.", moduleId, sn);
        }
        else Debug.LogFormat("<Gray Hole #{0}> Connected to system {1}.", moduleId, sn);
        system = GrayHoleSystem.Systems[sn];
        system.holes.Add(this);
    }
    IEnumerator GeneratePuzzle()
    {
        yield return null; //We need to wait for all Gray Holes to be initialized first.
        if (!system.alreadySet)
            system.GenPuzzle();
        
    }
    

    void CreateSwirls()
    {
        _swirlsActive = new Transform[49];
        _swirlsVisible = new Transform[49];
        for (int i = 0; i < 49; i++)
        {
            var ct = Instantiate(containerTemplate).transform;
            ct.parent = swirlContainer.transform;
            ct.localPosition = Vector3.zero;
            ct.localScale = Vector3.one;
            ct.gameObject.SetActive(true);

            var mr = Instantiate(imageTemplate);
            mr.material.mainTexture = swirlTextures[i / 7];
            mr.material.renderQueue = 2700 + i;
            mr.transform.parent = ct;
            mr.transform.localPosition = new Vector3((250 - 201 - 70 / 2) / 500f, (250 - 31 - 32 / 2) / 500f, 0);
            mr.transform.localScale = new Vector3(70f / 500, 32f / 500, 1);
            mr.gameObject.SetActive(true);

            _swirlsActive[i] = _swirlsVisible[i] = ct;
        }
        MoveSwirls();
    }
    void MoveSwirls()
    {
        float[] pulseSpeeds = new float[7];
        for (int i = 0; i < 7; i++)
            pulseSpeeds[i] = Rnd.Range(10f, 30f);
        _swirlsVisible.Shuffle();
        //We only do this once to set the positions
        for (int i = 0; i < 49; i++)
        {
            if (_swirlsVisible[i] != null)
                _swirlsVisible[i].localEulerAngles = new Vector3(0, 0, i * 360f / 49 + Rnd.Range(-2f, 2f));
            StartCoroutine(Pulse(_swirlsVisible[i]));
        }
    }
    IEnumerator Pulse(Transform tf)
    {
        float pulseSpeed = Rnd.Range(0.1f, 0.3f);
        while (true)
        {
            float delta = 0;
            while (delta < 1)
            {
                delta += pulseSpeed * Time.deltaTime;
                tf.localScale = Ut.InOutLerp(0.975f, 1.025f, delta) * Vector3.one;
                yield return null;
            }
            yield return null;
        }
    }

#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use <!{0} foobar> to do something.";
    #pragma warning restore 414

    IEnumerator ProcessTwitchCommand (string command)
    {
        command = command.Trim().ToUpperInvariant();
        List<string> parameters = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        yield return null;
    }

    IEnumerator TwitchHandleForcedSolve ()
    {
        yield return null;
    }
}
