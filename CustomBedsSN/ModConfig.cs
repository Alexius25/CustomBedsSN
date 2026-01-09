using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Nautilus.Json;
using Nautilus.Options;
using Nautilus.Options.Attributes;

namespace CustomBedsSN;

[Menu("Custom Beds")]
public class ModConfig : ConfigFile
{
    [Toggle("Enable Custom Beds", Tooltip = "Enable or disable the Custom Beds mod. Warning: Disabling this will remove all custom beds from the game and replace them with standard beds.")]
    public bool EnableCustomBeds { get; set; } = true;
    
    [Toggle("<color=#f1c353><size=35>File Settings:</size></color> <alpha=#00>----------------------------------------------------------------------------------------------</alpha>")]
    public bool VisibilityDivider;
    
    [Button("Open Custom Beds Folder", Tooltip = "Opens the Folder where all your BedPacks Should be located")]
    public void OpenCustomBedsFolder(ButtonClickedEventArgs e)
    {
        ConfigUtils.OpenCustomBedsFolder();
    }

    [Button("Automatic BedPack Install", Tooltip = "Pick a Zip File containing BedPacks to be installed automatically")]
    public void InstallBedPack(ButtonClickedEventArgs e)
    {
        ConfigUtils.InstallBedPack();
    }
}

internal static class ConfigUtils
{
    internal static void OpenCustomBedsFolder()
    {
        try
        {
            Process.Start("explorer.exe", Main.BedFolder);
        }
        catch (System.Exception e)
        {
            Main.Logger.LogError($"Failed to open Custom Beds folder: {e.Message}");
        }
    }
    
    internal static void InstallBedPack()
    {
        try
        {
            string downloadsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");
            
            ShowFileDialog(downloadsPath);
        }
        catch (Exception e)
        {
            Main.Logger.LogError($"Failed to open file dialog: {e.Message}");
        }
    }

    private static void ExtractZipFile(string zipFilePath, string outputDirectory)
    {
        try
        {
            System.IO.Compression.ZipFile.ExtractToDirectory(zipFilePath, outputDirectory);
            Main.Logger.LogInfo($"Extracted zip file: {zipFilePath} to {outputDirectory}");
        }
        catch (Exception e)
        {
            Main.Logger.LogError($"Failed to extract zip file: {e.Message}");
        }
    }

    private static void ShowMessage(string title, string message)
    {
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    
    private static void ShowFileDialog(string initialDirectory)
    {
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            openFileDialog.InitialDirectory = initialDirectory;
            openFileDialog.Filter = "Zip files (*.zip)|*.zip|All files (*.*)|*.*";
            openFileDialog.Title = "Select a BedPack Zip File";

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            int counter = 0;
            
            foreach (var file in openFileDialog.FileNames)
            {
                string outputDir = Path.Combine(Main.BedFolder, Path.GetFileNameWithoutExtension(file));
                ExtractZipFile(file, outputDir);
                counter++;
            }
            
            ShowMessage("BedPack Installation", $"Successfully installed {counter} BedPack(s).");
        }
    }
}