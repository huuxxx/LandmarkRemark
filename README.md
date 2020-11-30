# LandmarkRemark

# Backlog tasks

1 - This will be handled via the 3rd party mapping tool OpenLayers API Geolocation from the frontend - 'ol/Geolocation';
  - This could be automatically called on a periodic basis or manually via a UI button.

2 - From the front end I would use OpenLayers API method getCoordinateFromPixel(pixel) along with the note from an input field to call the local API Post method SaveNote().
  - The user could first click a 'new note' button then click the location on the map that they'd like to add the note. A dialogue for the note input would appear.

3 - I would call the local API to query the DB for others notes - GetNotes(). Then I would loop over the results and place them on the map using OpenLayers API - Text(opt_options)
  - This would possibly be called on initialisation, or as a UI button.

4 - The same approach as 3, just the parameter for GetNotes() bool selfNotes would be set to false.
  - As above

5 - I would call the local API get method SearchNotes(). I would then render the result (if any) to the map using OpenLayers API - Text(opt_options).
  - The UI would feature a 'search for notes' with inputs for 'user name' and 'note text'.  

# Overall time allocation - 5 hours

Time spent breakdown:

Design, planning, research and setup - 45 mins.

Implementation of API - 2 hours.

Integrate OpenLayers to the front end - 45 mins.

Documentation - 30 mins.

Packaging and delivering project - 15 mins.

# Known issues / limitations

The DB was not implemented into this solution and a basic DB design was assumed.

There is no login/authorisation functionality and the CORS policy is set to all sites.

The UI is only theorised. I would use a React Gatsby template.

The API contains catch blocks for errors, but they are not logged or visualised.

There is no mapping implemented into the GetNotes() API method. I would utilise AutoMapper https://automapper.org/

# Tools Used

IDE - VS2019 Community.

ORM - Entity Framework Core.

OpenLayers - Mapping API free to use https://openlayers.org/
