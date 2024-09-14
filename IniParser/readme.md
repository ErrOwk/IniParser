# IniParser
A completely .NET based ini file operation class.

## Installation
[NuGet](https://www.nuget.org/packages/ErrOwk.IniParser/)

## Usage

### Initialization
```
using ErrOwk.IniParser;
...
IniParser iniFile = new IniParser("YourConfigFilePath");
```

### Update
```
iniFile.Update("sectionName","keyName","keyValue");
```

### Get
```
iniFile.Get("sectionName","keyName");
```

### Remove
```
iniFile.Remove("sectionName","keyName");
// Remove a key

iniFile.Remove("sectionName");
// Remove a section
```

### Example
```
using ErrOwk.IniParser;
...

IniParser iniFile = new IniParser("config.ini");

iniFile.Update("Example","Value1","001");
iniFile.Update("Example","Value2","002");
iniFile.Update("Example","Value2","020");
iniFile.Update("Example","Value3","003");

Console.WriteLine(iniFile.Get("Example","Value1"));
Console.WriteLine(iniFile.Get("Example","Value2"));
Console.WriteLine(iniFile.Get("Example","Value3"));
//Output :
// 001
// 020
// 003
// 
// config.ini:
// [Example]
// Value1=001
// Value2=020
// Value3=003

iniFile.Remove("Example","Value3");
Console.WriteLine(iniFile.Get("Example","Value3"));
//Output a empty string

iniFile.Remove("Example");
Console.WriteLine(iniFile.Get("Example","Value1"));
//Output a empty string
// config.ini is null

```