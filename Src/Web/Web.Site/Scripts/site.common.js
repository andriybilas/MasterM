window.StringEmpty = "";
window.GuidEmpty = "00000000-0000-0000-0000-000000000000";
window.TrueStatus = "true";


function PostAjaxRequest( responceType, actionUrl, jsonData, successDelegate )
{
    $.ajax( {
        contentType: "application/json; charset=utf-8",
        dataType: responceType,
        type: "post",
        url: actionUrl,
        traditional: true,
        data: jsonData,
        processData: false,
        //ifModified: true,
        success: successDelegate,
        error: ExceptionHandler
    } );
}

function ExceptionHandler( data, textStatus )
{
    debugger;
   }
