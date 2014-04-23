///////////////////////////////////////////////////////////
//@file: TimeToTargetMission.cpp
//@author: Christian Sassi
//@purpose: TimeToTargetMission, a mission that simply computes the time to target.
//@Generated: July 28, 2013
///////////////////////////////////////////////////////////

#include "Missions/TimeToTargetMission.h"

#include <iostream>

#include <Tools/MathTools.h>
#include <Uavs/BaseUAS.h>

using std::cout;
using std::endl;

TimeToTargetMission::TimeToTargetMission(BaseUAS& theUAS, float theTargetLat, float theTargetLon, string theDecription, string theIdentifier, bool isAcive) :
    m_TargetLat(theTargetLat), m_TargetLon(theTargetLon), BaseMission(theUAS, theDecription, theIdentifier, isAcive)
{
}


TimeToTargetMission::~TimeToTargetMission(void)
{
}

void TimeToTargetMission::Execute(double theElapsedTime)
{
    if(!getIsActive())
    {
        return;
    }
    
    BaseMission::Execute(theElapsedTime);

    float uasLat = getUAV().getLat();
    float uasLon = getUAV().getLon();

    //d = v / t  
    //v = d * t
    //t = d / v

    //I am not worried about double precision.
    //I want meters/second for now, since 'computeDistance' returns KM, multiply by 1000 to get meters.
    float D = (float)MathTools::computeDistance(uasLat,uasLon,m_TargetLat,m_TargetLon) * 1000;
    float V = (float)(getUAV().getAirspeed() * oneKnotInMeterPerSecond); 
    float T = D / V;
    cout << "Mission - [" << getIdentifier() << "]: Time to target at the current airspeed [" 
        << getUAV().getAirspeed() << "] is " << (int)T << " seconds." << endl;

    setIsComplete(true);
    setIsActive(false);

    cout << "Mission - [" << getIdentifier() << "]: Complete!" << endl;
}

//EOF
