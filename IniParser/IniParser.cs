using System.Collections;

namespace ErrOwk.IniParser
{
    public class IniParser
    {
        private string iniFilePath;

        private Dictionary<string,string> keyPairs = new Dictionary<string, string>();
        //存储配置文件的键值

        private Dictionary<string,Dictionary<string, string>> sections = new Dictionary<string,Dictionary<string, string>>();
        //存储section

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
                    //确认非空白行
                    if (currentLine != "")
                    {
                        //判断是否为section
                        if (currentLine.StartsWith('[') && currentLine.EndsWith(']'))
                        {
                            //将已写入的keyPairs存入该section中
                            if (currentSection != null && keyPairs != null)
                            {
                                sections.Add(currentSection, keyPairs);
                                keyPairs = new();
                            }

                            currentSection = currentLine.Substring(1, currentLine.Length - 2);
                        }
                        else
                        {
                            //将行内容分割为key和value写入keyPairs中
                            var currentkeyPair = currentLine.Split(['='], 2);
                            keyPairs!.Add(currentkeyPair[0], currentkeyPair[1]);
                        }
                    }
                }
                //将已写入的keyPairs存入该section中
                if (currentSection != null && keyPairs != null) sections.Add(currentSection, keyPairs);
            }
        }

        /// <summary>
        /// 返回设置项的值
        /// </summary>
        /// <param name="sectionName">Section</param>
        /// <param name="keyName">Key</param>
        /// <returns></returns>
        public string Get(string sectionName, string keyName)
        {
            LoadIniFile();
            return (string)sections[sectionName][keyName]!;
        }

        /// <summary>
        /// 修改/创建配置项
        /// </summary>
        /// <param name="sectionName">Section</param>
        /// <param name="keyName">Key</param>
        /// <param name="keyValue">修改/创建的值</param>
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
                    //创建KeyPairs
                    sections[sectionName].Add(keyName, keyValue);
                    Save();
                }
            }
            else
            {
                //设置Keypair
                keyPairs = new();
                keyPairs.Add(keyName, keyValue);
                //创建section
                sections.Add(sectionName, keyPairs);
                Save();
            }
        }

        /// <summary>
        /// 将保存的所有信息写入文件中
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
