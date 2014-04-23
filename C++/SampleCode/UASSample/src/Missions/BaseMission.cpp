///////////////////////////////////////////////////////////
//@file: BaseMission.cpp
//@author: Christian Sassi
//@purpose: BaseMission, an ABC that is the basis for a mission.
//@Generated: July 27, 2013
/////////////////////////////////////////////////////////// 

#include "BaseMission.h"
#include <Uavs/BaseUAS.h>

BaseMission::BaseMission(BaseUAS& theUAS, string theDescription, string theIdentifier, bool isActive)
{
    m_UAS.reset(&theUAS);
    m_Description = theDescription;
    m_Identifier = theIdentifier;
    m_isActive =  isActive;
}


BaseMission::~BaseMission(void)
{
}

//EOF
