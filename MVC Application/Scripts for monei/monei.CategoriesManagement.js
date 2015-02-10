_version = '0.0.1';

var categories;
$(function () {  

    $.ajax({
        url: '/api/action/category/tree',
        type: 'GET',
        dataType: 'json',            
        success: function (data, textStatus, jqXhr) {
            //alert(data);
            categories = data; // = { categories: [] };
            //data.categories.push(data
            createTree();
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });       

});

function createTree() {
    //debug($(data.categories));
    var $ulCat = $('<ul class="categories">');
    $(categories).each(function (i, e) {
        var $liCat = createCategoryNode(e, $ulCat);

        var $ulSub = $('<ul class="subcategories">');
        $liCat.append($ulSub);
        $(e.subcategories).each(function (i, e) {
            var $liSub = $('<li class="subcategory">').attr('data-id', e.id).text(e.name);
            $ulSub.append($liSub);
            $liSub.draggable({ containment:"#tree", zIndex:10000, /*snap:".category",*/ /*cursor: "move", */ revert: true, /*opacity: 0.7*/ });
        });
    });

    $('#tree').empty().append($ulCat);
}

function createCategoryNode(e, $ulCat)
{
    var $liCat = $('<li class="category">').attr('data-id', e.id).text(e.name);
    $ulCat.append($liCat);
    $liCat.droppable({
        accept: '.subcategory', hoverClass:'hovered', greedy: true,
        drop: dropSubcategory
    });
    return $liCat;
}

function dropSubcategory(event, ui)
{
    //$(body).css("cursor", "auto");

    var subcategoryId = ui.draggable.attr("data-id");
    var fromCategoryId = getCategoryOfSubcategory(subcategoryId);
    var toCategoryId = $(this).attr("data-id");            

    if (fromCategoryId == toCategoryId)
        return;

    changeCategory(subcategoryId, toCategoryId,
        function () {
            var subcategory;
            // remove from old category
            var found = false;
            categories.forEach(function (e, i) {                        
                if (!found && e.subcategories && e.subcategories.length > 0) {
                    //alert(e.name);
                	var subcategoryFilter = e.subcategories.filter(function (item) { return item.id == subcategoryId; });
                    if (subcategoryFilter.length > 0) {
                        subcategory = subcategoryFilter[0];
                        found = true;
                        var index = e.subcategories.indexOf(subcategory);
                        e.subcategories.splice(index, 1); // remove one element starting from index
                    }
                }
            });
                       
            // add to new category
            var category = categories.filter(function (item) { return item.id == toCategoryId; })[0];
            category.subcategories.push(subcategory);

            createTree();
        },
        function (error) {
            alert("Fail: " + error);
        });
}

function getCategoryOfSubcategory(subcategoryId)
{
    var found = false;
    var categoryId;
    categories.forEach(function (e, i) {
        if (e.subcategories && e.subcategories.length > 0) {
            var subcategoryFilter = e.subcategories.filter(function (item) { return item.id == subcategoryId });
            if (subcategoryFilter.length > 0) {
                found = true;
                categoryId = e.id;
                return e.id;                        
            }
        }
    });
    return categoryId;
}

function changeCategory(subcategoryId, categoryId, successCallback, failCallback)
{
    $.ajax({
        url: '/api/action/Category/MoveSubcategory',
        type: 'POST',
        data: { subcategoryId: subcategoryId, categoryId: categoryId },
        dataType: 'json',
        success: function (data) {
            //alert("success: " + data);
            if (String(data) == 'OK')
                successCallback();
            else
                typeof failCallback == 'function' && failCallback( String(data));
        },
        error: function (x, y, z) { typeof failCallback == 'function' && failCallback(x) }
    });    
}
