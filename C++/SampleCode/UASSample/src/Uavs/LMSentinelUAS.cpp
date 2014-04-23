///////////////////////////////////////////////////////////
//@file: LMSentinelUAS.cpp
//@author: Christian Sassi
//@purpose: LMSentinelUAS, a object that describes a Lockheed Martin RQ-170 Sentinel drone.
//@Generated: July 28, 2013
/////////////////////////////////////////////////////////// 

#include "LMSentinelUAS.h"

using std::cout;
using std::endl;

LMSentinelUAS::LMSentinelUAS(string theACID, string theDescription, float theAltitude, float theHeading, float theLat, 
                             float theLon, float theAirspeed, float theMaxAirspeed, float theMaxCleiling) : BaseUAS(theACID, theDescription, theAltitude, theHeading, theLat, theLon, 
                             theAirspeed, theMaxAirspeed, theMaxCleiling)
{
}
LMSentinelUAS::LMSentinelUAS(string theACID, string theDescription, float theMaxAirspeed, float theMaxCleiling) : BaseUAS(theACID, theDescription,theMaxAirspeed, theMaxCleiling)
{

}

LMSentinelUAS::~LMSentinelUAS(void)
{
}

//For now the UAS will do the same thing - the idea is to have different functionality in each UAS.
void LMSentinelUAS::Update(double theElapsedTime)
{
    BaseUAS::Update(theElapsedTime);

    cout << "Lockheed Martin RQ-170 Sentinel Updating. [" << getACID().c_str() << "]" << endl;
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
void LMSentinelUAS::Initalize()
{
    setIsInitalized(true);
    //Setup sentinel specific stuff here.
}


//EOF