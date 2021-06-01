using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public List<GhostTransform> recordedGhostTransforms = new List<GhostTransform>();

    void Start()
    {
        if(recordedGhostTransforms != null)
            StartCoroutine(StartGhost());
    }

    private void Update()
    {
        if (transform.position == recordedGhostTransforms[recordedGhostTransforms.Count -1].position)
            StartCoroutine(StartGhost());
    }

    IEnumerator StartGhost()
    {
        yield return new WaitForSeconds(2.0f);

        for (int i = 0; i < recordedGhostTransforms.Count; i++)
        {
            transform.position = recordedGhostTransforms[i].position;
            transform.rotation = recordedGhostTransforms[i].rotation;
            yield return new WaitForFixedUpdate();
        }
    }

    public void FillList(List<GhostTransform> recordedGhostList)
    {
        for (int i = 0; i < recordedGhostList.Count; i++)
        {
            GhostTransform lastGhostTransform = recordedGhostList[i];
            recordedGhostTransforms.Add(lastGhostTransform);
        }
    }

    public void ResetGhost()
    {
        StopAllCoroutines();
        StartCoroutine(StartGhost());
    }
}
