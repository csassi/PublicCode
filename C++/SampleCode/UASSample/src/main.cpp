///////////////////////////////////////////////////////////
//@file: main.cpp
//@author: Christian Sassi
//@purpose: Main entry point for a sample program written for LMCO.
//@Generated: July 26, 2013
/////////////////////////////////////////////////////////// 

#include <Tour/Tour.h>
#include <iostream>
#include <windows.h>

using std::exception;
using std::cerr;
using std::cout;
using std::endl;



void SetWindow(int Width, int Height)
{
    _COORD coord;
    coord.X = Width;
    coord.Y = Height;

    _SMALL_RECT Rect;
    Rect.Top = 0;
    Rect.Left = 0;
    Rect.Bottom = Height - 1;
    Rect.Right = Width - 1;

    HANDLE Handle = GetStdHandle(STD_OUTPUT_HANDLE);      // Get Handle
    SetConsoleScreenBufferSize(Handle, coord);            // Set Buffer Size
    SetConsoleWindowInfo(Handle, TRUE, &Rect);            // Set Window Size
}



int main()
{
    cout << "==================================================================" << endl;
    cout << "====================RUNNING LMCO SAMPLE CODE======================" << endl;
    cout << "==================================================================" << endl << endl;

    try
    {
        Tour::GetInstance()->Initalize(3.5);
        Tour::GetInstance()->Run();
    }
    catch (std::exception& e)
    {
        cerr << "ERROR:" << e.what() << endl;
    }

    Tour::GetInstance()->Shutdown();
    system ("PAUSE");

   return 0;
	
}

//EOF