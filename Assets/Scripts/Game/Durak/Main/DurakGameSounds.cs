using System;
using UnityEngine;

public class DurakGameSounds : MonoBehaviour
{
    [SerializeField] private AudioClip cardsDistribution;
    [SerializeField] private AudioClip cardMove;
    [SerializeField] private AudioClip getCard;
    [SerializeField] private AudioClip takeCards;
    [SerializeField] private AudioClip victory;
    [SerializeField] private AudioClip defeat;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayDistribution() => _audioSource.PlayOneShot(cardsDistribution);

    public void PlayCardMove() => _audioSource.PlayOneShot(cardMove);

    public void PlayGetCard() => _audioSource.PlayOneShot(getCard);
    
    public void PlayTakeCard() => _audioSource.PlayOneShot(takeCards);
    
    public void PlayVictory() => _audioSource.PlayOneShot(victory);
    public void PlayDefeat() => _audioSource.PlayOneShot(defeat);
}