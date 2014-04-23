#pragma once

///////////////////////////////////////////////////////////
//@file: TimeToTargetMission.h
//@author: Christian Sassi
//@purpose: TimeToTargetMission, a mission that simply computes the time to target.
//@Generated: July 28, 2013
///////////////////////////////////////////////////////////

#include "Basemission.h"
class TimeToTargetMission :
    public BaseMission
{
private:

    TimeToTargetMission(const TimeToTargetMission&);
    TimeToTargetMission(); //No default construction, sorry.
    TimeToTargetMission& operator=(const TimeToTargetMission&);

public:
    TimeToTargetMission(BaseUAS& theUAS, float theTargetLat, float theTargetLon, string theDecription, string theIdentifier, bool isActive);
    ~TimeToTargetMission(void);
    float m_TargetLat;
    float m_TargetLon;
    virtual void Execute(double theElapsedTime); 

};

//EOF

