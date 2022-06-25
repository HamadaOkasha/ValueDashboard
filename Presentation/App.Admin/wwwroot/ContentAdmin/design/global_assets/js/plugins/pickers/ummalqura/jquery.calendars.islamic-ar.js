/* http://keith-wood.name/calendars.html
   Arabic localisation for Islamic calendar for jQuery v2.0.2.
   Written by Keith Wood (wood.keith{at}optusnet.com.au) August 2009. */
(function($) {
	$.calendars.calendars.islamic.prototype.regionalOptions['ar'] = {
		name: 'Islamic',
		epochs: ['BAM', 'AM'],
		monthNames: ['محرّم', 'صفر', 'ربيع الأول', 'ربيع الآخر أو ربيع الثاني', 'جمادى الاول', 'جمادى الآخر أو جمادى الثاني',
		'رجب', 'شعبان', 'رمضان', 'شوّال', 'ذو القعدة', 'ذو الحجة'],
		monthNamesShort: ['محرّم', 'صفر', 'ربيع الأول', 'ربيع الآخر أو ربيع الثاني', 'جمادى الاول', 'جمادى الآخر أو جمادى الثاني',
		'رجب', 'شعبان', 'رمضان', 'شوّال', 'ذو القعدة', 'ذو الحجة'],
		dayNames: ['أحد', ' أثنين', 'ثلاثاء', 'أربع', 'خميس', 'جمعة', 'سبت'],
		dayNamesShort: ['أحد', ' أثنين', 'ثلاثاء', 'أربع', 'خميس', 'جمعة', 'سبت'],
		dayNamesMin: ['أحد', ' أثنين', 'ثلاثاء', 'أربع', 'خميس', 'جمعة', 'سبت'],
		digits: $.calendars.substituteDigits(['٠', '١', '٢', '٣', '٤', '٥', '٦', '٧', '٨', '٩']),
		dateFormat: 'yyyy/mm/dd',
		firstDay: 6,
		isRTL: true
	};
})(jQuery);