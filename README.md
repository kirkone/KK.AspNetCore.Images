# KK.AspNetCore.Images

This project contains some helpers to make image processing and handling in asp.net core websites easier.

## KK.AspNetCore.Images.Processing

> **Caution**: This is not finished jet!

This is a nuget package containing the middleware for resizing images on request.

### Build

The build environment for this project is on Visual Studio Team Services and can be found here [kirkone.visualstudio.com](https://kirkone.visualstudio.com/KK.AspNetCore.Images/_releases2?definitionId=2&view=mine&_a=releases)

| Name | Status |
| --- | --- |
| KK.AspNetCore.Images-CI | [![Build Status](https://kirkone.visualstudio.com/KK.AspNetCore.Images/_apis/build/status/KK.AspNetCore.Images-CI)](https://kirkone.visualstudio.com/KK.AspNetCore.Images/_build/latest?definitionId=22) |
| Alpha | [![Alpha](https://kirkone.vsrm.visualstudio.com/_apis/public/Release/badge/5ffc2eec-6944-4a03-a8b9-4f73af2f2237/2/4)](https://kirkone.visualstudio.com/KK.AspNetCore.Images/_release?definitionId=2&_a=releases) |
| Beta | [![Beta](https://kirkone.vsrm.visualstudio.com/_apis/public/Release/badge/5ffc2eec-6944-4a03-a8b9-4f73af2f2237/2/5)](https://kirkone.visualstudio.com/KK.AspNetCore.Images/_release?definitionId=2&_a=releases) |
| Release | [![Release](https://kirkone.vsrm.visualstudio.com/_apis/public/Release/badge/5ffc2eec-6944-4a03-a8b9-4f73af2f2237/2/6)](https://kirkone.visualstudio.com/KK.AspNetCore.Images/_release?definitionId=2&_a=releases) |

## KK.AspNetCore.Images.TagHelpers

The `picture` ThagHelper will take the settings from the `appsettings.json` and adds a `source` for every configured size.

### Attributes

#### src

This contains the name of the image you want to be used without the extension.  
You must provide this attribute otherwise the TagHelper will not be used.

#### alt

Here you can provide a text for the `alt` attribute of the image tag inside the `picture` element.  
This attribute is optional.

#### class

Here you can provide a text for the `class` attribute of the image tag inside the `picture` element.  
This attribute is optional.

#### style

Here you can provide some additional style. This will be added to the image tag inside the `picture` element.  
This attribute is optional.


### Example

The following TagHelper:

```
<picture class="awesome_image" style="width: 100%;" src="DSC01766" alt="A simple text." />
```

will become:

```
<picture>
    <source media="(max-width: 450px)" srcset="/images/generated/DSC01766/small.jpg, /images/generated/DSC01766/small2x.jpg 2x">
    <source media="(max-width: 600px)" srcset="/images/generated/DSC01766/medium.jpg, /images/generated/DSC01766/medium2x.jpg 2x">
    <source srcset="/images/generated/DSC01766/large.jpg">
    <img class="awesome_image" style="width: 100%;" alt="A simple text." src="DSC01766">
</picture>
```

## KK.AspNetCore.Images.Samples.Web

This web site uses the two helpers above and shows how to use each of them.  
The ThagHelper is used on the **Gallery** page.  
For the configuration have a look in the `Startup.cs` and `appsettings.json`.