using System.Collections.Generic;

namespace CustomBedsSN.Dataclasses;

public class CustomBedDataJson
{
    public string TechType;
    public string DisplayName;
    public string Description;
    public string Author;
    public string Icon;
    public string UnlockTechType;
    public Dictionary<string, int> Ingredients; // TechType, Amount
    public string UpperPillowTexture;
    public string LowerPillowTexture;
    public string BlanketTexture;
    public string MattressTexture;
    public string OriginalBedType;
    public bool? DisableBlanket;

    public CustomBedDataJson(
        string techType, 
        string displayName, 
        string description, 
        string author, 
        string icon,
        string originalBedType,
        string unlockTechType = null,
        Dictionary<string, int> ingredients = null,
        string upperPillowTexture = null,
        string lowerPillowTexture = null,
        string blanketTexture = null,
        string mattressTexture = null,
        bool? disableBlanket = null
    )
    {
        TechType = techType;
        DisplayName = displayName;
        Description = description;
        Author = author;
        Icon = icon;
        OriginalBedType = originalBedType;
        UnlockTechType = unlockTechType;
        Ingredients = ingredients;
        UpperPillowTexture = upperPillowTexture;
        LowerPillowTexture = lowerPillowTexture;
        BlanketTexture = blanketTexture;
        MattressTexture = mattressTexture;
        DisableBlanket = disableBlanket;
    }
}