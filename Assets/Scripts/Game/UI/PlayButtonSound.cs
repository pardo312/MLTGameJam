﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonSound : MonoBehaviour
{
    public void playButtonSound()
    {
        SoundFXManager.instance.Play("ButtonSound");
    }
}
