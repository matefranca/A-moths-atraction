using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct GhostTransform
{
    public Vector3 position;
    public Quaternion rotation;
    public GhostTransform(Transform transform)
    {
       this.position = transform.position;
       this.rotation = transform.rotation;
    }
}

public class GhostManager : MonoBehaviour
{
    public Transform player;
    public GameObject ghostPrefab;

    private Vector3 spawnPoint;

    public bool recording;
    public bool playing;

    private List<GhostTransform> recordedGhostTransforms = new List<GhostTransform>();
    private GhostTransform lastrecordedGhostTransform;

    // Start is called before the first frame update
    void Awake()
    {
        spawnPoint = player.transform.position;
        recording = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (recording)
        {
            if (player.position != lastrecordedGhostTransform.position || player.rotation != lastrecordedGhostTransform.rotation)
            {
                var newGhostTransform = new GhostTransform(player);
                recordedGhostTransforms.Add(newGhostTransform);
                lastrecordedGhostTransform = newGhostTransform;
            }
        }

        if(playing)
        {
            Play();
        }
    }

    public void Play()
    {
        GameObject newGhost = Instantiate(ghostPrefab, spawnPoint, Quaternion.identity);
        newGhost.GetComponent<Ghost>().FillList(recordedGhostTransforms);
        ClearList();
        playing = false;
    }

    void ClearList()
    {
        while(recordedGhostTransforms.Count > 0)
        {
            GhostTransform lastGhost = recordedGhostTransforms[recordedGhostTransforms.Count -1];
            recordedGhostTransforms.Remove(lastGhost);
        }
    }
}
