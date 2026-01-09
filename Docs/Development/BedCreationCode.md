# Custom Bed Creation via Code

## Overview
1. [Initialize the CustomBedsData Class](#1-initialize-the-custombedsdata-class)
2. [Register the Custom Bed](#2-register-the-custom-bed)

### 1. Initialize the CustomBedsData Class
To create custom beds in the game, you need to initialize the `CustomBedsData` class. This class manages all the data related to custom beds.

```csharp
CustomBedsData customBedsData = new CustomBedsData(
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
);
```

Notes:
- `techType`: Unique identifier for the custom bed. Cannot contain spaces or special characters.
- Textures: If you do not provide custom textures, the bed will use the default textures based on the `originalBedType`.
- `disableBlanket`: If set to true, the blanket will not be rendered on the bed. (Only applies to the Single Bed)
- `ingredients`: If not provided, the bed will use the default ingredients based on the `originalBedType`.

### 2 Register the Custom Bed
After initializing the `CustomBedsData` class, you need to register the custom bed so that it becomes available in the game.
```csharp
CustomBedsHandler.RegisterCustomBed(customBedsData);
```