
    $("a[href^='#']").off();
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
        }
    });
    $(document).on('click', '.header-test-button', function(event){
        event.preventDefault();
        window.location = '/Search/?k=' + $('#newSearchInput').val();
    });
    $(document).on('keyup', '#newSearchInput', function(event){
        event.preventDefault();
        if(event.keyCode === '13'){
            window.location = '/Search/?k=' + $('#newSearchInput').val();
        }
    });
    $(document).on('click', '.doc-test-button', function(event){
        event.preventDefault();
        window.location = '/doctors/search/?k=' + $('#docSearchInput').val();
    });
    $(document).on('keyup', '#docSearchInput', function(event){
        event.preventDefault();
        if(event.keyCode === '13'){
            window.location = '/doctors/search/?k=' + $('#docSearchInput').val();
        }
    });

                