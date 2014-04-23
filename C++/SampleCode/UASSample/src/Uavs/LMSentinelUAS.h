#pragma once

///////////////////////////////////////////////////////////
//@file: LMSentinelUAS.h
//@author: Christian Sassi
//@purpose: LMSentinelUAS, a object that describes a Lockheed Martin RQ-170 Sentinel drone.
//@Generated: July 28, 2013
/////////////////////////////////////////////////////////// 

#include "BaseUAS.h"
class LMSentinelUAS :
    public BaseUAS
{
private:

    LMSentinelUAS(const LMSentinelUAS&);
    LMSentinelUAS(); //No default construction, sorry.
    LMSentinelUAS& operator=(const LMSentinelUAS&);
    //Add Sentinel specific stuff here, armaments, flight characteristics, etc.

public:

    LMSentinelUAS(string theACID, string theDescription, float theAltitude, float theHeading, float theLat, 
        float theLon, float theAirspeed, float theMaxAirspeed, float theMaxCleiling);
    LMSentinelUAS(string theACID, string theDescription, float theMaxAirspeed, float theMaxCleiling);
    virtual ~LMSentinelUAS(void);

    virtual void Update(double theElapsedTime); 
    virtual void Initalize();
};

