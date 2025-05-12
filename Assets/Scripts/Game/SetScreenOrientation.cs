using UnityEngine;

public class SetScreenOrientation
{
    private ScreenOrientation _orientation;

    public void GameSceneOrientation()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;

        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;

        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
    }
    
    public void MenuSceneOrientation()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
    }
    
    
}