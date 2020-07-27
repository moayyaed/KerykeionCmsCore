# KerykeionCms
Basic Content Management System for Asp.Net development.

Detailed documentation is still under development.

# Usage
In the future there will be public nuget packages for these projects, but for now you can clone this repository and build the projects locally. This will add a local nuget package in the bin\Debug folder of the specific project. Put these in the C:\Program Files (x86)\Microsoft SDKs\NuGetPackages folder so Visual Studio can access these with the package manager.

If you don't want to use the package manager for adding the necessary nuget package simply add the folowing code in the .csproj file.
  <ItemGroup>
    <PackageReference Include="KerykeionCmsCoreRazorLibrary" Version="1.0.28" />
  </ItemGroup>
