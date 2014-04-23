Name: Christian Sassi

Project: 
    Sample Code.

Description:
    This is a sample messaging system written in Java that utilizes a simple object to register for messages from other objects. I needed a way for objects to talk to each other without directly calling specific functions from those objects. Using Java's nifty reflection, I can call functions by their string representation. The sample comes with a manager that objects can register for user defined message types. Users can resolve  atributes from the messages via the CAEMessage object.

Know Issues off the top of my head:
    -Java reflection is pretty slow. Lots of messages might result in a bottleneck.
    -Vector<> isn't the best collection to use as it's depricated.
    
