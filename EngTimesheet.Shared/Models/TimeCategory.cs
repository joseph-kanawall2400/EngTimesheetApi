﻿using System.ComponentModel;

namespace EngTimesheet.Shared.Models
{
	public enum TimeCategory
	{
		[Description("Maintenance")]
		Maintenance,
		[Description("Enhancement")]
		Enhancement,
		[Description("New Development")]
		NewDevelopment,
		[Description("Research And Development")]
		ResearchAndDevelopment,
		[Description("Management")]
		Management
	}
}
