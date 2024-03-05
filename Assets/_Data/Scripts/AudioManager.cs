using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource audioSource;

    [SerializeField] private AudioClip soundtrack;
    [SerializeField] private AudioClip pokemonClickingSound;
    [SerializeField] private AudioClip success;
    [SerializeField] private AudioClip fail;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }    
        else
        {
            Instance = this;
        }    
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlaySoundtrack();
    }

    private void PlaySoundtrack()
    {
        audioSource.clip = soundtrack;
        audioSource.Play();
    }

    public void PlayOnClickPokemon()
    {
        audioSource.PlayOneShot(pokemonClickingSound);
    }

    public void PlaySuccessPokemon()
    {
        audioSource.PlayOneShot(success);
    }

    public void PlayFailPokemon()
    {
        audioSource.PlayOneShot(fail);
    }
}
