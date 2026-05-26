using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace AGInterfaces.ReportDynamicData;

public static class DataTypeBuilder
{
	public static Type CompileResultType(Type baseType, string className, Dictionary<string, Type> propertiesList)
	{
		TypeBuilder typeBuilder = GetTypeBuilder(baseType, className);
		foreach (KeyValuePair<string, Type> properties in propertiesList)
		{
			CreateProperty(typeBuilder, properties.Key, properties.Value);
		}
		return typeBuilder.CreateTypeInfo();
	}

	private static TypeBuilder GetTypeBuilder(Type baseType, string className)
	{
		return AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(className), AssemblyBuilderAccess.Run).DefineDynamicModule("AutoGRAPHReports").DefineType(className, TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.BeforeFieldInit, baseType);
	}

	private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
	{
		FieldBuilder field = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);
		MethodBuilder methodBuilder = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName, propertyType, Type.EmptyTypes);
		ILGenerator iLGenerator = methodBuilder.GetILGenerator();
		iLGenerator.Emit(OpCodes.Ldarg_0);
		iLGenerator.Emit(OpCodes.Ldfld, field);
		iLGenerator.Emit(OpCodes.Ret);
		MethodBuilder methodBuilder2 = tb.DefineMethod("set_" + propertyName, MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName, null, new Type[1] { propertyType });
		ILGenerator iLGenerator2 = methodBuilder2.GetILGenerator();
		Label loc = iLGenerator2.DefineLabel();
		Label loc2 = iLGenerator2.DefineLabel();
		iLGenerator2.MarkLabel(loc);
		iLGenerator2.Emit(OpCodes.Ldarg_0);
		iLGenerator2.Emit(OpCodes.Ldarg_1);
		iLGenerator2.Emit(OpCodes.Stfld, field);
		iLGenerator2.Emit(OpCodes.Nop);
		iLGenerator2.MarkLabel(loc2);
		iLGenerator2.Emit(OpCodes.Ret);
		PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
		propertyBuilder.SetGetMethod(methodBuilder);
		propertyBuilder.SetSetMethod(methodBuilder2);
	}
}
