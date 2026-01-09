using System.Collections.Generic;
using UnityEngine;

namespace CustomBedsSN.Dataclasses;

public class CustomBedData
{
    public string TechType;
    public string DisplayName;
    public string Description;
    public string Author;
    public Sprite Icon;
    public TechType UnlockTechType;
    public Dictionary<TechType, int> Ingredients;
    public Texture2D UpperPillowTexture;
    public Texture2D LowerPillowTexture;
    public Texture2D BlanketTexture;
    public Texture2D MattressTexture;
    public OriginalBedType OriginalBedType;
    public bool? DisableBlanket;
    
    public CustomBedData(
        string techType, 
        string displayName, 
        string description, 
        string author, 
        Sprite icon,
        OriginalBedType originalBedType,
        TechType unlockTechType = global::TechType.None,
        Dictionary<TechType, int> ingredients = null,
        Texture2D upperPillowTexture = null,
        Texture2D lowerPillowTexture = null,
        Texture2D blanketTexture = null,
        Texture2D mattressTexture = null,
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

public enum OriginalBedType
{
    SingleBed,
    DoubleBedQuilted,
    DoubleBedBasic
}