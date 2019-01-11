using System;

namespace LibBase.Struct {
	public struct Range {
		public DateTime Start { get; private set; }
		public DateTime End => Start.AddDays(6);
		public Range(DateTime start) : this() {
			Start = start;
		}
	}
}
