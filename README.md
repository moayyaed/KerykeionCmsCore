# KerykeionCms
Basic Content Management System for Asp.Net development in an Entity Framework Core environment.

The project itself as wel as a detailed documentation is still under development.

Obviously subject to improvement but feel free to clone and improve to your needs.

# Usage
In the future there will be public nuget packages for these projects, but for now you can clone this repository and build the projects locally. This will add a local nuget package in the bin\Debug folder of the specific project. Put these in the Program Files (x86)\Microsoft SDKs\NuGetPackages folder so Visual Studio can access these with the package manager.

If you don't want to use the package manager for adding the necessary nuget package simply add the folowing code in the .csproj file.

![alt text](https://github.com/Kerykeion7/KerykeionCmsCore/blob/master/PackageReference.PNG)

Save the file and it will add all needed supplements for the KerykeionCms.

Next create an Entity Framework Context that inherits from KerykeionCmsDbContext like so...

![alt text](https://github.com/Kerykeion7/KerykeionCmsCore/blob/master/Context.PNG)

Update the ConfigureServices method in the Startup.cs file like this...

![alt text](https://github.com/Kerykeion7/KerykeionCmsCore/blob/master/ConfigureServices.PNG)

Update the Configure method in the Startup.cs file like this...

![alt text](https://github.com/Kerykeion7/KerykeionCmsCore/blob/master/Configure.PNG)
