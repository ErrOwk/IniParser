# IniParser
A completely .NET based ini file operation class.

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

### Example
```
using ErrOwk.IniParser;
...

IniParser iniFile = new IniParser("config.ini");

string value = "Test123";
iniFile.Update("Example","Value",value);

Console.WriteLine(iniFile.Get("Example","Value"));

//Output : Test123


```