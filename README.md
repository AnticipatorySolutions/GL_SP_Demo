# GL_SP_Demo

Hi,

This is a .net core 2.2 web api project, use VisualStudio with the appropriate SDK installed.

To run the tests, update the NuGet Packages.

Once you've loaded the solution in VS, use IIS Express to run the API Project.
(You may want to adjust the launchSettings.json to disable the browser deployment.)

This project takes 2 approaches to resolve the problem of finding the shortest path between two locations, allowing for a direct comparison of call response times.

The first approach uses a look up to find the shortest route available between two locations from a repository of shortest paths.
This can be called with the following uri:

http://localhost:62301/api/Route/SP?src=KSA&dst=STW

        // GET: api/Route/SP?src=SOURCE_LOCATION_CODE3_ID&dst=DESTINATION_LOCATION_CODE3_ID
        // Param src = source location code 3 value of type string
        // Param dst = destination location code 3 value of type string
        // Returns piped string "|YYZ|DEN|" or message details.

The second approach uses Dijkstra to find the shortest path available between two nodes on a weighted graph.
This can be called with the following uri:

http://localhost:62301/api/Path/SP?src=KSA&dst=STW

        // GET: api/Path/SP?src=SOURCE_LOCATION_CODE3_ID&dst=DESTINATION_LOCATION_CODE3_ID
        // Param src = source location code 3 value of type string
        // Param dst = destination location code 3 value of type string
        // Returns piped string "|YYZ|DEN|" or message details.

        // Test Cases:
        //EXAMPLE: of 2 Step Trek:  src=YYZ&dst=LAX
        //EXAMPLE: of 12 Step Trek: src=ASF&dst=MAJ
        
        //FUN EXAMPLES: src= PWM or CRW or SPI or TMS or EIS or ENY or SXB to anywhere
        //FUN EXAMPLES: dst= NKC or GDN or TRD or PGA from anywhere
        //INTERESTING NOTE: src=LBL&dst=DDC or src=DDC&dst=LBL otherwise these nodes can be src but not dst
        
        
