# Update from .json to .bed

This document provides a guide on how to update your custom bed definitions from the older `.json` format to the newer `.bed` format. The `.bed` format offers improved structure and additional features for defining custom beds.

## Overview
1. [Steps to Update from .json to .bed](#steps-to-update-from-json-to-bed)
2. [Notes](#notes)

### Steps to Update from .json to .bed

1. Change the file extension from `.json` to `.bed`.
2. Update the structure of the file to match the `.bed` format specifications. Below is an example of how to convert a simple bed definition.
- Old `.json` format:
```json
{
  "techType": "techtype",
  "displayName": "displayname",
  "description": "description",
  "unlockTechType": "",
  "ingredients": [
    { "item": "Titanium", "amount": 2 },
    { "item": "FiberMesh", "amount": 1 }
  ],
  "pillowUpperTexture": "Texture1.png",
  "pillowLowerTexture": "",
  "blanketTexture": "",
  "mattressTexture": "Texture2.png",
  "bedType": "Bed1"
}
```
- New `.bed` format:
```json
{
  "TechType": "techtype",
  "DisplayName": "displayname",
  "Description": "description",
  "Author": "Your Name",
  "UnlockTechType": "",
  "Ingredients": {
    "Titanium": 2,
    "FiberMesh": 1
  },
  "UpperPillowTexture": "Texture1.png",
  "LowerPillowTexture": "",
  "BlanketTexture": "",
  "MattressTexture": "Texture2.png",
  "OriginalBedType": "Bed1"
}
```

### Notes:
- Ensure that all property names are updated to match the new format (e.g., `techType` to `TechType`, `displayName` to `DisplayName`, etc.).
- The `ingredients` array is replaced with an `Ingredients` object where each item is a key-value pair.
- Add an `Author` field to credit the creator of the bed.
- The `.bed` format is just a renamed version of the `.json` format, so any JSON syntax rules still apply.