﻿namespace System.Collections.Generic {
	public class CircleFragment <TElement, TLimit>
		where TLimit : IComparable <TLimit>
	{
		public TElement Element { get; set; }
		public Range <TLimit> Range { get; private set; }

		public CircleFragment ( TElement element, Range <TLimit> range ) {
			this.Element = element;
			this.Range = range;
		}

		public override string ToString () {
			return	$"(Element: {Element}, Range: {Range})";
		}
	}
}
