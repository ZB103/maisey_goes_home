using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFootstep : MonoBehaviour
{
    private Stress pStress;

    public void PlaySound()
    {
        //determine whether player is running or dragging by stress level
        pStress = GetComponent<Stress>();
        if (pStress.playerStress >= 60)
            PlayerSoundManager.PlaySound(SoundType.HOBBLE, 0.3f);
        else
            PlayerSoundManager.PlaySound(SoundType.RUN, 0.2f);
    }
}
