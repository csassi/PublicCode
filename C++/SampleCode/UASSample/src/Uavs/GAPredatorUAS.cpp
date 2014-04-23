///////////////////////////////////////////////////////////
//@file: GAPredatorUAS.cpp
//@author: Christian Sassi
//@purpose: GAPredatorUAS, a object that describes a General Atomics Predator drone.
//@Generated: July 27, 2013
/////////////////////////////////////////////////////////// 


#include "GAPredatorUAS.h"
#include <iostream>

using std::cout;
using std::endl;

GAPredatorUAS::GAPredatorUAS(string theACID, string theDescription, float theAltitude, float theHeading, float theLat, 
    float theLon, float theAirspeed, float theMaxAirspeed, float theMaxCleiling) : BaseUAS(theACID, theDescription, theAltitude, theHeading, theLat, theLon, 
    theAirspeed, theMaxAirspeed, theMaxCleiling)
{
}
GAPredatorUAS::GAPredatorUAS(string theACID, string theDescription, float theMaxAirspeed, float theMaxCleiling) : BaseUAS(theACID, theDescription,theMaxAirspeed, theMaxCleiling)
{

}

GAPredatorUAS::~GAPredatorUAS(void)
{
}


void GAPredatorUAS::Update(double theElapsedTime)
{
    BaseUAS::Update(theElapsedTime);

    cout << "General Atomics MQ-1 Predator Updating. [" << getACID().c_str() << "]" << endl;
    if (getCurrentMission().get() && getCurrentMission().get()->getIsActive())
    {
        getCurrentMission().get()->Execute(theElapsedTime);
    }
    else
    {
        CheckNextMission();
    }

    //Update predator specific stuff here.
}
void GAPredatorUAS::Initalize()
{
    setIsInitalized(true);
    //Setup predator specific stuff here.
}


//EOF
