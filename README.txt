Configuration :

1) Going to folder "Database".

 There is the database script to be created.
 
 -------------------------------------------------------------------------------------------------
2) Click on solution and restore NuGet Packages;

 -----------------------------------------------------------------------------------------------
3) Micway Project: Going to Web.config and change the connectionString named "MicwayEntities"

 <connectionStrings>
    <add name="MicwayEntities" connectionString="...
 <connectionStrings>
 
-------------------------------------------------------------------------------------------------
4)UnitTest project: 
  -> Going to UnitTest.cs and change Uri property named "uriTest" to your Micway Project Url
     Uri uriTest = new Uri("http://.../api/drivers/");

  -> Running the test: It needs to be set as startup project and on the solution -> properties
  set Multiple startup projects to:
  Micway   -> Action: Start without debugging
  UnitTest -> Action: Start
