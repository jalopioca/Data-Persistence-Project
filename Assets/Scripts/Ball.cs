using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    private AudioSource m_AudioSource;
    public float maxSpeed = 5f;

    public AudioClip bounceAudio;
    public AudioClip popAudio;
    private MainManager _mainManager;

    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_Rigidbody = GetComponent<Rigidbody>();
        _mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();

        if (_mainManager.tilesAbove)
        {
            transform.Translate(Vector3.up * 2);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Brick")
        {
            m_AudioSource.PlayOneShot(popAudio);
        } else
        {
            m_AudioSource.PlayOneShot(bounceAudio);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        var velocity = m_Rigidbody.velocity;
        
        //after a collision we accelerate a bit
        velocity += velocity.normalized * 0.01f;
        
        //check if we are not going totally vertically as this would lead to being stuck, we add a little vertical force
        //if (Vector3.Dot(velocity.normalized, Vector3.up) < 0.1f)
        //{
        //    velocity += velocity.y > 0 ? Vector3.up * 0.5f : Vector3.down * 0.5f;
        //}

        //max velocity
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        m_Rigidbody.velocity = velocity;
    }
}
