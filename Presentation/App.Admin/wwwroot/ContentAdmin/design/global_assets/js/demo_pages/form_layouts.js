/* ------------------------------------------------------------------------------
 *
 *  # Form layouts
 *
 *  Demo JS code for form layouts pages
 *
 * ---------------------------------------------------------------------------- */


// Setup module
// ------------------------------

var FormLayouts = function () {


    //
    // Setup module components
    //

    // Select2
    var _componentSelect2 = function () {
        if (!$().select2) {
            console.warn('Warning - select2.min.js is not loaded.');
            return;
        };

        // Basic example
        $('.form-control-select2').select2();


        //
        // Select with icons
        //

        // Format icon
        function iconFormat(icon) {
            var originalOption = icon.element;
            if (!icon.id) { return icon.text; }
            var $icon = "<i class='icon-" + $(icon.element).data('icon') + "'></i>" + icon.text;

            return $icon;
        }

        // Initialize with options
        $('.form-control-select2-icons').select2({
            templateResult: iconFormat,
            minimumResultsForSearch: Infinity,
            templateSelection: iconFormat,
            closeOnSelect: false,
            escapeMarkup: function (m) { return m; }
        }).on('select2:selecting', function (e) {
            var cur = e.params.args.data.id;
            var old = (e.target.value == '') ? [cur] : $(e.target).val().concat([cur]);
            $(e.target).val(old).trigger('change');

            $(e.params.args.originalEvent.currentTarget).attr('aria-selected', 'true');
            return false;
        });
        $('.category').select2({
            templateResult: iconFormat,
            minimumResultsForSearch: Infinity,
            templateSelection: iconFormat,
            closeOnSelect: false,
            escapeMarkup: function (m) { return m; }
        }).on('select2:selecting', function (e) {
            var cur = e.params.args.data.id;
            var old = (e.target.value == '') ? [cur] : $(e.target).val().concat([cur]);
            $(e.target).val(old).trigger('change');

            $(e.params.args.originalEvent.currentTarget).attr('aria-selected', 'true');
            return false;
        });
        $('.CityAirport').select2({
            templateResult: iconFormat,
            minimumResultsForSearch: Infinity,
            templateSelection: iconFormat,
            closeOnSelect: false,
            escapeMarkup: function (m) { return m; }
        }).on('select2:selecting', function (e) {
            var cur = e.params.args.data.id;
            var old = (e.target.value == '') ? [cur] : $(e.target).val().concat([cur]);
            $(e.target).val(old).trigger('change');

            $(e.params.args.originalEvent.currentTarget).attr('aria-selected', 'true');
            return false;
        });
        $('.FamousLandmark').select2({
            templateResult: iconFormat,
            minimumResultsForSearch: Infinity,
            templateSelection: iconFormat,
            closeOnSelect: false,
            escapeMarkup: function (m) { return m; }
        }).on('select2:selecting', function (e) {
            var cur = e.params.args.data.id;
            var old = (e.target.value == '') ? [cur] : $(e.target).val().concat([cur]);
            $(e.target).val(old).trigger('change');

            $(e.params.args.originalEvent.currentTarget).attr('aria-selected', 'true');
            return false;
        });
        $('.ActivityList').select2({
            templateResult: iconFormat,
            minimumResultsForSearch: Infinity,
            templateSelection: iconFormat,
            closeOnSelect: false,
            escapeMarkup: function (m) { return m; }
        }).on('select2:selecting', function (e) {
            var cur = e.params.args.data.id;
            var old = (e.target.value == '') ? [cur] : $(e.target).val().concat([cur]);
            $(e.target).val(old).trigger('change');

            $(e.params.args.originalEvent.currentTarget).attr('aria-selected', 'true');
            return false;
        });
        $('.RestaurantList').select2({
            templateResult: iconFormat,
            minimumResultsForSearch: Infinity,
            templateSelection: iconFormat,
            closeOnSelect: false,
            escapeMarkup: function (m) { return m; }
        }).on('select2:selecting', function (e) {
            var cur = e.params.args.data.id;
            var old = (e.target.value == '') ? [cur] : $(e.target).val().concat([cur]);
            $(e.target).val(old).trigger('change');

            $(e.params.args.originalEvent.currentTarget).attr('aria-selected', 'true');
            return false;
        });
        $('.AvailableDays').select2({
            templateResult: iconFormat,
            minimumResultsForSearch: Infinity,
            templateSelection: iconFormat,
            closeOnSelect: false,
            escapeMarkup: function (m) { return m; }
        }).on('select2:selecting', function (e) {
            var cur = e.params.args.data.id;
            var old = (e.target.value == '') ? [cur] : $(e.target).val().concat([cur]);
            $(e.target).val(old).trigger('change');

            $(e.params.args.originalEvent.currentTarget).attr('aria-selected', 'true');
            return false;
        });
        $('.BudgetRange').select2({
            templateResult: iconFormat,
            closeOnSelect: false,
            minimumResultsForSearch: Infinity,
            templateSelection: iconFormat,
            escapeMarkup: function (m) { return m; }
        }).on('select2:selecting', function (e) {
            var cur = e.params.args.data.id;
            var old = (e.target.value == '') ? [cur] : $(e.target).val().concat([cur]);
            $(e.target).val(old).trigger('change');
            $(e.params.args.originalEvent.currentTarget).attr('aria-selected', 'true');
            UpdateBudgetPrice(cur, true);

            return false;
        }).on("select2:unselecting", function (e) {
            var cur = e.params.args.data.id;
            UpdateBudgetPrice(cur, false);
        });
        $('.BudgetRangeOnly').select2({
            templateResult: iconFormat,
            closeOnSelect: false,
            minimumResultsForSearch: Infinity,
            templateSelection: iconFormat,
            escapeMarkup: function (m) { return m; }
        }).on('select2:selecting', function (e) {
            var cur = e.params.args.data.id;
            var old = (e.target.value == '') ? [cur] : $(e.target).val().concat([cur]);
            $(e.target).val(old).trigger('change');
            $(e.params.args.originalEvent.currentTarget).attr('aria-selected', 'true');

            return false;
        }).on("select2:unselecting", function (e) {
            var cur = e.params.args.data.id;
        });
        $('.WabiGuideCategoryMultiList').select2({
            templateResult: iconFormat,
            closeOnSelect: false,
            minimumResultsForSearch: Infinity,
            templateSelection: iconFormat,
            escapeMarkup: function (m) { return m; }
        }).on('select2:selecting', function (e) {
            var cur = e.params.args.data.id;
            var old = (e.target.value == '') ? [cur] : $(e.target).val().concat([cur]);
            $(e.target).val(old).trigger('change');
            $(e.params.args.originalEvent.currentTarget).attr('aria-selected', 'true');
            UpdateCategory(cur, true);

            return false;
        }).on("select2:unselecting", function (e) {
            var cur = e.params.args.data.id;
            UpdateCategory(cur, false);
        });

        $('.WabiGuideAirlineMultiList').select2({
            templateResult: iconFormat,
            closeOnSelect: false,
            minimumResultsForSearch: Infinity,
            templateSelection: iconFormat,
            escapeMarkup: function (m) { return m; }
        }).on('select2:selecting', function (e) {
            var cur = e.params.args.data.id;
            var old = (e.target.value == '') ? [cur] : $(e.target).val().concat([cur]);
            $(e.target).val(old).trigger('change');
            $(e.params.args.originalEvent.currentTarget).attr('aria-selected', 'true');
            UpdateAirline(cur, true);

            return false;
        }).on("select2:unselecting", function (e) {
            var cur = e.params.args.data.id;
            UpdateAirline(cur, false);
        });
        $('.NearestCities').select2({
            templateResult: iconFormat,
            closeOnSelect: false,
            minimumResultsForSearch: Infinity,
            templateSelection: iconFormat,
            escapeMarkup: function (m) { return m; }
        }).on('select2:selecting', function (e) {
            var cur = e.params.args.data.id;
            var old = (e.target.value == '') ? [cur] : $(e.target).val().concat([cur]);
            $(e.target).val(old).trigger('change');
            $(e.params.args.originalEvent.currentTarget).attr('aria-selected', 'true');
            UpdateNearestCities(cur, true);

            return false;
        }).on("select2:unselecting", function (e) {
            var cur = e.params.args.data.id;
            UpdateNearestCities(cur, false);
        });
    };

    // Uniform
    var _componentUniform = function () {
        if (!$().uniform) {
            console.warn('Warning - uniform.min.js is not loaded.');
            return;
        }

        // Initialize
        $('.form-input-styled').uniform({
            fileButtonClass: 'action btn bg-pink-400'
        });
    };


    //
    // Return objects assigned to module
    //

    return {
        init: function () {
            _componentSelect2();
            _componentUniform();
        }
    }
}();


// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    FormLayouts.init();
});
