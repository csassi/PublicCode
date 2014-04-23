///////////////////////////////////////////////////////////
//@file: CAEMessageManager.java
//@author: Christian Sassi - Sass
//@purpose: This is the CAEMessageManager module.
//@Generated: Jan 25, 2014 : 12:03:03 AM
///////////////////////////////////////////////////////////
package com.EFB.Messaging;

import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.HashMap;
import java.util.Map;
import java.util.Vector;

/**
 * The Class CAEMessageManager.
 *
 * @author Sass
 */
public class CAEMessageManager
{
	
	/**
	 * The Class CObjectMessage.
	 */
	class CObjectMessage
	{
		
		/**
		 * Instantiates a new c object message.
		 *
		 * @param nuMessage a new AndE message.
		 * @param nuObject - Object listening to the message.
		 */
		public CObjectMessage(CAEMessage nuMessage, IMessageHook nuObject)
		{
			message = nuMessage;
			object = nuObject;
		}
		
		/** The message. */
		CAEMessage message;
		
		/** The object. */
		IMessageHook object;
	};
	
	/** The map of registered objects.KEY is the Message Type, VALUE is the objects registered for a KEY message. */
	Map<String, Vector<IMessageHook>> m_RegisteredObjects;
	
	/** The m_ messages. */
	Vector<CAEMessage> m_Messages;
	
	/**
	 * Register.
	 *
	 * @param szType - The type of the message the object to listening to.
	 * @param objToRegister - The object you want to have the message sent to. 
	 */
	public void Register(String szType, IMessageHook objToRegister)
	{
		//No specified type in the manager, make it so!
		if(m_RegisteredObjects.get(szType) == null)
		{
			Vector<IMessageHook> nuVector = new Vector<IMessageHook>();
			nuVector.add(objToRegister);
			m_RegisteredObjects.put(szType, nuVector);
		}
		else //It's there!
		{
			m_RegisteredObjects.get(szType).add(objToRegister);
		}
		
	}
	
	/**
	 * Sends a message.
	 *
	 * @param message - The message to send.
	 */
	public void Send(CAEMessage message)
	{
		if(m_RegisteredObjects.get(message.GetType()) == null)
		{
			System.out.println("ERROR -- No object registered for message type " + message.GetType());
		}
		else
		{
			m_Messages.add(message);
		}
	}
	
	
	/**
	 * Dispatch messages. Call this once per frame. Before you update.
	 */
	public void DispatchMessages() 
	{
		for (int i = 0; i < m_Messages.size(); i++)
		{
			String szType = m_Messages.get(i).m_szType;
			
			Vector<IMessageHook> objectVector = m_RegisteredObjects.get(szType);
			
			for (int j = 0; j < objectVector.size(); j++)
			{
				IMessageHook object = (IMessageHook)objectVector.get(j);
				Class<?>[] params = {m_Messages.get(i).getClass()};
			
				try
				{
					Class<?> c = Class.forName(object.getClass().getName());
					Method  method = c.getDeclaredMethod("handle_" + szType, params);
					try
					{
						method.invoke(object,m_Messages.get(i));
					} 
					catch (IllegalArgumentException e)
					{
						System.out.println(e);
					} 
					catch (IllegalAccessException e)
					{
						System.out.println(e);
					} 
					catch (InvocationTargetException e)
					{
						System.out.println(e + "-- " + e.getMessage());
					}
				}
				catch(NoSuchMethodException ioe)
				{
					System.out.println(ioe);
				} 
				catch (ClassNotFoundException e)
				{
					e.printStackTrace();
				} 			
			}
			
			m_Messages.remove(i);
		}
	}
	
	/**
	 * Initializes the manager.
	 */
	public void Initialize()
	{
		m_Messages = new Vector<CAEMessage>(); 
		m_RegisteredObjects = new HashMap<String, Vector<IMessageHook>>();   
	}
}

//EOF
