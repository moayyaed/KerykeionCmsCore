# KerykeionCms
Basic Content Management System for Asp.Net development in an Entity Framework Core environment.

The project itself as wel as a detailed documentation is still under development.

Subject to improvement so feel free to clone and improve to your needs.

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

The last thing to do is add migrations and update the database. You can add Entities to the Context before you create the database but this can also be done afterwards. 
These (extra) code first added Entities will be able to be manipulated (performing CRUD operations on them) in the KerykeionCms.

# Translations API
Another Necessary step is to clone the following API project https://github.com/Kerykeion7/KerykeionTranslationAPI and add it as an existing project to your solution.
Run both projects in the solution to be able to consume the KerykieonTranslations API.

Another (maybe) necessary step might be that you update the API Urls in the KerykeionCmsCore/Services/KerykeionTranslationsService.cs file.
After you've done this you will have to update the versions of your project, in the future I'll look to Host this API on the web.

# Access
To access the KerykeionContentManagementSystem pages there will have to be an User that is in an Administrator role. You can do this in the server itself or you can make use of a workaround I've provided.

Add the following highlighted code to the ConfigureServices method in the Startup.cs file of your main project.

![alt text](https://github.com/Kerykeion7/KerykeionCmsCore/blob/master/Access.PNG)

This will grant anyone access to the KerykeionCms pages, so only to be used in Development Environment.
Now you can explore the KerykeionCms.

![alt text](https://github.com/Kerykeion7/KerykeionCmsCore/blob/master/Manager.PNG)

I will leave you here but there is a more detailed documentation underway.
