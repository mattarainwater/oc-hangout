﻿using System;
using System.ComponentModel;

public static class EnumExtensions
{
	public static string GetEnumDescription(this Enum value)
	{
		System.Reflection.FieldInfo fi = value.GetType().GetField(value.ToString());

		DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

		if (attributes != null && attributes.Length > 0)
			return attributes[0].Description;
		else
			return value.ToString();
	}
}