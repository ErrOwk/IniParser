using System.Collections;

namespace ErrOwk.IniParser
{
    public class IniParser
    {
        private string iniFilePath;

        private Dictionary<string, string> keyPairs = new Dictionary<string, string>();

        //Stores the key value of the configuration file

        private Dictionary<string, Dictionary<string, string>> sections =
            new Dictionary<string, Dictionary<string, string>>();

        //store section

        public IniParser(string iniPath)
        {
            iniFilePath = iniPath;

            if (!File.Exists(iniFilePath))
            {
                FileStream fs = File.Create(iniFilePath);
                fs.Close();
            }

            LoadIniFile();
        }

        private void LoadIniFile()
        {
            using (StreamReader iniFile = new StreamReader(iniFilePath))
            {
                string currentLine;
                string currentSection = null!;

                while ((currentLine = iniFile.ReadLine()!) != null)
                {
                    currentLine = currentLine.Trim();
                    //Check if it is a non-blank line
                    if (currentLine != "")
                    {
                        //Determine whether it is a section
                        if (currentLine.StartsWith('[') && currentLine.EndsWith(']'))
                        {
                            //Store the written keyPairs in this section
                            if (currentSection != null && keyPairs != null)
                            {
                                sections.Add(currentSection, keyPairs);
                                keyPairs = new();
                            }

                            currentSection = currentLine.Substring(1, currentLine.Length - 2);
                        }
                        else
                        {
                            //Split the row content into key and value and write them into keyPairs
                            var currentkeyPair = currentLine.Split('=', 2);
                            keyPairs!.Add(currentkeyPair[0].Trim(), currentkeyPair[1].Trim());
                        }
                    }
                }
                //Store the written keyPairs in this section
                if (currentSection != null && keyPairs != null)
                    sections.Add(currentSection, keyPairs);
            }
        }

        /// <summary>
        /// Return the value of a key
        /// </summary>
        /// <param name="sectionName">The section of the key</param>
        /// <param name="keyName">Key</param>
        /// <returns></returns>
        public string Get(string sectionName, string keyName)
        {
            LoadIniFile();
            if (sections.ContainsKey(sectionName))
            {
                if (sections[sectionName].ContainsKey(keyName))
                {
                    return (string)sections[sectionName][keyName]!;
                }
                else
                    return "";
            }
            else
                return "";
        }

        /// <summary>
        /// Update/Create Key
        /// </summary>
        /// <param name="sectionName">The section of the key</param>
        /// <param name="keyName">The key that needs to be updated/created</param>
        /// <param name="keyValue">Value of the Key</param>
        public void Update(string sectionName, string keyName, string keyValue)
        {
            if (sections.ContainsKey(sectionName))
            {
                if (sections[sectionName].ContainsKey(keyName))
                {
                    sections[sectionName][keyName] = keyValue;
                    Save();
                }
                else
                {
                    //Create keyPairs
                    sections[sectionName].Add(keyName, keyValue);
                    Save();
                }
            }
            else
            {
                //Set keyPair
                keyPairs = new();
                keyPairs.Add(keyName, keyValue);
                //Create section
                sections.Add(sectionName, keyPairs);
                Save();
            }
        }

        /// <summary>
        /// Remove a keyValuePair
        /// </summary>
        /// <param name="sectionName">Section of the key</param>
        /// <param name="keyName">The key that needs to be removed</param>
        public void Remove(string sectionName, string keyName)
        {
            if (sections.ContainsKey(sectionName))
            {
                if (sections[sectionName].ContainsKey(keyName))
                {
                    sections[sectionName].Remove(keyName);
                    Save();
                }
            }
        }

        /// <summary>
        /// Remove a section
        /// </summary>
        /// <param name="sectionName">The section that needs to be removed</param>
        public void Remove(string sectionName)
        {
            if (sections.ContainsKey(sectionName))
            {
                sections.Remove(sectionName);
                Save();
            }
        }

        /// <summary>
        /// Write the info to the file
        /// </summary>
        private void Save()
        {
            using (TextWriter iniFile = new StreamWriter(iniFilePath))
            {
                foreach (var section in sections)
                {
                    iniFile.WriteLine($"[{section.Key}]");
                    foreach (KeyValuePair<string, string> keyValuePair in section.Value)
                    {
                        iniFile.WriteLine($"{keyValuePair.Key}={keyValuePair.Value}");
                    }
                }
            }
        }
    }
}
