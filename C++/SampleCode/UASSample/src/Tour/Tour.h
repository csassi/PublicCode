#pragma once

///////////////////////////////////////////////////////////
//@file: Tour.h
//@author: Christian Sassi
//@purpose: This is the Tour, a single instance class that will take care
//          of managing the UAS and updating them and their missions.
//@Generated: July 27, 2013
/////////////////////////////////////////////////////////// 


#include <Uavs/BaseUAS.h>
#include <boost/shared_ptr.hpp>
#include <boost/timer.hpp>
#include <vector>

using std::vector;

class Tour
{
private:
    double m_TotalTourTime;
    vector<boost::shared_ptr<BaseUAS>> m_UAS;
    boost::timer m_Timer;
    bool m_isRunning;
    bool m_IsInitalized;
    double m_MaxTourTime;

    Tour(void);
    //	copy constructor
    Tour(const Tour&);
    //	assignment operator
    Tour& operator=(const Tour&);
    //	Plus destructor!
   virtual ~Tour(void);
   /**
    * Function that updates the UAS objects
    */
   void UpdateUAS(double theElapsedTime);

    /**
    * Updates the tours current time and checks to see if we can end it.
    */
   bool CheckTime(double theTime);

public:
     /**
    * Main loop of the Tour.
    */
    void Run();

     /**
    * Initializes the tour, pass in the tour time.
    */
    void Initalize(double tourTime);
    void Shutdown();
    static Tour* GetInstance(void);

     /**
    * To add a UAS to the tour, call this function.
    */
    void RegisterUAS(BaseUAS* theBaseUAS)
    {
        if (!theBaseUAS)
        {
            throw std::invalid_argument("Trying to register a null UAS. Badness may ensue."); 
        }
        if (!theBaseUAS->getIsInitalized())
        {
            theBaseUAS->Initalize();
        }

        boost::shared_ptr<BaseUAS> nuBaseUAS(theBaseUAS);
        m_UAS.push_back(nuBaseUAS);
    }

};

