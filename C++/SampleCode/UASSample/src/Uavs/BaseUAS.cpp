///////////////////////////////////////////////////////////
//@file: BaseUAS.cpp
//@author: Christian Sassi
//@purpose: BaseUAS, an ABC that is the basis for a UAS.
//@Generated: July 27, 2013
/////////////////////////////////////////////////////////// 


#include "BaseUAS.h"


BaseUAS::BaseUAS(string theACID, string theDescription, float theAltitude, float theHeading, float theLat, 
        float theLon, float theAirspeed, float theMaxAirspeed, float theMaxCleiling) : 
        m_ACID(theACID), m_Description(theDescription), m_Altitude(theAltitude), 
        m_Heading(theHeading), m_Lat(theLat), m_Lon(theLon), m_Airspeed(theAirspeed), 
        m_MaxAirspeed(theMaxAirspeed), m_MaxCleiling(theMaxCleiling), m_isInitalized(false), m_CurrentMissionIndex(0)
{
    
}

BaseUAS::BaseUAS(string theACID, string theDescription, float theMaxAirspeed, float theMaxCleiling) :
     m_ACID(theACID), m_Description(theDescription), m_MaxAirspeed(theMaxAirspeed), m_MaxCleiling(theMaxCleiling), m_isInitalized(false), m_CurrentMissionIndex(0)
{
    setAltitude(10000);
    setHeading(90);
    setLat(0);
    setLon(0);
    setAirspeed(140);
}

BaseUAS::~BaseUAS(void)
{
}

void BaseUAS::CheckNextMission()
{
    if(getCurrentMission() && getCurrentMission().get()->getIsComplete() && !getCurrentMission().get()->getIsActive())
    {
        int newIndex = ++m_CurrentMissionIndex;
        setCurrentMission(newIndex);
    }
}


//EOF