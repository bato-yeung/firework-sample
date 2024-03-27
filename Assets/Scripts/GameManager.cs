using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private ClickToEmitParticle[] _fireworks;

    private void Start()
    {
        System.Array.ForEach(_fireworks, f => f.enabled = false);
        _fireworks[0].enabled = true;
    }

    private void Update()
    {
        int length = _fireworks.Length;
        for (int i = 0; i < length; i++)
        {
            KeyCode key = (KeyCode)(49 + i);
            if (Input.GetKeyUp(key) == true)
            {
                System.Array.ForEach(_fireworks, f => f.enabled = false);

                _fireworks[i].enabled = true;
            }
        }
    }
}
