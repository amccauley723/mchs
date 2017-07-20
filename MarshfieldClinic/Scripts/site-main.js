
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
    
    
    // Search and Type Ahead
    $(document).on('click', '.header-test-button', function(event){
        event.preventDefault();
        window.location = '/Search/?k=' + $('#newSearchInput').val();
    });
    $(document).on('keyup', '#newSearchInput', function(event){
        event.preventDefault();
        if(event.keyCode === '13'){
            window.location = '/Search/?k=' + $('#newSearchInput').val();
        }
        if($(this).val() !== '' && $(this).val() !== 'undefined' && $(this).val() !== null){
            getServices($(this).val());
            getProviders($(this).val());
            getLocations($(this).val());
            getBlogPosts($(this).val());
            $("#typeAheadResults").show();
        }else{
             closeTypeAhead();
        }
        
    });
    $(document).on('blur', '#newSearchInput', function(event){
        // $("#typeAheadResults").hide();
    });
    $(document).on('focus', '#newSearchInput', function(event){
        // $("#typeAheadResults").html("")
        $(this).val("");
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
    
function getProviders(searchTerms){
        $.getJSON('/umbraco/Surface/Search/LookFor?d=' + "provider" + '&t=' + searchTerms)
            .done(function(data){
                if(data.length > 0){
                    html = '<ul>';
                    for(var i = 0; i < data.length; i++){
                
                         html += '<li><a href="' + data[i].NiceUrl + '">' + data[i].Name  + '</a></li>'; 
                     
                    }
                    html += '</ul><div style="clear: both;"></div>';
                    $("#typeAheadProviders > .menu-items").html(html);
                   
                }else{
                     $("#typeAheadProviders > .menu-items").html("No results for this topic.");
                }
            });
}
function getServices(searchTerms){
        $.getJSON('/umbraco/Surface/Search/LookFor?d=' + "providerType" + '&t=' + searchTerms)
            .done(function(data){
                if(data.length > 0){
                    html = '<ul>';
                    for(var i = 0; i < data.length; i++){
                        html += '<li><a href="' + data[i].NiceUrl + '">' + data[i].Name  + '</a></li>';
                    }
                    html += '</ul><div style="clear: both;"></div>';
                    $("#typeAheadServices > .menu-items").html(html);
                }else{
                     $("#typeAheadServices > .menu-items").html("No results for this topic.");
                }
            });
}
    function getLocations(searchTerms){
        $.getJSON('/umbraco/Surface/Search/LookFor?d=locations&t=' + searchTerms)
            .done(function(data){
                if(data.length > 0){
                    html = '<ul>';
                    for(var i = 0; i < data.length; i++){
                        html += '<li><a href="' + data[i].NiceUrl + '">' + data[i].Name  + '</a></li>';
                    }
                    html += '</ul><div style="clear: both;"></div>';
                    $("#typeAheadLocations > .menu-items").html(html);
                }else{
                    $("#typeAheadLocations > .menu-items").html("No results for this topic.");
                }
            });
    }
    function getBlogPosts(searchTerms){
        $.ajax({
            url: 'http://shine365.marshfieldclinic.org/wp-json/wp/v2/posts',
            data: {
                filter: {
                'per_page': 3,
                'search': searchTerms
                }
            },
            dataType: 'json',
            type: 'GET',
            success: function(data) {
                console.log(data);
                if(data.length > 0){
                    html = '<ul>';
                    for(var i = 0; i < 3; i++){
                        html += '<li><a href="' + data[i].link + '">' + data[i].title.rendered  + '</a></li>';
                    }
                    html += '</ul><div style="clear: both;"></div>';
                    $("#typeAheadBlog > .menu-items").html(html);
                }else{
                     $("#typeAheadBlog > .menu-items").html("No results for this topic.");
                }
            },
            error: function() {
                // error code
            }
        });

        
    }
    function closeTypeAhead(){
        $("#typeAheadServices > .menu-items").html("");
        $("#typeAheadLocations > .menu-items").html("");
        $("#typeAheadBlog > .menu-items").html("");
        $("#typeAheadProviders > .menu-items").html("");
        $("#typeAheadResults").hide();
    }
    

                