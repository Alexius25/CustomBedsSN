using Color = UnityEngine.Color;
using CustomBedsSN.Dataclasses;
using Graphics = UnityEngine.Graphics;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Crafting;
using Nautilus.Handlers;
using Nautilus.Utility;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CustomBedsSN.Handler;

internal static class BedPrefabHandler
{
    private static readonly Rect UpperPillowRect = new Rect(0, 500, 335, 270);
    private static readonly Rect LowerPillowRect = new Rect(0, 750, 335, 270);
    private static readonly Rect BlanketRect = new Rect(335, 770, 690, 255);
    private static readonly Rect MattressRect = new Rect(375, 1, 650, 795);
    
    internal static void RegisterBedPrefabJson(CustomBedDataJson bedDataJson, string bedFolderPath)
    {
        TechType bedTechType = TechType.None;

        switch (bedDataJson.OriginalBedType)
        {
            case "DoubleBedBasic":
                bedTechType = TechType.Bed1;
                break;
            case "DoubleBedQuilted":
                bedTechType = TechType.Bed2;
                break;
            case "SingleBed":
                bedTechType = TechType.NarrowBed;
                break;
            case "Bed1":
                bedTechType = TechType.Bed1;
                break;
            case "Bed2":
                bedTechType = TechType.Bed2;
                break;
            case "NarrowBed":
                bedTechType = TechType.NarrowBed;
                break;
            default:
                bedTechType = TechType.Bed1;
                break;
        }

        var prefabInfo = PrefabInfo.WithTechType(bedDataJson.TechType, bedDataJson.DisplayName, bedDataJson.Description);
        
        if (bedDataJson.Icon != null)
        {
            string iconPath = Path.Combine(bedFolderPath, bedDataJson.Icon);
            if (!File.Exists(iconPath))
            {
                Main.Logger.LogError($"Icon path {iconPath} not found");
                prefabInfo.WithIcon(SpriteManager.Get(bedTechType));
            }
            else
            {
                var icon = ImageUtils.LoadSpriteFromFile(iconPath);
                prefabInfo.WithIcon(icon);
            }
        }
        else
        {
            prefabInfo.WithIcon(SpriteManager.Get(bedTechType));
        }
        
        var prefab = new CustomPrefab(prefabInfo);
        var bedClone = new CloneTemplate(prefabInfo, bedTechType);
        
        bedClone.ModifyPrefab += go =>
        {
            ModifyPrefabJson(go, bedDataJson, bedFolderPath);
            
            if (bedDataJson.DisableBlanket == true && bedTechType == TechType.NarrowBed)
            {
                RemoveBlanket(go);
            }
        };
        
        prefab.SetGameObject(bedClone);

        var recipe = new List<Ingredient>();
        if (bedDataJson.Ingredients != null)
        {
            foreach (var ingredient in bedDataJson.Ingredients)
            {
                if (System.Enum.TryParse<TechType>(ingredient.Key, out TechType ingredientTechType))
                {
                    recipe.Add(new Ingredient(ingredientTechType, ingredient.Value));
                }
                else
                {
                    Main.Logger.LogError($"Invalid TechType {ingredient.Key} in ingredients for bed {bedDataJson.TechType}");
                }
            }
        }
        else
        {
            recipe.Add(new Ingredient(TechType.FiberMesh, 2));
            recipe.Add(new Ingredient(TechType.Titanium, 4));
        }
        
        TechType unlockTechType = TechType.Bed1;
        if (!string.IsNullOrEmpty(bedDataJson.UnlockTechType) && System.Enum.TryParse<TechType>(bedDataJson.UnlockTechType, out TechType parsedTechType))
        {
            unlockTechType = parsedTechType;
        }
        prefab.SetRecipe(new RecipeData(recipe.ToArray()));
        prefab.SetUnlock(unlockTechType);
        prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule);
        prefab.Register();
        
        LanguageHandler.SetLanguageLine(bedDataJson.TechType, bedDataJson.DisplayName);
        LanguageHandler.SetLanguageLine($"{bedDataJson.TechType}_Description", bedDataJson.Description);
        LanguageHandler.SetLanguageLine($"Tooltip_{bedDataJson.TechType}", bedDataJson.Description);
        
