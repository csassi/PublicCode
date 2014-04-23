#pragma once

///////////////////////////////////////////////////////////
//@file: TestMathToolsMission.h
//@author: Christian Sassi
//@purpose: I figured I would use the architecture I built for tests to save time,
//          typically it's nice to have a test suite to write some test code.
//@Generated: July 28, 2013
/////////////////////////////////////////////////////////// 


#include "Missions/BaseMission.h"

class TestMathToolsMission :
    public BaseMission
{
private:
    TestMathToolsMission(const TestMathToolsMission&);
    TestMathToolsMission(); //No default construction, sorry.
    TestMathToolsMission& operator=(const TestMathToolsMission&);

public:
    TestMathToolsMission(BaseUAS& theUAS, string theDecription, string theIdentifier, bool isActive);
    virtual ~TestMathToolsMission(void);
    virtual void Execute(double theElapsedTime); 
};

//EOF

