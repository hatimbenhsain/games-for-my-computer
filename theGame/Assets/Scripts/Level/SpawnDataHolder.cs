using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnDataHolder
{
    // 1 is post dancing game, 2 post surgery game, 3 post compliment, 4 post museum
    public static int spawnLocationIndex = 1;
    public static StarterAssets.ThirdPersonController.CharacterState targetCharacterState = StarterAssets.ThirdPersonController.CharacterState.Fish;
    public static StarterAssets.ThirdPersonController.CharacterState characterState = StarterAssets.ThirdPersonController.CharacterState.Fish;
    public static bool playTransformAnimation = false;
    public static bool playRain=false;

}
