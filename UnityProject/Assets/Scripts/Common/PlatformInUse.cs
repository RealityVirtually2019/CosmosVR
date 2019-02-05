using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlatformInUse 
{
    public enum Platform {NONE,VIVE,DAYDREAM,OCULUSGO };
    public static Platform CurrentPlatform = Platform.VIVE;
}
