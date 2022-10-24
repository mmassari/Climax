﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Climax.UnitTest
{
	public static class AssemblyUtilities
	{
      public static PropertyInfo GetPropertyInfo(this MethodBase method)=>		
         method.DeclaringType.GetProperty(method.Name.Substring("get_".Length));
      
      /// <summary>
      /// Use as first line in ad hoc tests (needed by XNA specifically)
      /// </summary>
      public static void SetEntryAssembly()
      {
         SetEntryAssembly(Assembly.GetCallingAssembly());
      }

      /// <summary>
      /// Allows setting the Entry Assembly when needed. 
      /// Use AssemblyUtilities.SetEntryAssembly() as first line in XNA ad hoc tests
      /// </summary>
      /// <param name="assembly">Assembly to set as entry assembly</param>
      public static void SetEntryAssembly(Assembly assembly)
      {
         AppDomainManager manager = new AppDomainManager();
         FieldInfo entryAssemblyfield = manager.GetType().GetField("m_entryAssembly", BindingFlags.Instance | BindingFlags.NonPublic);
         entryAssemblyfield.SetValue(manager, assembly);

         AppDomain domain = AppDomain.CurrentDomain;
         FieldInfo domainManagerField = domain.GetType().GetField("_domainManager", BindingFlags.Instance | BindingFlags.NonPublic);
         domainManagerField.SetValue(domain, manager);
      }
   }
}
