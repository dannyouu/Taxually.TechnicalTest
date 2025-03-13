I've refactored the controller to use a service method for the registration.

The separation of the code in the VatService is done with the "Strategy" pattern. 
We have an interface which every different registration method implements and it includes a "CountryCode" variable to help with the scafolding later.

I use a dictionary to hold all registration strategies and based on that I call the respective service.
Basic validation is added to the request model.
