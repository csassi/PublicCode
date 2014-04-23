Name: Christian Sassi

Project: 
    Sample Code.

Description:
    This is a sample system that uses a global 'Tour' object that manages UAS objects. The UAS objects are updated
    with a 'time' variable passed in. It is possible to update the UAS based on time; position, heading and altitude are fields
    each UAS has. Each UAS object is able to have 'Missions' registered with it. Missions also have the functionality to be 
    updated via a 'time' variable. In this sample there are two UAS objects extending functionality from a 'BaseUAS' object.
    There are also two mission: 'TimeToTargetMission' and 'TestMathToolsMission' that extend functionality from a 'BaseMission'
    abstract base class. The 'TimeToTargetMission' object simply calculates the Time To Target based on a UAS's airspeed and a passed
    in 'targetLat' and 'targetLon'. The 'TestMathToolsMission' tests the math functionality that the 'TimeToTargetMission' object
    utilizes. This applicaton uses Boost version 1.54.

Know Issues (to me):
    -Boost is not local to this directory structure. It will not build on another windows machine.
    -The 'Tour' time duration should be a command line option.
    -'Tour' object does not use 'Timer' class, Boost timer object should be used instead.
    -Using 'Uav' and 'Uas' to descibe the same thing needs to be refactored.
    -Calculations done in meters while uas objects use feet for altitude.
