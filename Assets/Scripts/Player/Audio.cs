using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource playerSounds;
    public AudioSource phoneSounds;
    public AudioSource friendVoice;
    public AudioSource clerkVoice;
    public AudioClip voiceLine1;
    public AudioClip shouldPickupPhone;
    public AudioClip phoneRinging;
    public AudioClip phoneCall;
    public AudioClip changeParty;
    public AudioClip lovelyFamily;
    public AudioClip seeGun;
    public AudioClip needMoney;
    public AudioClip safeDay;

    // Start is called before the first frame update
    void Start()
    {
        // Schedule the playerAudio method after 5 seconds
        Invoke("playerAudio", 5.0f);
    }

    void playerAudio() // Plays after the game starts
    {
        playerSounds.clip = voiceLine1; // Assign the correct clip
        playerSounds.Play(); // Play the clip
    }

    void phoneVoiceLine() // Should play a few seconds after player gets home from store
    {
        playerSounds.clip = shouldPickupPhone; // Assign the correct clip
        playerSounds.Play(); // Play the clip
    }

    void phoneConvo() // Should play when player picks up phone
    {
        phoneSounds.clip = phoneCall; // Assign the correct clip
        phoneSounds.Play(); // Play the clip
    }

    void phoneRing() // Should play when player gets home from store
    {
        phoneSounds.clip = phoneRinging; // Assign the correct clip
        phoneSounds.Play(); // Play the clip
    }

    void changeForParty() // Should play after player put the phone back down
    {
        playerSounds.clip = changeParty; // Assign the correct clip
        playerSounds.Play(); // Play the clip
    }

    void seeYourGun() // Should play when player returns to couch with drink
    {
        friendVoice.clip = seeGun; // Assign the correct clip
        friendVoice.Play(); // Play the clip
    }

    void clerkNeedMoney() // Should play when player tries to buy safe with not enough money
    {
        clerkVoice.clip = needMoney; // Assign the correct clip
        clerkVoice.Play(); // Play the clip
    }

    void haveSafeDay() // Should play when player buys gun and holster
    {
        clerkVoice.clip = safeDay; // Assign the correct clip
        clerkVoice.Play(); // Play the clip
    }

    void loveFamily() // Should play when looking at the family picture
    {
        playerSounds.clip = lovelyFamily; // Assign the correct clip
        playerSounds.Play(); // Play the clip
    }
}
