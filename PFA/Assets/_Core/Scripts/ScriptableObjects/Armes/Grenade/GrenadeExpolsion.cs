using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

public class GrenadeExpolsion : MonoBehaviour
{
    [SerializeField] private float secondes = 2f;
    private bool boom = false;
    private Zombies Zombies;
    private int damage = 100;
    private int Life;
    [SerializeField] GameObject explosionVFX;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        explosionVFX.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        StartCoroutine(Explosion());
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("ok");
        if (boom)
        {
            if (other.CompareTag("Zombies"))
            {
                Destroy(other.gameObject);
            }
        }
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(secondes);
        boom = true;
        rb.isKinematic = true;
        meshRenderer.enabled = false;
        explosionVFX.SetActive(true);
        Destroy(gameObject, 2);
    }
}