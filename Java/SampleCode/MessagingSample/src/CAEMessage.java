///////////////////////////////////////////////////////////
//@file: CAEMessage.java
//@author: Christian Sassi - Sass
//@purpose: This is the AEMessage module.
//@Generated: Jan 24, 2014 : 11:10:25 PM
///////////////////////////////////////////////////////////
package com.EFB.Messaging;

import javax.swing.text.StyledEditorKit.BoldAction;

/**
 * The Class CAEMessage.
 *
 * @author Sass
 */


public class CAEMessage
{
	
	/** The message. */
	String m_szMessage; 
	
	/** The message type. */
	String m_szType;
	
	/**
	 * Instantiates a new AndE message.
	 *
	 * @param szMessage - The message to send. format "Type TestMessage 3". You can just send a type 
	 * 					  IE: "Type Shutdown" or "Type MovePlayer XPos 231 YPos 234". Even number
	 * 					  of elements in the message! If you could have 3 elements in your message, a Type 
	 * 					  TYPE and a value to go along with it.
	 */
	public CAEMessage(String szMessage)
	{
		
		m_szMessage = szMessage;
	}
	
	/**
	 * Gets the value of the message as a float.
	 *
	 * @return the float
	 */
	public float GetAsFloat()
	{
		String[] message = CheckMessage();
		if(message == null)
		{
			System.out.println("ERROR Invalid Message for message" + m_szMessage);
			return -0;
		}
		
		return Float.parseFloat(message[2]);
	}
	
	/**
	 * Gets the value of the message as an int.
	 *
	 * @return the int
	 */
	public int GetAsInt()
	{
		String[] message = CheckMessage();
		if(message == null)
		{
			System.out.println("ERROR Invalid Message for message" + m_szMessage);
			return -0;
		}
		
		return Integer.parseInt(message[2]);
	}
	
	/**
	 * Gets the value of the message as a bool.
	 *
	 * @return the boolean
	 */
	public Boolean GetAsBool()
	{
		String[] message = CheckMessage();
		if(message == null)
		{
			System.out.println("ERROR Invalid Message for message" + m_szMessage);
			return false;
		}
		
		return Boolean.parseBoolean(message[2]);
	}
	
	/**
	 * Gets the type of the message.
	 *
	 * @return the string
	 */
	public String GetType()
	{
		String[] message = CheckMessage();
		if(message == null)
		{
			System.out.println("ERROR Invalid Message for message" + m_szMessage);
			return "ERROR!";
		}
		
		return m_szType;
	}
	
	/**
	 * Gets the value of the message as a string.
	 *
	 * @return the string
	 */
	public String GetString()
	{
		String[] message = CheckMessage();
		if(message == null)
		{
			System.out.println("ERROR Invalid Message for message" + m_szMessage);
			return "ERROR!";
		}
		
		return message[2];
	}
	
	/**
	 * Gets the value of the message as a float.
	 *
	 * @return the float
	 * @param szElement - Specific element in the message you want.
	 */
	public float GetAsFloat(String szElement)
	{
		String[] message = CheckMessage();
		if(message == null)
		{
			System.out.println("ERROR Invalid Message for message" + m_szMessage);
			return -0;
		}
		
		for(int i = 2; i < message.length; i+=2)
		{
			if(message[i].contentEquals(szElement))
			{
				return Float.parseFloat(message[i+1]);
			}
		}
		
		return -1;
	}
	
	/**
	 * Gets the value of the message as an int.
	 *
	 * @return the int
	 * @param szElement - Specific element in the message you want.
	*/
	public int GetAsInt(String szElement)
	{
		String[] message = CheckMessage();
		if(message == null)
		{
			System.out.println("ERROR Invalid Message for message" + m_szMessage);
			return -0;
		}
		
		for(int i = 2; i < message.length; i+=2)
		{
			if(message[i].contentEquals(szElement))
			{
				return Integer.parseInt(message[i+1]);
			}
		}
		return -1;
	}
	
	/**
	 * Gets the value of the message as a bool.
	 *
	 * @return the boolean
	 * @param szElement - Specific element in the message you want.
	*/
	public Boolean GetAsBool(String szElement)
	{
		String[] message = CheckMessage();
		if(message == null)
		{
			System.out.println("ERROR Invalid Message for message" + m_szMessage);
			return false;
		}
		for(int i = 2; i < message.length; i+=2)
		{
			if(message[i].contentEquals(szElement))
			{
				return Boolean.parseBoolean(message[i+1]);
			}
		}
		
		return false;
	}
		
	/**
	 * Gets the value of the message as a string.
	 *
	 * @return the string
	 * @param szElement - Specific element in the message you want.
	 */
	public String GetString(String szElement)
	{
		String[] message = CheckMessage();
		if(message == null)
		{
			System.out.println("ERROR Invalid Message for message" + m_szMessage);
			return "ERROR!";
		}
		
		for(int i = 2; i < message.length; i+=2)
		{
			if(message[i].contentEquals(szElement))
			{
				return message[i+1];
			}
		}
		return message[2];
	}
		
	/**
	 * Checks the message for validity.
	 *
	 * @return - string[], Array of the message 1st element is "Type", 2nd is "TYPE", 3rd is the value, if any.
	 * @throws Error - Your message was not an even number of elements. (This does not apply if you have a 
	 *                     message thats 'Type PlayMusic 2'. It apply's to a message that's 'Type MovePlayer XPos 45 Ypos 48').
	 */
	private String[] CheckMessage() 
	{
		 String[] message = m_szMessage.split(" ");
	     String szType = message[0].toString();
	     Boolean isEqual = szType.equalsIgnoreCase("Type");
	     if(((message.length) % 2) == 1 && message.length != 3)
	     {
	    	 System.out.println("ERROR - Message:--" + message + "-- " + "Need even number of elements!! " +
	    	 		"Dropping message on the floor...");
	    	return null;
	     }
	     if(message == null || isEqual == false)
	     {
	       	System.out.println("ERROR Type not found for message " + message);
	       	return null;
	     }
		 m_szType = message[1].toString();
		 		 
		 return message;
	}
}

//EOF
