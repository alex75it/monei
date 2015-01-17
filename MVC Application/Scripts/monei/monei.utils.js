monei.utils = (function () {

    var version = '0.1.1';

    return {

        askConfirm: function askConfirm(text, confirmCallback) {
            var modalId = "modal" + Math.random().toString(16).substring(2, 8)
            var modalHtml =
            '<div class="modal fade" > \
                    <div class="modal-body"><strong>'+ text + '</strong></div> \
                    <div class="modal-footer"> \
                        <button class="btn btn-success" id="btn_confirm">Yes, delete</button> \
                        <button class="btn btn-danger" id="btn_close">No, calcel</button> \
                    </div> \
                </div>';

            var $modal = $(modalHtml).attr('id', modalId);
            $modal.find('#btn_confirm').removeAttr('id').click(function () {
                $('#' + modalId).modal('hide');
                typeof confirmCallback === 'function' && confirmCallback();
            });
            $modal.find('#btn_close').removeAttr('id').click(function () {
                $('#' + modalId).modal('hide');
            });

            $(body).append($modal);
            var options = { backdrop: true, kayboard: true };
            $('#' + modalId).modal(options);
        }


    };
})();