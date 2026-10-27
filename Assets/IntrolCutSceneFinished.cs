using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class IntrolCutSceneFinished : MonoBehaviour
{
    public float duration = 5.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitForSeconds(duration);
        GetComponent<CinemachineSequencerCamera>().Priority.Value = 0;
    }
    
}
