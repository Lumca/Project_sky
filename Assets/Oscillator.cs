using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;
    [SerializeField] [Range(0,1)] float movementFactor; //0 not moving, 1 for fully moving
    
    Vector3 startingPos;
    
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon)
        {
            return;
        }
        //set movement factor
        float cycles = Time.time / period; // grows from 0

        const float tau = Mathf.PI * 2f; // cca 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);
        movementFactor = rawSinWave / 2f + 0.5f; // goes from -1 to +1

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}