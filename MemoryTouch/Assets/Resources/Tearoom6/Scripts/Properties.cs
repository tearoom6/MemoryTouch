using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Properties
{

    private string locale;
    private Dictionary<string, string> properties = new Dictionary<string, string>();

    /// <summary>
    /// Initializes a new instance of the <see cref="Properties"/> class.
    /// </summary>
    /// <param name="filename">Filename.</param>
    public Properties(string filename)
    {
        Logger.Info(string.Format("Language is {0}.", Application.systemLanguage));
        switch (Application.systemLanguage)
        {
            case SystemLanguage.Japanese:
                locale = "ja";
                break;
            case SystemLanguage.English:
            default:
                locale = "en";
                break;
        }

        string filePath = GameConstants.RESOURCE_SETTINGS_ROOT + locale + "/" + filename;
        Load(((TextAsset)Resources.Load(filePath)).text);
        Logger.Info(string.Format("Properties file ({0}) is loaded.", filePath));
    }

    /// <summary>
    /// Load the specified content.
    /// </summary>
    /// <param name="content">Content.</param>
    private bool Load(string content)
    {
        string[] lines = content.Split('\n');
        if (lines != null && lines.Length > 0)
        {
            foreach (string line in lines)
            {
                if (line.StartsWith("#"))
                    // skip comment
                    continue;
                string[] pair = line.Split('=');
                if (pair != null && pair.Length == 2)
                {
                    properties.Add(pair[0].Trim(), pair[1].Trim());
                }
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Get by the specified key and args.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="args">Arguments.</param>
    public string Get(string key, params object[] args)
    {
        if (!properties.ContainsKey(key))
            return "";
        if (args.Length == 0)
            return properties[key];
        return string.Format(properties[key], args);
    }
}
