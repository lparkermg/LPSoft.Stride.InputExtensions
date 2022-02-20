# LPSoft.Stride.InputExtensions
Input Extensions to use with Stride3D

## BuildInputConfig

BuildInputConfig is a package for building input configurations using Stride3Ds VirtualButtons, it allows setting up the bindings from dictionary and from json file.

Each from returns the current InputBuilder object and Build returns a populated VirtualButtonConfig object.

```
var builder = new InputBuilder();
var config = builder
  .FromDictionary(new Dictionary<string, VirtualButton[]> {{"Test_Binding", new[] { VirtualButton.GamePad.A }}})
  .FromJson("jsonFile.json")
  .Build();
```

`jsonFile.json`
```
{
    "Test_Binding_2": ["Keyboard.space", "GamePad.B"],
    ...
}
```

In the json file the key can be any string, where as any of the inputs use the same format that is returned by the VirtualButtons `.ToString()` function.
