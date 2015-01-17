
function setPeriod(e) {
    fromDate = null;
    toDate = null;

    period = $(e).attr('data-period');
    $('#selectedPeriod').val(period);
    //monei.debug("set:" + period);
    

    $("#periodButtonsContainer .btn").removeClass("btn-info");
    $(e).addClass("btn-info");
    //return;
    
    switch (period)
    {
        case 'current month':
            fromDate = moment().startOf('month');
            toDate = moment();
            break;
        case 'previous month':
            fromDate = moment().add('month', -1).startOf('month');
            toDate = moment(fromDate).endOf('month');
            break;
        case 'current year':
            fromDate = moment().startOf('year');
            toDate = moment().endOf('year');
            break;
        case 'previous year':
            fromDate = moment().add('year', -1).startOf('year');// .month(0).date(1);
            toDate = moment(fromDate).endOf('year'); // .month(11).endOf('month');
            break;
        case 'last 30 days':
            fromDate = moment().add('day', -30);
            toDate = moment();
            break;
        case 'last 7 days':
            fromDate = moment().add('day', -7);
            toDate = moment();
            break;
        case 'yesterday':
            fromDate = moment().add('day', -1);
            toDate = fromDate;
            break;
        case 'last 6 months':
            fromDate = moment().add('month', -6);
            toDate = moment();
            break;
        case 'last year':
            fromDate = moment().add('year', -1);
            toDate = moment();
            break;
    }

    $("#StartDate").val(fromDate.format('l'));  // L is for zero padded date
    $("#EndDate").val(toDate.format('l'));
    $("#searchButton").click();
}             



function getId(el) {
    var id = $(el).parents("tr").attr("data-id");
    return id;
}

function executeSearch()
{
    $('#searchButton').click();
}

function deleteItem(itemId) {
    monei.utils.askConfirm('Delete the record?', function () {
        $('#deleteInput').val(itemId).parents('form').submit();
    });
}

function saveItem()
{
    var $form = $('#modal_edit').find('form');
    var url = "/Registry/Edit";
    var postData = $form.serialize();

    // https://www.simple-talk.com/dotnet/asp.net/modal-input-forms-in-asp.net-mvc/
    $.post(url, postData, function (data) {
        if (data == 'ok') {
            cancelEdit();
            executeSearch();
            noty({ text: "Update success", type: "success", layout: "topCenter", timeout: 2 * 1000 });
        }
        else {
            noty({ text: "Fail to update record:" + data, layout: "topCenter", type: "error", /*timeout: 2 * 1000,*/ modal: true });
        }
    });        
    
}

function cancelEdit()
{
    $('#modal_edit').modal('hide');
}

function editItem(itemId) {
    var id = itemId; // getId(el);
    var url = "/Registry/Edit/" + id;

    // crete a modal dialog and fill it with the edit page
    $('#modal_edit').remove();
    var $popup = $('<div id="modal_edit" class="modal hide fade" style="width:80%; min-width:1024px; margin-left:-40%">');
        
    $popup.append('<div class="">');
    $popup.append(' \
        <div class="modal-header"> \
        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button> \
        <h3>Edit voice in Registry</h3> \
        </div> \
    ');

    var $body = $('<div class="modal-body"><img src="/images/ajax-loader.gif" /> loading...</div>');  // toldo: l10n
    $popup.append($body);

    $popup.append(' \
        <div class="modal-footer"> \
        <input type="button" onclick="saveItem();" value="Save" class="btn btn-primary" /> \
        <input type="button" onclick="cancelEdit();" value="Close" class="btn btn-danger" /> \
        </div> \
    ');

   
    $body.load(url, null, function (responseText, statusText, xhr) {
        if (xhr.status != 200)
        {
            noty({ text: "Fail to show edit form", layout: "topCenter", type: "error", /*timeout: 2 * 1000,*/ modal: true });
            cancelEdit();
            return;
        }

        data_loaded();
    })    

    $('body').append($popup);
    $popup.modal('show');
}