        Main.Logger.LogInfo($"Registered custom bed prefab: {bedDataJson.TechType}");
    }

    internal static void RegisterBedPrefab(CustomBedData customBedData)
    {
        TechType bedTechType = TechType.None;
        
        switch (customBedData.OriginalBedType)
        {
            case OriginalBedType.DoubleBedBasic:
                bedTechType = TechType.Bed1;
                break;
            case OriginalBedType.DoubleBedQuilted:
                bedTechType = TechType.Bed2;
                break;
            case OriginalBedType.SingleBed:
                bedTechType = TechType.NarrowBed;
                break;
        }
        
        var prefabInfo = PrefabInfo.WithTechType(customBedData.TechType, customBedData.DisplayName, customBedData.Description);
        prefabInfo.WithIcon(customBedData.Icon ?? SpriteManager.Get(bedTechType));
        
        var prefab = new CustomPrefab(prefabInfo);
        var bedClone = new CloneTemplate(prefabInfo, bedTechType);
        
        bedClone.ModifyPrefab += go =>
        {
            ModifyPrefab(go, customBedData);
            if (customBedData.DisableBlanket == true && bedTechType == TechType.NarrowBed)
            {
                RemoveBlanket(go);
            }
        };
        
        prefab.SetGameObject(bedClone);
        
        var recipe = new List<Ingredient>();
        if (customBedData.Ingredients != null)
        {
            foreach (var ingredient in customBedData.Ingredients)
            {
                recipe.Add(new Ingredient(ingredient.Key, ingredient.Value));
            }
        }
        else
        {
            recipe.Add(new Ingredient(TechType.FiberMesh, 2));
            recipe.Add(new Ingredient(TechType.Titanium, 4));
        }
        
        prefab.SetRecipe(new RecipeData(recipe.ToArray()));
        prefab.SetUnlock(customBedData.UnlockTechType);
        prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule);
        prefab.Register();
        
        LanguageHandler.SetLanguageLine(customBedData.TechType, customBedData.DisplayName);
        LanguageHandler.SetLanguageLine($"{customBedData.TechType}_Description", customBedData.Description);
        LanguageHandler.SetLanguageLine($"Tooltip_{customBedData.TechType}", customBedData.Description);
        Main.Logger.LogInfo($"Registered custom bed prefab: {customBedData.TechType}");
    }
    
    
    private static Texture2D CombineBedTextures(
        Texture2D baseTexture, 
        Texture2D upperPillowTexture, 
        Texture2D lowerPillowTexture, 
        Texture2D blanketTexture, 
        Texture2D mattressTexture
    )
    {
        int width = baseTexture.width, height = baseTexture.height;
        Texture2D resultTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        
        Color[] basePixels = baseTexture.GetPixels();
        Color[] resultPixels = new Color[basePixels.Length];
        System.Array.Copy(basePixels, resultPixels, basePixels.Length);
        
        void PasteTexture(Texture2D sourceTexture, Rect targetRect)
        {
            float sourceAspect = (float)sourceTexture.width / sourceTexture.height;
            float targetAspect = targetRect.width / targetRect.height;

            float scale;
            float offsetX = 0, offsetY = 0;

            if (sourceAspect > targetAspect)
            {
                scale = targetRect.height / sourceTexture.height;
                float croppedWidth = sourceTexture.width - (targetRect.width / scale);
                offsetX = croppedWidth / 2;
            }
            else
            {
                scale = targetRect.width / sourceTexture.width;
                float croppedHeight = sourceTexture.height - (targetRect.height / scale);
                offsetY = croppedHeight / 2;
            }

            for (int i = 0; i < targetRect.height; i++)
            {
                for (int j = 0; j < targetRect.width; j++)
                {
                    float sourceX = (j / scale) + offsetX;
                    float sourceY = (i / scale) + offsetY;
                    
                    int sourcePixelX = Mathf.Clamp(Mathf.FloorToInt(sourceX), 0, sourceTexture.width - 1);
                    int sourcePixelY = Mathf.Clamp(Mathf.FloorToInt(sourceY), 0, sourceTexture.height - 1);

                    int targetX = (int)targetRect.x + j;
                    int targetY = (int)targetRect.y + i;
                    if (targetY < 0 || targetX >= width || targetX < 0 || targetY >= height)
                        continue;
                    
                    int pixelIndex = targetY * width + targetX;
                    Color sourceColor = sourceTexture.GetPixel(sourcePixelX, sourcePixelY);
                    
                    float alpha = basePixels[pixelIndex].a;
                    resultPixels[pixelIndex] = new  Color(sourceColor.r, sourceColor.g, sourceColor.b, alpha);

                }
            }
        }
        
        if (upperPillowTexture != null) PasteTexture(upperPillowTexture, UpperPillowRect);
        if (lowerPillowTexture != null) PasteTexture(lowerPillowTexture, LowerPillowRect);
        if (blanketTexture != null) PasteTexture(blanketTexture, BlanketRect);
        if (mattressTexture != null) PasteTexture(mattressTexture, MattressRect);
        
        resultTexture.SetPixels(resultPixels);
        resultTexture.Apply();
        return resultTexture;
    }
    
    private static Texture2D MakeReadable(Texture2D source)
    {
        RenderTexture rt = RenderTexture.GetTemporary(
            source.width,
            source.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear
        );

        Graphics.Blit(source, rt);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;
        
        Texture2D readableTexture = new Texture2D(source.width, source.height);
        readableTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        readableTexture.Apply();
        
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);
        
        return readableTexture;
    }

    private static void ModifyPrefabJson(GameObject prefab, CustomBedDataJson bedDataJson, string bedFolderPath)
    {
        Texture2D upperPillowTexture = null;
        Texture2D lowerPillowTexture = null;
        Texture2D blanketTexture = null;
        Texture2D mattressTexture = null;
        
        if (!string.IsNullOrWhiteSpace(bedDataJson.UpperPillowTexture))
            upperPillowTexture = ImageUtils.LoadTextureFromFile(Path.Combine(bedFolderPath, bedDataJson.UpperPillowTexture));

        if (!string.IsNullOrWhiteSpace(bedDataJson.LowerPillowTexture))
            lowerPillowTexture = ImageUtils.LoadTextureFromFile(Path.Combine(bedFolderPath, bedDataJson.LowerPillowTexture));

        if (bedDataJson.DisableBlanket != true && !string.IsNullOrWhiteSpace(bedDataJson.BlanketTexture))
            blanketTexture = ImageUtils.LoadTextureFromFile(Path.Combine(bedFolderPath, bedDataJson.BlanketTexture));

        if (!string.IsNullOrWhiteSpace(bedDataJson.MattressTexture))
            mattressTexture = ImageUtils.LoadTextureFromFile(Path.Combine(bedFolderPath, bedDataJson.MattressTexture));
        
        Texture2D baseTexture = null;

        GetBaseTexture(prefab, out baseTexture);
        
        if (baseTexture == null)
        {
            Main.Logger.LogError($"Base texture not found for bed {bedDataJson.TechType}");
            return;
        }

        if (!baseTexture.isReadable)
            baseTexture = MakeReadable(baseTexture);
        
        Texture2D combinedTexture = CombineBedTextures(baseTexture, upperPillowTexture, lowerPillowTexture, blanketTexture, mattressTexture);

        if (combinedTexture == null)
        {
            Main.Logger.LogError($"Error while combining textures for bed {bedDataJson.TechType}");
            return;
        }
        
        ApplyCombinedTextureToMaterial(prefab, combinedTexture);
    }
    
    private static void ModifyPrefab(GameObject prefab, CustomBedData customBedData)
    {
        Texture2D upperPillowTexture = customBedData.UpperPillowTexture;
        Texture2D lowerPillowTexture = customBedData.LowerPillowTexture;
        Texture2D blanketTexture = null;
        if (customBedData.DisableBlanket != true)
        {
            blanketTexture = customBedData.BlanketTexture;
        }
        Texture2D mattressTexture = customBedData.MattressTexture;
        
        Texture2D baseTexture = null;

        GetBaseTexture(prefab, out baseTexture);
        
        if (baseTexture == null)
        {
            Main.Logger.LogError($"Base texture not found for bed {customBedData.TechType}");
            return;
        }
        
        if (!baseTexture.isReadable)
            baseTexture = MakeReadable(baseTexture);
        
        Texture2D combinedTexture = CombineBedTextures(baseTexture, upperPillowTexture, lowerPillowTexture, blanketTexture, mattressTexture);

        if (combinedTexture == null)
        {
            Main.Logger.LogError($"Error while combining textures for bed {customBedData.TechType}");
            return;
        }
        
        ApplyCombinedTextureToMaterial(prefab, combinedTexture);
    }
    
    private static void ApplyCombinedTextureToMaterial(GameObject prefab, Texture2D combinedTexture)
    {
        var renderers = prefab.GetComponentsInChildren<Renderer>(true);
        foreach (var renderer in renderers)
        {
            foreach (var material in renderer.materials)
            {
                if (material.name.StartsWith("matress_pillow"))
                {
                    var newMaterial = new Material(material);
                    newMaterial.SetTexture("_MainTex", combinedTexture);
                    material.CopyPropertiesFromMaterial(newMaterial);
                }
            }
        }
    }
    
    private static void RemoveBlanket(GameObject prefab)
    {
        foreach (Transform child in prefab.GetComponentsInChildren<Transform>(true))
        {
            if (child.name.ToLower().Contains("blanket"))
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    
    private static void GetBaseTexture(GameObject prefab, out Texture2D baseTexture)
    {
        baseTexture = null;
        var renderers = prefab.GetComponentsInChildren<Renderer>(true);

        foreach (var renderer in renderers)
        {
            foreach (var material in renderer.materials)
            {
                var texture = material.GetTexture("_MainTex") as Texture2D;
                if (texture != null && material.name.StartsWith("matress_pillow"))
                {
                    baseTexture = texture;
                    return;
                }
            }
        }
    }
}