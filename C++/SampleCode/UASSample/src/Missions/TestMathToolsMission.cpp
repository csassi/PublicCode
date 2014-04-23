///////////////////////////////////////////////////////////
//@file: TestMathToolsMission.cpp
//@author: Christian Sassi
//@purpose: I figured I would use the architecture I built for tests to save time,
//          typically it's nice to have a test suite to write some test code.
//@Generated: July 28, 2013
/////////////////////////////////////////////////////////// 

#include "Missions/TestMathToolsMission.h"
#include <iostream>

#include <Uavs/BaseUAS.h>
#include <Tools/MathTools.h>

using std::cout;
using std::endl;

TestMathToolsMission::TestMathToolsMission(BaseUAS& theUAS, string theDecription, string theIdentifier, bool isAcive) :
    BaseMission(theUAS, theDecription, theIdentifier, isAcive)
{
}

TestMathToolsMission::~TestMathToolsMission(void)
{
}


void TestMathToolsMission::Execute(double theElapsedTime)
{
    if(!getIsActive())
    {
        return;
    }

    BaseMission::Execute(theElapsedTime);

    float uasLat = 0;
    float uasLon = 0;

    float testLat = 1;
    float testLon = 0;

    //Some quick tests to check my math functions. These are cross referenced to calculators
    //online. http://www.csgnetwork.com/degreelenllavcalc.html

     float D = (float)MathTools::computeDistance(uasLat,uasLon,testLat,testLon);
     cout << "Mission - [" << getIdentifier() << "] Test 1: 1 degree of latitude is " << (int)D << " kilometers." << endl;
    
     //1 degree of lat is about 111 KM.
     if((int)D == 111)
     {
         cout << "Mission - [" << getIdentifier() << "] Test 1: PASS!" << endl;
     }
     else
     {
         cout << "Mission - [" << getIdentifier() << "] Test 1: FAIL!" << endl;
     }

     testLat = 0;
     testLon = 1;
     D = (float)MathTools::computeDistance(uasLat,uasLon,testLat,testLon);
     cout << "Mission - [" << getIdentifier() << "] Test 2: 1 degree of longitude is " << (int)D << " kilometers." << endl;
     //1 degree of lon is about 111 KM.

     if((int)D == 111)
     {
         cout << "Mission - [" << getIdentifier() << "] Test 2: PASS!" << endl;
     } 
     else
     {
         cout << "Mission - [" << getIdentifier() << "] Test 2: FAIL!" << endl;
     }

    setIsComplete(true);
    setIsActive(false);

    cout << "Mission - [" << getIdentifier() << "]: Complete!" << endl;
}

//EOF

