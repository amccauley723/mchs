
$('html').click(function () {
    $('.top-level-nav > li.hasKids.active').removeClass('active');
});
$('.top-level-nav').click(function (event) {
    event.stopPropagation();
});
$('.top-level-nav > li.hasKids > a').click(function (event) {
    var $clickedLi = $(this);
    $clickedLi.parent().siblings().removeClass('active');
    if ($clickedLi.parent().hasClass('active')) {
        $clickedLi.parent().removeClass('active');
    } else {
        $clickedLi.parent().addClass('active');
        closeTypeAhead();
    }
});

$('button[data-cmw="false"]').on("click", function(){
    $("#cmw-no-good").hide();
    $("#cmw-good").show();
}); 
$("button[data-cmw='true']").on("click", function(){
    $("#cmw-no-good").show();
    $("#cmw-good").hide();
}); 
function checkMobileScreen(){
    return window.innerWidth <= 700;
}

$('.body-margin:first').css('margin-top', $('.all-the-header').height());

$(window).resize(function(){
    if(checkMobileScreen()){
        $('body').addClass('mobile');
    }else{
        $('body').removeClass('mobile');
        $('.body-margin:first').css('margin-top', $('.all-the-header').height());
    }
});
    

                