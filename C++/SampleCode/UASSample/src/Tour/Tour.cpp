///////////////////////////////////////////////////////////
//@file: Tour.cpp
//@author: Christian Sassi
//@purpose: This is the Tour, a single instance class that will take care
//          of managing the UAS and updating them and their missions.
//@Generated: July 27, 2013
/////////////////////////////////////////////////////////// 

#include "Tour.h"
#include <boost/bind.hpp>
#include <iostream>
#include <stdexcept>
#include <Uavs/GAPredatorUAS.h>
#include <Uavs/LMSentinelUAS.h>
#include <Missions/TestMathToolsMission.h>
#include <Missions/TimeToTargetMission.h>

#include <Tools/Timer.h>

using std::cout;
using std::endl;

Tour::Tour(void)
{
}


Tour::~Tour(void)
{
}

void Tour::Run()
{
    if(!m_IsInitalized)
    {
        throw std::runtime_error("Global Tour object is not initialized. Badness may ensue."); 
    }

    double theTime = 0.0;
    DWORD TimeStamp = 0;
    float ElapsedTime = 0;
    DWORD PreviousTime = GetTickCount();

    m_isRunning = true;

    cout << "Tour has Started!!" << endl;

    while (m_isRunning)
    {
        //TODO: Replace with Timer.h or Boost timer.
        DWORD CurrentTime = GetTickCount();
        ElapsedTime = (float)(CurrentTime - PreviousTime) / 1000.0f;
        PreviousTime = CurrentTime;
       
        //Do stuff...

        UpdateUAS(ElapsedTime);

        if (CheckTime(ElapsedTime))
        {
            break;
        }

        //TODO: This should be done another way...
        Sleep(100);
    }
}

void Tour::Initalize(double tourTime)
{
    m_IsInitalized = true;
    m_isRunning = false;
    m_MaxTourTime = tourTime;
    m_TotalTourTime = 0;

    GAPredatorUAS* pred = new GAPredatorUAS("CSASSI_PRED", "GA PREDATOR", 240, 25000);
    GAPredatorUAS* pred2 = new GAPredatorUAS("CSASSI_PRED2", "GA PREDATOR2", 140, 25000);
    LMSentinelUAS* sentinel1 = new LMSentinelUAS("CSASSI_SENT1", "LM SENTINEL", 200, 50000);

    pred->RegisterMission(new TimeToTargetMission(*pred,1,0,"Predator 1 Time To Target Mission 1.", "PRED_TTT1", true));
    pred->RegisterMission(new TimeToTargetMission(*pred,2,0,"Predator 1 Time To Target Mission 2.", "PRED_TTT2", false));
    sentinel1->RegisterMission(new TestMathToolsMission(*sentinel1, "Testing math functions!", "SENT_TESTING1",true));

    RegisterUAS(pred);
    RegisterUAS(pred2);
    RegisterUAS(sentinel1);
}

void Tour::Shutdown()
{

}

Tour* Tour::GetInstance(void)
{
    //The Lazy way.
    static Tour instance;
    return &instance;
}

void Tour::UpdateUAS(double theElapsedTime)
{
    std::for_each (m_UAS.begin(), m_UAS.end(),
        boost::bind(&BaseUAS::Update, _1, theElapsedTime));
}

bool Tour::CheckTime(double theTime)
{
    m_TotalTourTime += theTime;
    if(m_TotalTourTime >= m_MaxTourTime)
    {
        cout << "Tour has ended after " << m_TotalTourTime << " seconds!!" << endl;
        m_isRunning = false;
        return true;
    }
    return false;
}
