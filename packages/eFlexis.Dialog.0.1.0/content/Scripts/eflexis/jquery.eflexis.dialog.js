(function ($, window, undefined) {

    $.fn.dialog = function (options) {

        var $this = $(this);

        // read values from data attributes. These will take priority over 'options' parameter
        var dataApi = {
            title: $this.data('title'),
            content: $this.data('content'),
            contentUrl: $this.data('href'),
            callback: $this.data('callback'),
            animate: $this.data('animate'),
            confirmButton: $this.data('confirm-button'),
            cancelButton: $this.data('cancel-button'),
            buttons: $this.data('extra-buttons'),
            prompt: $this.data('prompt')
        };

        // extend the options from defaults with user's options
        var settings = $.extend(true, {}, options, dataApi);

        return this.each(function () {

            var $this = $(this);

            if ($this.is('form')) {
                $this.on('submit', function (e) {
                    e.preventDefault();
                    eFlexis.dialog.show(settings);
                });
            } else {
                $this.on('click', function (e) {
                    e.preventDefault();
                    eFlexis.dialog.show(settings);
                });
            }

        });

    };

})(jQuery, window);

(function ($, window, undefined) {

    $.fn.alert = function (content, optionalParams) {

        var $this = $(this);

        // read values from data attributes. These will take priority over 'options' parameter
        var dataApi = {
            title: $this.data('title'),
            buttonText: $this.data('button-text'),
            callback: $this.data('callback'),
            contentUrl: $this.data('content-url'),
            extraButtons: $this.data('extra-buttons')
        };

        // extend the options from defaults with user's options
        var settings = $.extend(true, {}, optionalParams, dataApi);

        var ct = $this.data('content') || content;

        return this.each(function () {

            var $this = $(this);

            if ($this.is('form')) {
                $this.on('submit', function (e) {
                    e.preventDefault();
                    eFlexis.dialog.alert(ct, settings);
                });
            } else {
                $this.on('click', function (e) {
                    e.preventDefault();
                    eFlexis.dialog.alert(ct, settings);
                });
            }

        });

    };

})(jQuery, window);

(function ($, window, undefined) {

    $.fn.confirm = function (content, optionalParams) {

        var $this = $(this);

        // read values from data attributes. These will take priority over 'options' parameter
        var dataApi = {
            title: $this.data('title'),
            contentUrl: $this.data('content-url'),
            cancelButton: $this.data('cancel-button'),
            confirmButton: $this.data('confirm-button'),
            callback: $this.data('callback'),
            extraButtons: $this.data('extra-buttons')
        };

        // extend the options from defaults with user's options
        var settings = $.extend(true, {}, optionalParams, dataApi);

        var ct = $this.data('content') || content;

        return this.each(function () {

            var $this = $(this);

            if ($this.is('form')) {
                $this.on('submit', function (e) {

                    e.preventDefault();

                    // if there is no callback, let's add one to submit the form on confirm button click.                    
                    if (!settings.callback) {
                        settings.callback = function (args) {
                            if (args.confirmed) {
                                $this.unbind().submit();
                            }
                        }
                    }

                    eFlexis.dialog.confirm(ct, settings);

                });
            } else {
                $this.on('click', function (e) {
                    e.preventDefault();
                    eFlexis.dialog.confirm(ct, settings);
                });
            }

        });

    };

})(jQuery, window);

(function ($, window, undefined) {

    $.fn.prompt = function (options) {

        var $this = $(this);

        // read values from data attributes. These will take priority over 'options' parameter
        var dataApi = {
            title: $this.data('title'),
            content: $this.data('content'),
            contentUrl: $this.data('content'),
            required: $this.data('required'),
            cancelButton: $this.data('cancel-button'),
            confirmButton: $this.data('confirm-button'),
            callback: $this.data('callback'),
            extraButtons: $this.data('extra-buttons')
        };

        if (dataApi.callback) {
            dataApi.callback = window[dataApi.callback];
        }
        
        // extend the options from defaults with user's options
        var settings = $.extend(true, {}, options, dataApi);

        return this.each(function () {

            var $this = $(this);

            if ($this.is('form')) {
                $this.on('submit', function (e) {
                    e.preventDefault();
                    eFlexis.dialog.prompt(settings);
                });
            } else {
                $this.on('click', function (e) {
                    e.preventDefault();
                    eFlexis.dialog.prompt(settings);
                });
            }

        });

    };

})(jQuery, window);


$(function () {

    $('[data-toggle="alert"]').alert();
    $('[data-toggle="confirm"]').confirm();
    $('[data-toggle="prompt"]').prompt();

});
 
