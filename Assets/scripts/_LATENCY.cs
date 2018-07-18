using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _LATENCY : MonoBehaviour {

    public Text latencyText;
    int frameCount;
	void Update ()
    {
        if (frameCount == 60)
        {
            Latency("127.0.0.1");
        }
        frameCount++;        
    }

    void Latency(string ip)
    {
        Ping ping = new Ping(ip);
        while (!ping.isDone)
        {
            
        }
        latencyText.text = ping.time + " ms";
    }
}
