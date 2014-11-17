/***********************************************************************************************************************
DOCUMENT: includes/javascript.js
DEVELOPED BY: Ryan Stemkoski
COMPANY: Zipline Interactive
EMAIL: ryan@gozipline.com
PHONE: 509-321-2849
DATE: 2/26/2009
DESCRIPTION: This is the JavaScript required to create the accordion style menu.  Requires jQuery library
************************************************************************************************************************/

$(document).ready(function() {
	
	/********************************************************************************************************************
	SIMPLE ACCORDIAN STYLE MENU FUNCTION
	********************************************************************************************************************/	
	$('div.accordionButton').click(function() {
		$('div.accordionContent').slideUp('normal').prev().removeClass('active-accord');	
		$(this).next().slideDown('normal');
		$(this).addClass('active-accord');
	});
	$('div.accordionButton').addClass('active-accord');
	/********************************************************************************************************************
	CLOSES ALL DIVS ON PAGE LOAD
	********************************************************************************************************************/	
	$("div.first").slideDown('normal');
	//$("div.accordionContent").hide();
});
