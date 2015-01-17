(function init($, window, undefined) {

    // these are the global settings, used application wide
    var globalSettings = {
        animate: true,
        validatePrompt: false,
        cancelButton: {
            text: 'Cancel',
            cssClass: 'btn-danger'
        },
        confirmButton: {
            text: 'Yes',
            cssClass: 'btn-primary'
        },
        customButtons: {
            triggerCallback: false
        }
    }

    // references
    var $body = $('body');

    // get modal structure. this aloow us to keep it completelly isolated in case we build multiple modals.
    var template = function() {

        // modal parts

        this.modal = $('<div class="modal hide fade" tabindex="-1" role="dialog">');
        this.header = $('<div class="modal-header"></div>');
        this.title = $('<h3 id="modal-title"></h3>');
        this.close = $('<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>');
        this.content = $('<div class="modal-body"></div>');
        this.footer = $('<div class="modal-footer"></div>');
        this.btnok = $('<button class="btmodal-confirm btn">Ok</button>');
        this.btncancel = $('<button class="btmodal-cancel btn" data-dismiss="modal">Cancel</button>');

        // create prompt parts
        this.input = $('<input type="text" autocomplete="off" class="prompt-input input-block-level"/>', {});
        this.cgroup = $('<div class="control-group" />');
        this.controls = $('<div class="controls" />');

        // build modal
        this.init = function () {
            this.btncancel.appendTo(this.footer);
            this.btnok.appendTo(this.footer);
            this.close.appendTo(this.header);
            this.title.appendTo(this.header);
            this.header.appendTo(this.modal);
            this.content.appendTo(this.modal);
            this.footer.appendTo(this.modal);
        }

    };


    /*
     Private API
    ------------------------------------------*/

    // set title or hide header if there is no title
    function setTitle(modalTemplate, title) {
        if (title != null && title != '') {
            modalTemplate.title.html(title);
        } else {
            modalTemplate.header.hide();
        }
    }

    // configure default buttons (cancel and confirm)
    function setDefaultButtons(modalTemplate, confirmButton, cancelButton, callback, isPromptRequired) {
       
        // set confirm button text and visibility
        modalTemplate.btnok.html(confirmButton.text);
        confirmButton.visible == true ? modalTemplate.btnok.show() : modalTemplate.btnok.hide();

        // set confirm button class
        modalTemplate.btnok.addClass(confirmButton.cssClass ? confirmButton.cssClass : globalSettings.confirmButton.cssClass);

        // set cancel button text and visibility
        modalTemplate.btncancel.html(cancelButton.text);
        cancelButton.visible == true ? modalTemplate.btncancel.show() : modalTemplate.btncancel.hide();

        // set cancel button class
        modalTemplate.btncancel.addClass(cancelButton.cssClass ? cancelButton.cssClass : globalSettings.cancelButton.cssClass);

        // set click events
        modalTemplate.btnok.click(function () { onButtonClick(modalTemplate, true, callback, isPromptRequired) });
        modalTemplate.btncancel.click(function () { onButtonClick(modalTemplate, false, callback, isPromptRequired) });

    }

    // set animation
    function setAnimation(modalTemplate, animate) {
        if (animate) {
            modalTemplate.modal.addClass('fade');
        } else {
            modalTemplate.modal.removeClass('fade');
        }
    }

    // append a text box to the modal body to create a prompt dialog.
    // add an extra space if body already has any content.
    function setPrompt(modalTemplate, options) {

        if (!options.enabled) {
            return false;
        }

        if (!$.isEmptyObject(modalTemplate.content.html())) {
            modalTemplate.input.css('margin-top', '15px');
        }
        modalTemplate.input.appendTo(modalTemplate.controls);
        modalTemplate.controls.appendTo(modalTemplate.cgroup);
        modalTemplate.cgroup.appendTo(modalTemplate.content);

    }

    // handles internal buttons (cancel and confirm)
    // triggering callbacks when necessary
    function onButtonClick(modalTemplate, isConfirmed, callback, isPromptRequired, customValue) {

        if (isConfirmed && isPromptRequired && $.isEmptyObject(modalTemplate.input.val())) {
            modalTemplate.cgroup.addClass('error')
            return false;
        }
       
        // parameters to be send to the callback function if any
        var e = {
            confirmed: isConfirmed,
            prompt: isConfirmed ? modalTemplate.input.val() : null,
            customValue: customValue,
            close: true
        }
        
        // execute callback and wait for it to return
        var retValue = $.isFunction(callback) && callback(e) === false;

        // close modal unless callback set close = false
        if (e.close) {
            modalTemplate.modal.modal('hide');
        }

    }

    // our public object, augmented after latter
    var dialog = {};

    /*
     Public API
    ------------------------------------------*/

    // update global settings (application whide defaults)
    dialog.setDefaults = function (options) {
        $.extend(true, globalSettings, options);
    }

    dialog.show = function (options) {

        // default settings
        var defaults = {
            title: null,
            content: 'Are you sure you want to continue?',
            contentUrl: null,
            callback: null,
            animate: globalSettings.animate,
            confirmButton: {
                text: globalSettings.confirmButton.text,
                cssClass: globalSettings.confirmButton.cssClass,
                visible: true
            },
            cancelButton: {
                text: globalSettings.cancelButton.text,
                cssClass: globalSettings.cancelButton.cssClass,
                visible: true
            },
            buttons: null,
            prompt: {
                enabled: false,
                required: globalSettings.validatePrompt
            }
        };

        var settings = $.extend(true, {}, defaults, options);

        // create new instance of modal template and initialize it
        var mdiag = new template();
        mdiag.init();

        // wether or not the modal should be animated
        setAnimation(mdiag, settings.animate);

        // set title and content
        setTitle(mdiag, settings.title);

        //Set content
        mdiag.modal.modal({ remote: settings.contentUrl });
        mdiag.content.html(settings.content);

        // default buttons
        setDefaultButtons(mdiag, settings.confirmButton, settings.cancelButton, settings.callback, settings.prompt.required);

        // set promt
        setPrompt(mdiag, settings.prompt);

        // set additional buttons
        if (settings.buttons) {
            $.each(settings.buttons, function (i, item) {

                var triggercb = item.triggerCallback !== undefined ? item.triggerCallback : globalSettings.customButtons.triggerCallback;

                var $nbt = $('<button class="btn"></button>').addClass(item.cssClass).html(item.text);
                $.isFunction(item.click) && $nbt.click(item.click);

                if (settings.callback && triggercb) {
                    $nbt.click(function () {
                        onButtonClick(false, settings.callback, settings.prompt.required, item.value);
                    });
                }

                $nbt.prependTo(mdiag.footer);

            });
        }

        // set the modal 'closed' event handler. make sure we remove it after we hide it
        mdiag.modal.on('hidden', function () {
            // remove modal
            return mdiag.modal.remove();
        });

        // append modal to page's body        
        mdiag.modal.appendTo($body).modal('show');

    }

    dialog.alert = function (content, options) {

        var defaults = {
            title: null,
            buttonText: 'Ok',
            callback: null,
            contentUrl: null,
            extraButtons: null
        }

        var values = $.extend({}, defaults, options);

        dialog.show({
            title: values.title,
            contentUrl: values.contentUrl,
            content: content,
            confirmButton: {
                text: values.buttonText
            },
            cancelButton: {
                visible: false
            },
            callback: values.callback,
            buttons: values.extraButtons
        })
    }

    dialog.confirm = function (content, options) {

        var defaults = {
            title: null,
            contentUrl: null,
            cancelButton: {
                text: 'Cancel',
                cssClass: 'btn-primary'
            },
            confirmButton: {
                text: 'Yes',
                cssClass: 'btn-danger'
            },
            callback: null,
            extraButtons: null
        }

        var values = $.extend({}, defaults, options);

        eFlexis.dialog.show({
            title: values.title,
            contentUrl: values.contentUrl,
            content: content,
            confirmButton: {
                text: values.confirmButton.text,
                cssClass: values.confirmButton.cssClass
            },
            cancelButton: {
                text: values.cancelButton.text,
                cssClass: values.cancelButton.cssClass
            },
            callback: values.callback,
            buttons: values.extraButtons
        })
    }

    dialog.prompt = function (options) {

        var defaults = {
            title: null,
            content: null,
            contentUrl: null,
            required: false,
            cancelButton: {
                text: 'Cancel',
                cssClass: null
            },
            confirmButton: {
                text: 'Confirm',
                cssClass: 'btn-primary'
            },
            callback: null,
            extraButtons: null
        }

        var values = $.extend({}, defaults, options);

        eFlexis.dialog.show({
            title: values.title,
            contentUrl: values.contentUrl,
            content: values.content,
            confirmButton: {
                text: values.confirmButton.text,
                cssClass: values.confirmButton.cssClass
            },
            cancelButton: {
                text: values.cancelButton.text,
                cssClass: values.cancelButton.cssClass
            },
            callback: values.callback,
            buttons: values.extraButtons,
            prompt: {
                enabled: true,
                required: values.required
            }
        })
    }


    /*
     add public object to namespace 
    ------------------------------------------*/
    window.eFlexis = window.eFlexis || {};
    window.eFlexis.dialog = dialog;

    // initialize public object
    dialog.init = function (_window, _$) {
        window.eFlexis.dialog = init(_window || window, _$ || $);
    };

    return dialog;

})(jQuery, window)


