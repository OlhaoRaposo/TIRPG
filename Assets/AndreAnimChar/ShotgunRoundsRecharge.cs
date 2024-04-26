using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Esse script deve ir no parent dos cartuchos da escopeta
public class ShotgunRoundsRecharge : MonoBehaviour
{
    [SerializeField] private Transform shotgunBarrel;
    private Transform[] rounds = new Transform[8];
    private int currentRound = 0;
    private Quaternion desiredRotation;
    public void TryConsumeRound() {
        if (rounds[currentRound].gameObject.activeInHierarchy == true) { ConsumeRound(); }
        else { NextRound(); Debug.LogWarning("SHOTGUNROUNDRELOAD: chama o andré pra resolver"); }
    }
    public void TryRechargeRound() {
        if (rounds[(currentRound + rounds.Length - 1) % rounds.Length].gameObject.activeInHierarchy == false) { RechargeRound(); }
        else { PreviousRound(); Debug.LogWarning("SHOTGUNROUNDRELOAD: chama o andré pra resolver"); }
    }
    private void ConsumeRound() {
        rounds[currentRound].gameObject.SetActive(false);
        NextRound();
    }
    private void RechargeRound() {
        rounds[(currentRound + rounds.Length - 1) % rounds.Length].gameObject.SetActive(true);
        PreviousRound();

        // Se você tá pensando em referenciar seu script principal de reload para a escopeta, 
        // acredito que vai querer incrementar as munições aqui embaixo, nesse método :D



        //
    }
    private void NextRound()
    {
        int previousRound = currentRound;

        for (int i = 0; i < rounds.Length; i++)
        {
            if (rounds[currentRound % rounds.Length].gameObject.activeInHierarchy == false)
            {
                currentRound++;
            }
            else
            {
                break;
            }
        }

        int difference = currentRound - previousRound;
        currentRound = currentRound % rounds.Length;
        desiredRotation.eulerAngles += new Vector3(0, 45f * difference, 0);
    }
    private void PreviousRound()
    {
        int previousRound = currentRound;
        for (int i = 0; i < rounds.Length; i++)
        {
            if (rounds[(currentRound + rounds.Length - 1) % rounds.Length].gameObject.activeInHierarchy == true)
            {
                currentRound--;
            }
            else
            {
                break;
            }
        }

        int difference = currentRound - previousRound;
        currentRound = (currentRound + rounds.Length) % rounds.Length;
        desiredRotation.eulerAngles += new Vector3(0, 45f * difference, 0);
    }
    private void Start()
    {
        desiredRotation = shotgunBarrel.transform.localRotation;

        byte childCount = (byte)shotgunBarrel.transform.childCount;
        for (int i = 0; i < childCount; i++) { rounds[i] = shotgunBarrel.transform.GetChild(i); }

        Debug.Log((currentRound + rounds.Length - 1) % rounds.Length);
    }
    private void FixedUpdate()
    {
        shotgunBarrel.transform.localRotation = Quaternion.Lerp(shotgunBarrel.transform.localRotation, desiredRotation, Time.fixedDeltaTime * 10);
    }
}