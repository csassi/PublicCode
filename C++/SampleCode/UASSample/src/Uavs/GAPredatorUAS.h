#pragma once

///////////////////////////////////////////////////////////
//@file: GAPredatorUAS.h
//@author: Christian Sassi
//@purpose: GAPredatorUAS, a object that describes a General Atomics Predator drone.
//@Generated: July 27, 2013
/////////////////////////////////////////////////////////// 


#include <Uavs/BaseUAS.h>

class GAPredatorUAS :
    public BaseUAS
{
private:

    GAPredatorUAS(const GAPredatorUAS&);
    GAPredatorUAS(); //No default construction, sorry.
    GAPredatorUAS& operator=(const GAPredatorUAS&);
    //Add Predator specific stuff here, armaments, flight characteristics, etc.

public:

    GAPredatorUAS(string theACID, string theDescription, float theAltitude, float theHeading, float theLat, 
        float theLon, float theAirspeed, float theMaxAirspeed, float theMaxCleiling);
    GAPredatorUAS(string theACID, string theDescription, float theMaxAirspeed, float theMaxCleiling);
    virtual ~GAPredatorUAS(void);

    virtual void Update(double theElapsedTime); 
    virtual void Initalize();
};

//EOF

