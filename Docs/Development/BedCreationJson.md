# Custom Bed Creation via JSON (.bed files)

## Overview
1. [Json Structure](#1-json-structure)
2. [Example](#2-example)
3. [Example folder structure](#3-example-folder-structure)
4. [Additional Notes](#4-additional-notes)

### 1. Json Structure
```json
{
  "TechType": "TechType",
  "DisplayName": "Custom Bed Name",
  "Description": "A brief description of the custom bed.",
  "Author": "Your Name",
  "UnlockTechType": "Any valid TechType for unlocking",
  "Ingredients": {
    "Titanium": 2,
    "FiberMesh": 1
  },
  "UpperPillowTexture": "Texture1.png",
  "LowerPillowTexture": "Texture2.png",
  "BlanketTexture": "Texture3.png",
  "MattressTexture": "Texture4.png",
  "OriginalBedType": "Bed1"
}
```

Notes:
- `TechType`: Unique identifier for the custom bed. Cannot contain spaces and cannot reuse existing TechTypes.
- `Ingredients`: A dictionary of required items and their quantities for crafting the bed.
- Texture fields (`UpperPillowTexture`, `LowerPillowTexture`, `BlanketTexture`, `MattressTexture`): Filenames of the textures to be applied to the respective parts of the bed. These textures should be placed in the appropriate bed folder. Automatically resizes to fit the bed model. Any texture field can be left empty to use the default texture.
- `OriginalBedType`: Specifies the base bed model to use. Valid options include "Bed1" ("DoubleBedBasic"), "Bed2" ("DoubleBedQuilted") or "NarrowBed" ("SingleBed").

### 2. Example

Here's an example of a custom bed JSON configuration (Available at [NexusMods](https://www.nexusmods.com/subnautica/mods/2394)):
```json
{
  "TechType": "KeepCalmKittyBed1",
  "DisplayName": "Keep Calm Kitty Double Bed",
  "Description": "Always Keep Calm in this Nightmare Hell",
  "Author": "Alexius25",
  "UnlockTechType": "posterkitty",
  "Ingredients": {
    "Titanium": 2,
    "FiberMesh": 1
  },
  "UpperPillowTexture": "KeepCalmKitty1.png",
  "LowerPillowTexture": "",
  "BlanketTexture": "",
  "MattressTexture": "KeepCalmKitty.png",
  "OriginalBedType": "Bed1"
}
```

### 3. Example folder structure
```
CustomBeds/
└── KeepCalmKittyBed1/
    ├── KeepCalmKittyBed1.bed
    ├── KeepCalmKitty1.png
    └── KeepCalmKitty.png
```

### 4. Additional Notes
- The `.bed.json` can be named anything, but it's recommended to match the `TechType` for clarity.
- You can package multiple beds into a single folder with subfolders for each bed, each containing its own .bed file and textures.
- Ensure that the texture files are correctly named and placed in the same folder as the .bed file for proper loading.
- Test the custom bed in-game to verify that all textures and functionalities work as expected.

