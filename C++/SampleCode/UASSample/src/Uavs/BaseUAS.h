#pragma once

///////////////////////////////////////////////////////////
//@file: BaseUAS.h
//@author: Christian Sassi
//@purpose: BaseUAS, an ABC that is the basis for a UAS.
//@Generated: July 27, 2013
/////////////////////////////////////////////////////////// 

#include <string>
#include <stdexcept>
#include <vector>
#include <boost/shared_ptr.hpp>
#include <Missions/BaseMission.h>

using std::string;
using std::vector;

class BaseUAS
{
private:

//     VS 2012 doesn't support this???....
//     BaseUAS(const BaseUAS&) = delete;
//     BaseUAS& operator=(const BaseUAS&) = delete;
    BaseUAS(const BaseUAS&);
    BaseUAS(); //No default construction, sorry.
	BaseUAS& operator=(const BaseUAS&);

	float m_Altitude; //In feet
	float m_Heading;  //In Degrees.
	float m_Lat;      //In Degrees.
	float m_Lon;      //In Degrees.
    float m_Airspeed; //In Knots.
    float m_MaxAirspeed; //In Knots.
    float m_MaxCleiling; //In feet.
    double m_MaxFlightTime; //In seconds
    double m_CurrentFlightTime; //In seconds

    bool m_isActive;
    bool m_isInitalized;
    
    string m_Description;

    int m_CurrentMissionIndex;
    int m_NumMissions;

    vector<boost::shared_ptr<BaseMission>> m_Missions;
    boost::shared_ptr<BaseMission> m_CurrentMission;
    string m_ACID;

public:
    BaseUAS(string theACID, string theDescription, float theAltitude, float theHeading, float theLat, 
            float theLon, float theAirspeed, float theMaxAirspeed, float theMaxCleiling);
    BaseUAS(string theACID, string theDescription, float theMaxAirspeed, float theMaxCleiling);

	virtual ~BaseUAS(void);

   /**
    * Base update function. Be sure to call this in your override!
    */
	virtual void Update(double theElapsedTime)
    {
        if(!getIsInitalized())
        {
            string acid = getACID();
            string error = "Uas object [" + acid + "] is not initialized. Badness may ensue.";
            throw std::runtime_error(error); 
        }

        m_CurrentFlightTime += theElapsedTime;
    }
    /**
    * Base Initalize function. You should probably implement this...
    */
	virtual void Initalize() = 0; 

   /**
    * Register function that adds a mission to the UAS's mission container.
    */
    void RegisterMission(BaseMission* theMission)
    {
        if (!theMission)
        {
            throw std::invalid_argument("Trying to register a null Mission. Badness may ensue."); 
        }
        boost::shared_ptr<BaseMission> nuMission(theMission);
        m_Missions.push_back(nuMission);
        if (!getCurrentMission().get())
        {
            setCurrentMission(0);
        }
    }
    //Sets
    void setACID(string theACID) {m_ACID = theACID;}
    void setAltitude(float theAltitude) 
    { 
        if(theAltitude > m_MaxCleiling) 
        {
            throw std::out_of_range("Altitude for UAS is trying to be set higher than it's ceiling. Badness may ensue."); 
        }
        m_Altitude = theAltitude;
    }
    void setMaxFlightTime(double theFlighttime) {m_MaxFlightTime = theFlighttime;}
    void setCurrentFlightTime(double theFlighttime) {m_CurrentFlightTime = theFlighttime;}
    void setHeading(float theHeading) {m_Heading = theHeading;}
    void setLat(float theLat) {m_Lat = theLat;}
    void setLon(float theLon) {m_Lon = theLon;}
    void setAirspeed(float theAirspeed) 
    {
        if(theAirspeed > m_MaxAirspeed) 
        {
            throw std::out_of_range("Airspeed for UAS is trying to be set higher than it's max airspeed. Badness may ensue."); 
        }
        m_Airspeed = theAirspeed;
    }
    void setIsActive(bool isActive) {m_isActive = isActive;}
    void setIsInitalized(bool isInit) {m_isInitalized = isInit;}
    void setMaxAirspeed(float theMaxAirspeed) {m_MaxAirspeed= theMaxAirspeed;}
    void setMaxCleiling(float theMaxCleiling) {m_MaxCleiling = theMaxCleiling;}
    void setCurrentMission(int theMissionIndex) 
    {
        if(theMissionIndex < 0 || theMissionIndex >= (int)m_Missions.size())
        {
           return; //no more missions.
        }

        m_CurrentMission = m_Missions[theMissionIndex];
    }

    //Gets
    bool getIsActive() {return m_isActive;}
    bool getIsInitalized() {return m_isInitalized;}
    string getACID() {return m_ACID;}
    string getDescription() {return m_Description;}
    float getAltitude() {return m_Altitude;}
    float getHeading() {return m_Heading;}
    float getLat() {return m_Lat;}
    float getLon() {return m_Lon;}
    float getAirspeed() {return m_Airspeed;}
    float getMaxAirspeed() {return m_MaxAirspeed;}
    double getCurrentFlightTime() {return m_CurrentFlightTime;}
    double getMaxFlightTime() {return m_MaxFlightTime;}
    float getMaxCleiling() {return m_MaxCleiling;}
    boost::shared_ptr<BaseMission> getCurrentMission() {return m_CurrentMission;}
    int getCurrentMissionIndex() {return m_CurrentMissionIndex;}

   /**
    * Function that checks the current mission for completeness and tries to goto the next
    * mission if possible, call this if you want the base class to change missions for you
    * in a basic way.
    */
    void CheckNextMission();

};

//EOF
