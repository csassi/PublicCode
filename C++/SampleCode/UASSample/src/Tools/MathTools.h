#pragma once

///////////////////////////////////////////////////////////
//@file: Timer.h
//@author: Christian Sassi
//@purpose: Some math tools derived from old code and Wikipedia.
//@Generated: July 27, 2013
/////////////////////////////////////////////////////////// 

#include <cmath>  
#include <math.h>

//Could be #defines, I just chose not to.
const static double pi = 3.14159265358979323846;
const static double earthRadiusKm = 6371.0;
const static double oneKnotInFtPerSecond = 1.68781;
const static double oneKnotInMeterPerSecond = 0.514444444;
const static double kilometerToMile =  1.609344;
const static double mileToFt = 5280;

class MathTools
{
public:
    // This function converts decimal degrees to radians
    static double deg2rad(double deg) 
    {
        return (deg * pi / 180);
    }

    //  This function converts radians to decimal degrees
    static double rad2deg(double rad) 
    {
        return (rad * 180 / pi);
    }

    /**
    * Returns the distance between two points on the Earth in KM.
    * Direct translation from http://en.wikipedia.org/wiki/Haversine_formula
   */
    static double computeDistance(double lat1, double lon1, double lat2, double lon2) 
    {
        double theta, dist;
        theta = lon1 - lon2;
        dist = sin(deg2rad(lat1)) * sin(deg2rad(lat2)) + cos(deg2rad(lat1)) * cos(deg2rad(lat2)) * cos(deg2rad(theta));
        dist = acos(dist);
        dist = rad2deg(dist);
        dist = dist * 60 * 1.1515;
        dist *= 1.609344;
        return dist;
    }

};

//EOF