using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlatformInUse 
{
    public enum Platform {NONE,VIVE,DAYDREAM };
    public static Platform CurrentPlatform = Platform.VIVE;
}
