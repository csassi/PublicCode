#pragma once

///////////////////////////////////////////////////////////
//@file: BaseMission.h
//@author: Christian Sassi
//@purpose: BaseMission, an ABC that is the basis for a mission.
//@Generated: July 27, 2013
/////////////////////////////////////////////////////////// 

#include <string>
#include <boost/shared_array.hpp>

using std::string;

class BaseUAS;

class BaseMission
{
private:
    double m_CurrentMissionTime;
    boost::shared_ptr<BaseUAS> m_UAS; //Flying the mission
    string m_Description;
    string m_Identifier;
    bool m_isActive;
    bool m_isComplete;

    BaseMission(const BaseMission&);
    BaseMission(); //No default construction, sorry.
    BaseMission& operator=(const BaseMission&);

public:
    BaseMission(BaseUAS& theUAS, string theDescription, string theIdentifier, bool isActive);
    virtual ~BaseMission(void);

    virtual void Execute(double theElapsedTime)
    {
        m_CurrentMissionTime += theElapsedTime;
    }
            

    //Sets and Gets
    void setIsActive(bool isActive) {m_isActive = isActive;}
    void setIsComplete(bool isComplete) {m_isComplete = isComplete;}

    BaseUAS& getUAV() {return *m_UAS.get();}
    string getDescription() {return m_Description;}
    string getIdentifier() {return m_Identifier;}
    double getCurrentMissionTime() {return m_CurrentMissionTime;}
    bool getIsActive() {return m_isActive;}
    bool getIsComplete() {return m_isComplete;}
};

//EOF

