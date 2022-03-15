// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function getId(id){
    $("#video").attr("src", "https://www.youtube.com/embed/"+id+"?autoplay=0&rel=0");
    $("#chat").attr("src", "https://www.youtube.com/live_chat?v="+id+"&embed_domain=localhost");
    $( "#liveDiv" ).load(window.location.href + " #liveDiv" );
    $( "#upcomingDiv" ).load(window.location.href + " #upcomingDiv" );
    
}




