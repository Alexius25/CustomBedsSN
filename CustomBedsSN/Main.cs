using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using CustomBedsSN.Handler;
using HarmonyLib;
using Nautilus.Handlers;

namespace CustomBedsSN;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Main : BaseUnityPlugin
{
    public static string BedFolder => Path.Combine(Paths.PluginPath, "CustomBedsSN", "Beds");
    
    public new static ModConfig Config = OptionsPanelHandler.RegisterModOptions<ModConfig>();
    
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    
    private void Awake()
    {
        Logger = base.Logger;

        if (!Directory.Exists(BedFolder))
        {
            Directory.CreateDirectory(BedFolder);
            Logger.LogInfo($"Created bed folder at {BedFolder}");
        }
        
        if (!Config.EnableCustomBeds)
        {
            Logger.LogInfo("Custom Beds are disabled in the config.");
            return;
        }
        
        CustomBedsHandler.RegisterCustomBedsFolder(BedFolder);
        
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }
}