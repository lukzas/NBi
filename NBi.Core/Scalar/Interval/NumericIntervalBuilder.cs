﻿using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Scalar.Interval
{
	public class NumericIntervalBuilder
	{
		private readonly string value;
		private bool isBuild;
		protected NumericInterval interval;
		protected Exception ex;

		public NumericIntervalBuilder(string value)
		{
			this.value = value;
			isBuild = false;
		}

		public NumericIntervalBuilder(object value)
		{
			if (value is string)
				this.value = (string)value;
			else
				ex = new ArgumentException("This must be a string");
			
			isBuild = false;
		}

		public void Build()
		{
			if (ex == null)
			{ 
				var valueToBuild = value.Replace(" ", "").ToLower();

				if (valueToBuild.StartsWith("(") && valueToBuild.EndsWith(")"))
					interval = BuildGeneric(valueToBuild);
				else
					interval = BuildClassic(valueToBuild);
			}
			isBuild = true;
		}

		protected virtual NumericInterval BuildClassic(string value)
		{
			if (!(value.StartsWith("]") || value.StartsWith("[")))
				ex = new ArgumentException("The interval definition must start by '[' or ']'");
			if (!(value.EndsWith("]") || value.EndsWith("[")))
				ex = new ArgumentException("The interval definition must end by '[' or ']'");
			if (!(value.Contains(";")))
				ex = new ArgumentException("The interval definition must contain a delimitor ';'");

			var split = value.Split(';');
			if (split.Count() > 2)
			{
				ex = new ArgumentException("The interval definition must contain only one delimitor ';'");
			}

			if (ex != null)
				return null;

			EndPoint<double> left, right;
			if (split[0].Substring(1, split[0].Length - 1).ToLower() == "-inf")
				left = new LeftEndPointNegativeInfinity();
			else
				if (split[0].StartsWith("["))
					left = new LeftEndPointClosed<double>(
							Double.Parse(split[0].Substring(1, split[0].Length - 1), CultureInfo.InvariantCulture.NumberFormat));
				else
					left = new LeftEndPointOpen<double>(
							Double.Parse(split[0].Substring(1, split[0].Length - 1), CultureInfo.InvariantCulture.NumberFormat));

			if (split[1].Substring(0, split[1].Length - 1).ToLower() == "+inf"
				|| split[1].Substring(0, split[1].Length - 1).ToLower() == "inf")
				right = new RightEndPointPositiveInfinity();
			else
				if (split[1].EndsWith("]"))
					right = new RightEndPointClosed<double>(
							Double.Parse(split[1].Substring(0, split[1].Length - 1), CultureInfo.InvariantCulture.NumberFormat));
				else
					right = new RightEndPointOpen<double>(
							Double.Parse(split[1].Substring(0, split[1].Length - 1), CultureInfo.InvariantCulture.NumberFormat));

			return new NumericInterval(left, right);
		}

		protected virtual NumericInterval BuildGeneric(string value)
		{
			
			value = value.Substring(1, value.Length - 2);
			switch (value)
			{
				case "positive":
				case "0+":
					return new NumericInterval(new LeftEndPointClosed<double>(0), new RightEndPointPositiveInfinity());
				case "negative":
				case "-0":
					return new NumericInterval(new LeftEndPointNegativeInfinity(), new RightEndPointClosed<double>(0));
				case "absolutely-positive": 
				case "+": 
					return new NumericInterval(new LeftEndPointOpen<double>(0), new RightEndPointPositiveInfinity());
				case "absolutely-negative": 
				case "-": 
					return new NumericInterval(new LeftEndPointNegativeInfinity(), new RightEndPointOpen<double>(0));
			}

			if (double.TryParse(value.Substring(1, value.Length - 1), NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out double d))
			{
				switch (value.Substring(0,1))
				{
					case ">":
						return new NumericInterval(new LeftEndPointOpen<double>(d), new RightEndPointPositiveInfinity());
					case "<":
						return new NumericInterval(new LeftEndPointNegativeInfinity(), new RightEndPointOpen<double>(d));
				}
			}
			else if (double.TryParse(value.Substring(2, value.Length - 2), out d))
			{
				switch (value.Substring(0,2))
				{
					case ">=":
						return new NumericInterval(new LeftEndPointClosed<double>(d), new RightEndPointPositiveInfinity());
					case "<=":
						return new NumericInterval(new LeftEndPointNegativeInfinity(), new RightEndPointClosed<double>(d));
				}
			}

			ex = new ArgumentException(string.Format("Cannot interpret the interval {0}", value));
			return null;
		}

		public bool IsValid()
		{
			if (!isBuild)
				throw new InvalidOperationException("You must first apply the build method before a call to this method.");

			return interval != null;
		}

		public NumericInterval GetInterval()
		{
			if (!isBuild)
				throw new InvalidOperationException("You must first apply the build method before a call to this method.");

			return interval;
		}

		public Exception GetException()
		{
			if (!isBuild)
				throw new InvalidOperationException("You must first apply the build method before a call to this method.");

			return ex;
		}
	}
}
