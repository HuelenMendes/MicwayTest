Configuration :

1) Micway Project: Going to Web.config and change the connectionString named "MicwayEntities"

 <connectionStrings>
    <add name="MicwayEntities" connectionString="...
 <connectionStrings>


2)UnitTest project: Going to UnitTest.cs and change Uri property named "uriTest" 

 Uri uriTest = new Uri("http://.../api/drivers/");


3) Going to folder "Database".

 There is the database script to be created.

