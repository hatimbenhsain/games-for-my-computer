using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnDataHolder
{
    public static int spawnLocationIndex = 1;
    public static StarterAssets.ThirdPersonController.CharacterState targetCharacterState = StarterAssets.ThirdPersonController.CharacterState.Fish;
    public static StarterAssets.ThirdPersonController.CharacterState characterState = StarterAssets.ThirdPersonController.CharacterState.Fish;
    public static bool playTransformAnimation = false;
}
