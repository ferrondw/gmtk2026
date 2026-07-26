using Advanced_Audio_Sources.scripts.AudioSources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    [SerializeField] private int musicId;
    [SerializeField] private List<int> randomMusicIdList = new List<int>();
    [SerializeField] private bool restart = false;

    private void Start()
    {
        if (randomMusicIdList.Count > 0)
        {
            var randomId = Random.Range(0, randomMusicIdList.Count);
            MusicSource.Instance.PlayMusic(randomMusicIdList[randomId], restart, true);
            return;
        }
        MusicSource.Instance.PlayMusic(musicId, restart, true);
    }
}
