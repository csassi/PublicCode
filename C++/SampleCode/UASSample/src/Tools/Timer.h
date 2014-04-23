#pragma once

///////////////////////////////////////////////////////////
//@file: Timer.h
//@author: Christian Sassi
//@purpose: This is the Timer, a high resolution timer, used for...timing. NOT USED!
//@Generated: July 27, 2013
/////////////////////////////////////////////////////////// 


#include <iostream>
#include <windows.h>

class Timer
{

private:
    LARGE_INTEGER m_Start;
    LARGE_INTEGER m_Stop;

public:
    Timer()
    {
        m_Start.QuadPart = 0;
        m_Stop.QuadPart = 0;
    }

    void Start()
    {
        QueryPerformanceCounter(&m_Start);
    }

    void Stop()
    {
        QueryPerformanceCounter(&m_Stop);
    }

    double ResultInNanoseconds()
    {
        LARGE_INTEGER frequency;
        QueryPerformanceFrequency(&frequency);
        double cyclesPerNanosecond = static_cast<double>(frequency.QuadPart) / 1000000000.0;

        LARGE_INTEGER elapsed;
        elapsed.QuadPart = m_Stop.QuadPart - m_Start.QuadPart;
        return elapsed.QuadPart / cyclesPerNanosecond;
    }
};

//EOF
