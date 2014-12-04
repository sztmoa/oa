//创建xmlhttp对象
function createxmlhttp()
{
	var xmlhttp=false;
	try	
	{
  		xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
 	} 
	catch (e) 
	{
  		try 
  		{
   			xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
  		} 
		catch (e) 
		{
   			xmlhttp = false;
 		}
 	}
	if (!xmlhttp && typeof XMLHttpRequest!='undefined') 
	{
  		xmlhttp = new XMLHttpRequest();
		if (xmlhttp.overrideMimeType)
		{
			//设置MiME类别 
			xmlhttp.overrideMimeType('text/xml');
		}
	}
	return xmlhttp;	
}
