using System;
using CustomBedsSN.Dataclasses;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace CustomBedsSN.Handler;

public static class CustomBedsHandler
{
    
    /// <summary>
    /// Registers all custom beds located in the specified folder.
    /// </summary>
    /// <param name="customBedsFolder">Bed folder</param>
    public static void RegisterCustomBedsFolder(string customBedsFolder)
    {
        if (!Directory.Exists(customBedsFolder))
        {
            Main.Logger.LogError($"CustomBedsFolder doesn't exist: {customBedsFolder}");
            return;
        }
        
        var allJsonFiles = Directory.GetFiles(customBedsFolder, "*.json", SearchOption.AllDirectories);
        var bedfiles = allJsonFiles.Where(f => f.EndsWith(".bed.json", StringComparison.CurrentCultureIgnoreCase)).ToArray();
        Main.Logger.LogInfo($"Registered CustomBedsFolder: {customBedsFolder} with {bedfiles.Length} bed(s).");
        
        int index = 0;
        foreach (var bedFile in bedfiles)
        {
            string folder = Path.GetDirectoryName(bedFile);
            
            if (folder == null)
            {
                Main.Logger.LogError($"Couldn't get folder for bed file: {bedFile}");
                return;
            }
            CustomBedDataJson bedData;
            try
            {
                bedData = JsonConvert.DeserializeObject<CustomBedDataJson>(File.ReadAllText(bedFile));
            }
            catch (JsonException e)
            {
                Main.Logger.LogError($"Failed to deserialize bed file: {bedFile} with error: {e.Message}");
                return;
            }
            
            if (bedData == null)
            {
                Main.Logger.LogError($"Bed data is null for bed file: {bedFile}");
                return;
            }
            
            BedPrefabHandler.RegisterBedPrefabJson(bedData, folder);
            index++;
            Main.Logger.LogInfo($"Registered bed {index}/{bedfiles.Length}: {bedData.TechType}");
        }
    }
    
    /// <summary>
    /// Registers a single custom bed.
    /// </summary>
    /// <param name="bedData">The custom bed data to register.</param>
    public static void RegisterCustomBed(CustomBedData bedData)
    {
        BedPrefabHandler.RegisterBedPrefab(bedData);
        Main.Logger.LogInfo($"Registered custom bed: {bedData.TechType}");
    }
    
    /// <summary>
    /// Registers multiple custom beds.
    /// </summary>
    /// <param name="bedDatas">An array of custom bed data to register.</param>
    public static void RegisterCustomBeds(params CustomBedData[] bedDatas)
    {
        foreach (var bedData in bedDatas)
        {
            RegisterCustomBed(bedData);
        }
    }
}