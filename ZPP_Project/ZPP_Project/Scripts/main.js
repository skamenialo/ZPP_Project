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

function AddIdToGet(form, id) {
    var action = $(form).attr('action').split('/');
    $(form).attr('action', '/' + action[1] + '/' + action[2] + '/' + id).submit();
}

function ClearGet(form, action) {
    $(form).attr('action', '/' + $(form).attr('action').split('/')[1] + '/' + action).submit();
}