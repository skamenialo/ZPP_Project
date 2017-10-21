function ToggleVisibilityState(regionName)
{
	var button = $("#" + regionName + "_toggle");
	var region = $("#" + regionName);
	if (region.is(":hidden"))
	{
	    button.className = "fa fa-angle-right";
	}
	else
	{
	    button.className = "fa fa-angle-down";
	}
	region.slideToggle( 200 )
}